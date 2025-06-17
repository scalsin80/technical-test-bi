using System;
using System.Collections.Generic;

namespace APIUser.Models;

public partial class User
{
    public int idUser { get; set; }

    public string name { get; set; } = null!;

    public string email { get; set; } = null!;

    public string password { get; set; } = null!;

    public string address { get; set; } = null!;

    public DateTime birthday { get; set; }

    public int idUserRol { get; set; }

    public string role { get; set; } = null!;

    public DateTime registered { get; set; }

    public int iIdUserRegistered { get; set; }

    public DateTime updated { get; set; }

    public int idUserUpdated { get; set; }

    public bool canEdited { get; set; }

    public bool enabled { get; set; }

    public virtual UserRol idUserRolNavigation { get; set; } = null!;
}
