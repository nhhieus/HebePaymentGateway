using System;
using System.Linq;

namespace GenericPayment.Utilities
{
    public static class Utils
    {
        private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomID(int length)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            
            return result;
        }
    }
}