using System;
using System.IO;
using System.Windows.Forms;

namespace IGHW_Editor
{
	public partial class Form1 : Form
	{
		//The main text file
		TextFile IghwTxt;

		public Form1()
		{
			InitializeComponent();		//Set up the Form, managed by Visual Studio
		}
		void OpenFile(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "IGHW text files (*.pkg)|*.pkg|All files (*.*)|*.*";		//Only allow pkg files, with the option for all files just in case anyone wants that
				openFileDialog.FilterIndex = 0;														//Start with the first index
				openFileDialog.FilterIndex = 2;														//We want 2 filters
				openFileDialog.RestoreDirectory = true;												//Basically remember what folder you were in last time

				if (openFileDialog.ShowDialog() == DialogResult.OK)			//If the user selects a file
				{
					this.Text = $"IGHW Text Editor - v0.01 - \"{Path.GetFileName(openFileDialog.FileName)}\"";	//Change the window title
					switch (Path.GetExtension(openFileDialog.FileName))
					{
						case ".pkg":
							IghwTxt = new TextFile(openFileDialog.FileName);														//Load the selected file as an lxb file

							lbText.Items.Clear();												//Remove all previously loaded items
							for(uint i = 0; i < IghwTxt.numberOfItems; i++)						//For every text item
							{
								lbText.Items.Add(IghwTxt.ReadString(IghwTxt.itemAddresses[i]));		//Read the text item and add it to the listbox
							}
							break;
						default:
							throw new NotImplementedException("The file format is not yet implemented. Now go or Tachyon will come back in the next Ratchet and Clank.");
					}
				}
			}
		}

		//Triggered when double clicking an item in lbText
		void ViewData(object sender, MouseEventArgs e)
		{
			//Lol no, this is a carry over from SSA XPEC Editor, I'll add it in once i know how to, but in the meantime it's disabled

			return;
			if (IghwTxt == null) return;										//If there is no file loaded, cancel.
			int index = lbText.IndexFromPoint(e.Location);					//Figure out what file was double clicked
			if (index != ListBox.NoMatches)									//If a file was confirmed to be loaded
			{
				ViewItemForm viewItem = new ViewItemForm(lbText, index);	//Create a form where you can view and edit text
				viewItem.Show();											//Show said form
			}
		}

		//Triggered when pressing "File > Save"
		private void SaveFile(object sender, EventArgs e)
		{
			//No, this is also a carry over from SSA XPEC Editor, it'll come later

			//IghwTxt.Save(lbText.Items.OfType<string>().ToArray());				//Just save lol
		}
	}
}
