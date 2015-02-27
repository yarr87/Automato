using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    public class Device
    {
        /// <summary>
        /// Auto-gen id
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// Protocol-specific id (ie, zwave node id)
        /// </summary>
        public string NodeId { get; set; }

        public string Name { get; set; }

        public DeviceType Type { get; set; }

        public List<DeviceTag> Tags { get; set; }

        /// <summary>
        /// Not in the database, loaded directly from device
        /// </summary>
        [NotMapped]
        public string State { get; set; }

        public void CopyFrom(Device component)
        {
            this.NodeId = component.NodeId;
            this.Name = component.Name;
            this.Type = component.Type;
        }
    }
}
