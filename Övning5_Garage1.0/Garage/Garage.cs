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
    class Garage<V, P> :IGarage<V,P>  where V : IVehicle where P:IParkingLot 
    {
        private V[] vehicles;
        private P[] parkingLots;

        public event Action<object, LogMessage, MessageType> LogEvent;


        public int Count { get; private set; }
        public int Capacity { get; private set; }


        internal Garage(int size)
        {
            vehicles = new V[size];
            parkingLots = new P[size];
            Count = 0;
            Capacity = size;
        }

        public IEnumerable<P> GetParkingLots()
        {
            return parkingLots;
        }

        public P this[int index] 
        {
            get
            {                
                return parkingLots[index];
            }
            set
            {                
                parkingLots[index]= value;
            }
        }


        public void AddVehicleAt(V vehicle, int index) 
        {
            if (vehicles[index] == null)
            {
                if (!this.Contains(vehicle))
                {
                    vehicles[index] = vehicle;
                    Count++;
                    vehicle.Parking = index;
                    LogEvent?.Invoke(this, LogMessage.VEHICLE_PARKED, MessageType.SUCCESS);
                }
                else
                { 
                    LogEvent?.Invoke(this, LogMessage.VEHICLE_ALREADY_PARKED, MessageType.FAILURE);
                }
            }
            else
            {
                LogEvent?.Invoke(this, LogMessage.PARKING_OCCUPIED, MessageType.FAILURE);
            }
        }
               

        public void RemoveVehicleAt(int index)
        {
            if (vehicles[index] != null)
            {
                vehicles[index] = default(V);
                Count--;
                LogEvent?.Invoke(this, LogMessage.VEHICLE_REMOVED, MessageType.SUCCESS);
            }
            else
            {
                LogEvent?.Invoke(this, LogMessage.PARKING_ALREADY_EMPTY, MessageType.FAILURE);
            }
        }

        


        #region enumerator
        public IEnumerator<V> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void AddVehicle(V vehicle)
        {
            for(int i=0;i<vehicles.Length ;i++)
            {
                if (vehicles[i] == null)
                {
                    AddVehicleAt(vehicle, i);                    
                    return;
                }
            }
            LogEvent?.Invoke(this, LogMessage.NO_PARKING_AVAILABLE, MessageType.FAILURE);

        }

        private class Enumerator : IEnumerator<V>
        {
            private int index;            
            private Garage<V, P> garage;
            
            public Enumerator(Garage<V, P> garage)
            {
                this.garage = garage;
                Reset();
            }

            object IEnumerator.Current => garage.vehicles[index];

            V IEnumerator<V>.Current => garage.vehicles[index];

            public void Dispose()
            {
                // todo dispose something
                //throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                
                for (++index; index < garage.vehicles.Length;index++)
                {
                    if (garage.vehicles[index] != null)
                    {
                        return true;
                    }                    
                }
                return false;                
            }

            public void Reset()
            {
                index = -1;                
            }
        }
        #endregion
    }
}
