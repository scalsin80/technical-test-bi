using System;
using System.Collections.Generic;

namespace APIUser.Models;

public partial class UserRol
{
    public int idUserRole { get; set; }

    public string name { get; set; } = null!;

    public DateTime registered { get; set; }

    public int? idUserRegistered { get; set; }

    public DateTime? updated { get; set; }

    public int? idUserUpdated { get; set; }

    public bool enabled { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
