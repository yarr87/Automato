using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    public class Constants
    {
        /// <summary>
        /// Special-case user ids for rules
        /// </summary>
        public struct UserIds
        {
            /// <summary>
            /// Rule applies to any user
            /// </summary>
            public const string Anyone = "ANYONE";
            public const string NoOne = "NOONE";
        }
    }
}
