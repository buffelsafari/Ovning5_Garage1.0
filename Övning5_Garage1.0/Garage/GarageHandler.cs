using Garage10.Parking;
using Garage10.Vehicle;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Garage10.Garage
{
    class GarageHandler:IGarageHandler
    {
        IGarage<IVehicle, IParkingLot> currentGarage;

        public int Capacity => currentGarage.Capacity;
        public int Count => currentGarage.Count;

        public event Action<object, LogMessage, MessageType> LogEvent;

        IFactory vehicleFactory;
        public GarageHandler(IFactory vehicleFactory)
        {
            this.vehicleFactory = vehicleFactory;
        }



        public void Init(int capacity)
        { 
            currentGarage = new Garage<IVehicle, IParkingLot>(capacity);
            currentGarage.LogEvent += OnLogEvent;
            
            for (int i = 0; i < currentGarage.Capacity; i++)
            {                
                currentGarage[i] = new BasicParking();                
            }
            
        }

        public void Save(StreamWriter writer)
        {
            writer.Write("<header>\n");
            writer.Write(currentGarage.Capacity+"\n");
            writer.Write("<end>\n");

            writer.Write("<vehicle>\n");
            foreach (var v in currentGarage)
            {
                v.Save(writer);    
            }
            writer.Write("<end>\n");


            writer.Write("<parking>\n");
            int counter = 0;
            foreach (var v2 in currentGarage.GetParkingLots())
            {
                writer.Write(counter + ",");
                v2.Save(writer);
                writer.Write("\n");
                counter++;
            }

            writer.Write("<end>\n");

        }

        public void Load(StreamReader reader)
        {
            Action<string> LoadAction=null;

            while (!reader.EndOfStream)
            {
                string line=reader.ReadLine();
                                
                switch (line)
                {
                    case "<header>":
                        LoadAction = HeaderLoad;
                        continue;
                    case "<vehicle>":
                        LoadAction = VehicleLoad;
                        continue;
                    case "<parking>":
                        LoadAction = ParkingLoad;
                        continue;
                    case "<end>":
                        LoadAction = null;
                        continue;
                
                }                
                LoadAction?.Invoke(line);
            }
        }
        /// <summary>
        /// reads the size from the header file and makes a new garage        /// 
        /// </summary>
        /// <param name="line">the lines inside the file header</param>
        private void HeaderLoad(string line)  
        {
            string[] split = line.Split(',');
            int index;

            if (int.TryParse(split[0], out index))
            {
                currentGarage = new Garage<IVehicle, IParkingLot>(index);
                currentGarage.LogEvent += OnLogEvent;
            }
        }

        /// <summary>
        /// Reads and create AbstractParking objects from the string
        /// </summary>
        /// <param name="line">the lines inside the parking section</param>
        private void ParkingLoad(string line)
        { 
            string[] split = line.Split(',');
            int index;

            if (int.TryParse(split[0], out index))
            {
                AbstractParking[] parkings = new AbstractParking[split.Length - 1];  
                for (int i = 1; i < split.Length; i++)
                {
                    Type type = Type.GetType($"Garage10.Parking.{split[i]}");             
                    
                    if (type == null || !type.IsAssignableTo(typeof(AbstractParking)))
                    {
                        return;
                    }
                    parkings[i - 1] = (AbstractParking)Activator.CreateInstance(type);
                }

                AbstractParking bigP = parkings[parkings.Length - 1];
                for (int i = parkings.Length - 2; i >= 0; i--)
                {
                    bool result;
                    bigP = (AbstractParking)bigP.Add(parkings[i], out result);

                    if (!result)
                    {
                        LogEvent?.Invoke(this, LogMessage.COULD_NOT_ADD_FEATURE, MessageType.FAILURE);                        
                    }
                }
                currentGarage[index] = bigP;
                
            }
            else
            {
                LogEvent?.Invoke(this, LogMessage.OUT_OF_RANGE, MessageType.FAILURE);                
            }


        }

        /// <summary>
        /// creates a vehicle and puts it in the index
        /// </summary>
        /// <param name="line">the lines inside vehicle section</param>
        private void VehicleLoad(string line)
        {
            string[] split = line.Split(',');           

            int index;
            if (int.TryParse(split[0], out index))
            {
                Tuple<string, Type>[] tupp = vehicleFactory.GetParameterInfo(split[1]);

                if (tupp != null)
                {
                    Object[] para = new Object[tupp.Length];
                    for (int i = 0; i < para.Length; i++)
                    {
                        para[i] = Convert.ChangeType(split[i + 2], tupp[i].Item2, CultureInfo.InvariantCulture);
                    }
                    currentGarage.AddVehicleAt(vehicleFactory.CreateVehicle(split[1], para), index);
                }
            }
        }
                        
        /// <summary>
        /// creates a AbstractParking instance from a name
        /// </summary>
        /// <param name="name">the name of the class</param>
        /// <returns></returns>
        private AbstractParking CreateParkingExtra(string name) 
        {
            Object c = Activator.CreateInstance(Type.GetType($"Garage10.Parking.{name}"));            
            return (AbstractParking)c;
        }

        /// <summary>
        /// Creates and add a AbstractParking component at index
        /// </summary>
        /// <param name="index">the parking lot</param>
        /// <param name="name">name of the AbstractParking component</param>
        public void AddParkingExtras(int index, string name)
        {
            if (index >= 0 && index < currentGarage.Capacity)
            {
                Type type = Type.GetType($"Garage10.Parking.{name}"); //todo get assemby string             
                
                if (type == null || !type.IsAssignableTo(typeof(AbstractParking)))
                {
                    LogEvent?.Invoke(this, LogMessage.NOT_A_VALID_FEATURE, MessageType.FAILURE);
                    return;
                }                

                AbstractParking ab = CreateParkingExtra(name);
                bool result;
                currentGarage[index] = currentGarage[index].Add(ab, out result);
                if (result)
                {
                    LogEvent?.Invoke(this, LogMessage.PARKING_FEATURE_ADDED, MessageType.SUCCESS);
                }
                else
                {
                    LogEvent?.Invoke(this, LogMessage.COULD_NOT_ADD_FEATURE, MessageType.FAILURE);
                }
            }
            else
            {
                LogEvent?.Invoke(this, LogMessage.OUT_OF_RANGE, MessageType.FAILURE);
            }            
        }

        /// <summary>
        /// Removes a AbstractParking component from a parking lot
        /// </summary>
        /// <param name="index">the parking lot</param>
        /// <param name="name">name of the component</param>
        public void RemoveParkingExtras(int index, string name)  
        {
            if (index >= 0 && index < currentGarage.Capacity)
            {
                Type type = Type.GetType($"Garage10.Parking.{name}"); //todo get assemby string             
                                
                if (type == null || !type.IsAssignableTo(typeof(AbstractParking)))
                {
                    LogEvent?.Invoke(this, LogMessage.NOT_A_VALID_FEATURE, MessageType.FAILURE);
                    return;
                }

                bool result;
                currentGarage[index] = currentGarage[index].Remove(type, out result);
                if (result)
                {
                    LogEvent?.Invoke(this, LogMessage.PARKING_FEATURE_REMOVED, MessageType.SUCCESS);
                }
                else
                {
                    LogEvent?.Invoke(this, LogMessage.COULD_NOT_REMOVE_FEATURE, MessageType.FAILURE);
                }
            }
            else
            {
                LogEvent?.Invoke(this, LogMessage.OUT_OF_RANGE, MessageType.FAILURE);
            }
        }

        /// <summary>
        /// the entry method for getting a stream of query result
        /// </summary>
        /// <param name="stream">the stream</param>
        /// <param name="line">the filter expression</param>
        public void FilterCommand(Stream stream, string line)
        {
            GetFilteredDataStream(stream, TranslateFilterLine(line, currentGarage));             
        }

        /// <summary>
        /// writes some vehicles on the stream
        /// </summary>
        /// <param name="stream">the query stream</param>
        /// <param name="vehicles">a enumerable IVehicle collection</param>
        private void GetFilteredDataStream(Stream stream,IEnumerable<IVehicle> vehicles)
        {
            long start = stream.Position;

            StreamWriter writer = new StreamWriter(stream);
            
            writer.Write("begin\n");
            foreach (IVehicle v in vehicles)
            {
                var prop = v.GetType().GetProperties();

                writer.Write("row\n");
                writer.Write(v.GetType().Name + "\n");

                foreach (var p in prop)
                {
                    Object val = p.GetValue(v);

                    if (p.Name.Equals("Parking")) // parking is a special
                    {                        
                        int value = 0;
                        if (int.TryParse(val.ToString(), out value))
                        {
                            writer.Write("P:" + value);
                            writer.Write(currentGarage[value].GetDescription() + "\n");   
                        }
                    }
                    else
                    {
                        if (val.GetType() == typeof(float))
                        {
                            val = ((float)val).ToString("0.00",CultureInfo.InvariantCulture);                                                       
                        }
                        writer.Write($"{p.Name}:{val}\n");
                    }
                }
            }

            writer.Write("end\n");

            writer.Flush();
            stream.Position = start;
            
        }


        //----------------------------------------------------------------------------
        /// <summary>
        /// makes a linq ed selection from the string
        /// </summary>
        /// <param name="str">the filter expression</param>
        /// <param name="garage">the garage with an enumerable collection </param>
        /// <returns></returns>
        private static IEnumerable<IVehicle> TranslateFilterLine(string str, IGarage<IVehicle, IParkingLot> garage) 
        {

            string[] split = str.Split('&'); // only '&' implemented

            IEnumerable<IVehicle> arr=garage;

            foreach (string s in split)
            {                
                string[] arguments=TranslateFilterCommand(s);
                if (arguments == null)
                {
                    return garage;
                }

                if (arguments[0].Equals("type"))  // filtering on type
                {
                    arr = arr.Where((a) => TypeCompare(a, arguments[1], arguments[2]));
                }
                else  // filtering on property
                {
                    arr = arr.Where((a) => PropertyCompare(a, arguments[0], arguments[1], arguments[2]));
                }
                
            }            
            return arr;
        }

        private static string[] TranslateFilterCommand(string str)
        {
            string[] separators = new string[] {"<=", ">=", "=", "<", ">" };  //todo maybe dictionary

            int opIndex = -1; 
            for (int i = 0;i<separators.Length ; i++)
            {                
                if (str.Contains(separators[i]))
                {
                    opIndex = i;
                    break;
                }
            }

            string[] split = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 2)
            {
                return null;
            }                        
            
            return new string[]{split[0], split[1], separators[opIndex]};
        }


        private static bool TypeCompare(IVehicle vehicle, string typeName, string compOperator)
        {
            typeName = typeName.ToLower();
            string name = vehicle.GetType().Name.ToLower();

            return CompareLogic((string)name, typeName, compOperator);
            
        }

        private static bool PropertyCompare(IVehicle vehicle, string propName, string compareValue, string logicOperator)
        {
            propName = propName.ToLower();
            compareValue = compareValue.ToLower();
            PropertyInfo[] prop = vehicle.GetType().GetProperties();
            foreach (PropertyInfo info in prop)
            {
                if (propName.Equals(info.Name.ToLower()))
                {                    
                    Object propValue;
                    
                    switch (info.PropertyType.Name)  
                    {                        
                        case "String":                            
                            propValue = (string)info.GetValue(vehicle);
                            propValue = ((string)propValue).ToLower();
                            return CompareLogic((string)propValue, compareValue, logicOperator);                                                       
                        case "Int32":
                            propValue = (int)info.GetValue(vehicle);
                            int intCompare;
                            int.TryParse(compareValue, out intCompare);
                            return CompareLogic((int)propValue, intCompare, logicOperator);
                        case "Single":
                            propValue = (Single)info.GetValue(vehicle);
                            Single floatCompare;
                            Single.TryParse(compareValue,NumberStyles.AllowDecimalPoint ,CultureInfo.InvariantCulture, out floatCompare);
                            return CompareLogic((Single)propValue, floatCompare, logicOperator);

                    }                   
                }
            }            

            return false;
        }

        private static bool CompareLogic<T>(T a, T b, string op) where T: IComparable<T>
        {
            switch (op)
            {
                case "=":
                    return a.Equals(b);
                case "<":
                    return a.CompareTo(b) < 0;
                case ">":
                    return a.CompareTo(b) > 0;                    
                case "<=":
                    return a.CompareTo(b) <= 0;
                case ">=":
                    return a.CompareTo(b) >= 0;

            }
            return false;
        }

        public void Add(IVehicle vehicle)
        {
            currentGarage.AddVehicle(vehicle);    
        }


        void OnLogEvent(object sender, LogMessage logMessage, MessageType messageType)
        {
            // just resending messages to the next in the chain
            LogEvent?.Invoke(sender, logMessage, messageType);
        }

        public void Remove(int index)
        {
            currentGarage.RemoveVehicleAt(index);            
        }

        public Tuple<string, Type>[] GetVehicleParameterInfo(string input)
        {
            return vehicleFactory.GetParameterInfo(input);
        }

        public IVehicle CreateVehicle(string input, object[] para)
        {
            IVehicle vehicle= vehicleFactory.CreateVehicle(input, para);

            if (vehicle.IsValid())
            {
                return vehicle;
            }

            return null;
        }
    }

}
