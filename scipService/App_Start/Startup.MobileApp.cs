using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using scipService.DataObjects;
using scipService.Models;
using Owin;

namespace scipService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()                
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new scipInitializer());
           // Database.SetInitializer<scipContext>(new scipReInitializer());



            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<scipContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            //add ***************
            config.MessageHandlers.Add(new AuthenticationHandler());

            app.UseWebApi(config);
        }
    }

    public class scipInitializer : CreateDatabaseIfNotExists<scipContext>
    {
        protected override void Seed(scipContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            List<User> listUser = new List<User>
            {
                new User { Id=Guid.NewGuid().ToString(), UserID=0,NickName="admin",Password="admin",Date=2016 },
                new User { Id=Guid.NewGuid().ToString(), UserID=1,NickName="thanh",Password="thanh",Date=1996,Sitter= new List<uint> { 0}, Avatar="https://i.ytimg.com/vi/f24xRCjJbIc/hqdefault.jpg" },
                new User { Id=Guid.NewGuid().ToString(), UserID=2,NickName="thai",Password="thai",Date=1997,Sitter= new List<uint> { 0,1} },
                new User { Id=Guid.NewGuid().ToString(), UserID=3,NickName="linh",Password="linh",Date=1997,Sitter= new List<uint> { 0,1,2} },
            };

            foreach (User user in listUser)
            {
                context.Set<User>().Add(user);
            }

            base.Seed(context);
        }
    }

    public class scipReInitializer : DropCreateDatabaseIfModelChanges<scipContext>
    {
        protected override void Seed(scipContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            List<User> listUser = new List<User>
            {
                new User { UserID=0,NickName="admin",Password="admin",Date=2016 },
                new User { UserID=1,NickName="thanh",Password="thanh",Date=1996,Sitter= new List<uint> { 0}, Avatar="https://i.ytimg.com/vi/f24xRCjJbIc/hqdefault.jpg" },
                new User {UserID=2,NickName="thai",Password="thai",Date=1997,Sitter= new List<uint> { 0,1} },
                new User {UserID=3,NickName="linh",Password="linh",Date=1997,Sitter= new List<uint> { 0,1,2} },
            };

            foreach (User user in listUser)
            {
                context.Set<User>().Add(user);
            }

            base.Seed(context);
        }

    }
}

