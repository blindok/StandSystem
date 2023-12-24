using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StandSystem.IdentityScheme;

[Table("User")]
public class User
{
    [Key, Required]
    public int Id { get; set; }

    [Required, MaxLength(128)]
    public string UserName { get; set; }

    [Required, MaxLength(128)]
    public string NormalizedUserName { get; set; }

    [Required, MaxLength(1024)]
    public string PasswordHash { get; set; }

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
    [MaxLength(10)]
    [Column(TypeName = "varchar")]
    public string? Group { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}