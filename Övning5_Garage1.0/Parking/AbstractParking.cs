using Garage10.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Garage10.Parking
{
    abstract class AbstractParking:IParkingLot,ISavable
    {
        protected AbstractParking PreviousLink { get; set; }        
                       

        internal AbstractParking()
        {
            
        }
         
        public IParkingLot Remove(Type type, out bool result)
        {            
            if (this.GetType().Equals(type))
            {
                result = true;
                return PreviousLink;
            }

            if (PreviousLink != null)
            {
                PreviousLink = (AbstractParking)PreviousLink.Remove(type, out result);
            }

            result = false;
            return this;
        }

        private bool HasType(Type type)
        {                   
            if ((this.GetType()).Equals(type))
            {                
                return true;
            }
            
            if (PreviousLink == null)
            {                
                return false;                
            } 
            return PreviousLink.HasType(type);
        }
        

        public virtual string GetDescription()
        {
            return new string(PreviousLink?.GetDescription());
        }
        

        public IParkingLot Add(AbstractParking b, out bool result)
        {
            if (HasType(b.GetType()))
            {
                result = false;                
                return this;
            }

            result = true;
            b.PreviousLink = this;
            return b;
        }

        public void Save(StreamWriter writer)
        {
            writer.Write(this.GetType().Name);
            if (PreviousLink != null)
            {
                writer.Write(",");
                PreviousLink?.Save(writer);
            }            
        }
    }
}
