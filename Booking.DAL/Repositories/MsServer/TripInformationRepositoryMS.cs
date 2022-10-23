using System;
using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System.Data.SqlClient;
using Booking.Domain.ViewModels.Trip;

namespace Booking.DAL.Repositories.MsServer
{
    public class TripInformationRepositoryMS : ITripInformationRepository
    {
        private readonly string _connectionString;

        public TripInformationRepositoryMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<UserTripViewModel>> GetExecutedTripsByUser(int userId)
        {
            string sql = @$"Select Trips.Id As TripId,startCity.Name AS StartCity,endCities.Name AS EndCity,
                         startCountry.Name AS StartCountry,endCountry.Name AS EndCountry, TripDetails.Place AS Place, 
                         Trips.StartDate AS startDate,Trips.EndDate AS endDate
                         FROM Trips,Cities As startCity,Cities As endCities,Countries as startCountry, 
                         Countries as endCountry,TripDetails
                         WHERE Trips.StartCityId=startCity.Id AND Trips.EndCityId=endCities.Id
                         AND startCountry.Id=startCity.CountryId AND endCountry.Id= endCities.CountryId
                         AND TripDetails.TripId=Trips.Id AND TripDetails.UserId=@userId
                         AND DATEDIFF(MINUTE,Trips.StartDate,GETDATE())>0";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@userId", SqlDbType.Int).Value = userId;

