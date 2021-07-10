using Garage10.Garage;
using Garage10.IO;
using Garage10.Parking;
using Garage10.UI;
using Garage10.Vehicle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Remoting;
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
            
            ServiceCollection serviceCollection = new ServiceCollection(); 
            serviceCollection.AddSingleton<IStorage, FileStorage>();
            serviceCollection.AddSingleton<IStorage, TestCollectionStorage>();
            serviceCollection.AddSingleton<IGarageHandler, GarageHandler>();
            serviceCollection.AddSingleton<IUI, ConsoleUI>();
            serviceCollection.AddSingleton<IFactory, VehicleFactory>();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            


            App app= new App(serviceProvider);
            app.Init();
            app.Run();
            
        }

        

    }
}
