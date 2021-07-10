using Garage10.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.UI
{
    interface IUI
    {
        string GetCommandLine();
        void NewDataRow();
        void EndDataRows();
        void StartDataRows();
        //public void PrintPropertyData(string name, string value);
        void PrintData(string name);

        Object[] AskForParameters(Tuple<string, Type>[] para);

        void OnLogEvent(object sender, LogMessage logMessage, MessageType messageType);
        void PrintHelpMessage();
        void PrintRows(IEnumerable<string> rows);
        void PrintWelcomeMessage();
        void PrintQuitMessage();
        void PrintListInformation(int dataRows, int capacity, int parkedVehicles);
    }
}
