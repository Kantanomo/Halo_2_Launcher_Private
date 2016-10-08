namespace Halo_2_Launcher.Controllers
{
    public class LoginResult
    {
        private LoginResultEnum _LoginResultEnum;
        private string _LoginToken;
        public LoginResultEnum LoginResultEnum
        {
            get
            {
                return _LoginResultEnum;
            }
            set
            {
                this._LoginResultEnum = value;
            }
        }
        public string LoginToken
        {
            get
            {
                return this._LoginToken;
            }
            set
            {
                this._LoginToken = value;
            }
        }
    }
    public enum LoginResultEnum
    {
        InvalidLoginToken,
        InvalidUsernameOrPassword,
        Banned,
        Successfull,
        GenericFailure
    }
}
