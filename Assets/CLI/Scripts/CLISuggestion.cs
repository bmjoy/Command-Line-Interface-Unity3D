
public class CLISuggestion
{
    public CLISuggestion() { }

    public CLISuggestion(string path, CLINode node)
    {
        this.path = path;
        this.node = node;
    }

    public string path;
    public CLINode node;
}
