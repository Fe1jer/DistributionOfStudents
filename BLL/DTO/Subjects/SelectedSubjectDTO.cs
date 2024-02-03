using BLL.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Subjects
{
    public class SelectedSubjectDTO : EntityDTO
    {
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
