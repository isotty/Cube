#nullable disable
using System;
using System.Collections.Generic;

namespace Cube.Models
{
    public partial class Cities
    {
        public Cities()
        {
            Employs = new HashSet<Employs>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Employs> Employs { get; set; }
    }
}