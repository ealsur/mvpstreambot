using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
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

        public async Task StartAsync(IDialogContext context)

        {

            context.Wait(MessageReceivedAsync);

        }



        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)

        {

            var message = await argument;

            await context.PostAsync("You said: " + message.Text);

            context.Wait(MessageReceivedAsync);

        }

    }


}