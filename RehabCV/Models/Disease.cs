﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Models
{
    public class Disease
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Child Child { get; set; }
    }
}