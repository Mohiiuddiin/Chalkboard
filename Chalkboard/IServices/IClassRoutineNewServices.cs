using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.IServices
{
    public interface IClassRoutineNewServices
    {

        List<ClassRoutineNew> Gets();
        List<ClassRoutineNew> Get(int id);
        Task<bool> Create(ClassRoutineNew entity);//Insert API 
        Task<bool> Update(int id, ClassRoutineNew entity);//Update API
        Task<bool> Delete(int id); //Delete API
    }
}
