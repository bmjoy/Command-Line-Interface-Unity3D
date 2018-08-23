using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CLI
{
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

        private MethodData[] methodData;

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
            ClearOutput();

            m_InputField.onValueChanged.AddListener(delegate { OnInputFieldChanged(); });

            methodData = GetMethodData();

            // Construct node tree
            ConstructNodeTree();

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

                CLISuggestion[] suggestions = GetSuggestions(input);

                if (suggestions.Length > 0)
                {
                    m_InputField.text = suggestions[0].path;
                    m_InputField.MoveTextEnd(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                string[] split = m_InputField.text.Split(' ');

                CLINode node = FindNode(split[0]);

                if (node != null && node.method != null)
                {
                    // Valid command

                    List<object> parameters = new List<object>();

                    var parameterTypes = node.method.GetParameters();

                    for (int i = 1; i < split.Length; i++)
                    {
                        //int x = Convert.ToInt32(split[i]);
                        //print(parameterTypes[i-1].ParameterType.ToString() + (parameterTypes[i - 1].ParameterType == typeof(int)));

                        try
                        {
                            var x = Convert.ChangeType(split[i], parameterTypes[i - 1].ParameterType);
                            parameters.Add(x);

                        }
                        catch (Exception e)
                        {

                            Debug.LogError(e);
                        }

                    }
                    object obj = node.method.Invoke(node.monoBehaviour, parameters.ToArray());
                    PrintOutput(m_InputField.text + "\n" + obj);

                }
                else
                {
                    PrintOutput("'" + m_InputField.text + "' is not a valid command!");
                }

                m_InputField.text = "";
                m_InputField.ActivateInputField();
            }
        }

        private void OnInputFieldChanged()
        {
            string input = m_InputField.text;


            CLISuggestion[] suggestions = GetSuggestions(input);
            m_SuggestionsText.text = "";

            string s = "";
            for (int i = 0; i < suggestions.Length; i++)
            {
                s += suggestions[i].path + ((suggestions[i].node.children.Count > 0) ? "..." : "") + " " + suggestions[i].typeText + "\n";
            }

            m_SuggestionsText.text = s;
        }


        private CLISuggestion[] GetSuggestions(string path)
        {
            List<CLISuggestion> suggestions = new List<CLISuggestion>();

            if (path == "") return suggestions.ToArray();

            string[] split = path.Split('.');

            int lastSeparatorIndex = path.LastIndexOf('.');

            if (lastSeparatorIndex < 0) lastSeparatorIndex = 0;

            string parentPath = path.Substring(0, lastSeparatorIndex);

            CLINode lastCompleteNode = FindNode(parentPath);

            List<CLINode> currentNodes = new List<CLINode>();

            if (lastCompleteNode == null)
            {
                // Only use the whole node tree if there is no seperator ('.') present
                if (!path.Contains("."))
                    currentNodes = nodes;
            }
            else
            {
                currentNodes = lastCompleteNode.children;
            }

            foreach (var node in currentNodes)
            {
                if (/*split[split.Length - 1] != "" &&*/ node.name.ToLower().StartsWith(split[split.Length - 1].ToLower()))
                {
                    string suggestion = parentPath + ((parentPath == "") ? "" : ".") + node.name;
                    string typeText = "";

                    if (node.method != null)
                    {
                        var parameters = node.method.GetParameters();

                        if (parameters.Length > 0)
                        {
                            typeText = "(";

                            for (int i = 0; i < parameters.Length; i++)
                            {
                                typeText += TypeAliases[parameters[i].ParameterType];
                                if (i < parameters.Length - 1) typeText += ", ";
                            }

                            typeText += ")";
                        }
                    }


                    suggestions.Add(new CLISuggestion(suggestion, typeText, node));
                }

            }


            return suggestions.ToArray();
        }

        private void PrintOutput(string text)
        {
            m_OutputText.text += text + "\n\n";
        }

        private void ClearSuggestions()
        {
            m_SuggestionsText.text = "";
        }

        private void ClearOutput()
        {
            m_OutputText.text = "";
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

        private CLINode GetNode(List<CLINode> nodeCollection, string name)
        {
            for (int i = 0; i < nodeCollection.Count; i++)
            {
                if (nodeCollection[i].name == name)
                {
                    return nodeCollection[i];
                }
                else
                {
                    continue;
                }
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

        private void ConstructNodeTree()
        {
            //List<MethodInfo> methods = new List<MethodInfo>();
            //for (int i = 0; i < methodData.Length; i++)
            //{
            //    methods.AddRange(methodData[i].methods);
            //}

            //MethodInfo[] me = GetMethodData();

            for (int m = 0; m < methodData.Length; m++)
            {

                for (int i = 0; i < methodData[m].methods.Count; i++)
                {
                    ConsoleCommand command = GetConsoleCommand(methodData[m].methods[i]);

                    if (command != null)
                    {
                        // Custom path

                        if (command.customPath != "")
                        {

                            string[] split = command.customPath.Split('.');

                            List<CLINode> currentNodeList = nodes;

                            for (int splitIndex = 0; splitIndex < split.Length; splitIndex++)
                            {
                                CLINode n = new CLINode();
                                n = GetNode(currentNodeList, split[splitIndex]);

                                if (n == null)
                                {
                                    n = new CLINode();
                                    n.name = split[splitIndex];
                                    currentNodeList.Add(n);
                                }

                                currentNodeList = n.children;

                                if (splitIndex == split.Length - 1)
                                {
                                    CLINode finalNode = new CLINode();
                                    finalNode.name = methodData[m].methods[i].Name;
                                    finalNode.method = methodData[m].methods[i];
                                    finalNode.monoBehaviour = methodData[m].monoBehaviour;
                                    n.children.Add(finalNode);
                                }
                            }

                        }

                        // Type path

                        CLINode childNode = new CLINode();

                        childNode.name = methodData[m].methods[i].Name;
                        childNode.method = methodData[m].methods[i];
                        childNode.monoBehaviour = methodData[m].monoBehaviour;

                        string baseNodeName = methodData[m].methods[i].DeclaringType.ToString();

                        CLINode baseNode = GetNode(nodes, baseNodeName);

                        if (baseNode != null)
                        {
                            // Base node exists
                            baseNode.children.Add(childNode);
                        }
                        else
                        {
                            // Base node doesn't exist
                            baseNode = new CLINode();

                            baseNode.name = methodData[m].methods[i].DeclaringType.ToString();
                            baseNode.children.Add(childNode);

                            nodes.Add(baseNode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a list of <see cref="MethodData"/> from active <see cref="MonoBehaviour"/>s in the scene
        /// </summary>
        /// <returns></returns>
        private MethodData[] GetMethodData()
        {
            MonoBehaviour[] active = FindObjectsOfType<MonoBehaviour>();

            List<MethodData> methodData = new List<MethodData>();

            foreach (MonoBehaviour mono in active)
            {
                MethodData data = new MethodData();
                data.methods.AddRange(mono.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public));
                data.monoBehaviour = mono;

                if (data.methods.Count > 0)
                    methodData.Add(data);
            }

            return methodData.ToArray();
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

        private static readonly Dictionary<Type, string> TypeAliases = new Dictionary<Type, string>()
    {
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(object), "object" },
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(string), "string" },
        { typeof(void), "void" },
        { typeof(Nullable<byte>), "byte?" },
        { typeof(Nullable<sbyte>), "sbyte?" },
        { typeof(Nullable<short>), "short?" },
        { typeof(Nullable<ushort>), "ushort?" },
        { typeof(Nullable<int>), "int?" },
        { typeof(Nullable<uint>), "uint?" },
        { typeof(Nullable<long>), "long?" },
        { typeof(Nullable<ulong>), "ulong?" },
        { typeof(Nullable<float>), "float?" },
        { typeof(Nullable<double>), "double?" },
        { typeof(Nullable<decimal>), "decimal?" },
        { typeof(Nullable<bool>), "bool?" },
        { typeof(Nullable<char>), "char?" }
    };

    }
}
