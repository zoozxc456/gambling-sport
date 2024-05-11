using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace gamblingsportmember.Models
{
    public class MemberModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Account { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string Role { get; set; } = "User";
        [Required]
        public decimal Balance { get; set; }
    }

    public class SettleModel
    {
        public Guid MemberId { get; set; }
        public decimal ReturnVal { get; set; }
    }
}