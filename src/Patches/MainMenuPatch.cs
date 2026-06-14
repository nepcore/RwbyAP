using HarmonyLib;
using Roost;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RwbyAP.Patches;

[HarmonyPatch(typeof(MainMenuController), "Initialize")]
public class MainMenuPatch : IRwbyEssentialPatch
{
    public static bool Prefix() {
        // clean up patches from previous sessions so they don't leak into non-ap sessions
        RWBYAP.Harmony.UnpatchSelf();
        // reload profile after unpatching to go back to the players normal save data
        Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.Reload();
        return true;
    }

    public static void Postfix(MainMenuPanel ___m_mainMenuPanel)
    {
        InjectTranslations();
        // foreach (var upgrade in Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.CharacterUpgradeDatabase.GetData())
        // {
        //     RWBYAP.Logger.LogInfo($"{upgrade.ID} / {upgrade.Name} / {upgrade.Description} / {upgrade.name}");
        // }
        // foreach (var pc in Singleton_MonoBehaviour<ApplicationManager>.Instance.GameplayDatabase.PlayableCharacters.GetData())
        // {
        //     RWBYAP.Logger.LogInfo($"{pc.ID} / {pc.Name}");
        // }

        var label = GameObject.Find("/Global - GUI(Clone)/PanelRoot/MainMenuPanel(Clone)/ButtonPanel (1)/Profile/Text");
        var text = label.GetComponent<Text>();
        text.text = "ARCHIPELAGO";
        ___m_mainMenuPanel.ProfileButton.gameObject.SetActive(true);

        // create ap connection dialog
        Transform host = null;
        Transform port = null;
        Transform slot = null;
        Transform pass = null;
        var btn = ___m_mainMenuPanel.ProfileButton.gameObject.GetComponent<Button>();
        btn.onClick = new();
        var data = new YesNoWidgetController.Data();
        data.Mode = YesNoWidgetController.Data.WidgetMode.Binary;
        data.TitleTerm = "AP_ConnectTitle";
        data.DescriptionTerm = "AP_Nothing";
        data.PrimaryChoiceTerm = "AP_ConnectConfirm";
        data.SecondaryChoiceTerm = "General_Cancel";
        data.OnPrimaryChoice = () => {
            OnArchipelago(
                host.Find("Input Field").GetComponent<InputField>().text,
                          port.Find("Input Field").GetComponent<InputField>().text,
                          slot.Find("Input Field").GetComponent<InputField>().text,
                          pass.Find("Input Field").GetComponent<InputField>().text
            );
        };

        var dialog = new YesNoWidgetController(btn, ___m_mainMenuPanel, data);

        btn.onClick.AddListener(() => {
            var container = GameObject.Find("/Global - GUI(Clone)/Modal Overlay Transform/YesNoPrompt(Clone)/Window/ButtonPanel/Description");
            for (var i = 0; i < container.transform.childCount; i++)
            {
                container.transform.GetChild(i).gameObject.SetActive(false);
            }

            host = Singleton_MonoBehaviour<UIManager>.Instance.CreateWidget(Singleton_MonoBehaviour<UIManager>.Instance.PanelPrefabs.Chat.transform.Find("Chat/Input Box"), container.transform);
            host.transform.Find("Label").GetComponent<Text>().text = "Host:";
            host.transform.Find("Input Field").GetComponent<InputField>().text = "archipelago.gg";
            host.localPosition = new(360, 65, 0);

            port = Singleton_MonoBehaviour<UIManager>.Instance.CreateWidget(Singleton_MonoBehaviour<UIManager>.Instance.PanelPrefabs.Chat.transform.Find("Chat/Input Box"), container.transform);
            port.transform.Find("Label").GetComponent<Text>().text = "Port:";
            port.transform.Find("Input Field").GetComponent<InputField>().text = "38281";
            port.localPosition = new(360, 30, 0);

            slot = Singleton_MonoBehaviour<UIManager>.Instance.CreateWidget(Singleton_MonoBehaviour<UIManager>.Instance.PanelPrefabs.Chat.transform.Find("Chat/Input Box"), container.transform);
            slot.transform.Find("Label").GetComponent<Text>().text = "Slot:";
            slot.localPosition = new(360, -5, 0);

            pass = Singleton_MonoBehaviour<UIManager>.Instance.CreateWidget(Singleton_MonoBehaviour<UIManager>.Instance.PanelPrefabs.Chat.transform.Find("Chat/Input Box"), container.transform);
            pass.transform.Find("Label").GetComponent<Text>().text = "Password:";
            pass.localPosition = new(360, -40, 0);
        });

        // move menu up a little bit to not lose half the exit button
        var img = GameObject.Find("/Global - GUI(Clone)/PanelRoot/MainMenuPanel(Clone)/RWBY: GE");
        var btns = GameObject.Find("/Global - GUI(Clone)/PanelRoot/MainMenuPanel(Clone)/ButtonPanel (1)");
        var imgpos = img.transform.position;
        imgpos.y = imgpos.y + 50;
        img.transform.position = imgpos;
        var btnspos = btns.transform.position;
        btnspos.y = btnspos.y + 50;
        btns.transform.position = btnspos;
    }

    public static void InjectTranslations()
    {
        var terms = new System.Collections.Generic.Dictionary<string, string>();
        terms.Add("AP_ConnectTitle", "CONNECT TO ARCHIPELAGO");
        terms.Add("AP_Nothing", " ");
        terms.Add("AP_ConnectConfirm", "CONNECT");

        var source = I2.Loc.LocalizationManager.Sources[0];

        foreach (var term in terms)
        {
            var termData = source.AddTerm(term.Key);
            for (var i = 0; i < termData.Languages.Length; i++)
            {
                termData.Languages[i] = term.Value;
                termData.Languages_Touch[i] = term.Value;
            }
        }
    }

    public static void Inject()
    {
        RWBYAP.Harmony.UnpatchSelf();
        RWBYAP.Harmony.PatchByInterface(typeof(IRwbyGameplayPatch));
    }

    public static void OnArchipelago(string host, string port, string slot, string pass)
    {
        RWBYAP.Logger.LogInfo($"Connect: {slot}:{pass}@{host}:{port}");
        if (RWBYAP.Connection != null) RWBYAP.Connection.Disconnect();
        RWBYAP.ProfileLoaded = false;
        RWBYAP.ItemsProcessed = 0;
        RWBYAP.Connection = new(host, port, slot, pass);
        RWBYAP.Connection.Connect(() => {
            Inject();
            // reload profile after patching, reload is patched to set up an AP specific profile state
            Singleton_MonoBehaviour<ApplicationManager>.Instance.Profile.Reload();
            typeof(ApplicationManager).GetMethod("PlayFriends_CreateLobby", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Singleton_MonoBehaviour<ApplicationManager>.Instance, []);
        });
    }
}
