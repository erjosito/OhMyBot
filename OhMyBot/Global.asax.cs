using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;
using OhMyBot.Dialogs.Help;

namespace OhMyBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // register our scorables
            var builder = new ContainerBuilder();
            builder.RegisterType<ScorableHelp>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();
            builder.Update(Conversation.Container);
        }
    }
}
