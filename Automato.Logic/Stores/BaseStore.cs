using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Stores
{
    public abstract class BaseStore<T>
    {
        public virtual IEnumerable<T> GetAll()
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                var query = session.Query<T>();

                return query.ToList();
            }
        }

        public virtual void DeleteById(string id)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
        }
    }
}
