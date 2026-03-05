//
// MD4.cs: MD4 Encryption algorithm implementation in managed code.
//

using System;
using System.Text;

namespace CryptAPI
{
	/// <summary>
	/// MD4 Hash generator.
	/// Ported to C# using:
	///    http://www.zvon.cz/tmRFC/RFC1186/Output/chapter7.html
	/// </summary>
	public class MD4 : HashAlgorithm
	{
		private const uint I0 = 0x67452301;
		private const uint I1 = 0xefcdab89;
		private const uint I2 = 0x98badcfe;
		private const uint I3 = 0x10325476;
		private const int fs1 = 3;
		private const int fs2 = 7;
		private const int fs3 = 11;
		private const int fs4 = 19;
		private const int gs1 = 3;
		private const int gs2 = 5;
		private const int gs3 = 9;
		private const int gs4 = 13;
		private const int hs1 = 3;
		private const int hs2 = 9;
		private const int hs3 = 11;
		private const int hs4 = 15;
		public class MDstruct {
			public uint[] buffer = new uint[4];
			public byte[] count = new byte[8];
			public uint done;
		}
		public string MDToString(MDstruct MDp)
		{
			string MDstr = "";
			for (int i = 0 ; i < 4 ; i++)
				for (int j = 0 ; j < 32 ; j += 8)
					MDstr += String.Format("{0:x2}", ((MDp.buffer[i] >> j) & 0xFF));
			return MDstr;
		}
		public void MDbegin(ref MDstruct MDp)
		{
			MDp.buffer[0] = I0;
			MDp.buffer[1] = I1;
			MDp.buffer[2] = I2;
			MDp.buffer[3] = I3;
			for (int i = 0 ; i < 8 ; i++)
				MDp.count[i] = 0;
			MDp.done = 0;
		}
/* ONLY USED FOR BIG-ENDIAN MACHINES!
		private void MDreverse(ref uint[] X)
		{
			uint t;
			for (int i = 0 ; i < 16 ; i++) {
				t = ((X[i] << 16) | (X[i] >> 16));
				X[i] = (((t & 0xFF00FF00) >> 8) |((t & 0x00FF00FF) << 8));
			}
		}
*/
		private void ff(ref uint a, uint b, uint c, uint d, uint x, int s)
		{
			uint t = (a + ((b & c) | (~b & d)) + x);
			a = ((t << s) | (t >> (32 - s)));
		}
		private void gg(ref uint a, uint b, uint c, uint d, uint x, int s)
		{
			uint t = (a + ((b & (c | d)) | (c & d)) + x + 0x5A827999);
			a = ((t << s) | (t >> (32 - s)));
		}
		private void hh(ref uint a, uint b, uint c, uint d, uint x, int s)
		{
			uint t = (a + (b ^ c ^ d) + x + 0x6ED9EBA1);
			a = ((t << s) | (t >> (32 - s)));
		}
		private void MDblock(ref MDstruct MDp, ref uint[] X)
		{
			uint A, B, C, D;
//			MDreverse(ref X);
			A = MDp.buffer[0];
			B = MDp.buffer[1];
			C = MDp.buffer[2];
			D = MDp.buffer[3];
			ff(ref A , B , C , D ,  X[0] , fs1);
			ff(ref D , A , B , C ,  X[1] , fs2);
			ff(ref C , D , A , B ,  X[2] , fs3);
			ff(ref B , C , D , A ,  X[3] , fs4);
			ff(ref A , B , C , D ,  X[4] , fs1);
			ff(ref D , A , B , C ,  X[5] , fs2);
			ff(ref C , D , A , B ,  X[6] , fs3);
			ff(ref B , C , D , A ,  X[7] , fs4);
			ff(ref A , B , C , D ,  X[8] , fs1);
			ff(ref D , A , B , C ,  X[9] , fs2);
			ff(ref C , D , A , B , X[10] , fs3);
			ff(ref B , C , D , A , X[11] , fs4);
			ff(ref A , B , C , D , X[12] , fs1);
			ff(ref D , A , B , C , X[13] , fs2);
			ff(ref C , D , A , B , X[14] , fs3);
			ff(ref B , C , D , A , X[15] , fs4);
			gg(ref A , B , C , D ,  X[0] , gs1);
			gg(ref D , A , B , C ,  X[4] , gs2);
			gg(ref C , D , A , B ,  X[8] , gs3);
			gg(ref B , C , D , A , X[12] , gs4);
			gg(ref A , B , C , D ,  X[1] , gs1);
			gg(ref D , A , B , C ,  X[5] , gs2);
			gg(ref C , D , A , B ,  X[9] , gs3);
			gg(ref B , C , D , A , X[13] , gs4);
			gg(ref A , B , C , D ,  X[2] , gs1);
			gg(ref D , A , B , C ,  X[6] , gs2);
			gg(ref C , D , A , B , X[10] , gs3);
			gg(ref B , C , D , A , X[14] , gs4);
			gg(ref A , B , C , D ,  X[3] , gs1);
			gg(ref D , A , B , C ,  X[7] , gs2);
			gg(ref C , D , A , B , X[11] , gs3);
			gg(ref B , C , D , A , X[15] , gs4);
			hh(ref A , B , C , D ,  X[0] , hs1);
			hh(ref D , A , B , C ,  X[8] , hs2);
			hh(ref C , D , A , B ,  X[4] , hs3);
			hh(ref B , C , D , A , X[12] , hs4);
			hh(ref A , B , C , D ,  X[2] , hs1);
			hh(ref D , A , B , C , X[10] , hs2);
			hh(ref C , D , A , B ,  X[6] , hs3);
			hh(ref B , C , D , A , X[14] , hs4);
			hh(ref A , B , C , D ,  X[1] , hs1);
			hh(ref D , A , B , C ,  X[9] , hs2);
			hh(ref C , D , A , B ,  X[5] , hs3);
			hh(ref B , C , D , A , X[13] , hs4);
			hh(ref A , B , C , D ,  X[3] , hs1);
			hh(ref D , A , B , C , X[11] , hs2);
			hh(ref C , D , A , B ,  X[7] , hs3);
			hh(ref B , C , D , A , X[15] , hs4);
			MDp.buffer[0] += A;
			MDp.buffer[1] += B;
			MDp.buffer[2] += C;
			MDp.buffer[3] += D;
		}
		public void MDupdate(ref MDstruct MDp, ref byte[] X, uint count)
		{
			uint tmp, bit, _byte, mask;
			byte[] XX = new byte[64];
			uint[] X_uint;
			if (count == 0 && Convert.ToBoolean(MDp.done)) return;
			if (Convert.ToBoolean(MDp.done))
				return;
			tmp = count;
			for (int i = 0 ; Convert.ToBoolean(tmp) ; ) {
				tmp += MDp.count[i];
				MDp.count[i++] = (byte)tmp;
				tmp >>= 8;
			}
			if (count == 512) {
				X_uint = ArrayByteToUint(X);
				MDblock(ref MDp, ref X_uint);
				X = ArrayUintToByte(X_uint);
			}
			else if (count > 512)
				return;
			else {
				_byte = count >> 3;
				bit = count & 7;
				for (int i = 0 ; i <= _byte ; i++) XX[i] = X[i];
				for (uint i = _byte + 1 ; i < 64 ; i++) XX[i] = 0;
				mask = (uint)(1 << (int)(7 - bit));
				XX[_byte] = (byte)((XX[_byte] | (byte)mask) &  (~(mask - 1)));
				if (_byte <= 55) {
					for (int i = 0 ; i < 8 ; i++)
						XX[56 + i] = MDp.count[i];
					uint[] XX_uint = ArrayByteToUint(XX);
					MDblock(ref MDp, ref XX_uint);
					XX = ArrayUintToByte(XX_uint);
				}
				else {
					uint[] XX_uint = ArrayByteToUint(XX);
					MDblock(ref MDp, ref XX_uint);
					XX = ArrayUintToByte(XX_uint);
					for (int i = 0 ; i < 56 ; i++) XX[i] = 0;
					for (int i = 0 ; i < 8 ; i++) XX[56 + i] = MDp.count[i];
					XX_uint = ArrayByteToUint(XX);
					MDblock(ref MDp, ref XX_uint);
					XX = ArrayUintToByte(XX_uint);
				}
				MDp.done = 1;
			}
		}
		private uint[] ArrayByteToUint(byte[] b)
		{
			uint[] u;
			if ((b.Length % 4) != 0) {
				u = new uint[(b.Length / 4) + 1];
				for (int i = 0 ; i < (b.Length / 4) ; i++)
					u[i] = BitConverter.ToUInt32(b, i * 4);
				byte[] extra = new byte[4];
				for (int i = 0 ; i < (b.Length % 4) ; i++)
					extra[i] = b[b.Length - (b.Length % 4) + i];
				for (int i = (b.Length % 4) ; i < 4; i++)
					extra[i] = 0x00;
				u[(b.Length / 4)] = BitConverter.ToUInt32(extra, 0);
			}
			else {
				u = new uint[b.Length / 4];
				for (int i = 0 ; i < (b.Length / 4) ; i++)
					u[i] = BitConverter.ToUInt32(b, i * 4);
			}
			return u;
		}
		private byte[] ArrayUintToByte(uint[] u)
		{
			byte[] b = new byte[u.Length * 4];
			byte[] tmp;
			for (int i = 0 ; i < u.Length ; i++) {
				tmp = BitConverter.GetBytes(u[i]);
				for (int j = 0 ; j < 4 ; j++)
					b[(i * 4) + j] = tmp[j];
			}
			return b;
		}
		private byte[] E_md4hash(string passwd)
		{
			byte[] p16 = new byte[16];
			byte[] wpwd_t = (new UnicodeEncoding()).GetBytes(passwd);
			byte[] wpwd = new byte[wpwd_t.Length + 1];
			byte[] wpwd_cut;
			MDstruct MD = new MDstruct();
			MDbegin(ref MD);
			int len = passwd.Length * 2;
			int i;
			Array.Copy(wpwd_t, 0, wpwd, 0, wpwd_t.Length);
			wpwd[len] = 0x00;
			for (i = 0 ; (i + 64) <= len ; i += 64) {
				wpwd_cut = new byte[(wpwd.Length - (i / 2))];
				Array.Copy(wpwd, (i / 2), wpwd_cut, 0, (wpwd.Length - (i / 2)));
				MDupdate(ref MD, ref wpwd_cut, 512);
				Array.Copy(wpwd_cut, 0, wpwd, (i / 2), wpwd_cut.Length);
			}
			wpwd_cut = new byte[(wpwd.Length - (i / 2))];
			Array.Copy(wpwd, (i / 2), wpwd_cut, 0, (wpwd.Length - (i / 2)));
			MDupdate(ref MD, ref wpwd_cut, (uint)((len - i) * 8));
			Array.Copy(wpwd_cut, 0, wpwd, (i / 2), wpwd.Length);
			p16 = ArrayUintToByte(MD.buffer);
			return p16;
		}
		
