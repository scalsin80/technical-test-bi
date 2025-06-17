using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APIUser.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int idUser { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string name { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string email { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string password { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string address { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime birthday { get; set; }

    public int idUserRol { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string role { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime registered { get; set; }

    public int iIdUserRegistered { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime updated { get; set; }

    public int idUserUpdated { get; set; }

    public bool canEdited { get; set; }

    public bool enabled { get; set; }

    [ForeignKey("idUserRol")]
    [InverseProperty("Users")]
    public virtual UserRol idUserRolNavigation { get; set; } = null!;
}
