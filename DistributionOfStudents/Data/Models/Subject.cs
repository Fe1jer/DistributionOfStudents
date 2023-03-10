using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class Subject : Entity
    {
        [Display(Name = "Название")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public List<GroupOfSpecialties>? GroupsOfSpecialties { get; set; }
    }
}