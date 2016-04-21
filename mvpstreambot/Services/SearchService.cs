using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Search;
using mvpstreambot.Models;
using Microsoft.Azure.Search.Models;
using System.Configuration;

namespace mvpstreambot.Services
{
    public static class SearchService
    {

        static readonly SearchServiceClient client;
        static readonly SearchIndexClient indexClient;
        static SearchService()
        {
            client = new SearchServiceClient(ConfigurationManager.AppSettings["SearchAccount"], new SearchCredentials(ConfigurationManager.AppSettings["SearchKey"]));
            indexClient = client.Indexes.GetClient("entries");
        }
        
        public static SearchResults SearchDocuments(string searchText, string filter, string orderBy, int page = 1, int pageSize = 10)
        {

            var sp = new SearchParameters();
            if (!string.IsNullOrEmpty(filter))
            {
                sp.Filter = filter;
            }
            sp.Top = pageSize;
            sp.Skip = (page - 1) * pageSize;
            if (!string.IsNullOrEmpty(orderBy))
            {
                sp.OrderBy = orderBy.Split(',');
            }
            sp.IncludeTotalResultCount = true;
            var results = indexClient.Documents.Search(searchText, sp);
            var model = new SearchResults() { };

            model.Entries = results.Results.Select(x => new Entry()
            {
                id = (string)x.Document["id"],
                Descripcion = (string)x.Document["Descripcion"],
                Fecha = ((DateTimeOffset)x.Document["Fecha"]).DateTime,
                Imagen = (string)x.Document["Imagen"],
                Lenguajes = (string)x.Document["Lenguajes"],
                PublisherId = (string)x.Document["PublisherId"],
                PublisherImagen = (string)x.Document["PublisherImagen"],
                PublisherNombre = (string)x.Document["PublisherNombre"],
                Tags = ((string[])x.Document["Tags"]).ToList(),
                Tipo = (string)x.Document["Tipo"],
                Titulo = (string)x.Document["Titulo"],
                Url = (string)x.Document["Url"]
            }).ToList();
            model.Count = results.Count.Value;
            return model;
        }
    }
}