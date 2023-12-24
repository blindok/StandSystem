using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StandSystem.IdentityScheme;

[Table("UserRole")]
public class UserRole
{
    [Key, Required]
    public Guid Id { get; set; }

    [Required, ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    [Required, ForeignKey(nameof(Role))]
    public Guid RoleId { get; set; }

    public virtual Role Role { get; set; }
    public virtual User User { get; set; }
}