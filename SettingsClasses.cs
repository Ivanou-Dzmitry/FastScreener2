using System.ComponentModel;
using System.Globalization;  
using System.Drawing; // If Color is from System.Drawing namespace

namespace FastScreener2
{
    //ARROW
        public class Arrow : INotifyPropertyChanged
        {
            private int length;
            private Color color;

            [Category("Arrow Settings")]
            [Description("Set arrow length. The maximum length is equal to the hypotenuse. Minimum - 8. Default - 50.")]
            [DisplayName("Arrow Length")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int Length
            {
                get => length;
                set
                {
                if (value < 8 || value > 4000)
                {
                    // Show the error message on top of the form
                    MessageBox.Show("Length must be between 8 and 4000.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                length = value;
                    OnPropertyChanged(nameof(Length));
                }
            }

            [Category("Arrow Settings")]
            [Description("Set arrow color.")]
            [DisplayName("Arrow Color")]
            [TypeConverter(typeof(ColorConverter))]
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


    public class Guide : INotifyPropertyChanged
        {
            private int topindent, bottomindent, leftindent, rightindent;
            private Color color;
            private bool lockind;
            private string type;

            [Category("1. General Settings")]
            [DisplayName("Guide Type")]
            [Description("Choose grid type. 3x3 and 4x4 divides the area into equal parts, custom - set arbitrary padding.")]
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
            [TypeConverter(typeof(ColorConverter))]
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
                if (value < 1 || value > 3840)
                {
                    // Show the error message on top of the form
                    MessageBox.Show("Indent must be between 1 and 3840.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }

                    topindent = value;
                    OnPropertyChanged(nameof(topIndent));
                }
            }

            [Category("2. Custom Type Settings")]
            [Description("Max size screenshot height/2. Minimum - 1. Default - 10.")]
            [DisplayName("Bottom Indent")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int bottomIndent
            {
                get => bottomindent;
                set
                {
                if (value < 1 || value > 3840)
                {
                    // Show the error message on top of the form
                    MessageBox.Show("Indent must be between 1 and 3840.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }

                    bottomindent = value;
                    OnPropertyChanged(nameof(bottomIndent));
                }
            }

            [Category("2. Custom Type Settings")]
            [Description("Max size screenshot height/2. Minimum - 1. Default - 10.")]
            [DisplayName("Left Indent")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int leftIndent
            {
                get => leftindent;
                set
                {
                    if (value < 1 || value > 3840)
                    {
                        // Show the error message on top of the form
                        MessageBox.Show("Indent must be between 1 and 3840.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Don't set the value if it's invalid
                    }
                    leftindent = value;
                    OnPropertyChanged(nameof(leftIndent));
                }
            }

            [Category("2. Custom Type Settings")]
            [Description("Max size screenshot height/2. Minimum - 1. Default - 10.")]
            [DisplayName("Right Indent")]
            [TypeConverter(typeof(Int32OnlyConverter))]
            public int rightIndent
            {
                get => rightindent;
                set
                {
                    if (value < 1 || value > 3840)
                    {
                        // Show the error message on top of the form
                        MessageBox.Show("Indent must be between 1 and 3840.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    public class Frame
    {
        private int framewidth;
        private int frameheight;
        private int strokewidth;
        private Color color;
        private string type;

        [Category("2. Fixed Frame Settings")]
        [Description("Frame width. Max - screenshot width, min - 16. Default 80.")]
        [DisplayName("Frame Width")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int frameWidth
        {
            get => framewidth;
            set
            {
                // Validation for FrameWidth
                if (value < 16 || value > 3840)
                {
                    MessageBox.Show("Frame width must be between 16 and 3840.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Don't set the value if it's invalid
                }
                framewidth = value;
                OnPropertyChanged(nameof(frameWidth));
            }
        }

        [Category("2. Fixed Frame Settings")]
        [Description("Frame height. Max - screenshot height, min - 16. Default 80.")]
        [DisplayName("Frame Height")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int frameHeight
        {
            get => frameheight;
            set
            {
                // Validation for FrameHeight
                if (value < 16 || value > 3840)
                {
                    MessageBox.Show("Frame height must be between 16 and 3840.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        [TypeConverter(typeof(ColorConverter))]
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
        [Description("Set number font size. Minimum - 8. Default - 26.")]
        [DisplayName("Number Font Size")]
        [TypeConverter(typeof(Int32OnlyConverter))]
        public int Size
        {
            get => size;
            set
            {
                if (value < 8 || value > 72)
                {
                    // Show the error message on top of the form
                    MessageBox.Show("Size must be between 8 and 72.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    size = 8;
                    return; // Don't set the value if it's invalid
                }

                size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        [Category("Numbers Settings")]
        [Description("Set numbers color.")]
        [DisplayName("Number Color")]
        [TypeConverter(typeof(ColorConverter))]
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
        [TypeConverter(typeof(ColorConverter))]
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
        [Description("Size 1 width. Max depends on your monitor resolution. Min - 300px.")]
        [DisplayName("1.1 Width")]
        [TypeConverter(typeof(ResWidthConverter))]
        public int res1Width
        {
            get => r1w;
            set
            {
                r1w = value;
                OnPropertyChanged(nameof(res1Width));
            }
        }

        [Category("Size 1")]
        [Description("Size 1 height. Max depends on your monitor resolution. Min - 200px.")]
        [DisplayName("1.2 Height")]
        [TypeConverter(typeof(ResHeightConverter))]
        public int res1Height
        {
            get => r1h;
            set
            {
                r1h = value;
                OnPropertyChanged(nameof(res1Height));
            }
        }

        [Category("Size 2")]
        [Description("Size 2 width. Max depends on your monitor resolution. Min - 300px.")]
        [DisplayName("2.1 Width")]
        [TypeConverter(typeof(ResWidthConverter))]
        public int res2Width
        {
            get => r2w;
            set
            {
                r2w = value;
                OnPropertyChanged(nameof(res2Width));
            }
        }

        [Category("Size 2")]
        [Description("Size 2 height. Max depends on your monitor resolution. Min - 200px.")]
        [DisplayName("2.2 Height")]
        [TypeConverter(typeof(ResHeightConverter))]
        public int res2Height
        {
            get => r2h;
            set
            {
                r2h = value;
                OnPropertyChanged(nameof(res2Height));
            }
        }

        [Category("Size 3")]
        [Description("Size 3 width. Max depends on your monitor resolution. Min - 300px.")]
        [DisplayName("3.1 Width")]
        [TypeConverter(typeof(ResWidthConverter))]
        public int res3Width
        {
            get => r3w;
            set
            {
                r3w = value;
                OnPropertyChanged(nameof(res3Width));
            }
        }

        [Category("Size 3")]
        [Description("Size 3 height. Max depends on your monitor resolution. Min - 200px.")]
        [DisplayName("3.2 Height")]
        [TypeConverter(typeof(ResHeightConverter))]
        public int res3Height
        {
            get => r3h;
            set
            {
                r3h = value;
                OnPropertyChanged(nameof(res3Height));
            }
        }

        [Category("Size 4")]
        [Description("Size 4 width. Max depends on your monitor resolution. Min - 300px.")]
        [DisplayName("4.1 Width")]
        [TypeConverter(typeof(ResWidthConverter))]
        public int res4Width
        {
            get => r4w;
            set
            {
                r4w = value;
                OnPropertyChanged(nameof(res4Width));
            }
        }

        [Category("Size 4")]
        [Description("Size 4 height. Max depends on your monitor resolution. Min - 200px.")]
        [DisplayName("4.2 Height")]
        [TypeConverter(typeof(ResHeightConverter))]
        public int res4Height
        {
            get => r4h;
            set
            {
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


    public class ResWidthConverter : Int16Converter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is int && Convert.ToInt32(value) <= FS2SettingsManager.virtScreenWidth && Convert.ToInt32(value) >= FS2SettingsManager.MIN_WIDTH)
            {
                return ((int)value).ToString();
            }
            else
            {
                value = "Invalid value! Max is " + FS2SettingsManager.virtScreenWidth.ToString() + "px, min - " + FS2SettingsManager.MIN_WIDTH.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    public class ResHeightConverter : Int16Converter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is int && Convert.ToInt32(value) <= FS2SettingsManager.virtScreenHeight && Convert.ToInt32(value) >= FS2SettingsManager.MIN_HEIGHT)
            {
                return ((int)value).ToString();
            }
            else
            {
                value = "Invalid value! Max is " + FS2SettingsManager.virtScreenHeight.ToString() + "px, min - " + FS2SettingsManager.MIN_HEIGHT.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }


    public class Int32OnlyConverter : TypeConverter
        {
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
                bool messageBoxShown = false;

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
                        MessageBox.Show(context?.Instance as IWin32Window, "Invalid integer format. Please enter a valid integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        messageBoxShown = true; // Set the flag to true after showing the message
                    }

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
