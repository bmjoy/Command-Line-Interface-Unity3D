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

    private List<CLINode> nodes = new List<CLINode>();

    private void Start()
    {
        var eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
        {
            Debug.LogError("No UI EventSystem is present in the current scene!");
            return;
        }

        // Initialization
        m_InputField.onValueChanged.AddListener(delegate { OnInputFieldChange(); });

        MethodInfo[] methods = GetAllMethods();

        

        // Construct node tree
        for (int i = 0; i < methods.Length; i++)
        {
            CLINode childNode = new CLINode();

            childNode.name = methods[i].Name;
            childNode.method = methods[i];

            string baseNodeName = methods[i].DeclaringType.ToString();

            CLINode baseNode = GetNodeByName(baseNodeName);

            if(baseNode != null)
            {
                // Base node exists
                baseNode.children.Add(childNode);
            }
            else
            {
                // Base node doesn't exist
                baseNode = new CLINode();

                baseNode.name = methods[i].DeclaringType.ToString();
                baseNode.children.Add(childNode);

                nodes.Add(baseNode);
            }
            
            
            
        }


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

        if (Input.GetKeyDown(KeyCode.Tab))
        {

        }


    }

    private void OnInputFieldChange()
    {
        foreach (var node in nodes)
        {

        }
    }

    private CLINode GetNodeByName(string name)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            return (nodes[i].name == name) ? nodes[i] : null;
        }
        return null;
    }

    private bool NodeExists(string name)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            return (nodes[i].name == name) ? true : false;
        }
        return false;
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
    /// Returns an array of <see cref="ConsoleCommand"/>s
    /// </summary>
    /// <param name="methods"></param>
    /// <returns></returns>
    private ConsoleCommand[] GetConsoleCommands(MethodInfo[] methods)
    {
        List<ConsoleCommand> cmds = new List<ConsoleCommand>();

        for (int i = 0; i < methods.Length; i++)
        {
            ConsoleCommand cmd = Attribute.GetCustomAttribute(methods[i], typeof(ConsoleCommand)) as ConsoleCommand;

            if (cmd != null)
                cmds.Add(cmd);
        }

        return cmds.ToArray();
    }

    /// <summary>
    /// Returns true if a method is a console command
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    private bool IsConsoleCommand(MethodInfo method)
    {
        ConsoleCommand cmd = Attribute.GetCustomAttribute(method, typeof(ConsoleCommand)) as ConsoleCommand;

        return (cmd == null) ? false : true;
    }


}
