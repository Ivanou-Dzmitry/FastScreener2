using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastScreener2
{
    public partial class formFS2Settings : Form
    {
        public formFS2Settings()
        {
            InitializeComponent();

            //select grid in list
            lboxSetCat.SetSelected(0, true);
            ArrowSettings();

            FSUtils utils = new FSUtils();
            utils.AttachDragEvents(pnlSetHeader);
            utils.AttachDragEvents(pnlSetBottom);
        }

        private void lboxSetCat_Click(object sender, EventArgs e)
        {
            int selCat = lboxSetCat.SelectedIndex;

            switch (selCat)
            {
                case 0:
                    ArrowSettings();
                    break;
                case 1:
                    BarSettings();
                    break;
                case 2:
                    FrameSettings();
                    break;
                case 3:
                    GuideSettings();
                    break;
                case 4:
                    NumberSettings();
                    break;
                case 5:
                    ResSettings();
                    break;
                default:
                    break;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            pgSettings.Refresh();
            FS2SettingsManager.Save();
            Close();
        }


        // --- Setting Property Grid ---
        //ARROW
        private Arrow arrowSettings;
        private void ArrowSettings()
        {
            arrowSettings = new Arrow
            {
                Color = FS2SettingsManager.arrowColor,
                Length = FS2SettingsManager.arrowLenght
            };

            pgSettings.SelectedObject = arrowSettings;
            pgSettings.PropertyValueChanged += PgSettings_PropertyValueChanged;
        }

        //GUIDES
        private Guide guideSettings;
        private void GuideSettings()
        {
            int tempTypeInt = FS2SettingsManager.guidlineType;
            string tempTypeStr = "";

            switch (tempTypeInt)
            {
                case 1:
                    tempTypeStr = "3x3";
                    break;
                case 2:
                    tempTypeStr = "4x4";
                    break;
                case 3:
                    tempTypeStr = "Custom";
                    break;
                default:
                    break;
            }

            guideSettings = new Guide
            {
                topIndent = FS2SettingsManager.customGuide[0],
                bottomIndent = FS2SettingsManager.customGuide[1],
                leftIndent = FS2SettingsManager.customGuide[2],
                rightIndent = FS2SettingsManager.customGuide[3],
                Color = FS2SettingsManager.guideColor,
                lockIndent = FS2SettingsManager.lockIndent,
                Type = tempTypeStr
            };



            pgSettings.SelectedObject = guideSettings;
            pgSettings.PropertyValueChanged += PgSettings_PropertyValueChanged;
        }

        //Frame
        private Frame frameSettings;
        private void FrameSettings()
        {

            int tempTypeInt = FS2SettingsManager.frameType;
            string tempTypeStr = "";

            switch (tempTypeInt)
            {
                case 1:
                    tempTypeStr = "Free";
                    break;
                case 2:
                    tempTypeStr = "Fixed";
                    break;
                default:
                    break;
            }

            frameSettings = new Frame
            {
                frameHeight = FS2SettingsManager.frameHeight,
                frameWidth = FS2SettingsManager.frameWidth,
                Color = FS2SettingsManager.frameColor,
                strokeWidth = FS2SettingsManager.frameStrokeWidth,
                Type = tempTypeStr
            };

            pgSettings.SelectedObject = frameSettings;
            pgSettings.PropertyValueChanged += PgSettings_PropertyValueChanged;
        }

        private Numbers numberSettings;

        private void NumberSettings()
        {
            numberSettings = new Numbers
            {
                Size = FS2SettingsManager.numberFontSize,
                Color = FS2SettingsManager.numberColor

            };

            pgSettings.SelectedObject = numberSettings;
            pgSettings.PropertyValueChanged += PgSettings_PropertyValueChanged;
        }

        private Resolutions resSettings;

        private void ResSettings()
        {
            resSettings = new Resolutions
            {
                res1Width = FS2SettingsManager.resWorked[0, 0],
                res1Height = FS2SettingsManager.resWorked[1, 0],

                res2Width = FS2SettingsManager.resWorked[0, 1],
                res2Height = FS2SettingsManager.resWorked[1, 1],

                res3Width = FS2SettingsManager.resWorked[0, 2],
                res3Height = FS2SettingsManager.resWorked[1, 2],

                res4Width = FS2SettingsManager.resWorked[0, 3],
                res4Height = FS2SettingsManager.resWorked[1, 3]

            };

            pgSettings.SelectedObject = resSettings;
            pgSettings.PropertyValueChanged += PgSettings_PropertyValueChanged;
        }

        private Bars barsSettings;

        private void BarSettings()
        {
            barsSettings = new Bars
            {
                Color = FS2SettingsManager.barColor
            };

            pgSettings.SelectedObject = barsSettings;
            pgSettings.PropertyValueChanged += PgSettings_PropertyValueChanged;
        }

        // --- Save Changes Automatically ---
        private void PgSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //ARROW
            if (e.ChangedItem.Label == "Arrow Length")
            {
                FS2SettingsManager.arrowLenght = arrowSettings.Length;
                FS2SettingsManager.SetSetting("arrow_lenght", arrowSettings.Length.ToString());
            }

            if (e.ChangedItem.Label == "Arrow Color")
            {
                FS2SettingsManager.arrowColor = arrowSettings.Color;
                FS2SettingsManager.SetSetting("arrow_color", ColorTranslator.ToHtml(arrowSettings.Color));
            }

            //GUIDES 
            if (e.ChangedItem.Label == "Top Indent")
            {
                FS2SettingsManager.customGuide[0] = guideSettings.topIndent;
                FS2SettingsManager.SetSetting("top_indent", guideSettings.topIndent.ToString());
            }

            if (e.ChangedItem.Label == "Bottom Indent")
            {
                FS2SettingsManager.customGuide[1] = guideSettings.bottomIndent;
                FS2SettingsManager.SetSetting("bottom_indent", guideSettings.bottomIndent.ToString());
            }

            if (e.ChangedItem.Label == "Left Indent")
            {
                FS2SettingsManager.customGuide[2] = guideSettings.leftIndent;
                FS2SettingsManager.SetSetting("left_indent", guideSettings.leftIndent.ToString());
            }

            if (e.ChangedItem.Label == "Right Indent")
            {
                FS2SettingsManager.customGuide[3] = guideSettings.rightIndent;
                FS2SettingsManager.SetSetting("right_indent", guideSettings.rightIndent.ToString());
            }


            if (e.ChangedItem.Label == "Guides Color")
            {
                FS2SettingsManager.guideColor = guideSettings.Color;
                FS2SettingsManager.SetSetting("guidlines_color", ColorTranslator.ToHtml(guideSettings.Color));
            }

            if (e.ChangedItem.Label == "Lock Indent")
            {
                FS2SettingsManager.lockIndent = guideSettings.lockIndent;
                FS2SettingsManager.SetSetting("lock_indent", guideSettings.lockIndent.ToString().ToLower());
            }

            if (e.ChangedItem.Label == "Guide Type")
            {
                string tempTypeStr = guideSettings.Type;
                int tempTypeInt = 0;

                switch (tempTypeStr)
                {
                    case "3x3":
                        tempTypeInt = 1;
                        break;
                    case "4x4":
                        tempTypeInt = 2;
                        break;
                    case "Custom":
                        tempTypeInt = 3;
                        break;
                    default:
                        break;
                }

                FS2SettingsManager.guidlineType = tempTypeInt;
                FS2SettingsManager.SetSetting("guidline_type", tempTypeInt.ToString());
            }

            //FRAME
            if (e.ChangedItem.Label == "Frame Color")
            {
                FS2SettingsManager.frameColor = frameSettings.Color;
                FS2SettingsManager.SetSetting("frame_color", ColorTranslator.ToHtml(frameSettings.Color));
            }


            if (e.ChangedItem.Label == "Frame Width")
            {
                FS2SettingsManager.frameWidth = frameSettings.frameWidth;
                FS2SettingsManager.SetSetting("frame_width", frameSettings.frameWidth.ToString());
            }

            if (e.ChangedItem.Label == "Frame Height")
            {
                FS2SettingsManager.frameHeight = frameSettings.frameHeight;
                FS2SettingsManager.SetSetting("frame_height", frameSettings.frameHeight.ToString());
            }

            if (e.ChangedItem.Label == "Frame Stroke Width")
            {
                FS2SettingsManager.frameStrokeWidth = frameSettings.strokeWidth;
                FS2SettingsManager.SetSetting("frame_stroke_width", frameSettings.strokeWidth.ToString());
            }


            if (e.ChangedItem.Label == "Frame Type")
            {
                string tempTypeStr = frameSettings.Type;
                int tempTypeInt = 0;

                switch (tempTypeStr)
                {
                    case "Free":
                        tempTypeInt = 1;
                        break;
                    case "Fixed":
                        tempTypeInt = 2;
                        break;
                    default:
                        break;
                }

                FS2SettingsManager.frameType = tempTypeInt;
                FS2SettingsManager.SetSetting("frame_type", tempTypeInt.ToString());
            }


            //NUMBER
            if (e.ChangedItem.Label == "Number Font Size")
            {
                FS2SettingsManager.numberFontSize = numberSettings.Size;
                FS2SettingsManager.SetSetting("number_size", numberSettings.Size.ToString());
            }

            if (e.ChangedItem.Label == "Number Color")
            {
                FS2SettingsManager.numberColor = numberSettings.Color;
                FS2SettingsManager.SetSetting("number_color", ColorTranslator.ToHtml(numberSettings.Color));
            }

            //BAR
            if (e.ChangedItem.Label == "Bar Color")
            {
                FS2SettingsManager.barColor = barsSettings.Color;
                FS2SettingsManager.SetSetting("bar_color", ColorTranslator.ToHtml(barsSettings.Color));
            }

            // Update Width for res1
            if (e.ChangedItem.Label == "1.1 Width")
            {
                FS2SettingsManager.resWorked[0, 0] = resSettings.res1Width;
                UpdateResolutionSetting("res1", "Width", resSettings.res1Width);
            }

            // Update Height for res1
            if (e.ChangedItem.Label == "1.2 Height")
            {
                FS2SettingsManager.resWorked[1, 0] = resSettings.res1Height;
                UpdateResolutionSetting("res1", "Height", resSettings.res1Height);
            }

            // res2
            if (e.ChangedItem.Label == "2.1 Width")
            {
                FS2SettingsManager.resWorked[0, 1] = resSettings.res2Width;
                UpdateResolutionSetting("res2", "Width", resSettings.res2Width);
            }

            if (e.ChangedItem.Label == "2.2 Height")
            {
                FS2SettingsManager.resWorked[1, 1] = resSettings.res2Height;
                UpdateResolutionSetting("res2", "Height", resSettings.res2Height);
            }

            // res3
            if (e.ChangedItem.Label == "3.1 Width")
            {
                FS2SettingsManager.resWorked[0, 2] = resSettings.res3Width;
                UpdateResolutionSetting("res3", "Width", resSettings.res3Width);
            }

            if (e.ChangedItem.Label == "3.2 Height")
            {
                FS2SettingsManager.resWorked[1, 2] = resSettings.res3Height;
                UpdateResolutionSetting("res3", "Height", resSettings.res3Height);
            }


            // res4
            if (e.ChangedItem.Label == "4.1 Width")
            {
                FS2SettingsManager.resWorked[0, 3] = resSettings.res4Width;
                UpdateResolutionSetting("res4", "Width", resSettings.res4Width);
            }

            if (e.ChangedItem.Label == "4.2 Height")
            {
                FS2SettingsManager.resWorked[1, 3] = resSettings.res4Height;
                UpdateResolutionSetting("res4", "Height", resSettings.res4Height);
            }



            FS2SettingsManager.Save();
        }


        public void UpdateResolutionSetting(string resKey, string changedLabel, int newValue)
        {
            // Retrieve the current resolution setting from FS2SettingsManager
            string currentRes = FS2SettingsManager.GetSetting(resKey);
            string tempRes = "";

            if (!string.IsNullOrEmpty(currentRes))
            {
                // Split the current resolution into width and height
                string[] resParts = currentRes.Split(',');

                if (resParts.Length == 2)
                {
                    int currentWidth = int.Parse(resParts[0]);
                    int currentHeight = int.Parse(resParts[1]);

                    // If width is being changed
                    if (changedLabel == "Width")
                    {
                        tempRes = newValue.ToString() + "," + currentHeight.ToString();
                    }
                    // If height is being changed
                    else if (changedLabel == "Height")
                    {
                        tempRes = currentWidth.ToString() + "," + newValue.ToString();
                    }
                }
            }
            else
            {
                // If no previous value exists, create a default resolution (for the first time setup)
                if (changedLabel == "Width")
                {
                    tempRes = newValue.ToString() + ",650"; // Default height
                }
                else if (changedLabel == "Height")
                {
                    tempRes = "650," + newValue.ToString(); // Default width
                }
            }

            // Save the updated resolution back into the settings
            FS2SettingsManager.SetSetting(resKey, tempRes);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to reset settings to default?",
                "Confirm Reset",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.OK)
            {
                FS2SettingsManager.CreateDefaultSettings();
                FS2SettingsManager.Load();
                FS2MainForm.isReseted = true; //call reset text
                Close();                
            }

        }
    }
}
