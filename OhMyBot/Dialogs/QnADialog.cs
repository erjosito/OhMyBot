using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;


namespace OhMyBot.Dialogs
{
    #pragma warning disable 1998

        [Serializable]
    public class QnADialog : QnAMakerDialog
    {

        public QnADialog() : base(new QnAMakerService(new QnAMakerAttribute(ConfigurationManager.AppSettings["QnaSubscriptionKey"], ConfigurationManager.AppSettings["QnaKnowledgebaseId"], "Sorry, I couldn't find an answer for that", 0.5)))
        {
        }

        
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            // answer is a string
            var answer = result.Answers.First().Answer;
            Activity reply = ((Activity)context.Activity).CreateReply();
            string[] qnaAnswerData = answer.Split(';');
            string title = "";
            string description = "";
            string url = "";
            string imageURL = "";

            if (qnaAnswerData.Length == 4) {
                title = qnaAnswerData[0];
                description = qnaAnswerData[1];
                url = qnaAnswerData[2];
                imageURL = qnaAnswerData[3];
            }
            else
            {
                description = qnaAnswerData[0];
                title = "Azure VMs";
                url = "http://docs.microsoft.com";
                imageURL = "https://i0.wp.com/www.aidanfinn.com/wp-content/uploads/2016/08/Capture-620x264.png";
            }
            HeroCard card = new HeroCard
                {
                    Title = title,
                    Subtitle = description,
                };
                card.Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Learn More", value: url)
                };
                card.Images = new List<CardImage>
                {
                    new CardImage( url = imageURL)
                };
            reply.Attachments.Add(card.ToAttachment());
            await context.PostAsync(reply);
        }

        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            context.Done<IMessageActivity>(null);
        }
    }
}