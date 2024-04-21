using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;

namespace Jina.Domain.Entity.Language;

/// <summary>
/// 다국어 번역 설정
/// </summary>
[Table("MultilingualConfigs", Schema = "language")]
public class MultilingualConfig : TenantBase
{
    [Key, Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required, StringLength(5)]
    public string LanguageCode { get; set; }
    
    [Required]
    public int Sort { get; set; }
    
    // 부모 MultilingualContent의 Id
    public int? ParentId { get; set; }

    // 자식 MultilingualContent의 컬렉션
    public virtual ICollection<MultilingualConfig> ChildContents { get; set; }

    // 부모 MultilingualContent를 가리키는 외래 키 설정
    [ForeignKey("ParentId")]
    public virtual MultilingualConfig ParentContent { get; set; }    
}