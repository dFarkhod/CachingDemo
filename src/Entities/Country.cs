namespace VirtualDars.CachingDemo.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
