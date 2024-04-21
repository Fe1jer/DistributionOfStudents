namespace BLL.DTO.Specialities
{
    public class ArchiveSpecialityPlanDTO
    {
        public string Code { get; set; } = string.Empty;
        public string SpecialityName { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Target { get; set; }
        public int TargetPassingScore { get; set; }
        public int PassingScore { get; set; }
    }
}
