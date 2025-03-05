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
    public partial class FS2SettingsForm : Form
    {
        public FS2SettingsForm()
        {
            InitializeComponent();

            //select grid in list
            lboxSetCat.SetSelected(0, true);
            ArrowSettings();
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
                    FrameSettings();
                    break;
                case 2:
                    GuideSettings();
                    break;
                default:
                    break;
            }

            labelSetDebug.Text = selCat.ToString(); 
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
                Type = tempTypeStr
            };

            pgSettings.SelectedObject = frameSettings;
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
            if(e.ChangedItem.Label == "Top Indent")
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
                FS2SettingsManager.arrowColor = frameSettings.Color;
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


            FS2SettingsManager.Save();
        }



    }
}
