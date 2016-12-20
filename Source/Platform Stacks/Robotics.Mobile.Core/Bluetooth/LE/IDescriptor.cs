namespace Robotics.Mobile.Core.Bluetooth.LE
{
    using System;

    public interface IDescriptor
    {
        object NativeDescriptor { get; }
        Guid ID { get; }
        string Name { get; }
    }
}

