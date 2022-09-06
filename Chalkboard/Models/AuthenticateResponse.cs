//using ChalkboardAPI.Entities;
using ESCHOOL.Models;

namespace ChalkboardAPI.Models
{
    public class AuthenticateResponse
    {
        private Exams user;
        private ExamDetails user1;

        //public int StudentloginId { get; set; }
        public int StudentId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string GuardianEmail { get; set; }
        public string GuardianPassword { get; set; }
        public string SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string Token { get; set; }

        //public Teachers User { get; }
        //public Students User { get; }



        //public int StudentId { get; set; }
        //public int sl { get; set; }
        //public string RegNo { get; set; }
        //public int ClassId { get; set; }
        //public int SectionId { get; set; }
        //public string Name { get; set; }
        //public string DOB { get; set; }
        //public string MobNo { get; set; }
        ////public string Email { get; set; }
        //public string Password { get; set; }
        //public string PhotoId { get; set; }
        //public string EntryBy { get; set; }
        //public string EntryDate { get; set; }
        //public string StudentPhoto { get; set; }



        //public Studentlogin User { get; }

        public AuthenticateResponse(VW_StudentLogin user, string token)
        {
            StudentId = user.StudentId;
            Email = user.Email;
            Password = user.Password;
            SchoolId = user.SchoolId;
            SchoolName = user.SchoolName;
            Token = token;
            if (user != null && !string.IsNullOrWhiteSpace(user.GuardianEmail) && !string.IsNullOrWhiteSpace(user.GuardianPassword))
            {
                GuardianEmail = user.GuardianEmail;
                GuardianPassword = user.GuardianPassword;
            }
        }
        public AuthenticateResponse(Students user, string token)
        {
            //StudentloginId = user.StudentId;
            //Email = user.Email;
            ////sl = user.sl;         
            ////RegNo = user.RegNo;

            ////ClassId = user.ClassId;
            ////SectionId = user.SectionId;
            ////Name = user.Name;
            ////DOB = user.DOB;
            ////MobNo = user.MobNo;
            ////PhotoId = user.PhotoId;
            ////EntryBy = user.EntryBy;
            ////EntryDate = user.EntryDate;
            ////StudentPhoto = user.StudentPhoto;
            //Password = user.Password;
            //Token = token;
        }

        public AuthenticateResponse(Teachers user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Vw_TeacherNew user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(vw_Bravo user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Vw_StudentProfileView user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(vw_Foxtrot user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(vw_Alpha user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Tasks user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Chat user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Classes user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Vw_TimeTable user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Subjects user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(StdAttendance user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(NewsUpdates user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(TaskChats user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Sections user, string token)
        {
            Token = token;
        }

        public AuthenticateResponse(Exams user, string token)
        {
            this.user = user;
            Token = token;
        }

        public AuthenticateResponse(ExamDetails user2, string token)
        {
            Token = token;
        }

        //public AuthenticateResponse(ExamDetails user1, string token)
        //{
        //    this.user1 = user1;
        //    Token = token;
        //}

        //public AuthenticateResponse(Teachers user, string token)
        //{
        //    //User = user;
        //    Token = token;
        //}

        //public AuthenticateResponse(Vw_TeacherNew user, string token)
        //{
        //    Token = token;
        //}
        //public AuthenticateResponse(Students user, string token)
        //{
        //    User = user;
        //    Token = token;
        //}
    }
}