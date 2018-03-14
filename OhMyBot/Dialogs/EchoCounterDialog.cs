﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace OhMyBot.Dialogs
{

    #pragma warning disable 1998

    // Serializable requires using System;
    [Serializable]
    public class EchoCounterDialog : IDialog<int>
    {
        private int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            await this.SendWelcomeMessageAsync(context);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            /* When MessageReceivedAsync is called, it's passed an IAwaitable<IMessageActivity>. To get the message,
                   *  await the result. */
            var message = await argument;
            await this.SendWelcomeMessageAsync(context);
        }
        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            await context.PostAsync("Hi, you are now in the Echo Counter Dialog. Let's get started.");
            context.Wait(CounterLoop);
        }

        private async Task CounterLoop(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;

            if (message.Text == "exit")
            {
                context.Done(count);
            }
            else if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.None);
            }
            else
            {
                await context.PostAsync($"{this.count++}: You said {message.Text}");
                context.Wait(CounterLoop);
            }
        }

        private async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(CounterLoop);
        }
    }

}