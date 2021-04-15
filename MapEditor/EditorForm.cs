using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MapEditor
{
    //Ken Adachi-Bartholomay
    //Purpose: Displays the editor form used to make/edit map files
    //Restrictions: Update height and width restrictions as necessary, update enum names as necessary
    public partial class EditorForm : Form
    {
        //fields
        private int width;
        private int height;
        private bool pendingChanges;//to notify the user in case they do something without saving
        private PictureBox[,] pictureBoxes;//so we can refer to each button
        private StreamWriter writer;
        private StreamReader reader;

        //Purpose: Creates a new map
        //Restrictions: Length and width must be integers between 20 and 50 inclusive
        //Params: width for pixels wide, height for pixels tall
        public EditorForm(int width, int height)
        {
            this.width = width;
            this.height = height;
            pendingChanges = false;

            InitializeComponent();

            pictureBoxes = new PictureBox[width, height];
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    pictureBoxes[w, h] = new PictureBox();//creates a new picturebox
                    pictureBoxes[w, h].BackColor = Color.FromArgb(-16744448);
                    pictureBoxes[w, h].Size = new Size(500 / height, 500 / height);//scales the size according to height
                    pictureBoxes[w, h].Location = new Point(w * (500 / height) + 5, h * (500 / height) + 20);//scales location according to height
                    pictureBoxes[w, h].MouseDown += ChangeBackgroundColor;//assign the click event to the picturebox
                    pictureBoxes[w, h].MouseEnter += KeepColoring;//assign the drag event to the picturebox
                    groupBoxMap.Controls.Add(pictureBoxes[w, h]);//add it to the groupbox

                }
            }
            //adjust the group box and and client size according to the number of boxes wide
            groupBoxMap.Size = new Size(width * (500 / height) + 10, 430);
            ClientSize = new Size(130 + groupBoxMap.Width, 461);
        }

        //Purpose: Loads a map from a file
        //Restrictions: if file is not formatted properly, will not load
        //Params: file for level loaded
        public EditorForm(string file)
        {
            InitializeComponent();
            if (!ReadFile(file))//if the file could not be loaded, and an existing map is not already being displayed
            {
                Close();//close the window
            }
        }

        //Purpose: Reads the map from a file
        //Restrictions: if file is not formatted properly, will not load
        //Params: file for level loaded
        private bool ReadFile(string file)
        {
            try
            {
                reader = new StreamReader($"{file}");//open file stream
                string[] widthAndHeight = reader.ReadLine().Split('|');

                //Flags to check if everything is allowed
                bool widthNaN = !int.TryParse(widthAndHeight[0], out width);
                bool heightNaN = !int.TryParse(widthAndHeight[1], out height);
                bool widthTooLow = false;
                bool widthTooHigh = false;
                bool heightTooLow = false;
                bool heightTooHigh = false;

                if (!widthNaN && width < 20)
                {
                    widthTooLow = true;
                }
                if (!widthNaN && width > 50)
                {
                    widthTooHigh = true;
                }
                if (!heightNaN && height < 20)
                {
                    heightTooLow = true;
                }
                if (!heightNaN && height > 50)
                {
                    heightTooHigh = true;
                }

                if (widthNaN || heightNaN || widthTooLow || widthTooHigh || heightTooLow || heightTooHigh)//if any errors get flagged,
                {
                    string errors = "Errors:";
                    if (widthNaN)
                    {
                        errors += "\n - Width is not an integer";
                    }
                    if (widthTooLow)
                    {
                        errors += "\n - Width too small. Minimum is 20";
                    }
                    if (widthTooHigh)
                    {
                        errors += "\n - Width too high. Maximum is 50";
                    }
                    if (heightNaN)
                    {
                        errors += "\n - Height is not an integer";
                    }
                    if (heightTooLow)
                    {
                        errors += "\n - Height too small. Minimum is 20";
                    }
                    if (heightTooHigh)
                    {
                        errors += "\n - Height too high. Maximum is 50";
                    }
                    MessageBox.Show(errors, "Error loading map", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;//lists the errors, and then returns false to show that the file could not be loaded
                }
                if (pictureBoxes != null)//if there is an existing picture
                {
                    for (int w = 0; w < pictureBoxes.GetLength(0); w++)
                    {
                        for (int h = 0; h < pictureBoxes.GetLength(1); h++)
                        {
                            pictureBoxes[w, h].Dispose();//deletes each one 
                        }
                    }
                }
                pictureBoxes = new PictureBox[width, height];//then creates a new array with corresponding width and height
                for (int w = 0; w < width; w++)
                {
                    for (int h = 0; h < height; h++)
                    {
                        pictureBoxes[w, h] = new PictureBox();//creates a new picturebox
                        switch (reader.ReadLine())
                        {
                            case "Grass":
                                pictureBoxes[w, h].BackColor = Color.FromArgb(-16744448);
                                break;

                            case "Wall":
                                pictureBoxes[w, h].BackColor = Color.FromArgb(-4144960);
                                break;

                            case "Gravel":
                                pictureBoxes[w, h].BackColor = Color.FromArgb(-3308225);
                                break;

                            case "Lava":
                                pictureBoxes[w, h].BackColor = Color.FromArgb(-4194304);
                                break;

                            case "Speed":
                                pictureBoxes[w, h].BackColor = Color.FromArgb(-5383962);
                                break;

                            case "ZombieSpawn":
                                pictureBoxes[w, h].BackColor = Color.FromArgb(-16777216);
                                break;
                        }
                        pictureBoxes[w, h].Size = new Size(410 / height, 410 / height);//scales size according to height
                        pictureBoxes[w, h].Location = new Point(w * (410 / height) + 5, h * (410 / height) + 20);//scales location according to height
                        pictureBoxes[w, h].MouseDown += ChangeBackgroundColor;//assigns the click event
                        pictureBoxes[w, h].MouseEnter += KeepColoring;//assign the drag event
                        groupBoxMap.Controls.Add(pictureBoxes[w, h]);//adds the picturebox
                    }
                }
                reader.Close();

                groupBoxMap.Size = new Size(width * (410 / height) + 10, 430);//adjusts the size of the 
                ClientSize = new Size(130 + groupBoxMap.Width, 461);//groupbox and client respectively

                MessageBox.Show("File loaded successfully", "File loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);//confirmation message
                pendingChanges = false;//no pending changes
            }
            catch (NullReferenceException err)//in case it is unable to read width and height
            {
                MessageBox.Show(err.Message + "\nUnable to read width and height.", "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception err)//any other errors
            {
                MessageBox.Show(err.Message, "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (writer != null)//if stream does not close, closes it
                {
                    writer.Close();
                }
            }
            this.Text = $"Level Editor - {file.Substring(file.LastIndexOf('\\') + 1)}";//changes name of window
            return true;//returns true to notify that the map was successfully loaded
        }

        //Purpose: Changes the color of the selected color
        //Restrictions: N/A
        //Params: sender for button clicked, e for any arguments attached
        private void buttonColorChange_Click(object sender, EventArgs e)
        {
            Button newColor = (Button)sender;//casts to a button
            buttonCurrentTile.BackColor = newColor.BackColor;//changes selected color to the color of the button selected
        }

        //Purpose: Changes the color of the pixel
        //Restrictions: N/A
        //Params: sender for button clicked, e for any arguments attached
        private void ChangeBackgroundColor(object sender, EventArgs e)
        {
            PictureBox tile = (PictureBox)sender;//casts to a picturebox
            tile.Capture = false;
            tile.BackColor = buttonCurrentTile.BackColor;//changes tile color to selected color

            pendingChanges = true;//notifys user that unsaved changes have been made
            if (pendingChanges && !Text.EndsWith(" *"))
            {
                Text += " *";
            }
        }

        private void KeepColoring(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                ChangeBackgroundColor(sender, e);
            }
        }

        //Purpose: saves the map to a file
        //Restrictions: N/A
        //Params: sender for button clicked, e for any arguments attached
        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save a level file";
            sfd.Filter = "Level Files|*.level";
            sfd.FileName = "untitled.level";//default name

            DialogResult result = sfd.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    writer = new StreamWriter($"{sfd.FileName}");
                    writer.WriteLine($"{width}|{height}");//writes down width and height
                    for (int w = 0; w < width; w++)
                    {
                        for (int h = 0; h < height; h++)
                        {
                            switch(pictureBoxes[w, h].BackColor.ToArgb())//records state of every tile
                            {
                                case -16744448:
                                    writer.WriteLine($"{Tile.Grass}");
                                    break;

                                case -4144960:
                                    writer.WriteLine($"{Tile.Wall}");
                                    break;

                                case -3308225:
                                    writer.WriteLine($"{Tile.Gravel}");
                                    break;

                                case -4194304:
                                    writer.WriteLine($"{Tile.Lava}");
                                    break;

                                case -5383962:
                                    writer.WriteLine($"{Tile.Speed}");
                                    break;

                                case -16777216:
                                    writer.WriteLine($"{Tile.ZombieSpawn}");
                                    break;

                            }
                        }
                    }
                    writer.Close();

                    MessageBox.Show("File saved successfully", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);//confirmation
                    this.Text = $"Level Editor - {sfd.FileName.Substring(sfd.FileName.LastIndexOf('\\') + 1)}";//change window title
                    pendingChanges = false;//no more unsaved changes
                    if (!pendingChanges && Text.EndsWith(" *"))
                    {
                        Text = Text.Substring(0, Text.LastIndexOf(" *"));
                    }
                }
                catch (Exception err)//in case any errors occur
                {
                    MessageBox.Show(err.Message, "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally//if stream is still open, closes it
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
        }

        //Purpose: Loads a file
        //Restrictions: N/A
        //Params: sender for button clicked, e for any arguments attached
        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            if (pendingChanges)//in case there are any unsaved changes
            {
                DialogResult unsaved = MessageBox.Show("There are unsaved changes. Are you sure you want to proceed?",
                    "Unsaved changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (unsaved == DialogResult.No)//if no, closes and does not proceed
                {
                    return;
                }
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a level file";
            ofd.Filter = "Level Files|*.level";
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                ReadFile(ofd.FileName);//runs readfile, which opens the selected map
            }
        }

        //Purpose: Makes sure the user does not close without forgetting to save
        //Restrictions: N/A
        //Params: sender for button clicked, e for any arguments attached
        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pendingChanges)
            {
                DialogResult unsaved = MessageBox.Show("There are unsaved changes. Are you sure you want to quit?",
                    "Unsaved changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (unsaved == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
