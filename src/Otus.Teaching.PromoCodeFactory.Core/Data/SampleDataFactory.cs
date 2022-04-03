using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Otus.Teaching.PromoCodeFactory.Core;
using System.Threading;
using System.Diagnostics;

namespace Otus.Teaching.PromoCodeFactory.DataAccess
{
    public static class SampleDataFactory
    {


        public static void FillTheModel(
                                            IAsyncRepositoryT<Employee> employeesRepo,
                                            IAsyncRepositoryT<EmployeeRole> rolesRepo,
                                            IAsyncRepositoryT<ConsoleToApiMessage> messagesRepo,
                                            IAsyncRepositoryT<Customer> customersRepo,
                                            IAsyncRepositoryT<Preference> preferencesRepo,
                                            IAsyncRepositoryT<PromoCode> promoCodesRepo,
                                            int numberOfEmployees = 15,
                                            int numberOfCustomers = 500,
                                            int numberOfPromoCodes = 1350)
        {
            //этот класс заполняет модель данными
            //здесь интересно то, что заполняется именно модель, а не конкртеная сущность


            #region Messages

            if (messagesRepo != null)
            {
                messagesRepo.AddAsync(new ConsoleToApiMessage("Message1"));
                messagesRepo.AddAsync(new ConsoleToApiMessage("Message2"));
                messagesRepo.AddAsync(new ConsoleToApiMessage("Message3"));
            }

            #endregion

            #region Roles
            rolesRepo.AddAsync(new EmployeeRole()
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            });

            rolesRepo.AddAsync(new EmployeeRole()
            {
                Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            });
            #endregion

            #region Employees
            //Employees админские
            List<EmployeeRole> roles = rolesRepo.GetItemsListAsync().Result;

            //Console.WriteLine($"Starting generation of Employees col={numberOfEmployees}");

            employeesRepo.AddAsync(new Employee()
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Email = "owner@somemail.ru",
                FirstName = "Иван",
                LastName = "Сергеев",
                Roles = new List<EmployeeRole>()
                {
                    roles.FirstOrDefault(x => x.Name == "Admin")
                },
                AppliedPromocodesCount = 5
            });

            employeesRepo.AddAsync(new Employee()
            {
                Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Email = "ibis@somemail.ru",
                FirstName = "Николай",
                LastName = "Иванов",
                Roles = new List<EmployeeRole>()
                {
                    roles.FirstOrDefault(x => x.Name == "Admin")
                },
                AppliedPromocodesCount = 10
            });

            //Employees обычные

            var EmployeeSampler = CreateEmployeeFaker();

            EmployeeRole role01 = roles.FirstOrDefault(x => x.Name == "PartnerManager");

            for (int i = 1; i <= numberOfEmployees; i++)
            {
                Employee employee = EmployeeSampler.Generate();
                employee.Roles = new List<EmployeeRole>();
                employee.Roles.Add(role01);
                employeesRepo.AddAsync(employee);
            }

            #endregion

            #region Customers

            var customerSampler = CreateCustomerFaker();

            Customer customer = null;

            for (int i = 1; i <= numberOfCustomers; i++)
            {
                customer = customerSampler.Generate();
                //
                customer.Preferences = preferencesRepo.GetRandomObject().Result;
                customer.PromoCodes = 
                customersRepo.AddAsync(customer);
            }


            #endregion

            #region Preferences
            for (int i = 1; i <= 10; i++)
            {
                preferencesRepo.AddAsync(new Preference { Name = $"Pref{i}" });
            }

            #endregion

            #region PromoCodes

                var promoCodeFaker = CreatePromoCodeFaker();

                PromoCode promoCode;

                for (int i = 1; i <= numberOfPromoCodes; i++)
                {
                    promoCode = promoCodeFaker.Generate();
                    promoCode.PartnerManager = employeesRepo.GetRandomObject().Result;
                    promoCode.Preference = preferencesRepo.GetRandomObject().Result;
                    promoCodesRepo.AddAsync(promoCode);
                }

            #endregion
        }

        public static Faker<Employee> CreateEmployeeFaker()
        {
            Random random = new Random();
            var customersFaker = new Faker<Employee>()
                .CustomInstantiator(f => new Employee()
                {
                    Id = Guid.NewGuid(),
                })
                .RuleFor(u => u.Id, (u) => Guid.NewGuid())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                .RuleFor(u => u.AppliedPromocodesCount, (u) => random.Next(0, 20));
            
                //.RuleFor(u => u, (f, u) => f.Phone.PhoneNumber("1-###-###-####"));

            return customersFaker;
        }

        public static Faker<Customer> CreateCustomerFaker()
        {
            Random random = new Random();
            var customersFaker = new Faker<Customer>()
                .CustomInstantiator(f => new Customer()
                {
                    Id = Guid.NewGuid()
                     
                })
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email());

            //.RuleFor(u => u, (f, u) => f.Phone.PhoneNumber("1-###-###-####"));

            return customersFaker;
        }


        private static Random random = new Random();

        public static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private static Faker<PromoCode> CreatePromoCodeFaker()
        {
            Random random = new Random();
            var promoCodeFaker = new Faker<PromoCode>()
                .CustomInstantiator(f => new PromoCode()
                {
                    Id = Guid.NewGuid(),
                    Code = randomString(10),
                    ServiceInfo = "MyServiceInfo",
                    PartnerName = GetRandomWord(new string[]{"Fujitsu", "Siemens", "Microsoft", "Avtovaz", "IKEA", "Gazprom"}),
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now
                });

            return promoCodeFaker;
        }



        public static IEnumerable<Preference> Preferences => new List<Preference>()
        {
            new Preference()
            {
                Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                Name = "Театр",
            },
            new Preference()
            {
                Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                Name = "Семья",
            },
            new Preference()
            {
                Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                Name = "Дети",
            }
        };

        public static IEnumerable<Customer> Customers
        {
            get
            {
                var customerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0");
                var customers = new List<Customer>()
                {
                    new Customer()
                    {
                        Id = customerId,
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Иван",
                        LastName = "Петров",
                        //TODO: Добавить предзаполненный список предпочтений
                    }
                };

                return customers;
            }
        }

        public static string GetRandomWord(string[] input)
        {
            if (input.Length == 0) return "";

            Random random = new Random();
            int no = random.Next(0, input.Length + 1);
            if (no > input.Length - 1) no = input.Length - 1;
            return input[no];
        }


    }
}
      
