using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

        GetAllMethods();
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

    private MethodInfo[] GetAllMethods()
    {
        MonoBehaviour[] active = FindObjectsOfType<MonoBehaviour>();

        List<MethodInfo> methods = new List<MethodInfo>();

        foreach (MonoBehaviour mono in active)
        {
            methods.AddRange(mono.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public));

            //for (int i = 0; i < methods.Length; i++)
            //{
            //    ConsoleCommand cmd = Attribute.GetCustomAttribute(methods[i], typeof(ConsoleCommand)) as ConsoleCommand;



            //    if (cmd != null)
            //    {
            //        Debug.Log(methods[i].DeclaringType);
            //        Debug.Log(methods[i].Name);
            //        Debug.Log(methods[i].ReturnParameter.ParameterType);
            //        methods[i].Invoke(mono, null);
            //    }
            //}
        }

        return methods.ToArray();
    }
}
