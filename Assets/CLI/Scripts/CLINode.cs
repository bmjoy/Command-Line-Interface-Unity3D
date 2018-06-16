using System.Collections.Generic;
using System.Reflection;

public class CLINode
{

    public CLINode() {}

    public CLINode(MethodInfo method, List<CLINode> children, string name)
    {
        this.method = method;
        this.children = children;
        this.name = name;
    }

    public MethodInfo method;

    public List<CLINode> children = new List<CLINode>();

    public string name;

}
