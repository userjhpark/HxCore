//
// Algorithm.cs: This class define the enum which list all the supported hashes.
//
  
using System;

namespace CryptAPI
{
	/// <summary>
	/// This enum defines the supported algorithms.
	/// </summary>
	public enum Algorithm {
		DES = 1,
		MD4,
		MD5,
		BF,
		LM,
		NTLM
	}
}
