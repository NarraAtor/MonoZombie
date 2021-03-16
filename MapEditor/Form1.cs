using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor
{
    public enum Tile
    {
        Grass,
        Wall,
        Gravel,
        Lava,
        Speed,
        ZombieSpawn
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Purpose: Creates a new map
        //Restrictions: Length and width must be integers between 10 and 30 inclusive
        //Params: sender for the button that was clicked, e for any arguments attached
        private void buttonCreateMap_Click(object sender, EventArgs e)
        {
            //Dimensions
            int width;
            int height;

            //Flags to check if each field is correct
            bool widthNaN = !int.TryParse(textBoxWidth.Text, out width);
            bool heightNaN = !int.TryParse(textBoxHeight.Text, out height);
            bool widthTooLow = false;
            bool widthTooHigh = false;
            bool heightTooLow = false;
            bool heightTooHigh = false;

            if (!widthNaN && width < 10)
            {
                widthTooLow = true;
            }
            if (!widthNaN && width > 30)
            {
                widthTooHigh = true;
            }
            if (!heightNaN && height < 10)
            {
                heightTooLow = true;
            }
            if (!heightNaN && height > 30)
            {
                heightTooHigh = true;
            }

            if (widthNaN || heightNaN || widthTooLow || widthTooHigh || heightTooLow || heightTooHigh)
            {//if any errors get flagged,
                string errors = "Errors:";//Lists all the errors
                if (widthNaN)
                {
                    errors += "\n - Width is not an integer";
                }
                if (widthTooLow)
                {
                    errors += "\n - Width too small. Minimum is 10";
                }
                if (widthTooHigh)
                {
                    errors += "\n - Width too high. Maximum is 30";
                }
                if (heightNaN)
                {
                    errors += "\n - Height is not an integer";
                }
                if (heightTooLow)
                {
                    errors += "\n - Height too small. Minimum is 10";
                }
                if (heightTooHigh)
                {
                    errors += "\n - Height too high. Maximum is 30";
                }
                MessageBox.Show(errors, "Error creating map", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;//displays the errors, and then returns
            }

            EditorForm ef = new EditorForm(width, height);//if everything is correct, 
            ef.ShowDialog();//creates a new level with width and height
        }

        //Purpose: Loads an existing map
        //Restrictions: Length and width must be integers between 10 and 30 inclusive
        //Params: sender for the button that was clicked, e for any arguments attached
        private void buttonLoadMap_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();//file loader
            ofd.Title = "Open a level file";
            ofd.Filter = "Level Files|*.level";
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                EditorForm ef = new EditorForm(ofd.FileName);//loads the existing file
                try
                {
                    ef.ShowDialog();
                }
                catch
                {//if anything goes wrong, Editor form closes and this error message displays
                    MessageBox.Show("Could not open up file. Please check the file for any errors.", 
                        "Error loading map", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
