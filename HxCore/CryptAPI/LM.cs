//
// LM.cs: LM Encryption algorithm implementation in managed code.
//

using System;
namespace CryptAPI
{
	/// <summary>
	/// LANMAN Hash generator
	/// Ported to C# using samba's:
	///    libsmb/smbencrypt.c
	///    libsmb/smbdes.c
	/// </summary>
	public class LM : HashAlgorithm
	{
		private
		byte[] perm1 = {57, 49, 41, 33, 25, 17, 9,
						1, 58, 50, 42, 34, 26, 18,
						10, 2, 59, 51, 43, 35, 27,
						19, 11, 3, 60, 52, 44, 36,
						63, 55, 47, 39, 31, 23, 15,
						7, 62, 54, 46, 38, 30, 22,
						14, 6, 61, 53, 45, 37, 29,
						21, 13, 5, 28, 20, 12, 4};
		private
		byte[] perm2 = {14, 17, 11, 24, 1, 5,
						3, 28, 15, 6, 21, 10,
						23, 19, 12,  4, 26, 8,
						16, 7, 27, 20, 13, 2,
						41, 52, 31, 37, 47, 55,
						30, 40, 51, 45, 33, 48,
						44, 49, 39, 56, 34, 53,
						46, 42, 50, 36, 29, 32};
		private
		byte[] perm3 = {58, 50, 42, 34, 26, 18, 10, 2,
						60, 52, 44, 36, 28, 20, 12, 4,
						62, 54, 46, 38, 30, 22, 14, 6,
						64, 56, 48, 40, 32, 24, 16, 8,
						57, 49, 41, 33, 25, 17,  9, 1,
						59, 51, 43, 35, 27, 19, 11, 3,
						61, 53, 45, 37, 29, 21, 13, 5,
						63, 55, 47, 39, 31, 23, 15, 7};
		private
		byte[] perm4 = {32, 1, 2, 3, 4, 5,
						4, 5, 6, 7, 8, 9,
						8,  9, 10, 11, 12, 13,
						12, 13, 14, 15, 16, 17,
						16, 17, 18, 19, 20, 21,
						20, 21, 22, 23, 24, 25,
						24, 25, 26, 27, 28, 29,
						28, 29, 30, 31, 32, 1};
		private
		byte[] perm5 = {16, 7, 20, 21,
						29, 12, 28, 17,
						1, 15, 23, 26,
						5, 18, 31, 10,
						2, 8, 24, 14,
						32, 27, 3, 9,
						19, 13, 30, 6,
						22, 11, 4, 25};
		private
		byte[] perm6 = {40, 8, 48, 16, 56, 24, 64, 32,
						39, 7, 47, 15, 55, 23, 63, 31,
						38, 6, 46, 14, 54, 22, 62, 30,
						37, 5, 45, 13, 53, 21, 61, 29,
						36, 4, 44, 12, 52, 20, 60, 28,
						35, 3, 43, 11, 51, 19, 59, 27,
						34, 2, 42, 10, 50, 18, 58, 26,
						33, 1, 41,  9, 49, 17, 57, 25};
		private
		byte[] sc = {1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};
		private
		byte[,,] sbox = {
		{{14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7},
		{0, 15,  7,  4, 14,  2, 13,  1, 10,  6, 12, 11,  9,  5,  3,  8},
		{4,  1, 14,  8, 13,  6,  2, 11, 15, 12,  9,  7,  3, 10,  5,  0},
		{15, 12,  8,  2,  4,  9,  1,  7,  5, 11,  3, 14, 10,  0,  6, 13}},

		{{15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10},
		{3, 13,  4,  7, 15,  2,  8, 14, 12,  0,  1, 10,  6,  9, 11,  5},
		{0, 14,  7, 11, 10,  4, 13,  1,  5,  8, 12,  6,  9,  3,  2, 15},
		{13,  8, 10,  1,  3, 15,  4,  2, 11,  6,  7, 12,  0,  5, 14,  9}},

		{{10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8},
		{13,  7,  0,  9,  3,  4,  6, 10,  2,  8,  5, 14, 12, 11, 15,  1},
		{13,  6,  4,  9,  8, 15,  3,  0, 11,  1,  2, 12,  5, 10, 14,  7},
		{1, 10, 13,  0,  6,  9,  8,  7,  4, 15, 14,  3, 11,  5,  2, 12}},

		{{7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15},
		{13,  8, 11,  5,  6, 15,  0,  3,  4,  7,  2, 12,  1, 10, 14,  9},
		{10,  6,  9,  0, 12, 11,  7, 13, 15,  1,  3, 14,  5,  2,  8,  4},
		{3, 15,  0,  6, 10,  1, 13,  8,  9,  4,  5, 11, 12,  7,  2, 14}},

		{{2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9},
		{14, 11,  2, 12,  4,  7, 13,  1,  5,  0, 15, 10,  3,  9,  8,  6},
		{4,  2,  1, 11, 10, 13,  7,  8, 15,  9, 12,  5,  6,  3,  0, 14},
		{11,  8, 12,  7,  1, 14,  2, 13,  6, 15,  0,  9, 10,  4,  5,  3}},

		{{12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11},
		{10, 15,  4,  2,  7, 12,  9,  5,  6,  1, 13, 14,  0, 11,  3,  8},
		{9, 14, 15,  5,  2,  8, 12,  3,  7,  0,  4, 10,  1, 13, 11,  6},
		{4,  3,  2, 12,  9,  5, 15, 10, 11, 14,  1,  7,  6,  0,  8, 13}},

		{{4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1},
		{13,  0, 11,  7,  4,  9,  1, 10, 14,  3,  5, 12,  2, 15,  8,  6},
		{1,  4, 11, 13, 12,  3,  7, 14, 10, 15,  6,  8,  0,  5,  9,  2},
		{6, 11, 13,  8,  1,  4, 10,  7,  9,  5,  0, 15, 14,  2,  3, 12}},

		{{13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7},
		{1, 15, 13,  8, 10,  3,  7,  4, 12,  5,  6, 11,  0, 14,  9,  2},
		{7, 11,  4,  1,  9, 12, 14,  2,  0,  6, 10, 13, 15,  3,  5,  8},
		{2,  1, 14,  7,  4, 10,  8, 13, 15, 12,  9,  0,  3,  5,  6, 11}}};

