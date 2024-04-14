using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
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
            UIInventoryWindow invInstance = UIRoot.instance.uiGame.inventoryWindow;
            GameObject inv = invInstance.gameObject;
            RectTransform windowTrans = inv.gameObject.GetComponent<RectTransform>(); 

            UIDESwarmPanel swarmPanel = UIRoot.instance.uiGame.dysonEditor.controlPanel.hierarchy.swarmPanel;
            UIButton src = swarmPanel.orbitButtons[0];
            UIButton btn = GameObject.Instantiate<UIButton>(src);
            RectTransform btnRect = Util.NormalizeRectWithTopLeft(btn, 50f, 50f, windowTrans);
            btnRect.sizeDelta = new Vector2(100f, 24f);
            btn.transform.Find("frame").gameObject.SetActive(false);
            Text btnText = btn.transform.Find("Text").GetComponent<Text>();
            btnText.text = "Clear filters";
            btnText.fontSize = 16;

            Transform invParentTransform = inv.transform;

            btn.transform.SetParent(invParentTransform);
        }


       /*
       public void BeginGame()
        {
            if (DSPGame.IsMenuDemo)
            {
                return;
            }
        }*/

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
