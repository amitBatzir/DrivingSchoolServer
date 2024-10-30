using Microsoft.AspNetCore.Mvc;
using DrivingSchoolServer.Models;
using DrivingSchoolServer.DTO;
namespace DrivingSchoolServer.Controllers;

[Route("api")]
[ApiController]
public class DrivingSchoolAPIController : ControllerBase
{
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
}

