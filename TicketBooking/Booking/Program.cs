using Booking.DAL.Repositories.MongoDB;
using MongoDB.Driver;
using Booking.Domain.ViewModels.Account;
using Booking.Service.Interfaces;
using Booking.Service.Implementations;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using Booking.Domain.Helpers;
using Booking.DAL.Repositories.MsServer;
using Booking.Service.Interfaces.Repositories;
using Booking.Service.Implementations.Repositories;
using Booking.Service.Interfaces.Accounts;
using Booking.Service.Implementations.Accounts;

namespace Booking
{
    public class Program
    {
        public static async Task Main()
        {
            IUserRepository userRepository = new UserRepository("mongodb://localhost:27017");

            /*            var user = new RegisterViewModel()
                        {
                            UserName = "Mebry",
                            Password = "pass123",
                            PasswordConfirm = "pass123",
                            Codeword = "something"
                        };

                        await accountServer.Register(user);*/
            /*            await accountServer.ChangePassword(new ChangePasswordViewModel()
                        { 
                            Codeword= "something",
                            UserName="Mebry",
                            OldPassword= "admin2",
                            NewPassword="admin"
                        });*/

            //var res=await accountServer.Login(new LoginViewModel() { Password = "admin", UserName = "Mebry" });

            //Console.WriteLine(res.Data);
            /*await userService.Update(new User()
            {
                Id = 1,
                UserName ="Admin",
                Password ="Admin",
                Codeword ="Data"
            });
            var findUser = await userRepository.GetById(2);

            Console.WriteLine( findUser.Password);

            Console.WriteLine(HashPasswordHelper.HashPassowrd("admin"));*/

            string path = "mongodb://localhost:27017";

            /*await AddPlaneAsync(new PlaneRepository(path));
            await AddPlaneAsync2(new PlaneRepository(path));
            await GetByIdPlane(new PlaneRepository(path), 1);
            await UpdatePlane(new PlaneRepository(path), 1);
            await GetAllPlane(new PlaneRepository(path));*/
            //await CreateCity(new CityRepository(path));
            //await GellAllCities(new CountryRepository(path), 2);
            //await DeleteCityById(new CityRepository(path), 6);
            //await GetCityById(new CityRepository(path), 16);
            //await GellAllCities2(new CityRepository(path));
            //await UpdateCity(new CityRepository(path));
            //await CreateUserDelailsCity(new UserDelailsRepository(path));
            //await DeleteUserDelailsById(new UserDelailsRepository(path),2);
            /*await AddRole(new RoleRepository(path), "Client");
            await AddRole(new RoleRepository(path), "Dispatcher");
            await AddRole(new RoleRepository(path), "Admin");
            await AddRole(new RoleRepository(path), "Owner");*/
            //await GetUserRoleByUserName(new UserRoleRepository(path), "Mebry");
            //await AddUserRole(new UserRoleRepository(path), 1, 2);
            /*await AddUserRole(new UserRoleRepository(path), 1, 3);
            await AddUserRole(new UserRoleRepository(path), 2, 1);*/
            //await AddUserRole(new UserRoleRepository(path), 2, 3);
            //await AddCountry(new CountryRepository(path));
            //await CreateCity(new CityRepository(path));
            string _connetionString = @"Data Source=DESKTOP-SEHI244;Initial Catalog=TicketBooking;Integrated Security=True";
            UserRepositoryMS userRepositoryMS = new UserRepositoryMS(_connetionString);

            //await AddNewUser(userRepositoryMS);
            //await GetUserById(userRepositoryMS,1);
            //await AddTrip(new TripRepository(path));
            //await AddNewUser(new UserRepository(path), new UserRoleRepository(path));
            //await AddTripDetails(new TripDetailRepository(path));
            //await GetUserTripDetails(new TripRepository(path),1);
            //await GetDetailsByTripId(new TripDetailRepository(path), 3);
            //await GetPlannedTripsByUser(new TripInformationRepository(path), 2);
            //await GetExecutedTripsByUser(new TripInformationRepository(path), 2);
            //await GetInformationByDate(new TripInformationRepository(path), 11);
            //await GetInformationByTripsInside(new TripInformationRepository(path));
            //await AddUserDetailsById(new UserDetailsRepositoryMS(path), 3);
            //await AddNewUser(new UserRepositoryMS(_connetionString), new UserRoleRepositoryMS(_connetionString));
            //await updateUserDetailsById(new UserDelailsRepository(path), 1);
            //await GetUserDetailsById(new UserDelailsRepository(path), 1);
            //await AddPlaneAsync(new PlaneRepositoryMS(_connetionString));
            //await GetAllPlane(new PlaneRepositoryMS(_connetionString));
            //await UpdatePlane(new PlaneRepositoryMS(_connetionString),2);
            //await DeleteByIdPlane(new PlaneRepositoryMS(_connetionString),1);
            /*await AddRole(new RoleRepositoryMS(_connetionString), "Client");
            await AddRole(new RoleRepositoryMS(_connetionString), "Dispatcher");
            await AddRole(new RoleRepositoryMS(_connetionString), "Admin");
            await AddRole(new RoleRepositoryMS(_connetionString), "Owner");*/
            //await GetInformationByTrip(new TripInformationRepository(path), 10);

            //await DeleteUserById(new UserRepositoryMS(_connetionString), 1);
            //await DeleteUserById(new UserRepositoryMS(_connetionString), 2);
            //await AddCountry(new CountryRepositoryMS(_connetionString), "Belarus");
            //await AddCountry(new CountryRepositoryMS(_connetionString), "Russia");
            //await AddCountry(new CountryRepositoryMS(_connetionString), "Ukrain");
            //await CreateCity(new CityRepositoryMS(_connetionString), 3,"Kiev");
            //await CreateCity(new CityRepositoryMS(_connetionString),1, "Gomel");
            //await CreateCity(new CityRepositoryMS(_connetionString),1, "Minsk");
            //await CreateCity(new CityRepositoryMS(_connetionString),2, "Moscow");
            //await CreateCity(new CityRepositoryMS(_connetionString),2, "Voronez");
            //await AddTrip(new TripRepositoryMS(_connetionString));
            //await DeleteTripById(new TripRepositoryMS(_connetionString),6);
            //await AddTripDetails(new TripDetailRepositoryMS(_connetionString));
            //await GetInformationByTrip(new TripInformationRepositoryMS(_connetionString),10);
            //await GetTripInfoByUserId(new TripInformationRepositoryMS(_connetionString),3);
            //await GetTripInfoByUserId(new TripInformationRepository(path),3);
            //await AddNewUser(new UserRepositoryMS(_connetionString), new UserRoleRepositoryMS(_connetionString));
            //await GetInformationByTripsInside(new TripInformationRepositoryMS(_connetionString));
            await CreateDeletedUser(new DeletedUserRepository(path), new DeletedUser()
            {
                DateTime = new DateTime(),
                UserId = 3,
                Reason = "test"
            });
        }
        public static async Task CreateDeletedUser(IDeletedUserRepository deletedUserRepository, DeletedUser deletedUser)
        {
            await deletedUserRepository.Create(deletedUser);
        }
        public static async Task DeleteTripById(ITripRepository trip, int id)
        {
            await trip.Delete(id);
        }

