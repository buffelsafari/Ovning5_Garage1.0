using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Parking
{
    internal class Lights:AbstractParking
    {       

        public override string GetDescription()
        {
            return new string(":lgt" + PreviousLink?.GetDescription());
        }
    }
}
