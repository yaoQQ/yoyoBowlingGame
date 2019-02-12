﻿/// Credit drobina, w34edrtfg, playemgames 
/// Sourced from - http://forum.unity3d.com/threads/sprite-icons-with-text-e-g-emoticons.265927/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
    // Image according to the label inside the name attribute to load, read from the resources directory. The size of the image is controlled by the size property.
    // Use: <quad name=NAME size=25 width=1 />
    [AddComponentMenu("UI/Extensions/TextPic")]

    [ExecuteInEditMode] // Needed for culling images that are not used //
    public class TextPic : Text, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, ISelectHandler
    {
        /// <summary>
        /// Image Pool
        /// </summary>
        private readonly List<Image> m_ImagesPool = new List<Image>();
        private readonly List<GameObject> culled_ImagesPool = new List<GameObject>();
        private bool clearImages = false;

        /// <summary>
        /// Vertex Index
        /// </summary>
        private readonly List<int> m_ImagesVertexIndex = new List<int>();

        /// <summary>
        /// Regular expression to replace 
        /// </summary>
        private static readonly Regex s_Regex =
            new Regex(@"<quad name=(.+?) size=(\d*\.?\d+%?) width=(\d*\.?\d+%?) />", RegexOptions.Singleline);

        private string fixedString;



        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();
            UpdateQuadImage();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateQuadImage();
        }
