using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CLIManager : MonoBehaviour
{

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
        m_InputField.Select();
        m_InputField.ActivateInputField();

        ClearSuggestions();
        //ClearOutput();

        m_InputField.onValueChanged.AddListener(delegate { OnInputFieldChanged(); });

        MethodInfo[] methods = GetAllMethods();



        // Construct node tree
        for (int i = 0; i < methods.Length; i++)
        {
            ConsoleCommand command = GetConsoleCommand(methods[i]);

            if (command != null)
            {
                CLINode childNode = new CLINode();

                childNode.name = methods[i].Name;
                childNode.method = methods[i];

                string baseNodeName = methods[i].DeclaringType.ToString();

                CLINode baseNode = GetNodeByName(baseNodeName);

                if (baseNode != null)
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

        //print(nodes[0].children[0].name);

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
            string input = m_InputField.text;

            foreach (var node in nodes)
            {
                if (input != "" && node.name.ToLower().StartsWith(input.ToLower()))
                {
                    m_InputField.text = node.name + ".";
                    m_InputField.caretPosition = m_InputField.text.Length;
                }
            }
        }

    }

    private void OnInputFieldChanged()
    {
        string input = m_InputField.text;

        string[] splitInput = input.Split('.');

        int splitIndex = 0;

        CLINode n = FindNode(input);

        if (n != null)
            print(n.name);

        //foreach (var node in nodes)
        //{
        //    // Compare nodes with input
        //}

        //foreach (var node in nodes)
        //{
        //    if (input != "" && node.name.ToLower().StartsWith(input.ToLower()))
        //    {
        //        ClearSuggestions();
        //        m_SuggestionsText.text = node.name + "...";
        //    }
        //    else
        //    {
        //        ClearSuggestions();
        //    }
        //}
    }

    private void ClearSuggestions()
    {
        m_SuggestionsText.text = "";
    }

    private void ClearOutput()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a node with the given path (node names separated with '.')
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private CLINode FindNode(string path)
    {
        string[] split = path.Split('.');
        
        List<CLINode> currentNodes = nodes;

        for (int splitIndex = 0; splitIndex < split.Length; splitIndex++)
        {
            foreach (var node in currentNodes)
            {
                if (node.name == split[splitIndex]) 
                {
                    // Found a matching node

                    if (splitIndex == split.Length - 1) return node;

                    if (node.children.Count > 0)
                    {
                        currentNodes = node.children;
                    }
                    else
                    {
                        return node;
                    }
                }
            }
        }

        return null;
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
