using Newtonsoft.Json;

namespace Pingu.Settings
{
    public class Config
    {
        [JsonProperty(Required = Required.Always)]
        public string Salt { get; set; }
    }
}
