using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
        public const int MIN_WIDTH = 550, MIN_HEIGHT = 240;

        //all monitors
        public static int virtScreenWidth = 0, virtScreenHeight = 0;

        public static int[] customGuide = new int[] { 0, 0, 0, 0 };

        public static int guidlineType, arrowType, arrowLenght, numberFontSize, frameWidth, frameHeight, frameType, frameStrokeWidth;

        public static int startResW, startResH, fileQuality, watermarkSize, watermarkPadding;

        public static float textSize;

        //for guidlines
        public static bool drawGuides, drawArrows, saveToFile, drawNumber, drawFrame, drawText, showInfoLabel, drawWatermark;

        public static Font textFont;
        public static string textFontFam = "";

        public static bool lockIndent = false;

        //colors
        public static Color guideColor = Color.LightGray;
        public static Color arrowColor = Color.Aqua;
        public static Color numberColor = Color.Yellow;
        public static Color frameColor = Color.Gray;
        public static Color textColor = Color.Gray;
        public static Color barColor;
        public static Color panelColor = Color.SlateGray;


        private static string settingsFilePath = "fs2_settings.xml";
        private static Dictionary<string, string> settings = new Dictionary<string, string>();

        public const int ARROW_SIZE = 6;

        public const int MIN_DRAWN_SIZE_FRAME = 3;

        public const string SUBPATH = "screenshots";

        public static int currentRes;

        public static string fileFormat;

        public static Image watermarkImage = null;
        public static string watermarkPath = "";
        public static string watermarkPosition = "";

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
            try
            {
                guideColor = ColorTranslator.FromHtml(settings["guidlines_color"]);
                drawGuides = Convert.ToBoolean(settings["draw_guidlines"]);
                guidlineType = int.Parse(settings["guidline_type"]);
                customGuide[0] = int.Parse(settings["top_indent"]);
                customGuide[1] = int.Parse(settings["bottom_indent"]);
                customGuide[2] = int.Parse(settings["left_indent"]);
                customGuide[3] = int.Parse(settings["right_indent"]);

                lockIndent = Convert.ToBoolean(settings["lock_indent"]);
            }
            catch
            {
                guideColor = Color.DarkGray;
                EnsureSettingExists("guidlines_color", "DarkGray");

                drawGuides = false;
                EnsureSettingExists("draw_guidlines", "false");

                guidlineType = 3;
                EnsureSettingExists("guidline_type", "3");

                customGuide[0] = 10;
                customGuide[1] = 10;
                customGuide[2] = 10;
                customGuide[3] = 10;
                EnsureSettingExists("top_indent", "10");
                EnsureSettingExists("bottom_indent", "10");
                EnsureSettingExists("left_indent", "10");
                EnsureSettingExists("right_indent", "10");

                lockIndent = false;
                EnsureSettingExists("lock_indent", "false");
            }   


            //load number
            try
            {
                numberColor = ColorTranslator.FromHtml(settings["number_color"]);
                drawNumber = Convert.ToBoolean(settings["draw_number"]);
                numberFontSize = int.Parse(settings["number_size"]);
            }
            catch
            {
                numberColor = Color.Orange;
                drawNumber = false;
                numberFontSize = 26;

                EnsureSettingExists("number_color", "#FFBB00");
                EnsureSettingExists("draw_number", "false");
                EnsureSettingExists("number_size", "26");
            }

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
            try
            {
                saveToFile = Convert.ToBoolean(settings["save_to_file"]);
            }
            catch
            { 
                saveToFile = false;
                EnsureSettingExists("save_to_file", "false");
            }


            //load sizes
            // Load sizes
            for (int i = 1; i <= 4; i++)  // Loop from 1 to 4
            {
                string key = "res" + i;

                // Ensure the setting exists with the default value if not already present
                string defaultValue = i switch
                {
                    1 => "650,366",
                    2 => "650,650",
                    3 => "650,650",
                    4 => "960,600",
                    _ => "650,366" // Default to "650,366" for any unknown index
                };

                EnsureSettingExists(key, defaultValue);  // Ensure the setting is present

                if (settings.TryGetValue(key, out string tempValueFromConfig))  // Get value from dictionary
                {
                    string[] tempStringArray = tempValueFromConfig.Split(',');

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

            //bar
            try
            {
                barColor = ColorTranslator.FromHtml(settings["bar_color"]);
            }
            catch
            {
                barColor = Color.DarkGray;                
                EnsureSettingExists("bar_color", "#313131");
            }

            //format
            try
            {
                fileFormat = (string)settings["picture_format"];
            }
            catch
            {
                fileFormat = "png";
                EnsureSettingExists("picture_format", "png");
            }


            //quality
            try
            {
                fileQuality = int.Parse(settings["picture_quality"]);
            }
            catch
            {
                fileQuality = 75;
                EnsureSettingExists("picture_quality", "75");
            }

            //text
            try
            {
                drawText = Convert.ToBoolean(settings["draw_text"]);
            }
            catch
            {
                drawText = false;
                EnsureSettingExists("draw_text", "false");
            }

            try
            {
                textSize = int.Parse(settings["text_size"]);
            }
            catch
            {
                textSize = 26;
                EnsureSettingExists("text_size", "26");
            }

            try
            {
                textColor = ColorTranslator.FromHtml(settings["text_color"]);
            }
            catch
            {
                textColor = Color.LightGray;
                EnsureSettingExists("text_color", "#D9D9D9");
            }

            try
            {
                textFontFam = (string)settings["text_font"];
            }
            catch
            {
                textFontFam = SystemFonts.DefaultFont.FontFamily.Name;
                EnsureSettingExists("text_font", SystemFonts.DefaultFont.FontFamily.Name);
            }

            try
            {
                showInfoLabel = Convert.ToBoolean(settings["show_info_label"]);
            }
            catch
            {
                showInfoLabel = false;
                EnsureSettingExists("show_info_label", "false");
            }

            //panel
            try
            {
                panelColor = ColorTranslator.FromHtml(settings["panel_color"]);
            }
            catch
            {
                panelColor = Color.SlateGray;
                EnsureSettingExists("panel_color", "#708090");
            }

            //watermark
            try
            {
                drawWatermark = Convert.ToBoolean(settings["draw_watermark"]);
            }
            catch
            {
                drawWatermark = false;
                EnsureSettingExists("draw_watermark", "false");
            }

            try
            {
                watermarkPath = (string)settings["watermark_path"];
            }
            catch
            {
                watermarkPath = "";
                EnsureSettingExists("watermark_path", "");
            }

            try
            {
                watermarkPosition = (string)settings["watermark_position"];
            }
            catch
            {
                watermarkPosition = "top-left";
                EnsureSettingExists("watermark_position", "top-left");
            }

            try
            {
                watermarkSize = int.Parse(settings["watermark_size"]);
            }
            catch
            {
                watermarkSize = 64;
                EnsureSettingExists("watermark_size", "64");
            }


            try
            {
                watermarkPadding = int.Parse(settings["watermark_padding"]);
            }
            catch
            {
                watermarkPadding = 10;
                EnsureSettingExists("watermark_padding", "10");
            }

            EnsureSingleTrueParam();

            SetCurResBasedOnResOnClose();
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
        public static void CreateDefaultSettings()
        {
            settings = new Dictionary<string, string>
        {
            { "guidlines_color", "#D3D3D3" },
            { "arrow_color", "#00FFFF" },
            { "arrow_lenght", "50" },
            { "arrow_type", "1" },
            { "number_color", "#FFA500" },
            { "frame_color", "#FFA500" },
            { "guidline_type", "3" },
            { "draw_guidlines", "false" },
            { "draw_arrows", "false" },
            { "draw_number", "false" },
            { "number_size", "26" },
            { "draw_frame", "false" },
            { "save_to_file", "false" },
            { "frame_type", "1" },
            { "frame_width", "80" },
            { "frame_height", "80" },
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
            { "frame_stroke_width", "1" },
            { "picture_format", "png" },
            { "picture_quality", "75" },
            { "text_size", "26" },
            { "text_color", "#FFA500" },
            { "draw_text", "false" },
            { "text_font", "" },
            { "show_info_label", "false" },
            { "panel_color", "#708090" },
            { "draw_watermark", "false" },
            { "watermark_position", "false" },
            { "watermark_path", "" },
            { "watermark_size", "64" },
            {"watermark_padding", "10" }
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


        public static void SetCurResBasedOnResOnClose()
        {
            // Get the value of res_on_close from settings
            if (settings.TryGetValue("res_on_close", out string resOnCloseValue))
            {
                // Loop through the resolutions (res1 to res4)
                for (int i = 1; i <= 4; i++)
                {
                    string key = "res" + i;

                    if (settings.TryGetValue(key, out string resValue))
                    {
                        // Check if the current resolution matches the res_on_close value
                        if (resValue == resOnCloseValue)
                        {
                            // If matched, set currentRes to the appropriate index (0-based)
                            currentRes = i - 1; // i-1 because currentRes should be 0 for res1, 1 for res2, etc.
                            return;  // Exit the loop since we've found the match
                        }
                    }
                }
            }

            // If no match found, set currentRes to 0 (default resolution)
            currentRes = 0;
        }

        private static void EnsureSingleTrueParam()
        {
            // Get the current state of the 4 booleans (assuming these are properties or variables)
            bool[] paramValues = new bool[] { drawArrows, drawNumber, drawText, drawFrame };

            // Count how many of the params are true
            int trueCount = paramValues.Count(p => p);

            // If more than 1 is true, reset all to false
            if (trueCount > 1)
            {
                drawArrows = false;
                drawNumber = false;
                drawText = false;
                drawFrame = false;

                SetSetting("draw_frame", "false");
                SetSetting("draw_number", "false");
                SetSetting("draw_arrows", "false");
                SetSetting("draw_text", "false");
            }
        }


    }
}
