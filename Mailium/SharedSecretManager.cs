using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailium
{
    internal static class SharedSecretManager
    {
        private static string _sharedSecret;

        public static void SetSharedSecret(string sharedSecret)
        {
            _sharedSecret = sharedSecret;
        }

        public static string GetSharedSecret()
        {
            return _sharedSecret;
        }
    }
}
