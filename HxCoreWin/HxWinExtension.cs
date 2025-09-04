using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HxCore.Win
{
    public static partial class HxWinExtension
    {
        public static IEnumerable<T> GetFindAllControlEx<T>(this Control control)
            where T : Control
        {
            try
            {
                return HxWin.GetFindAllControl<T>(control);
            }
            catch (Exception)
            {
                IEnumerable<Control> controls = control.Controls.Cast<Control>();
                return controls
                    .OfType<T>()
                    .Concat<T>(controls.SelectMany<Control, T>(ctrl => GetFindAllControlEx<T>(ctrl)));
                //throw;
            }
            
        }
        public static string ToStopwatchCaptionEx(this System.Diagnostics.Stopwatch sw, DateTime? inputDateTime = null, string dateFormat = "yyyy-MM-dd HH:mm:ss")
        {
            string Result = null;
            
            if (sw != null)
            {
                if (inputDateTime != null)
                {
                    if (inputDateTime == DateTime.MinValue)
                    {
                        inputDateTime = DateTime.Now;
                    }
                    Result  = $"{inputDateTime.ToDateTimeStringEx(dateFormat)} / ";
                }
                Result += $"{sw.Elapsed} ({sw.ElapsedMilliseconds} ms)";
            }
            return Result;
        }
    }
}
