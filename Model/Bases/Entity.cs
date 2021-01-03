namespace Model.Bases
{
    public class Entity : IEntity
    {
        public int Id           { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}