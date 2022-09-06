using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.IServices
{
    public interface IVw_StudentProfileServices
    {
        List<Vw_StudentProfile> Gets();
        List<Vw_StudentProfile> Get(int classId);
    }
}
