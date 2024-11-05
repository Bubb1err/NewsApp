namespace NewsApp.API.Atributes;

[AttributeUsage(AttributeTargets.Class)]
public class RepositoryAttribute : Attribute
{
    public Type EntityType { get; }

    public RepositoryAttribute(Type entityType)
    {
        EntityType = entityType;
    }
}
