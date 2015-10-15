using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// Representation of a tag within a device
    /// </summary>
    public class DeviceTag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        /// <summary>
        /// True if this tag is directly applied to this device, false if inherited via the tag hierarchy
        /// </summary>
        public bool IsDirect { get; set; }
    }

    public class DeviceTagComparer : IEqualityComparer<DeviceTag>
    {
        public bool Equals(DeviceTag x, DeviceTag y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(DeviceTag obj)
        {
            return obj.Id.GetHashCode();
        }
    }

}