        public static async Task GetPlannedTripInfoByUserId(ITripInformationRepository tripInformationRepository, int id)
        {
            var data = await tripInformationRepository.GetPlannedTripsByUser(id);
            Console.WriteLine();
        }
        public static async Task GetTripInfoByUserId(ITripInformationRepository tripInformationRepository,int id)
        {
            var data=await tripInformationRepository.GetTripsByUserId(id);
            Console.WriteLine();
        }

        public static async Task DeleteUserById(IUserRepository userRepository, int id)
        {
            IUserService userService= new UserService(userRepository);

            await userRepository.Delete(id);

        }


        public static async Task updateUserDetailsById(IUserDetailsRepository userRepository, int id)
        {
            IUserDetailsService userService = new UserDetailsService(userRepository);

            await userService.Update(new UserDelails()
            {
                UserId = id,
                FirstName = "Vadim",
                LastName = "Goncharov",
                Patronymic = "Sergeevich",
                YearOfBirth = 2003
            });
            Console.WriteLine();
        }


        public static async Task GetUserDetailsById(IUserDetailsRepository userRepository, int id)
        {
            IUserDetailsService userService = new UserDetailsService(userRepository);

            var data = await userService.GetByUserId(id);
            Console.WriteLine();
        }
        public static async Task AddUserDetailsById(IUserDetailsRepository userRepository, int id)
        {
            IUserDetailsService userService = new UserDetailsService(userRepository);

            var data = await userService.Create(new UserDelails()
            {
                UserId = id,
                FirstName = "Masha",
                LastName = "Leonove",
                Patronymic= "Aaa",
                YearOfBirth = 2003
            });
            Console.WriteLine();
        }


