using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Automato.Web.Models
{
    /// <summary>
    /// Object used to post temp to thermostat endpoint
    /// </summary>
    public class TemperatureModel
    {
        public decimal Temperature { get; set; }
    }
}