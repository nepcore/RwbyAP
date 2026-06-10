using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using UnityEngine;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RwbyAP;

public class APConnection
{
    private class DummyLocations : ILocationCheckHelper
    {
        public ReadOnlyCollection<long> AllLocations => new(new List<long>());
        public ReadOnlyCollection<long> AllLocationsChecked => new(new List<long>());
        public ReadOnlyCollection<long> AllMissingLocations => new(new List<long>());
        #pragma warning disable CS0067
        public event LocationCheckHelper.CheckedLocationsUpdatedHandler CheckedLocationsUpdated;
        #pragma warning restore CS0067
        public void CompleteLocationChecks(params long[] ids) {}
        public void CompleteLocationChecksAsync(Action<bool> f, params long[] ids) {
            return;
        }
        public void ScoutLocationsAsync(Action<Dictionary<long, ScoutedItemInfo>> f, HintCreationPolicy hcp, params long[] ids) {
            return;
        }
        public void ScoutLocationsAsync(Action<Dictionary<long, ScoutedItemInfo>> f, bool createHint, params long[] ids) {
            return;
        }
        public void ScoutLocationsAsync(Action<Dictionary<long, ScoutedItemInfo>> f, params long[] ids) {
            return;
        }
        public long GetLocationIdFromName(string game, string name) {
            return -1;
        }
        public string GetLocationNameFromId(long id, string game) {
            return null;
        }
    }

    private class DummyItems : IReceivedItemsHelper
    {
        public int Index => 0;
        public ReadOnlyCollection<ItemInfo> AllItemsReceived => new(new List<ItemInfo>());
        #pragma warning disable CS0067
        public event ReceivedItemsHelper.ItemReceivedHandler ItemReceived;
        #pragma warning restore CS0067
        public string GetItemName(long id, string game) {
            return null;
        }
        public bool Any()
        {
            return false;
        }
        public ItemInfo PeekItem()
        {
            return null;
        }
        public ItemInfo DequeueItem()
        {
            return null;
        }
    }

    private class LifecycleHook : MonoBehaviour
    {
        private APConnection self;
        private Coroutine reconnectTask;
        private Coroutine connStableCheck;
        private int backoff = 5;

        public void SetSelf(APConnection self)
        {
            this.self = self;
        }

        public void CancelReconnectTask()
        {
            if (reconnectTask != null) StopCoroutine(reconnectTask);
            reconnectTask = null;
            if (connStableCheck != null) StopCoroutine(connStableCheck);
            connStableCheck = null;
        }

        public void Reconnect()
        {
            if (self.shouldDisconnect) return;
            RWBYAP.Logger.LogInfo("Connection lost");
            CancelReconnectTask();
            var msg = $"Connection to Archipelago server failed, reconnecting in {backoff} seconds...";
            RWBYAP.SendChat($"<color=\"orange\">{msg}</color>");
            reconnectTask = StartCoroutine(WaitAndReconnect(backoff));
        }

        private IEnumerator<WaitForSecondsRealtime> WaitAndReconnect(int delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            backoff = Math.Min(backoff + 5, 60);
            self.Connect(() => {
                connStableCheck = StartCoroutine(TestConnection());
            }, true);
        }

        private IEnumerator<WaitForSecondsRealtime> TestConnection()
        {
            yield return new WaitForSecondsRealtime(60);
            backoff = 5;
            reconnectTask = null;
            connStableCheck = null;
        }

        void OnApplicationQuit()
        {
            RWBYAP.Logger.LogInfo("Application quitting");
            self.Disconnect();
        }
    }

    private string host;
    private string port;
    private string slot;
    private string pass;

    private ArchipelagoSession session;
    private GameObject lifecycle;
    private bool shouldDisconnect = false;

    public bool Connected => session != null && session.Socket != null && session.Socket.Connected;
    public string Seed => session?.RoomState?.Seed;

    public ILocationCheckHelper Locations => Connected ? session.Locations : new DummyLocations();
    public IReceivedItemsHelper Items => Connected ? session.Items : new DummyItems();

    public APConnection(string host, string port, string slot, string pass)
    {
        this.host = host;
        this.port = port;
        this.slot = slot;
        this.pass = pass;
        lifecycle = new GameObject("APConnection Lifecycle Hook");
        lifecycle.AddComponent<LifecycleHook>();
    }

    public void Connect(Action onConnected)
    {
        new System.Threading.Thread(() => {
            Connect(onConnected, false);
        }).Start();
    }

    private void Connect(Action onConnected, bool isReconnect)
    {
        RWBYAP.Logger.LogInfo("Creating session");
        session = ArchipelagoSessionFactory.CreateSession(host + ":" + port);
        session.MessageLog.OnMessageReceived += RWBYAP.AddAPMessage;
        try
        {
            RWBYAP.Logger.LogInfo("Initiating connection and logging in");
            var loginResult = session.TryConnectAndLogin(
                RWBYAP.GAME,
                slot,
                ItemsHandlingFlags.AllItems,
                password: pass,
                requestSlotData: true
            );

            switch (loginResult)
            {
                case LoginFailure fail:
                    RWBYAP.Logger.LogError(fail.Errors.Join(delimiter: "\n"));
                    CreateErrorWidget("LOGIN FAILED", fail.Errors.Join(delimiter: "\n"), "CLOSE");
                    UnityEngine.Object.Destroy(lifecycle);
                    return;
                case LoginSuccessful login:
                    RWBYAP.Logger.LogInfo("Setting up handlers");
                    session.Socket.ErrorReceived += delegate {
                        lifecycle.GetComponent<LifecycleHook>().Reconnect();
                    };
                    //InjectPatches();
                    RWBYAP.Logger.LogInfo("Connection successful");
                    onConnected();
                    return;
            }
        }
        catch (System.Exception ex)
        {
            RWBYAP.Logger.LogError($"Failed connecting to archipelago: {ex}");
            if (isReconnect)
            {
                lifecycle.GetComponent<LifecycleHook>().Reconnect();
            }
            else
            {
                CreateErrorWidget("ERROR", "Connection failed. Are host and port correct?", "CLOSE");
                UnityEngine.Object.Destroy(lifecycle);
            }
        }
    }

    private void CreateErrorWidget(string title, string description, string button)
    {
        YesNoPrompt prompt = Roost.Singleton_MonoBehaviour<UIManager>.Instance.CreateWidget(Roost.Singleton_MonoBehaviour<UIManager>.Instance.PanelPrefabs.YesNoPrompt, Roost.Singleton_MonoBehaviour<UIManager>.Instance.ModalOverlayTransform);
        prompt.Title.text = title;
        prompt.Description.text = description;
        prompt.NoButtonText.text= button;
        prompt.NoButton.onClick.AddListener(() => {
            Roost.Util.DestroyGameObject(prompt.gameObject);
        });
        prompt.NoButton.Select();
        prompt.Spacer.SetActive(value: true);
        prompt.YesButton.gameObject.SetActive(value: false);
    }

    public void SetGoalAchieved()
    {
        session.SetGoalAchieved();
    }

    public void Disconnect()
    {
        shouldDisconnect = true;
        lifecycle?.GetComponent<LifecycleHook>()?.CancelReconnectTask();
        if (session == null || session.Socket == null || !session.Socket.Connected) return;
        session.Socket.Disconnect();
    }
}
