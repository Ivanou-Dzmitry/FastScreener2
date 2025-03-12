using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FastScreener2
{
    internal class FS2SettingsManager
    {

        //screen sizes
        public static int[,] RES_DEFAULT = { { 650, 650, 650, 960 }, { 366, 650, 700, 600 } };
        public static int[,] resWorked = new int[2, 4];

        //Min size
        public const int MIN_WIDTH = 300, MIN_HEIGHT = 200;

        //all monitors
        public static int virtScreenWidth = 0, virtScreenHeight = 0;

        public static int[] customGuide = new int[] { 0, 0, 0, 0 };

        public static int guidlineType, arrowType, arrowLenght, numberFontSize, frameWidth, frameHeight, frameType, frameStrokeWidth;

        public static int startResW, startResH;

        //for guidlines
        public static bool drawGuides, drawArrows, saveToFile, drawNumber, drawFrame;

        public static bool lockIndent = false;

        //colors
        public static Color guideColor = Color.LightGray;
        public static Color arrowColor = Color.Aqua;
        public static Color numberColor = Color.Yellow;
        public static Color frameColor = Color.Gray;
        public static Color barColor;


        private static string settingsFilePath = "fs2_settings.xml";
        private static Dictionary<string, string> settings = new Dictionary<string, string>();

        public const int ARROW_SIZE = 6;

        public const int MIN_DRAWN_SIZE_FRAME = 3;

        public const string SUBPATH = "screenshots";

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
            try
            {
                arrowColor = ColorTranslator.FromHtml(settings["arrow_color"]); // Retrieve the value
                arrowLenght = int.Parse(settings["arrow_lenght"]);
                drawArrows = Convert.ToBoolean(settings["draw_arrows"]);
                arrowType = int.Parse(settings["arrow_type"]);
            }
            catch
            {
                arrowColor = Color.Cyan; // Retrieve the value
                arrowLenght = 50;
                drawArrows = false;
                arrowType = 1;

                EnsureSettingExists("arrow_color", "#00FFFF");
                EnsureSettingExists("arrow_lenght", "50");
                EnsureSettingExists("draw_arrows", "false");
                EnsureSettingExists("arrow_type", "1");
            }

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
            try
            {
                frameColor = ColorTranslator.FromHtml(settings["frame_color"]);
                drawFrame = Convert.ToBoolean(settings["draw_frame"]);
                frameWidth = int.Parse(settings["frame_width"]);
                frameHeight = int.Parse(settings["frame_height"]);
                frameType = int.Parse(settings["frame_type"]);
                frameStrokeWidth = int.Parse(settings["frame_stroke_width"]);
            }
            catch
            {
                frameColor = Color.OrangeRed;
                drawFrame = false;
                frameWidth = 80;
                frameHeight = 80;
                frameType = 1;
                frameStrokeWidth = 1;

                EnsureSettingExists("frame_color", "#00FFFF");
                EnsureSettingExists("draw_frame", "false");
                EnsureSettingExists("frame_width", "80");
                EnsureSettingExists("frame_height", "80");
                EnsureSettingExists("frame_type", "1");
                EnsureSettingExists("frame_stroke_width", "1");
            }


            //file
            saveToFile = Convert.ToBoolean(settings["save_to_file"]);

            //sizes
            for (int i = 1; i <= 4; i++)  // Loop from 1 to 4
            {
                string key = "res" + i;

                if (settings.TryGetValue(key, out string tempValueFromConfig))  // Get value from dictionary
                {
                    string[] tempStringArray = tempValueFromConfig.Split(','); // Fix here

                    try
                    {
                        resWorked[0, i - 1] = int.Parse(tempStringArray[0]); // Width
                    }
                    catch
                    {
                        resWorked[0, i - 1] = RES_DEFAULT[0, i - 1]; // Default width
                    }

                    try
                    {
                        resWorked[1, i - 1] = int.Parse(tempStringArray[1]); // Height
                    }
                    catch
                    {
                        resWorked[1, i - 1] = RES_DEFAULT[1, i - 1]; // Default height
                    }
                }
            }

            //res on close
            if (settings.TryGetValue("res_on_close", out string tempValueFromConfig2))
            {
                string[] tempStringArray = tempValueFromConfig2.Split(','); // Fix here

                // Set client size
                try
                {
                    startResW = Convert.ToInt32(tempStringArray[0]); //set width
                    startResH = Convert.ToInt32(tempStringArray[1]); //set height
                }
                catch
                {
                    startResW = Convert.ToInt32(resWorked[0, 0]);                    
                    startResH = Convert.ToInt32(resWorked[1, 0]);
                    EnsureSettingExists("res_on_close", "650,366");
                }
            }

            //number
            try
            {
                barColor = ColorTranslator.FromHtml(settings["bar_color"]);
            }
            catch
            {
                barColor = Color.DarkGray;                
                EnsureSettingExists("bar_color", "#313131");
            }
                

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
            { "lock_indent", "true" },
            { "res1", "650,366" },
            { "res2", "650,650" },
            { "res3", "650,700" },
            { "res4", "960,600" },
            { "res_on_close", "650,366" },
            { "bar_color", "#313131" },
            { "frame_stroke_width", "1" }            
        };

            Save(); // Create the XML file with default values
        }


        private static void EnsureSettingExists(string key, string defaultValue)
        {
            if (!settings.ContainsKey(key))
            {
                settings[key] = defaultValue; // Update in dictionary

                XDocument doc = XDocument.Load(settingsFilePath);
                XElement root = doc.Root;

                // Check if the key exists in XML before adding
                XElement existingSetting = root.Elements("Setting")
                                               .FirstOrDefault(x => x.Attribute("Key")?.Value == key);

                if (existingSetting == null)
                {
                    // Add new setting if missing
                    root.Add(new XElement("Setting",
                                new XAttribute("Key", key),
                                new XAttribute("Value", defaultValue)));
                }
                else
                {
                    // Update existing setting if incorrect
                    existingSetting.SetAttributeValue("Value", defaultValue);
                }

                // Save the updated settings file
                doc.Save(settingsFilePath);
            }
        }

    }
}
