using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI
{
    public class CLIMenuButton : MonoBehaviour
    {
        public GameObject menuDropdownPrefab;
        [HideInInspector]
        public bool dropdownOpen = false;
        [HideInInspector]
        public bool isRoot = false;

        public void OnClick()
        {
            if (!dropdownOpen)
            {
                if (isRoot)
                    CLIManager.Instance.DestroyAllDropDowns();

                RectTransform rectTransform = GetComponent<RectTransform>();

                RectTransform dropdown = Instantiate(menuDropdownPrefab).GetComponent<RectTransform>();
                dropdown.transform.SetParent(CLIManager.Instance.transform);

                dropdown.anchoredPosition = rectTransform.anchoredPosition - new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);

                CLIMenuDropDown d = dropdown.GetComponent<CLIMenuDropDown>();
                d.button = this;

                CLIManager.Instance.openDropdowns.Add(d);

                dropdownOpen = true;
            }
        }
    }
}
