using Xunit;
using Moq;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using Booking.Domain.Responses;
using Booking.Service.Interfaces;
using Booking.Service.Implementations;
using Booking.Service.Interfaces.Repositories;
using Booking.Service.Implementations.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using Booking.Domain.Enums;
using System.Linq;
using Booking.DAL.Repositories.MsServer;
using System;
using Booking.Service.Implementations.Calculations;
using Booking.Service.Interfaces.Calculations;

namespace TicketBooking.Tests.Integrated
{
    [TestCaseOrderer("TicketBooking.Tests.Integrated.Configuration", "TicketBooking.Tests.Integrated")]
    public class PlaneServiceIntegratedTest
    {
        public static string _stringConnection = @"Data Source=DESKTOP-SEHI244;Initial Catalog=TicketBooking;Integrated Security=True";
        public static Trip _foundTrip = new Trip();
        
        [Fact, TestPriority(0)]
        public async Task CreateTestData()
        {
            try
            {
                // Arrange
                IPlaneService planeService = new PlaneService(new PlaneRepositoryMS(_stringConnection));
                ICountryService countryRepository = new CountryService(new CountryRepositoryMS(_stringConnection));
                ICityService cityService = new CityService(new CityRepositoryMS(_stringConnection));
                ITripService tripService = new TripService(new TripRepositoryMS(_stringConnection));
                ITripDetailService tripDetailService = new TripDetailService(new TripDetailRepositoryMS(_stringConnection),
                    new TripRepositoryMS(_stringConnection), new UserRepositoryMS(_stringConnection));

                var plane = GetPlane();
                var countryName = "countryName";
                string firstCityName = "some1";
                string secondCityName = "some2";

                Trip addTrip = new Trip()
                {
                    StartDate = new DateTime(2022, 11, 20, 9, 00, 00),
                    EndDate = new DateTime(2022, 11, 20, 11, 30, 00),
                    Capacity = 10,
                    Price = 40
                };
                // Act
                await planeService.Create(plane);
                var planeData = await planeService.GetByName(plane.Name);
                await countryRepository.Create(new Country() { Name = countryName });
                var countryData = await countryRepository.GetAll();
                int countryId = countryData.Data.Max(x => x.Id);

                await cityService.Create(new City() { Name = firstCityName, CountryId = countryId });
                await cityService.Create(new City() { Name = secondCityName, CountryId = countryId });

                var foundcity1 = await cityService.GetByCityName(firstCityName);
                var foundcity2 = await cityService.GetByCityName(secondCityName);
                addTrip.StartCityId = foundcity1.Data.Id;
                addTrip.EndCityId = foundcity2.Data.Id;
                addTrip.PlaneId = planeData.Data.Id;

                await tripService.Create(addTrip);
                var trips = await tripService.GetAll();

                _foundTrip = trips.Data.Last();
                await tripDetailService.Create(new TripDetails() { Place = 1, UserId = 3, TripId = _foundTrip.Id });
                await tripDetailService.Create(new TripDetails() { Place = 2, UserId = 3, TripId = _foundTrip.Id });
                await tripDetailService.Create(new TripDetails() { Place = 3, UserId = 3, TripId = _foundTrip.Id });
                await tripDetailService.Create(new TripDetails() { Place = 4, UserId = 3, TripId = _foundTrip.Id });
                await tripDetailService.Create(new TripDetails() { Place = 6, UserId = 3, TripId = _foundTrip.Id });

                Assert.True(true);
            }
            catch (System.Exception)
            {

                Assert.True(false);
            }
        }
        [Fact, TestPriority(1)]
        public async Task TestCalculation()
        {
            // Arrange
            ITripDetailService tripDetailService = new TripDetailService(new TripDetailRepositoryMS(_stringConnection),
                new TripRepositoryMS(_stringConnection), new UserRepositoryMS(_stringConnection));
            ITripTreatmentService tripTreatmentService = new TripTreatmentService();

            // Act
            var data = await tripDetailService.GetDetailsByTripId(_foundTrip.Id);

            var availablePlacesResponse = tripTreatmentService.GetAvailablePlaces(data.Data, _foundTrip.Capacity);
            var aavailablePlaces = availablePlacesResponse.Data;
            // Assert

            Assert.Equal(_foundTrip.Capacity - data.Data.Count(), aavailablePlaces.Count());
        }
        [Fact, TestPriority(2)]
        public async Task TestDeleteAllData()
        {
            try
            {
                // Arrange
                IPlaneService planeService = new PlaneService(new PlaneRepositoryMS(_stringConnection));
                ICountryService countryService = new CountryService(new CountryRepositoryMS(_stringConnection));
                ICityService cityService = new CityService(new CityRepositoryMS(_stringConnection));
                ITripService tripService = new TripService(new TripRepositoryMS(_stringConnection));
                ITripDetailService tripDetailService = new TripDetailService(new TripDetailRepositoryMS(_stringConnection),
                    new TripRepositoryMS(_stringConnection), new UserRepositoryMS(_stringConnection));

                // Act
                var details = await tripDetailService.GetDetailsByTripId(_foundTrip.Id);
                await DeleteTripDetails(details.Data);
                await tripService.DeleteById(_foundTrip.Id);
                await planeService.DeleteById(_foundTrip.PlaneId);

                var country = await cityService.GetById(_foundTrip.StartCityId);

                await cityService.DeleteById(_foundTrip.StartCityId);
                await cityService.DeleteById(_foundTrip.EndCityId);
                await countryService.DeleteById(country.Data.CountryId);

                Assert.True(true);
            }
            catch (Exception)
            {

                Assert.True(false);
            }
        }
        

        private Plane GetPlane()
            => new Plane()
            {
                Name = "TestName",
                Capacity = 10
            };

        private List<TripDetails> GetTripDetails()
        {
            return new List<TripDetails>()
            {
                new TripDetails(){ Place = 1 },
                new TripDetails(){ Place = 3 },
                new TripDetails(){ Place = 5 },
                new TripDetails(){ Place = 6 },
            };
        }
        private async Task DeleteTripDetails(List<TripDetails> tripDetails)
        {
            ITripDetailService tripDetailService = new TripDetailService(new TripDetailRepositoryMS(_stringConnection),
                new TripRepositoryMS(_stringConnection), new UserRepositoryMS(_stringConnection));
            foreach (var item in tripDetails)
            {
                await tripDetailService.DeleteById(item.Id);
            }
        }
    }
}