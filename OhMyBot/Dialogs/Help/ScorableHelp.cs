using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using OhMyBot.Dialogs;
using OhMyBot.Dialogs.Help;

namespace OhMyBot.Dialogs.Help
{
#pragma warning disable 1998

    public class ScorableHelp : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogStack stack;

        public ScorableHelp(IDialogStack stack)
        {
            SetField.NotNull(out this.stack, nameof(stack), stack);
        }

        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item.AsMessageActivity();
            if (message == null)
                return null;

            var messageText = message.Text;

            return messageText == "help" ? "scorable1-triggered" : null; // this value is passed to GetScore/HasScore/PostAsync and can be anything meaningful to the scoring
        }

        protected override double GetScore(IActivity item, string state)
        {
            return state != null && state == "scorable1-triggered" ? 1 : 0;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null && state == "scorable1-triggered";
        }

        // Actions to be performed
        protected override Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;
            var dialog = new ScorableHelpDialog();
            var interruption = dialog.Void(stack);
            stack.Call(interruption, null);
            return Task.CompletedTask;

            //context.Call(new EchoCounter(), this.ResumeAfterEchoCounterDialog);

        }

        // After the scoring process is completed
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

    }
}