using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.IServices
{
   public interface IStudentRegistrationServices
    {
        List<StudentRegistration> Gets();
        List<StudentRegistration> Get(int id);
        Task<bool> Create(StudentRegistration entity);//Insert API 
        Task<bool> Update(int id, StudentRegistration entity);//Update API
        Task<bool> Delete(int id); //Delete API
    }
}
