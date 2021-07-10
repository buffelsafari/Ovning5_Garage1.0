using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Parking
{
    class Electricity:AbstractParking
    {
        public override string GetDescription()
        {
            return new string(":16A" + PreviousLink?.GetDescription());
        }
    }
}
