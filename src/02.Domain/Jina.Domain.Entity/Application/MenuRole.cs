﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;

namespace Jina.Domain.Entity.Application;

[Table("MenuRoles", Schema = "Application")]
public class MenuRole : NumberEntityBase
{
    [Key, Column(Order = 2), MaxLength(450)]
    public string RoleId { get; set; }

    public virtual ICollection<MenuGroup> MenuGroups { get; set; }
}