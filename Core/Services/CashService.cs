using Domain.Contracts;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CashService(ICashRepository cashRepository) : ICashService
    {
        public async Task<string?> GetCashValueAsync(string key)
        {
            var value = await cashRepository.GetAsync(key);
            return value == null ? null : value;
        }

        public async Task SetCashValueAsync(string key, object value, TimeSpan duration)
        {
            await cashRepository.SetAsync(key, value, duration);
        }
    }
}
