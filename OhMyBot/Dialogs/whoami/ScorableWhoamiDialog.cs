using System;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace OhMyBot.Dialogs.whoami
{
#pragma warning disable 1998

    [Serializable]
    public class ScorableWhoamiDialog : IDialog<object>
    {

        protected string topic;

        // Entry point to the Dialog
        // This dialog only prints information out of the context argument, and leaves
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"You have been shortcutted to the whoami dialog");
            await context.PostAsync($"Context.Activity.From.Id: " + context.Activity.From.Id.ToString());
            await context.PostAsync($"Context.Activity.From.Name: " + context.Activity.From.Name.ToString());
            await context.PostAsync($"Context.Activity.Recipient.Id: " + context.Activity.Recipient.Id.ToString());
            await context.PostAsync($"Context.Activity.Recipient.Name: " + context.Activity.Recipient.Name.ToString());
            await context.PostAsync($"Context.Activity.ChannelId: " + context.Activity.ChannelId.ToString());
            await context.PostAsync($"Context.Activity.ServiceUrl: " + context.Activity.ServiceUrl.ToString());
            await context.PostAsync($"Context.Activity.LocalTimestamp: " + context.Activity.LocalTimestamp.ToString());
            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }

    }
}