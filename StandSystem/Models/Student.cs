using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace StandSystem.Models;

public class Student : IdentityUser
{
    [Required]
    [DisplayName("Имя")]
    [MaxLength(35)]
    [Column(TypeName = "varchar")]
    public string FirstName { get; set; }

    [Required]
    [DisplayName("Фамилия")]
    [MaxLength(35)]
    [Column(TypeName = "varchar")]
    public string LastName { get; set; }

    [DisplayName("Отчество")]
    [MaxLength(35)]
    [Column(TypeName = "varchar")]
    public string? Patronymic { get; set; }

    [DisplayName("Группа")]
    [Column(TypeName = "varchar")]
    public string? Group { get; set; }
}
