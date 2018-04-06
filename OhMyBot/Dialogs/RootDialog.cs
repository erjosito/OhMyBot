using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using OhMyBot.Dialogs.Sandwich;
using Microsoft.Bot.Builder.FormFlow;

namespace OhMyBot.Dialogs
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await this.SendWelcomeMessage(context);
            //return Task.CompletedTask;
        }

        private async Task RootLoop(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("count"))
            {
                // User said 'count', so invoke the Echo Counter and wait for it to finish.
                // Then, call ResumeAfterEchoCounterDialog.
                var myEchoCounter = new EchoCounterDialog();
                context.Call(myEchoCounter, this.ResumeAfterEchoCounterDialog);
            }
            else if (activity.Text.ToLower().Contains("sandw"))
            {
                //await Conversation.SendAsync(activity, MakeSandwichDialog);
                var mySandwichDialog = MakeSandwichDialog();
                context.Call(mySandwichDialog, ResumeAfterSandwichDialog);
            }
            else if (activity.Text.ToLower().Contains("city"))
            {
                await context.PostAsync("Let us start with the city dialog...");
                context.Call(new StateDialog(), ResumeAfterStateDialog);
            }
            else if (activity.Text.ToLower().Contains("qna"))
            {
                await context.PostAsync("Let us start with the QnA dialog...");
                context.Call(new QnADialog(), ResumeAfterQnADialog);
            }
            else if (activity.Text.ToLower().Contains("azure"))
            {
                await context.PostAsync("You are now entering the LUIS dialog for Azure commands");
                context.Call(new AzureLUISDialog(), ResumeAfterAzureDialog);
            }
            else
            {
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;
                // return our reply to the user
                await context.PostAsync($"You sent {activity.Text} which was {length} characters");

                // Wait for the next message
                context.Wait(RootLoop);
            }

        }

        private async Task SendWelcomeMessage(IDialogContext context)
        {
            await context.PostAsync("You are now at the root dialog");
            //await context.PostAsync("I am going to invoke now the Echo Counter Dialog");
            //context.Call(new EchoCounterDialog(), this.ResumeAfterEchoCounterDialog);
            context.Wait(RootLoop);
        }

    
        private async Task ResumeAfterEchoCounterDialog(IDialogContext context, IAwaitable<int> result)
        {
            // Store the value that NewOrderDialog returned. 
            // (At this point, new order dialog has finished and returned some value to use within the root dialog.)
            int resultFromEchoCounter = await result;

            await context.PostAsync($"Echo Counter Dialog is finished with result "
                                     + resultFromEchoCounter.ToString() 
                                     + ". You are now back into the root dialog");

            // Go back to the root loop
            context.Wait(this.RootLoop);
        }

        private async Task ResumeAfterSandwichDialog(IDialogContext context, IAwaitable<SandwichOrder> result)
        {
            SandwichOrder order = await result;
            await context.PostAsync($"I will order that sandwich for you. You are now back into the root dialog");
            // Go back to the root loop
            context.Wait(this.RootLoop);
        }

        private async Task ResumeAfterAzureDialog(IDialogContext context, IAwaitable<object> result)
        {
            object AzureObject = await result;
            await context.PostAsync($"Thanks for using Azure. You are now back into the root dialog");
            // Go back to the root loop
            context.Wait(this.RootLoop);
        }

        private async Task ResumeAfterQnADialog(IDialogContext context, IAwaitable<object> result)
        {
            object AzureObject = await result;
            await context.PostAsync($"Thanks for using the QnA dialog, I hope I could answer your questions. You are now back into the root dialog");
            // Go back to the root loop
            context.Wait(this.RootLoop);
        }


        private async Task ResumeAfterStateDialog(IDialogContext context, IAwaitable<object> result)
        {
            object StateObject = await result;
            await context.PostAsync($"Thanks for using the state dialog. You are now back into the root dialog");
            // Go back to the root loop
            context.Wait(this.RootLoop);
        }

        internal static IDialog<SandwichOrder> MakeSandwichDialog()
        {
            return Chain.From(() => FormDialog.FromForm(SandwichOrder.BuildForm, options: FormOptions.PromptInStart));
        }
    }
}