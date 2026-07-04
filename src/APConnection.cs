using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using UnityEngine;
using HarmonyLib;
using RwbyAP.Models;
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

    private class DummyDataStorage : IDataStorageHelper
    {
        private DataStorageElement dummy;
        public DataStorageElement this[Scope s, string key]
        {
            get => null;
            set => dummy = value;
        }
        public DataStorageElement this[string key]
        {
            get => null;
            set => dummy = value;
        }
        public Hint[] GetHints(int? i1, int? i2)
        {
            return [];
        }
        public void GetHintsAsync(Action<Hint[]> f, int? i1, int? i2) {}
        public void TrackHints(Action<Hint[]> f, bool b, int? i1, int? i2) {}
        public Dictionary<string, object> GetSlotData(int? i1)
        {
            return new();
        }
        public void GetSlotDataAsync(Action<Dictionary<string, object>> f, int? i1) {}
        public T GetSlotData<T>(int? i1) where T : class
        {
            return null;
        }
        public void GetSlotDataAsync<T>(Action<T> f, int? i1) where T : class {}
        public Dictionary<string, string[]> GetItemNameGroups(string s)
        {
            return new();
        }
        public void GetItemNameGroupsAsync(Action<Dictionary<string, string[]>> f, string s) {}
        public Dictionary<string, string[]> GetLocationNameGroups(string s)
        {
            return new();
        }
        public void GetLocationNameGroupsAsync(Action<Dictionary<string, string[]>> f, string s) {}
        public ArchipelagoClientState GetClientStatus(int? i1, int? i2)
        {
            return new();
        }
        public void GetClientStatusAsync(Action<ArchipelagoClientState> f, int? i1, int? i2) {}
        public void TrackClientStatus(Action<ArchipelagoClientState> f, bool b, int? i1, int? i2) {}
        public bool GetRaceMode()
        {
            return true;
        }
        public void GetRaceModeAsync(Action<bool> f) {}
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

    public string Host => host;
    public string Port => port;
    public string SlotName => slot;

    private ArchipelagoSession session;
    private GameObject lifecycle;
    private DeathLinkService deathlink;
    private long lastDeathLink;
    private bool shouldDisconnect = false;

    public bool Connected => session != null && session.Socket != null && session.Socket.Connected;
    public string Seed => session?.RoomState?.Seed;
    public int? Slot => session?.ConnectionInfo?.Slot;
    public int? Team => session?.ConnectionInfo?.Team;
    public DeathLink DeathLinkWaitingToProcess;
    public DeathLinkMode DeathLinkReceiveMode {
        get;
        private set;
    }
    public DeathLinkMode DeathLinkSendMode {
        get;
        private set;
    }

    public ILocationCheckHelper Locations => Connected ? session.Locations : new DummyLocations();
    public IReceivedItemsHelper Items => Connected ? session.Items : new DummyItems();
    public IDataStorageHelper DataStorage => Connected ? session.DataStorage : new DummyDataStorage();

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
                    var slotDataError = false;
                    var artifactsInPool = 50L;
                    var artifactsRequiredPercentage = 80L;

                    switch (login.SlotData.GetValueSafe("artifacts_in_pool"))
                    {
                        case long inPool:
                            artifactsInPool = inPool;
                            break;
                        default:
                            slotDataError = true;
                            break;
                    }

                    switch (login.SlotData.GetValueSafe("artifacts_required_percentage"))
                    {
                        case long required:
                            artifactsRequiredPercentage = required;
                            break;
                        default:
                            slotDataError = true;
                            break;
                    }

                    if (slotDataError)
                    {
                        CreateErrorWidget("SLOT DATA ERROR", "Couldn't fetch required artifact count from slot data, assuming default (40)", "CLOSE");
                        RWBYAP.ArtifactsRequired = 40;
                    }
                    else
                    {
                        RWBYAP.ArtifactsRequired = (long) Math.Floor(artifactsInPool * (artifactsRequiredPercentage / 100.0));
                    }

                    RWBYAP.Logger.LogInfo("Preparing deathlink handler");
                    deathlink = DeathLinkProvider.CreateDeathLinkService(session);

                    Action<DeathLink> deathLinkReceived = dl => {};
                    switch (login.SlotData.GetValueSafe("death_link"))
                    {
                        case long deathlinkState:
                            if (deathlinkState == 1) deathlink.EnableDeathLink();
                            break;
                    }

                    switch (login.SlotData.GetValueSafe("death_link_receive_mode"))
                    {
                        case long mode:
                            if (mode == 2) DeathLinkReceiveMode = DeathLinkMode.All;
                            else DeathLinkReceiveMode = DeathLinkMode.Single;
                            break;
                        default:
                            DeathLinkReceiveMode = DeathLinkMode.Single;
                            break;
                    }

                    switch (login.SlotData.GetValueSafe("death_link_send_mode"))
                    {
                        case long mode:
                            if (mode == 1) DeathLinkSendMode = DeathLinkMode.Single;
                            else DeathLinkSendMode = DeathLinkMode.All;
                            break;
                        default:
                            DeathLinkSendMode = DeathLinkMode.All;
                            break;
                    }

                    lastDeathLink = 0;
                    deathlink.OnDeathLinkReceived += dl => {
                        var now = (long) System.DateTimeOffset.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
                        if (lastDeathLink < now - 2) {
                            lastDeathLink = now;
                            DeathLinkWaitingToProcess = dl;
                        }
                    };

                    RWBYAP.Logger.LogInfo("Setting up handlers");
                    session.Socket.ErrorReceived += delegate {
                        lifecycle.GetComponent<LifecycleHook>().Reconnect();
                    };
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

    public void CompleteLocationChecks(params long[] ids)
    {
        new System.Threading.Thread(() => {
            Locations.CompleteLocationChecks(ids);
        }).Start();
    }

    public void SetGoalAchieved()
    {
        session.SetGoalAchieved();
    }

    public void SendDeathLink() {
        if (!IsDeathLinkOn()) return;
        new System.Threading.Thread(() => {
            var now = (long) System.DateTimeOffset.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
            if (lastDeathLink < now - 2) {
                lastDeathLink = now;
                deathlink.SendDeathLink(new DeathLink(SlotName));
            }
        }).Start();
    }

    public void ToggleDeathLink()
    {
        new System.Threading.Thread(() => {
            if (!IsDeathLinkOn()) deathlink.EnableDeathLink();
            else deathlink.DisableDeathLink();
        }).Start();
    }

    public bool IsDeathLinkOn()
    {
        return Array.IndexOf(session.ConnectionInfo.Tags, "DeathLink") != -1;
    }

    public void Disconnect()
    {
        shouldDisconnect = true;
        try {
            lifecycle?.GetComponent<LifecycleHook>()?.CancelReconnectTask();
        } catch {} // ignore exception
        if (session == null || session.Socket == null || !session.Socket.Connected) return;
        session.Socket.Disconnect();
    }
}
