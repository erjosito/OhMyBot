using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;
using OhMyBot.Dialogs.Help;
using OhMyBot.Dialogs.whoami;

namespace OhMyBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            Conversation.UpdateContainer(mybuilder =>
                {
                    mybuilder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));
                    var store = new TableBotDataStore(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
                    mybuilder.Register(c => store)
                       .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                       .AsSelf()
                       .SingleInstance();
                });

            GlobalConfiguration.Configure(WebApiConfig.Register);

            // register our scorables
            var builder = new ContainerBuilder();
            builder.RegisterType<ScorableHelp>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ScorableWhoami>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            builder.Update(Conversation.Container);
        }


    }
}
