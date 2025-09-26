using Kms.Common.Util;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Kms.Security.Util
{
    public class RSAHelper
    {

        public static string Decrypt(string encryptedText, string privateKey)
        {
            try
            {
                if (string.IsNullOrEmpty(privateKey) || string.IsNullOrWhiteSpace(privateKey))
                    return default;

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                AsymmetricCipherKeyPair keyPair;

                using (var reader = new StringReader(privateKey))
                {
                    keyPair = (AsymmetricCipherKeyPair)new PemReader(reader).ReadObject();
                }

                var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)keyPair.Private));

                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                return "Decryption Error: " + ex.Message;
            }
        }


        public static GenerateKeyDto GenerateKeys()
        {
            var keyGenerator = new RsaKeyPairGenerator();
            keyGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            var keyPair = keyGenerator.GenerateKeyPair();
            return new GenerateKeyDto
            {
                KeyId = Guid.NewGuid().ToString(),
                PublicKeyString = ConvertPublicKeyToPem(keyPair.Public as RsaKeyParameters),
                PrivateKeyString = ConvertPublicKeyToPem(keyPair.Private as RsaKeyParameters),
                PrivateKey = keyPair.Private
            };
        }

        private static string ConvertPublicKeyToPem(RsaKeyParameters publicKey)
        {
            using (TextWriter textWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(textWriter);
                pemWriter.WriteObject(publicKey);
                pemWriter.Writer.Flush();
                return textWriter.ToString();
            }
        }

    }


}
