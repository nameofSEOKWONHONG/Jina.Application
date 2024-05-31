using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace Jina.Domain.Entity.Base;

public abstract class Entity : IAuditableEntity
{
    public string CreatedBy { get; set; }

    public string CreatedName { get; set; }

    public DateTime CreatedOn { get; set; }

    public string LastModifiedBy { get; set; }

    public string LastModifiedName { get; set; }

    public DateTime? LastModifiedOn { get; set; }
}

public interface ITenantEntity
{
    string TenantId { get; set; }
}

public abstract class TenantEntity : Entity, ITenantEntity
{
    public string TenantId { get; set; }

    public bool IsActive { get; set; }
}

/// <summary>
/// Master - Detail에서 Master에 사용.
/// </summary>
public abstract class NumberEntity : TenantEntity
{
    public int Id { get; set; }
}

/// <summary>
/// Master - Detail 구조에서 Detail에 사용.
/// GUID는 Clustered Index로 사용하면 안됨. 데이터가 많아지면 read에서(-정렬-) 성능에 문제 발생함. None Clustered Index로 사용해야 함.
/// 또한, GUID 순번이 깨진 상황 - 자동채번에서 수동채번이 된 상황, 또는 반대 -, DB 자동 채번을 사용하면 안되며, 직접 프로그래밍으로 처리해야 함. (-Sql Server의 경우 GUID 채번도 오름차순에 따른 정렬 속성에 따라 GUID가 채번됨) 
/// </summary>
public abstract class GuidEntity : TenantEntity
{
    public Guid Guid { get; set; } = Guid.Empty;
    public int SortNo { get; set; }
}

public abstract class TagEntityById : TenantEntity
{
    public int Id { get; set; }
    [MaxLength(36)]
    public string MTag { get; set; }
}

public abstract class TagEntityByGuid : TenantEntity
{
    public Guid Guid { get; set; } = Guid.Empty;
    [MaxLength(36)]
    public string MTag { get; set; }
}


// GUID 테이블의 기초
// public class GuidEntityModelBuilder : IModelBuilder
// {
//     public void Build(ModelBuilder builder)
//     {
//         builder.Entity<GuidEntity>(e =>
//         {
//             e.HasKey(m => new { m.TenantId, m.Id })
//                 .IsClustered(false);
//         });
//     }
// }