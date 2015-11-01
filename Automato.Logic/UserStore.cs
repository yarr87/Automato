using Automato.Integration;
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
    public class UserStore
    {
        public IEnumerable<User> GetUsers()
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                var users = session.Query<User>();
                
                return users.ToList();
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
                if (user.Id == string.Empty)
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
                    }
                    else
                    {
                        throw new Exception(string.Format("Saving user with id {0} but that id wasn't found in the database", user.Id));
                    }
                }

                session.SaveChanges();
            }
        }

        public void DeleteById(string id)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
        }
    }
}
