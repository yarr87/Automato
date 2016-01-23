using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Stores
{
    public abstract class BaseStore<T> where T : ICopyable<T>
    {
        protected virtual Func<T, string> SortExpr
        {
            get
            {
                return null;
            }
        }

        public virtual T GetById(string id)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                return session.Load<T>(id);
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

        public virtual void Save(T entity)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                // Client sends empty string for new devices (null breaks stuff), but we need to send null to the db
                // to generate the correct id
                if (string.IsNullOrWhiteSpace(entity.Id))
                {
                    entity.Id = null;

                    OnBeforeSave(entity);

                    session.Store(entity);
                }
                else
                {
                    // Don't overwrite existing stats on save
                    var existing = session.Load<T>(entity.Id);

                    if (existing != null)
                    {
                        entity.CopyTo(existing);
                    }
                    else
                    {
                        throw new Exception(string.Format("Saving with id {0} but that id wasn't found in the database", entity.Id));
                    }

                    OnBeforeSave(existing);
                }

                session.SaveChanges();
            }
        }

        public virtual void OnBeforeSave(T entity) { }

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
