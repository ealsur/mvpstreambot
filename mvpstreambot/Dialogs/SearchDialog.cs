using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using mvpstreambot.Models;
using mvpstreambot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace mvpstreambot.Dialogs
{
    [Serializable]

    public class SearchDialog : IDialog<object>

    {
        private string query = string.Empty;
        private string filter = string.Empty;
        private int page = 1;
        public async Task StartAsync(IDialogContext context)

        {

            context.Wait(MessageReceivedAsync);

        }



        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)

        {
            var message = await argument;

            //LUIS integration
            var luisResponse = await LUISService.GetIntent(message.Text);
            var mostRelevantIntent = luisResponse.Intents.FirstOrDefault();
            if (mostRelevantIntent==null) {
                await context.PostAsync("No entendí lo que necesitabas, probá en [Stackoverflow](http://stackoverflow.com/).");
                return;
            }
            switch (mostRelevantIntent.Intent)
            {
                case "FindContent":
                    filter = luisResponse.Entities.Where(x => x.Type == "ContentType").Select(x=>x.Entity).FirstOrDefault();
                    query = luisResponse.Entities.Where(x => x.Type == "Topic").Select(x => x.Entity).FirstOrDefault();
                    if (string.IsNullOrEmpty(query))
                    {
                        query = message.Text.Split(' ').LastOrDefault();
                    }
                    page = 1;
                    await context.PostAsync($"Buscando contenido sobre {query}...");
                    await context.PostAsync(DoSearch(query, filter, page).ToMarkDown(query));
                    break;
                case "MoreResults":
                    ++page;
                    await context.PostAsync($"Buscando más sobre {query}...");
                    await context.PostAsync(DoSearch(query, filter, page).ToMarkDown(query));
                    break;
                case "AddContent":
                    var newQuery = luisResponse.Entities.Where(x => x.Type == "Topic").Select(x => x.Entity).FirstOrDefault();
                    if (string.IsNullOrEmpty(newQuery))
                    {
                        newQuery = message.Text.Split(' ').LastOrDefault();
                    }
                    query = query +" "+ newQuery;
                    page = 1;
                    await context.PostAsync($"Buscando contenido sobre {query}...");
                    await context.PostAsync(DoSearch(query, filter, page).ToMarkDown(query));
                    break;
                case "Greetings":
                    await context.PostAsync("Hola! Soy una entidad etérea creada por [ealsur](https://twitter.com/ealsur) usando [Bot Framework](https://dev.botframework.com/) y [LUIS](https://www.luis.ai/).");
                    await context.PostAsync("Probá *quiero ver videos de azure* o *mostrame lo que tengas sobre powershell*.");
                    break;
                default:
                    await context.PostAsync("No entendí lo que necesitabas, probá con preguntas como *quiero ver videos de azure* o *mostrame material sobre sharepoint*.");
                    break;
            }
            
            context.Wait(MessageReceivedAsync);

        }

        private static SearchResults DoSearch(string query, string filter, int page=1)
        {
            string tipo = null;
            if (filter.ToLowerInvariant().Contains("video"))
            {
                tipo = "Tipo eq 'Video'";
            }
            if (filter.ToLowerInvariant().Contains("articul") || filter.ToLowerInvariant().Contains("post") || filter.ToLowerInvariant().Contains("artícul"))
            {
                tipo = "Tipo eq 'RSS'";
            }
            return SearchService.SearchDocuments(query, tipo, "Fecha desc", page);
        }

    }


}