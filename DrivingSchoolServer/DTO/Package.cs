using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrivingSchoolServer.DTO
{
    public class Package
    {
        public int PackageId { get; set; }
        public int ManagerId { get; set; }
        public string Title { get; set; } = null!;
        public string TheText { get; set; } = null!;

        public Package() { }
        public Package(Models.Package p)
        {
            PackageId =p.PackageId;
            ManagerId = p.ManagerId;
            Title = p.Title;
            TheText = p.TheText;
        }

        public Models.Package GetModel()
        {
            Models.Package p = new Models.Package();
            p.PackageId = PackageId;
            p.ManagerId = ManagerId;
            p.Title = Title;
            p.TheText = TheText;
            return p;
        }


    }
}
