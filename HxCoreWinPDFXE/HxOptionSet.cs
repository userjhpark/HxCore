using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win.PDFXE
{
    public struct OptionSettingRec
    {
        public bool IsSaveRegistry;
        public OptionSettingRec(bool b = true)
        {
            //this.IsOptionSave = false;
            this.IsSaveRegistry = false;
        }

    }
}
