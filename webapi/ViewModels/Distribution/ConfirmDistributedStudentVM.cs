using webapi.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.Distribution
{
    public class ConfirmDistributedStudentVM
    {
        public ConfirmDistributedStudentVM() { }
        public ConfirmDistributedStudentVM(Student student)
        {
            FullName = student.Surname + " " + student.Name + " " + student.Patronymic;
            StudentId = student.Id;
        }

        public int StudentId { get; set; }

        [Display(Name = "ФИО")]
        public string FullName { get; set; } = String.Empty;
    }
}