#endif

        /// <summary>
        /// After parsing the final text
        /// </summary>
        private string m_OutputText;

        [System.Serializable]
        public struct IconName
        {
            public string name;
            public Sprite sprite;
        }
        public IconName[] inspectorIconList;

        private Dictionary<string, Sprite> iconList = new Dictionary<string, Sprite>();

        public float ImageScalingFactor = 1;


        public float NormalLineSpacing = 1f;

        public float PicLineSpacing = 1.5f;


        // Write the name or hex value of the hyperlink color
        public string hyperlinkColor = "blue";

        // Offset image by x, y
        public Vector2 imageOffset = Vector2.zero;

        private Button button;

        //Commented out as private and not used.. Yet?
        //private bool selected = false;

        private List<Vector2> positions = new List<Vector2>();

        /**
        * Unity Inspector cant display Dictionary vars,
        * so we use this little hack to setup the iconList
        */
        new void Start()
        {
            alignByGeometry = true;

            button = GetComponent<Button>();
            if (inspectorIconList != null && inspectorIconList.Length > 0)
            {
                foreach (IconName icon in inspectorIconList)
                {
                    // Debug.Log(icon.sprite.name);
                    iconList.Add(icon.name, icon.sprite);
                }
            }
        }

        protected void UpdateQuadImage(bool setSizeSign=true)
        {
#if UNITY_EDITOR
            if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
            {
                return;
            }
#endif
            m_OutputText = GetOutputText(setSizeSign);
            m_ImagesVertexIndex.Clear();
            foreach (Match match in s_Regex.Matches(m_OutputText))
            {
                var picIndex = match.Index;
                var endIndex = picIndex * 4 + 3;
                m_ImagesVertexIndex.Add(endIndex);

                m_ImagesPool.RemoveAll(image => image == null);
                if (m_ImagesPool.Count == 0)
                {
                    GetComponentsInChildren<Image>(m_ImagesPool);
                }
                if (m_ImagesVertexIndex.Count > m_ImagesPool.Count)
                {
                    var resources = new DefaultControls.Resources();
                    var go = DefaultControls.CreateImage(resources);
                    go.layer = gameObject.layer;
                    var rt = go.transform as RectTransform;
                    if (rt)
                    {
                        rt.SetParent(rectTransform);
                        rt.localPosition = Vector3.zero;
                        rt.localRotation = Quaternion.identity;
                        rt.localScale = Vector3.one;
                    }
                    m_ImagesPool.Add(go.GetComponent<Image>());
                }

                var spriteName = match.Groups[1].Value;
                //var size = float.Parse(match.Groups[2].Value);
                var img = m_ImagesPool[m_ImagesVertexIndex.Count - 1];
                if (img.sprite == null || img.sprite.name != spriteName)
                {
                    // img.sprite = resources.Load<Sprite>(spriteName);
                    if (inspectorIconList != null && inspectorIconList.Length > 0)
                    {
                        foreach (IconName icon in inspectorIconList)
                        {
                            if (icon.name == spriteName)
                            {
                                img.sprite = icon.sprite;
                                break;
                            }
                        }
                    }
                }
                //if (setSizeSign)
                //{
                    img.rectTransform.sizeDelta = new Vector2(fontSize * ImageScalingFactor, fontSize * ImageScalingFactor);
                    img.enabled = true;
                    
                //}
                if (positions.Count == m_ImagesPool.Count)
                {
                    img.rectTransform.anchoredPosition = positions[m_ImagesVertexIndex.Count - 1];
                }
            }

            for (var i = m_ImagesVertexIndex.Count; i < m_ImagesPool.Count; i++)
            {
                if (m_ImagesPool[i])
                {
                    /* TEMPORARY FIX REMOVE IMAGES FROM POOL DELETE LATER SINCE CANNOT DESTROY */
                    // m_ImagesPool[i].enabled = false;
                    m_ImagesPool[i].gameObject.SetActive(false);
                    m_ImagesPool[i].gameObject.hideFlags = HideFlags.HideAndDontSave;
                    culled_ImagesPool.Add(m_ImagesPool[i].gameObject);
                    m_ImagesPool.Remove(m_ImagesPool[i]);
                }
            }
            if (culled_ImagesPool.Count > 1)
            {
                clearImages = true;
            }
            if (m_ImagesPool.Count > 0)
            {
                lineSpacing = PicLineSpacing;
            }
            else
            {
                lineSpacing = NormalLineSpacing;
            }

        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            var orignText = m_Text;
            m_Text = m_OutputText;
            base.OnPopulateMesh(toFill);
            m_Text = orignText;
            positions.Clear();

            UIVertex vert = new UIVertex();
            for (var i = 0; i < m_ImagesVertexIndex.Count; i++)
            {
                var endIndex = m_ImagesVertexIndex[i];
                var rt = m_ImagesPool[i].rectTransform;
                var size = rt.sizeDelta;
                if (endIndex < toFill.currentVertCount)
                {
                    toFill.PopulateUIVertex(ref vert, endIndex);
                    positions.Add(new Vector2((vert.position.x + size.x / 2), (vert.position.y + size.y / 2)) + imageOffset);
                    // Erase the lower left corner of the black specks
                    toFill.PopulateUIVertex(ref vert, endIndex - 3);
                    var pos = vert.position;
                    for (int j = endIndex, m = endIndex - 3; j > m; j--)
                    {
                        toFill.PopulateUIVertex(ref vert, endIndex);
                        vert.position = pos;
                        vert.uv0 = size * 2;
                        toFill.SetUIVertex(vert, j);
                    }
                }
            }

            if (m_ImagesVertexIndex.Count != 0)
            {
                m_ImagesVertexIndex.Clear();
            }   
            // Hyperlinks surround processing box
            foreach (var hrefInfo in m_HrefInfos)
            {
                hrefInfo.boxes.Clear();
                if (hrefInfo.startIndex >= toFill.currentVertCount)
                {
                    continue;
                }

                // Hyperlink inside the text is added to surround the vertex index coordinate frame
                toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);
                var pos = vert.position;
                var bounds = new Bounds(pos, Vector3.zero);
                for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i++)
                {
                    if (i >= toFill.currentVertCount)
                    {
                        break;
                    }

                    toFill.PopulateUIVertex(ref vert, i);
                    pos = vert.position;
                    if (pos.x < bounds.min.x) // Wrap re-add surround frame
                    {
                        hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                        bounds = new Bounds(pos, Vector3.zero);
                    }
                    else
                    {
                        bounds.Encapsulate(pos); // Extended enclosed box
                    }
                }
                hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
            }
            UpdateQuadImage(false);
        }

        /// <summary>
        /// Hyperlink List
        /// </summary>
        private readonly List<HrefInfo> m_HrefInfos = new List<HrefInfo>();

        /// <summary>
        /// Text Builder
        /// </summary>
        private static readonly StringBuilder s_TextBuilder = new StringBuilder();

        /// <summary>
        /// Hyperlink Regular Expression
        /// </summary>
        private static readonly Regex s_HrefRegex =
            new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

        [Serializable]
        public class HrefClickEvent : UnityEvent<string> { }

        [SerializeField]
        private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

        /// <summary>
        /// Hyperlink Click Event
        /// </summary>
        public HrefClickEvent onHrefClick
        {
            get { return m_OnHrefClick; }
            set { m_OnHrefClick = value; }
        }



        /// <summary>
        /// Finally, the output text hyperlinks get parsed
        /// </summary>
        /// <returns></returns>
        protected string GetOutputText(bool clearSign=false)
        {
            s_TextBuilder.Length = 0;
            if(clearSign)
            {
                m_HrefInfos.Clear();
            }
            var indexText = 0;
            fixedString = this.text;
            if (inspectorIconList != null && inspectorIconList.Length > 0)
            {
                foreach (IconName icon in inspectorIconList)
                {
                    if (icon.name != null && icon.name != "")
                    {
                        fixedString = fixedString.Replace(icon.name, "<quad name=" + icon.name + " size=" + fontSize + " width=" + ImageScalingFactor + " />");
                    }
                }
            }
            foreach (Match match in s_HrefRegex.Matches(fixedString))
            {
                s_TextBuilder.Append(fixedString.Substring(indexText, match.Index - indexText));
                s_TextBuilder.Append("<color=" + hyperlinkColor + ">");  // Hyperlink color

                var group = match.Groups[1];
                var hrefInfo = new HrefInfo
                {
                    startIndex = s_TextBuilder.Length * 4, // Hyperlinks in text starting vertex indices
                    endIndex = (s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                    name = group.Value
                };
                m_HrefInfos.Add(hrefInfo);

                s_TextBuilder.Append(match.Groups[2].Value);
                s_TextBuilder.Append("</color>");
                indexText = match.Index + match.Length;
            }
            s_TextBuilder.Append(fixedString.Substring(indexText, fixedString.Length - indexText));

            return s_TextBuilder.ToString();
        }

        /// <summary>
        /// Click event is detected whether to click a hyperlink text
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 lp;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, eventData.position, eventData.pressEventCamera, out lp);

            foreach (var hrefInfo in m_HrefInfos)
            {
                var boxes = hrefInfo.boxes;
                for (var i = 0; i < boxes.Count; ++i)
                {
                    if (boxes[i].Contains(lp))
                    {
                        m_OnHrefClick.Invoke(hrefInfo.name);
                        return;
                    }
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //do your stuff when highlighted
            //selected = true;
            if (m_ImagesPool.Count >= 1)
            {
                foreach (Image img in m_ImagesPool)
                {
                    if (button != null && button.isActiveAndEnabled)
                    {
                        img.color = button.colors.highlightedColor;
                    }
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {

            //do your stuff when highlighted
            //selected = false;
            if (m_ImagesPool.Count >= 1)
            {
                foreach (Image img in m_ImagesPool)
                {
                    if (button != null && button.isActiveAndEnabled)
                    {
                        img.color = button.colors.normalColor;
                    }
                    else
                    {
                        img.color = color;
                    }
                }
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            //do your stuff when selected
            //selected = true;
            if (m_ImagesPool.Count >= 1)
            {
                foreach (Image img in m_ImagesPool)
                {
                    if (button != null && button.isActiveAndEnabled)
                    {
                        img.color = button.colors.highlightedColor;
                    }
                }
            }
        }

        /// <summary>
        /// Hyperlinks Info
        /// </summary>
        private class HrefInfo
        {
            public int startIndex;

            public int endIndex;

            public string name;

            public readonly List<Rect> boxes = new List<Rect>();
        }

        /* TEMPORARY FIX REMOVE IMAGES FROM POOL DELETE LATER SINCE CANNOT DESTROY */
        void Update()
        {
            if (clearImages)
            {
                for (int i = 0; i < culled_ImagesPool.Count; i++)
                {
                    DestroyImmediate(culled_ImagesPool[i]);
                }
                culled_ImagesPool.Clear();
                clearImages = false;
            }
        }

        public void SetIconList(IconName[] iconName)
        {
            inspectorIconList = iconName;
            if (inspectorIconList != null && inspectorIconList.Length > 0)
            {
                foreach (IconName icon in inspectorIconList)
                {
                    // Debug.Log(icon.sprite.name);
                    iconList.Add(icon.name, icon.sprite);
                }
            }
        }
    }
}