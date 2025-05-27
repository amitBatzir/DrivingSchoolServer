using System.ComponentModel.DataAnnotations;

namespace DrivingSchoolServer.DTO
{
    public class Status
    {
        public int StatusId { get; set; }
        public string StatusDescription { get; set; } = null!;
        public Status() { }
        public Status(Models.S s)
        {
            StatusId = s.StatusId;
            StatusDescription = s.StatusDescription;
         
        }

        public Models.S GetModel()
        {
            Models.S s = new Models.S();
            s.StatusId = StatusId;
            s.StatusDescription = StatusDescription;
            return s;
        }


    }
}
