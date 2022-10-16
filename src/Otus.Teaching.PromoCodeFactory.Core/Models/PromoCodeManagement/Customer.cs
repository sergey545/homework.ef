using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.Core
{
    public class Customer
        :BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public List<Preference> Preferences { get; set; }

        public List<PromoCode> PromoCodes { get; set; }
    }
}