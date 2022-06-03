using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyCommon;

public class CurrentHttpContext
{
    public static void Init(int id, string account)
    {
        if (id <= 0 || string.IsNullOrWhiteSpace(account))
        {
            throw new UnauthorizedAccessException();
        }
        currentAdminId = currentAdminId > 0 ? currentAdminId : id;
        currentAdminAccount = string.IsNullOrWhiteSpace(currentAdminAccount) ? account : currentAdminAccount;
    }
    public static int currentAdminId { get; private set; }
    public static string currentAdminAccount { get; private set; }

}
