namespace MarinosV2Prototype.Models.BaseModels
{
    public abstract class TreeEntity<T> : Entity where T : TreeEntity<T>
    {
        public         Guid?                      IdParent { get; set; }
        public virtual T?             Parent   { get; set; }
        public virtual ICollection<T>? Childs   { get; set; }
    }
}
