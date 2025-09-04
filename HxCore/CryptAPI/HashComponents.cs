//
// HashComponents.cs: Defines a class which contains a Algorithm an a salt.
//

using System;

namespace CryptAPI {
	
	public class HashComponents {
		private Algorithm a;
		private string salt;
		
		public HashComponents(Algorithm a, string salt)
		{
			this.a = a;
			this.salt = salt;
		}
		
		public Algorithm AlgorithmName {
			set { a = value; }
			get { return a; }
		}
		
		public string Salt {
			set { salt = value; }
			get { return salt; }
		}
	}
}
