using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO.Pipes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using IDE_GCrazy;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Antlr4.Runtime.Tree;

namespace IDE_GCrazy
{
    public partial class Form1 : Form
    {
        string type = "ANTLR";
        string pathtosave;
        string test;
        bool stop = false;
        bool onetime = false;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        SaveFileDialog savefiledlg = new SaveFileDialog();
        List<int> indextoremove = new List<int>();
        List<int> lenghttoremove = new List<int>();
        List<int> ntoadd = new List<int>();
        List<AnaliserList> dataList = new List<AnaliserList>();
        List<int> nreaisindex = new List<int>();
        List<int> nreaislenght = new List<int>();
        public Form1()
        {
            InitializeComponent();
            this.txtTest.TextChanged += this.txtTest_TextChanged;
            savefiledlg.Filter = "C Files (*.c)|*.c";
            savefiledlg.DefaultExt = "c";
            savefiledlg.AddExtension = true;
            savefiledlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            savefiledlg.RestoreDirectory = true;
            savefiledlg.Title = "Save File";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Open C file";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Filter = "C Files (*.c)|*.c";
            openFileDialog1.DefaultExt = "c";
            openFileDialog1.AddExtension = true;
        }
        string patternreserve = @"\b(string|define|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extem|return|union|const|float|short|unsigned|continue|for|signed|void|default|goto|sizeof|volatile|do|if|static|while)\b";
        string patternreais = @"^[-+]?\d+(\.\d+)?([eE][-+]?\d+)?\b";
        string patterninteiro = @"\b\d+\b";
        string Identificador(string input)
        {
            string result = "";
            string pattern = @"\b[a-zA-Z][a-zA-Z0-9]*\b";
            RegexOptions options = RegexOptions.Multiline;

            foreach (Match m in Regex.Matches(input, pattern, options))
            {
                if (!Regex.Match(m.Value, patternreserve).Success)
                {
                    dataList.Add(new AnaliserList("Identificador",m.Index,m.Value));
                    result = result + "\r\nIdentificador:" + ("'{0}' found at index {1}.", m.Value, m.Index);
                }
            }
            return result;
        }

        string NInteiro(string input)
        {
            int listindex=0;
            string result = "";
            int nrealcomplet = 0;
            RegexOptions options = RegexOptions.Multiline;

            foreach (Match m in Regex.Matches(input, patterninteiro, options))
            {
                if(nrealcomplet ==1)
                if ((nreaislenght[listindex] + nreaisindex[listindex]) < (m.Index + m.Length))
                {
                    nrealcomplet = 0;
                    listindex++;
                }
                if (nreaisindex.Contains(m.Index)|| nrealcomplet>=1)
                {
                    if(nrealcomplet<1)
                    {
                        nrealcomplet++;
                    }
                    else
                    {
                        if ((nreaislenght[listindex] + nreaisindex[listindex]) == (m.Index + m.Length))
                        {
                            nrealcomplet = 0;
                            listindex++;
                        }
                    }
                }
                else
                {
                    try
                    { 
                        if ((nreaislenght[listindex] + nreaisindex[listindex]) != (m.Index + m.Length))
                        {
                            dataList.Add(new AnaliserList("NInteiro", m.Index, m.Value));
                            result = result + "\r\nNInteiro:" + ("'{0}' found at index {1}.", m.Value, m.Index);
                        }
                        else
                            listindex++;
                    }
                    catch 
                    {
                        dataList.Add(new AnaliserList("NInteiro", m.Index, m.Value));
                        result = result + "\r\nNInteiro:" + ("'{0}' found at index {1}.", m.Value, m.Index);
                    }
                }
            }
            return result;
        }

        string NReais(string input)
        {
            string result = "";
            nreaisindex.Clear();
            nreaislenght.Clear();
            RegexOptions options = RegexOptions.Multiline;

            foreach (Match m in Regex.Matches(input, patternreais, options))
            {
                if(m.Value.Split('e').Length ==2 || m.Value.Split('E').Length == 2|| m.Value.Split('.').Length == 2)
                {
                    nreaisindex.Add(m.Index);
                    nreaislenght.Add(m.Length);
                    dataList.Add(new AnaliserList("float", m.Index, m.Value));
                    result = result + "\r\nNReais:" + ("'{0}' found at index {1}.", m.Value, m.Index);
                }
            }
            return result;
        }

