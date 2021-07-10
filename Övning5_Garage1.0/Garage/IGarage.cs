using Garage10.Parking;
using Garage10.Vehicle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Garage
{
    interface IGarage<V, P> : IEnumerable<V> where V : IVehicle where P : IParkingLot
    {

        public int Count { get; }

        public int Capacity { get; }

        public P this[int index] { get; set; }

        public event Action<object, LogMessage, MessageType> LogEvent;

        public void AddVehicleAt(V vehicle, int index);
        public void RemoveVehicleAt(int index);

        public IEnumerable<P> GetParkingLots();
        public void AddVehicle(V vehicle);
    }
}
