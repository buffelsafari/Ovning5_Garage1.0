using Garage10.Vehicle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGarage
{
    [TestClass]
    public class UnitTestAirplane
    {
        string regNumber = "VIC020";
        string brand = "highflyer";
        string color = "white";
        int weight = 1234;
        int propeller = 1;

        [TestInitialize]
        public void Setup()
        { 
        
        }

        [TestCleanup]
        public void TearDown()
        { 
        
        }

        [TestMethod]
        public void TestConstructorProperties()
        {
            string expectedRegNumber = "VIC020";
            string expectedBrand = "highflyer";
            string expectedColor = "white";
            int expectedWeight = 1234;
            int expectedPropellers = 1;

            Airplane airplane = new Airplane(expectedRegNumber,expectedBrand,expectedColor,expectedWeight,expectedPropellers);

            string actualRegNumber = airplane.RegNumber;
            string actualBrand = airplane.Brand;
            string actualColor = airplane.Color;
            int actualWeight = airplane.Weight;
            int actualPropellers = airplane.Propellers;

            Assert.AreEqual(expectedRegNumber, actualRegNumber);
            Assert.AreEqual(expectedBrand, actualBrand);
            Assert.AreEqual(expectedColor, actualColor);
            Assert.AreEqual(expectedWeight, actualWeight);
            Assert.AreEqual(expectedPropellers, actualPropellers);

        }

        [TestMethod]
        public void TestValidationPropellerMin()
        {            
            int propellersSuccess = 0;           

            Airplane validAirplane = new Airplane(regNumber, brand, color, weight, propellersSuccess);
            
            bool expected = true;
            bool actual=validAirplane.IsValid();

            Assert.AreEqual(expected, actual);            

        }

        [TestMethod]
        public void TestValidationPropellerLessThenMin()
        {
            int propellersFail = -1;

            Airplane validAirplane = new Airplane(regNumber, brand, color, weight, propellersFail);

            bool expected = false;
            bool actual = validAirplane.IsValid();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidationPropellerMax()
        {
            int propellersSuccess = 999999;

            Airplane validAirplane = new Airplane(regNumber, brand, color, weight, propellersSuccess);

            bool expected = true;
            bool actual = validAirplane.IsValid();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestValidationPropellerMoreThenMax()
        {
            int propellersFail = 1000000;

            Airplane validAirplane = new Airplane(regNumber, brand, color, weight, propellersFail);

            bool expected = false;
            bool actual = validAirplane.IsValid();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSaveStream()
        {
            Airplane airplane = new Airplane(regNumber, brand, color, weight, propeller);

            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);            
            airplane.Save(writer);
            writer.Flush();

            StreamReader reader = new StreamReader(stream);
            stream.Position = 0;
            string expected = $"{airplane.Parking},{typeof(Airplane).Name},{regNumber},{brand},{color},{weight},{propeller}\n";
            string actual=reader.ReadToEnd();

            Assert.AreEqual(expected, actual);

            stream.Close();
        }

    }
}
