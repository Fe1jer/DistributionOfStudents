namespace BLL.DTO.Base
{
    public abstract class EntityDTO
    {
        public bool IsNew => Id == Guid.Empty;
        public virtual Guid Id { get; set; }
    }
}
