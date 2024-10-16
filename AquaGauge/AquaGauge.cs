using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace AquaControls
{
    /// <summary>
    /// Aqua Gauge Control - A Windows User Control.
    /// Author  : Ambalavanar Thirugnanam
    /// Date    : 24th August 2007
    /// email   : ambalavanar.thiru@gmail.com
    /// This is control is for free. You can use for any commercial or non-commercial purposes.
    /// [Please do no remove this header when using this control in your application.]
    /// 
    /// Modified to get a full 360deg Gage
    /// Author  : DL2ALF
    /// Date    : 2041-02-18
    /// </summary>
    public partial class AquaGauge : UserControl
    {
        #region Private Attributes
        private float minValue;
        private float maxValue;
        private float threshold;
        private float currentValue;
        private float recommendedValue;
        private int noOfDivisions;
        private int noOfSubDivisions;
        private string dialText;
        private Color dialColor = Color.Lavender;
        private float glossinessAlpha = 25;
        private int oldWidth, oldHeight;
        public int x, y, width, height;
//        float fromAngle = 135F;
//        float toAngle = 405F;
        public float fromAngle = -90F;
        public float toAngle = 270F;
        private bool enableTransparentBackground;
        private bool requiresRedraw;
        private Image backgroundImg;
        private Rectangle rectImg;
        #endregion

        public AquaGauge()
        {
            InitializeComponent();
            x = 5;
            y = 5;
            width = this.Width - 10;
            height = this.Height - 10;
            this.noOfDivisions = 12;
            this.noOfSubDivisions = 3;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);                     
            this.BackColor = Color.Transparent;
            this.Resize += new EventHandler(AquaGauge_Resize);
            this.requiresRedraw = true;
        }

        #region Public Properties
        /// <summary>
        /// Mininum value on the scale
        /// </summary>
        [DefaultValue(0)]
        [Description("Mininum value on the scale")]
        public float MinValue
        {
            get { return minValue; }
            set
            {
                if (value < maxValue)
                {
                    minValue = value;
                    if (currentValue < minValue)
                        currentValue = minValue;
                    if (recommendedValue < minValue)
                        recommendedValue = minValue;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Maximum value on the scale
        /// </summary>
        [DefaultValue(100)]
        [Description("Maximum value on the scale")]
        public float MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value > minValue)
                {
                    maxValue = value;
                    if (currentValue > maxValue)
                        currentValue = maxValue;
                    if (recommendedValue > maxValue)
                        recommendedValue = maxValue;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Threshold area from the Recommended Value. (1-99%)
        /// </summary>
        [DefaultValue(25)]
        [Description("Gets or Sets the Threshold area from the Recommended Value. (1-99%)")]
        public float ThresholdPercent
        {
            get { return threshold; }
            set
            {
                if (value > 0 && value < 100)
                {
                    threshold = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Threshold value from which green area will be marked.
        /// </summary>
        [DefaultValue(25)]
        [Description("Threshold value from which green area will be marked.")]
        public float RecommendedValue
        {
            get { return recommendedValue; }
            set
            {
                if (value > minValue && value < maxValue) 
                {
                    recommendedValue = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Value where the pointer will point to.
        /// </summary>
        [DefaultValue(0)]
        [Description("Value where the pointer will point to.")]
        public float Value
        {
            get { return currentValue; }
            set
            {
                if (value >= minValue && value <= maxValue)
                {
                    currentValue = value;
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Background color of the dial
        /// </summary>
        [Description("Background color of the dial")]
        public Color DialColor
        {
            get { return dialColor; }
            set
            {
                dialColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Glossiness strength. Range: 0-100
        /// </summary>
        [DefaultValue(72)]
        [Description("Glossiness strength. Range: 0-100")]
        public float Glossiness
        {
            get
            {
                return (glossinessAlpha * 100) / 220;
            }
            set
            {
                float val = value;
                if(val > 100) 
                    value = 100;
                if(val < 0)
                    value = 0;
                glossinessAlpha = (value * 220) / 100;
                this.Refresh();
            }
        }

        /// <summary>
        /// Get or Sets the number of Divisions in the dial scale.
        /// </summary>
        [DefaultValue(10)]
        [Description("Get or Sets the number of Divisions in the dial scale.")]
        public int NoOfDivisions
        {
            get { return this.noOfDivisions; }
            set
            {
                if (value > 1 && value < 25)
                {
                    this.noOfDivisions = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or Sets the number of Sub Divisions in the scale per Division.
        /// </summary>
        [DefaultValue(3)]
        [Description("Gets or Sets the number of Sub Divisions in the scale per Division.")]
        public int NoOfSubDivisions
        {
            get { return this.noOfSubDivisions; }
            set
            {
                if (value > 0 && value <= 10)
                {
                    this.noOfSubDivisions = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Text to be displayed in the dial
        /// </summary>
        [Description("Gets or Sets the Text to be displayed in the dial")]
        public string DialText
        {
            get { return this.dialText; }
            set
            {
                this.dialText = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Enables or Disables Transparent Background color.
        /// Note: Enabling this will reduce the performance and may make the control flicker.
        /// </summary>
        [DefaultValue(false)]
        [Description("Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker.")]
        public bool EnableTransparentBackground
        {
            get { return this.enableTransparentBackground; }
            set
            {
                this.enableTransparentBackground = value;
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, !enableTransparentBackground);
                requiresRedraw = true;
                this.Refresh();  
            }
        }
        #endregion

        #region Overriden Control methods
        /// <summary>
        /// Draws the pointer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            width = this.Width - x * 2;
            height = this.Height - y*2;
            DrawPointer(e.Graphics, 0,0);
            //Draw Digital Value
            DisplayNumber(e.Graphics,0,0);
        }
                
        /// <summary>
        /// Draws the dial background.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (!enableTransparentBackground)
            {
                base.OnPaintBackground(e);
            }         
            
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0,0,Width,Height));
            if (backgroundImg == null || requiresRedraw)
            {
                backgroundImg = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(backgroundImg);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                width = this.Width - x * 2;
                height = this.Height - y * 2;
                rectImg = new Rectangle(x, y, width, height);

                //Draw background color
                Brush backGroundBrush = new SolidBrush(Color.FromArgb(120, dialColor));
                if (enableTransparentBackground && this.Parent != null)
                {
                    float gg = width / 60;
                    //g.FillEllipse(new SolidBrush(this.Parent.BackColor), -gg, -gg, this.Width+gg*2, this.Height+gg*2);
                }
                g.FillEllipse(backGroundBrush, x, y, width, height);

                //Draw Rim
                SolidBrush outlineBrush = new SolidBrush(Color.FromArgb(100, Color.SlateGray));
                Pen outline = new Pen(outlineBrush, (float)(width * .03));
                g.DrawEllipse(outline, rectImg);
                Pen darkRim = new Pen(Color.SlateGray);
                g.DrawEllipse(darkRim, x, y, width, height);

                //Draw Callibration
                DrawCalibration(g, 0, 0);

                //Draw Colored Rim
                Pen colorPen = new Pen(Color.FromArgb(190, Color.Gainsboro), this.Width / 40);
                Pen blackPen = new Pen(Color.FromArgb(250, this.ForeColor), this.Width / 200);
//                int gap = (int)(this.Width * 0.03F);
                int gap = 0;
                Rectangle rectg = new Rectangle(rectImg.X + gap, rectImg.Y + gap, rectImg.Width - gap * 2, rectImg.Height - gap * 2);
                g.DrawArc(colorPen, rectg, 135, 270);

                //Draw Threshold
                colorPen = new Pen(Color.FromArgb(200, Color.LawnGreen), this.Width / 50);
                rectg = new Rectangle(rectImg.X + gap, rectImg.Y + gap, rectImg.Width - gap * 2, rectImg.Height - gap * 2);
                float val = MaxValue - MinValue;
                val = (100 * (this.recommendedValue - MinValue)) / val;
                val = ((toAngle - fromAngle) * val) / 100;
                val += fromAngle;
                float stAngle = val - ((270 * threshold) / 200);
                if (stAngle <= 135) stAngle = 135;
                float sweepAngle = ((270 * threshold) / 100);
                if (stAngle + sweepAngle > 405) sweepAngle = 405 - stAngle;
                g.DrawArc(colorPen, rectg, stAngle, sweepAngle);

                //Draw Digital Value
//                RectangleF digiRect = new RectangleF((float)this.Width / 2F - (float)this.width / 5F, (float)this.height / 1.2F, (float)this.width / 2.5F, (float)this.Height / 9F);
//                RectangleF digiFRect = new RectangleF(this.Width / 2 - this.width / 7, (int)(this.height / 1.18), this.width / 4, this.Height / 12);
                RectangleF digiRect = new RectangleF((float)this.Width / 2F - (float)this.width / 5F, (float)this.height / 1.5F, (float)this.width / 2.5F, (float)this.Height / 9F);
                RectangleF digiFRect = new RectangleF(this.Width / 2 - this.width / 7, (int)(this.height / 1.47), this.width / 4, this.Height / 12);
                g.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Gray)), digiRect);
//                DisplayNumber(g, this.currentValue, digiFRect);

                SizeF textSize = g.MeasureString(this.dialText, this.Font);
//                RectangleF digiFRectText = new RectangleF(this.Width / 2 - textSize.Width / 2, (int)(this.height / 1.5), textSize.Width, textSize.Height);
                RectangleF digiFRectText = new RectangleF(this.Width / 2 - textSize.Width / 2, (int)(this.height / 4), textSize.Width, textSize.Height);
                g.DrawString(dialText, this.Font, new SolidBrush(this.ForeColor), digiFRectText);
                requiresRedraw = false;
            }
            e.Graphics.DrawImage(backgroundImg, rectImg);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }
        #endregion


        #region Drawing methods

        public void DrawDialText (Graphics g, int ofsX, int ofsY)
        {
            SizeF textSize = g.MeasureString(this.dialText, this.Font);
            RectangleF digiFRectText = new RectangleF(this.Width / 2 - textSize.Width / 2, (int)(this.height / 4), textSize.Width, textSize.Height);
            digiFRectText.Offset(ofsX, ofsY);
            g.DrawString(dialText, this.Font, new SolidBrush(this.ForeColor), digiFRectText);
        }

        /// <summary>
        /// Draws the Pointer.
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        public void DrawPointer(Graphics gr, int ofsX, int ofsY)
        {
            int cx = ((width) / 2) + this.x;
            int cy = ((height) / 2) + this.y;

            float radius = this.Width / 2 - (this.Width * .12F);
            float val = MaxValue - MinValue;

            Image img = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(img);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            val = (100 * (this.currentValue - MinValue)) / val;
            val = ((toAngle - fromAngle) * val) / 100;
            val += fromAngle;

            float angle = GetRadian(val);
            float gradientAngle = angle;

            PointF[] pts = new PointF[5];

            pts[0].X = (float)(cx + radius * Math.Cos(angle));
            pts[0].Y = (float)(cy + radius * Math.Sin(angle));

            pts[4].X = (float)(cx + radius * Math.Cos(angle - 0.02));
            pts[4].Y = (float)(cy + radius * Math.Sin(angle - 0.02));

            angle = GetRadian((val + 20));
            pts[1].X = (float)(cx + (this.Width * .09F) * Math.Cos(angle));
            pts[1].Y = (float)(cy + (this.Width * .09F) * Math.Sin(angle));

            pts[2].X = cx;
            pts[2].Y = cy;

            angle = GetRadian((val - 20));
            pts[3].X = (float)(cx + (this.Width * .09F) * Math.Cos(angle));
            pts[3].Y = (float)(cy + (this.Width * .09F) * Math.Sin(angle));

            Brush pointer = new SolidBrush(this.ForeColor);
            g.FillPolygon(pointer, pts);

            PointF[] shinePts = new PointF[3];
            angle = GetRadian(val);
            shinePts[0].X = (float)(cx + radius * Math.Cos(angle));
            shinePts[0].Y = (float)(cy + radius * Math.Sin(angle));

            angle = GetRadian(val + 20);
            shinePts[1].X = (float)(cx + (this.Width * .09F) * Math.Cos(angle));
            shinePts[1].Y = (float)(cy + (this.Width * .09F) * Math.Sin(angle));

            shinePts[2].X = cx;
            shinePts[2].Y = cy;

            LinearGradientBrush gpointer = new LinearGradientBrush(shinePts[0], shinePts[2], Color.SlateGray, this.ForeColor);
            g.FillPolygon(gpointer, shinePts);

            Rectangle rect = new Rectangle(x, y, width, height);
            DrawCenterPoint(g, ofsX,ofsY);

            DrawGloss(g);

            gr.DrawImage(img, ofsX,ofsY);
        }

        /// <summary>
        /// Draws the glossiness.
        /// </summary>
        /// <param name="g"></param>
        private void DrawGloss(Graphics g)
        {
            RectangleF glossRect = new RectangleF(
               x + (float)(width * 0.10),
               y + (float)(height * 0.07),
               (float)(width * 0.80),
               (float)(height * 0.7));
            LinearGradientBrush gradientBrush =
                new LinearGradientBrush(glossRect,
                Color.FromArgb((int)glossinessAlpha, Color.White),
                Color.Transparent,
                LinearGradientMode.Vertical);
            g.FillEllipse(gradientBrush, glossRect);

            //TODO: Gradient from bottom
            glossRect = new RectangleF(
               x + (float)(width * 0.25),
               y + (float)(height * 0.77),
               (float)(width * 0.50),
               (float)(height * 0.2));
            int gloss = (int)(glossinessAlpha / 3);
            gradientBrush =
                new LinearGradientBrush(glossRect,
                Color.Transparent, Color.FromArgb(gloss, this.BackColor),
                LinearGradientMode.Vertical);
            g.FillEllipse(gradientBrush, glossRect);
        }

        /// <summary>
        /// Draws the center point.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cX"></param>
        /// <param name="cY"></param>
        public void DrawCenterPoint(Graphics g, int ofsX, int ofsY)
        {
            int cX = ((width) / 2) + this.x + ofsX;
            int cY = ((height) / 2) + this.y + ofsY;
            float shift = Width / 5;
            Rectangle rect = new Rectangle(this.x, this.y, width, height);
            RectangleF rectangle = new RectangleF(cX - (shift / 2), cY - (shift / 2), shift, shift);
            LinearGradientBrush brush = new LinearGradientBrush(rect, this.ForeColor, Color.FromArgb(100,this.dialColor), LinearGradientMode.Vertical);
            g.FillEllipse(brush, rectangle);
            shift = Width / 7;
            rectangle = new RectangleF(cX - (shift / 2), cY - (shift / 2), shift, shift);
            brush = new LinearGradientBrush(rect, Color.SlateGray, this.ForeColor, LinearGradientMode.ForwardDiagonal);
            g.FillEllipse(brush, rectangle);
        }

        /// <summary>
        /// Draws the Ruler
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cX"></param>
        /// <param name="cY"></param>
        public void DrawCalibration(Graphics g, int ofsX, int ofsY)
        {
            int cX = ((width) / 2) + this.x + ofsX;
            int cY = ((height) / 2) + this.y + ofsY;
            width = this.Width - this.x * 2;
            height = this.Height - this.y * 2;
            Rectangle rect = new Rectangle(this.x, this.y, width, height);
            int noOfParts = this.noOfDivisions + 1;
            int noOfIntermediates = this.noOfSubDivisions;
            float currentAngle = GetRadian(fromAngle);
            int gap = (int)(this.Width * 0.01F);
            float shift = this.Width / 25;
            Rectangle rectangle = new Rectangle(rect.Left + gap, rect.Top + gap, rect.Width - gap, rect.Height - gap);
                                   
            float x,y,x1,y1,tx,ty,radius;
            radius = rectangle.Width/2 - gap*5;
            float totalAngle = toAngle - fromAngle;
            float incr = GetRadian(((totalAngle) / ((noOfParts - 1) * (noOfIntermediates + 1))));
            
            Pen thickPen = new Pen(this.ForeColor, Width/50);
            Pen thinPen = new Pen(this.ForeColor, Width/100);
            float rulerValue = MinValue;
            for (int i = 0; i <= noOfParts; i++)
            {
                //Draw Thick Line
                x = (float)(cX + radius * Math.Cos(currentAngle));
                y = (float)(cY + radius * Math.Sin(currentAngle));
                x1 = (float)(cX + (radius - Width/20) * Math.Cos(currentAngle));
                y1 = (float)(cY + (radius - Width/20) * Math.Sin(currentAngle));
                g.DrawLine(thickPen, x, y, x1, y1);
                
                //Draw Strings
                StringFormat format = new StringFormat();
                tx = (float)(cX + (radius - Width / 10) * Math.Cos(currentAngle));
                ty = (float)(cY-shift + (radius - Width / 10) * Math.Sin(currentAngle));
                Brush stringPen = new SolidBrush(this.ForeColor);
                StringFormat strFormat = new StringFormat(StringFormatFlags.NoClip);
                strFormat.Alignment = StringAlignment.Center;
                Font f = new Font(this.Font.FontFamily, (float)(this.Width / 23), this.Font.Style);
                g.DrawString(rulerValue.ToString() + "", f, stringPen, new PointF(tx, ty), strFormat);
                rulerValue += (float)((MaxValue - MinValue) / (noOfParts - 1));
                rulerValue = (float)Math.Round(rulerValue, 2);
                
                //currentAngle += incr;
                if (i == noOfParts -1)
                    break;
                for (int j = 0; j <= noOfIntermediates; j++)
                {
                    //Draw thin lines 
                    currentAngle += incr;
                    x = (float)(cX + radius * Math.Cos(currentAngle));
                    y = (float)(cY + radius * Math.Sin(currentAngle));
                    x1 = (float)(cX + (radius - Width/50) * Math.Cos(currentAngle));
                    y1 = (float)(cY + (radius - Width/50) * Math.Sin(currentAngle));
                    g.DrawLine(thinPen, x, y, x1, y1);                    
                }
            }
        }

        /// <summary>
        /// Converts the given degree to radian.
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        public float GetRadian(float theta)
        {
            return theta * (float)Math.PI / 180F;
        }

        /// <summary>
        /// Displays the given number in the 7-Segement format.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="number"></param>
        /// <param name="drect"></param>
        public void DisplayNumber(Graphics g, int ofsX, int ofsY)
        {
            try
            {
                RectangleF digiRect = new RectangleF((float)this.Width / 2F - (float)this.width / 5F, (float)this.height / 1.5F, (float)this.width / 2.5F, (float)this.Height / 9F);
                RectangleF digiFRect = new RectangleF(this.Width / 2 - this.width / 7, (int)(this.height / 1.47), this.width / 3, this.Height / 12);
                digiRect.Offset(ofsX,ofsY);
                digiFRect.Offset(ofsX,ofsY);
                g.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Gray)), digiRect);
                string num = this.currentValue.ToString("000.00",CultureInfo.InvariantCulture);
                num.PadLeft(3, '0');
                float shift = 0; 
                if (this.currentValue < 0)
                {
                    shift -= width/17;
                }
                bool drawDPS = false;
                char[] chars = num.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    char c = chars[i];
                    if (i < chars.Length - 1 && chars[i + 1] == '.')
                        drawDPS = true;
                    else
                        drawDPS = false;
                    if (c != '.')
                    {
                        if (c == '-')
                        {
                            DrawDigit(g, -1, new PointF(digiFRect.X + shift, digiFRect.Y), drawDPS, digiFRect.Height);
                        }
                        else
                        {
                            DrawDigit(g, Int16.Parse(c.ToString()), new PointF(digiFRect.X + shift, digiFRect.Y), drawDPS, digiFRect.Height);
                        }
                        shift += 15 * this.width / 250;
                    }
                    else
                    {
                        shift += 2 * this.width / 250;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Draws a digit in 7-Segement format.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="number"></param>
        /// <param name="position"></param>
        /// <param name="dp"></param>
        /// <param name="height"></param>
        private void DrawDigit(Graphics g, int number, PointF position, bool dp, float height)
        {
            // calculate widths and heights
            float width, thickwidth, halfthickwidth, thickheight, halfthickheight;
            width = 10F * height/13.0F;
            thickwidth = 1.6F;
            halfthickwidth = thickwidth / 2.0F;
            thickheight = thickwidth * width / height;
            halfthickheight = thickheight / 2.0F;

            // get pens
            Pen outline = new Pen(Color.FromArgb(30, this.ForeColor));
            Pen fillPen = new Pen(this.ForeColor);

            // keep old graphics settings
            Matrix oldtrans = g.Transform;
            SmoothingMode oldsmooth = g.SmoothingMode;
            PixelOffsetMode oldpixel = g.PixelOffsetMode;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Default;

            #region Form Polygon Points

            PointF[] segmentA = new PointF[7];
            segmentA[0] = segmentA[6] = new PointF(position.X + GetX(1.2F + halfthickwidth, width), position.Y + GetY(1F + halfthickwidth, height));
            segmentA[1] = new PointF(position.X + GetX(1.2F + thickwidth, width), position.Y + GetY(1F, height));
            segmentA[2] = new PointF(position.X + GetX(7.8F - thickwidth, width), position.Y + GetY(1F, height));
            segmentA[3] = new PointF(position.X + GetX(7.8F - halfthickwidth, width), position.Y + GetY(1F + halfthickheight, height));
            segmentA[4] = new PointF(position.X + GetX(7.8F - thickwidth, width), position.Y + GetY(1F + thickheight, height));
            segmentA[5] = new PointF(position.X + GetX(1.2F + thickwidth, width), position.Y + GetY(1F + thickheight, height));

            //Segment B
            PointF[] segmentB = new PointF[7];
            segmentB[0] = segmentB[6] = new PointF(position.X + GetX(8F - thickwidth, width), position.Y + GetY(1.2F + thickheight, height));
            segmentB[1] = new PointF(position.X + GetX(8F - halfthickwidth, width), position.Y + GetY(1.2F + halfthickheight, height));
            segmentB[2] = new PointF(position.X + GetX(8F, width), position.Y + GetY(1.2F + thickheight, height));
            segmentB[3] = new PointF(position.X + GetX(8F, width), position.Y + GetY(6.8F, height));
            segmentB[4] = new PointF(position.X + GetX(8F - halfthickwidth, width), position.Y + GetY(6.8F, height));
            segmentB[5] = new PointF(position.X + GetX(8F - thickwidth, width), position.Y + GetY(6.8F - halfthickheight, height));

            //Segment C
            PointF[] segmentC = new PointF[7];
            segmentC[0] = segmentC[6] = new PointF(position.X + GetX(8F - thickwidth, width), position.Y + GetY(7.2F + halfthickheight, height));
            segmentC[1] = new PointF(position.X + GetX(8F - halfthickwidth, width), position.Y + GetY(7.2F, height));
            segmentC[2] = new PointF(position.X + GetX(8F, width), position.Y + GetY(7.2F, height));
            segmentC[3] = new PointF(position.X + GetX(8F, width), position.Y + GetY(12.8F - thickheight, height));
            segmentC[4] = new PointF(position.X + GetX(8F - halfthickwidth, width), position.Y + GetY(12.8F - halfthickheight, height));
            segmentC[5] = new PointF(position.X + GetX(8F - thickwidth, width), position.Y + GetY(12.8F - thickheight, height));

            //Segment D
            PointF[] segmentD = new PointF[7];
            segmentD[0] = segmentD[6] = new PointF(position.X + GetX(1.2F + halfthickwidth, width), position.Y + GetY(13F - halfthickheight, height));
            segmentD[1] = new PointF(position.X + GetX(1.2F + thickwidth, width), position.Y + GetY(13F - thickheight, height));
            segmentD[2] = new PointF(position.X + GetX(7.8F - thickwidth, width), position.Y + GetY(13F - thickheight, height));
            segmentD[3] = new PointF(position.X + GetX(7.8F - halfthickwidth, width), position.Y + GetY(13F - halfthickheight, height));
            segmentD[4] = new PointF(position.X + GetX(7.8F - thickwidth, width), position.Y + GetY(13F, height));
            segmentD[5] = new PointF(position.X + GetX(1.2F + thickwidth, width), position.Y + GetY(13F, height));

            //Segment E
            PointF[] segmentE = new PointF[7];
            segmentE[0] = segmentE[6] = new PointF(position.X + GetX(1F, width), position.Y + GetY(7.2F, height));
            segmentE[1] = new PointF(position.X + GetX(1F + halfthickwidth, width), position.Y + GetY(7.2F, height));
            segmentE[2] = new PointF(position.X + GetX(1F + thickwidth, width), position.Y + GetY(7.2F + halfthickheight, height));
            segmentE[3] = new PointF(position.X + GetX(1F + thickwidth, width), position.Y + GetY(12.8F - thickheight, height));
            segmentE[4] = new PointF(position.X + GetX(1F + halfthickwidth, width), position.Y + GetY(12.8F - halfthickheight, height));
            segmentE[5] = new PointF(position.X + GetX(1F, width), position.Y + GetY(12.8F - thickheight, height));

            //Segment F
            PointF[] segmentF = new PointF[7];
            segmentF[0] = segmentF[6] = new PointF(position.X + GetX(1F, width), position.Y + GetY(1.2F + thickheight, height));
            segmentF[1] = new PointF(position.X + GetX(1F + halfthickwidth, width), position.Y + GetY(1.2F + halfthickheight, height));
            segmentF[2] = new PointF(position.X + GetX(1F + thickwidth, width), position.Y + GetY(1.2F + thickheight, height));
            segmentF[3] = new PointF(position.X + GetX(1F + thickwidth, width), position.Y + GetY(6.8F - halfthickheight, height));
            segmentF[4] = new PointF(position.X + GetX(1F + halfthickwidth, width), position.Y + GetY(7F, height));
            segmentF[5] = new PointF(position.X + GetX(1F, width), position.Y + GetY(7F, height));

            //Segment G
            PointF[] segmentG = new PointF[7];
            segmentG[0] = segmentG[6] = new PointF(position.X + GetX(1F + halfthickwidth, width), position.Y + GetY(7F, height));
            segmentG[1] = new PointF(position.X + GetX(1F + thickwidth, width), position.Y + GetY(7F - halfthickheight, height));
            segmentG[2] = new PointF(position.X + GetX(8F - thickwidth, width), position.Y + GetY(7F - halfthickheight, height));
            segmentG[3] = new PointF(position.X + GetX(8F - halfthickwidth, width), position.Y + GetY(7F, height));
            segmentG[4] = new PointF(position.X + GetX(8F - thickwidth, width), position.Y + GetY(7F + halfthickheight, height));
            segmentG[5] = new PointF(position.X + GetX(1F + thickwidth, width), position.Y + GetY(7F + halfthickheight, height));

            // Shear display
            using (Matrix m = new Matrix())
            {
                m.Translate(position.X, position.Y);
                m.Shear(-0.15F, 0.0F);
                m.Translate(-position.X, -position.Y);
                g.Transform = m;
            }

            //Segment DP
            #endregion

            #region Draw Segments Outline
            g.FillPolygon(outline.Brush, segmentA);
            g.FillPolygon(outline.Brush, segmentB);
            g.FillPolygon(outline.Brush, segmentC);
            g.FillPolygon(outline.Brush, segmentD);
            g.FillPolygon(outline.Brush, segmentE);
            g.FillPolygon(outline.Brush, segmentF);
            g.FillPolygon(outline.Brush, segmentG);
            #endregion

            #region Fill Segments
            //Fill SegmentA
            if (IsNumberAvailable(number, 0, 2, 3, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentA);
            }

            //Fill SegmentB
            if (IsNumberAvailable(number, 0, 1, 2, 3, 4, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentB);
            }

            //Fill SegmentC
            if (IsNumberAvailable(number, 0, 1, 3, 4, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentC);
            }

            //Fill SegmentD
            if (IsNumberAvailable(number, 0, 2, 3, 5, 6, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentD);
            }

            //Fill SegmentE
            if (IsNumberAvailable(number, 0, 2, 6, 8))
            {
                g.FillPolygon(fillPen.Brush, segmentE);
            }

            //Fill SegmentF
            if (IsNumberAvailable(number, 0, 4, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentF);
            }

            //Fill SegmentG
            if (IsNumberAvailable(number, 2, 3, 4, 5, 6, 8, 9, -1))
            {
                g.FillPolygon(fillPen.Brush, segmentG);
            }
            #endregion
            
            //Draw decimal point
            if (dp)
            {
                g.FillEllipse(fillPen.Brush, new RectangleF(
                    position.X + GetX(10F, width), 
                    position.Y + GetY(12F, height),
                    width/7, 
                    width/7));
            }

            // restore graphics settings
            g.Transform = oldtrans;
            g.SmoothingMode = oldsmooth;
            g.PixelOffsetMode = oldpixel;
        }

        /// <summary>
        /// Gets Relative X for the given width to draw digit
        /// </summary>
        /// <param name="x"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private float GetX(float x, float width)
        {
            return x * width / 12;
        }

        /// <summary>
        /// Gets relative Y for the given height to draw digit
        /// </summary>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private float GetY(float y, float height)
        {
            return y * height / 15;
        }

        /// <summary>
        /// Returns true if a given number is available in the given list.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="listOfNumbers"></param>
        /// <returns></returns>
        private bool IsNumberAvailable(int number, params int[] listOfNumbers)
        {
            if (listOfNumbers.Length > 0)
            {
                foreach (int i in listOfNumbers)
                {
                    if (i == number)
                        return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Restricts the size to make sure the height and width are always same.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AquaGauge_Resize(object sender, EventArgs e)
        {
            if (this.Width < 136)
            {
                this.Width = 136;
            }
            if (oldWidth != this.Width)
            {
                this.Height = this.Width;
                oldHeight = this.Width;
            }
            if (oldHeight != this.Height)
            {
                this.Width = this.Height;
                oldWidth = this.Width;
            }
        }
        #endregion
    }
}
