using OpenZWaveDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave;
using ZWave.CommandClasses;

namespace Automato.Config
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting up...");

            //SetRemoteConfig().Wait();
            openzwave();

            Console.ReadLine();
        }

        private static ZWManager mgr;
        private static uint homeId;

        public static void openzwave()
        {
            try
            {
                var options = new ZWOptions();
                options.Create(@".\config\", @"", @"");

                // Add any app specific options here...
                //options.AddOptionInt("SaveLogLevel", (int)ZWLogLevel.Detail);
                // ordinarily, just write "Detail" level messages to the log
                //options.AddOptionInt("QueueLogLevel", (int)ZWLogLevel.Debug);
                // save recent messages with "Debug" level messages to be dumped if an error occurs
                //options.AddOptionInt("DumpTriggerLevel", (int)ZWLogLevel.Error);
                // only "dump" Debug  to the log emessages when an error-level message is logged

                // Lock the options
                options.Lock();

                mgr = new ZWManager();
                mgr.Create();

                Console.WriteLine("Created manager");
                //m_manager.OnNotification += new ManagedNotificationsHandler(NotificationHandler);

                // Add a driver
                var m_driverPort = @"\\.\COM3";
                mgr.AddDriver(m_driverPort);

                Console.WriteLine("Added driver");

                mgr.OnNotification += new ManagedNotificationsHandler(NotificationHandler);


                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ");
                Console.WriteLine(ex.Message);
            }
        }

        public static void NotificationHandler(ZWNotification notification)
        {
            if (notification.GetType() == ZWNotification.Type.DriverReady)
            {
                homeId = notification.GetHomeId();

                Console.WriteLine("Got home id " + homeId);


                

                //Console.ReadLine();
            }
            else
            {
                var node = notification.GetNodeId();

                if (node == 16)
                {
                    var v = notification.GetValueID();

                    Console.WriteLine(string.Format("Got notification of type {0}", notification.GetType()));
                    Console.WriteLine("  Node : " + v.GetNodeId().ToString());
                    Console.WriteLine("  CC   : " + v.GetCommandClassId().ToString());
                    Console.WriteLine("  Type : " + v.GetType().ToString());
                    Console.WriteLine("  Index: " + v.GetIndex().ToString());
                    Console.WriteLine("  Inst : " + v.GetInstance().ToString());
                    Console.WriteLine("  Value: " + GetValue(v).ToString());
                    Console.WriteLine("  Label: " + mgr.GetValueLabel(v));
                    Console.WriteLine("  Help : " + mgr.GetValueHelp(v));
                    Console.WriteLine("  Units: " + mgr.GetValueUnits(v));
                    Console.WriteLine();

                    // *** THIS WORKS!!! No idea why!
                    if (notification.GetType() == ZWNotification.Type.NodeQueriesComplete)
                    {
                        Console.WriteLine("************************************  Setting config value");

                        var val = mgr.SetConfigParam(homeId, 16, 250, 1);

                        Console.WriteLine("Set config param: " + val);

                    }
                }
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        private static string GetValue(ZWValueID v)
        {
            switch (v.GetType())
            {
                case ZWValueID.ValueType.Bool:
                    bool r1;
                    mgr.GetValueAsBool(v, out r1);
                    return r1.ToString();
                case ZWValueID.ValueType.Byte:
                    byte r2;
                    mgr.GetValueAsByte(v, out r2);
                    return r2.ToString();
                case ZWValueID.ValueType.Decimal:
                    decimal r3;
                    mgr.GetValueAsDecimal(v, out r3);
                    return r3.ToString();
                case ZWValueID.ValueType.Int:
                    Int32 r4;
                    mgr.GetValueAsInt(v, out r4);
                    return r4.ToString();
                case ZWValueID.ValueType.List:
                    string[] r5;
                    mgr.GetValueListItems(v, out r5);
                    string r6 = "";
                    foreach (string s in r5)
                    {
                        r6 += s;
                        r6 += "/";
                    }
                    return r6;
                case ZWValueID.ValueType.Schedule:
                    return "Schedule";
                case ZWValueID.ValueType.Short:
                    short r7;
                    mgr.GetValueAsShort(v, out r7);
                    return r7.ToString();
                case ZWValueID.ValueType.String:
                    string r8;
                    mgr.GetValueAsString(v, out r8);
                    return r8;
                default:
                    return "";
            }
        }

        public static async Task SetRemoteConfig()
        {
            try
            {
                // the nodeID of the minimote
                byte minimoteId = 16;

                // create the controller
                var controller = new ZWaveController("COM3");

                // open the controller
                controller.Open();

                Console.WriteLine("Connection opened");

                // get the included nodes
                var nodes = await controller.GetNodes();

                // get the motionSensor
                var minimote = nodes[minimoteId];

                // get the SensorAlarm commandclass
                var config = minimote.GetCommandClass<Configuration>();

                Console.WriteLine("Setting value");

                //var val = await config.Get(250);

                //Console.WriteLine("Current value: " + val);
                
                await config.Set(244, 1);

                Console.WriteLine("Config set");

                // wait
                Console.ReadLine();

                // close the controller
                controller.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
