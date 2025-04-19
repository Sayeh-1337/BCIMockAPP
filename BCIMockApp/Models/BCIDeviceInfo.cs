using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCIMockApp.Models
{
    public class BCIDeviceInfo
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public bool IsConnected { get; set; }
        public int BatteryLevel { get; set; }
        public string SignalQuality { get; set; }
    }
}
