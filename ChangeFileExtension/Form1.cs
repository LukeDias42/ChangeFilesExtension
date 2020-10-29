using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
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

        
        private void browseButton_Click(object sender, EventArgs e)
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

        private void okButton_Click(object sender, EventArgs e)
        {
            if (filePaths.Count > 0)
            {
                foreach (string filePath in filePaths)
                {
                    try
                    {
                        //Checks to see if there is a dot in the text box, it appears that Path.ChangeExtension doesn't work with too many dots?
                        foreach(char character in userInputBox.Text)
                        {
                            if (character == '.') throw new Exception();
                        }

                        //If the file already has that extension, it would be useless to change it
                        var newExtension = '.' + userInputBox.Text;
                        if (Path.GetExtension(filePath) != newExtension)
                        {
                            File.Move(filePath, Path.ChangeExtension(filePath, newExtension);
                        }

                    }
                    catch
                    {
                        MessageBox.Show("Some error occured when trying to change the extension\n" +
                            "Try not using any '.' characters in the new extension name.");
                    }
                    filePaths = new List<string>();
                }
            }
        }
    }
}
