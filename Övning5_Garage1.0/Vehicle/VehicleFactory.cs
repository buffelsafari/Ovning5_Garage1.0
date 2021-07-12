using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    class VehicleFactory:IFactory
    {


        public Tuple<string, Type>[] GetParameterInfo(string name)
        {
            // test if name is a valid Vehicle class
            bool error=false;  
            Type type = Type.GetType($"Garage10.Vehicle.{name}", error); //todo get assemby string             

            if (!error)
            {
                if (type == null || !type.IsAssignableTo(typeof(BaseVehicle)))
                {
                    return null;
                }

                // get the constructor parameters from the first constructor                
                              
                return type.GetConstructors()[0].GetParameters().Select((a) => new Tuple<string, Type>(a.Name, a.ParameterType)).ToArray();
               
            }
            return null;
        }

        public IVehicle CreateVehicle(string type, Object[] para)
        {

            Object c = Activator.CreateInstance(Type.GetType($"Garage10.Vehicle.{type}"), para);
            return (BaseVehicle)c;

        }
    }
}
