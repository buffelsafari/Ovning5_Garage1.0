using Garage10.Garage;
using Garage10.Parking;
using Garage10.Vehicle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace TestGarage
{



    [TestClass]
    public class Garage
    {
        Garage<IVehicle, IParkingLot> garage10Capacity;
        Mock<IVehicle>[] vehicleMocks;
        Mock<IParkingLot>[] parkingLotMocks;


        [TestInitialize]
        public void Setup()
        {
            garage10Capacity = new Garage<IVehicle, IParkingLot>(10);

            vehicleMocks = new Mock<IVehicle>[11];
            for (int i = 0; i < vehicleMocks.Length; i++)
            {
                vehicleMocks[i] = new Mock<IVehicle>();
                vehicleMocks[i].Setup(m => m.Parking).Returns(1);
            }

            parkingLotMocks = new Mock<IParkingLot>[10];
            for (int i = 0; i < parkingLotMocks.Length; i++)
            {
                parkingLotMocks[i] = new Mock<IParkingLot>();
            }

        }

        [TestCleanup]
        public void TearDown()
        {
            garage10Capacity = null;
        }

        [TestMethod]
        public void CapacityValueTest()
        {
            int expected = 10;
            int actual = garage10Capacity.Capacity;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CountValueTest()
        {
            int expected = 0;
            int actual = garage10Capacity.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddVehicleTest()
        {
            garage10Capacity.AddVehicle(vehicleMocks[0].Object);
            int expected = 1;
            int actual = garage10Capacity.Count;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]

        public void AddVehicleRejectDuplicatesTest()
        {
            garage10Capacity.AddVehicle(vehicleMocks[0].Object);
            garage10Capacity.AddVehicle(vehicleMocks[0].Object);

            int expected = 1;
            int actual = garage10Capacity.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddDifferentEntitiesTest()
        {
            garage10Capacity.AddVehicle(vehicleMocks[0].Object);
            garage10Capacity.AddVehicle(vehicleMocks[1].Object);

            int expected = 2;
            int actual = garage10Capacity.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddToCapacityTest()
        {
            for (int i = 0; i < 10; i++)
            {
                garage10Capacity.AddVehicle(vehicleMocks[i].Object);
            }

            int expected = 10;
            int actual = garage10Capacity.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddPastCapacityTest()
        {
            for (int i = 0; i < 11; i++)
            {
                garage10Capacity.AddVehicle(vehicleMocks[i].Object);
            }

            int expected = 10;
            int actual = garage10Capacity.Count;

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void IterateFullCollectionTest()
        {
            for (int i = 0; i < 10; i++)
            {
                garage10Capacity.AddVehicle(vehicleMocks[i].Object);
            }

            int index = 0;
            foreach (IVehicle v in garage10Capacity)
            {
                IVehicle expected = vehicleMocks[index].Object;
                IVehicle actual = v;

                Assert.AreEqual(expected, actual);
                index++;
            }
        }

        [TestMethod]
        public void CheckIfParkingLostAreNullAtInitTest()
        {
            for (int i = 0; i < 10; i++)
            {
                IParkingLot actual = garage10Capacity[i];
                Assert.IsNull(actual);
            }
        }

        [TestMethod]
        public void AddParkingLotsTest()
        {
            for (int i = 0; i < 10; i++)
            {
                garage10Capacity[i] = parkingLotMocks[i].Object;
            }

            for (int i = 0; i < 10; i++)
            {
                IParkingLot actual = garage10Capacity[i];
                Assert.IsNotNull(actual);
            }
        }

        [TestMethod]
        public void EnumerateParkingLotsTest()
        {
            for (int i = 0; i < 10; i++)
            {
                garage10Capacity[i] = parkingLotMocks[i].Object;
            }

            IEnumerable<IParkingLot> collection = garage10Capacity.GetParkingLots();

            int index = 0;
            foreach (IParkingLot p in collection)
            {
                IParkingLot expected = parkingLotMocks[index].Object;
                IParkingLot actual = p;
                Assert.AreEqual(expected, actual);

                index++;
            }

        }

        [TestMethod]
        public void AddVehicleAtTest()
        {            

            garage10Capacity.AddVehicleAt(vehicleMocks[0].Object, 0);
            garage10Capacity.AddVehicleAt(vehicleMocks[9].Object, 9);
            garage10Capacity.AddVehicleAt(vehicleMocks[4].Object, 4);

            int expected = 3;
            int actual = garage10Capacity.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RemoveVehicleAtTest()
        {
            garage10Capacity.AddVehicleAt(vehicleMocks[0].Object, 0);
            garage10Capacity.AddVehicleAt(vehicleMocks[9].Object, 9);
            garage10Capacity.AddVehicleAt(vehicleMocks[3].Object, 3);

            garage10Capacity.RemoveVehicleAt(0);
            garage10Capacity.RemoveVehicleAt(9);

            int expected = 1;
            int actual = garage10Capacity.Count;

            Assert.AreEqual(expected, actual);


        }

    }
}
