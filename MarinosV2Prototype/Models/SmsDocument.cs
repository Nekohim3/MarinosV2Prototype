using MarinosV2Prototype.Models.BaseModels;
using MarinosV2Prototype.Utils;

namespace MarinosV2Prototype.Models;

public class SmsDocument : Entity
{
    public string Name   { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;

    public         Guid           IdPartition { get; set; }
    public virtual SmsPartition? Partition   { get; set; }

    public virtual ObservableCollectionWithSelectedItem<SmsDocumentChange>? DocumentChanges { get; set; }
    public virtual ObservableCollectionWithSelectedItem<SmsDocumentFile>?   DocumentFiles   { get; set; }
}