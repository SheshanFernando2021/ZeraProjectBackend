using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ZeraAPI.Model;

public class UserCustomised : IdentityUser
{
    [StringLength(100)]
    [MaxLength(100)]
    [Required]
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Password { get; set; }
}