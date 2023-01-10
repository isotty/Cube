#nullable disable
using System;
using System.Collections.Generic;

namespace Cube.Models
{
    public partial class Services
    {
        public Services()
        {
            Employs = new HashSet<Employs>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Employs> Employs { get; set; }
    }
}