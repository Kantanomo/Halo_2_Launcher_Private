namespace Halo_2_Launcher.Private.Controllers
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
				_LoginResultEnum = value;
			}
		}
		public string LoginToken
		{
			get
			{
				return _LoginToken;
			}
			set
			{
				_LoginToken = value;
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
