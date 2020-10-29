using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            fileNamesList.Columns.Add("File name:", 1000, HorizontalAlignment.Left);
        }

        
        private void browseButton_Click(object sender, EventArgs e)
        {
            filePaths = new List<string>();
            fileNamesList.Items.Clear();
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    try
                    {
                        filePaths.Add(filePath);
                        fileNamesList.Items.Add(Path.GetFileName(filePath));
                    }
                    catch (Exception)
                    {
                        // Could not add the file path - probably related to Windows file system permissions.
                        MessageBox.Show("Cannot add the file: " + filePath.Substring(filePath.LastIndexOf('\\'))
                            + ". You may not have permission to read the file");
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
                        if (userInputBox.Text == "" || userInputBox.Text == null) throw new NothingInTheTextBoxException();
                        foreach (char character in userInputBox.Text)
                        {
                            if (character == '.') throw new UsedDotInTheTextBoxException();
                        }

                        //If the file already has that extension, it would be useless to change it
                        var newExtension = '.' + userInputBox.Text;
                        if (Path.GetExtension(filePath) != newExtension)
                        {
                            var fileName = Path.GetFileName(filePath);
                            var dotCount = fileName.Count(f => f == '.');

                            if (dotCount == 0 || dotCount == 1)
                            {
                                //Just changes changes the extension
                                File.Move(filePath, Path.ChangeExtension(filePath, newExtension));
                            }
                            else if (dotCount > 1)
                            {
                                //Modifies the extension AND the name to a better format
                                var fileNameSplit = fileName.Split('.');
                                string newName = fileNameSplit[0] + " - " + fileNameSplit[1] + newExtension;
                                File.Move(filePath, Path.GetFullPath(filePath).Replace(fileName, newName));
                            }
                        }
                    }
                    catch (NothingInTheTextBoxException)
                    {
                        //User just forgot to write down the new extension
                        MessageBox.Show("Some error occured when trying to change the extension\n" +
                            "It happened because nothing was written on the text box!");
                        return;
                    }
                    catch (UsedDotInTheTextBoxException)
                    {
                        //User used a dot in the extension, that is troublesome, he shouldn't do that
                        MessageBox.Show("Some error occured when trying to change the extension\n" +
                            "It happened because you wrote a dot in the text box!");
                        return;
                    }
                    catch (Exception)
                    {
                        //If the file is in being seeded in a torrent or something, it will stop any changes to it
                        MessageBox.Show("Some error occured when trying to change the extension\n" +
                            "It could be that the files are opened somewhere else");
                        return;
                    }
                }
                filePaths = new List<string>();
                fileNamesList.Items.Clear();
                MessageBox.Show("Done! The extentions have all been changed to: " + '.' + userInputBox.Text);
            }
        }

        private void userInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            //I've done this because it is natural for the user to press enter after writing something and expect some result
            if(e.KeyCode == Keys.Enter)
            {
                okButton_Click(sender, e);
            }
        }
    }
}
