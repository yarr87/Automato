using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Automato.Model
{
    public class Tag
    {
        public Int64 Id { get; set; }

        public string Name { get; set; }

        public Int64? ParentId { get; set; }

        [IgnoreDataMember]
        public List<Device> Devices { get; set; }

        public void CopyFrom(Tag tag)
        {
            this.Name = tag.Name;
            this.ParentId = tag.ParentId;
        }
    }
}
