using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CLI
{
    public class CLIMenuItem : MonoBehaviour
    {
        public RectTransform panelPrefab;
        public RectTransform menuItemPrefab;

        [HideInInspector]
        public CLINode node;

        public RectTransform arrow;

        RectTransform rectTransform;

        [HideInInspector]
        public bool panelOpen = false;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnClick()
        {
            if (node.children.Count > 0)
            {
                if (!panelOpen)
                {
                    RectTransform panel = Instantiate(panelPrefab, CLIManager.Instance.transform).GetComponent<RectTransform>();
                    panel.SetParent(CLIManager.Instance.transform);

                    CLIMenu.Instance.openPanels.Add(panel);

                    //RectTransform parent = transform.parent.GetComponentInParent<RectTransform>();

                    RectTransform parentPanel = transform.parent.GetComponent<RectTransform>();


                    //panel.anchoredPosition = parent.anchoredPosition + new Vector2(parent.rect.width, 0);
                    panel.anchoredPosition = parentPanel.anchoredPosition + new Vector2(parentPanel.rect.width, rectTransform.anchoredPosition.y);

                    foreach (var n in node.children)
                    {
                        RectTransform menuItem = Instantiate(menuItemPrefab).GetComponent<RectTransform>();
                        menuItem.SetParent(panel);
                        menuItem.GetComponentInChildren<Text>().text = n.name;
                        CLIMenuItem m = menuItem.GetComponent<CLIMenuItem>();
                        m.node = n;
                        if (n.children.Count > 0)
                        {
                            m.arrow.gameObject.SetActive(true);
                        }
                        else
                        {
                            m.arrow.gameObject.SetActive(false);
                        }
                    }

                    panelOpen = true;
                }
            }
            else
            {
                if (node.method.GetParameters().Length == 0)
                {
                    node.method.Invoke(node.monoBehaviour, null);
                    CLIMenu.Instance.ResetAllMenuButtons();
                    CLIMenu.Instance.DestroyAllOpenPanels();
                }
                else
                    Debug.LogWarning("Methods with parameters in the button menu are not supported yet!");
            }
        }
    }
}
