using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CLI
{
    public class CLIMenuButton : MonoBehaviour
    {
        public GameObject menuDropdownPrefab;
        public GameObject menuItemPrefab;
        [HideInInspector]
        public bool panelOpen = false;
        [HideInInspector]
        public bool isRoot = false;

        [HideInInspector]
        public CLINode node;

        public void OnClick()
        {
            if (!panelOpen)
            {
                CLIMenu.Instance.ResetAllMenuButtons();
                CLIMenu.Instance.DestroyAllOpenPanels();

                RectTransform rectTransform = GetComponent<RectTransform>();

                RectTransform panel = Instantiate(menuDropdownPrefab).GetComponent<RectTransform>();
                panel.transform.SetParent(CLIManager.Instance.transform);

                CLIMenu.Instance.openPanels.Add(panel);

                panel.anchoredPosition = rectTransform.anchoredPosition - new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);

                CLIMenuItemPanel p = panel.GetComponent<CLIMenuItemPanel>();
                p.button = this;

                CLIMenu.Instance.openPanels.Add(panel);

                foreach (var node in node.children)
                {
                    RectTransform menuItem = Instantiate(menuItemPrefab).GetComponent<RectTransform>();
                    menuItem.SetParent(panel);
                    menuItem.GetComponentInChildren<Text>().text = node.name;
                    CLIMenuItem m = menuItem.GetComponent<CLIMenuItem>();
                    m.node = node;
                    if (node.children.Count > 0)
                        m.arrow.gameObject.SetActive(true);
                }
                

                panelOpen = true;
            }
        }
    }
}
