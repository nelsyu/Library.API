using Newtonsoft.Json;

namespace Library.API.Models
{
    public abstract class Resource
    {
        [JsonProperty("_links", Order = 100)]
        public List<Link> Links { get; } = new List<Link>();
    }
}
