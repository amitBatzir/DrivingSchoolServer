using Microsoft.AspNetCore.Mvc;
using DrivingSchoolServer.Models;
using DrivingSchoolServer.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace DrivingSchoolServer.Controllers;

[Route("api")]
[ApiController]
public class DrivingSchoolAPIController : ControllerBase
{
    //https://colorhunt.co/palette/008dda41c9e2ace2e1f7eedd צבעים לאפליקציה


    //a variable to hold a reference to the db context!
    private DrivingSchoolDbContext context;
    //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
    private IWebHostEnvironment webHostEnvironment;
    //Use dependency injection to get the db context and web host into the constructor
    public DrivingSchoolAPIController(DrivingSchoolDbContext context, IWebHostEnvironment env)
    {
        this.context = context;
        this.webHostEnvironment = env;
    }

    [HttpGet]
    [Route("TestServer")]
    public ActionResult<string> TestServer()
    {
        return Ok("Server Responded Successfully");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] DTO.LoginInfo loginDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Get model user class from DB with matching email. 
            if (loginDto.UserTypes == UserTypes.STUDENT)
            {
                Models.Student? modelsStudent = context.GetStudent(loginDto.Email);
                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsStudent == null || modelsStudent.StudentPass != loginDto.Password)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInStudent", modelsStudent.StudentEmail);
                DTO.Student st = new DTO.Student(modelsStudent);
                return Ok(st);
            }
            if(loginDto.UserTypes == UserTypes.TEACHER)
            {
                Models.Teacher? modelsTeacher = context.GetTeacher(loginDto.Email);
                if (modelsTeacher == null || modelsTeacher.TeacherPass != loginDto.Password)
                {
                    return Unauthorized();
                }
                HttpContext.Session.SetString("loggedInTeacher", modelsTeacher.TeacherEmail);
                DTO.Teacher t = new DTO.Teacher(modelsTeacher);
                return Ok(t);
            }
            if (loginDto.UserTypes == UserTypes.MANAGER)
            {
                Models.Manager? modelsManager = context.GetManager(loginDto.Email);
                if (modelsManager == null || modelsManager.ManagerPass != loginDto.Password)
                {
                    return Unauthorized();
                }
                HttpContext.Session.SetString("loggedInManager", modelsManager.ManagerEmail);
                DTO.Manager m = new DTO.Manager(modelsManager);
                return Ok(m);
            }
            return Ok();                
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("registerStudent")]
    public IActionResult RegisterStudent([FromBody] DTO.Student StudentDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Create model user class
            Models.Student modelsStudent = StudentDto.GetModel();

            context.Students.Add(modelsStudent);
            context.SaveChanges(); // מכניס לדאטא בייס

            //User was added!
            DTO.Student dtoStudent = new DTO.Student(modelsStudent);
            //dtoStudent.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoStudent);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("registerTeacher")]
    public IActionResult RegisterTeacher([FromBody] DTO.Teacher TeacherDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Create model user class
            Models.Teacher modelsTeacher = TeacherDto.GetModel();

            context.Teachers.Add(modelsTeacher);
            context.SaveChanges(); // מכניס לדאטא בייס

            //User was added!
            DTO.Teacher dtoTeacher = new DTO.Teacher(modelsTeacher);
            //dtoStudent.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoTeacher);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("registerManager")]
    public IActionResult RegisterManager([FromBody] DTO.Manager ManagerDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Create model user class
            Models.Manager modelsManager = ManagerDto.GetModel();

            context.Managers.Add(modelsManager);
            context.SaveChanges(); // מכניס לדאטא בייס

            //User was added!
            DTO.Manager dtoManager = new DTO.Manager(modelsManager);
            //dtoStudent.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoManager);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //[HttpPost("CheckSchoolExist")]
    //public IActionResult CheckSchoolExist([FromBody] DTO.Manager managerDto)
    //{
    //    try
    //    {

    //        if (managerDto.SchoolName != null)
    //        {
    //            Models.Manager? modelsManagerToCheck = context.GetSchoolName(managerDto.SchoolName);
    //            if (modelsManagerToCheck == null)
    //            {
    //                return Unauthorized();
    //            }
    //            return Ok();
    //        }
    //        return Unauthorized();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

}

