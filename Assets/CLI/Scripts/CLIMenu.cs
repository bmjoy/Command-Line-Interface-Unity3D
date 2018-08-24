using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI
{
    public class CLIMenu : MonoBehaviour
    {

        public static CLIMenu Instance { get; set; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public List<CLIMenuButton> buttons = new List<CLIMenuButton>();
        public List<RectTransform> openPanels = new List<RectTransform>();

        public void DestroyAllOpenPanels()
        {
            for (int i = openPanels.Count - 1; i >= 0; i--)
            {
                Destroy(openPanels[i].gameObject);
                openPanels.Remove(openPanels[i]);
            }
        }

        public void ResetAllMenuButtons()
        {
            foreach (var button in buttons)
            {
                button.panelOpen = false;
            }
        }

    }
}
