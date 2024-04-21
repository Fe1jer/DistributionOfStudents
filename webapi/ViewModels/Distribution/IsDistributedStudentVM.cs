using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.Distribution
{
    public class IsDistributedStudentVM
    {
        [Display(Name = "Выбран для распределения на специальность")]
        public bool IsDistributed { get; set; }

        public Guid StudentId { get; set; }
    }
}
