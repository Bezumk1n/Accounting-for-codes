using System.ComponentModel.DataAnnotations;

namespace CodesAccounting.Model
{
    public class Templates
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string ISBN { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Course { get; set; }
        [MaxLength(20)]
        public string Level { get; set; }
    }
}