        string Aritemetico(string input)
        {
            string result = "";
            string pattern = @"[\+\-\*\/]";
            RegexOptions options = RegexOptions.Multiline;

            foreach (Match m in Regex.Matches(input, pattern, options))
            {
                dataList.Add(new AnaliserList("Aritemetico", m.Index, m.Value));
                result = result + "\r\nAritemetico:" + ("'{0}' found at index {1}.", m.Value, m.Index);
            }
            return result;
        }

        string Relacionais(string input)
        {
            string result = "";
            string pattern = @"(>=|<=|==|!=|=|>|<|&&|\|\|)";
            RegexOptions options = RegexOptions.Multiline;

            foreach (Match m in Regex.Matches(input, pattern, options))
            {
                dataList.Add(new AnaliserList("Relacionais", m.Index, m.Value));
                result = result + "\r\nRelacionais:" + ("'{0}' found at index {1}.", m.Value, m.Index);
            }
            return result;
        }

        string Reservadas(string input)
        {
            string result = "";
            
            RegexOptions options = RegexOptions.Multiline;

            foreach (Match m in Regex.Matches(input, patternreserve, options))
            {
                dataList.Add(new AnaliserList("Reservadas", m.Index, m.Value));
                result = result + "\r\nReservadas:" + ("'{0}' found at index {1}.", m.Value, m.Index);
            }
            return result;
        }

        string Comments(string imput)
        {
            string pattern = @"\/\*(\*(?!\/)|[^*])*\*\/";
            RegexOptions options = RegexOptions.Multiline;
            foreach (Match m in Regex.Matches(imput, pattern, options))
            {
                indextoremove.Add(m.Index);
                lenghttoremove.Add(m.Value.Count());
                ntoadd.Add(m.Value.Split('\n').Length-1);
            }
            for (int i = indextoremove.Count()-1; i>=0;i--)
            {
                imput = imput.Remove(indextoremove[i], lenghttoremove[i]);
                for (int y = 0; y < ntoadd[i]; y++)
                    imput = imput.Insert(indextoremove[i], "\r\n");
            }
            ntoadd.Clear();
            indextoremove.Clear();
            lenghttoremove.Clear();
            pattern = @"\/\/[^\r\n]*";
            foreach (Match m in Regex.Matches(imput, pattern, options))
            {
                indextoremove.Add(m.Index);
                lenghttoremove.Add(m.Value.Count());
            }
            for (int i = indextoremove.Count() - 1; i >= 0; i--)
            {
                imput = imput.Remove(indextoremove[i], lenghttoremove[i]);
            }
            indextoremove.Clear();
            lenghttoremove.Clear();
            return imput;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtTest.Text = "";
                txtTest.Enabled = false;
                tslblStatus.Text = "Loading File Content...";
                tspgbStatus.Visible = true;
                testToolStripMenuItem.Enabled = false;
                autoToolStripMenuItem.Enabled = false;
                fileToolStripMenuItem.Enabled = false; ;
                if (autoToolStripMenuItem.Checked==true)
                this.txtTest.TextChanged -= this.txtTest_TextChanged;
                
                bgwReadFile.RunWorkerAsync();
                //string filepath = openFileDialog1.FileName;

            }
        }

        void regextest()
        {
            dataList.Clear();
            test = txtTest.Text;
            if (!bgwRegexAnalyze.IsBusy)
                bgwRegexAnalyze.RunWorkerAsync();
            /*test = Comments(test);
            txtResult.Clear();
            txtResult.Text = Reservadas(test);
            txtResult.AppendText(Relacionais(test));
            txtResult.AppendText(Aritemetico(test));
            txtResult.AppendText(NReais(test));
            txtResult.AppendText(NInteiro(test));
            txtResult.AppendText(Identificador(test));
            //txtResult.AppendText((test.Split('\n').Length).ToString());*/
        }

