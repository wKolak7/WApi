using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAppi.Models;

namespace WAppi.Services.Interfaces
{
    public interface IP24Service
    {
        Task<Dictionary<string, string>> RegisterTransactionAsync(Order order);
        Task<Dictionary<string, string>> VerifyTransactionAsync(IFormCollection form);
    }
}
