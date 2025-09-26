using Org.BouncyCastle.Crypto;

namespace Kms.Security.Util
{
    public class GenerateKeyDto
    {
        public string KeyId { get; set; }
        public string PrivateKeyString { get; set; }
        public string PublicKeyString { get; set; }
        public AsymmetricKeyParameter PublicKey { get; set; }
        public AsymmetricKeyParameter PrivateKey { get; set; }

    }

}
