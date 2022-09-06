using ChalkboardAPI.Models;
using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.IServices
{
   public interface IvW_EchoServices
    {

        List<vW_Echo> Gets();
        List<vW_Echo> Get(int id);
       
    }
}
