using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage10.IO
{
    class TestCollectionStorage : IStorage
    {
        const string file =
            (
            "<header>\n"+
            "20\n"+
            "<end>\n"+
            "<vehicle>\n"+
            "0,Car,ABC080,porsche,red,1600,5\n"+
            "1,Car,ABC081,volvo,green,1400,2\n"+
            "2,Boat,ABC082,nimbus,pink,3500,6\n"+
            "3,Motorcycle,ABC083,kawazaki,yellow,200,280\n"+
            "4,Bus,ABC084,volvo,red,3200,40\n"+
            "5,Airplane,ABC085,skyace,white,750,2\n"+
            "<end>\n"+
            "<parking>\n"+
            "0,Lights,Electricity,BasicParking\n"+
            "1,Lights,Electricity,BasicParking\n"+
            "2,Lights,Electricity,BasicParking\n"+
            "3,Lights,Electricity,BasicParking\n"+
            "4,Lights,Electricity,BasicParking\n"+
            "5,Lights,Electricity,BasicParking\n"+
            "6,Lights,Electricity,BasicParking\n"+
            "7,Lights,Electricity,BasicParking\n"+
            "8,Lights,Electricity,BasicParking\n"+
            "9,Lights,Electricity,BasicParking\n"+
            "10,Lights,Electricity,BasicParking\n"+
            "11,Lights,Electricity,BasicParking\n"+
            "12,Lights,Electricity,BasicParking\n"+
            "13,Lights,Electricity,BasicParking\n"+
            "14,Lights,Electricity,BasicParking\n"+
            "15,Lights,Electricity,BasicParking\n"+
            "16,Lights,Electricity,BasicParking\n"+
            "17,Lights,Electricity,BasicParking\n"+
            "18,Lights,Electricity,BasicParking\n"+
            "19,Lights,Electricity,BasicParking\n"+
            "<end>\n"
            );
        
        public void Load(ILoadable thing)
        {                       
            byte[] data = Encoding.ASCII.GetBytes(file);
            MemoryStream stm = new MemoryStream(data, 0, data.Length);
           
            StreamReader reader = new StreamReader(stm);
            thing.Load(reader);
            
        }

        public void Save(ISavable thing)
        {
            throw new NotImplementedException();
        }
    }
}
