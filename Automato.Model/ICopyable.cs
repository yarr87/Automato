using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// An object can copy its properties to another instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopyable<T> : IEntity
    {
        void CopyTo(T destination);
    }
}
