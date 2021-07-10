using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Vehicle
{
    abstract class BaseVehicle : IVehicle
    {
        protected bool isValid;

        public string RegNumber { get; private set; }
        public string Brand { get; private set; }
        public string Color { get; private set; }
        public int Weight { get; private set; }
        public int Parking { get; set; }


        internal BaseVehicle(string regNumber, string brand, string color, int weight)
        {
            isValid = true;
            regNumber = regNumber.ToUpper();
            if (!ValidateRegNumber(regNumber))
            {
                isValid = false;
                Debug.Write("registration number not valid");
            }
            this.RegNumber = regNumber;

            if (!ValidateBrand(brand))
            {
                isValid = false;
                Debug.Write("brand name not valid");
            }
            this.Brand = brand;

            if (!ValidateColor(color))
            {
                isValid = false;
                Debug.Write("color name not valid");
            }
            this.Color = color;

            if (!ValidateWeight(weight))
            {
                isValid = false;
                Debug.Write("weight not valid");
            }
            this.Weight = weight;
        }

        public bool IsValid()  // returns true if this was created with correct parameters
        {
            return isValid;
        }

        private bool ValidateWeight(int weight)
        {
            return weight > 0 && weight < 1000000;
        }

        private bool ValidateColor(string color) //min 2 and all of them letters
        {
            return (color.Length >= 2) && (color.Where(a => Char.IsLetter(a)).Count() == color.Length);

        }

        private bool ValidateBrand(string brand) // min 2 and atleast 1 letter
        {
            return (brand.Length >= 2) && (brand.Where(a => Char.IsLetter(a)).Count() >= 1);
        }

        private bool ValidateRegNumber(string number) // max 6 atleast 1 number and 1 letter
        {
            return number.Length <= 6 && number.Where(a => Char.IsNumber(a)).Count() >= 1 && number.Where(a => Char.IsLetter(a)).Count() >= 1;
        }

        public override bool Equals(Object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.RegNumber.GetHashCode();
        }

        public virtual void Save(StreamWriter writer)
        {
            writer.Write(Parking+",");

            writer.Write(this.GetType().Name + ",");
            writer.Write(RegNumber + ",");
            writer.Write(Brand + ",");
            writer.Write(Color + ",");
            writer.Write(Weight + ",");
        }
        
        
    }
}
