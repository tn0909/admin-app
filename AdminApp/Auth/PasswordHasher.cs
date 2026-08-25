using Microsoft.AspNetCore.Identity;

namespace AdminApp.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly Microsoft.AspNetCore.Identity.PasswordHasher<object> _hasher = new();
        private static readonly object HasherUser = new();

        public string Hash(string password)
        {
            return _hasher.HashPassword(HasherUser, password);
        }

        public bool Verify(string hash, string password)
        {
            var result = _hasher.VerifyHashedPassword(HasherUser, hash, password);
            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
