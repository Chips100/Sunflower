namespace Sunflower.Entities
{
    /// <summary>
    /// Base implementation of all domain entities used in the Sunflower project.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets or sets the Id of this entity.
        /// </summary>
        public int Id { get; set; }
    }
}