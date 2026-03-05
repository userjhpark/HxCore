using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    //TODO : HxLoader 미구현
    [Serializable]
    public class HxLoader : MarshalByRefObject
    {
        Dictionary<string, AppDomain> AppDomains;
        public HxLoader()
        {
            this.AppDomains = new Dictionary<string, AppDomain>();
            if (AppDomains != null && AppDomains.Count > 0)
            {
                AppDomains.Clear();
                AppDomains = null;
            }
        }
       
    }
}
