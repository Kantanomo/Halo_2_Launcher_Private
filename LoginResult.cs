using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halo_2_Launcher.Private
{
    public enum LoginResult
    {
        InvalidLoginToken,
        InvalidUsernameOrPassword,
        Banned,
        Successfull,
        GenericFailure
    }
}
