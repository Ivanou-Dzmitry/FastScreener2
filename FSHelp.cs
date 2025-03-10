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
    public partial class formFSHelp : Form
    {
        public formFSHelp()
        {
            InitializeComponent();

            FSUtils utils = new FSUtils();
            utils.AttachDragEvents(pnlFSHelpHead);
        }


        private void btnCloseHelp_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
