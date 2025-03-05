using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FastScreener2
{
    internal class FS2SettingsManager
    {
        public struct Line
        {
            public Point startPoint { get; set; }
            public Point endPoint { get; set; }
            public Color lineColor { get; set; }
            public float lineWidth { get; set; }

            public Line(Point start, Point end, Color color, float width)
            {
                startPoint = start;
                endPoint = end;
                lineColor = color;
                lineWidth = width;
            }
        }

        //screen sizes
        public static object[,] RES_DEFAULT = { { 600, 600, 600, 960 }, { 337, 600, 700, 600 } };
        public static object[,] RES_WORKED = new object[2, 4];

        public static int[] customGuide = new int[] { 0, 0, 0, 0 };

        public static int guidlineType, arrowType, arrowLenght, numberFontSize, frameWidth, frameHeight, frameType;

        //for guidlines
        public static bool drawGuides, drawArrows, saveToFile, drawNumber, drawFrame;

        public static bool lockIndent = false;

        //colors
        public static Color guideColor = Color.LightGray;
        public static Color arrowColor = Color.Aqua;
        public static Color numberColor = Color.Yellow;
        public static Color frameColor = Color.Gray;


        private static string settingsFilePath = "fs2_settings.xml";
        private static Dictionary<string, string> settings = new Dictionary<string, string>();

        // Load settings from XML (create file if missing)
        public static void Load()
        {
            if (!File.Exists(settingsFilePath))
            {
                CreateDefaultSettings();
            }

            XDocument doc = XDocument.Load(settingsFilePath);
            settings = doc.Root.Elements("Setting")
                              .ToDictionary(x => x.Attribute("Key").Value, x => x.Attribute("Value").Value);

            //load arrow
            arrowColor = ColorTranslator.FromHtml(settings["arrow_color"]); // Retrieve the value
            arrowLenght = int.Parse(settings["arrow_lenght"]);
            drawArrows = Convert.ToBoolean(settings["draw_arrows"]);
            arrowType = int.Parse(settings["arrow_type"]);

            //load guidlines
            guideColor = ColorTranslator.FromHtml(settings["guidlines_color"]);
            drawGuides = Convert.ToBoolean(settings["draw_guidlines"]);
            guidlineType = int.Parse(settings["guidline_type"]);
            customGuide[0] = int.Parse(settings["top_indent"]);
            customGuide[1] = int.Parse(settings["bottom_indent"]);
            customGuide[2] = int.Parse(settings["left_indent"]);
            customGuide[3] = int.Parse(settings["right_indent"]);

            lockIndent = Convert.ToBoolean(settings["lock_indent"]);

            //number
            numberColor = ColorTranslator.FromHtml(settings["number_color"]);
            drawNumber = Convert.ToBoolean(settings["draw_number"]);
            numberFontSize = int.Parse(settings["number_size"]);

            //frame
            frameColor = ColorTranslator.FromHtml(settings["frame_color"]);
            drawFrame = Convert.ToBoolean(settings["draw_frame"]);
            frameWidth = int.Parse(settings["frame_width"]);
            frameHeight = int.Parse(settings["frame_height"]);
            frameType = int.Parse(settings["frame_type"]);

            //file
            saveToFile = Convert.ToBoolean(settings["save_to_file"]);

            
        }

        // Save settings to XML
        public static void Save()
        {
            XDocument doc = new XDocument(new XElement("Settings",
                settings.Select(kv => new XElement("Setting",
                    new XAttribute("Key", kv.Key),
                    new XAttribute("Value", kv.Value)))));

            doc.Save(settingsFilePath);
        }

        // Update or add a setting value
        public static void SetSetting(string key, string value)
        {
            settings[key] = value;
        }

        // Get a setting value
        public static string GetSetting(string key, string defaultValue = "")
        {
            return settings.TryGetValue(key, out string value) ? value : defaultValue;
        }

        // Create default settings file if it does not exist
        private static void CreateDefaultSettings()
        {
            settings = new Dictionary<string, string>
        {
            { "guidlines_color", "#FF0000" },
            { "arrow_color", "#00FF00" },
            { "arrow_lenght", "50" },
            { "arrow_type", "1" },
            { "number_color", "#0000FF" },
            { "frame_color", "#FFFFFF" },
            { "guidline_type", "1" },
            { "draw_guidlines", "true" },
            { "draw_arrows", "true" },
            { "draw_number", "true" },
            { "number_size", "20" },
            { "draw_frame", "false" },
            { "save_to_file", "false" },
            { "frame_type", "1" },
            { "frame_width", "100" },
            { "frame_height", "100" },
            { "top_indent", "10" },
            { "bottom_indent", "10" },
            { "left_indent", "10" },
            { "right_indent", "10" },
            { "lock_indent", "true" }
        };

            Save(); // Create the XML file with default values
        }




    }
}
