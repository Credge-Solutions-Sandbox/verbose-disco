        using System.ComponentModel.DataAnnotations;
    public class Learn
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
    }