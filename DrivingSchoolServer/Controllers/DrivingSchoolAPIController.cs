﻿using Microsoft.AspNetCore.Mvc;
using DrivingSchoolServer.Models;
using DrivingSchoolServer.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

    #region Add Lesson
    [HttpPost("addLesson")]
    public IActionResult AddLesson([FromBody] DTO.Lesson lessonDto)
    {
      
        try
        {
            //Check if who is logged in
            string? email = HttpContext.Session.GetString("loggedInStudent");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("המשתמש לא מחובר");
            }

            Models.Lesson l = lessonDto.GetModel();

            context.Entry(l).State = EntityState.Added;

            context.SaveChanges();

            //Task was updated!
            return Ok(l);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    #endregion


    [HttpGet("approvingLessons")]
    public IActionResult ApprovingLessons([FromQuery] int lessonId)
    {
        try
        {
            Models.Lesson? l = context.Lessons.Where(ll => ll.LessonId == lessonId).FirstOrDefault();
            if (l == null)
                return BadRequest("No Such lesson ID");
            l.StatusId = 2;
            context.Update(l);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("decliningLessons")]
    public IActionResult DecliningLessons([FromQuery] int lessonId)
    {
        try
        {
            Models.Lesson? l = context.Lessons.Where(ll => ll.LessonId == lessonId).FirstOrDefault();
            if (l == null)
                return BadRequest("No Such lesson ID");
            l.StatusId = 4;
            context.Update(l);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #region Packages 
    [HttpPost("UpdatePackage")]
    public IActionResult UpdatePackage([FromBody] DTO.Package PackageDto)
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
            if (manager == null || (PackageDto.ManagerId != manager.UserManagerId))
            {
                return Unauthorized("המשתמש מנסה לעדכן חבילה של בית ספר אחר");
            }

            Models.Package p = PackageDto.GetModel();

            context.Entry(p).State = EntityState.Modified;

            context.SaveChanges();

            //Task was updated!
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


    #region Add Lesson
    [HttpPost("addpackage")]
    public IActionResult addpackage([FromBody] DTO.Package packageDto)
    {

        try
        {
            //Check if who is logged in
            string? email = HttpContext.Session.GetString("loggedInManager");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("המשתמש לא מחובר");
            }

            Models.Package p= packageDto.GetModel();

            if (p.PackageId == 0)
                context.Entry(p).State = EntityState.Added;
            else
                context.Entry(p).State = EntityState.Modified;

            context.SaveChanges();

            //Task was updated!
            return Ok(p);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    #endregion

    #endregion

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

            //Student was added!
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

            //Teacher was added!
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

            //Manager was added!
            DTO.Manager dtoManager = new DTO.Manager(modelsManager);
            //dtoStudent.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoManager);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getAllStudentsOfSchool")]
    public IActionResult GetAllStudentsOfSchool()
    {
        try
        {
            //Check if teacher is logged in
            string? email = HttpContext.Session.GetString("loggedInTeacher");
            int manId = 0;
            Models.Teacher? t = null;

            if (email != null)
            {
                t = context.Teachers.Where(tt => tt.TeacherEmail == email).FirstOrDefault();
                if (t == null)
                    return BadRequest();
                manId = t.ManagerId;
            }
            else
            {
                email = HttpContext.Session.GetString("loggedInManager");
                if (email == null)
                {
                    return Unauthorized();
                }
                else
                {
                    Models.Manager? m = context.Managers.Where(mm => mm.ManagerEmail == email).FirstOrDefault();
                    if (m == null)
                        return BadRequest();
                    manId = m.UserManagerId;
                }
            }

            if (manId == 0)
                return Unauthorized();

            //Get list of students from DB
            List<Models.Student> students = context.Students.Include(s=>s.Teacher).ToList();
            List<DTO.Student> dtoStudents = new List<DTO.Student>();

            foreach (Models.Student s in students)
            {
                if(s.StudentStatus == 2 && 
                    ((t == null && s.Teacher.ManagerId == manId) || //check if the logged in user is manager and the student belong to a teacher from this school
                    (t != null && t.UserTeacherId == s.TeacherId))) //check if the logged in user is teahcer and the student belog to him
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

    [HttpGet("approvingManager")]
    public IActionResult approvingManager([FromQuery] int managerId)
    {
        try
        {
            Models.Manager? m = context.Managers.Where(mm => mm.UserManagerId == managerId).FirstOrDefault();
            if (m == null)
                return BadRequest("No Such manager ID");
            m.ManagerStatus = 2;
            context.Update(m);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("decliningManager")]
    public IActionResult decliningManager([FromQuery] int managerid)
    {
        try
        {
            Models.Manager? m = context.Managers.Where(mm => mm.UserManagerId == managerid).FirstOrDefault();
            if (m == null)
                return BadRequest("No Such manager ID");
            m.ManagerStatus = 3;
            context.Update(m);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("showPendingManagers")]
    public IActionResult showPendingManagers()
    {
        try
        {

            List<Models.Manager> managers = context.Managers.ToList();
            List<DTO.Manager> dtomanager = new List<DTO.Manager>();

            foreach (Models.Manager m in managers)
            {
                if (m.ManagerStatus == 1)
                {
                    dtomanager.Add(new DTO.Manager(m));
                }

            }
            return Ok(dtomanager);
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

    [HttpPost("updateTeacher")]
    public IActionResult UpdateTeacher([FromBody] DTO.Teacher TeacherDto)
    {
        try
        {
            //Check if who is logged in
            string? TeacherEmail = HttpContext.Session.GetString("loggedInTeacher");
            if (string.IsNullOrEmpty(TeacherEmail))
            {
                return Unauthorized("המשתמש לא מחובר");
            }

            //Get model user class from DB with matching email. 
            Models.Teacher? teacher = context.GetTeacher(TeacherEmail);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            //Check if the user that is logged in is the same user of the task
            //this situation is ok only if the user is a manager
            if (teacher == null || (TeacherDto.UserTeacherId != teacher.UserTeacherId))
            {
                return Unauthorized("המשתמש מנסה לעדכן משתמש אחר");
            }

            Models.Teacher t = TeacherDto.GetModel();

            context.Entry(t).State = EntityState.Modified;

            context.SaveChanges();

            //Task was updated!
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPost("updateStudent")]
    public IActionResult UpdateStudent([FromBody] DTO.Student StudentDto)
    {
        try
        {
            //Check if who is logged in
            string? StudentEmail = HttpContext.Session.GetString("loggedInStudent");
            if (string.IsNullOrEmpty(StudentEmail))
            {
                return Unauthorized("המשתמש לא מחובר");
            }

            //Get model user class from DB with matching email. 
            Models.Student? student = context.GetStudent(StudentEmail);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            //Check if the user that is logged in is the same user of the task
            //this situation is ok only if the user is a manager
            if (student == null || (StudentDto.UserStudentId != student.UserStudentId))
            {
                return Unauthorized("המשתמש מנסה לעדכן משתמש אחר");
            }

            Models.Student s = StudentDto.GetModel();

            context.Entry(s).State = EntityState.Modified;

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

    #region Lessons
    [HttpGet("getTeacherLessons")]
    public IActionResult GetTeacherLessons()
    {
        try
        {
            //Check if teacher is logged in
            string? teacherEmail = HttpContext.Session.GetString("loggedInTeacher");
            if (teacherEmail == null) 
            {
                return GetTeacherLessonsByStudent();
            }

            //Get Teacher with lewssons
            Models.Teacher? t = context.GetTeacherWithLessons(teacherEmail);

            if (t == null)
            {
                return Unauthorized();
            }

            // Get list of lessons
            List<Models.Lesson> lessons = t.Lessons.ToList();
            List<DTO.Lesson> dtoLessons = new List<DTO.Lesson>();

            foreach (Models.Lesson l in lessons)
            {
                if (l.StatusId < 4) //Show only non cancelled and non declined lessons
                {
                    dtoLessons.Add(new DTO.Lesson(l));
                }
            }

            return Ok(dtoLessons);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getTeacherLessonsByStudent")]
    public IActionResult GetTeacherLessonsByStudent()
    {
        try
        {
            //Check if student is logged in
            string? studentEmail = HttpContext.Session.GetString("loggedInStudent");
            if (studentEmail == null)
            {
                return Unauthorized();
            }

            //Get Student
            Models.Student? student = context.GetStudent(studentEmail);
            if(student == null)
            {
                return Unauthorized();
            }

            int teacherId = student.TeacherId;


            //Get Teacher with lewssons
            Models.Teacher? t = context.GetTeacherWithLessons(teacherId);

            if (t == null)
            {
                return BadRequest();
            }

            // Get list of lessons
            List<Models.Lesson> lessons = t.Lessons.ToList();
            List<DTO.Lesson> dtoLessons = new List<DTO.Lesson>();

            foreach (Models.Lesson l in lessons)
            {
                if (l.StatusId < 4) //Show only non cancelled and non declined lessons
                {
                    dtoLessons.Add(new DTO.Lesson(l));
                }
            }

            return Ok(dtoLessons);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getStudentPreviousLessons")]
    public IActionResult GetStudentPreviousLessons([FromQuery] int StudentId)
    {
        try
        {
            // Get list of lessons from DB
            List<Models.Lesson> lessons = context.Lessons.ToList();
            List<DTO.Lesson> dtoLessons = new List<DTO.Lesson>();

            foreach (Models.Lesson l in lessons)
            {
                // Check if the lesson exists and the date has passed
                //if(l.DidExist == true && l.DateOfLesson < DateTime.Now)
                //{
                //    dtoLessons.Add(new DTO.Lesson(l));
                //}

                // Check if the date has passed
                if (l.DateOfLesson < DateTime.Now && l.StudentId == StudentId)
                {
                    dtoLessons.Add(new DTO.Lesson(l));
                }
            }

            return Ok(dtoLessons);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("getFutureLessons")]
    public IActionResult GetFutureLessons()
    {
        string? studentEmail = HttpContext.Session.GetString("loggedInStudent");
        if (studentEmail == null)
        {
            return Unauthorized();
        }

        //Get Student
        Models.Student? student = context.GetStudent(studentEmail);
        if (student == null)
        {
            return Unauthorized();
        }
        try
        {
            // Get list of lessons from DB
            List<Models.Lesson> lessons = context.Lessons.ToList();
            List<DTO.Lesson> dtoLessons = new List<DTO.Lesson>();

            foreach (Models.Lesson l in lessons)
            {
                // Check if the lesson exists and the date has passed
                //if(l.DidExist == true && l.DateOfLesson < DateTime.Now)
                //{
                //    dtoLessons.Add(new DTO.Lesson(l));
                //}

                // Check if the date has passed
                if(l.StudentId==student.UserStudentId && l.DateOfLesson > DateTime.Now)
                {                  
                     dtoLessons.Add(new DTO.Lesson(l));
                   
                }
                
            }

            return Ok(dtoLessons);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getTeacherPreviousLessons")]
    public IActionResult GetTeacherPreviousLessons([FromQuery] int TeacherId)
    {
        try
        {
            // Get list of lessons from DB
            List<Models.Lesson> lessons = context.Lessons.ToList();
            List<DTO.Lesson> dtoLessons = new List<DTO.Lesson>();

            foreach (Models.Lesson l in lessons)
            {
                // Check if the lesson exists and the date has passed
                //if(l.DidExist == true && l.DateOfLesson < DateTime.Now)
                //{
                //    dtoLessons.Add(new DTO.Lesson(l));
                //}

                // Check if the date has passed
                if (l.DateOfLesson < DateTime.Now && l.TeacherId == TeacherId)
                {
                    dtoLessons.Add(new DTO.Lesson(l));
                }
            }

            return Ok(dtoLessons);
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

    #region Approving Teachers
    [HttpGet("showPendingTeachers")]
            public IActionResult ShowPendingTeachers(int ManagerId)
            {
                try
                {

                    List<Models.Teacher> teachers = context.Teachers.ToList();
                    List<DTO.Teacher> dtoteachers = new List<DTO.Teacher>();

                    foreach (Models.Teacher t in teachers)
                    {
                        if (t.ManagerId==ManagerId && t.TeacherStatus == 1)
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
    [HttpGet("decliningTeacher")]
    public IActionResult DecliningTeacher([FromQuery] int TeacherId)
    {
        try
        {
            Models.Teacher? t = context.Teachers.Where(tt => tt.UserTeacherId == TeacherId).FirstOrDefault();
            if (t == null)
                return BadRequest("No Such Teacher ID");
            t.TeacherStatus = 3;
            context.Update(t);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region Approving Students
    [HttpGet("showPendingStudent")]
    public IActionResult ShowPendingStudent(int TeacherId)
    {
        try
        {

            List<Models.Student> students = context.Students.ToList();
            List<DTO.Student> dtoStudents = new List<DTO.Student>();

            foreach (Models.Student s in students)
            {
                if (s.TeacherId == TeacherId && s.StudentStatus == 1)
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
    [HttpGet("approvingStudent")]
    public IActionResult ApprovingStudent([FromQuery] int StudentId)
    {
        try
        {
            Models.Student? s = context.Students.Where(ss => ss.UserStudentId == StudentId).FirstOrDefault();
            if (s == null)
                return BadRequest("No Such Student ID");
            s.StudentStatus = 2;
            context.Update(s);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("decliningStudent")]
    public IActionResult DecliningStudent([FromQuery] int StudentId)
    {
        try
        {
            Models.Student? s = context.Students.Where(ss => ss.UserStudentId == StudentId).FirstOrDefault();
            if (s == null)
                return BadRequest("No Such Student ID");
            s.StudentStatus = 3;
            context.Update(s);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion



    #region Pending Lessons
    [HttpGet("ShowPendingLessons")]
    public IActionResult ShowPendingLessons()
    {
        try
        {
            //Check if teacher is logged in
            string? teacherEmail = HttpContext.Session.GetString("loggedInTeacher");
            if (teacherEmail == null)
            {
                return GetTeacherLessonsByStudent();
            }

            //Get Teacher with lewssons
            Models.Teacher? t = context.GetTeacherWithLessons(teacherEmail);

            if (t == null)
            {
                return Unauthorized();
            }

            // Get list of lessons
            List<Models.Lesson> lessons = t.Lessons.ToList();
            List<DTO.Lesson> dtoLessons = new List<DTO.Lesson>();

            foreach (Models.Lesson l in lessons)
            {
                if (l.StatusId == 1) //Show only pending lessons
                {
                    dtoLessons.Add(new DTO.Lesson(l));
                }
            }

            return Ok(dtoLessons);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("ApprovingLesson")]
    public IActionResult ApprovingLesson([FromQuery] int lessonId)
    {
        try
        {
            Models.Lesson? l = context.Lessons.Where(LL => LL.LessonId == lessonId).FirstOrDefault();
            if ( l== null)
                return BadRequest("No Such lesson ID");
           l.StatusId = 2;
            context.Update(l);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("DecliningLesson")]
    public IActionResult DecliningLesson([FromQuery] int LessonId)
    {
        try
        {
            Models.Lesson? l = context.Lessons.Where(ll => ll.LessonId == LessonId).FirstOrDefault();
            if (l == null)
                return BadRequest("No Such lessons ID");
           l.StatusId = 4;
            context.Update(l);
            context.SaveChanges();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
}