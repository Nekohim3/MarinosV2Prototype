using MarinosV2Prototype.Models.BaseModels;

namespace MarinosV2Prototype.Models;

public class SmsDocumentFile : Entity
{
    public string File { get; set; }

    public         Guid          IdDocument { get; set; }
    public virtual SmsDocument? Document   { get; set; }
}