		public LM() {}
		private void permute(ref sbyte[] _out, sbyte[] _in, byte[] p, int n)
		{
			for (int i = 0 ; i < n ; i++)
				_out[i] = _in[p[i] - 1];

		}
		private void lshift(ref sbyte[] d, int count, int n)
		{
			sbyte[] _out = new sbyte[64];
			for (int i = 0 ; i < n ; i++)
				_out[i] = d[(i + count) % n];
			for (int i = 0 ; i < n ; i++)
				d[i] = _out[i];
		}
		private void concat(ref sbyte[] _out, sbyte[] in1, sbyte[] in2, int l1,
							int l2)
		{
			int i, d;
			for (i = 0 ; i < l1 ; i++)
				_out[i] = in1[i];
			d = i;
			for (i = 0 ; i < l2 ; i++)
				_out[d + i] = in2[i];
		}
		private void xor(ref sbyte[] _out, sbyte[] in1, sbyte[] in2, int n)
		{
			for (int i = 0 ; i < n ; i++)
				_out[i] = (sbyte)(in1[i] ^ in2[i]);
		}
		private sbyte[] dohash(sbyte[] _in, sbyte[] key, int forw)
		{
			sbyte[] pk1 = new sbyte[56];
			sbyte[] c = new sbyte[28];
			sbyte[] d = new sbyte[28];
			sbyte[] cd = new sbyte[56];
			object[] ki = new object[16];
			sbyte[] pd1 = new sbyte[64];
			sbyte[] l = new sbyte[32];
			sbyte[] r = new sbyte[32];
			sbyte[] rl = new sbyte[64];
			sbyte[] _out = new sbyte[64];
			for (int i = 0 ; i < 16 ; i++)
				ki[i] = new sbyte[48];
			permute(ref pk1, key, perm1, 56);
			for (int i = 0 ; i < 28 ; i++)
				c[i] = pk1[i];
			for (int i = 0 ; i < 28 ; i++)
				d[i] = pk1[i + 28];
			for (int i = 0 ; i < 16 ; i++) {
				lshift(ref c, sc[i], 28);
				lshift(ref d, sc[i], 28);
				concat(ref cd, c, d, 28, 28);
				sbyte[] t = (sbyte[])ki[i];
				permute(ref t, cd, perm2, 48);
			}
			permute(ref pd1, _in, perm3, 64);
			for (int j = 0 ; j < 32 ; j++) {
				l[j] = pd1[j];
				r[j] = pd1[j + 32];
			}
			for (int i = 0 ; i < 16 ; i++) {
				sbyte[] er = new sbyte[48];
				sbyte[] erk = new sbyte[48];
				sbyte[,] b = new sbyte[8,6];
				sbyte[] cb = new sbyte[32];
				sbyte[] pcb = new sbyte[32];
				sbyte[] r2 = new sbyte[32];
				permute(ref er, r, perm4, 48);
				sbyte[] t = (sbyte[])ki[Convert.ToBoolean(forw) ? i : 15 - i];
				xor(ref erk, er, t, 48);
				for (int j = 0 ; j < 8 ; j++)
					for (int k = 0 ; k < 6 ; k++)
						b[j,k] = erk[j * 6 + k];
				for (int j = 0 ; j < 8 ; j++) {
					int m, n;
					m = (int)(((int)(b[j,0] << 1)) | ((int)b[j,5]));
					n = (int)((int)(b[j,1] << 3) | (int)(b[j,2] << 2) | (int)(b[j,3] << 1)
						| ((int)b[j,4]));
					for (int k = 0 ; k < 4 ; k++)
						b[j,k] = (sbyte)(Convert.ToBoolean(sbox[j,m,n] & (1 << (3 - k))) ? 1 : 0);
				}
				for (int j = 0 ; j < 8 ; j++)
					for (int k = 0 ; k < 4 ; k++)
						cb[j * 4 + k] = b[j,k];
				permute(ref pcb, cb, perm5, 32);
				xor(ref r2, l, pcb, 32);
				for (int j = 0 ; j < 32 ; j++)
					l[j] = r[j];
				for (int j = 0 ; j < 32 ; j++)
					r[j] = r2[j];
			}
			concat(ref rl, r, l, 32, 32);
			permute(ref _out, rl, perm6, 64);
			return _out;
		}
		private byte[] str_to_key(byte[] str)
		{
			byte[] key = new byte[8];
			key[0] = (byte)(str[0] >> 1);
			key[1] = (byte)(((str[0] & 0x01) << 6) | (str[1] >> 2));
			key[2] = (byte)(((str[1] & 0x03) << 5) | (str[2] >> 3));
			key[3] = (byte)(((str[2] & 0x07) << 4) | (str[3] >> 4));
			key[4] = (byte)(((str[3] & 0x0F) << 3) | (str[4] >> 5));
			key[5] = (byte)(((str[4] & 0x1F) << 2) | (str[5] >> 6));
			key[6] = (byte)(((str[5] & 0x3F) << 1) | (str[6] >> 7));
			key[7] = (byte)(str[6] & 0x7F);
			for (int i = 0 ; i < 8 ; i++)
				key[i] = (byte)(key[i] << 1);
			return key;
		}
		private byte[] smbhash(byte[] _in, string strKey, int forw)
		{
			byte[] _out = new byte[8];
			sbyte[] outb = new sbyte[64];
			sbyte[] inb = new sbyte[64];
			sbyte[] keyb = new sbyte[64];
			System.Text.ASCIIEncoding ac = new System.Text.ASCIIEncoding();
			byte[] key = ac.GetBytes(strKey);
			byte[] key2 = str_to_key(key);
			for (int i = 0 ; i < 64 ; i++) {
				inb[i] = (sbyte)(Convert.ToBoolean(_in[i/8] & (1 << (7 - (i % 8)))) ? 1 : 0);
				keyb[i] = (sbyte)(Convert.ToBoolean(key2[i/8] & (1 << (7 - (i % 8)))) ? 1 : 0);
				outb[i] = (sbyte)0;
			}
			outb = dohash(inb, keyb, forw);
			for (int i = 0 ; i < 8 ; i++)
				_out[i] = 0;
			for (int i = 0 ; i < 64 ; i++)
				if (Convert.ToBoolean(outb[i]))
					_out[i/8] |= (byte)(1 << (7 - (i % 8)));
			return _out;
		}
		private byte[] E_P16(string p14)
		{
			byte[] _out = new byte[16];
			byte[] sp8 = {0x4b, 0x47, 0x53, 0x21, 0x40, 0x23, 0x24, 0x25};
			byte[] p8_1 = smbhash(sp8, p14.Remove(7,7),1);
			byte[] p8_2 = smbhash(sp8, p14.Remove(0,7),1);
			int i, d;
			for (i = 0 ; i < 8 ; i++)
				_out[i] = p8_1[i];
			d = i;
			for (i = 0 ; i < 8 ; i++)
				_out[i + d] = p8_2[i];
			return _out;
		}
		
