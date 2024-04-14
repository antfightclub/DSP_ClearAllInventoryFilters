using BepInEx;
//using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ClearAllInventoryFilters
{
    [BepInPlugin(__GUID__, __NAME__, "1.0.2")]
    public class ClearFiltersMod : BaseUnityPlugin
    {
        public const string __NAME__ = "ClearInventoryFilters";
        public const string __GUID__ = "com.antfightclub.dsp." + __NAME__;

        //new internal static ManualLogSource Logger;
        void Awake()
        {

            //Logger = base.Logger;

            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            harmony.PatchAll(typeof(Patch));
            

        }


        public static void CreateUI()
        {
            //Logger.LogInfo("CreateUI called");

            // Grab instance of player inventory
            UIInventoryWindow invInstance = UIRoot.instance.uiGame.inventoryWindow;
            GameObject inv = invInstance.gameObject;
            GameObject bgPanel_child = inv.transform.GetChild(1).gameObject; 
            RectTransform windowTrans = bgPanel_child.gameObject.GetComponent<RectTransform>();


            UIDESwarmPanel swarmPanel = UIRoot.instance.uiGame.dysonEditor.controlPanel.hierarchy.swarmPanel;
            UIButton src = swarmPanel.orbitButtons[0];
            UIButton btn = GameObject.Instantiate<UIButton>(src);
            RectTransform btnRect = NormalizeRectWithTopLeft(btn, 115f, 30f, windowTrans);
            btnRect.sizeDelta = new Vector2(68f, 16f);
            btn.transform.Find("frame").gameObject.SetActive(false);
            Text btnText = btn.transform.Find("Text").GetComponent<Text>();
            btnText.text = "Clear filters";
            btnText.fontSize = 12;


            Transform invParentTransform = bgPanel_child.transform;
            btn.transform.SetParent(invParentTransform);

            // onclick
            btn.button.onClick.AddListener(OnClearFiltersButtonClick);
        }

        public static void OnClearFiltersButtonClick()
        {
            //Logger.LogInfo("OnClearFiltersButtonClick called!");
            UIInventoryWindow instance = UIRoot.instance.uiGame.inventoryWindow;
            
            int size = instance.inventory.storage.size;
            for (int i = 0; i < size; i++)
            {
                instance.inventory.storage.SetFilter(i, 0);
            }
            instance.inventory.OnStorageContentChanged();

        }

        // This method is based on Hetima's DSP_PlanetFinder mod
        // parent upper left origin, cmp upper left reference Y axis is also passed as a positive number
        public static RectTransform NormalizeRectWithTopLeft(Component cmp, float left, float top, Transform parent = null)
        {
            RectTransform rect = cmp.transform as RectTransform;
            if (parent != null)
            {
                rect.SetParent(parent, false);
            }
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition3D = new Vector3(left, -top, 0f);
            return rect;
        }



        static class Patch
        {

            internal static bool _initialized = false;

            [HarmonyPrefix, HarmonyPatch(typeof(GameMain), "Begin")]
            public static void GameMain_Begin_Prefix()
            {
                if (!_initialized)
                {
                    _initialized = true;
                    CreateUI();
                }
            }
            
        }
        
    }
}
