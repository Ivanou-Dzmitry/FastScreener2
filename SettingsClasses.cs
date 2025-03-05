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
                    UpdateIndentVisibility();
                    OnPropertyChanged(nameof(lockIndent));
                }
            }


            [Category("2. Custom Type Settings")]
            [Description("Max size screenshot height/2. Minimum - 1. Default - 10.")]
            [DisplayName("Top Indent")]
            [TypeConverter(typeof(Int32OnlyConverter))]
        [Browsable(true)]
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
        [Browsable(true)]
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
            [Browsable(true)]
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
        [Browsable(true)]
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


        private void UpdateIndentVisibility()
        {
            // Here we dynamically change the Browsable attribute based on lockIndent
            var typeDescriptor = TypeDescriptor.GetProperties(this);

            foreach (PropertyDescriptor property in typeDescriptor)
            {
                if (property.Name == nameof(topIndent) ||
                    property.Name == nameof(bottomIndent) ||
                    property.Name == nameof(leftIndent) ||
                    property.Name == nameof(rightIndent))
                {
                    if (lockIndent)
                    {
                        // Hide the indent properties if lockIndent is true
                        TypeDescriptor.AddAttributes(property, new BrowsableAttribute(false));
                    }
                    else
                    {
                        // Show the indent properties if lockIndent is false
                        TypeDescriptor.AddAttributes(property, new BrowsableAttribute(true));
                    }
                }
            }

            // To ensure PropertyGrid updates the display, you may want to refresh it
            // pgSettings.Refresh(); // This could be useful if you're using a PropertyGrid control
        }


        public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
