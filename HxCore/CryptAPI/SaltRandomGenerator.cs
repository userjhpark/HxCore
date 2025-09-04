//
// SaltRandomGenerator.cs: Creates a random salt.
//

using System;

namespace CryptAPI {
	
	public class SaltRandomGenerator : StringGenerator {
		
		public override void DefineSets()
		{
			CharsLCase  = "abcdefgijkmnopqrstwxyz";
			CharsUCase  = "ABCDEFGHJKLMNPQRSTWXYZ";
			CharsNumeric = "23456789";
			CharsSpecial = "./";
		}
		
		public override string ReturnFinalString()
		{
			switch(Final.Length)
			{
				case 8:	return "_" + Final;
				case 22: return "$2$" + Final;
				case 16: return "$1$" + Final;
			}
			
			return null;
		}
	}
}
