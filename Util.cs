using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.Experimental.UIElements;

/*
 * 
 *  Based on Hetima's DSP_PlanetFinder mod. Special thanks! 
 * 
 */


namespace ClearAllInventoryFilters
{
    public class Util
    {

        public static T CreateGameObject<T>(string name, float width = 0f, float height = 0f) where T : Component
        {
            GameObject go = new GameObject(name);
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            if (width > 0f || height > 0)
            {
                rect.sizeDelta = new Vector2(width, height);
            }
            T item = go.AddComponent<T>();

            return item;
        }

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

        public static RectTransform NormalizeRectWithBottomLeft(Component cmp, float left, float bottom, Transform parent = null)
        {
            RectTransform rect = cmp.transform as RectTransform;
            if (parent != null)
            {
                rect.SetParent(parent, false);
            }
            rect.anchorMax = new Vector2(0f, 0f);
            rect.anchorMin = new Vector2(0f, 0f);
            rect.pivot = new Vector2(0f, 0f);
            rect.anchoredPosition3D = new Vector3(left, bottom, 0f);
            return rect;
        }

        public static RectTransform NormalizeRectWithMargin(Component cmp, float top, float left, float bottom, float right, Transform parent = null)
        {
            RectTransform rect = cmp.transform as RectTransform;
            if (parent != null)
            {
                rect.SetParent(parent, false);
            }
            rect.anchoredPosition3D = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.anchorMax = Vector2.one;
            rect.anchorMin = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMax = new Vector2(-right, -top);
            rect.offsetMin = new Vector2(left, bottom);
            return rect;
        }

