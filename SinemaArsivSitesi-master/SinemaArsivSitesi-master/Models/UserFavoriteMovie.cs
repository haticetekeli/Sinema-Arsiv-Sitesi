using SinemaArsivSitesi.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class UserFavoriteMovie
{
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    [ForeignKey("Movie")]
    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedById { get; set; }
}
