using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    class Airplane:BaseVehicle
    {
        public int Propellers { get; set; }
        public Airplane(string regNumber, string brand, string color, int weight, int propellers) : base(regNumber, brand, color, weight)
        {
            if (!ValidatePropellers(propellers))
            {
                isValid = false;                
            }
            this.Propellers = propellers;
        }

        private bool ValidatePropellers(int propellers) // 0-1000000 propellers
        {
            return propellers >= 0 && propellers < 1000000;
        }

        public override void Save(StreamWriter writer) 
        {            
            base.Save(writer);
            writer.Write(Propellers+"\n");
            
            
        }
    }
}
