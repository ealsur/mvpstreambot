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
        public async Task StartAsync(IDialogContext context)

        {

            context.Wait(MessageReceivedAsync);

        }



        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)

        {
            var message = await argument;
            if(!string.Empty.Equals(query) && message.Text.ToLowerInvariant().StartsWith("y "))
            {
                ;//Agregado de filtros
            }
            var resultados = SearchService.SearchDocuments(message.Text, null, null);
            if (resultados.Count > 0)
            {
                await context.PostAsync(string.Join("",resultados.Entries.Select(x=>x.ToMarkDown()).ToArray()));
            }
            else
            {
                await context.PostAsync("No encontré resultados para lo que estabas buscando.");
            }
            

            context.Wait(MessageReceivedAsync);

        }

    }


}