        /*void ANTLR()
        {
            string toanalyse = test;

            GrammarParse.GrammarParser grammarParser = new GrammarParse.GrammarParser();

            // Parse the input code
            IParseTree parseTree = grammarParser.Parse(toanalyse);

            txtResult.Text = parseTree.ToStringTree(grammarParser.GetParser());
        }*/

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void txtTest_TextChanged(object sender, EventArgs e)
        {
            regextest();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onetime = true;
            
            regextest();
        }

        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(autoToolStripMenuItem.Checked==true)
            {
                autoToolStripMenuItem.Checked = false;
                this.txtTest.TextChanged -= this.txtTest_TextChanged;
                try
                {
                    if (bgwRegexAnalyze.IsBusy)
                        bgwRegexAnalyze.CancelAsync();
                }
                catch { }   
                stop=true;
            }
            else
            {
                autoToolStripMenuItem.Checked = true;
                this.txtTest.TextChanged += this.txtTest_TextChanged;
                stop=false;
                regextest();
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savefile(0);
        }

        async void countlines(string texto)
        {
            string[] textolinescoiunty = texto.Split('\n');
            try
            {
                tsstxtLines.Text = textolinescoiunty.Length.ToString() + " lines";
            }
            catch
            {
                this.Invoke(new MethodInvoker(delegate { tsstxtLines.Text = textolinescoiunty.Count().ToString() + " lines"; }));
            }
        }

        private void bgwRegexAnalyze_DoWork(object sender, DoWorkEventArgs e)
        {
            string toanalyse="";
            while (!stop||onetime) 
            {
                if (test != toanalyse)
                {
                    toanalyse = test;
                    countlines(toanalyse);
                    string result = "";
                      if (type== "regex")
                        {
                        
                        toanalyse = Comments(toanalyse);
                        result = Reservadas(toanalyse);
                        result = result + (Relacionais(toanalyse));
                        result = result + (Aritemetico(toanalyse));
                        result = result + (NReais(toanalyse));
                        result = result + (NInteiro(toanalyse));
                        result = result + (Identificador(toanalyse));
                    }
                    else
                    {
                        GrammarParse.GrammarParser grammarParser = new GrammarParse.GrammarParser();

                        // Parse the input code
                        IParseTree parseTree = grammarParser.Parse(toanalyse);

                        result = parseTree.ToStringTree(grammarParser.GetParser());
                    }
                    try
                    {
                        this.Invoke(new MethodInvoker(delegate { txtResult.Text = result; }));
                    }
                    catch (Exception problem) {
                        if (!stop)
                            MessageBox.Show("Problem in background worker, could not invoke result text box:\n" + problem.Message, "Error BackgroundWorker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //txtResult.AppendText((test.Split('\n').Length).ToString());
                    if (onetime)
                    {
                        onetime = false;
                        break;
                    }
                }
                Thread.Sleep(500);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop = true;
        }

        private void saveFileAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savefile(1);
        }

        void savefile(int dontusesamepath)
        {
            if (dontusesamepath == 1)
            {
                if (savefiledlg.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(savefiledlg.FileName, txtTest.Text);
                    pathtosave = savefiledlg.FileName;
                }
            }
            else
            {
                try
                {

                    File.WriteAllText(pathtosave, txtTest.Text);
                }
                catch {
                    if (savefiledlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(savefiledlg.FileName, txtTest.Text);
                        pathtosave = savefiledlg.FileName;
                    }
                }
            }
        }

        private void bgwReadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            using (StreamReader reader = new StreamReader(openFileDialog1.OpenFile()))
            {
                string content="";
                while (!reader.EndOfStream)
                {
                    content = content + reader.ReadLine();
                    content = content + "\r\n";
                    if (content.Length>= 483647)
                    {
                        this.Invoke(new MethodInvoker(delegate { txtTest.AppendText(content); }));
                        content = "";
                    }
                }
                if(content!="")
                {
                    this.Invoke(new MethodInvoker(delegate { txtTest.AppendText(content); }));
                }
                this.Invoke(new MethodInvoker(delegate { txtTest.Enabled = true; tslblStatus.Text = "Ready"; tspgbStatus.Visible = false; testToolStripMenuItem.Enabled = true;
                    autoToolStripMenuItem.Enabled = true;
                    fileToolStripMenuItem.Enabled=true;
                    if (autoToolStripMenuItem.Checked == true)
                    {
                        this.txtTest.TextChanged += this.txtTest_TextChanged;
                        regextest();
                    }  
                }));
            }
            pathtosave = openFileDialog1.FileName;
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text == "ANTLR")
            {
                type = "ANTLR";
            }
            else
                type = "regex";
        }
    }
}
