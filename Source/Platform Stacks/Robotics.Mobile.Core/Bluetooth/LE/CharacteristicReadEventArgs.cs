namespace Robotics.Mobile.Core.Bluetooth.LE
{
    using System;

    public class CharacteristicReadEventArgs : EventArgs
    {
        public ICharacteristic Characteristic { get; set; }

        public CharacteristicReadEventArgs()
        {
        }
    }
}

