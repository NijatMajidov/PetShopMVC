﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PetShop.Core.Models
{
    public class AppUser:IdentityUser
    {
       public string Name { get; set; }
       public string Surname { get; set; }
    }
}
