using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Parking
{
    class BasicParking:AbstractParking
    {
        

        public override string GetDescription()
        {
            return new string(":bas"+PreviousLink?.GetDescription());
        }

    }
}
