using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.Core
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public List<EmployeeRole> Roles { get; set; }

        public int AppliedPromocodesCount { get; set; }

        public override string ToString()
        {
            return $"{Id} {FullName} {Email} {AppliedPromocodesCount}";
        }

    }
}