                    return await GetUserTrip(sqlCommand);
                }
            }
        }

        public async Task<List<TripInfo>> GetInformationByDate(int countDays)
        {
            string sql = @$"Select Trips.Id As TripId,startCity.Name AS StartCity,endCities.Name AS EndCity,
                            startCountry.Name AS StartCountry,endCountry.Name AS EndCountry, tripDet.countPlaces AS NumberOfOrderedTickets,
                            Trips.StartDate AS StartDate,Trips.EndDate AS EndDate, Trips.Capacity AS AllTictets, (tripDet.countPlaces*Trips.Price) AS Profit
                            FROM Trips,Cities As startCity,Cities As endCities,Countries as startCountry, Countries as endCountry,
	                            (SELECT TripDetails.TripId, Count(TripDetails.Place) as countPlaces
	                            FROM TripDetails
	                            GROUP BY TripDetails.TripId) AS tripDet
                            WHERE Trips.StartCityId=startCity.Id AND Trips.EndCityId=endCities.Id
                            AND startCountry.Id=startCity.CountryId AND endCountry.Id= endCities.CountryId
                            AND tripDet.TripId=Trips.Id
                            AND DATEDIFF(DAY,Trips.StartDate,GETDATE())>0 AND DATEDIFF(DAY,Trips.StartDate,DATEADD(day,-@countDays,GETDATE()))<0";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@countDays", SqlDbType.Int).Value = countDays;
                    var data = await GetTripInfo(sqlCommand);

                    return data;
                }
            }
        }

        public async Task<TripInfo> GetInformationByTrip(int tripId)
        {
            string sql = @$"Select Trips.Id As TripId,startCity.Name AS StartCity,endCities.Name AS EndCity,
                            startCountry.Name AS StartCountry,endCountry.Name AS EndCountry, tripDet.countPlaces AS NumberOfOrderedTickets,
                            Trips.StartDate AS StartDate,Trips.EndDate AS EndDate, Trips.Capacity AS AllTictets, (tripDet.countPlaces*Trips.Price) AS Profit
                            FROM Trips,Cities As startCity,Cities As endCities,Countries as startCountry, Countries as endCountry,
	                            (SELECT TripDetails.TripId, Count(TripDetails.Place) as countPlaces
	                            FROM TripDetails
	                            GROUP BY TripDetails.TripId) AS tripDet
                            WHERE Trips.Id=@tripId AND Trips.StartCityId=startCity.Id AND Trips.EndCityId=endCities.Id
                            AND startCountry.Id=startCity.CountryId AND endCountry.Id= endCities.CountryId
                            AND tripDet.TripId=Trips.Id
                            AND DATEDIFF(MINUTE,Trips.StartDate,GETDATE())>0";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@tripId", SqlDbType.Int).Value = tripId;
                    var data = await GetTripInfo(sqlCommand);

                    return data?[0];
                }
            }
        }

        public async Task<List<TripInfo>> GetInformationByTrips()
        {
            string sql = @$"Select Trips.Id As TripId,startCity.Name AS StartCity,endCities.Name AS EndCity,
                            startCountry.Name AS StartCountry,endCountry.Name AS EndCountry, tripDet.countPlaces AS NumberOfOrderedTickets,
                            Trips.StartDate AS StartDate,Trips.EndDate AS EndDate, Trips.Capacity AS AllTictets, (tripDet.countPlaces*Trips.Price) AS Profit
                            FROM Trips,Cities As startCity,Cities As endCities,Countries as startCountry, Countries as endCountry,
	                            (SELECT TripDetails.TripId, Count(TripDetails.Place) as countPlaces
	                            FROM TripDetails
	                            GROUP BY TripDetails.TripId) AS tripDet
                            WHERE Trips.StartCityId=startCity.Id AND Trips.EndCityId=endCities.Id
                            AND startCountry.Id=startCity.CountryId AND endCountry.Id= endCities.CountryId
                            AND tripDet.TripId=Trips.Id
                            AND DATEDIFF(MINUTE,Trips.StartDate,GETDATE())>0";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    return await GetTripInfo(sqlCommand);
                }
            }
        }

        public async Task<List<TripInfo>> GetInformationByTripsInside()
        {
            string sql = @$"Select Trips.Id As TripId,startCity.Name AS StartCity,endCities.Name AS EndCity,
                            startCountry.Name AS StartCountry,endCountry.Name AS EndCountry, tripDet.countPlaces AS NumberOfOrderedTickets,
                            Trips.StartDate AS StartDate,Trips.EndDate AS EndDate, Trips.Capacity AS AllTictets, (tripDet.countPlaces*Trips.Price) AS Profit
                            FROM Trips,Cities As startCity,Cities As endCities,Countries as startCountry, Countries as endCountry,
	                            (SELECT TripDetails.TripId, Count(TripDetails.Place) as countPlaces
	                            FROM TripDetails
	                            GROUP BY TripDetails.TripId) AS tripDet
                            WHERE Trips.StartCityId=startCity.Id AND Trips.EndCityId=endCities.Id
                            AND startCountry.Id=startCity.CountryId AND endCountry.Id= endCities.CountryId
                            AND tripDet.TripId=Trips.Id AND startCountry.Name='Belarus' AND endCountry.Name='Belarus'";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    return await GetTripInfo(sqlCommand);
                }
            }
        }

        public async Task<List<TripInfo>> GetInformationByTripsOutside()
        {
            string sql = @$"Select Trips.Id As TripId,startCity.Name AS StartCity,endCities.Name AS EndCity,
                            startCountry.Name AS StartCountry,endCountry.Name AS EndCountry, tripDet.countPlaces AS NumberOfOrderedTickets,
                            Trips.StartDate AS StartDate,Trips.EndDate AS EndDate, Trips.Capacity AS AllTictets, (tripDet.countPlaces*Trips.Price) AS Profit
                            FROM Trips,Cities As startCity,Cities As endCities,Countries as startCountry, Countries as endCountry,
	                            (SELECT TripDetails.TripId, Count(TripDetails.Place) as countPlaces
	                            FROM TripDetails
	                            GROUP BY TripDetails.TripId) AS tripDet
                            WHERE Trips.StartCityId=startCity.Id AND Trips.EndCityId=endCities.Id
                            AND startCountry.Id=startCity.CountryId AND endCountry.Id= endCities.CountryId
                            AND tripDet.TripId=Trips.Id AND startCountry.Name!='Belarus' AND endCountry.Name!='Belarus'";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    return await GetTripInfo(sqlCommand);
                }
            }
        }

        public async Task<List<UserTripViewModel>> GetPlannedTripsByUser(int userId)
        {
            string sql = @$"Select Trips.Id As TripId,startCity.Name AS StartCity,endCities.Name AS EndCity,
                         startCountry.Name AS StartCountry,endCountry.Name AS EndCountry, TripDetails.Place AS Place, 
                         Trips.StartDate AS startDate,Trips.EndDate AS endDate
                         FROM Trips,Cities As startCity,Cities As endCities,Countries as startCountry, 
                         Countries as endCountry,TripDetails
                         WHERE Trips.StartCityId=startCity.Id AND Trips.EndCityId=endCities.Id
                         AND startCountry.Id=startCity.CountryId AND endCountry.Id= endCities.CountryId
                         AND TripDetails.TripId=Trips.Id AND TripDetails.UserId=@userId
                         AND DATEDIFF(MINUTE,Trips.StartDate,GETDATE())<0";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@userId", SqlDbType.Int).Value = userId;

                    return await GetUserTrip(sqlCommand);
                }
            }
        }

        public async Task<List<UserTripViewModel>> GetTripsByUserId(int id)
        {
            string sql = @$"Select Trips.Id As TripId,startCity.Name AS StartCity,endCities.Name AS EndCity,
                         startCountry.Name AS StartCountry,endCountry.Name AS EndCountry, TripDetails.Place AS Place, 
                         Trips.StartDate AS startDate,Trips.EndDate AS endDate
                         FROM Trips,Cities As startCity,Cities As endCities,Countries as startCountry, 
                         Countries as endCountry,TripDetails
                         WHERE Trips.StartCityId=startCity.Id AND Trips.EndCityId=endCities.Id
                         AND startCountry.Id=startCity.CountryId AND endCountry.Id= endCities.CountryId
                         AND TripDetails.TripId=Trips.Id AND TripDetails.UserId=@userId";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@userId", SqlDbType.Int).Value = id;

                    return await GetUserTrip(sqlCommand);
                }
            }
        }

        private async Task<List<UserTripViewModel>> GetUserTrip(SqlCommand sqlCommand)
        {
            List<UserTripViewModel> data = new List<UserTripViewModel>();

            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
            {
                while (await sqlDataReader.ReadAsync())
                {
                    data.Add(new UserTripViewModel()
                    {
                        TripId = (int)sqlDataReader["TripId"],
                        StartCity = sqlDataReader["StartCity"] as string ?? "Undefined",
                        EndCity = sqlDataReader["EndCity"] as string ?? "Undefined",
                        StartCountry = sqlDataReader["StartCountry"] as string ?? "Undefined",
                        EndCountry = sqlDataReader["EndCountry"] as string ?? "Undefined",
                        Place = (int)sqlDataReader["Place"],
                        StartDate = (DateTime)sqlDataReader["StartDate"],
                        EndDate = (DateTime)sqlDataReader["EndDate"],

                    }); ;
                }
                if (data.Count > 0)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<List<TripInfo>> GetTripInfo(SqlCommand sqlCommand)
        {
            List<TripInfo> data = new List<TripInfo>();

            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
            {
                while (await sqlDataReader.ReadAsync())
                {
                    data.Add(new TripInfo()
                    {
                        TripId = (int)sqlDataReader["TripId"],
                        StartCity = sqlDataReader["StartCity"] as string ?? "Undefined",
                        EndCity = sqlDataReader["EndCity"] as string ?? "Undefined",
                        StartCountry = sqlDataReader["StartCountry"] as string ?? "Undefined",
                        EndCountry = sqlDataReader["EndCountry"] as string ?? "Undefined",
                        Profit = (int)sqlDataReader["Profit"],
                        AllTictets = (int)sqlDataReader["AllTictets"],
                        NumberOfOrderedTickets = (int)sqlDataReader["NumberOfOrderedTickets"],
                        StartDate = (DateTime)sqlDataReader["StartDate"],
                        EndDate = (DateTime)sqlDataReader["EndDate"],

                    });
                }
                if (data.Count > 0)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
