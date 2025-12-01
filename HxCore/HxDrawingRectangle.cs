using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace HxCore
{
    public struct HxDrawingRectangle
    {
        //
        // 요약:
        //     Represents a System.Drawing.Rectangle structure with its properties left uninitialized.
        public static readonly HxDrawingRectangle Empty;

        private int x;

        private int y;

        private int width;

        private int height;

        //
        // 요약:
        //     Gets or sets the coordinates of the upper-left corner of this System.Drawing.Rectangle
        //     structure.
        //
        // 반환 값:
        //     A System.Drawing.Point that represents the upper-left corner of this System.Drawing.Rectangle
        //     structure.
        [Browsable(false)]
        public HxDrawingPoint Location
        {
            get
            {
                return new HxDrawingPoint(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        //
        // 요약:
        //     Gets or sets the size of this System.Drawing.Rectangle.
        //
        // 반환 값:
        //     A System.Drawing.Size that represents the width and height of this System.Drawing.Rectangle
        //     structure.
        [Browsable(false)]
        public HxDrawingSize Size
        {
            get
            {
                return new HxDrawingSize(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        //
        // 요약:
        //     Gets or sets the x-coordinate of the upper-left corner of this System.Drawing.Rectangle
        //     structure.
        //
        // 반환 값:
        //     The x-coordinate of the upper-left corner of this System.Drawing.Rectangle structure.
        //     The default is 0.
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        //
        // 요약:
        //     Gets or sets the y-coordinate of the upper-left corner of this System.Drawing.Rectangle
        //     structure.
        //
        // 반환 값:
        //     The y-coordinate of the upper-left corner of this System.Drawing.Rectangle structure.
        //     The default is 0.
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        //
        // 요약:
        //     Gets or sets the width of this System.Drawing.Rectangle structure.
        //
        // 반환 값:
        //     The width of this System.Drawing.Rectangle structure. The default is 0.
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        //
        // 요약:
        //     Gets or sets the height of this System.Drawing.Rectangle structure.
        //
        // 반환 값:
        //     The height of this System.Drawing.Rectangle structure. The default is 0.
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        //
        // 요약:
        //     Gets the x-coordinate of the left edge of this System.Drawing.Rectangle structure.
        //
        //
        // 반환 값:
        //     The x-coordinate of the left edge of this System.Drawing.Rectangle structure.
        [Browsable(false)]
        public int Left => X;

        //
        // 요약:
        //     Gets the y-coordinate of the top edge of this System.Drawing.Rectangle structure.
        //
        //
        // 반환 값:
        //     The y-coordinate of the top edge of this System.Drawing.Rectangle structure.
        [Browsable(false)]
        public int Top => Y;

        //
        // 요약:
        //     Gets the x-coordinate that is the sum of System.Drawing.Rectangle.X and System.Drawing.Rectangle.Width
        //     property values of this System.Drawing.Rectangle structure.
        //
        // 반환 값:
        //     The x-coordinate that is the sum of System.Drawing.Rectangle.X and System.Drawing.Rectangle.Width
        //     of this System.Drawing.Rectangle.
        [Browsable(false)]
        public int Right => X + Width;

        //
        // 요약:
        //     Gets the y-coordinate that is the sum of the System.Drawing.Rectangle.Y and System.Drawing.Rectangle.Height
        //     property values of this System.Drawing.Rectangle structure.
        //
        // 반환 값:
        //     The y-coordinate that is the sum of System.Drawing.Rectangle.Y and System.Drawing.Rectangle.Height
        //     of this System.Drawing.Rectangle.
        [Browsable(false)]
        public int Bottom => Y + Height;

        //
        // 요약:
        //     Tests whether all numeric properties of this System.Drawing.Rectangle have values
        //     of zero.
        //
        // 반환 값:
        //     This property returns true if the System.Drawing.Rectangle.Width, System.Drawing.Rectangle.Height,
        //     System.Drawing.Rectangle.X, and System.Drawing.Rectangle.Y properties of this
        //     System.Drawing.Rectangle all have values of zero; otherwise, false.
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                if (height == 0 && width == 0 && x == 0)
                {
                    return y == 0;
                }

                return false;
            }
        }

        //
        // 요약:
        //     Initializes a new instance of the System.Drawing.Rectangle class with the specified
        //     location and size.
        //
        // 매개 변수:
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle.
        //
        //   width:
        //     The width of the rectangle.
        //
        //   height:
        //     The height of the rectangle.
        public HxDrawingRectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        //
        // 요약:
        //     Initializes a new instance of the System.Drawing.Rectangle class with the specified
        //     location and size.
        //
        // 매개 변수:
        //   location:
        //     A System.Drawing.Point that represents the upper-left corner of the rectangular
        //     region.
        //
        //   size:
        //     A System.Drawing.Size that represents the width and height of the rectangular
        //     region.
        public HxDrawingRectangle(HxDrawingPoint location, HxDrawingSize size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        //
        // 요약:
        //     Creates a System.Drawing.Rectangle structure with the specified edge locations.
        //
        //
        // 매개 변수:
        //   left:
        //     The x-coordinate of the upper-left corner of this System.Drawing.Rectangle structure.
        //
        //
        //   top:
        //     The y-coordinate of the upper-left corner of this System.Drawing.Rectangle structure.
        //
        //
        //   right:
        //     The x-coordinate of the lower-right corner of this System.Drawing.Rectangle structure.
        //
        //
        //   bottom:
        //     The y-coordinate of the lower-right corner of this System.Drawing.Rectangle structure.
        //
        //
        // 반환 값:
        //     The new System.Drawing.Rectangle that this method creates.
        public static HxDrawingRectangle FromLTRB(int left, int top, int right, int bottom)
        {
            return new HxDrawingRectangle(left, top, right - left, bottom - top);
        }

        //
        // 요약:
        //     Tests whether obj is a System.Drawing.Rectangle structure with the same location
        //     and size of this System.Drawing.Rectangle structure.
        //
        // 매개 변수:
        //   obj:
        //     The System.Object to test.
        //
        // 반환 값:
        //     This method returns true if obj is a System.Drawing.Rectangle structure and its
        //     System.Drawing.Rectangle.X, System.Drawing.Rectangle.Y, System.Drawing.Rectangle.Width,
        //     and System.Drawing.Rectangle.Height properties are equal to the corresponding
        //     properties of this System.Drawing.Rectangle structure; otherwise, false.
        public override bool Equals(object obj)
        {
            if (!(obj is HxDrawingRectangle rectangle))
            {
                return false;
            }

            if (rectangle.X == X && rectangle.Y == Y && rectangle.Width == Width)
            {
                return rectangle.Height == Height;
            }

            return false;
        }

        //
        // 요약:
        //     Tests whether two System.Drawing.Rectangle structures have equal location and
        //     size.
        //
        // 매개 변수:
        //   left:
        //     The System.Drawing.Rectangle structure that is to the left of the equality operator.
        //
        //
        //   right:
        //     The System.Drawing.Rectangle structure that is to the right of the equality operator.
        //
        //
        // 반환 값:
        //     This operator returns true if the two System.Drawing.Rectangle structures have
        //     equal System.Drawing.Rectangle.X, System.Drawing.Rectangle.Y, System.Drawing.Rectangle.Width,
        //     and System.Drawing.Rectangle.Height properties.
        public static bool operator ==(HxDrawingRectangle left, HxDrawingRectangle right)
        {
            if (left.X == right.X && left.Y == right.Y && left.Width == right.Width)
            {
                return left.Height == right.Height;
            }

            return false;
        }

        //
        // 요약:
        //     Tests whether two System.Drawing.Rectangle structures differ in location or size.
        //
        //
        // 매개 변수:
        //   left:
        //     The System.Drawing.Rectangle structure that is to the left of the inequality
        //     operator.
        //
        //   right:
        //     The System.Drawing.Rectangle structure that is to the right of the inequality
        //     operator.
        //
        // 반환 값:
        //     This operator returns true if any of the System.Drawing.Rectangle.X, System.Drawing.Rectangle.Y,
        //     System.Drawing.Rectangle.Width or System.Drawing.Rectangle.Height properties
        //     of the two System.Drawing.Rectangle structures are unequal; otherwise false.
        public static bool operator !=(HxDrawingRectangle left, HxDrawingRectangle right)
        {
            return !(left == right);
        }

        /*
        //
        // 요약:
        //     Converts the specified System.Drawing.RectangleF structure to a System.Drawing.Rectangle
        //     structure by rounding the System.Drawing.RectangleF values to the next higher
        //     integer values.
        //
        // 매개 변수:
        //   value:
        //     The System.Drawing.RectangleF structure to be converted.
        //
        // 반환 값:
        //     Returns a System.Drawing.Rectangle.
        public static HxDrawingRectangle Ceiling(RectangleF value)
        {
            return new HxDrawingRectangle((int)Math.Ceiling(value.X), (int)Math.Ceiling(value.Y), (int)Math.Ceiling(value.Width), (int)Math.Ceiling(value.Height));
        }
        */

        //
        // 요약:
        //     Converts the specified System.Drawing.RectangleF to a System.Drawing.Rectangle
        //     by truncating the System.Drawing.RectangleF values.
        //
        // 매개 변수:
        //   value:
        //     The System.Drawing.RectangleF to be converted.
        //
        // 반환 값:
        //     The truncated value of the System.Drawing.Rectangle.
        public static HxDrawingRectangle Truncate(HxDrawingRectangle value)
        {
            return new HxDrawingRectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
        }

        /*
        //
        // 요약:
        //     Converts the specified System.Drawing.RectangleF to a System.Drawing.Rectangle
        //     by rounding the System.Drawing.RectangleF values to the nearest integer values.
        //
        //
        // 매개 변수:
        //   value:
        //     The System.Drawing.RectangleF to be converted.
        //
        // 반환 값:
        //     The rounded interger value of the System.Drawing.Rectangle.
        public static Rectangle Round(RectangleF value)
        {
            return new Rectangle((int)Math.Round(value.X), (int)Math.Round(value.Y), (int)Math.Round(value.Width), (int)Math.Round(value.Height));
        }
        */

        //
        // 요약:
        //     Determines if the specified point is contained within this System.Drawing.Rectangle
        //     structure.
        //
        // 매개 변수:
        //   x:
        //     The x-coordinate of the point to test.
        //
        //   y:
        //     The y-coordinate of the point to test.
        //
        // 반환 값:
        //     This method returns true if the point defined by x and y is contained within
        //     this System.Drawing.Rectangle structure; otherwise false.
        public bool Contains(int x, int y)
        {
            if (X <= x && x < X + Width && Y <= y)
            {
                return y < Y + Height;
            }

            return false;
        }

        //
        // 요약:
        //     Determines if the specified point is contained within this System.Drawing.Rectangle
        //     structure.
        //
        // 매개 변수:
        //   pt:
        //     The System.Drawing.Point to test.
        //
        // 반환 값:
        //     This method returns true if the point represented by pt is contained within this
        //     System.Drawing.Rectangle structure; otherwise false.
        public bool Contains(HxDrawingPoint pt)
        {
            return Contains(pt.X, pt.Y);
        }

        //
        // 요약:
        //     Determines if the rectangular region represented by rect is entirely contained
        //     within this System.Drawing.Rectangle structure.
        //
        // 매개 변수:
        //   rect:
        //     The System.Drawing.Rectangle to test.
        //
        // 반환 값:
        //     This method returns true if the rectangular region represented by rect is entirely
        //     contained within this System.Drawing.Rectangle structure; otherwise false.
        public bool Contains(HxDrawingRectangle rect)
        {
            if (X <= rect.X && rect.X + rect.Width <= X + Width && Y <= rect.Y)
            {
                return rect.Y + rect.Height <= Y + Height;
            }

            return false;
        }

        //
        // 요약:
        //     Returns the hash code for this System.Drawing.Rectangle structure. For information
        //     about the use of hash codes, see System.Object.GetHashCode .
        //
        // 반환 값:
        //     An integer that represents the hash code for this rectangle.
        public override int GetHashCode()
        {
            //return X ^ ((Y << 13) | (Y >>> 19)) ^ ((Width << 26) | (Width >>> 6)) ^ ((Height << 7) | (Height >>> 25));
            return X ^ Y ^ Width ^ Height;
        }

        //
        // 요약:
        //     Enlarges this System.Drawing.Rectangle by the specified amount.
        //
        // 매개 변수:
        //   width:
        //     The amount to inflate this System.Drawing.Rectangle horizontally.
        //
        //   height:
        //     The amount to inflate this System.Drawing.Rectangle vertically.
        public void Inflate(int width, int height)
        {
            X -= width;
            Y -= height;
            Width += 2 * width;
            Height += 2 * height;
        }

        //
        // 요약:
        //     Enlarges this System.Drawing.Rectangle by the specified amount.
        //
        // 매개 변수:
        //   size:
        //     The amount to inflate this rectangle.
        public void Inflate(HxDrawingSize size)
        {
            Inflate(size.Width, size.Height);
        }

        //
        // 요약:
        //     Creates and returns an enlarged copy of the specified System.Drawing.Rectangle
        //     structure. The copy is enlarged by the specified amount. The original System.Drawing.Rectangle
        //     structure remains unmodified.
        //
        // 매개 변수:
        //   rect:
        //     The System.Drawing.Rectangle with which to start. This rectangle is not modified.
        //
        //
        //   x:
        //     The amount to inflate this System.Drawing.Rectangle horizontally.
        //
        //   y:
        //     The amount to inflate this System.Drawing.Rectangle vertically.
        //
        // 반환 값:
        //     The enlarged System.Drawing.Rectangle.
        public static HxDrawingRectangle Inflate(HxDrawingRectangle rect, int x, int y)
        {
            HxDrawingRectangle result = rect;
            result.Inflate(x, y);
            return result;
        }

        //
        // 요약:
        //     Replaces this System.Drawing.Rectangle with the intersection of itself and the
        //     specified System.Drawing.Rectangle.
        //
        // 매개 변수:
        //   rect:
        //     The System.Drawing.Rectangle with which to intersect.
        public void Intersect(HxDrawingRectangle rect)
        {
            HxDrawingRectangle rectangle = Intersect(rect, this);
            X = rectangle.X;
            Y = rectangle.Y;
            Width = rectangle.Width;
            Height = rectangle.Height;
        }

        //
        // 요약:
        //     Returns a third System.Drawing.Rectangle structure that represents the intersection
        //     of two other System.Drawing.Rectangle structures. If there is no intersection,
        //     an empty System.Drawing.Rectangle is returned.
        //
        // 매개 변수:
        //   a:
        //     A rectangle to intersect.
        //
        //   b:
        //     A rectangle to intersect.
        //
        // 반환 값:
        //     A System.Drawing.Rectangle that represents the intersection of a and b.
        public static HxDrawingRectangle Intersect(HxDrawingRectangle a, HxDrawingRectangle b)
        {
            int num = Math.Max(a.X, b.X);
            int num2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int num3 = Math.Max(a.Y, b.Y);
            int num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
            if (num2 >= num && num4 >= num3)
            {
                return new HxDrawingRectangle(num, num3, num2 - num, num4 - num3);
            }

            return Empty;
        }

        //
        // 요약:
        //     Determines if this rectangle intersects with rect.
        //
        // 매개 변수:
        //   rect:
        //     The rectangle to test.
        //
        // 반환 값:
        //     This method returns true if there is any intersection, otherwise false.
        public bool IntersectsWith(HxDrawingRectangle rect)
        {
            if (rect.X < X + Width && X < rect.X + rect.Width && rect.Y < Y + Height)
            {
                return Y < rect.Y + rect.Height;
            }

            return false;
        }

        //
        // 요약:
        //     Gets a System.Drawing.Rectangle structure that contains the union of two System.Drawing.Rectangle
        //     structures.
        //
        // 매개 변수:
        //   a:
        //     A rectangle to union.
        //
        //   b:
        //     A rectangle to union.
        //
        // 반환 값:
        //     A System.Drawing.Rectangle structure that bounds the union of the two System.Drawing.Rectangle
        //     structures.
        public static HxDrawingRectangle Union(HxDrawingRectangle a, HxDrawingRectangle b)
        {
            int num = Math.Min(a.X, b.X);
            int num2 = Math.Max(a.X + a.Width, b.X + b.Width);
            int num3 = Math.Min(a.Y, b.Y);
            int num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
            return new HxDrawingRectangle(num, num3, num2 - num, num4 - num3);
        }

        //
        // 요약:
        //     Adjusts the location of this rectangle by the specified amount.
        //
        // 매개 변수:
        //   pos:
        //     Amount to offset the location.
        public void Offset(HxDrawingPoint pos)
        {
            Offset(pos.X, pos.Y);
        }

        //
        // 요약:
        //     Adjusts the location of this rectangle by the specified amount.
        //
        // 매개 변수:
        //   x:
        //     The horizontal offset.
        //
        //   y:
        //     The vertical offset.
        public void Offset(int x, int y)
        {
            X += x;
            Y += y;
        }

        //
        // 요약:
        //     Converts the attributes of this System.Drawing.Rectangle to a human-readable
        //     string.
        //
        // 반환 값:
        //     A string that contains the position, width, and height of this System.Drawing.Rectangle
        //     structure ¾ for example, {X=20, Y=20, Width=100, Height=50}
        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) + ",Width=" + Width.ToString(CultureInfo.CurrentCulture) + ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}
