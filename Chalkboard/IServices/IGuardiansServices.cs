using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.IServices
{
   public interface IGuardiansServices
    {
        List<Guardians> Gets();
        List<Guardians> Get(int id);
        Task<bool> Create(Guardians entity);//Insert API 
        Task<bool> Update(int id, Guardians entity);//Update API
        Task<bool> Delete(int id); //Delete API
    }
}