        public static RectTransform NormalizeRect(GameObject go, float width = 0, float height = 0)
        {
            RectTransform rect = (RectTransform)go.transform;
            rect.anchorMax = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            rect.pivot = Vector2.zero;
            if (width > 0 && height > 0)
            {
                rect.sizeDelta = new Vector2(width, height);
            }
            return rect;
        }
        /*
        public static Text CreateText(string label, int fontSize = 14, string objectName = "text")
        {
            Text txt_;
            Text stateText = UIRoot.instance.uiGame.assemblerWindow.stateText;
            txt_ = GameObject.Instantiate<Text>(stateText);
            txt_.gameObject.name = objectName;
            txt_.text = label;
            txt_.color = new Color(1f, 1f, 1f, 0.4f);
            txt_.alignment = TextAnchor.MiddleLeft;
            //txt_.supportRichText = false;
            txt_.fontSize = fontSize;
            return txt_;
        }

        public static UIListView CreateListView(Action<UIData> dataFunc, string goName = "", Transform parent = null, float vsWidth = 10f)
        {
            UIListView result;
            UIListView src = UIRoot.instance.uiGame.tutorialWindow.entryList;
            GameObject go = GameObject.Instantiate(src.gameObject);
            if (parent != null)
            {
                go.transform.SetParent(parent, false);
            }
            if (!string.IsNullOrEmpty(goName))
            {
                go.name = goName;
            }

            result = go.GetComponent<UIListView>();
            if (dataFunc != null)
            {
                RectTransform itemResTrans = result.m_ItemRes.gameObject.transform as RectTransform;

                //transform.anchorMin = new Vector2(0f, 1f);
                //transform.anchorMax = new Vector2(0f, 1f);
                //transform.localScale = new Vector2(0f, 1f);
                //itemResTrans.sizeDelta = transform.sizeDelta;

                //result.m_ItemRes.transform.DetachChildren
                for (int i = itemResTrans.childCount - 1; i >= 0; i--)
                {
                    GameObject.Destroy(itemResTrans.GetChild(i).gameObject);
                }
                itemResTrans.DetachChildren();
                GameObject.Destroy(itemResTrans.GetComponent<UITutorialListEntry>());
                //GameObject.Destroy(itemResTrans.GetComponent<Image>());//
                //GameObject.Destroy(itemResTrans.GetComponent<Button>());//

                dataFunc(result.m_ItemRes);

                result.RowHeight = (int)itemResTrans.sizeDelta.y;
                result.m_ItemRes.sel_highlight = null;

                result.m_ContentPanel.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                result.m_ContentPanel.constraintCount = 1;
                result.ColumnWidth = (int)itemResTrans.sizeDelta.x;
                result.RowHeight = (int)itemResTrans.sizeDelta.y;
            }
            result.HorzScroll = false;
            result.VertScroll = true;
            result.CullOutsideItems = false;
            result.ColumnSpacing = 0;
            result.RowSpacing = 4;

            Image barBg = result.m_ScrollRect.verticalScrollbar.GetComponent<Image>();
            if (barBg != null)
            {
                barBg.color = new Color(0f, 0f, 0f, 0.62f);
            }

            //あんまり上手くはないけどとりあえず変わる
            RectTransform vsRect = result.m_ScrollRect.verticalScrollbar.transform as RectTransform;
            vsRect.sizeDelta = new Vector2(vsWidth, vsRect.sizeDelta.y);

            return result;
        }

        public static UIButton MakeIconButtonB(Sprite sprite, float size = 60)
        {
            GameObject go = GameObject.Instantiate(UIRoot.instance.uiGame.researchQueue.pauseButton.gameObject);
            UIButton btn = go.GetComponent<UIButton>();
            RectTransform rect = (RectTransform)go.transform;
            //rect.sizeDelta = new Vector2(size, size);
            float scale = size / 60;
            rect.localScale = new Vector3(scale, scale, scale);
            Image img = go.transform.Find("icon")?.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = sprite;
            }
            btn.tips.tipText = "";
            btn.tips.tipTitle = "";
            btn.tips.delay = 0.7f;
            return btn;
        }

        public static UIButton MakeIconButtonC(Sprite sprite, float size = 30)
        {
            GameObject src = UIRoot.instance.uiGame.starmap.northButton?.transform.parent.Find("tip")?.gameObject;
            GameObject go = GameObject.Instantiate(src);

            RemovePersistentCalls(go);
            UIButton btn = go.GetComponent<UIButton>();
            RectTransform rect = (RectTransform)go.transform;
            for (int i = rect.childCount - 1; i >= 0; --i)
            {
                GameObject.Destroy(rect.GetChild(i).gameObject);
            }
            rect.DetachChildren();

            if (size > 0)
            {
                rect.sizeDelta = new Vector2(size, size);
                //float scale = size / rect.sizeDelta.y; //y=30
                //rect.localScale = new Vector3(scale, scale, scale);
            }

            Image img = go.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = sprite;
            }
            btn.tips.tipText = "";
            btn.tips.tipTitle = "";
            btn.tips.delay = 0.6f;

            if (btn.transitions != null && btn.transitions.Length > 0)
            {
                btn.transitions = new UIButton.Transition[] { btn.transitions[0] };
            }
            return btn;
        }

        public static UIButton MakeSmallTextButton(string label = "", float width = 0, float height = 0)
        {
            UIAssemblerWindow assemblerWindow = UIRoot.instance.uiGame.assemblerWindow;
            GameObject go = GameObject.Instantiate(assemblerWindow.copyButton.gameObject);
            UIButton btn = go.GetComponent<UIButton>();
            Transform child = go.transform.Find("Text");
            GameObject.DestroyImmediate(child.GetComponent<Localizer>());
            Text txt = child.GetComponent<Text>();
            txt.text = label;
            btn.tips.tipText = "";
            btn.tips.tipTitle = "";

            if (width > 0 || height > 0)
            {
                RectTransform rect = (RectTransform)go.transform;
                if (width == 0)
                {
                    width = rect.sizeDelta.x;
                }
                if (height == 0)
                {
                    height = rect.sizeDelta.y;
                }
                rect.sizeDelta = new Vector2(width, height);
            }

            go.transform.localScale = Vector3.one;

            return btn;
        }

        public static UIButton MakeHiliteTextButton(string label, float width = 0f, float height = 0f)
        {
            UIDESwarmPanel swarmPanel = UIRoot.instance.uiGame.dysonEditor.controlPanel.hierarchy.swarmPanel;
            UIButton src = swarmPanel.orbitButtons[0];
            UIButton btn = GameObject.Instantiate<UIButton>(src);
            // btn.transitions[0] btn btn.transitions[1]==text btn.transitions[2]==frame
            if (btn.transitions.Length >= 2)
            {
                btn.transitions[0].normalColor = new Color(0.1f, 0.1f, 0.1f, 0.68f);
                btn.transitions[0].highlightColorOverride = new Color(0.9906f, 0.5897f, 0.3691f, 0.4f);
                btn.transitions[1].normalColor = new Color(1f, 1f, 1f, 0.6f);
                btn.transitions[1].highlightColorOverride = new Color(0.2f, 0.1f, 0.1f, 0.9f);
            }
            Text btnText = btn.transform.Find("Text").GetComponent<Text>();
            btnText.text = label;
            if (btnText.font.name == "MPMK85") //JP MOD
            {
                btnText.fontSize = 16;
            }
            else // "DIN"
            {
                btnText.fontSize = 14;
            }
            btn.transform.Find("frame")?.gameObject.SetActive(false);
            RectTransform btnRect = btn.transform as RectTransform;
            if (width == 0f || height == 0f)
            {
                btnRect.sizeDelta = new Vector2(btnText.preferredWidth + 14f, 22f);
            }
            else
            {
                btnRect.sizeDelta = new Vector2(width, height);
            }
            //(btn.transform.Find("frame").transform as RectTransform).sizeDelta = btnRect.sizeDelta;

            return btn;
        }
        */
    }
}
