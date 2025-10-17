using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum Religion
{
    [Display(Name = "Islam")]
    Islam = 1,
    [Display(Name = "Kristen Protestan")]
    Kristen = 2,
    [Display(Name = "Kristen Katolik")]
    Katolik = 3,
    [Display(Name = "Hindu")]
    Hindu = 4,
    [Display(Name = "Konghucu")]
    Konghucu = 5,
    [Display(Name = "Budha")]
    Budha = 6
}
