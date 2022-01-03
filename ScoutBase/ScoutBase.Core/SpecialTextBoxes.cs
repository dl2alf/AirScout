using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace ScoutBase.Core
{
    [System.ComponentModel.DesignerCategory("")]
    public class SilentTextBox : TextBox
    {
        /// <summary>
        /// Sets the text box content without firing a TextChanged event
        /// </summary>
        public string SilentText
        {
            set
            {
                Silent = true;
                this.Text = value;
                Silent = false;
                oldtext = Text;
                oldselectionstart = SelectionStart;
                oldselectionlength = SelectionLength;
            }
        }
        private bool Silent = false;

        protected string oldtext = "";
        /// <summary>
        /// Keeps the old value before a TextChanged event is raised.
        /// </summary>
        public string OldText
        {
            get
            {
                return oldtext;
            }
        }

        protected int oldselectionstart = 0;
        /// <summary>
        /// Keeps the old value of SelectionStart before a TextChanged event is raised.
        /// </summary>
        public int OldSelectionStart
        {
            get
            {
                return oldselectionstart;
            }
        }

        protected int oldselectionlength = 0;
        /// <summary>
        /// Keeps the old value of SelectionStop before a TextChanged event is raised.
        /// </summary>
        public int OldSelectionLength
        {
            get
            {
                return oldselectionlength;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            // keep the old selection
            oldselectionstart = SelectionStart;
            oldselectionlength = SelectionLength;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            // keep the old selection
            oldselectionstart = SelectionStart;
            oldselectionlength = SelectionLength;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // raise event only if the control is in non-silent state
            if (!Silent)
            {
                base.OnTextChanged(e);
            }
            // keep old values for next TextChanged event
            oldtext = Text;
            oldselectionstart = SelectionStart;
            oldselectionlength = SelectionLength;
        }
    }

    public class LongTextBox : SilentTextBox
    {
        string formatspecifier = "F0";
        /// <summary>
        /// String format specifier for numeric value
        /// </summary>
        public string FormatSpecifier
        {
            get
            {
                return formatspecifier;
            }
            set
            {
                try
                {
                    // try a ToString call to verfiy the format specifier
                    string s = this.value.ToString(formatspecifier, CultureInfo.InvariantCulture);
                    formatspecifier = value;
                    Text = s;
                }
                catch
                {
                    throw new ArgumentException("Invalid string format specifier: " + value);
                }
            }
        }

        double minvalue = double.NaN;
        /// <summary>
        /// Minimum allowed value
        /// </summary>
        public long MinValue
        {
            get
            {
                if (double.IsNaN(minvalue))
                    return 0;
                return (long)minvalue;
            }
            set
            {
                // checking for min/max 
                if (!double.IsNaN(maxvalue) && (value > maxvalue))
                    throw new ArgumentOutOfRangeException("MinValue must be less or equal than MaxValue: " + value.ToString());
                minvalue = value;
            }
        }

        double maxvalue = double.NaN;
        /// <summary>
        /// Maximum allowed value
        /// </summary>
        public long MaxValue
        {
            get
            {
                if (double.IsNaN(maxvalue))
                    return 0;
                return (long)maxvalue;
            }
            set
            {
                // checking for min/max 
                if (!double.IsNaN(minvalue) && (value < minvalue))
                    throw new ArgumentOutOfRangeException("MaxValue must be greater or equal than MinValue: " + value.ToString());
                maxvalue = value;
            }
        }

        double value = double.NaN;
        /// <summary>
        /// Value
        /// </summary>
        public long Value
        {
            get
            {
                if (double.IsNaN(value))
                    return 0;
                return (long)value;
            }
            set
            {
                if (double.IsNaN(value) || CheckBounds(value))
                {
                    this.value = value;
                    if (!double.IsNaN(value))
                        this.Text = value.ToString(FormatSpecifier, CultureInfo.InvariantCulture);
                    else this.Text = "";
                }
            }
        }

        /// <summary>
        /// Set the value without firing a TextChanged event
        /// </summary>
        public long SilentValue
        {
            set
            {
                if (double.IsNaN(value) || CheckBounds(value))
                {
                    this.value = value;
                    this.SilentText = value.ToString(FormatSpecifier, CultureInfo.InvariantCulture);
                }
            }
        }

        private string CharsAllowed = "0123456789-\b";

        private bool CheckBounds(double v)
        {
            // no range check when min == max
            if (minvalue == maxvalue)
                return true;
            // range checking only when min/max are set
            if (!double.IsNaN(minvalue) && (v < minvalue))
                return false;
            if (!double.IsNaN(maxvalue) && (v > maxvalue))
                return false;
            return true;
        }

        private void UndoText()
        {
            // simulate an Undo() --> Undo() does not work inside Keypressed or TextChanged
            Console.Beep();
            int start = OldSelectionStart;
            int length = OldSelectionLength;
            SilentText = OldText;
            Select(start, length);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // check for valid numeric input
            if (!char.IsControl(e.KeyChar) && (CharsAllowed.IndexOf(e.KeyChar) < 0))
            {
                Console.Beep();
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!String.IsNullOrEmpty(Text))
            {
                // do checks and reset Text to old value in case of error
                // check '-' is allowed only once and only on 1st position
                if (Text.LastIndexOf('-') > 0)
                    UndoText();
                // check bounds
                try
                {
                    value = System.Convert.ToDouble(Text, CultureInfo.InvariantCulture);
                }
                catch
                {
                    value = double.NaN;
                }
                if (!double.IsNaN(value) && !CheckBounds(value))
                    UndoText();
                base.OnTextChanged(e);
            }
        }
    }

    public class Int32TextBox : SilentTextBox
    {
        string formatspecifier = "F0";
        /// <summary>
        /// String format specifier for numeric value
        /// </summary>
        public string FormatSpecifier
        {
            get
            {
                return formatspecifier;
            }
            set
            {
                try
                {
                    // try a ToString call to verfiy the format specifier
                    string s = this.value.ToString(formatspecifier, CultureInfo.InvariantCulture);
                    formatspecifier = value;
                    Text = s;
                }
                catch
                {
                    throw new ArgumentException("Invalid string format specifier: " + value);
                }
            }
        }

        double minvalue = double.NaN;
        /// <summary>
        /// Minimum allowed value
        /// </summary>
        public int MinValue
        {
            get
            {
                if (double.IsNaN(minvalue))
                    return 0;
                return (int)minvalue;
            }
            set
            {
                // checking for min/max 
                if (!double.IsNaN(maxvalue) && (value > maxvalue))
                    throw new ArgumentOutOfRangeException("MinValue must be less or equal than MaxValue: " + value.ToString());
                minvalue = value;
            }
        }

        double maxvalue = double.NaN;
        /// <summary>
        /// Maximum allowed value
        /// </summary>
        public int MaxValue
        {
            get
            {
                if (double.IsNaN(maxvalue))
                    return 0;
                return (int)maxvalue;
            }
            set
            {
                // checking for min/max 
                if (!double.IsNaN(minvalue) && (value < minvalue))
                    throw new ArgumentOutOfRangeException("MaxValue must be greater or equal than MinValue: " + value.ToString());
                maxvalue = value;
            }
        }

        double value = double.NaN;
        /// <summary>
        /// Value
        /// </summary>
        public int Value
        {
            get
            {
                if (double.IsNaN(value))
                    return 0;
                return (int)value;
            }
            set
            {
                if (double.IsNaN(value) || CheckBounds(value))
                {
                    this.value = value;
                    if (!double.IsNaN(value))
                        this.Text = value.ToString(FormatSpecifier, CultureInfo.InvariantCulture);
                    else this.Text = "";
                }
            }
        }

        /// <summary>
        /// Set the value without firing a TextChanged event
        /// </summary>
        public int SilentValue
        {
            set
            {
                if (double.IsNaN(value) || CheckBounds(value))
                {
                    this.value = value;
                    this.SilentText = value.ToString(FormatSpecifier, CultureInfo.InvariantCulture);
                }
            }
        }

        private string CharsAllowed = "0123456789-\b";

        private bool CheckBounds(double v)
        {
            // no range check when min == max
            if (minvalue == maxvalue)
                return true;
            // range checking only when min/max are set
            if (!double.IsNaN(minvalue) && (v < minvalue))
                return false;
            if (!double.IsNaN(maxvalue) && (v > maxvalue))
                return false;
            return true;
        }

        private void UndoText()
        {
            // simulate an Undo() --> Undo() does not work inside Keypressed or TextChanged
            Console.Beep();
            int start = OldSelectionStart;
            int length = OldSelectionLength;
            SilentText = OldText;
            Select(start, length);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // check for valid numeric input
            if (!char.IsControl(e.KeyChar) && (CharsAllowed.IndexOf(e.KeyChar) < 0))
            {
                Console.Beep();
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!String.IsNullOrEmpty(Text))
            {
                // do checks and reset Text to old value in case of error
                // check '-' is allowed only once and only on 1st position
                if (Text.LastIndexOf('-') > 0)
                    UndoText();
                // check bounds
                try
                {
                    value = System.Convert.ToDouble(Text, CultureInfo.InvariantCulture);
                }
                catch
                {
                    value = double.NaN;
                }
                if (!double.IsNaN(value) && !CheckBounds(value))
                    UndoText();
                base.OnTextChanged(e);
            }
        }
    }

    public class DoubleTextBox : SilentTextBox
    {
        string formatspecifier = "";
        /// <summary>
        /// String format specifier for numeric value
        /// </summary>
        public string FormatSpecifier
        {
            get
            {
                return formatspecifier;
            }
            set
            {
                try
                {
                    // try a ToString call to verfiy the format specifier
                    string s = this.value.ToString(formatspecifier, CultureInfo.InvariantCulture);
                    formatspecifier = value;
                    Text = s;
                }
                catch
                {
                    throw new ArgumentException("Invalid string format specifier: " + value);
                }
            }
        }

        double minvalue = double.NaN;
        /// <summary>
        /// Minimum allowed value
        /// </summary>
        public double MinValue
        {
            get
            {
//                if (double.IsNaN(minvalue))
//                    return 0;
                return minvalue;
            }
            set
            {
                // checking for min/max 
                if (!double.IsNaN(maxvalue) && (value > maxvalue))
                    throw new ArgumentOutOfRangeException("MinValue must be less or equal than MaxValue: " + value.ToString());
                minvalue = value;
            }
        }

        double maxvalue = double.NaN;
        /// <summary>
        /// Maximum allowed value
        /// </summary>
        public double MaxValue
        {
            get
            {
//                if (double.IsNaN(maxvalue))
//                    return 0;
                return maxvalue;
            }
            set
            {
                // checking for min/max 
                if (!double.IsNaN(minvalue) && (value < minvalue))
                    throw new ArgumentOutOfRangeException("MaxValue must be greater or equal than MinValue: " + value.ToString());
                maxvalue = value;
            }
        }

        double value = double.NaN;
        /// <summary>
        /// Value
        /// </summary>
        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                if (double.IsNaN(value) || CheckBounds(value))
                {
                    this.value = value;
                    if (!double.IsNaN(value))
                        this.Text = value.ToString(FormatSpecifier, CultureInfo.InvariantCulture);
                    else this.Text = "";
                }
            }
        }

        /// <summary>
        /// Set the value without firing a TextChanged event
        /// </summary>
        public double SilentValue
        {
            set
            {
                if (double.IsNaN(value) || CheckBounds(value))
                {
                    this.value = value;
                    if (!double.IsNaN(value))
                        this.SilentText = value.ToString(FormatSpecifier, CultureInfo.InvariantCulture);
                    else this.SilentText = "";
                }
            }
        }

        private string CharsAllowed = "0123456789-.\b";

        private bool CheckBounds(double v)
        {
            // no range check when min == max
            if (minvalue == maxvalue)
                return true;
            // range checking only when min/max are set
            if (!double.IsNaN(minvalue) && (v < minvalue))
                return false;
            if (!double.IsNaN(maxvalue) && (v > maxvalue))
                return false;
            return true;
        }

        private void UndoText()
        {
            // simulate an Undo() --> Undo() does not work inside Keypressed or TextChanged
            Console.Beep();
            int start = OldSelectionStart;
            int length = OldSelectionLength;
            SilentText = OldText;
            Select(start, length);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // check for valid numeric input
            if (!char.IsControl(e.KeyChar) && (CharsAllowed.IndexOf(e.KeyChar) < 0))
            {
                Console.Beep();
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && (Text.IndexOf('.') > -1))
            {
                Console.Beep();
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!String.IsNullOrEmpty(Text))
            {
                // do checks and reset Text to old value in case of error
                // check '-' is allowed only once and only on 1st position
                if (Text.LastIndexOf('-') > 0)
                    UndoText();
                // check bounds
                try
                {
                    value = System.Convert.ToDouble(Text, CultureInfo.InvariantCulture);
                }
                catch
                {
                    value = double.NaN;
                }
                if (!double.IsNaN(value) && !CheckBounds(value))
                    UndoText();
                base.OnTextChanged(e);
            }
            else
                value = double.NaN;
        }
}

    public class LocatorTextBox : SilentTextBox
    {

        private bool smalllettersforsubsquares = true;
        /// <summary>
        /// Use small letters for subsquares.
        /// </summary>
        public bool SmallLettersForSubsquares
        {
            get
            {
                return smalllettersforsubsquares;
            }
            set
            {
                if (smalllettersforsubsquares != value)
                {
                    smalllettersforsubsquares = value;
                    // convert Text
                    Text = MaidenheadLocator.Convert(Text, smalllettersforsubsquares);
                }
            }
        }

        private Color errorbackcolor = Color.Red;
        /// <summary>
        /// Sets the text box back color in case of an error.
        /// </summary>
        public Color ErrorBackColor
        {
            get
            {
                return errorbackcolor;
            }
            set
            {
                errorbackcolor = value;
            }
        }

        private Color errorforecolor = Color.White;
        /// <summary>
        /// Sets the text box fore color in case of an error.
        /// </summary>
        public Color ErrorForeColor
        {
            get
            {
                return errorforecolor;
            }
            set
            {
                errorforecolor = value;
            }
        }

        /// <summary>
        /// Sets the text box back color.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                // keep the BackColor
                OriginalBackColor = value;
            }
        }

        /// <summary>
        /// Sets the text box fore color.
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                // keep the ForeColor
                OriginalForeColor = value;
            }
        }

        private string CharsAllowed = "ABCDEFGHIJKLMNOPQRTSUVWXabcdefghijklmnopqrstuvwx0123456789";
        private string ControlsAllowed = "\b";

        private Color OriginalBackColor;
        private Color OriginalForeColor;

        public LocatorTextBox()
            : base()
        {
            OriginalBackColor = base.BackColor;
            OriginalForeColor = base.ForeColor;
        }
                  
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // check for valid numeric input
            if (!char.IsControl(e.KeyChar) && (CharsAllowed.IndexOf(e.KeyChar) < 0) && (ControlsAllowed.IndexOf(e.KeyChar) < 0))
            {
                Console.Beep();
                e.Handled = true;
            }
            // convert char if necessary
            if (CharsAllowed.IndexOf(e.KeyChar) >= 0)
            {
                if (Text.Length <= 1)
                    e.KeyChar = Char.ToUpper(e.KeyChar);
                else if (smalllettersforsubsquares)
                {
                    e.KeyChar = Char.ToLower(e.KeyChar);
                }
                else
                    e.KeyChar = Char.ToUpper(e.KeyChar);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            // reset text color to original color
            ForeColor = OriginalForeColor;
            BackColor = OriginalBackColor;
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (!String.IsNullOrEmpty(Text))
            {
                // check if text is a valid Maidenhead Locator
                if (MaidenheadLocator.Check(Text))
                {
                    ForeColor = OriginalForeColor;
                    BackColor = OriginalBackColor;
                }
                else
                {
                    base.ForeColor = ErrorForeColor;
                    base.BackColor = ErrorBackColor;
                    e.Cancel = true;
                }
            }
        }
    }

    public class CallsignTextBox : SilentTextBox
    {

        private Color errorbackcolor = Color.Red;
        /// <summary>
        /// Sets the text box back color in case of an error.
        /// </summary>
        public Color ErrorBackColor
        {
            get
            {
                return errorbackcolor;
            }
            set
            {
                errorbackcolor = value;
            }
        }

        private Color errorforecolor = Color.White;
        /// <summary>
        /// Sets the text box fore color in case of an error.
        /// </summary>
        public Color ErrorForeColor
        {
            get
            {
                return errorforecolor;
            }
            set
            {
                errorforecolor = value;
            }
        }

        /// <summary>
        /// Sets the text box back color.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                // keep the BackColor
                OriginalBackColor = value;
            }
        }

        /// <summary>
        /// Sets the text box fore color.
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                // keep the ForeColor
                OriginalForeColor = value;
            }
        }

        private string CharsAllowed = "ABCDEFGHIJKLMNOPQRTSUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/";
        private string ControlsAllowed = "\b";

        private Color OriginalBackColor;
        private Color OriginalForeColor;

        public CallsignTextBox()
            : base()
        {
            OriginalBackColor = base.BackColor;
            OriginalForeColor = base.ForeColor;
            CharacterCasing = CharacterCasing.Upper;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // check for valid numeric input
            if (!char.IsControl(e.KeyChar) && (CharsAllowed.IndexOf(e.KeyChar) < 0) && (ControlsAllowed.IndexOf(e.KeyChar) < 0))
            {
                Console.Beep();
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            // reset text color to original color
            ForeColor = OriginalForeColor;
            BackColor = OriginalBackColor;
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (!String.IsNullOrEmpty(Text))
            {
                // check if text is a valid callsign
                if (Callsign.Check(Text))
                {
                    ForeColor = OriginalForeColor;
                    BackColor = OriginalBackColor;
                }
                else
                {
                    base.ForeColor = ErrorForeColor;
                    base.BackColor = ErrorBackColor;
                    e.Cancel = true;
                }
            }
        }
    }

    public class VerticalTextBox : System.Windows.Forms.Control
    {
        public VerticalTextBox()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            using (StringFormat format = new StringFormat(StringFormatFlags.DirectionVertical))
            {
                using (SolidBrush brush = new SolidBrush(ForeColor))
                {
                    g.DrawString(Text, Font, brush, 0, 0, format);
                    g.RotateTransform(180);
                }
            }
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    public class SilentComboBox : ComboBox
    {
        /// <summary>
        /// Sets the text box content without firing a TextChanged event
        /// </summary>
        public string SilentText
        {
            set
            {
                Silent = true;
                this.Text = value;
                Silent = false;
                oldtext = Text;
                oldselectionstart = SelectionStart;
                oldselectionlength = SelectionLength;
            }
        }
        private bool Silent = false;

        protected string oldtext = "";
        /// <summary>
        /// Keeps the old value before a TextChanged event is raised.
        /// </summary>
        public string OldText
        {
            get
            {
                return oldtext;
            }
        }

        protected int oldselectionstart = 0;
        /// <summary>
        /// Keeps the old value of SelectionStart before a TextChanged event is raised.
        /// </summary>
        public int OldSelectionStart
        {
            get
            {
                return oldselectionstart;
            }
        }

        protected int oldselectionlength = 0;
        /// <summary>
        /// Keeps the old value of SelectionStop before a TextChanged event is raised.
        /// </summary>
        public int OldSelectionLength
        {
            get
            {
                return oldselectionlength;
            }
        }

        /// <summary>
        /// Sets the text box content without firing a TextChanged event
        /// </summary>
        public System.Windows.Forms.CharacterCasing CharacterCasing { get; set; }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                // change keychar according to character casing property
                switch (this.CharacterCasing)
                {
                    case CharacterCasing.Upper:
                        e.KeyChar = Char.ToUpper(e.KeyChar);
                        break;
                    case CharacterCasing.Lower:
                        e.KeyChar = Char.ToLower(e.KeyChar);
                        break;
                }
            }
            base.OnKeyPress(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            // keep the old selection
            oldselectionstart = SelectionStart;
            oldselectionlength = SelectionLength;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            // keep the old selection
            oldselectionstart = SelectionStart;
            oldselectionlength = SelectionLength;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // raise event only if the control is in non-silent state
            if (!Silent)
            {
                base.OnTextChanged(e);
            }
            // keep old values for next TextChanged event
            oldtext = Text;
            oldselectionstart = SelectionStart;
            oldselectionlength = SelectionLength;
        }
    }

    public class LocatorComboBox : SilentComboBox
    {

        private bool smalllettersforsubsquares = true;
        /// <summary>
        /// Sets the text box back color in case of an error.
        /// </summary>
        public bool SmallLettersForSubsquares
        {
            get
            {
                return smalllettersforsubsquares;
            }
            set
            {
                if (smalllettersforsubsquares != value)
                {
                    smalllettersforsubsquares = value;
                    // convert Text
                    Text = MaidenheadLocator.Convert(Text, smalllettersforsubsquares);
                }
            }
        }

        private Color errorbackcolor = Color.Red;
        /// <summary>
        /// Sets the text box back color in case of an error.
        /// </summary>
        public Color ErrorBackColor
        {
            get
            {
                return errorbackcolor;
            }
            set
            {
                errorbackcolor = value;
            }
        }

        private Color errorforecolor = Color.White;
        /// <summary>
        /// Sets the text box fore color in case of an error.
        /// </summary>
        public Color ErrorForeColor
        {
            get
            {
                return errorforecolor;
            }
            set
            {
                errorforecolor = value;
            }
        }

        /// <summary>
        /// Sets the text box back color.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                // keep the BackColor
                OriginalBackColor = value;
            }
        }

        /// <summary>
        /// Sets the text box fore color.
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                // keep the ForeColor
                OriginalForeColor = value;
            }
        }

        private LatLon.GPoint geolocation = new LatLon.GPoint(double.NaN, double.NaN);
        /// <summary>
        /// The geographical loctaion represented by the locator in the TextBox, null if locator is not valid
        /// </summary>
        public LatLon.GPoint GeoLocation
        {
            get
            {
                return geolocation;
            }
            set
            {
                if ((value != null) && (GeographicalPoint.Check(value.Lat, value.Lon)))
                {
                    geolocation = value;
                    locationchanged = true;
                    this.Text = MaidenheadLocator.LocFromLatLon(geolocation.Lat, geolocation.Lon, smalllettersforsubsquares, precision, autolength);
                    this.OnTextChanged(new EventArgs());
                }
            }
        }

        private int precision = 3;
        /// <summary>
        /// Precision for generating locator from lat/lon
        /// </summary>
        public int Precision
        {
            get
            {
                return precision;
            }
            set
            {
                precision = value;
            }
        }

        private bool autolength = false;
        /// <summary>
        /// Cut the locator length automatically according to precision
        /// </summary>
        public bool AutoLength
        {
            get
            {
                return autolength;
            }
            set
            {
                autolength = value;
            }
        }

        /// <summary>
        /// Changes the selected item in a drop drown list without firing a TextChanged event
        /// </summary>
        public bool SilentItemChange { get; set; }


        private bool itemchanged = false;
        private bool locationchanged = false;

        private string CharsAllowed = "ABCDEFGHIJKLMNOPQRTSUVWXabcdefghijklmnopqrstuvwx0123456789";
        private string ControlsAllowed = "\b";

        private Color OriginalBackColor;
        private Color OriginalForeColor;

        public LocatorComboBox()
            : base()
        {
            OriginalBackColor = base.BackColor;
            OriginalForeColor = base.ForeColor;
            SilentItemChange = false;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // check for valid numeric input
            if (!char.IsControl(e.KeyChar) && (CharsAllowed.IndexOf(e.KeyChar) < 0) && (ControlsAllowed.IndexOf(e.KeyChar) < 0))
            {
                Console.Beep();
                e.Handled = true;
            }
            // convert char if necessary
            if (CharsAllowed.IndexOf(e.KeyChar) >= 0)
            {
                if (Text.Length <= 1)
                    e.KeyChar = Char.ToUpper(e.KeyChar);
                else if (smalllettersforsubsquares)
                {
                    e.KeyChar = Char.ToLower(e.KeyChar);
                }
                else
                    e.KeyChar = Char.ToUpper(e.KeyChar);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {

            if (!SilentItemChange || locationchanged)
            {
                base.OnTextChanged(e);
            }
            // reset text color to original color
            ForeColor = OriginalForeColor;
            BackColor = OriginalBackColor;
            itemchanged = false;
            locationchanged = false;
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            base.OnTextUpdate(e);
            // set the location value 
            if (MaidenheadLocator.Check(this.Text))
                if (this.Text.Length >= 6)
                    GeoLocation = MaidenheadLocator.GPointFromLoc(this.Text);
                else 
                    geolocation = MaidenheadLocator.GPointFromLoc(this.Text);
            else
                GeoLocation = null;
            itemchanged = false;
            locationchanged = false;
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
        }

        protected override void OnSelectedItemChanged(EventArgs e)
        {
            base.OnSelectedItemChanged(e);
        }

        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            itemchanged = true;
            base.OnSelectionChangeCommitted(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (!String.IsNullOrEmpty(Text))
            {
                // check if text is a valid Maidenhead Locator
                if (MaidenheadLocator.Check(Text))
                {
                    ForeColor = OriginalForeColor;
                    BackColor = OriginalBackColor;
                }
                else
                {
                    base.ForeColor = ErrorForeColor;
                    base.BackColor = ErrorBackColor;
                    e.Cancel = true;
                }
            }
        }
    }

    public class CallsignComboBox : SilentComboBox
    {

        private Color errorbackcolor = Color.Red;
        /// <summary>
        /// Sets the text box back color in case of an error.
        /// </summary>
        public Color ErrorBackColor
        {
            get
            {
                return errorbackcolor;
            }
            set
            {
                errorbackcolor = value;
            }
        }

        private Color errorforecolor = Color.White;
        /// <summary>
        /// Sets the text box fore color in case of an error.
        /// </summary>
        public Color ErrorForeColor
        {
            get
            {
                return errorforecolor;
            }
            set
            {
                errorforecolor = value;
            }
        }

        /// <summary>
        /// Sets the text box back color.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                // keep the BackColor
                OriginalBackColor = value;
            }
        }

        /// <summary>
        /// Sets the text box fore color.
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                // keep the ForeColor
                OriginalForeColor = value;
            }
        }

        private string CharsAllowed = "ABCDEFGHIJKLMNOPQRTSUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/";
        private string ControlsAllowed = "\b";

        private Color OriginalBackColor;
        private Color OriginalForeColor;

        public CallsignComboBox()
            : base()
        {
            OriginalBackColor = base.BackColor;
            OriginalForeColor = base.ForeColor;
            this.CharacterCasing = CharacterCasing.Upper;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // check for valid numeric input
            if (!char.IsControl(e.KeyChar) && (CharsAllowed.IndexOf(e.KeyChar) < 0) && (ControlsAllowed.IndexOf(e.KeyChar) < 0))
            {
                Console.Beep();
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            // reset text color to original color
            ForeColor = OriginalForeColor;
            BackColor = OriginalBackColor;
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            if (!String.IsNullOrEmpty(Text))
            {
                // check if text is a valid callsign
                if (Callsign.Check(Text))
                {
                    ForeColor = OriginalForeColor;
                    BackColor = OriginalBackColor;
                }
                else
                {
                    base.ForeColor = ErrorForeColor;
                    base.BackColor = ErrorBackColor;
                    e.Cancel = true;
                }
            }
        }
    }


}
