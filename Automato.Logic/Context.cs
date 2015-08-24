using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic
{
    public class Context
    {
        public static Lazy<DocumentStore> DocumentStore = new Lazy<DocumentStore>(() =>
        {
            var documentStore = new DocumentStore()
            {
                Url = ConfigurationManager.AppSettings["Raven.Server"],
                DefaultDatabase = ConfigurationManager.AppSettings["Raven.DatabaseName"]
            };

            // Don't use slash in ids, since it messes up urls
            documentStore.Conventions.IdentityPartsSeparator = ConfigurationManager.AppSettings["Raven.KeySeparator"];

            documentStore.Initialize();

            return documentStore;
        });
    }
}
