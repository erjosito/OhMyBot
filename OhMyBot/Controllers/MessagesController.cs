using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

#pragma warning disable 1998


namespace OhMyBot
{

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// Returns a task object, which represents the task that is responsible for sending replies to the passed-in message
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.Type == ActivityTypes.Message)
            {
                // Conversation.SendAsync does the following:
                // 1. Instantiates the required components
                // 2. Deserializes the conversation state (the dialog stack and the state of each dialog in the stack) from IBotDataStore
                // 3. Resumes the conversation process where the bot suspended and waits for a message
                // 4. Sends the replies
                // 5. Serializes the updated conversation state and saves it back to IBotDataStore

                // Root dialog
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());

                // Echo dialog
                //await Conversation.SendAsync(activity, () => new EchoDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            //return Request.CreateResponse(HttpStatusCode.OK);
            //return response;
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }



        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels

                // Welcome message, using user ID to prevent duplicate messages
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    Activity reply = message.CreateReply("I am your virtual assistant, How can I help you today? ");
                    connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}