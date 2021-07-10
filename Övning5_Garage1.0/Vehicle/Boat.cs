using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    class Boat:BaseVehicle
    {
        public float Length { get; set; }

        public Boat(string regNumber, string brand, string color, int weight, float length) : base(regNumber, brand, color, weight)
        {
            if (!ValidateLength(length))
            {
                isValid = false;
                Debug.Write("length not valid");
            }
            this.Length = length;
        }

        private bool ValidateLength(float length)
        {
            return length >= 1 && length < 100;            
        }

        public override void Save(StreamWriter writer)
        {
            base.Save(writer);
            writer.Write(Length + "\n");
            
            
        }
    }
}
