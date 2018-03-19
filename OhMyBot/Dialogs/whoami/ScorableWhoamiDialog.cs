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
        string Username;

        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"You have been shorcutted to the whoami dialog.{Environment.NewLine}You must be someone important");
            if (context.UserData.TryGetValue("Name", out Username))
            {
                await context.PostAsync($"You must be " + Username);
            }
            else
            {
                await context.PostAsync($"Context.Activity.From.Id: " + context.Activity.From.Id.ToString());
                await context.PostAsync($"Context.Activity.ChannelId: " + context.Activity.ChannelId.ToString());
            }
            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }

    }
}