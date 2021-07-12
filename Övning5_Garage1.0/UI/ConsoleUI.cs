using Garage10.Garage;
using Garage10.Vehicle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Garage10.UI
{
    class ConsoleUI:IUI
    {
        public void EndDataRows()
        {
            Console.ForegroundColor= ConsoleColor.Blue;
            Console.WriteLine("\n--------------------------------------------------------------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public string GetCommandLine()
        {
            Console.ForegroundColor = ConsoleColor.White;
            return Console.ReadLine();
        }

        public void NewDataRow()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\n||   ");
            Console.ForegroundColor = ConsoleColor.Yellow;

        }        

        public void PrintData(string name)
        {
            //Hack trying to get nice columns
            string str = name + "                         ";

            Console.Write(str.Substring(0,20));
        }

               

        public void StartDataRows()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("--------------------------------------------------------------------------------------------------------------------------------------------");
            
        }


        public void OnLogEvent(object sender, LogMessage logMessage, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.FAILURE:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case MessageType.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case MessageType.INFORMATION:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }

            Console.WriteLine(logMessage);
            Console.ForegroundColor = ConsoleColor.White;
        }



        public Object[] AskForParameters(Tuple<string, Type>[] para)
        {
            Console.WriteLine("Enter creation parameters:");
            Object[] outPara = new Object[para.Length];
                        
            for(int i=0;i<para.Length ;i++)
            {
                Console.Write($"{para[i].Item1}?:");
                string line=Console.ReadLine();

                try
                {
                    // todo nicer exeption
                    Object ob = Convert.ChangeType(line, para[i].Item2);  
                    outPara[i] = ob; 
                }
                catch(Exception e)
                {                    
                    return null;
                }               
                               

            }           

            return outPara;
        }

        public void PrintHelpMessage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Commands:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("help");
            Console.WriteLine("quit");
            Console.Write("load");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" (loads from a file)");
            Console.ForegroundColor = ConsoleColor.Yellow;            
            Console.Write("save");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" (saves to a file)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("parking n+lights, parking n-electicity");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" where n is the parking lot");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("types");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" (lists available vehicle types)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("new {size}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" (creates a new garage)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("add {type}");
            Console.WriteLine("remove {lot}");
            Console.WriteLine("list {type=car&weight<500}...");
            Console.Write("testpop");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" (loads a tespopulation from a fake loader)");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PrintRows(IEnumerable<string> rows)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (string s in rows)
            {
                Console.WriteLine(s);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PrintWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine("\t\t***********************");
            Console.WriteLine("\t\t* Welcome to Garage10 *");
            Console.WriteLine("\t\t***********************");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            PrintHelpMessage();
        }

        public void PrintQuitMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Bye, Thank you for playing!");
            Console.ForegroundColor = ConsoleColor.White;            
        }

        public void PrintListInformation(int dataRows, int capacity, int parkedVehicles)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"a list with {dataRows} vehicles of {parkedVehicles}, in a garage with capacity of {capacity}");


            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
