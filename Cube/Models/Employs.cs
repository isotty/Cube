#nullable disable
using System;
using System.Collections.Generic;

namespace Cube.Models
{
    public partial class Employs
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string FixPhone { get; set; }
        public string Phone { get; set; }
        public int ServiceId { get; set; }
        public int CitiesId { get; set; }
        public bool Admin { get; set; }
        public double PassWord { get; set; }

        public virtual Cities Cities { get; set; }
        public virtual Services Service { get; set; }
    }
}