using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    class Motorcycle:BaseVehicle
    {
        public int Speed { get; set; }
        public Motorcycle(string regNumber,string brand, string color, int weight, int speed) : base(regNumber,brand, color, weight)
        {
            if (!ValidateSpeed(speed))
            {
                isValid = false;
                Debug.Write("speed is not valid");
            }
            this.Speed = speed;
        }

        private bool ValidateSpeed(int speed) // 0-500 km/h
        {
            return speed >= 0 && speed < 500;
        }

        public override void Save(StreamWriter writer)
        {
            base.Save(writer);
            writer.Write(Speed + "\n");            
            
        }
    }
}
