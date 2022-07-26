namespace Module5HW1.Models
{
    using Newtonsoft.Json;

    public class PageInfo<T>
    {
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        public List<T>? Data { get; set; }
    }
}
