using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ClearAllInventoryFilters
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class ClearFiltersMod : BaseUnityPlugin
    {
        void Awake()
        {

            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            harmony.PatchAll(typeof(Patch));

        }

        public static void CreateUI()
        {
            // Grab instance of player inventory
            UIInventoryWindow inventory = UIRoot.instance.uiGame.inventoryWindow;
            GameObject inv = inventory.gameObject;
            RectTransform windowTrans = inv.gameObject.GetComponent<RectTransform>(); 

            UIDESwarmPanel swarmPanel = UIRoot.instance.uiGame.dysonEditor.controlPanel.hierarchy.swarmPanel;
            UIButton src = swarmPanel.orbitButtons[0];
            UIButton btn = GameObject.Instantiate<UIButton>(src);
            RectTransform btnRect = Util.NormalizeRectWithTopLeft(btn, 50f, 50f, windowTrans);

        }

        static class Patch
        {

        }
        
    }
}