        public static async Task GetInformationByTripsInside(ITripInformationRepository tripRepository)
        {
            ITripInformationService tripInformationService = new TripInformationService(tripRepository,null,null);
            var data = await tripInformationService.GetInformationByTripsInside();

            Console.WriteLine();
        }
        public static async Task GetInformationByDate(ITripInformationRepository tripRepository, int count)
        {
            var data = await tripRepository.GetInformationByDate(count);

            Console.WriteLine();
        }


        public static async Task GetInformationByTrip(ITripInformationRepository tripRepository, int id)
        {
            var data = await tripRepository.GetInformationByTrip(id);

            Console.WriteLine();
        }


        public static async Task GetExecutedTripsByUser(ITripInformationRepository tripRepository, int id)
        {
            var data = await tripRepository.GetExecutedTripsByUser(id);

            Console.WriteLine();
        }
        public static async Task GetPlannedTripsByUser(ITripInformationRepository tripRepository, int id)
        {
            var data = await tripRepository.GetPlannedTripsByUser(id);

            Console.WriteLine();
        }
        public static async Task GetDetailsByTripId(ITripDetailRepository tripDetailRepository, int id)
        {
            var data = await tripDetailRepository.GetDetailsByTripId(id);

            Console.WriteLine();
        }
        public static async Task GetUserTripDetails(ITripInformationRepository tripRepository, int id)
        {
            var data = await tripRepository.GetTripsByUserId(id);
            Console.WriteLine();
        }
        public static async Task AddTrip(ITripRepository tripRepository)
        {
            await tripRepository.Create(new Trip()
            {
                PlaneId = 3,
                StartCityId = 1,
                EndCityId = 5,
                StartDate = new DateTime(2022, 11, 20, 9, 00, 00),
                EndDate = new DateTime(2022, 11, 20, 11, 30, 00),
                Capacity = 130,
                Price = 40
            });
        }

        public static async Task AddTripDetails(ITripDetailRepository tripDetailRepository)
        {
            await tripDetailRepository.Create(
                new TripDetails()
                {
                    TripId = 7,
                    UserId = 3,
                    Place = 1
                });
        }

        public static async Task AddNewUser(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            IAccountService userService = new AccountService(userRepository, userRoleRepository);

            await userService.Register(new RegisterViewModel()
            {
                UserName = "admin",
                Codeword = "admin",
                Password = "admin",
                PasswordConfirm = "admin"
            });
        }

        public static async Task GetUserById(IUserRepository userRepository, int id)
        {
            IUserService userService = new UserService(userRepository);

            var data = await userService.GetById(id);
            Console.WriteLine();
        }

        public static async Task AddPlaneAsync(IPlaneRepository planeRepository)
        {
            IPlaneService planeService = new PlaneService(planeRepository);

            await planeService.Create(new Plane()
            {
                Name = "Ty-154",
                Capacity = 150
            });
        }

        public static async Task AddPlaneAsync2(IPlaneRepository planeRepository)
        {
            IPlaneService planeService = new PlaneService(planeRepository);

            await planeService.Create(new Plane()
            {
                Name = "Ty-154",
                Capacity = 150
            });
        }

        public static async Task GetByIdPlane(IPlaneRepository planeRepository, int id)
        {
            IPlaneService planeService = new PlaneService(planeRepository);

            var data = await planeService.GetById(id);
            var plane = data.Data;
            Console.WriteLine(plane.Capacity + " " + plane.Name);
        }

        public static async Task GetAllPlane(IPlaneRepository planeRepository)
        {
            IPlaneService planeService = new PlaneService(planeRepository);

            var data = await planeService.GetAll();
            var plane = data.Data;
            Console.WriteLine(plane.Count);
        }

