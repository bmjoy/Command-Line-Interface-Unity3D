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

    /// <summary>
    /// Returns all methods from active <see cref="MonoBehaviour"/>s in the scene
    /// </summary>
    /// <returns></returns>
    private MethodInfo[] GetAllMethods()
    {
        MonoBehaviour[] active = FindObjectsOfType<MonoBehaviour>();

        List<MethodInfo> methods = new List<MethodInfo>();

        foreach (MonoBehaviour mono in active)
        {
            methods.AddRange(mono.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public));
        }

        return methods.ToArray();
    }

    /// <summary>
    /// Returns a <see cref="ConsoleCommand"/>, if there is no Atrribute assigned it will return null
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    private ConsoleCommand GetConsoleCommand(MethodInfo method)
    {
        return Attribute.GetCustomAttribute(method, typeof(ConsoleCommand)) as ConsoleCommand;
    }

    /// <summary>
    /// Returns true if a method is a console command
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    private bool IsCommand(MethodInfo method)
    {
        ConsoleCommand cmd = Attribute.GetCustomAttribute(method, typeof(ConsoleCommand)) as ConsoleCommand;

        return (cmd == null) ? false : true;
    }


}
