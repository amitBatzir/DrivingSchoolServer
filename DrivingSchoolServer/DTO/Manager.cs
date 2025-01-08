using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrivingSchoolServer.DTO
{
    public class Manager
    {
        public int UserManagerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ManagerEmail { get; set; } = null!;
        public string ManagerPass { get; set; } = null!;
        public int ManagerStatus { get; set; }
        public string ManagerId { get; set; } = null!;
        public string SchoolAddress { get; set; } = null!;
        public string ManagerPhone { get; set; } = null!;
        public string SchoolPhone { get; set; } = null!;
        public string SchoolName { get; set; } = null;
        public string? ProfilePic { get; set; } = null;


        public Manager() { }
        public Manager(Models.Manager m)
        {
            UserManagerId = m.UserManagerId;
            FirstName = m.FirstName;
            LastName = m.LastName;
            ManagerEmail = m.ManagerEmail;
            ManagerPass = m.ManagerPass;
            ManagerStatus = m.ManagerStatus;
            ManagerId = m.ManagerId;
            SchoolAddress = m.SchoolAddress;
            ManagerPhone = m.ManagerPhone;
            SchoolPhone = m.SchoolPhone;
            SchoolName = m.SchoolName;
            ProfilePic = m.ProfilePic;
        }

        public Models.Manager GetModel()
        {
            Models.Manager m = new Models.Manager();
            m.UserManagerId = UserManagerId;
            m.FirstName = FirstName;
            m.LastName = LastName;
            m.ManagerEmail = ManagerEmail;
            m.ManagerPass = ManagerPass;
            m.ManagerStatus = ManagerStatus;
            m.ManagerId = ManagerId;
            m.SchoolAddress = SchoolAddress;
            m.ManagerPhone = ManagerPhone;
            m.SchoolPhone = SchoolPhone;
            m.SchoolName = SchoolName;
            m.ProfilePic = ProfilePic;
            return m;
        }

    }
}
