using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ClearAllInventoryFilters
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class ClearFiltersMod : BaseUnityPlugin
    {
        new internal static ManualLogSource Logger;
        void Awake()
        {

            Logger = base.Logger;

            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            harmony.PatchAll(typeof(Patch));
            

        }


        public static void CreateUI()
        {
            Logger.LogInfo("CreateUI called");

            // Grab instance of player inventory
            UIInventoryWindow invInstance = UIRoot.instance.uiGame.inventoryWindow;
            GameObject inv = invInstance.gameObject;
            GameObject bgPanel_child = inv.transform.GetChild(1).gameObject; 
            RectTransform windowTrans = bgPanel_child.gameObject.GetComponent<RectTransform>();


            UIDESwarmPanel swarmPanel = UIRoot.instance.uiGame.dysonEditor.controlPanel.hierarchy.swarmPanel;
            UIButton src = swarmPanel.orbitButtons[0];
            UIButton btn = GameObject.Instantiate<UIButton>(src);
            RectTransform btnRect = Util.NormalizeRectWithTopLeft(btn, 125f, 27f, windowTrans);
            btnRect.sizeDelta = new Vector2(75f, 19f);
            btn.transform.Find("frame").gameObject.SetActive(false);
            Text btnText = btn.transform.Find("Text").GetComponent<Text>();
            btnText.text = "Clear filters";
            btnText.fontSize = 14;


            Transform invParentTransform = bgPanel_child.transform;
            btn.transform.SetParent(invParentTransform);

            // onclick
            btn.button.onClick.AddListener(OnClearFiltersButtonClick);
        }

        public static void OnClearFiltersButtonClick()
        {
            Logger.LogInfo("OnClearFiltersButtonClick called!");
            UIInventoryWindow instance = UIRoot.instance.uiGame.inventoryWindow;
            
            int size = instance.inventory.storage.size;
            for (int i = 0; i < size; i++)
            {
                instance.inventory.storage.SetFilter(i, 0);
            }
            instance.inventory.OnStorageContentChanged();

        }

        static class Patch
        {

            [HarmonyPrefix, HarmonyPatch(typeof(GameMain), "Begin")]
            public static void GameMain_Begin_Prefix()
            {
                CreateUI();
            }
            
        }
        
    }
}
