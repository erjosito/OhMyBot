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

        /*
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            // answer is a string
            var answer = result.Answers.First().Answer;
            Activity reply = ((Activity)context.Activity).CreateReply();
            string[] qnaAnswerData = answer.Split(';');
            string title = qnaAnswerData[0];
            string description = qnaAnswerData[1];
            string url = qnaAnswerData[2];
            string imageURL = qnaAnswerData[3];
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
        */

    }
}