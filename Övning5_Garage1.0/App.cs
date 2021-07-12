using Garage10.Garage;
using Garage10.IO;
using Garage10.UI;
using Garage10.Vehicle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private bool isRunning = true;
        private Stream stream;
        private readonly IUI ui;
        private readonly IGarageHandler garageHandler;        
        private readonly IStorage fileStorage;
        private readonly IStorage testStorage;

        private IConfiguration configuration;

        private Dictionary<string, Action<string>> inputActions;
        private Dictionary<char, Action<int, string>> parkingOperatorActions;
        private Dictionary<string, Action> filterStreamActions;

        public App(IServiceProvider serviceProvider) // preparing for DI
        {            
            this.garageHandler = serviceProvider.GetService<IGarageHandler>();
            this.ui = serviceProvider.GetService<IUI>();
            this.fileStorage = serviceProvider.GetServices<IStorage>().First(s => s.GetType() == typeof(FileStorage)); 
            this.testStorage = serviceProvider.GetServices<IStorage>().First(s => s.GetType() == typeof(TestCollectionStorage));
            this.configuration = GetConfig();
        }

        

        public void Init()
        {
            // garage init
            IConfigurationSection garageSection = configuration.GetSection("app:garage"); 
            int size = int.TryParse(garageSection["size"], out int result) ? result : 10;
            
            garageHandler.Init(size);
            garageHandler.LogEvent += OnLogEvent;

            LoadInputActionConfig(configuration.GetSection("app:input:action"));
            LoadParkingOperatorActionConfig(configuration.GetSection("app:input:parkingoperatoraction"));
            LoadFilterStreamActionConfig(configuration.GetSection("app:input:filterstreamaction"));
           
        }

        #region cofiguration methods
        private IConfiguration GetConfig()  // stolen from NET21B
        {
            return new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();
        }

        private void LoadInputActionConfig(IConfigurationSection configSection)
        {            
            inputActions = new Dictionary<string, Action<string>>();            
            IEnumerable<IConfigurationSection> inputSection = configSection.GetChildren();
            foreach (IConfigurationSection post in inputSection)
            {
                MethodInfo info = this.GetType().GetMethod(post.Value, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
                Action<string> action = (Action<string>)info.CreateDelegate(typeof(Action<string>), this);
                inputActions.Add(post.Key, action);
            }
        }

        private void LoadParkingOperatorActionConfig(IConfigurationSection configSection)
        {
            parkingOperatorActions = new Dictionary<char, Action<int, string>>();            
            IEnumerable<IConfigurationSection> inputSection = configSection.GetChildren();
            foreach (IConfigurationSection post in inputSection)
            {
                MethodInfo info = garageHandler.GetType().GetMethod(post.Value, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
                Action<int, string> action = (Action<int, string>)info.CreateDelegate(typeof(Action<int, string>), garageHandler);
                char c;
                if (char.TryParse(post.Key, out c))
                {
                    parkingOperatorActions.Add(c, action);
                }
            }
        }

        private void LoadFilterStreamActionConfig(IConfigurationSection configSection)
        {
            filterStreamActions = new Dictionary<string, Action>();  
            IEnumerable<IConfigurationSection> inputSection = configSection.GetChildren();
            foreach (IConfigurationSection post in inputSection)
            {
                MethodInfo info = ui.GetType().GetMethod(post.Value, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
                Action action = (Action)info.CreateDelegate(typeof(Action), ui);

                filterStreamActions.Add(post.Key, action);

            }
        }
        #endregion



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

        #region input actions

        private void HandleHelp(string input)
        {
            ui.PrintHelpMessage();
        }
        private void HandleLoad(string input)
        {
            try
            {
                fileStorage.Load(garageHandler);
                ui.OnLogEvent(this, LogMessage.FILE_LOADED, MessageType.SUCCESS);
            }
            catch (Exception e)
            {
                ui.OnLogEvent(this, LogMessage.IO_ERROR, MessageType.FAILURE);
            }
        }

        private void HandleSave(string input)
        {
            try
            {
                fileStorage.Save(garageHandler);
                ui.OnLogEvent(this, LogMessage.FILE_SAVED, MessageType.SUCCESS);
            }
            catch (Exception e)
            {
                ui.OnLogEvent(this, LogMessage.IO_ERROR, MessageType.FAILURE);
            }
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
            try
            {
                testStorage.Load(garageHandler);
                ui.OnLogEvent(this, LogMessage.FILE_LOADED, MessageType.SUCCESS);
            }
            catch(Exception e)
            {
                ui.OnLogEvent(this, LogMessage.IO_ERROR, MessageType.FAILURE);
            }
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

        #endregion


        //-------------------------------------------------------------------------------------------------
        public void Run()
        {
            stream = new MemoryStream(); // stream for getting garage data
            ui.PrintWelcomeMessage();

            while (isRunning)   // the main loop
            {
                string input=ui.GetCommandLine();
                HandleInput(input);
                HandleStream();
            }

            ui.PrintQuitMessage();
            stream.Close();
        }
        //-------------------------------------------------------------------------------------------------

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

        

        // events from the garagehandler, resending them to UI
        void OnLogEvent(object sender, LogMessage logMessage, MessageType messageType)
        {            
            ui.OnLogEvent(sender, logMessage, messageType);            
        }


    }
}
