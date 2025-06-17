using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APIUser.Models;

[Table("UserRol")]
public partial class UserRol
{
    [Key]
    public int idUserRole { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string name { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime registered { get; set; }

    public int? idUserRegistered { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated { get; set; }

    public int? idUserUpdated { get; set; }

    public bool enabled { get; set; }

    [InverseProperty("idUserRolNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
