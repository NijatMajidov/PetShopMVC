﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShop.Business.Exceptions
{
    public class FileSizeException : Exception
    {
        public string PropertyName { get; set; }
        public FileSizeException(string name, string? message) : base(message)
        {
            PropertyName = name;
        }
    }
}
