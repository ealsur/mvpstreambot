using System;
using System.Collections.Generic;

namespace mvpstreambot.Models
{
    public class Entry : Document
    {
        public Entry() : base("entry")
        {

        }
        public string id { get; set; }
        public string PublisherId { get; set; }
        public string PublisherNombre { get; set; }
        public string PublisherImagen { get; set; }
        public string Url { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }
        public string Imagen { get; set; }
        public string Lenguajes { get; set; }
        public DateTime Fecha { get; set; }
        public List<string> Tags { get; set; }
        public string Descripcion { get; set; }
    }

    public static class EntryExtensions
    {
        public static string ToMarkDown(this Entry source)
        {
            return $"* [{source.Titulo}]({source.Url}) por [{source.PublisherNombre}](http://mvpstream.azurewebsites.net/mvp/{source.PublisherId}) - {source.Fecha.ToString("dd/MM/yy")}";
        }
    }
}