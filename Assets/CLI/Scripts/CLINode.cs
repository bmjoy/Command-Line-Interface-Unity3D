using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CLINode
{

    public CLINode() {}

    public CLINode(MonoBehaviour monoBehaviour, MethodInfo method, List<CLINode> children, string name)
    {
        this.monoBehaviour = monoBehaviour;
        this.method = method;
        this.children = children;
        this.name = name;
    }

    public MonoBehaviour monoBehaviour;

    public MethodInfo method;

    public List<CLINode> children = new List<CLINode>();

    public string name;

}
