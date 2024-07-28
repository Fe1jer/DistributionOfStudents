namespace Shared.Helpers
{
    public class ConfigurationHelper
    {
        public static ConfigurationHelper Current;

        public ConfigurationHelper()
        {
            Current = this;
        }
        public bool IsClient { get; set; }
        public bool IsRelease { get; set; }
        public string Base { get; set; }
    }
}
