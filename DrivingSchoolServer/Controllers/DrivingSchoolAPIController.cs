﻿using Microsoft.AspNetCore.Mvc;
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

    #region login
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
            if (loginDto.UserTypes == UserTypes.TEACHER)
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
    #endregion

    #region Student
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
    #endregion

    #region Teacher
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

    [HttpGet("getSchools")]
    public IActionResult GetSchools()
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Get list of schools from DB
            List<Models.Manager> managers = context.Managers.ToList();
            List<DTO.Manager> dtoManagers = new List<DTO.Manager>();

            foreach (Models.Manager m in managers)
            {
                dtoManagers.Add(new DTO.Manager(m));
            }
            return Ok(dtoManagers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("UploadProfileImage")]
    public async Task<IActionResult> UploadProfileImageAsync(IFormFile file)
    {
        //Check if who is logged in
        string? teacherEmail = HttpContext.Session.GetString("loggedInTeacher");
        if (string.IsNullOrEmpty(teacherEmail))
        {
            return Unauthorized("המשתמש לא מחובר למערכת");
        }

        //Get model user class from DB with matching email. 
        Models.Teacher? teacher = context.GetTeacher(teacherEmail);
        //Clear the tracking of all objects to avoid double tracking
        context.ChangeTracker.Clear();

        if (teacher == null)
        {
            return Unauthorized("המשתמש לא נמצא בדאטא בייס");
        }


        //Read all files sent
        long imagesSize = 0;

        if (file.Length > 0)
        {
            //Check the file extention!
            string[] allowedExtentions = { ".png", ".jpg" };
            string extention = "";
            if (file.FileName.LastIndexOf(".") > 0)
            {
                extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
            }
            if (!allowedExtentions.Where(e => e == extention).Any())
            {
                //Extention is not supported
                return BadRequest("File sent with non supported extention");
            }

            //Build path in the web root (better to a specific folder under the web root
            string filePath = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{teacher.TeacherId}{extention}";

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);

                if (IsImage(stream))
                {
                    imagesSize += stream.Length;
                }
                else
                {
                    //Delete the file if it is not supported!
                    System.IO.File.Delete(filePath);
                }

            }

        }

        DTO.Teacher dtoTeacher = new DTO.Teacher(teacher);
        dtoTeacher.ProfilePic = GetProfileImageVirtualPath(dtoTeacher.TeacherId);
        return Ok(dtoTeacher);
    }

    //Helper functions

    //this function gets a file stream and check if it is an image
    private static bool IsImage(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);

        List<string> jpg = new List<string> { "FF", "D8" };
        List<string> bmp = new List<string> { "42", "4D" };
        List<string> gif = new List<string> { "47", "49", "46" };
        List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
        List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

        List<string> bytesIterated = new List<string>();

        for (int i = 0; i < 8; i++)
        {
            string bit = stream.ReadByte().ToString("X2");
            bytesIterated.Add(bit);

            bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
            if (isImage)
            {
                return true;
            }
        }

        return false;
    }

    //this function check which profile image exist and return the virtual path of it.
    //if it does not exist it returns the default profile image virtual path
    private string GetProfileImageVirtualPath(string teacherId)
    {
        string virtualPath = $"/profileImages/{teacherId}";
        string path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{teacherId}.png";
        if (System.IO.File.Exists(path))
        {
            virtualPath += ".png";
        }
        else
        {
            path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{teacherId}.jpg";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".jpg";
            }
            else
            {
                virtualPath = $"/profileImages/default.png";
            }
        }

        return virtualPath;
    }

    #endregion

    #region Manager
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
    #endregion





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

    

