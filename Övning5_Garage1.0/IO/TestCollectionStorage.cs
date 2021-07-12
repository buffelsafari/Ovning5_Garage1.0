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
            "100\n"+
            "<end>\n"+
            "<vehicle>\n"+
            "0,Car,ABC080,porsche,red,1600,5.0\n"+
            "1,Car,ABC081,volvo,green,1400,2.0\n"+
            "2,Boat,ABC082,nimbus,pink,3500,6\n"+
            "3,Motorcycle,ABC083,kawazaki,yellow,200,280\n"+
            "4,Bus,ABC084,volvo,red,3200,40\n"+
            "5,Airplane,ABC085,skyace,white,750,2\n"+
            "7,Car,BAK120,porsche,gray,1600,4.8\n" +
            "8,Car,SAK281,volvo,blue,1400,2.6\n" +
            "11,Car,EBY678,bmw,green,1300,3.6\n" +
            "12,Car,AYM563,skoda,blue,1400,1.6\n" +
            "13,Car,GRT323,nissan,orange,1500,3.6\n" +
            "14,Car,YGI434,saab,brown,1350,2.2\n" +
            "20,Car,KIN063,volvo,blue,1480,2.4\n" +
            "21,Car,NIB555,volvo,green,1400,2.4\n" +
            "22,Car,OYE025,volvo,blue,1400,2.4\n" +
            "23,Car,K1,volvo,green,1400,4.8\n" +
            "24,Car,AMB1,volvo,white,1800,7.2\n" +
            "25,Car,NIL322,volvo,black,1500,2.2\n" +
            "26,Car,ASN976,volvo,blue,1400,2\n" +            
            "27,Car,HUT682,datsun,orange,1300,2.1\n" +
            "28,Bus,RAB084,volvo,blue,5200,80\n" +
            "29,Motorcycle,MTR110,kawazaki,black,230,320\n" +
            "30,Car,RIT221,volvo,pink,1500,3.6\n" +
            "31,Car,ABI823,toyota,gray,1400,2.0\n" +
            "32,Car,VIC020,volvo,beige,1400,2.0\n" +
            "33,Car,CMD064,volvo,beige,1400,1.8\n" +
            "35,Airplane,SKY001,skyace,white,750,2\n" +
            "36,Airplane,SKY010,jetpack,black,150,0\n" +
            "37,Airplane,SKY011,ramsky,white,4750,4\n" +
            "38,Airplane,HUB658,skyport,white,650,1\n" +
            "40,Boat,BOT082,nimbus,pink,3500,6\n" +
            "41,Boat,TIT123,norboat,white,3200,4.6\n" +
            "42,Boat,ANI384,craken,white,3500,6\n" +
            "43,Boat,CKK641,revsitter,white,3300,6.3\n" +
            "44,Boat,SNI563,voyage,red,3000,4.8\n" +
            "45,Boat,GYT456,erebus,yellow,4500,8.5\n" +
            "60,Car,ABY345,porsche,gray,1500,4.6\n" +
            "61,Car,LEM870,porsche,yellow,1200,2.8\n" +
            "62,Car,CAR123,porsche,black,1600,5.0\n" +
            "63,Car,BLA664,seat,black,1600,1.6\n" +
            "64,Car,UIU085,audi,brown,1600,4.2\n" +
            "65,Car,FTR023,porsche,white,1600,5.0\n" +
            "66,Car,ESK480,bmw,blue,1600,5.0\n" +
            "<end>\n" +
            "<parking>\n"+
            "0,BasicParking\n"+
            "1,BasicParking\n"+
            "2,BasicParking\n"+
            "3,BasicParking\n"+
            "4,BasicParking\n"+
            "5,BasicParking\n"+
            "6,BasicParking\n"+
            "7,BasicParking\n"+
            "8,BasicParking\n"+
            "9,BasicParking\n"+
            "10,BasicParking\n"+
            "11,BasicParking\n"+
            "12,BasicParking\n"+
            "13,BasicParking\n"+
            "14,BasicParking\n"+
            "15,BasicParking\n"+
            "16,BasicParking\n"+
            "17,BasicParking\n"+
            "18,BasicParking\n"+
            "19,BasicParking\n"+
            "20,Lights,BasicParking\n" +
            "21,Lights,BasicParking\n" +
            "22,Lights,BasicParking\n" +
            "23,Lights,BasicParking\n" +
            "24,Lights,BasicParking\n" +
            "25,Lights,BasicParking\n" +
            "26,Lights,BasicParking\n" +
            "27,Lights,BasicParking\n" +
            "28,Lights,BasicParking\n" +
            "29,Lights,BasicParking\n" +
            "30,Lights,BasicParking\n" +
            "31,Lights,BasicParking\n" +
            "32,Lights,BasicParking\n" +
            "33,Lights,BasicParking\n" +
            "34,Lights,BasicParking\n" +
            "35,Lights,BasicParking\n" +
            "36,Lights,BasicParking\n" +
            "37,Lights,BasicParking\n" +
            "38,Lights,BasicParking\n" +
            "39,Lights,BasicParking\n" +
            "40,Lights,Electricity,BasicParking\n" +
            "41,Lights,Electricity,BasicParking\n" +
            "42,Lights,Electricity,BasicParking\n" +
            "43,Lights,Electricity,BasicParking\n" +
            "44,Lights,Electricity,BasicParking\n" +
            "45,Lights,Electricity,BasicParking\n" +
            "46,Lights,Electricity,BasicParking\n" +
            "47,Lights,Electricity,BasicParking\n" +
            "48,Lights,Electricity,BasicParking\n" +
            "49,Lights,Electricity,BasicParking\n" +
            "50,Lights,Electricity,BasicParking\n" +
            "51,Lights,Electricity,BasicParking\n" +
            "52,Lights,Electricity,BasicParking\n" +
            "53,Lights,Electricity,BasicParking\n" +
            "54,Lights,Electricity,BasicParking\n" +
            "55,Lights,Electricity,BasicParking\n" +
            "56,Lights,Electricity,BasicParking\n" +
            "57,Lights,Electricity,BasicParking\n" +
            "58,Lights,Electricity,BasicParking\n" +
            "59,Lights,Electricity,BasicParking\n" +
            "60,Lights,Electricity,BasicParking\n" +
            "61,Lights,Electricity,BasicParking\n" +
            "62,Lights,Electricity,BasicParking\n" +
            "63,Lights,Electricity,BasicParking\n" +
            "64,Lights,Electricity,BasicParking\n" +
            "65,Lights,Electricity,BasicParking\n" +
            "66,Lights,Electricity,BasicParking\n" +
            "67,Lights,Electricity,BasicParking\n" +
            "68,Lights,Electricity,BasicParking\n" +
            "69,Lights,Electricity,BasicParking\n" +
            "70,BasicParking\n" +
            "71,BasicParking\n" +
            "72,BasicParking\n" +
            "73,BasicParking\n" +
            "74,BasicParking\n" +
            "75,BasicParking\n" +
            "76,BasicParking\n" +
            "77,BasicParking\n" +
            "78,BasicParking\n" +
            "79,BasicParking\n" +
            "80,BasicParking\n" +
            "81,BasicParking\n" +
            "82,BasicParking\n" +
            "83,BasicParking\n" +
            "84,BasicParking\n" +
            "85,BasicParking\n" +
            "86,BasicParking\n" +
            "87,BasicParking\n" +
            "88,BasicParking\n" +
            "89,BasicParking\n" +
            "90,BasicParking\n" +
            "91,BasicParking\n" +
            "92,BasicParking\n" +
            "93,BasicParking\n" +
            "94,BasicParking\n" +
            "95,BasicParking\n" +
            "96,BasicParking\n" +
            "97,BasicParking\n" +
            "98,BasicParking\n" +
            "99,BasicParking\n" +
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
            //throw new NotImplementedException();
        }
    }
}
