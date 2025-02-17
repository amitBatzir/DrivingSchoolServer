using Microsoft.AspNetCore.Mvc;
using DrivingSchoolServer.Models;
using DrivingSchoolServer.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
            dtoStudent.ProfilePic = GetProfileImageVirtualPath(dtoStudent.UserStudentId);
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
            dtoTeacher.ProfilePic = GetProfileImageVirtualPath(dtoTeacher.UserTeacherId);
            return Ok(modelsTeacher);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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

    [HttpGet("getAllStudents")]
    public IActionResult GetAllStudents()
    {
        try
        {
            //Get list of students from DB
            List<Models.Student> students = context.Students.ToList();
            List<DTO.Student> dtoStudents = new List<DTO.Student>();

            foreach (Models.Student s in students)
            {
                if(s.StudentStatus == 2)
                {
                    dtoStudents.Add(new DTO.Student(s));
                }
            }
            return Ok(dtoStudents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Photo

    [HttpPost("UploadProfileImage")]
        public async Task<IActionResult> UploadProfileImageAsync(IFormFile file, [FromBody] DTO.LoginInfo loginDto)
        {
            //Check if who is logged in
            if (loginDto.UserTypes == UserTypes.STUDENT)
            {
                string? StudentEmail = HttpContext.Session.GetString("loggedInStudent");
                if (string.IsNullOrEmpty(StudentEmail))
                {
                    return Unauthorized("המשתמש לא מחובר למערכת");
                }

                //Get model user class from DB with matching email. 
                Models.Student? student = context.GetStudent(StudentEmail);
                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();

                if (student == null)
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
                    string filePath = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{student.UserStudentId}{extention}";

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

                DTO.Student dtoStudent = new DTO.Student(student);
                dtoStudent.ProfilePic = GetProfileImageVirtualPath(dtoStudent.UserStudentId);
                return Ok(dtoStudent);
            }

            // Teacher
            if (loginDto.UserTypes == UserTypes.TEACHER)
            {
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
                    string filePath = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{teacher.UserTeacherId}{extention}";

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
                dtoTeacher.ProfilePic = GetProfileImageVirtualPath(dtoTeacher.UserTeacherId);
                return Ok(dtoTeacher);
            }

            //Manager

            if (loginDto.UserTypes == UserTypes.MANAGER)
            {
                string? managerEmail = HttpContext.Session.GetString("loggedInManager");
                if (string.IsNullOrEmpty(managerEmail))
                {
                    return Unauthorized("המשתמש לא מחובר למערכת");
                }

                //Get model user class from DB with matching email. 
                Models.Manager? manager = context.GetManager(managerEmail);
                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();

                if (manager == null)
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
                    string filePath = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{manager.UserManagerId}{extention}";

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

                DTO.Manager dtoManager = new DTO.Manager(manager);
                dtoManager.ProfilePic = GetProfileImageVirtualPath(dtoManager.UserManagerId);
                return Ok(dtoManager);
            }
            return Ok();
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
        private string GetProfileImageVirtualPath(int UserId)
        {
            string virtualPath = $"/profileImages/{UserId}";
            string path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{UserId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{UserId}.jpg";
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

    #region Profile  
    //This method call the UpdateUser web API on the server and return true if the call was successful
    //or false if the call fails
    [HttpPost("updateManager")]
    public IActionResult UpdateManager([FromBody] DTO.Manager ManagerDto)
    {
        try
        {
            //Check if who is logged in
            string? ManagerEmail = HttpContext.Session.GetString("loggedInManager");
            if (string.IsNullOrEmpty(ManagerEmail))
            {
                return Unauthorized("המשתמש לא מחובר");
            }

            //Get model user class from DB with matching email. 
            Models.Manager? manager = context.GetManager(ManagerEmail);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            //Check if the user that is logged in is the same user of the task
            //this situation is ok only if the user is a manager
            if (manager == null || (ManagerDto.UserManagerId != manager.UserManagerId))
            {
                return Unauthorized("המשתמש מנסה לעדכן משתמש אחר");
            }

            Models.Manager m = ManagerDto.GetModel();

            context.Entry(m).State = EntityState.Modified;

            context.SaveChanges();

            //Task was updated!
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    #endregion


    // פעולה שמחזירה רשימה של המנהלים - כל מנהל הוא בבית ספר אחר
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
                if (m.ManagerStatus == 2)
                    {
                        dtoManagers.Add(new DTO.Manager(m));

                    }
            }
                return Ok(dtoManagers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // פעולה שמחזירה את כל המורים מבית ספר ספציפי - שולחים אליה את האיי די הייחודי למנהל ולפי זה בודקים
        [HttpGet("getTeacherOfSchool")]
        public IActionResult GetTeacherOfSchool([FromQuery] int ManagerId)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get list of teac from DB
                List<Models.Teacher> teachers = context.Teachers.ToList();
                List<DTO.Teacher> dtosTeachers = new List<DTO.Teacher>();

                foreach (Models.Teacher t in teachers)
                {
                    if (t.ManagerId == ManagerId && t.TeacherStatus == 2)
                    {
                        dtosTeachers.Add(new DTO.Teacher(t));
                    }

                }
                return Ok(dtosTeachers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("getPackageOfSchool")]
        public IActionResult GetPackageOfSchool([FromQuery] int ManagerId)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get list of PACKAGES  from DB
                List<Models.Package> packages = context.Packages.ToList();
                List<DTO.Package> dtoPackages = new List<DTO.Package>();

                foreach (Models.Package p in packages)
                {
                    if (p.ManagerId == ManagerId)
                    {
                        dtoPackages.Add(new DTO.Package(p));
                    }

                }
                return Ok(dtoPackages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            [HttpGet("showPendingTeachers")]
            public IActionResult ShowPendingTeachers()
            {
                try
                {

                    List<Models.Teacher> teachers = context.Teachers.ToList();
                    List<DTO.Teacher> dtoteachers = new List<DTO.Teacher>();

                    foreach (Models.Teacher t in teachers)
                    {
                        if (t.TeacherStatus == 1)
                        {
                           dtoteachers.Add(new DTO.Teacher(t));
                        }

                    }                  
                    return Ok(dtoteachers);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
    [HttpGet("approvingTeacher")]
    public IActionResult ApprovingTeacher([FromQuery] int TeacherId)
    {
        try
        {
            Models.Teacher? t = context.Teachers.Where(tt => tt.UserTeacherId == TeacherId).FirstOrDefault();
            if (t == null)
                return BadRequest("No Such Teacher ID");
            t.TeacherStatus = 2;
            context.Update(t);
            context.SaveChanges();
            
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}