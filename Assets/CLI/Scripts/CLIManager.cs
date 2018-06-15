using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CLIManager : MonoBehaviour {

    [SerializeField]
    private RectTransform m_CLIPanel;

    [SerializeField]
    private InputField m_InputField;

    [SerializeField]
    private Text m_SuggestionsText;

    [SerializeField]
    private Text m_OutputText;

    private void Start()
    {
        var eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
            Debug.LogError("No UI EventSystem is present in the current scene!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Toggled CLI");
            m_CLIPanel.gameObject.SetActive(!m_CLIPanel.gameObject.activeInHierarchy);

            if (m_CLIPanel.gameObject.activeInHierarchy)
            {
                m_InputField.Select();
                m_InputField.ActivateInputField();
            }
        }
    }
}
