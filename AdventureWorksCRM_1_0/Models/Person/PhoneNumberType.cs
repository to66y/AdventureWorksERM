﻿using System;
using System.Collections.Generic;

#nullable disable

namespace AdventureWorksCRM_1_0.Models.AppDbContext
{
    public partial class PhoneNumberType
    {
        public PhoneNumberType()
        {
            PersonPhones = new HashSet<PersonPhone>();
        }

        public int PhoneNumberTypeId { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<PersonPhone> PersonPhones { get; set; }
    }
}
