    using System.ComponentModel.DataAnnotations;
    public class Live
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
    }