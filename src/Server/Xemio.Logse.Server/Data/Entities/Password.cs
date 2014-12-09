using System.Linq;
using Xemio.Logse.Server.Encryption;

namespace Xemio.Logse.Server.Data.Entities
{
    public class Password
    {
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }

        public void Change(string newHash)
        {
            this.Salt = SecretGenerator.Generate();
            this.Hash = SaltCombiner.Combine(this.Salt, newHash);
        }

        public bool IsCorrect(string hash)
        {
            byte[] computedHash = SaltCombiner.Combine(this.Salt, hash);
            return this.Hash.SequenceEqual(computedHash);
        }
    }
}