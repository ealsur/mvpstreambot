using System.Collections.Generic;

namespace mvpstreambot.Models
{
    public class SearchResults
    {
        public long Count { get; set; }
        public IEnumerable<Entry> Entries { get; set; }
    }
}