		/// <summary>This method encrypt the given string, using NTLM Algorithm</summary>
		/// <param name="pwd">The password to be encrypted.</param>
		/// <returns>The encrypted string </returns>
		public string EncryptNTLM(string pwd)
		{
			string password;
			string NTLMHash = "";
			if (pwd.Length > 128)
				password = pwd.Remove(127,pwd.Length);
			byte[] nt_p16 = E_md4hash(pwd);
			for (int i = 0 ; i < 16 ; i++)
				NTLMHash += String.Format("{0:x2}",nt_p16[i]);
			return NTLMHash;
		}

		/// <summary> This method encrypt the given string, using MD4 Algorithm</summary>
		/// <param name="s">The string to be encrypted. </param>
		/// <returns>The encrypted password.</returns>
		public override string Encrypt(string s)
		{
			byte[] wpwd;
			byte[] wpwd_t;
			byte[] wpwd_cut;
			int i = 0, len;
			wpwd_t = (new ASCIIEncoding()).GetBytes(s);
			len = wpwd_t.Length;
			wpwd = new byte[wpwd_t.Length + 1];
			Array.Copy(wpwd_t, 0, wpwd, 0, wpwd_t.Length);
			wpwd[wpwd_t.Length] = 0;
			MDstruct MD = new MDstruct();
			MDbegin(ref MD);
			for (i = 0 ; (i + 64) <= len ; i += 64) {
				wpwd_cut = new byte[len - i + 1];
				Array.Copy(wpwd, i, wpwd_cut, 0, wpwd_cut.Length);
				wpwd_cut[len - i] = 0;
				MDupdate(ref MD, ref wpwd_cut, 512);
				Array.Copy(wpwd_cut, 0, wpwd, i, wpwd_cut.Length);
			}
			wpwd_cut = new byte[len - i + 1];
			Array.Copy(wpwd, i, wpwd_cut, 0, wpwd_cut.Length);
			wpwd_cut[len - i] = 0;
			MDupdate(ref MD, ref wpwd_cut, (uint)((len - i) * 8));
			Array.Copy(wpwd_cut, 0, wpwd, i, wpwd_cut.Length);
			return MDToString(MD);
		}
		public MD4() {}
		
		public override string Encrypt(string s, string p) { return null; }
	}
}
