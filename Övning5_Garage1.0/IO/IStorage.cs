using Garage10.Garage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.IO
{
    interface IStorage
    {
        void Save(ISavable thing);
        void Load(ILoadable thing);
    }
}
