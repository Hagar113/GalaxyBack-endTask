using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.helpers.IHelper
{
    public interface IEncryption
    {
        string Decrypt(string Input);
        string Encrypt(string Input);

    }
}
