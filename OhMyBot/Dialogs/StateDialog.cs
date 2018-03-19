namespace OhMyBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class StateDialog : IDialog<object>
    {
        private const string HelpMessage = "\n * If you want to see the current 'current city'. \n * Want to change the current city? Type 'change city to cityName'. \n * Want to change it just for your searches? Type 'change my city to cityName'";
        private bool userWelcomed;
        private const string CityKey = "City";

        public async Task StartAsync(IDialogContext context)
        {
            string defaultCity;

            if (!context.ConversationData.TryGetValue(CityKey, out defaultCity))
            {
                defaultCity = "Seattle";
                context.ConversationData.SetValue(CityKey, defaultCity);
            }

            await context.PostAsync($"Welcome to the state dialog. Your current city is {defaultCity}");
            await context.PostAsync($"These are the rules: {HelpMessage}");
            this.userWelcomed = true;

            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!this.userWelcomed)
            {
                this.userWelcomed = true;
                await context.PostAsync($"Welcome back! Remember the rules: {HelpMessage}");

                context.Wait(this.MessageReceivedAsync);
                return;
            }

            if (message.Text.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync($"Thanks for using the city bot, see you around!");
                context.Done<object>(null);
            }
            else if (message.Text.Equals("current city", StringComparison.InvariantCultureIgnoreCase))
            {
                string userCity;

                var city = context.ConversationData.GetValue<string>(CityKey);

                if (context.PrivateConversationData.TryGetValue(CityKey, out userCity))
                {
                    await context.PostAsync($"You have overridden the city. Your current city is now {userCity}. The default conversation city is {city}.");
                }
                else
                {
                    await context.PostAsync($"Hey, I'm currently configured with city {city}.");
                }
            }
            else if (message.Text.StartsWith("change city to", StringComparison.InvariantCultureIgnoreCase))
            {
                var newCity = message.Text.Substring("change city to".Length).Trim();
                context.ConversationData.SetValue(CityKey, newCity);

                await context.PostAsync($"All set. From now on, your city is {newCity}.");
            }
            else if (message.Text.StartsWith("change my city to", StringComparison.InvariantCultureIgnoreCase))
            {
                var newCity = message.Text.Substring("change my city to".Length).Trim();
                context.PrivateConversationData.SetValue(CityKey, newCity);

                await context.PostAsync($"All set. I have overridden the city to {newCity} just for you.");
            }
            else
            {
                string city;

                if (!context.PrivateConversationData.TryGetValue(CityKey, out city))
                {
                    city = context.ConversationData.GetValue<string>(CityKey);
                }

                await context.PostAsync($"Wait a few seconds. Searching for '{message.Text}' in '{city}'...");
                await context.PostAsync($"https://www.bing.com/search?q={HttpUtility.UrlEncode(message.Text)}+in+{HttpUtility.UrlEncode(city)}");
            }

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterPrompt(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var userName = await result;
                this.userWelcomed = true;

                await context.PostAsync($"Welcome! {HelpMessage}");

                //context.UserData.SetValue(ContextConstants.UserNameKey, userName);
            }
            catch (TooManyAttemptsException)
            {
            }

            context.Wait(this.MessageReceivedAsync);
        }
    }
}