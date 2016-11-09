using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Robotics.Mobile.Core.Bluetooth.LE
{
	public interface ICharacteristic
	{
		// events
		event EventHandler<CharacteristicReadEventArgs> ValueUpdated;

		// properties
		Guid ID { get; }
		string Uuid { get; }
		byte[] Value { get; }
		string StringValue { get; }
		IList<IDescriptor> Descriptors { get; }
		object NativeCharacteristic { get; }
		string Name { get; }
		CharacteristicPropertyType Properties { get; }

		bool CanRead { get; }
		bool CanUpdate { get; }
		bool CanWrite { get; }

		// methods
//		void EnumerateDescriptors ();

		void StartUpdates();

        /// <summary>
        /// Subscribe updates
        /// </summary>
        /// <param name="useNotification">Use Notify to subscribe | False to use Indicate instead</param>
        void StartUpdates(bool useNotify);

        void StopUpdates();

		Task<ICharacteristic> ReadAsync ();

		void Write (byte[] data);


        void Write(byte[] data, WriteType writeType);        
    }
}

