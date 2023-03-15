namespace DistributionOfStudents.ViewModels.Students
{
    public class DetailsStudentsVM
    {
        public DetailsStudentsVM() { }
        public DetailsStudentsVM(List<DetailsStudentVM> students, string? searchValue)
        {
            Students = students;
            SearchValue = searchValue;
        }

        public List<DetailsStudentVM> Students { get; set; } = new();
        public string? SearchValue { get; set; }
    }
}
