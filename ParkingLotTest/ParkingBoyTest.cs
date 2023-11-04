﻿using ParkingLotProj.ErrorHandling;
using ParkingLotProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParkingLotTest
{
    public class ParkingBoyTest
    {
        [Fact]
        public void Should_Get_Same_Car_When_Fetch_By_Ticket()
        {
            ParkingLot parkingLot = new ParkingLot();
            ParkingBoy parkingBoy = new ParkingBoy(parkingLot);

            string ticket = parkingBoy.Park("car");
            string car = parkingBoy.Fetch(ticket);

            Assert.Equal("car", car);
        }

        [Fact]
        public void Should_Get_Correct_Car_When_Fetch_By_Corresponding_Ticket()
        {
            string car1 = "car1";
            string car2 = "car2";
            ParkingLot parkingLot = new ParkingLot();
            ParkingBoy parkingBoy = new ParkingBoy(parkingLot);
            string ticket1 = parkingBoy.Park(car1);
            string ticket2 = parkingBoy.Park(car2);

            string actualCar1 = parkingBoy.Fetch(ticket1);
            string actualCar2 = parkingBoy.Fetch(ticket2);

            Assert.Equal(car1, actualCar1);
            Assert.Equal(car2, actualCar2);
        }

        [Fact]
        public void Should_Not_Return_Car_When_Fetch_With_Wrong_Ticket()
        {
            ParkingLot parkingLot = new ParkingLot();
            ParkingBoy parkingBoy = new ParkingBoy(parkingLot);
            string ticket1 = parkingBoy.Park("car1");

            InvalidTicketException exception = Assert.Throws<InvalidTicketException>(() => parkingBoy.Fetch("wrongTicketId"));
            Assert.Equal("Unrecognized parking ticket.", exception.Message);
        }

        [Fact]
        public void Should_Not_Return_Car_When_Fetch_With_Used_Ticket()
        {
            ParkingLot parkingLot = new ParkingLot();
            ParkingBoy parkingBoy = new ParkingBoy(parkingLot);
            string ticket1 = parkingBoy.Park("car1");

            string actualCar1 = parkingBoy.Fetch(ticket1);

            InvalidTicketException exception = Assert.Throws<InvalidTicketException>(() => parkingBoy.Fetch(ticket1));
            Assert.Equal("Unrecognized parking ticket.", exception.Message);
        }

        [Fact]
        public void Should_Not_Allow_Parking_When_ParkingLot_Full()
        {
            ParkingLot parkingLot = new ParkingLot();
            ParkingBoy parkingBoy = new ParkingBoy(parkingLot);
            for (int i = 0; i < 10; i++)
            {
                parkingBoy.Park($"{i}");
            }

            OutOfCapacityException exception = Assert.Throws<OutOfCapacityException>(() => parkingBoy.Park("car1"));
            Assert.Equal("No available position.", exception.Message);
        }

        [Fact]
        public void Should_Not_Allow_Parking_When_Park_a_Parked_Car()
        {
            ParkingLot parkingLot = new ParkingLot();
            ParkingBoy parkingBoy = new ParkingBoy(parkingLot);

            string ticket1 = parkingBoy.Park("car1");
            string actualResult = parkingBoy.Park("car1");

            Assert.Null(actualResult);
        }

        [Fact]
        public void Should_Park_To_First_Available_ParkingLot_Given_Two_ParkingLot()
        {
            ParkingLot parkingLot1 = new ParkingLot();
            ParkingLot parkingLot2 = new ParkingLot();

            ParkingBoy parkingBoy = new ParkingBoy(parkingLot1);
            parkingBoy.AddManageParkingLot(parkingLot2);

            string ticket1 = parkingBoy.Park("car1");

            Assert.Equal(1, parkingLot1.CurrentCapacity);
        }
    }
}
