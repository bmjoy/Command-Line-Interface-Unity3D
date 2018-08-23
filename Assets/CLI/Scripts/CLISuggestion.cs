
namespace CLI
{
    public class CLISuggestion
    {
        public CLISuggestion() { }

        public CLISuggestion(string path, string typeText, CLINode node)
        {
            this.path = path;
            this.typeText = typeText;
            this.node = node;
        }

        public string path;
        public string typeText;
        public CLINode node;
    }
}
