using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mailium
{
    internal class DHKeyExchange
    {
        private ECDiffieHellmanCng clientECDH;

        public DHKeyExchange()
        {
            clientECDH = new ECDiffieHellmanCng();
            clientECDH.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            clientECDH.HashAlgorithm = CngAlgorithm.Sha256;
        }

        public string GetClientPublicKey()
        {
            return Convert.ToBase64String(clientECDH.PublicKey.ToByteArray());
        }

        public string GenerateSharedSecret(string serverPublicKey)
        {
            byte[] serverPublicKeyBytes = Convert.FromBase64String(serverPublicKey);
            byte[] sharedSecret = clientECDH.DeriveKeyMaterial(ECDiffieHellmanCngPublicKey.FromByteArray(serverPublicKeyBytes, CngKeyBlobFormat.EccPublicBlob));
            return Convert.ToBase64String(sharedSecret);
        }
    }
}
