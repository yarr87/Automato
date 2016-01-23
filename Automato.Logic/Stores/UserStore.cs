using Automato.Integration;
using Automato.Logic.Stores;
using Automato.Model;
using Automato.Model.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Automato.Logic
{
    public class UserStore : BaseStore<User>
    {
        protected override Func<User, string> SortExpr
        {
            get
            {
                return (u) => u.Name;
            }
        }

        public void UpdateUserPresence(IEnumerable<UserPresenceUpdate> updates)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                foreach (var update in updates)
                {
                    var user = session.Query<User>().FirstOrDefault(u => u.DeviceMac == update.DeviceMac);

                    if (user != null)
                    {
                        user.IsHome = update.IsHome;
                    }
                }

                session.SaveChanges();
            }            
        }
    }
}
