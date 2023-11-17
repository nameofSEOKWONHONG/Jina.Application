using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Base;

public class NumberEntityBase : EntityBase
{
    [Key, Column(Order = 1), Comment("인덱스")]
    public Int64 Id { get; set; }
}