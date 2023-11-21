using Newtonsoft.Json;

namespace MarinosV2Prototype.Models
{
    [JsonObject]
    public abstract class ChangeInfoEntity : IdEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
