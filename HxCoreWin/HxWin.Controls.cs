using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HxCore.Win
{
    partial class HxWin
    {
        #region Windows Control Event
        public static bool? SetControlEvent_KeyPress_OnlyNumberic(object sender, KeyPressEventArgs e)
        {
            bool? Result = null;
            if (sender != null && e != null)
            {
                //숫자만 입력되도록 필터링
                char keyChar = e.KeyChar;
                if (!(char.IsDigit(keyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(Keys.Delete)))    //숫자와 백스페이스, 삭제를 제외한 나머지를 바로 처리
                {
                    e.Handled = true;
                }
                Result = e.Handled;
            }
            return Result;
        }

        public static bool? SetControlEvent_KeyPress_EscapeToClear(Control cmp, KeyPressEventArgs e)
        {
            bool? Result = null;
            if (cmp != null && e != null)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    cmp.Text = null;
                    e.Handled = true;
                }
                Result = e.Handled;
            }
            return Result;
        }
        #endregion
    }
}
