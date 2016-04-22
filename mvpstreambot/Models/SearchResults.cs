using System.Collections.Generic;
using System.Text;

namespace mvpstreambot.Models
{
    public class SearchResults
    {
        public long Count { get; set; }
        public IEnumerable<Entry> Entries { get; set; }
    }

    public static class SearchResultsExtensions
    {
        public static string ToMarkDown(this SearchResults source, string query)
        {
            var retval = new StringBuilder();
            retval.AppendLine($"Encontré **{source.Count} resultados.** para tu búsqueda *{query}*");
            foreach (var item in source.Entries)
            {
                retval.AppendLine(item.ToMarkDown());
            }
            if (source.Count > 10)
            {
                retval.AppendLine($"Si querés ver más, pedime *ver más*");
            }
            return retval.ToString();
        }
    }
}