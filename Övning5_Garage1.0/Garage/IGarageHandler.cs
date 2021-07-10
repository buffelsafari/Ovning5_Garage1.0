using Garage10.IO;
using Garage10.Vehicle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.Garage
{
    interface IGarageHandler:ISavable,ILoadable
    {
        int Capacity { get; }
        int Count { get; }

        event Action<object, LogMessage, MessageType> LogEvent;
        void FilterCommand(Stream stream, string line);

        void Add(IVehicle vehicle);
        void Remove(int index);
        void Init(int v);
        void MakeTestPopulation();
        void AddParkingExtras(int index, string name);
        void RemoveParkingExtras(int index, string name);
        Tuple<string, Type>[] GetVehicleParameterInfo(string input);
        IVehicle CreateVehicle(string input, object[] para);
    }
}
