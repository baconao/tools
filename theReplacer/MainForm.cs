using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace theReplacer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            richTextBoxResults.Clear();
            buttonExecute.Enabled = false;
            labelFileCount.Text = "";

            ArrayList list = FileUtils.GetFileList(textBoxPath.Text, textBoxFilter.Text);
            foreach (string item in list)
            {
                richTextBoxResults.AppendText(item + "\n");
            }

            labelFileCount.Text = list.Count.ToString() + " files found";

            if (richTextBoxResults.TextLength > 0)
            {
                buttonExecute.Enabled = true;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBoxWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            richTextBoxResults.WordWrap = checkBoxWordWrap.Checked;
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {

        }

        private void buttonBrowsePath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBoxPath.Text;
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.ShowDialog();
            textBoxPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void buttonBrowseParameters_Click(object sender, EventArgs e)
        {
            string paramFilename =textBoxParameters.Text;
            
            if (String.IsNullOrEmpty(paramFilename)==false)
            {
                if (File.Exists(paramFilename))
                {
                    openFileDialog1.InitialDirectory = Path.GetDirectoryName(paramFilename);
                }                
            }

            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "XML Files |*.xml";
            openFileDialog1.FileName = "";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.ShowDialog();
            textBoxParameters.Text = openFileDialog1.FileName;
           
            
        }
    }
}
