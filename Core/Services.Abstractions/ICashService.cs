using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ICashService
    {
        Task SetCashValueAsync(string key,object value,TimeSpan duration);
        Task<string?> GetCashValueAsync(string key); 
    }
}
