using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TextEditor
{
    public partial class Form2 : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;                    //disables the close button on forms
        bool saved = false;                                             //is doc saved
        string prev = "";                                               //for checking saved status
        public Form2()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams                    //completely disables 'x' button on child forms
        {
            get
            {
                CreateParams myCp = base.CreateParams;                  //parameters for base class
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;  //my class will have no 'x' button
                return myCp;
            }
        }
        public string Get_TextEditor() { return textBox1.Text; }
        public void Set_TextEditor(string input) { textBox1.Text = input; prev = input; }
        public bool Get_saved() { return saved; }
        public void Set_saved(bool issaved) { saved = issaved; }
        public string Get_savedText() { return textBox2.Text; }
        public void Set_savedText(bool issaved) { if (issaved = true) { textBox2.Text = "  Saved."; } else { textBox2.Text = " Unsaved..."; } }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != prev) { saved = false; textBox2.Text = " Unsaved..."; } 
            else { saved = true; textBox2.Text = "  Saved."; }
            textBox3.Text = "Characters: " + textBox1.TextLength;       //checks/updates character count on each file
        }
    }
}
