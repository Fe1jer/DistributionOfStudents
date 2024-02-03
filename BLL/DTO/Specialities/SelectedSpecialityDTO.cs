using BLL.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Specialities
{
    public class SelectedSpecialityDTO : EntityDTO
    {
        public string FullName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
