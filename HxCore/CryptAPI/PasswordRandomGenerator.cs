//
// PasswordRandomGenerator.cs: Creates a Random Password.
//

using System;

namespace CryptAPI {
	
	public class PasswordRandomGenerator : StringGenerator {
		
		public override void DefineSets()
		{
			CharsLCase = "abcdefgijkmnopqrstwxyz";
			CharsUCase  = "ABCDEFGHJKLMNPQRSTWXYZ";
			CharsNumeric = "23456789";
			CharsSpecial = "*$-+?_&=!%{}/";
		}
		
		public override string ReturnFinalString()
		{
			return Final;
		}
	}
}
