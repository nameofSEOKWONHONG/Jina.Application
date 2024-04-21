using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Common;

[Table("CodeGroups", Schema = "common")]
public class CodeGroup : CodeTenantBase
{
    [Key, Column(Order = 2), Comment("코드 그룹")]
    [MaxLength(10)]
    public string GroupCode { get; set; }
    
    [Key, Column(Order = 3), Comment("코드 그룹 키")]
    [MaxLength(20)]
    public string Key { get; set; }
    
    [Column(Order = 4), Comment("코드 그룹 값")]
    [MaxLength(4000)]
    public string Value { get; set; }
}