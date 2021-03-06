﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {

        internal static List<USState> GetStates()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            List<USState> allStates = db.USStates.ToList();

            return allStates;
        }

        internal static Client GetClient(string userName, string password)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.AddressLine2 = null;
                newAddress.Zipcode = zipCode;
                newAddress.USStateId = stateId;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            // find corresponding Client from Db
            Client clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.AddressLine2 = null;
                newAddress.Zipcode = clientAddress.Zipcode;
                newAddress.USStateId = clientAddress.USStateId;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;

            // submit changes
            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).Single();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }

        internal static void AddUsernameAndPassword(Employee employee)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        // Link for employee -https://www.tutorialspoint.com/linq/linq_sql.htm
        internal static void RunEmployeeQueries(Employee employee, string input)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            switch (input)
            {
                case "create":
                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();
                    break;
                case "delete":
                    var deleteEmployee = db.Employees.Where(d => d.LastName == employee.LastName && d.EmployeeNumber == employee.EmployeeNumber).Single();
                    db.Employees.DeleteOnSubmit(deleteEmployee);
                    db.SubmitChanges();
                    break;
                case "read":
                    var employeeList = db.Employees.Where(r => r.EmployeeNumber == employee.EmployeeNumber).ToList();
                    {
                        foreach (Employee list in employeeList)
                        {
                            Console.WriteLine($"Employee ID: {list.EmployeeNumber}\nEmployee Name: { list.FirstName} {list.LastName}\nEmail: {list.Email}");
                            Console.ReadKey();
                        }
                    }
                    break;
                case "update":
                    Employee updateEmployee = db.Employees.FirstOrDefault(u => u.EmployeeNumber == employee.EmployeeNumber);
                    updateEmployee.FirstName = employee.FirstName;
                    updateEmployee.LastName = employee.LastName;
                    updateEmployee.Email = employee.Email;
                    db.SubmitChanges();
                    break;
                default:
                    break;
            }
        }
        internal static Animal GetAnimalByID(int iD)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            //return db.Animals.Where(a => a.AnimalId == animalID).Select(a => a.AnimalId).Single();

            var animals = (from i in db.Animals where i.AnimalId.Equals(iD) select i).SingleOrDefault();
            return animals;
        }
        internal static void Adopt(Animal animal, Client client)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Adoption adoption = new Adoption();
            adoption.AnimalId = animal.AnimalId;
            adoption.ClientId = client.ClientId;
            adoption.ApprovalStatus = "PENDING";
            adoption.AdoptionFee = 75;
            db.Adoptions.InsertOnSubmit(adoption);

            var statusUpdate = db.Animals.Where(a => a.AnimalId == animal.AnimalId).First();
            statusUpdate.AdoptionStatus = "PENDING";
            db.SubmitChanges();
        }
        internal static List<Animal> SearchForAnimalByMultipleTraits()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            //Can put in UI??
            Console.WriteLine("What criterion would you like to search by? Enter the corresponding numbers, seperated by a space.");
            Console.WriteLine("Enter [1] to search by the animal's name");
            Console.WriteLine("Enter [2] to search by the animal's gender");
            Console.WriteLine("Enter [3] to search by the animal's age");
            Console.WriteLine("Enter [4] to search by the animal's demeanor");
            Console.WriteLine("Enter [5] to search by the type of animal");
            Console.WriteLine("Enter [6] to search for an animal that is kid friendly");
            Console.WriteLine("Enter [7] to search for an animal that is pet friendly");

            List<string> userChoice = Console.ReadLine().Split(' ').ToList();
            var animalsFromDb = db.Animals.ToList();

            foreach (string u in userChoice)
            {
                int searchCriteria = int.Parse(u);

                switch (searchCriteria)
                {

                    case 1:
                        Console.WriteLine("Enter the name of the animal you wish to search: ");
                        var animalNameChoice = Console.ReadLine();

                        var refinedNameSearch = from animal in animalsFromDb
                                                where animal.Name == animalNameChoice
                                                select animal;

                        animalsFromDb = refinedNameSearch.ToList();
                        break;

                    case 2:
                        Console.WriteLine("Enter the gender of the animal you wish to search: [M] or [F] ");
                        var animalGenderChoice = Console.ReadLine().ToUpper();

                        var refinedGenderSearch = from animal in animalsFromDb
                                                  where animal.Gender == animalGenderChoice
                                                  select animal;

                        animalsFromDb = refinedGenderSearch.ToList();
                        break;

                    //TERNARY option??
                    //var genderToSearchBy = animalGenderChoice == "M" ? SearchByMale() : SearchByFemale();
                    //create methods for male and female

                    case 3:
                        Console.WriteLine("Enter the age of the animal you wish to search: ");
                        var animalAgeChoice = int.Parse(Console.ReadLine());

                        var refinedAgeSearch = from animal in animalsFromDb
                                               where animal.Age == animalAgeChoice
                                               select animal;

                        animalsFromDb = refinedAgeSearch.ToList();
                        break;

                    case 4:
                        Console.WriteLine("Enter the demeanor of the animal you wish to search: ");
                        var animalDemeanorChoice = Console.ReadLine();

                        var refinedDemeanorSearch = from animal in animalsFromDb
                                                    where animal.Demeanor == animalDemeanorChoice
                                                    select animal;

                        animalsFromDb = refinedDemeanorSearch.ToList();
                        break;

                    case 5:
                        //Might have to list options by categoryID 

                        Console.WriteLine("Enter the type of animal you wish to search: ");
                        var animalTypeChoice = Console.ReadLine();

                        var refinedTypeSearch = from animal in animalsFromDb
                                                where animal.Category.Name == animalTypeChoice
                                                select animal;

                        animalsFromDb = refinedTypeSearch.ToList();
                        break;

                    case 6:
                        Console.WriteLine("Do you want your search to include kid friendly animals?: [Y] or [N] ");
                        bool? kidFriendlyChoice = (Console.ReadLine().ToUpper() == "Y");

                        var refinedKidFriendlySearch = from animal in animalsFromDb
                                                       where animal.KidFriendly == kidFriendlyChoice
                                                       select animal;

                        animalsFromDb = refinedKidFriendlySearch.ToList();
                        //db.Animals.Where(a => a.KidFriendly == true);
                        break;

                    case 7:
                        Console.WriteLine("Do you want your search to include pet friendly animals?: [Y] or [N] ");
                        bool? petFriendlyChoice = (Console.ReadLine().ToUpper() == "Y");

                        var refinedPetFriendlySearch = from animal in animalsFromDb
                                                       where animal.PetFriendly == petFriendlyChoice
                                                       select animal;

                        animalsFromDb = refinedPetFriendlySearch.ToList();
                        break;

                    default:
                        Console.WriteLine("Couldn't process your request. Please enter a valid criteria to search by.");
                        SearchForAnimalByMultipleTraits();
                        break;
                }

            }

            return animalsFromDb;
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var adoption = db.Adoptions.Where(a => a.ApprovalStatus == "PENDING");
            return adoption;
        }
        internal static void UpdateAdoption(bool result, Adoption adoption)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var updateAdoption = db.Adoptions.Where(a => a.ClientId == adoption.ClientId).First();
            updateAdoption.ApprovalStatus = result ? "APPROVED" : "PENDING";
            db.SubmitChanges();
        }
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            return db.AnimalShots.Where(c => c.AnimalId == animal.AnimalId);

        }
        internal static void NewShot(string booster, Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            AnimalShot getShot = new AnimalShot();
            getShot.AnimalId = animal.AnimalId;
            getShot.ShotId = UserInterface.GetIntegerData("shot", "The ID of the ");
            getShot.DateReceived = DateTime.Now;
            db.AnimalShots.InsertOnSubmit(getShot);
            db.SubmitChanges();

        }

        internal static void UpdateShot(string booster, Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            UserInterface.DisplayShot();
            int input = UserInterface.GetIntegerData("to update", "Select the ID # ");
            AnimalShot updateAnimalShot = db.AnimalShots.FirstOrDefault(u => u.ShotId == input);
            updateAnimalShot.DateReceived = DateTime.Now;
            db.SubmitChanges();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal deleteAnimal = db.Animals.Where(x => x.AnimalId.Equals(animal.AnimalId)).Single();
            db.Animals.DeleteOnSubmit(deleteAnimal);
            db.SubmitChanges();
        }

        internal static void RemoveAnimalFromRoom(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Room deleteRoom = db.Rooms.Where(r => r.AnimalId.Equals(animal.AnimalId)).Single();
            db.Rooms.DeleteOnSubmit(deleteRoom);
            db.SubmitChanges();
        }

        internal static int GetCategoryId(string categoryID)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            var categoryName = db.Categories.FirstOrDefault(i => i.Name == categoryID);
            if (categoryName == null)
            {
                Console.Clear();
                Console.WriteLine("The category you've entered does not exist\n");
                Console.WriteLine($"Adding {categoryID} to database\n");
                addNewCategory(categoryID);
            }
            var categoryId = db.Categories.Where(c => c.Name == categoryID).Select(i => i.CategoryId).SingleOrDefault();
            return categoryId;

            //return db.Categories.Where(c => c.CategoryId.Equals(categoryID)).Select(c => c.CategoryId).FirstOrDefault();
        }

        internal static void addNewCategory(string name)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Category categoryToAdd = new Category();
            categoryToAdd.Name = name;
            db.Categories.InsertOnSubmit(categoryToAdd);
            db.SubmitChanges();
        }
        //////////////////////////////////////
        internal static void MoveAnimal()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            var duplicates = db.Rooms
                            .GroupBy(r => r.AnimalId)
                            .SelectMany(g => g.OrderByDescending(i => i.RoomNumber).Skip(1));

            db.Rooms.DeleteAllOnSubmit(duplicates);
            db.SubmitChanges();

        }

        internal static void ChangeAnimalRoom()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            CreateRoom();

        }

        internal static void EditDietPlan(string editPlan)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            DietPlan updateDietPlan = db.DietPlans.FirstOrDefault(d => d.Name == editPlan);

            //updateDietPlan.Name = UserInterface.GetStringData("name", "The diet plan's");
            updateDietPlan.FoodType = UserInterface.GetStringData("food type", "The diet plan's new");
            updateDietPlan.FoodAmountInCups = int.Parse(UserInterface.GetStringData("in cup serving", "The food amount"));
            db.SubmitChanges();

        }
        internal static int GetDietPlanId(string dietPlanID)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var dietName = db.DietPlans.FirstOrDefault(d => d.Name == dietPlanID);
            if (dietName == null)
            {
                Console.Clear();
                Console.WriteLine("The diet plan you've entered does not exist\n");
                Console.WriteLine($"Adding {dietPlanID} to database\n");
                addNewDietPlan(dietPlanID);
            }
            var dietId = db.DietPlans.Where(c => c.Name == dietPlanID).Select(i => i.DietPlanId).SingleOrDefault();
            return dietId;

            //return db.DietPlans.Where(d => d.DietPlanId.Equals(dietPlanID)).Select(d => d.DietPlanId).Single();
        }

        internal static void addNewDietPlan(string name)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            DietPlan dietPlanToAdd = new DietPlan();
            dietPlanToAdd.Name = name;
            dietPlanToAdd.FoodType = UserInterface.GetStringData("type is", "the food");
            dietPlanToAdd.FoodAmountInCups = UserInterface.GetIntegerData("cup is", "Serving size in ");
            db.DietPlans.InsertOnSubmit(dietPlanToAdd);
            db.SubmitChanges();
        }

        internal static void EnterAnimalUpdate(Animal animal)
        {
            UserEmployee userEmployee = new UserEmployee();
            int input = UserInterface.GetIntegerData();
            if (input == 1)
            {
                ChangeCategory(animal);
                userEmployee.UpdateAnimal(animal);
            }
            else if (input == 2)
            {
                ChangeName(animal);
                userEmployee.UpdateAnimal(animal);
            }
            else if (input == 3)
            {
                ChangeAge(animal);
                userEmployee.UpdateAnimal(animal);
            }
            else if (input == 4)
            {
                ChangeDemeanor(animal);
                userEmployee.UpdateAnimal(animal);
            }
            else if (input == 5)
            {
                ChangeKidFriendly(animal);
                userEmployee.UpdateAnimal(animal);
            }
            else if (input == 6)
            {
                ChangePetFriendly(animal);
                userEmployee.UpdateAnimal(animal);
            }
            else if (input == 7)
            {
                ChangeWeight(animal);
                userEmployee.UpdateAnimal(animal);
            }
            else if (input == 8)
            {
                //finish

            }
        }


        internal static void ChangeCategory(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal updateAnimal = db.Animals.Where(c => c.CategoryId == animal.CategoryId && c.AnimalId == animal.AnimalId).Single();
            Console.WriteLine("Enter the animal's new Category.");
            int category = Convert.ToInt32(Console.ReadLine());
            updateAnimal.CategoryId = category;
            db.SubmitChanges();
        }

        internal static void ChangeName(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal updateAnimal = db.Animals.Where(n => n.Name == animal.Name && n.AnimalId == animal.AnimalId).Single();
            Console.WriteLine("Enter the animal's new Name.");
            string name = Console.ReadLine();
            updateAnimal.Name = name;
            db.SubmitChanges();
        }

        internal static void ChangeAge(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal updateAnimal = db.Animals.Where(a => a.Age == animal.Age && a.AnimalId == animal.AnimalId).Single();
            Console.WriteLine("Enter the animal's new Age.");
            int age = Convert.ToInt32(Console.ReadLine());
            updateAnimal.Age = age;
            db.SubmitChanges();
        }

        internal static void ChangeDemeanor(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal updateAnimal = db.Animals.Where(d => d.Demeanor == animal.Demeanor && d.AnimalId == animal.AnimalId).Single();
            Console.WriteLine("Enter the animal's new Demeanor.");
            string demeanor = Console.ReadLine();
            updateAnimal.Demeanor = demeanor;
            db.SubmitChanges();
        }

        internal static void ChangeKidFriendly(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal updateAnimal = db.Animals.Where(k => k.KidFriendly == animal.KidFriendly && k.AnimalId == animal.AnimalId).Single();
            Console.WriteLine("Is the animal kid friendly, 'Yes' or 'No'.");
            string input = Console.ReadLine();
            bool kidFriendly = input.ToUpper() == "YES" ? true : false;
            updateAnimal.KidFriendly = kidFriendly;
            db.SubmitChanges();
        }
        internal static void ChangePetFriendly(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal updateAnimal = db.Animals.Where(p => p.PetFriendly == animal.PetFriendly && p.AnimalId == animal.AnimalId).Single();
            Console.WriteLine("Is the animal pet friendly, 'Yes' or 'No'.");
            string input = Console.ReadLine();
            bool petFriendly = input.ToUpper() == "YES" ? true : false;
            updateAnimal.PetFriendly = petFriendly;
            db.SubmitChanges();
        }
        internal static void ChangeWeight(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal updateAnimal = db.Animals.Where(w => w.Weight == animal.Weight && w.AnimalId == animal.AnimalId).Single();
            Console.WriteLine("Enter the animal's new weight.");
            int weight = Convert.ToInt32(Console.ReadLine());
            updateAnimal.Weight = weight;
            db.SubmitChanges();
        }

        //finish

        internal static void AddAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
            CreateRoom();
        }
        internal static Room GetRoom(int animalID)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            return db.Rooms.Where(r => r.AnimalId ==animalID).SingleOrDefault();
        }

        internal static void CreateRoom()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Room room = new Room();
            room.RoomNumber = db.Rooms.Max(r => r.RoomNumber)+1;
            db.Rooms.InsertOnSubmit(room);
            db.SubmitChanges();
            AssignRoom(); 
        }

        internal static void AssignRoom()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            IQueryable<Room> room = db.Rooms;
            foreach (Room row in room)
            {
                if (row.AnimalId == null)

                {
                    row.AnimalId = db.Animals.Max(i => i.AnimalId);
                    db.SubmitChanges();
                    return;
                }
            }

        }
    }
}