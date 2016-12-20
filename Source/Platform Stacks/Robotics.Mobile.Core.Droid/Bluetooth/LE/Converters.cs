namespace Robotics.Mobile.Core.Bluetooth.LE
{
    using System;

    using Android.Bluetooth;

    public static class Converters
    {
        public static GattCommunicationStatus ToGattCommunicationStatus(GattStatus status)
        {
            switch (status)
            {
                case GattStatus.ConnectionCongested:
                    return GattCommunicationStatus.ConnectionCongested;
                case GattStatus.Failure:
                    return GattCommunicationStatus.Failure;
                case GattStatus.InsufficientAuthentication:
                    return GattCommunicationStatus.InsufficientAuthentication;
                case GattStatus.InsufficientEncryption:
                    return GattCommunicationStatus.InsufficientEncryption;
                case GattStatus.InvalidAttributeLength:
                    return GattCommunicationStatus.InvalidAttributeLength;
                case GattStatus.InvalidOffset:
                    return GattCommunicationStatus.InvalidOffset;
                case GattStatus.ReadNotPermitted:
                    return GattCommunicationStatus.ReadNotPermitted;
                case GattStatus.RequestNotSupported:
                    return GattCommunicationStatus.RequestNotSupported;
                case GattStatus.Success:
                    return GattCommunicationStatus.Success;
                case GattStatus.WriteNotPermitted:
                    return GattCommunicationStatus.WriteNotPermitted;
                default:
                    throw new ArgumentException("Unsupported Gatt Status");
            }
        }
    }
}