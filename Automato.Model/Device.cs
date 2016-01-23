using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    public class Device : ICopyable<Device>
    {
        /// <summary>
        /// Auto-gen id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Protocol-specific id (ie, zwave node id for config file)
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Friendly name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name used to send commands (no spaces, used in config file)
        /// </summary>
        public string InternalName { get; set; }

        public DeviceType Type { get; set; }

        public List<DeviceTag> Tags { get; set; }

        /// <summary>
        /// Time this device last updated (light turned on/off/etc)
        /// </summary>
        public DateTime? LastStateChange { get; set; }

        /// <summary>
        /// Not in the database, loaded directly from device
        /// </summary>
        [NotMapped]
        public string State { get; set; }

        //public void CopyFrom(Device device)
        //{
        //    this.NodeId = device.NodeId;
        //    this.Name = device.Name;
        //    this.InternalName = device.InternalName;
        //    this.Type = device.Type;
        //    this.Tags = device.Tags;
        //}

        public void CopyTo(Device destination)
        {
            throw new NotImplementedException();
        }
    }
}
