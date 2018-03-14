using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OhMyBot.Dialogs
{


    [Serializable]
    [LuisModel("3f9cd9dc-03bd-4281-b152-330cd0195f9e", "dc80ab4f6fc94349b5c95f445f156b84")]
    public class AzureLUISDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("show")]
        public async Task Show(IDialogContext context, LuisResult result)
        {
            EntityRecommendation ObjectType;
            string message = "";
            // Try to find the entity "type"
            if (result.TryFindEntity("type", out ObjectType))
            {
                // Try to find the resolution
                string ResolvedName = ((List<object>)ObjectType.Resolution["values"]).Cast<string>().FirstOrDefault();
                message = $"Hello there, it looks like you want to show " + ObjectType.Entity +
                           " (also known as '" + ResolvedName + "'). " +
                           "Unfortunately, I do not know yet how to do that just yet :-(";
            }
            else
            {
                message = $"Hello there, it looks like you want to show something, but I dont know what";
            }
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("exit")]
        public async Task Exit(IDialogContext context, LuisResult result)
        {
            context.Done<object>(null);
        }

    }

}