using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class StudentScore : Entity
    {
        public Subject Subject { get; set; } = new();

        [Display(Name = "Баллы")]
        public int Score { get; set; }
    }
}