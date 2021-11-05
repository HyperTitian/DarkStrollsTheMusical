using DarkStrollsAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Security
{
    /// <summary>
    /// Hooks the PasswordHandler class into the User class.
    /// </summary>
    public class AuthenticationChecker
    {
        /// <summary>
        /// Check if the password successfully logs in.
        /// </summary>
        /// <param name="user">User to check against.</param>
        /// <param name="password">Password used.</param>
        /// <returns>Whether the password successfully logs in.</returns>
        public bool CheckPassword(User user, string password)
        {
            PasswordHandler handler = new PasswordHandler();
            if(user.SaltHash is null)
            {
                return false;
            }
            return handler.CheckPassword(password, user.SaltHash);
        }

        /// <summary>
        /// Set the user's salthash to a new password.
        /// </summary>
        /// <param name="user">User to set salthash of.</param>
        /// <param name="password">Password to set it to.</param>
        public void SetPassword(User user, string password)
        {
            PasswordHandler handler = new PasswordHandler();
            user.SaltHash = handler.CreateHash(password);
        }

        /// <summary>
        /// Reset the password of the user.
        /// </summary>
        /// <param name="user">User to reset the password of.</param>
        public void ResetPassword(User user)
        {
            user.SaltHash = null;
        }

        /// <summary>
        /// Check whether the password meets the rules set.
        /// </summary>
        /// <param name="password">Password to verify.</param>
        /// <returns>Whether the password meets the rules.</returns>
        public bool PasswordPassesRules(string password)
        {
            PasswordHandler handler = new PasswordHandler();
            return handler.PasswordMeetsRules(password);
        }
    }
}
