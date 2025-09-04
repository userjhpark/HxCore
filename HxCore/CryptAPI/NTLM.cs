//
// NTLM.cs: NTLM Encryption algorithm implementation in managed code.
//

using System;
using System.Text;
namespace CryptAPI
{
	/// <summary>
	/// NTLM Hash generator
	/// Ported from samba's:
	///	   smbencrypt.c
	///	   md4.c
	///	   md4.h
	/// </summary>
	public class NTLM : HashAlgorithm
	{
		private uint A, B, C, D;
		public NTLM() {}
		private void ROUND1(ref uint a, uint b, uint c, uint d, uint Xk, int s)
		{
			a = lshift(a + F(b, c, d) + Xk, s);
		}
		private void ROUND2(ref uint a, uint b, uint c, uint d, uint Xk, int s)
		{
			a = lshift(a + G(b, c, d) + Xk + (uint)0x5A827999, s);
		}
		private void ROUND3(ref uint a, uint b, uint c, uint d, uint Xk, int s)
		{
			a = lshift(a + H(b, c, d) + Xk + (uint)0x6ED9EBA1, s);
		}
		private uint F(uint X, uint Y, uint Z) { return (X & Y) | ((~X) & Z); }
		private uint G(uint X, uint Y, uint Z) { return (X & Y) | (X & Z) | (Y & Z); }
		private uint H(uint X, uint Y, uint Z) { return X ^ Y ^ Z; }
		private uint lshift(uint x, int s)
		{
			x &= 0xFFFFFFFF;
			return ((x << s) & 0xFFFFFFFF) | (x >> (32 - s));
		}
		private void mdfour64(uint[] M)
		{
			uint AA, BB, CC, DD;
			uint[] X = new uint[16];
			for (int j = 0 ; j < 16 ; j++)
				X[j] = M[j];
			AA = A; BB = B; CC = C; DD = D;
			ROUND1(ref A,B,C,D,  X[0],  3);  ROUND1(ref D,A,B,C,  X[1],  7);
			ROUND1(ref C,D,A,B,  X[2], 11);  ROUND1(ref B,C,D,A,  X[3], 19);
			ROUND1(ref A,B,C,D,  X[4],  3);  ROUND1(ref D,A,B,C,  X[5],  7);
			ROUND1(ref C,D,A,B,  X[6], 11);  ROUND1(ref B,C,D,A,  X[7], 19);
			ROUND1(ref A,B,C,D,  X[8],  3);  ROUND1(ref D,A,B,C,  X[9],  7);
			ROUND1(ref C,D,A,B, X[10], 11);  ROUND1(ref B,C,D,A, X[11], 19);
			ROUND1(ref A,B,C,D, X[12],  3);  ROUND1(ref D,A,B,C, X[13],  7);
			ROUND1(ref C,D,A,B, X[14], 11);  ROUND1(ref B,C,D,A, X[15], 19);

			ROUND2(ref A,B,C,D,  X[0],  3);  ROUND2(ref D,A,B,C,  X[4],  5);
			ROUND2(ref C,D,A,B,  X[8],  9);  ROUND2(ref B,C,D,A, X[12], 13);
			ROUND2(ref A,B,C,D,  X[1],  3);  ROUND2(ref D,A,B,C,  X[5],  5);
			ROUND2(ref C,D,A,B,  X[9],  9);  ROUND2(ref B,C,D,A, X[13], 13);
			ROUND2(ref A,B,C,D,  X[2],  3);  ROUND2(ref D,A,B,C,  X[6],  5);
			ROUND2(ref C,D,A,B, X[10],  9);  ROUND2(ref B,C,D,A, X[14], 13);
			ROUND2(ref A,B,C,D,  X[3],  3);  ROUND2(ref D,A,B,C,  X[7],  5);
			ROUND2(ref C,D,A,B, X[11],  9);  ROUND2(ref B,C,D,A, X[15], 13);

			ROUND3(ref A,B,C,D,  X[0],  3);  ROUND3(ref D,A,B,C,  X[8],  9);
			ROUND3(ref C,D,A,B,  X[4], 11);  ROUND3(ref B,C,D,A, X[12], 15);
			ROUND3(ref A,B,C,D,  X[2],  3);  ROUND3(ref D,A,B,C, X[10],  9);
			ROUND3(ref C,D,A,B,  X[6], 11);  ROUND3(ref B,C,D,A, X[14], 15);
			ROUND3(ref A,B,C,D,  X[1],  3);  ROUND3(ref D,A,B,C,  X[9],  9);
			ROUND3(ref C,D,A,B,  X[5], 11);  ROUND3(ref B,C,D,A, X[13], 15);
			ROUND3(ref A,B,C,D,  X[3],  3);  ROUND3(ref D,A,B,C, X[11],  9);
			ROUND3(ref C,D,A,B,  X[7], 11);  ROUND3(ref B,C,D,A, X[15], 15);

			A += AA; B += BB; C += CC; D += DD;

			A &= 0xFFFFFFFF; B &= 0xFFFFFFFF;
			C &= 0xFFFFFFFF; D &= 0xFFFFFFFF;
			for (int j = 0 ; j < 16 ; j++)
				X[j] = 0;
		}
		private void copy64(ref uint[] M, byte[] _in)
		{
			for (int i = 0 ; i < 16 ; i++)
				M[i] = (uint)((_in[i * 4 + 3] << (byte)24) | (_in[i * 4 + 2] << (byte)16) |
						(_in[i * 4 + 1] << (byte)8) | (_in[i * 4 +0 ] << (byte)0));
		}
		private void copy4(ref byte[] _out, int i, uint x)
		{
			_out[i + 0] = (byte)(x & 0xFF);
			_out[i + 1] = (byte)((x >> 8) & 0xFF);
			_out[i + 2] = (byte)((x >> 16) & 0xFF);
			_out[i + 3] = (byte)((x >> 24) & 0xFF);
		}
		private void mdfour(ref byte[] _out, byte[] _in, int n)
		{
			byte[] buf = new byte[128];
			uint[] M = new uint[16];
			int i, d = 0;
			uint b = (uint)(n * 8);
			A = 0x67452301;
			B = 0xefcdab89;
			C = 0x98badcfe;
			D = 0x10325476;
			while (n > 64) {
				byte[] _in_64 = new byte[64];
				for (i = 0 ; i < 64 ; i++)
					_in_64[i] = _in[i + d];
				d += i;
				copy64(ref M, _in_64);
				mdfour64(M);
				n -= 64;
			}
			for (i = 0 ; i < 128 ; i++)
				buf[i] = 0;
			for (i = 0 ; i < n ; i++)
				buf[i] = _in[i];
			buf[n] = (byte)0x80;
			if (n <= 55) {
				copy4(ref buf, 56, b);
				copy64(ref M, buf);
				mdfour64(M);
			}
			else {
				copy4(ref buf, 120, b);
				copy64(ref M, buf);
				mdfour64(M);
				byte[] _buf = new byte[128 - 65];
				for (i = 0 ; i < 128 ; i++)
					_buf[i] = buf[i + 64];
				copy64(ref M, _buf);
				mdfour64(M);
			}
			for (i = 0 ; i < 128 ; i++)
				buf[i] = 0;
			copy64(ref M, buf);
			copy4(ref _out, 0, A);
			copy4(ref _out, 4, B);
			copy4(ref _out, 8, C);
			copy4(ref _out, 12, D);
			A = B = C = D = 0;
		}
		private byte[] E_md4hash(string passwd)
		{
			byte[] p16 = new byte[16];
			byte[] wpwd = (new UnicodeEncoding()).GetBytes(passwd);
			mdfour(ref p16, wpwd, passwd.Length * 2);
			return p16;
		}
		
		/// <summary>This method encrypt the given string, using NTLM Algorithm</summary>
		/// <param name="pwd">The password to be encrypted.</param>
		/// <returns>The encrypted password.</returns>
		public override string Encrypt(string s)
		{
			string password;
			string NTLMHash = "";
			if (s.Length > 128)
				password = s.Remove(127,s.Length);
			byte[] nt_p16 = E_md4hash(s);
			for (int i = 0 ; i < 16 ; i++)
				NTLMHash += String.Format("{0:x2}",nt_p16[i]);
			return NTLMHash;
		}
		
		public override string Encrypt(string s, string p) { return null; }
	}
}
