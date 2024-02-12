/// <summary>
/// Defines a contract for entities that can be activated or deactivated.
/// </summary>
public interface IActiveEntity
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity is active.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    long Id { get; set; }
}