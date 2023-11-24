using Newtonsoft.Json;

namespace MarinosV2Prototype.Models.BaseModels;

[JsonObject]
public abstract class ChangeEntity : IdEntity
{
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}