using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    interface IHxCrypt
    {
        string Base64Decode(string value);
        string Base64Encode(string value);
        string Decrypt(string value, string key);
        string Encrypt(string value, string key);
        string Md5(string value);
        string RandPass();
    }
}
