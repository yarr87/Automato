using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Stores
{
    public abstract class BaseStore<T>
    {
        protected virtual Func<T, string> SortExpr
        {
            get
            {
                return null;
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            // TODO: store the list statically, and return from memory if it's there.  No need to go to the db each time since there's only
            // one instance.   ...is that true?  Need to make sure that won't break the job either though.
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                var query = session.Query<T>();

                var data = query.ToList();

                if (SortExpr != null)
                {
                    // TODO: do the sort in the database
                    // I think you need to set up indexes or something in Raven.  Not expecting a large dat load here, so in memory isn't a huge deal.
                    return data.OrderBy(SortExpr);
                }
                else
                {
                    return data;
                }
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
