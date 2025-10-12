using DAL.Postgres.Entities.Base;
using FilesManager;

namespace DAL.Postgres.Entities
{
    public class Faculty : Entity
    {
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Img { get; set; } = null!;
        public List<Speciality>? Specialities { get; set; }
    }
}