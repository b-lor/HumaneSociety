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
            if(updatedAddress == null)
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

            if(employeeFromDb == null)
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

            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

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
                    break;
                case "update":
                    Employee updateEmployee = db.Employees.Where(u => u.EmployeeNumber == employee.EmployeeNumber).Single();
                    updateEmployee.FirstName = employee.FirstName;
                    updateEmployee.LastName = employee.LastName;
                    updateEmployee.Email = employee.Email;
                    db.SubmitChanges();
                    break;
                default:
                    break;
            }
        }
        internal static int GetAnimalByID(int animalID)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            return db.Animals.Where(a => a.AnimalId == animalID).Select(a => a.AnimalId).Single();
        }
        internal static void Adopt()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

        }
        internal static List<Animal> SearchForAnimalByMultipleTraits()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            //Can put in UI??
			//
            Console.WriteLine("What criteria would you like to search for? Enter the corresponding number.");
            Console.WriteLine("Enter [1] to search by the animal's name");
            Console.WriteLine("Enter [2] to search by the animal's gender");
            Console.WriteLine("Enter [3] to search by the animal's age");
            Console.WriteLine("Enter [4] to search by the animal's demeanor");
            Console.WriteLine("Enter [5] to search by the type of animal");
            Console.WriteLine("Enter [6] to search for an animal that is kid friendly");
            Console.WriteLine("Enter [7] to search for an animal that is pet friendly");

            // should userChoice be a list???
            var userChoice = int.Parse(Console.ReadLine());
            var animalsFromDb = db.Animals.ToList();

            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("Enter the name of the animal you wish to search: ");
                    var animalNameChoice = Console.ReadLine();

                    db.Animals.Where(a => a.Name == animalNameChoice);
                    break;

                case 2:
                    Console.WriteLine("Enter the gender of the animal you wish to search: [M] or [F] ");
                    var animalGenderChoice = Console.ReadLine().ToUpper();

                    db.Animals.Where(a => a.Gender == animalGenderChoice);

                    //TERNARY option??
                    //var genderToSearchBy = animalGenderChoice == "M" ? SearchByMale() : SearchByFemale();
                    //create methods for male and female
                    break;

                case 3:
                    Console.WriteLine("Enter the age of the animal you wish to search: ");
                    var animalAgeChoice = int.Parse(Console.ReadLine());

                    db.Animals.Where(a => a.Age == animalAgeChoice);
                    break;

                case 4:
                    Console.WriteLine("Enter the demeanor of the animal you wish to search: ");
                    var animalDemeanorChoice = Console.ReadLine();

                    db.Animals.Where(a => a.Demeanor == animalDemeanorChoice);
                    break;

                case 5:
                    //Might have to list options by categoryID 
                    //switch case for animal type in UI??
    
                    Console.WriteLine("Enter the type of animal you wish to search: ");
                    var animalTypeChoice = Console.ReadLine();

                    db.Categories.Where(c => c.Name == animalTypeChoice);
                    break;

                case 6:
                    Console.WriteLine("Do you want your search to include kid friendly animals?: [Y] or [N] ");
                    var kidFriendlyChoice = Console.ReadLine().ToUpper();

                    if(kidFriendlyChoice == "Y")
                    {
                        db.Animals.Where(a => a.KidFriendly == true);
                    }
                    else
                    {
                        db.Animals.Where(a => a.KidFriendly == false);
                    }
                    break;

                case 7:
                    Console.WriteLine("Do you want your search to include pet friendly animals?: [Y] or [N] ");
                    var petFriendlyChoice = Console.ReadLine().ToUpper();

                    if(petFriendlyChoice == "Y")
                    {
                        db.Animals.Where(a => a.PetFriendly == true);
                    }
                    else
                    {
                        db.Animals.Where(a => a.PetFriendly == false);
                    }
                    break;

                default:
                    Console.WriteLine("Couldn't process your request. Please enter a valid criteria to search by.");
                    SearchForAnimalByMultipleTraits();
                    break;
            }

            return animalsFromDb;
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var adoption = db.Adoptions.Where(a => a.ApprovalStatus.ToUpper() == "PENDING");
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
        internal static void UpdateShot(string booster, Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            AnimalShot updateAnimalShot = db.AnimalShots.Where(u => u.AnimalId == animal.AnimalId).Single();

            updateAnimalShot.AnimalId = animal.AnimalId;
            updateAnimalShot.DateReceived = DateTime.Now;
            db.SubmitChanges();
        }
        internal static void RemoveAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal deleteAnimal = db.Animals.Where(x => x.AnimalId == animal.AnimalId).Single();

            db.Animals.DeleteOnSubmit(deleteAnimal);
            db.SubmitChanges();
        }
        internal static int GetCategoryId(string categoryID)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            return db.Categories.Where(c => c.Name == categoryID).Select(c => c.CategoryId).Single();
        }
        internal static int GetDietPlanId(string dietPlanID)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            return db.DietPlans.Where(d => d.Name == dietPlanID).Select(d => d.DietPlanId).Single();
        }
        internal static void EnterAnimalUpdate(Animal animal, Dictionary<int, string> dictionary)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Animal updateAnimal = db.Animals.Where(a => a.AnimalId == animal.AnimalId).Single();
            //--https://social.msdn.microsoft.com/Forums/vstudio/en-US/c98e9d32-7f06-4b3a-8917-8b58eee31e58/change-dictionary-values-while-iterating?forum=netfxbcl

            //"1. Category", "2. Name", "3. Age", "4. Demeanor", "5. Kid friendly", "6. Pet friendly", "7. Weight", "8. Finished"
            foreach (KeyValuePair<int, string> item in dictionary)
            {
                if (item.Key == 1)
                {
                    int categoryID = Convert.ToInt32(item.Value);
                    updateAnimal.CategoryId = categoryID;
                }
                else if (item.Key == 2)
                {
                    updateAnimal.Name = item.Value;
                }
                else if (item.Key == 3)
                {
                    int age = Convert.ToInt32(item.Value);
                    updateAnimal.Age = age;
                }
                else if (item.Key == 4)
                {
                    updateAnimal.Demeanor = item.Value;
                }
                else if (item.Key == 5)
                {
                    bool kidFriendly = item.Value.ToUpper() == "TRUE" ? true : false;
                    updateAnimal.KidFriendly = kidFriendly;
                }
                else if (item.Key == 6)
                {
                    bool petFriendly = item.Value.ToUpper() == "TRUE" ? true : false;
                    updateAnimal.PetFriendly = petFriendly;
                }
                else if (item.Key == 7)
                {
                    int weight = Convert.ToInt32(item.Value);
                    updateAnimal.Weight = weight;
                }
                else if (item.Key == 8)
                {
                    // Finished
                }
            }


        }
        internal static void AddAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }
        internal static void GetRoom()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

        }
    }
}