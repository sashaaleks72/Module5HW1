#nullable disable

namespace Module5HW1.Models
{
    using Newtonsoft.Json;

    public class Resource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public string Color { get; set; }

        [JsonProperty("pantone_value")]
        public string PantoneValue { get; set; }
    }
}
