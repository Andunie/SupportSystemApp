using SupportSystem.Application.Interfaces;

namespace SupportSystemApp.Application.Services
{
	public class ResetPasswordService
	{
		private readonly IUserRepository _userRepository;

		public ResetPasswordService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}


	}
}
