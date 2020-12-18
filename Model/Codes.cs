using System.ComponentModel.DataAnnotations;

namespace CodesAccounting.Model
{
    public class Codes
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string ISBN { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        public string Month { get; set; }
        [MaxLength(50)]
        public string Course { get; set; }
        [MaxLength(20)]
        public string Level { get; set; }
        public string AddDate { get; set; }
        public string UseDate { get; set; }
        public string Active { get; set; }
        public bool IsUsed { get; set; }
        [MaxLength(500)]
        public string Comments { get; set; }

        public int TemplateId { get; set; }
        public Templates Template { get; set; }
    }
}
