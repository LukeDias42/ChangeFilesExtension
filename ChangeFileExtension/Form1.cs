using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChangeFileExtension
{
    public partial class fileExtensionChanger : Form
    {
        List<string> filePaths = new List<string>();
        public fileExtensionChanger()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog1.FileNames)
                {
                    try
                    {
                        filePaths.Add(filePath);
                    }
                    catch (SecurityException ex)
                    {
                        // The user lacks appropriate permissions to read files, discover paths, etc.
                        MessageBox.Show("Security error. Please contact your administrator for details.\n\n" +
                                        "Error message: " + ex.Message + "\n\n" +
                                        "Details (send to Support):\n\n" + ex.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        // Could not add the file path - probably related to Windows file system permissions.
                        MessageBox.Show("Cannot play the sound " + filePath.Substring(filePath.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filePaths.Count > 0)
            {
                foreach (string fileFullPath in filePaths)
                {
                    try
                    {
                        foreach(char character in textBox2.Text)
                        {
                            if (character == '.') throw new Exception();
                        }
                        if (Path.GetExtension(fileFullPath) != '.' + textBox2.Text)
                        {
                            File.Move(fileFullPath, Path.ChangeExtension(fileFullPath, '.' + textBox2.Text));
                        }

                    }
                    catch
                    {
                        MessageBox.Show("Some error occured when trying to change the extension\n" +
                            "Try not using any '.' characters in the new extension name.");
                    }
                }
            }
        }
    }
}
