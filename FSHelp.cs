using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastScreener2
{
    public partial class formFSHelp : Form
    {
        public formFSHelp()
        {
            InitializeComponent();

            FSUtils utils = new FSUtils();
            utils.AttachDragEvents(pnlFSHelpHead);

            LoadHelpText();
        }


        private void btnCloseHelp_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadHelpText()
        {
            // Get the current assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Specify the path of the embedded resource (namespace + filename)
            string resourceName = "FastScreener2.fs2_help.txt"; // Adjust namespace accordingly

            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    richTextBoxHelp.Text = $"Help text resource '{resourceName}' was not found.";
                    return;
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    // Read the content and set it to the RichTextBox
                    richTextBoxHelp.Text = reader.ReadToEnd();
                }
            }
        }

        private void formFSHelp_Shown(object sender, EventArgs e)
        {
            //need for drag from 1.5 to 1 scale monitor
            using (Graphics g = this.CreateGraphics())
            {
                float dpiScale = g.DpiX / 96f;

                this.ClientSize = new Size(
                    (int)(400 * dpiScale),
                    (int)(500 * dpiScale)
                );
            }
        }
    }
}
