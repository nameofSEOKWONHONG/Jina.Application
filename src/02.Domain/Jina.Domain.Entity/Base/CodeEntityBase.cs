using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Base;

[Index(nameof(CodeName), IsUnique = false, Name = "IX_CODE_NAME")]
public class CodeEntityBase : EntityBase
{
    [Key, Column(Order = 1), Comment("코드")]
    [MaxLength(10)]
    public string Code { get; set; }
    
    [Comment("코드명")]
    [MaxLength(100)]
    public string CodeName { get; set; }
}