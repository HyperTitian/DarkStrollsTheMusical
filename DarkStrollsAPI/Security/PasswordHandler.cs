using DarkStrollsAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Security
{
    /// <summary>
    /// Handles password hashing and comparison.
    /// Passwords have a 128 bit salt and 256 bit subkey and 10,000 iterations.
    /// </summary>
    public class PasswordHandler
    {
        /// <summary>
        /// Number of bits to use for the salt.
        /// </summary>
        public const int SaltBits = 128;

        /// <summary>
        /// Number of bits to use for the hash.
        /// </summary>
        public const int HashBits = 256;

        /// <summary>
        /// Number of iterations to hash.
        /// </summary>
        public const int Iterations = 11111;

        /// <summary>
        /// Create an all new hash and salt for the given password.
        /// </summary>
        /// <param name="password">Password to hash.</param>
        /// <returns>A byte array with the combined hash and salt.</returns>
        public byte[] CreateHash(string password)
        {
            // Create array to hold salt.
            byte[] salt = new byte[SaltBits / 8];

            // Randomly generate a salt.
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password.
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashBits / 8);

            // Combine the salt and hash.
            byte[] saltHash = new byte[(SaltBits + HashBits) / 8];
            Array.Copy(salt, saltHash, salt.Length);
            Array.Copy(hashed, 0, saltHash, SaltBits / 8, hashed.Length);

            // Return the combined salt and hash.
            return saltHash;
        }

        /// <summary>
        /// Checks a password against the given saltHash.
        /// </summary>
        /// <param name="password">Password to check.</param>
        /// <param name="saltHash">Salthash to check against.</param>
        /// <returns>Whether the password matches.</returns>
        public bool CheckPassword(string password, byte[] saltHash)
        {
            // Seperate the salt from the given saltHash.
            byte[] salt = new byte[SaltBits / 8];
            Array.Copy(saltHash, salt, salt.Length);

            // Hash the given password with the salt.
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashBits / 8);

            // Compare the given hash with the generated hash.
            for (int i = 0; i < hashed.Length; i++)
            {
                if (saltHash[i + SaltBits / 8] != hashed[i])
                {
                    // Return false if it doesn't match.
                    return false;
                }
            }

            // Return true for a match.
            return true;
        }

        /// <summary>
        /// Returns true if password meets all set guidelines.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <returns></returns>
        public bool PasswordMeetsRules(string password)
        {
            if(string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reset the given user's password.
        /// </summary>
        /// <param name="employeeNumber">The user to reset the password of.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task ResetUserPassword(string username)
        {
            // Create the context.
            DarkDbContext context = new DarkDbContext();

            // Get the user.
            var user = await context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();

            // Validate the selection.
            if (user is null)
            {
                throw new ArgumentException($"The supplied user \"{username}\" does not exist!", nameof(username));
            }

            // Update the user.
            user.SaltHash = null;
            await context.SaveChangesAsync();
            await context.DisposeAsync();
        }
    }
}