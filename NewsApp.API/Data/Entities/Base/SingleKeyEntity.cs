using System.ComponentModel.DataAnnotations;

namespace NewsApp.API.Data.Entities.Base;

public class SingleKeyEntity : BaseEntity
{
    public SingleKeyEntity()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }
}