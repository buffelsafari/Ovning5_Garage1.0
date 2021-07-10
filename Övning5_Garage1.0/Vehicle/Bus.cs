using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    class Bus:BaseVehicle
    {
        public int Seats { get; set; }
        
        public Bus(string regNumber, string brand, string color, int weight, int seats) : base(regNumber, brand, color, weight)
        {
            if (!ValidateSeats(seats))
            {
                isValid = false;
                Debug.Write("seats not valid");
            }
            this.Seats = seats;
        }

        private bool ValidateSeats(int seats)
        {
            return seats >= 1 && seats < 1000;            
        }

        public override void Save(StreamWriter writer)
        {
            base.Save(writer);
            writer.Write(Seats + "\n");            
            
        }
    }
}
