using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class GroupOfSpecialitiesStatistic : Entity
    {
        public GroupOfSpecialties GroupOfSpecialties { get; set; } = new();

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int CountOfAdmissions { get; set; }
    }
}
