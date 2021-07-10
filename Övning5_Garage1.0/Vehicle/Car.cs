using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    class Car:BaseVehicle
    {
        public float Engine { get; set; }

        public Car(string regNumber, string brand, string color ,int weight, float engine):base(regNumber, brand ,color, weight)
        {
            if (!ValidateEngine(engine))
            {
                isValid = false;
                Debug.Write("engine not valid");
            }
            this.Engine = engine;            
        }

        private bool ValidateEngine(float engine)
        {
            return engine >= 0 && engine < 100;            
        }

        public override void Save(StreamWriter writer)
        {
            base.Save(writer);
            writer.Write(Engine + "\n");
            
            
        }
    }
}
