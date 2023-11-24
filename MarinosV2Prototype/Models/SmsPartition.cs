using MarinosV2Prototype.Models.BaseModels;
using MarinosV2Prototype.Utils;
using Newtonsoft.Json;

namespace MarinosV2Prototype.Models;

[JsonObject]
public class SmsPartition : TreeEntity<SmsPartition>
{
    public string Name   { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
        
    public virtual ObservableCollectionWithSelectedItem<SmsDocument>? Documents { get; set; }
}