        public static async Task DeleteByIdPlane(IPlaneRepository planeRepository, int id)
        {
            IPlaneService planeService = new PlaneService(planeRepository);

            var plane = await planeService.DeleteById(id);
        }

        public static async Task UpdatePlane(IPlaneRepository planeRepository, int id)
        {
            IPlaneService planeService = new PlaneService(planeRepository);

            var data = await planeService.Update(new Plane()
            {
                Id = id,
                Name = "Ty-134",
                Capacity = 87
            });
        }

        public static async Task GellAllCities(ICountryRepository countryRepository, int id)
        {
            //ICountryRepository countryRepository = new PlaneService(countryRepository);
            var data = await countryRepository.GetAllCities(id);
            data.ForEach(x => Console.WriteLine(x.Name + " " + x.Id + " " + x.CountryId));
        }


        public static async Task GetCityById(ICityRepository cityRepository, int id)
        {
            var data = await cityRepository.GetById(id);
            if (data == null)
                return;
            Console.WriteLine(data.Name);

        }

        public static async Task DeleteCityById(ICityRepository cityRepository, int id)
        {
            await cityRepository.Delete(id);

        }


        public static async Task GellAllCities2(ICityRepository cityRepository)
        {
            //ICountryRepository countryRepository = new PlaneService(countryRepository);
            var data = await cityRepository.GetAll();
            data.ForEach(x => Console.WriteLine(x.Name + " " + x.Id + " " + x.CountryId));
        }



        public static async Task UpdateCity(ICityRepository cityRepository)
        {
            //ICountryRepository countryRepository = new PlaneService(countryRepository);
            await cityRepository.Update(new City() { Name = "Moscow", Id = 7, CountryId = 2 });
            //data.ForEach(x => Console.WriteLine(x.Name + " " + x.Id + " " + x.CountryId));
        }

        public static async Task CreateUserDelailsCity(IBaseRepository<UserDelails> baseRepository)
        {
            //ICountryRepository countryRepository = new PlaneService(countryRepository);
            await baseRepository.Create(new UserDelails()
            {
                UserId = 1,
                FirstName = "Vadim",
                LastName = "Goncharov",
                Patronymic = "Sergeevich"
            });
            //data.ForEach(x => Console.WriteLine(x.Name + " " + x.Id + " " + x.CountryId));
        }


        public static async Task DeleteUserDeta(IBaseRepository<UserDelails> baseRepository)
        {
            //ICountryRepository countryRepository = new PlaneService(countryRepository);
            await baseRepository.Delete(1);
            //data.ForEach(x => Console.WriteLine(x.Name + " " + x.Id + " " + x.CountryId));
        }
        public static async Task DeleteUserDelailsById(IUserDetailsRepository userDetailsRepository, int id)
        {
            IUserDetailsService userDetailsService = new UserDetailsService(userDetailsRepository);

            await userDetailsService.UpdateFirstName(id, "Vadim");
            //await userDetailsService.UpdateLastName(id, "Vadim");
            //await userDetailsService.UpdatePatronymic(id, "Vadim");
            await userDetailsService.DeleteByUserId(id);
        }

        public static async Task AddRole(IRoleRepository roleRepository, string name)
        {
            IRoleService roleService = new RoleService(roleRepository);

            await roleService.Create(new Role() { Name = name });
        }

        public static async Task AddUserRole(IUserRoleRepository userRoleRepository, int userId, int roleId)
        {
            await userRoleRepository.Create(new UserRole() { RoleId = roleId, UserId = userId });
        }
        public static async Task GetUserRoleByUserName(IUserRoleRepository userRoleRepository, string username)
        {
            var data = await userRoleRepository.GetByUserName(username);

        }

        public static async Task AddCountry(ICountryRepository countryRepository, string name)
        {
            await countryRepository.Create(new Country() { Name = name });
        }

        public static async Task CreateCity(ICityRepository cityRepository,int countryId,string name)
        {
            //ICountryRepository countryRepository = new PlaneService(countryRepository);
            await cityRepository.Create(new City() { CountryId = countryId, Name = name });
        }

    }
}