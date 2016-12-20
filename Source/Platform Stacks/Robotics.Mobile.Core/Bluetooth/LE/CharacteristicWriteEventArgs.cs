namespace Robotics.Mobile.Core.Bluetooth.LE
{
    using System;

    public class CharacteristicWriteEventArgs : EventArgs
    {
        public ICharacteristic Characteristic { get; set; }

        public GattCommunicationStatus Status { get; set; }

        public CharacteristicWriteEventArgs()
        {
        }
    }
}

