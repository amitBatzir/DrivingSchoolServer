using System.ComponentModel.DataAnnotations.Schema;

namespace DrivingSchoolServer.DTO
{
    public class HomePage
    {
        public string HomePageText { get; set; } = null!;
        public DateTime UpdateTime {  get; set;  }
        public HomePage() { }
        public HomePage(Models.HomePage hm)
        {
            HomePageText = hm.HomePageText;
            UpdateTime = hm.UpdateTime;        
        }

        public Models.HomePage GetModel()
        {
            Models.HomePage hm = new Models.HomePage();
            hm.HomePageText = HomePageText;
            hm.UpdateTime = UpdateTime;
            return hm;
        }
    }
}
