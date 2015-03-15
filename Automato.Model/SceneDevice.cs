using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// Device state within a scene
    /// </summary>
    public class SceneDevice
    {
        public Int64 SceneId { get; set; }

        public Int64 DeviceId { get; set; }

        public string State { get; set; }

        [NotMapped]
        public string Name { get; set; }

        [NotMapped]
        public string InternalName { get; set; }

        [NotMapped]
        public DeviceType Type { get; set; }
    }
}
