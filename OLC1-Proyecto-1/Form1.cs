using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using MetroFramework;

namespace OLC1_Proyecto_1
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        /**
         * PROPIEDADES GLOBALES 
         */
        private int tabContador = 2;
        //string auxiliar = "";
        public string charInicial = "";
        string appPath = Application.StartupPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void nuevaPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tabContador++;
            var tabPage = new TabPage("Pestaña No. " + tabContador);
            tabControl1.Controls.Add(tabPage);
            var fastColoredTextBox = new FastColoredTextBox();
            fastColoredTextBox.Width = 700;
            fastColoredTextBox.Height = 465;
            tabPage.Controls.Add(fastColoredTextBox);
        }

        private void abrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "er files (*.er)|*.er";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                tabControl1.SelectedTab.Text = filePath;
            }

            if (File.Exists(filePath))
            {
                StreamReader streamReader = new StreamReader(filePath);
                string line;
                foreach (Control c in tabControl1.SelectedTab.Controls)
                {
                    FastColoredTextBox fastColoredTextBox = c as FastColoredTextBox;
                    c.Text = "";
                    try
                    {
                        line = streamReader.ReadLine();
                        while (line != null)
                        {
                            fastColoredTextBox.AppendText(line + "\n");
                            line = streamReader.ReadLine();
                        }
                    }
                    catch (Exception)
                    {
                        MostrarAlerta("Ha ocurrido un error D:");
                    }
                    streamReader.Close();
                }
            }
        }

        private void guardarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(tabControl1.SelectedTab.Text))
            {
                String dir = tabControl1.SelectedTab.Text;
                StreamWriter streamWriter = new StreamWriter(@dir);
                try
                {
                    foreach (Control c in tabControl1.SelectedTab.Controls)
                    {
                        FastColoredTextBox fastColoredTextBox = c as FastColoredTextBox;
                        try
                        {
                            streamWriter.WriteLine(fastColoredTextBox.Text);
                            streamWriter.WriteLine("\n");
                        }
                        catch (Exception)
                        {
                            MostrarAlerta("Ha ocurrido un error D:");
                        }
                    }
                }
                catch (Exception) { }
                streamWriter.Close();
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save ER Files";
                saveFileDialog.DefaultExt = "er";
                saveFileDialog.Filter = "ER files (*.er)|*.er";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = tabControl1.SelectedTab.Text;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    String dir = saveFileDialog.FileName;
                    StreamWriter streamWriter = new StreamWriter(@dir);
                    tabControl1.SelectedTab.Text = dir;
                    try
                    {
                        foreach (Control c in tabControl1.SelectedTab.Controls)
                        {
                            FastColoredTextBox fastColoredTextBox = c as FastColoredTextBox;
                            try
                            {
                                streamWriter.WriteLine(fastColoredTextBox.Text);
                                streamWriter.WriteLine("\n");
                            }
                            catch (Exception)
                            {
                                MostrarAlerta("Ha ocurrido un error D:");

                            }
                        }
                    }
                    catch
                    {
                        MostrarAlerta("Ha ocurrido un error D:");
                    }
                    streamWriter.Close();
                }
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save ER Files";
            saveFileDialog.DefaultExt = "er";
            saveFileDialog.Filter = "ER files (*.er)|*.er";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = tabControl1.SelectedTab.Text;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                String dir = saveFileDialog.FileName;
                StreamWriter streamWriter = new StreamWriter(@dir);
                try
                {
                    foreach (Control c in tabControl1.SelectedTab.Controls)
                    {
                        FastColoredTextBox fastColoredTextBox = c as FastColoredTextBox;
                        try
                        {
                            streamWriter.WriteLine(fastColoredTextBox.Text);
                            streamWriter.WriteLine("\n");
                        }
                        catch (Exception)
                        {
                            MostrarAlerta("Ha ocurrido un error D:");

                        }
                    }
                }
                catch
                {
                    MostrarAlerta("Ha ocurrido un error D:");
                }
                streamWriter.Close();
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /**
         * MÉTODO DE ALERTA 
         */
        public void MostrarAlerta(String mensaje)
        {
            MessageBox.Show(mensaje, "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
