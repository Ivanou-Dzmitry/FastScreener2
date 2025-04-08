using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using static FastScreener2.FSUtils;
using static FastScreener2.FS2SettingsManager;
using System.Numerics;
using System.Drawing;

namespace FastScreener2
{
   
    public class SafeColorConverter : ColorConverter
    {
        [ThreadStatic]
        private static bool _showingMessage;

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                return base.ConvertFrom(context, culture, value);
            }
            catch
            {
                if (!_showingMessage)
                {
                    _showingMessage = true;

                    MessageBox.Show("Invalid color input. Reverting to previous value.",
                                    "Invalid Color",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                    _showingMessage = false;
                }

                if (context?.Instance != null)
                {
                    var property = context.PropertyDescriptor;
                    var currentValue = property?.GetValue(context.Instance);
                    return currentValue ?? Color.Cyan;
                }

                return Color.Cyan;
            }
        }
    }


    public class AppAppearance : INotifyPropertyChanged
    {
        private Color color;

        [Category("Appearance")]
        [Description("Set panels color.")]
        [DisplayName("Panel Color")]
        [TypeConverter(typeof(SafeColorConverter))]
        public Color PanelColor
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(PanelColor));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //ARROW
    public class Arrow : INotifyPropertyChanged
        {
            private int length;
            private Color color;

            [Category("Arrow Settings")]
            [Description("Set arrow length. The maximum length is equal to the hypotenuse of max resolution. Minimum - 8. Default - 50.")]
            [DisplayName("Arrow Length")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int Length
            {
                get => length;
                set
                {
                    //get max lenght
                    double halfDiag = GetMaxArrowLenght(resWorked);

                    if (value < 8 || value > halfDiag)
                    {
                        // Show the error message on top of the form
                        MessageBox.Show(
                            $"Length must be between 8 and {halfDiag:N0}.\n(Current input: {value})",
                            "Invalid Value",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return; // Don't set the value if it's invalid
                    }
                    length = value;
                    OnPropertyChanged(nameof(Length));
                }
            }

            [Category("Arrow Settings")]
            [Description("Set arrow color.")]
            [DisplayName("Arrow Color")]
            [TypeConverter(typeof(SafeColorConverter))]
            public Color Color
            {
                get => color;
                set
                {
                color = value;
                OnPropertyChanged(nameof(Color));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class FileFormatTypeConverter : StringConverter
        {
            private readonly List<string> validValues = new List<string> { "png", "jpg" };

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(validValues);
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string str)
                {
                    if (validValues.Contains(str))
                    {
                        return str;  // Return the valid string
                    }
                    else
                    {
                        // Show a message box with an error message
                        MessageBox.Show("Invalid value. Please select from the predefined list: png or jpg", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Optionally, return a default value if invalid input is entered
                        return "png";  // Set a default valid value
                    }
                }

                return base.ConvertFrom(context, culture, value);  // Delegate to the base method for other types
            }
        }

        public class GuideTypeConverter : StringConverter
        {
            private readonly List<string> validValues = new List<string> { "3x3", "4x4", "Custom" };

            // GetStandardValues will provide the list of allowed values
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(validValues);
            }

            // Check if StandardValues are supported
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            // ConvertFrom will ensure that only valid values are accepted
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string str)
                {
                    if (validValues.Contains(str))
                    {
                        return str;  // Return the valid string
                    }
                    else
                    {
                        // Show a message box with an error message
                        MessageBox.Show("Invalid value. Please select from the predefined list: 3x3, 4x4, or Custom.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Optionally, return a default value if invalid input is entered
                        return "3x3";  // Set a default valid value
                    }
                }

                return base.ConvertFrom(context, culture, value);  // Delegate to the base method for other types
            }
        }


    public class FrameTypeConverter : StringConverter
    {
        private readonly List<string> validValues = new List<string> { "Free", "Fixed"};

        // GetStandardValues will provide the list of allowed values
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(validValues);
        }

        // Check if StandardValues are supported
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        // ConvertFrom will ensure that only valid values are accepted
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                if (validValues.Contains(str))
                {
                    return str;  // Return the valid string
                }
                else
                {
                    // Show a message box with an error message
                    MessageBox.Show("Invalid value. Please select from the predefined list: Free or Fixed", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Optionally, return a default value if invalid input is entered
                    return "Free";  // Set a default valid value
                }
            }

            return base.ConvertFrom(context, culture, value);  // Delegate to the base method for other types
        }
    }

    //GUIDE
    public class Guide : INotifyPropertyChanged
        {
            private int topindent, bottomindent, leftindent, rightindent;
            private Color color;
            private bool lockind;
            private string type;

            [Category("1. General Settings")]
            [DisplayName("Guide Type")]
            [Description("Choose guides type. 3x3, 4x4 divides the area into equal parts, custom - set arbitrary indent from the edge")]
            [TypeConverter(typeof(GuideTypeConverter))] // Apply the custom TypeConverter here    
            public string Type
            {
                get => type;
                set
                {
                    type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }

            [Category("1. General Settings")]
            [Description("Set guides color.")]
            [DisplayName("Guides Color")]
            [TypeConverter(typeof(SafeColorConverter))]
            public Color Color
            {
                get => color;
                set
                {
                    color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }

            [Category("1. General Settings")]
            [Description("For Custom guide type. Set Bottom padding - other paddings change automatically.")]
            [DisplayName("Lock Indent")]
            public Boolean lockIndent
            {
                get => lockind;
                set
                {
                    lockind = value;
                    OnPropertyChanged(nameof(lockIndent));
                }
            }


            [Category("2. Custom Type Settings")]
            [Description("Max size screenshot height/2. Minimum - 1. Default - 10.")]
            [DisplayName("Top Indent")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int topIndent
            {
                get => topindent;
                set
                {

                Vector2 halfSize = GetHalfMaxScreenSize(resWorked);

                if (value < 1 || value > halfSize.Y)
                {
                    // Show the error message on top of the form
                    MessageBox.Show($"Indent must be between 1 and {halfSize.Y}.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                    topindent = value;
                    OnPropertyChanged(nameof(topIndent));
                }
            }

            [Category("2. Custom Type Settings")]
            [Description("Max size screenshot height/2. Minimum - 1. Default - 10. If Lock Indent is ON - it's main parameter")]
            [DisplayName("Bottom Indent")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int bottomIndent
            {
                get => bottomindent;
                set
                {
                    Vector2 halfSize = GetHalfMaxScreenSize(resWorked);

                    if (value < 1 || value > halfSize.Y)
                    {
                        // Show the error message on top of the form
                        MessageBox.Show($"Indent must be between 1 and {halfSize.Y}.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Don't set the value if it's invalid
                    }

                    if (!lockind)
                    {
                        bottomindent = value;
                    }
                    else
                    {
                        bottomindent = value;
                        topindent = value;
                        leftindent = value;
                        rightindent = value;
                    }                    
                    
                    OnPropertyChanged(nameof(bottomIndent));
                }
            }

            [Category("2. Custom Type Settings")]
            [Description("Max size screenshot width/2. Minimum - 1. Default - 10.")]
            [DisplayName("Left Indent")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int leftIndent
            {
                get => leftindent;
                set
                {
                    Vector2 halfSize = GetHalfMaxScreenSize(resWorked);
                    if (value < 1 || value > halfSize.X)
                    {
                        // Show the error message on top of the form
                        MessageBox.Show($"Indent must be between 1 and {halfSize.X}.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Don't set the value if it's invalid
                    }
                    leftindent = value;
                    OnPropertyChanged(nameof(leftIndent));
                }
            }

            [Category("2. Custom Type Settings")]
            [Description("Max size screenshot width/2. Minimum - 1. Default - 10.")]
            [DisplayName("Right Indent")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int rightIndent
            {
                get => rightindent;
                set
                {
                    Vector2 halfSize = GetHalfMaxScreenSize(resWorked);
                    if (value < 1 || value > halfSize.X)
                    {
                        // Show the error message on top of the form
                        MessageBox.Show($"Indent must be between 1 and {halfSize.X}.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Don't set the value if it's invalid
                    }
                    rightindent = value;
                    OnPropertyChanged(nameof(topIndent));
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

    }


    // FILE
    public class FileFormat
    {
        private string filetype;
        private int filecompess;

        [Category("File")]
        [Description("Chose file format. Default - png")]
        [DisplayName("Format")]
        [TypeConverter(typeof(FileFormatTypeConverter))]
        public string fileType
        {
            get => filetype;
            set
            {
                filetype = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        [Category("File")]
        [Description("Set сompression quality of JPG file. From 1 to 100. Default - 75")]
        [DisplayName("Compression")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int fileCompress
        {
            get => filecompess;
            set
            {
                if (value < 1 || value > 100)
                {
                    MessageBox.Show("Compression quality can be from 1 to 100", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                filecompess = value;
                OnPropertyChanged(nameof(fileCompress));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //FRAME
    public class Frame
    {
        private int framewidth;
        private int frameheight;
        private int strokewidth;
        private Color color;
        private string type;

        [Category("2. Fixed Frame Settings")]
        [Description("Frame width (in px). Max - screenshot width/2, min - 16. Default 80.")]
        [DisplayName("Frame Width")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int frameWidth
        {
            get => framewidth;
            set
            {
                Vector2 halfResolution = GetHalfMaxScreenSize(resWorked);

                // Validation for FrameWidth
                if (value < 16 || value > halfResolution.X)
                {
                    MessageBox.Show($"Frame width must be between 16 and {halfResolution.X}.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                framewidth = value;
                OnPropertyChanged(nameof(frameWidth));
            }
        }

        [Category("2. Fixed Frame Settings")]
        [Description("Frame height (in px). Max - screenshot height/2, min - 16. Default 80.")]
        [DisplayName("Frame Height")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int frameHeight
        {
            get => frameheight;
            set
            {
                Vector2 halfResolution = GetHalfMaxScreenSize(resWorked);
                // Validation for FrameHeight
                if (value < 16 || value > halfResolution.Y)
                {
                    MessageBox.Show($"Frame height must be between 16 and {halfResolution.Y}.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                frameheight = value;
                OnPropertyChanged(nameof(frameHeight));
            }
        }

        [Category("1. Base Frame Settings")]
        [Description("Frame stroke width (in px). Max - 10, min - 1. Default 1.")]
        [DisplayName("Frame Stroke Width")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int strokeWidth
        {
            get => strokewidth;
            set
            {
                // Validation for FrameWidth
                if (value < 1 || value > 10)
                {
                    MessageBox.Show("Frame stroke width must be between 1 and 10.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                strokewidth = value;
                OnPropertyChanged(nameof(strokeWidth));
            }
        }


        [Category("1. Base Frame Settings")]
        [Description("Set frame color.")]
        [DisplayName("Frame Color")]
        [TypeConverter(typeof(SafeColorConverter))]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        [Category("1. Base Frame Settings")]
        [DisplayName("Frame Type")]
        [Description("Choose frame type. Fixed (click and draw with fixed size)  or Free (drag draw). Set W and H for Fixed type.")]
        [TypeConverter(typeof(FrameTypeConverter))]
        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }



    //NUMBER
    class Numbers
    {
        private int size;
        private Color color;

        [Category("Numbers Settings")]
        [Description("Set number font size. Max - 100, min - 8. Default - 26.")]
        [DisplayName("Number Font Size")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int Size
        {
            get => size;
            set
            {
                if (value < 8 || value > 100)
                {
                    // Show the error message on top of the form
                    MessageBox.Show("Font size must be between 8 and 100.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    size = 26;
                    return; // Don't set the value if it's invalid
                }

                size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        [Category("Numbers Settings")]
        [Description("Set numbers color.")]
        [DisplayName("Number Color")]
        [TypeConverter(typeof(SafeColorConverter))]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //BAR
    class Bars
    {        
        private Color color;

        [Category("Bar Settings")]
        [Description("Set bar color.")]
        [DisplayName("Bar Color")]
        [TypeConverter(typeof(SafeColorConverter))]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //RESOLUTION
    class Resolutions
    {
        private int r1w, r1h, r2w, r2h, r3w, r3h, r4w, r4h;

        [Category("Size 1")]
        [Description("Size 1 width. Max depends on your smalest monitor resolution. Min - 550px")]
        [DisplayName("1.1 Width")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int res1Width
        {
            get => r1w;
            set
            {
                Vector2 smalestRes = GetSmallestScreenResolution();

                if (value < MIN_WIDTH || value > smalestRes.X)
                {
                    MessageBox.Show($"The value can be from {MIN_WIDTH}px to {smalestRes.X}px", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    return; // Don't set the value if it's invalid
                }

                r1w = value;
                OnPropertyChanged(nameof(res1Width));
            }
        }

        [Category("Size 1")]
        [Description("Size 1 height. Max depends on your smalest monitor resolution. Min - 200px")]
        [DisplayName("1.2 Height")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int res1Height
        {
            get => r1h;
            set
            {
                Vector2 smalestRes = GetSmallestScreenResolution();

                if (value < MIN_HEIGHT || value > smalestRes.Y)
                {
                    MessageBox.Show($"The value can be from {MIN_HEIGHT}px to {smalestRes.Y}px", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }

                r1h = value;
                OnPropertyChanged(nameof(res1Height));
            }
        }

        [Category("Size 2")]
        [Description("Size 2 width. Max depends on your smalest monitor resolution. Min - 550px")]
        [DisplayName("2.1 Width")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int res2Width
        {
            get => r2w;
            set
            {
                Vector2 smalestRes = GetSmallestScreenResolution();

                if (value < MIN_WIDTH || value > smalestRes.X)
                {
                    MessageBox.Show($"The value can be from {MIN_WIDTH}px to {smalestRes.X}px", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                r2w = value;
                OnPropertyChanged(nameof(res2Width));
            }
        }

        [Category("Size 2")]
        [Description("Size 2 height. Max depends on your smalest monitor resolution. Min - 200px")]
        [DisplayName("2.2 Height")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int res2Height
        {
            get => r2h;
            set
            {
                Vector2 smalestRes = GetSmallestScreenResolution();

                if (value < MIN_HEIGHT || value > smalestRes.Y)
                {
                    MessageBox.Show($"The value can be from {MIN_HEIGHT}px to {smalestRes.Y}px", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                r2h = value;
                OnPropertyChanged(nameof(res2Height));
            }
        }

        [Category("Size 3")]
        [Description("Size 3 width. Max depends on your smalest monitor resolution. Min - 550px")]
        [DisplayName("3.1 Width")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int res3Width
        {
            get => r3w;
            set
            {
                Vector2 smalestRes = GetSmallestScreenResolution();

                if (value < MIN_WIDTH || value > smalestRes.X)
                {
                    MessageBox.Show($"The value can be from {MIN_WIDTH}px to {smalestRes.X}px", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                r3w = value;
                OnPropertyChanged(nameof(res3Width));
            }
        }

        [Category("Size 3")]
        [Description("Size 3 height. Max depends on your smalest monitor resolution. Min - 200px")]
        [DisplayName("3.2 Height")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int res3Height
        {
            get => r3h;
            set
            {
                Vector2 smalestRes = GetSmallestScreenResolution();

                if (value < MIN_HEIGHT || value > smalestRes.Y)
                {
                    MessageBox.Show($"The value can be from {MIN_HEIGHT}px to {smalestRes.Y}px", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                r3h = value;
                OnPropertyChanged(nameof(res3Height));
            }
        }

        [Category("Size 4")]
        [Description("Size 4 width. Max depends on your smalest monitor resolution. Min - 550px")]
        [DisplayName("4.1 Width")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int res4Width
        {
            get => r4w;
            set
            {
                Vector2 smalestRes = GetSmallestScreenResolution();

                if (value < MIN_WIDTH || value > smalestRes.X)
                {
                    MessageBox.Show($"The value can be from {MIN_WIDTH}px to {smalestRes.X}px", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                r4w = value;
                OnPropertyChanged(nameof(res4Width));
            }
        }

        [Category("Size 4")]
        [Description("Size 4 height. Max depends on your smalest monitor resolution. Min - 200px")]
        [DisplayName("4.2 Height")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int res4Height
        {
            get => r4h;
            set
            {
                Vector2 smalestRes = GetSmallestScreenResolution();

                if (value < MIN_HEIGHT || value > smalestRes.Y)
                {
                    MessageBox.Show($"The value can be from {MIN_HEIGHT}px to {smalestRes.Y}px", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                r4h = value;
                OnPropertyChanged(nameof(res4Height));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Int32OnlyConverter : TypeConverter
    {
        private static bool messageBoxShown = false; // Static flag to track message box display

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string str)
            {
                if (int.TryParse(str, out int result))
                {
                    return result;
                }
                else
                {
                    // Show the message box only if it hasn't been shown yet
                    if (!messageBoxShown)
                    {
                        // Display message box to inform the user about invalid input
                        MessageBox.Show(context?.Instance as IWin32Window, "Invalid integer format. Please enter a valid integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        messageBoxShown = true; // Set the flag to true after showing the message
                    }

                    // Return default value in case of failure
                    return 0;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }



}
