namespace Robotics.Mobile.Core.Bluetooth.LE
{
    using System;
    using System.Collections.Generic;

    public interface IService
    {
        event EventHandler CharacteristicsDiscovered;

        Guid ID { get; }
        String Name { get; }
        bool IsPrimary { get; } // iOS only?
        IList<ICharacteristic> Characteristics { get; }


        ICharacteristic FindCharacteristic(KnownCharacteristic characteristic);
        //IDictionary<Guid, ICharacteristic> Characteristics { get; }
        void DiscoverCharacteristics();

    }
}