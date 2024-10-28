using System.ComponentModel.DataAnnotations;

namespace DrivingSchoolServer.DTO
{
    public class Status
    {
        public int StatusId { get; set; }
        public string StatusDescription { get; set; } = null!;
        public Status() { }
        public Status(Models.Status s)
        {
            StatusId = s.StatusId;
            StatusDescription = s.StatusDescription;
         
        }

        public Models.Status GetModel()
        {
            Models.Status s = new Models.Status();
            s.StatusId = StatusId;
            s.StatusDescription = StatusDescription;
            return s;
        }


    }
}
