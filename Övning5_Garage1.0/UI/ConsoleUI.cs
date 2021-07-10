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
            Console.WriteLine("types (lists available vehicle types)");
            Console.WriteLine("new {size} (creates a new garage)");
            Console.WriteLine("add {type}");
            Console.WriteLine("remove {lot}");
            Console.WriteLine("list {type=car&weight<500}...");
            Console.WriteLine("testpop (get a test population)");
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
            Console.WriteLine("***********************");
            Console.WriteLine("* Welcome to Garage10 *");
            Console.WriteLine("***********************");
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
