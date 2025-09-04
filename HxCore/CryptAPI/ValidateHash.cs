//
// ValidateHash.cs: Hash validator and salt retriever.
//

using System;
using System.Text.RegularExpressions;

namespace CryptAPI
{
	/// <summary>
	/// Validates the type of hash to be cracked.
	/// As read from /etc/shadow or /etc/passwd or SAM
	/// </summary>
	public class ValidateHash
	{
		
		/// <summary>Check if the string is MD5</summary>
		/// <param name="s">The encrypted string</summary>
		/// <returns> If the string was encrypted by MD5 Algorithm returns the salt value</returns>
		public static string isMD5(string s)
		{
			string ret = null;
			Regex r = new Regex(@"^\$1\$(?<salt>[A-Za-z0-9./]{1,8})\$(?<e_pass>[A-Za-z0-9./]{22})$");
			if(r.IsMatch(s)) {
				Match m = r.Match(s);
				ret = m.Groups["salt"].Value;
				return ret;
			}
			return null;
		}
		
		/// <summary>Check if the string is Blowfish</summary>
		/// <param name="s">The encrypted string</param>
		/// <returns> If the string was encrypted by Blowfish Algorithm returns the salt value</returns>
		public static string isBlowFish(string s)
		{
			string ret = null;
			Regex r;
			/* Short BlowFish pattern (FreeBSD) */
			r = new Regex(@"^\$2\$(?<salt>[A-Za-z0-9./]{22})(?<e_pass>[A-Za-z0-9./]{31})$");
			if (r.IsMatch(s)) {
				Match m = r.Match(s);
				ret = "$2$" + m.Groups["salt"].Value;
				return ret;
			}
			/* Long BlowFish pattern (OpenBSD) */
			r = new Regex(@"^\$2(a)?\$(?<rounds>[0-9]{2})\$(?<salt>[A-Za-z0-9./]{22})(?<e_pass>[A-Za-z0-9./]{31})$");
			if (r.IsMatch(s)) {
				Match m = r.Match(s);
				ret = "$2a$" + m.Groups["rounds"].Value + "$" + m.Groups["salt"].Value;
				return ret;
			}
			return null;
		}

		/// <summary>Check if the string is DES</summary>
		/// <param name="s">The encrypted string</param>
		/// <returns> If the string was encrypted by DES Algorithm returns the salt value</returns>
		public static string isDES(string s)
		{
			string ret = null;
			Regex r;
			/* Traditional DES pattern (All) */
			r = new Regex(@"^(?<salt>[A-Za-z0-9./]{2})(?<e_pass>[A-Za-z0-9./]{11})$");
			if (r.IsMatch(s)) {
				Match m = r.Match(s);
				ret = m.Groups["salt"].Value;
				return ret;
			}
			/* Extended DES pattern (FreeBSD, OpenBSD, NetBSD) */
			r = new Regex(@"^_(?<count>[A-Za-z0-9./]{4})(?<salt>[A-Za-z0-9./]{4})(?<e_pass>[A-Za-z0-9./]{11})$");
			if (r.IsMatch(s)) {
				Match m = r.Match(s);
				ret = m.Groups["count"].Value + m.Groups["salt"].Value;
				return ret;
			}
			return null;
		}
		
		/// <summary>Check if the string is MD5</summary>
		/// <param name="s">The encrypted string</param>
		public static string isNTLM(string s)
		{
			string ret = null;
			/* NTLM in FreeBSD. */
			Regex r = new Regex(@"^\$3\$(?<e_pass>[A-Za-z0-9]{32})$");
			if (r.IsMatch(s)) {
				Match m = r.Match(s);
				ret = "";
				return ret;
			}
			return null;
		}
		/// <summary>
		/// Check if the string is LM or NTLM.
		/// </summary>
		/// <returns>returns null if not hash type.
		/// returns string[1] if valid LM or NTLM hash.
		///    string[0] = encrypted password</returns>
		public static string isLM_NTLM(string s)
		{
			string ret = null;
			/* LM or NTLM */
			Regex r = new Regex(@"^(?<e_pass>[A-Za-z0-9]{32})$");
			if (r.IsMatch(s)) {
				Match m = r.Match(s);
				ret = "";
				return ret;
			}
			return null;
		}

		/// <summary>Validate the string with each algorithm and retrieve the salt.</summary>
		/// <param name="s">The encrypted string.</param>
		/// <returns> An array of objects. The salt and the Algorithm.</returns>
		public static HashComponents HashType(string s)
		{
			string salt;
			HashComponents hashcomponent;
			
			if ((salt = isBlowFish(s)) != null) {
				hashcomponent = new HashComponents(Algorithm.BF, salt);
				return hashcomponent;
			}
			if ((salt = isDES(s)) != null) {
				hashcomponent = new HashComponents(Algorithm.DES, salt);
				return hashcomponent;
			}
			if ((salt = isMD5(s)) != null) {
				hashcomponent = new HashComponents(Algorithm.MD5, salt);
				return hashcomponent;
			}
			if ((salt = isLM_NTLM(s)) != null) {
				hashcomponent = new HashComponents(Algorithm.LM | Algorithm.LM, salt);
				return hashcomponent;
			}
			if ((salt = isNTLM(s)) != null) {
				hashcomponent = new HashComponents(Algorithm.NTLM, salt);
				return hashcomponent;
			}
			return null;
		}
	}
}
