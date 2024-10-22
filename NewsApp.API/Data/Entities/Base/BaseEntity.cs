namespace NewsApp.API.Data.Entities.Base;

public class BaseEntity
{
    public BaseEntity()
    {
        CreatedDate = DateTime.UtcNow;
    }


    public DateTime? CreatedDate { get; set; }


    public DateTime? UpdatedDate { get; set; }


    public DateTime? DeletedDate { get; set; }
}