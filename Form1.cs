using System.Drawing.Printing;
using System.IO;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        public int newFileCounter = 0;
        public Form1()
        {
            InitializeComponent();
            Text = "Text Editor";
        }

        public void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (newFileCounter == 0)
            {
                windowToolStripMenuItem.DropDownItems.Clear();      //clears placeholder item from drop down
            }
            string form2Name = "Untitled(" + newFileCounter + ")";  //formatting for name of file: Untitled(x)
            Form2 f = new Form2();                                  //creates new file
            f.Text = form2Name;
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.Show();

            newFileCounter++;                                       //counts how many new files have been created, used to differentiate between file names
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorClose();                                          //connects to external function so function can be repurposed elsewhere if needed
        }
        public void EditorClose()
        {
            Form2 active = (Form2)this.ActiveMdiChild;              
            if (MdiChildren.Length > 0)
            {
                if (!active.Get_saved())                            //if file has not been saved, move to next function
                {
                    CloseError();

                }
                else
                {
                    ActiveMdiChild.Close();                         //if file has been saved, close it
                }
            }
        }
        private void CloseError()
        {
            SystemSounds.Beep.Play();
            DialogResult result = MessageBox.Show("You are about to close and unsaved file. Would you like to save it?", "File Close Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Cancel)                      //if user chooses cancel, they'll be returned to their work
            {
                return;
            }
            else if (result == DialogResult.Yes)                    //if user chooses yes, will be directed to the save menu
            {
                Save();
                return;
            }
            else if(result == DialogResult.No)                      //if user chooses no, file will be closed
            {
                ActiveMdiChild.Close();
                return;
            }
            else
            {
                CloseError();                                       //for any other action, reroute the user to answer the question
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (this.ActiveMdiChild != null)                            //checking if active child exists
            {
                Form2 active = (Form2)this.ActiveMdiChild;
                SaveFileDialog fdlg = new SaveFileDialog();             //creating save file dialog
                fdlg.Title = "Save file as";
                fdlg.FileName = ActiveMdiChild.Text;
                fdlg.Filter = "Text File|*.txt|All Files|*.*";          //filtering for type of files
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    TextWriter txt = new StreamWriter(fdlg.FileName);   //writing file to stream
                    txt.Write(active.Get_TextEditor());
                    active.Text = fdlg.FileName;
                    txt.Flush();
                    txt.Close();
                    active.Set_savedText(true);                         //setting file status to 'saved'
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();                 //creating open dialog
            fdlg.Title = "Open file";
            fdlg.Filter = "Text File|*.txt|All Files|*.*";              //filtering for file type
            if (fdlg.ShowDialog() == DialogResult.OK)                   //if user selects file and hits 'ok'
            {
                Form2 form = new Form2();                               //create new form
                form.Show();
                string text = System.IO.File.ReadAllText(fdlg.FileName);//new form contents will be the same as the text from the opened file
                form.Set_TextEditor(text);
                form.Text = fdlg.FileName;
                form.MdiParent = this;

            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();                            //print dialog
            PrintDocument printDoc = new PrintDocument();                           //printable doc

            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);    //creating event handler for printing
            printDoc.DocumentName = ActiveMdiChild.Name;                            //set name of doc

            printDialog.Document = printDoc;
            printDialog.ShowHelp = true;

            if (printDialog.ShowDialog() == DialogResult.OK)                        //if user hits 'ok'
            {
                printDoc.Print();                                                   //print document
            }
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Form2 form = (Form2)ActiveMdiChild;                                     //active child will be the one printed

            String drawString = form.Get_TextEditor();
            Font drawFont = new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            PointF drawPoint = new PointF(15.0F, 15.0F);

            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);      //draws string of characters onto pdf
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();                                                     //quits application
        }
    }
}