using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace HxCore
{
	public struct HxCustomNumberCircleRec
    {
		public string ID => $"{TextValue.PadLeftEx(5, '0')}.{DateTime.Now.ToDateStringEx("yyyyMMddTHHmmss")}_{HxString.GetRandomString()}";
		public string TextValue;
		public int CircleWidth;

		public Font TextFont;

		public Color ForegroundColor;
		public Color BackgroundColor;
		public Color BoardColor;

		public bool IsBorderDraw;
		public bool IsBackgroundGradation;

        public HxCustomNumberCircleRec(string textValue, int circleWidth, Font textFont, Color foregroundColor, Color backgroundColor, Color boardColor, bool isDrawBorder = true, bool isBackgroundGradation = false)
        {
            this.TextValue = textValue;
            this.CircleWidth = circleWidth;

			this.TextFont = textFont;

			this.ForegroundColor = foregroundColor;
            this.BackgroundColor = backgroundColor;
            this.BoardColor = boardColor;

			this.IsBorderDraw = isDrawBorder;
            this.IsBackgroundGradation = isBackgroundGradation;
        }

		public HxCustomNumberCircleRec(string textValue, int circleWidth, Font textFont)
			: this(textValue, circleWidth, textFont, Color.Red, Color.Transparent, Color.Black)
		{
			; ;
		}
		public HxCustomNumberCircleRec(string textValue, int circleWidth)
			: this(textValue, circleWidth, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular))
		{
			; ;
		}
	}
    public class HxBitmap
    {
		#region 샘플 비트맵 구하기 - GetSampleBitmap(width, backgroundColor, foregroundColor, drawBorder, font, text)
		public static Bitmap CreateBitmapCustomNumberCircle(HxCustomNumberCircleRec input)
        {
			return CreateBitmapCustomNumberCircle(input.TextValue, input.CircleWidth, input.TextFont, input.ForegroundColor, input.BackgroundColor, input.BoardColor, input.IsBorderDraw, input.IsBackgroundGradation);
		}
		/// <summary>
		/// 샘플 비트맵 구하기
		/// </summary>
		/// <param name="text">텍스트</param>
		/// <param name="width">너비</param>
		/// <param name="font">폰트</param>
		/// <param name="foregroundColor">글자색</param>
		/// <param name="backgroundColor">배경색</param>
		/// <param name="boardColor">테두리색</param>
		/// <param name="isBorderDraw">테두리 그리기 여부</param>
		/// <param name="isBackgroundGradation">배경 그라데이션 적용 여부</param>
		/// <returns>비트맵</returns>
		public static Bitmap CreateBitmapCustomNumberCircle(string text, int width, Font font, Color foregroundColor, Color backgroundColor, Color boardColor, bool isBorderDraw, bool isBackgroundGradation = false)
		{
			Bitmap Result = new Bitmap(width, width);

			using (Graphics graphics = Graphics.FromImage(Result))
			{
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

				graphics.Clear(Color.Transparent);

				Rectangle rectangle = new Rectangle(2, 2, width - 4, width - 4);
				Color gradationColor = backgroundColor;
				if (isBackgroundGradation == true)
				{
					gradationColor = Color.White;

				}
				using (LinearGradientBrush brush = new LinearGradientBrush(rectangle, gradationColor, backgroundColor, LinearGradientMode.BackwardDiagonal))
				{
					graphics.FillEllipse(brush, rectangle);
				}


				if (isBorderDraw)
				{
					/*
					using (Pen pen = new Pen(backgroundColor))
					{
						graphics.DrawEllipse(pen, rectangle);
					}
					*/
					if (boardColor == null && boardColor == Color.Transparent)
					{
						boardColor = backgroundColor;

					}
					using (Pen pen = new Pen(boardColor))
					{
						pen.Width = 2;
						graphics.DrawEllipse(pen, rectangle);
					}
				}

				using (StringFormat stringFormat = new StringFormat())
				{
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Center;
					stringFormat.FormatFlags = StringFormatFlags.FitBlackBox;

					using (Brush brush = new SolidBrush(foregroundColor))
					{
						graphics.DrawString(text, font, brush, rectangle, stringFormat);
					}
				}
			}

			return Result;
		}

		#endregion
	}
}