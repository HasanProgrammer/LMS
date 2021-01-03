using System.Collections.Generic;

namespace Model.Bases
{
    public partial class RecEntity<T> : IEntity where T : IEntity
    {
        public int Id           { get; set; }
        public int? ParentId    { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
    
    public partial class RecEntity<T>
    {
        public virtual T Parent              { get; set; } /*برای استفاده در EF Core*/
        public virtual ICollection<T> Childs { get; set; } /*برای استفاده در EF Core*/
        
        /*-----------------------------------------------------------*/
        
        public virtual List<T> Node { get; set; } /*برای خروجی Node بیس مورد استفاده قرار میگیرد*/
    }
}