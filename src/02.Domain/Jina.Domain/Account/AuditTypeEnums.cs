using Ardalis.SmartEnum;

namespace Jina.Domain.Account;

public class ENUM_AUDIT_TYPE : SmartEnum<ENUM_AUDIT_TYPE>
{
    public static readonly ENUM_AUDIT_TYPE None = new ENUM_AUDIT_TYPE(nameof(None), 0);
    public static readonly ENUM_AUDIT_TYPE Create = new ENUM_AUDIT_TYPE(nameof(Create), 1);
    public static readonly ENUM_AUDIT_TYPE Update = new ENUM_AUDIT_TYPE(nameof(Update), 2);
    public static readonly ENUM_AUDIT_TYPE Delete = new ENUM_AUDIT_TYPE(nameof(Delete), 3);

    public ENUM_AUDIT_TYPE(string name, int value) : base(name, value)
    {
    }
}