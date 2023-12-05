using BlogStar.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserDto user);
    }
}
