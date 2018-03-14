using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace OhMyBot.Dialogs.Help
{
#pragma warning disable 1998

    [Serializable]
    public class ScorableHelpDialog : IDialog<object>
    {

        protected string topic;

        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"You have been shorcutted to the help dialog.{Environment.NewLine}What do you need help with?");

            // State transition - wait for 'payee' message from user
            context.Wait(MessageReceivedTopic);
        }

        public async Task MessageReceivedTopic(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            this.topic= message.Text;

            await context.PostAsync($"To get help on {this.topic}, I can recommend you to Bing it up.{Environment.NewLine}Now I will take you to where you were before...");


            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }

    }
}