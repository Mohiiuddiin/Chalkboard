using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.IServices
{
   public interface IStudentprofileServices
    {
        List<Studentprofile> Gets();
        List<Studentprofile> Get(int id);
        Task<bool> Create(Studentprofile entity);//Insert API 
        Task<bool> Update(int id, Studentprofile entity);//Update API
        Task<bool> Delete(int id); //Delete API

    }
}
