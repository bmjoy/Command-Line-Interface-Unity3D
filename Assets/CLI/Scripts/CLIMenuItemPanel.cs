using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CLI
{
    public class CLIMenuItemPanel : MonoBehaviour
    {
        [HideInInspector]
        public CLIMenuButton button;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    CLIMenu.Instance.DestroyAllOpenPanels();
                    CLIMenu.Instance.ResetAllMenuButtons();
                }
            }
        }
    }
}
