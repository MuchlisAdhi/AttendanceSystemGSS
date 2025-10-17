using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum ApproverCategory
{
    [Display(Name = "Direct Supervisor")]
    DirectSupervisor = 0,

    [Display(Name = "Employee")]
    Employee = 1
}
