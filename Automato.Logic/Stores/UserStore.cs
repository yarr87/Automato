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

        public void AddOrEditUser(User user)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                // Client sends empty string for new devices (null breaks stuff), but we need to send null to the db
                // to generate the correct id
                if (string.IsNullOrWhiteSpace(user.Id))
                {
                    user.Id = null;

                    session.Store(user);
                }
                else
                {
                    // Don't overwrite existing presence on save
                    var existingUser = session.Load<User>(user.Id);

                    if (existingUser != null)
                    {
                        existingUser.Name = user.Name;
                        existingUser.DeviceMac = user.DeviceMac;
                        existingUser.Email = user.Email;
                        existingUser.TextAddress = user.TextAddress;
                    }
                    else
                    {
                        throw new Exception(string.Format("Saving user with id {0} but that id wasn't found in the database", user.Id));
                    }
                }

                session.SaveChanges();
            }
        }        
    }
}
