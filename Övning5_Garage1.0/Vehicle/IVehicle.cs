using Garage10.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    interface IVehicle:ISavable
    {
        public int Parking { get; set; }        

        public bool IsValid();
        
    }
}
