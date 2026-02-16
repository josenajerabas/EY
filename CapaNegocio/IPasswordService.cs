using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
