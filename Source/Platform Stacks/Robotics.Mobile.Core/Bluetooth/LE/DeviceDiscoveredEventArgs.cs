namespace Robotics.Mobile.Core.Bluetooth.LE
{
    using System;

    public class DeviceDiscoveredEventArgs : EventArgs
    {
        public IDevice Device;

        public DeviceDiscoveredEventArgs()
            : base()
        {
            
        }
    }
}

