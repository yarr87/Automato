using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automato.Model
{
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        /// <summary>
        /// Used to indicate a tag is not a direct member of a device.  Not in the database, and no meaning
        /// unless this tag is in a device's tag list.
        /// </summary>
        [NotMapped]
        public bool? IsIndirect { get; set; }

        [IgnoreDataMember]
        public List<Device> Devices { get; set; }

        public void CopyFrom(Tag tag)
        {
            this.Name = tag.Name;
            this.ParentId = tag.ParentId;
        }
    }
}
