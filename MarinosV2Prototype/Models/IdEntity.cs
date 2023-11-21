using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MarinosV2Prototype.Models;

[JsonObject]
public abstract class GuidEntity : VersionEntity
{
    public Guid Id { get; set; }

    public static bool operator !=(GuidEntity? a, GuidEntity? b)
    {
        return !(a == b);
    }

    public static bool operator ==(GuidEntity? a, GuidEntity? b)
    {
        if (a is null && b is null)
            return true;
        if (a is null || b is null)
            return false;
        return a.Equals(b);
    }

    public override bool Equals(object? o)
    {
        if (o is not GuidEntity e)
            return false;
        if (e.Id                   == Guid.Empty && Id == Guid.Empty)
            return e.GetHashCode() == GetHashCode();
        return e.Id == Id;
    }
}