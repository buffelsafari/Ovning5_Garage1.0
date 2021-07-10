using Garage10.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Parking
{
    interface IParkingLot:ISavable
    {
        
        public IParkingLot Remove(Type type, out bool result);
        public IParkingLot Add(AbstractParking b, out bool result);        

        public string GetDescription();

                
    }
}
