using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DrivingSchoolServer.Models;

namespace DrivingSchoolServer.DTO
{
    public class Test
    {
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public int ManagerId { get; set; }
        public DateTime DateOfTest { get; set; }
        public bool PassedOrNot { get; set; }
        public string Comments { get; set; } = null!;
        public Test(Models.Test tst)
        {
            TestId = tst.TestId;
            StudentId = tst.StudentId;
            ManagerId = tst.ManagerId;
            DateOfTest = tst.DateOfTest;
            PassedOrNot = tst.PassedOrNot;
            Comments = tst.Comments;
        }

        public Models.Test GetModel()
        {
            Models.Test tst = new Models.Test();
            tst.TestId = TestId;
            tst.StudentId = StudentId;
            tst.ManagerId = ManagerId;
            tst.DateOfTest = DateOfTest;
            tst.PassedOrNot = PassedOrNot;
            tst.Comments = Comments;
            return tst;
        }

    }
}
