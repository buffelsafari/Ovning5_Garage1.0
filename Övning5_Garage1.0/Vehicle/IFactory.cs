using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    interface IFactory
    {
        public Tuple<string, Type>[] GetParameterInfo(string name);
        public IVehicle CreateVehicle(string type, Object[] para);
    }
}
