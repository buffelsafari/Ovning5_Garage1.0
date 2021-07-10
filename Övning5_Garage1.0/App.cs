using Garage10.Garage;
using Garage10.IO;
using Garage10.UI;
using Garage10.Vehicle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Garage10
{
    class App
    {
        //public event Action<object, LogMessage, MessageType> LogEvent;

        bool isRunning = true;
        Stream stream;
        IUI ui;
        IGarageHandler garageHandler;        
        IStorage fileStorage;
        IStorage testStorage;

        Dictionary<string, Action<string>> inputActions;
        Dictionary<char, Action<int, string>> parkingOperatorActions;
        Dictionary<string, Action> filterStreamActions;

        public App(IStorage fileStorage, IStorage testStorage, IGarageHandler garageHandler, IUI ui) // preparing for DI
        {            
            this.garageHandler = garageHandler;
            this.ui = ui;
            this.fileStorage = fileStorage;
            this.testStorage = testStorage;
            
        }

        public void Init()
        {
            garageHandler.Init(10);
            garageHandler.LogEvent += OnLogEvent;

            inputActions = new Dictionary<string, Action<string>>()
            {
                {"load", HandleLoad},
                {"save", HandleSave},
                {"parking", HandleParking},
                {"list", HandleListCommand},
                {"add", HandleAddCommand},
                {"remove", HandleRemoveCommand},
                {"help", HandleHelp},
                {"quit", Quit},
                {"q", Quit},
                {"testpop", MakeTestPopulation},
                {"new", MakeNewGarage},
                {"types", AvailableTypes}
            };

            parkingOperatorActions = new Dictionary<char, Action<int, string>>()
            {
                {'+', garageHandler.AddParkingExtras},
                {'-', garageHandler.RemoveParkingExtras}
            };

            filterStreamActions = new Dictionary<string, Action>() // todo local methods
            {
                {"begin",  ui.StartDataRows },
                {"end",  ui.EndDataRows},
                {"row", ui.NewDataRow }

            };

        }

        public void HandleInput(string input)  
        {
            input = input.ToLower();            
            string[] split=input.Split(' ');            
            input = input.Substring(split[0].Length);
            input = String.Concat(input.Where(c => !Char.IsWhiteSpace(c)));

            Action<string> inputAction;
            if(inputActions.TryGetValue(split[0], out inputAction))
            {
                inputAction(input);
            }        
            
        }

        private void HandleHelp(string input)
        {
            ui.PrintHelpMessage();
        }
        private void HandleLoad(string input)
        {
            fileStorage.Load(garageHandler);
        }

        private void HandleSave(string input)
        {
            fileStorage.Save(garageHandler);
        }

        private void HandleParking(string input)  //todo refactory
        {
            foreach (var ops in parkingOperatorActions)
            {
                string[] split = input.Split(ops.Key);
                if (split.Length > 1)
                {
                    int value;
                    if (int.TryParse(split[0], out value))  // adding features
                    {
                        char[] chrstr = split[1].ToCharArray();
                        chrstr[0] = char.ToUpper(chrstr[0]); // make first letter big to match Class name
                        split[1] = new string(chrstr);
                        ops.Value(value, split[1]);                        
                    }
                    else
                    {
                        ui.OnLogEvent(this, LogMessage.SYNTAX_ERROR, MessageType.FAILURE);
                    }
                    return;
                }
            }          
        }        


        private void MakeNewGarage(string input)
        {
            int size = -1;
            int.TryParse(input, out size);
            garageHandler.Init(size);
            ui.OnLogEvent(this, LogMessage.GARAGE_CREATED, MessageType.SUCCESS);
            
        }

        private void AvailableTypes(string input)
        {
            var children = Assembly.GetAssembly(typeof(BaseVehicle)).GetTypes().Where(c => c.IsSubclassOf(typeof(BaseVehicle))).Select(c=>c.Name);            
            ui.PrintRows(children);              
            

        }

        private void MakeTestPopulation(string input)
        {
            testStorage.Load(garageHandler);
            //garageHandler.MakeTestPopulation();
        }

        private void Quit(string input)
        {
            isRunning = false;
            
            // todo cleanup
        }

        private void HandleRemoveCommand(string input)
        {
            int index;
            if (int.TryParse(input, out index))
            {
                garageHandler.Remove(index);
            }
        }

        private void HandleAddCommand(string input)
        {
                        
            
            char[] chrstr=input.ToCharArray();   
            chrstr[0]=char.ToUpper(chrstr[0]); // make first letter big to match Class name
            input = new string(chrstr);            
                       

            // add wehicles
            Tuple<string, Type>[] tupp=garageHandler.GetVehicleParameterInfo(input);

            if (tupp != null)
            {
                Object[] para = ui.AskForParameters(tupp);
                if (para != null)
                {
                    IVehicle vehicle = garageHandler.CreateVehicle(input, para);  // create and add in handler?
                    if (vehicle!=null)
                    { 
                        garageHandler.Add(vehicle);
                    }
                    else
                    {                        
                        ui.OnLogEvent(this,LogMessage.INVALID_VEHICLE_PARAMETERS,MessageType.FAILURE);
                    }
                }
                else
                {
                    ui.OnLogEvent(this, LogMessage.SYNTAX_ERROR, MessageType.FAILURE);
                }
            }
            else
            {
                ui.OnLogEvent(this, LogMessage.SYNTAX_ERROR, MessageType.FAILURE);
            }
        }

        private void HandleListCommand(string input)
        {
            // todo test
            garageHandler.FilterCommand(stream, input);
            stream.Flush();
        }

        public void Run()
        {
            stream = new MemoryStream();
            ui.PrintWelcomeMessage();

            while (isRunning)
            {
                string input=ui.GetCommandLine();

                HandleInput(input);

                HandleStream();  
                
            }

            ui.PrintQuitMessage();
            stream.Close();
        }

        private void HandleStream()
        {
            StreamReader reader = new StreamReader(stream);

            int dataRows = -2;  // top and bottom frame subtracted
            while (!reader.EndOfStream)
            {                
                string data = reader.ReadLine();

                Action UIDataAction;
                if (filterStreamActions.TryGetValue(data, out UIDataAction))
                {
                    dataRows++;
                    UIDataAction();
                }
                else
                {
                    ui.PrintData(data);
                }                
              
            }            
            if (dataRows > 0)
            {
                ui.PrintListInformation(dataRows, garageHandler.Capacity, garageHandler.Count);
            }
        }

        

        void OnLogEvent(object sender, LogMessage logMessage, MessageType messageType)
        {            
            ui.OnLogEvent(sender, logMessage, messageType);            
        }


    }
}