		/// <summary>This method encrypt the given string, using LM Algorithm</summary>
		/// <param name="pwd">The string to be encrypted. </param>
		/// <returns> The encrypted password.</returns>
		public override string Encrypt(string pwd)
		{
			string password = "";
			string LMHash = "";
			byte[] p16;

			if (pwd.Length > 14)
				password = pwd.Remove(14,pwd.Length-14);
			else
				if (pwd.Length <= 14) {
					password += pwd;
					if (pwd.Length < 14) {
						for (int i = pwd.Length - 1; i < 14; i++)
							password += '\0';
					}
				}
			password = password.ToUpper();
			p16 = E_P16(password);
			for (int i = 0 ; i < 16 ; i++)
				LMHash += String.Format("{0:x2}",p16[i]);
			return LMHash;
		}
		public string EncryptHalf(string pwd)
		{
			string password = "";
			string LMHash = "";
			if (pwd.Length > 7)
				password = pwd.Remove(7,pwd.Length-7);
			else
				if (pwd.Length < 7) {
					password += pwd;
					for (int i = pwd.Length - 1 ; i < 14 ; i++)
						password += '\0';
				}
			password = password.ToUpper();
			byte[] sp8 = {0x4b, 0x47, 0x53, 0x21, 0x40, 0x23, 0x24, 0x25};
			byte[] p8_1 = smbhash(sp8, password.Remove(7,7),1);
			for (int i = 0 ; i < 8 ; i++)
				LMHash += String.Format("{0:x2}",p8_1[i]);
			return LMHash;
		}
		
		public override string Encrypt(string s, string p) { return null; }
	}
}
