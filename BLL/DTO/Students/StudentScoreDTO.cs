﻿using BLL.DTO.Base;
using BLL.DTO.Subjects;

namespace BLL.DTO.Students
{
    public class StudentScoreDTO : EntityDTO
    {
        public SubjectDTO Subject { get; set; } = null!;
        public Guid SubjectId { get; set; }

        public int Score { get; set; }
    }
}