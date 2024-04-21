using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;

namespace Jina.Domain.Entity.Language;

/// <summary>
/// 다국어 번역 저장
/// </summary>
[Table("MultilingualContents", Schema = "language")]
public class MultilingualContent : TenantBase
{
    [Key, Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>
    /// 국가 언어 코드, (en-US, ja-JP, ko-KR...) 
    /// </summary>
    [Required]
    [StringLength(5)]
    public string LanguageCode { get; set; }
    
    /// <summary>
    /// 번역 대상 TEXT
    /// </summary>
    [Required]
    [Column(TypeName = "nvarchar(MAX)")]
    public string Text { get; set; }
    
    /// <summary>
    /// ai 질의 기록
    /// </summary>
    [Column(TypeName = "nvarchar(MAX)")]
    public string Comment { get; set; }
    
    /// <summary>
    /// 부모 MultilingualContent의 Id
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 자식 MultilingualContent의 컬렉션
    /// </summary>
    public virtual ICollection<MultilingualContent> ChildContents { get; set; }

    /// <summary>
    /// 부모 MultilingualContent를 가리키는 외래 키 설정
    /// </summary>
    [ForeignKey("ParentId")]
    public virtual MultilingualContent ParentContent { get; set; }
}