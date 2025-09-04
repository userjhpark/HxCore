//
// CryptServiceProvider.cs: Crypt Service Provider.
//

using System;
namespace CryptAPI
{
	/// <summary>
	/// Class that serve as a Service Provider which depends on the HashType defined in the Algorithm enum.
	/// </summary>
	public class CryptServiceProvider
	{
		Algorithm A;
		
		/// <summary>
		/// Empty Constructor.
		/// </summary>
		public CryptServiceProvider() {}

		/// <summary>
		/// Default Constructor.
		/// </summary>
		/// <param name="a">The hash type</param>
		public CryptServiceProvider(Algorithm a) 
		{
			A = a;
		}

		/// <summary>
		/// Based on the hash type defined by the enum Algorithm, choose the correct object which can encrypt the string.
		/// </summary>
		/// <returns>Returns the correct type of algorithm to be use. </returns> 
		public HashAlgorithm GetCryptServiceProvider()
		{
			switch(A) {
				case Algorithm.BF: {
					return (new BlowFish());
				}
				case Algorithm.DES: {
					return (new DES());
				}
				case (Algorithm.LM | Algorithm.NTLM): {
					return (new LM());
					
				}
				case Algorithm.NTLM: {
					return (new NTLM());
				}
				case Algorithm.MD4: {
					return (new MD4());
				}
				case Algorithm.MD5: {
					return (new MD5());
				}
				default: {
					return null;
				}
			}
			
		}
		
		/// <summary> Based on the hash type defined by the enum Algorithm, choose the correct object which can encrypt the 			///	string.</summary>
		/// <param name="a">The hash type </param>
		/// <return>Returns the correct type of algorithm to be use.</return>		
		public HashAlgorithm GetCryptServiceProvider(Algorithm a) 
		{
			A = a;
			return GetCryptServiceProvider();
		}		
	}
}
