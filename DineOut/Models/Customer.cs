﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public String Name { get; set; }
        public String Email {get; set;}
        public String PasswordHash { get; set; }

    }
}
