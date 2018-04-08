
namespace OhMyBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using System.Net.Http;
    using System.Threading;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using BotAuth.AADv2;
    using BotAuth.Dialogs;
    using BotAuth.Models;
    using BotAuth;

    [Serializable]
    public class SigninCardDialog : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Welcome to the sign-in dialog");
            context.Wait(this.MessageReceivedAsync);

        }
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            if (activity.Text.ToLower().Contains("card"))
            {
                var attachment = GetSigninCard();
                var message = context.MakeMessage();
                message.Attachments.Add(attachment);
                await context.PostAsync(message);
                // Go back to loop
                context.Wait(this.MessageReceivedAsync);
            }
            else if (activity.Text.ToLower().Contains("sign"))
            {
                /*
                // Initialize AuthenticationOptions and forward to AuthDialog for token
                AuthenticationOptions options = new AuthenticationOptions()
                {
                    Authority = ConfigurationManager.AppSettings["aad:Authority"],
                    ClientId = ConfigurationManager.AppSettings["aad:ClientId"],
                    ClientSecret = ConfigurationManager.AppSettings["aad:ClientSecret"],
                    //Scopes = new string[] { "https://management.azure.com/" },
                    Scopes = new string[] { "User.Read" },
                    RedirectUrl = ConfigurationManager.AppSettings["aad:Callback"],
                    MagicNumberView = "/magic.html#{0}"
                };

                await context.Forward(new AuthDialog(new MSALAuthProvider(), options), async (IDialogContext authContext, IAwaitable<AuthResult> authResult) =>
                {
                    var authresult = await authResult;
                    // Use token to call into service
                    var json = await new HttpClient().GetWithAuthAsync(authresult.AccessToken, "https://graph.microsoft.com/v1.0/me");
                    await authContext.PostAsync($"I'm a simple bot that doesn't do much, but I know your name is {json.Value<string>("displayName")} and your UPN is {json.Value<string>("userPrincipalName")}");
                }, activity, CancellationToken.None);
                */
                // Go back to loop
                await context.PostAsync("Sign in does not work properly just yet");
                context.Wait(this.MessageReceivedAsync);

            }

            else if (activity.Text.ToLower().Contains("exit"))
            {
                context.Done<IMessageActivity>(null);
            }
            else
            {
                await context.PostAsync($"I cannot do much just yet, please type card, signin or exit");
                // Go back to loop
                context.Wait(this.MessageReceivedAsync);
            }

        }

        private static Attachment GetSigninCard()
        {
            var signinCard = new SigninCard
            {
                Text = "BotFramework Sign-in Card",
                Buttons = new List<CardAction> { new CardAction(ActionTypes.Signin, "Sign-in", value: "https://login.microsoftonline.com/") }
            };

            return signinCard.ToAttachment();
        }
    }
}