using Garage10.Garage;
using Garage10.IO;
using Garage10.Parking;
using Garage10.UI;
using Garage10.Vehicle;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestGarage")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Garage10
{
    class Program
    {
        static void Main(string[] args)
        {
            IStorage fileStorage= new FileStorage();
            IStorage testStorage = new TestCollectionStorage();

            IUI consoleUI = new ConsoleUI();
            IFactory vFactory = new VehicleFactory(); 
            IGarageHandler garageHandler = new GarageHandler(vFactory);


            App app= new App(fileStorage, testStorage, garageHandler, consoleUI);
            app.Init();
            app.Run();
            
            
            


        }

        
    }
}
