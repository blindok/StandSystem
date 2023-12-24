using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StandSystem.IdentityScheme;

[Table("Role")]
public class Role
{
    [Key, Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string NormalizedName { get; set; }

    [Required]
    public string ConcurrencyStamp { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}
