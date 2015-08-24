using Automato.Data;
using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic
{
    public class SceneStore
    {
        public IEnumerable<Scene> GetScenes()
        {
            return Enumerable.Empty<Scene>();

            //using (var db = new TomatoContext())
            //{
            //    var scenes = (from scene in db.Scenes
            //                  let sceneDevices = from sceneDevice in db.SceneDevices
            //                                     join device in db.Devices on sceneDevice.DeviceId equals device.Id
            //                                     where sceneDevice.SceneId == scene.Id
            //                                     select new
            //                                     {
            //                                         device = device,
            //                                         sceneDevice = sceneDevice
            //                                     }
            //                  select new
            //                  {
            //                      scene = scene,
            //                      sceneDevices = sceneDevices
            //                  }).ToList();

            //    var allScenes = scenes.Select(s =>
            //    {
            //        var scene = s.scene;
            //        scene.Devices = new List<SceneDevice>();

            //        foreach (var deviceWrapper in s.sceneDevices)
            //        {
            //            var sceneDevice = deviceWrapper.sceneDevice;
            //            sceneDevice.Name = deviceWrapper.device.Name;
            //            sceneDevice.InternalName = deviceWrapper.device.InternalName;
            //            sceneDevice.Type = deviceWrapper.device.Type;

            //            scene.Devices.Add(sceneDevice);
            //        }

            //        return scene;
            //    }).ToList();

            //    return allScenes;
            //}
        }
    }
}
