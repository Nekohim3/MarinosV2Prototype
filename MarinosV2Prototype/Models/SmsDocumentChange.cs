using MarinosV2Prototype.Models.BaseModels;

namespace MarinosV2Prototype.Models;

public class SmsDocumentChange : Entity
{
    public string   DocumentVersion     { get; set; } = string.Empty;
    public DateTime DocumentVersionDate { get; set; }
    public string   Description         { get; set; } = string.Empty;

    public         Guid          IdDocument { get; set; }
    public virtual SmsDocument? Document   { get; set; }
}