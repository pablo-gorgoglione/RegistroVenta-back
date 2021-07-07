using System;
using System.Collections.Generic;

#nullable disable

namespace WSVenta.Models
{
    public partial class User
    {
        public User()
        {
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
