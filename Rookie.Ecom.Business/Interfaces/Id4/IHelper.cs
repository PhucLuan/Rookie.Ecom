using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Interfaces.Id4
{
    public interface IHelper
    {
        Task<string> GetAccessTokenAsync();
    }
}
