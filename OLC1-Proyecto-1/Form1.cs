using System;
using System.Collections;
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
using OLC1_Proyecto_1.Analizador;
using OLC1_Proyecto_1.Controlador;
using OLC1_Proyecto_1.Modelo;

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
        string fileName = "";
        string tipo = "";

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
            fastColoredTextBox.Width = 555;
            fastColoredTextBox.Height = 312;
            fastColoredTextBox.BackColor = Color.FromArgb(34,34,34);
            fastColoredTextBox.IndentBackColor = Color.FromArgb(34, 34, 34);
            fastColoredTextBox.ForeColor = Color.White;
            fastColoredTextBox.Font = new Font("Consolas", 10);
            System.Drawing.Font currentFont = fastColoredTextBox.Font;
            FontStyle newFontStyle = (FontStyle)(currentFont.Style | FontStyle.Regular);
            fastColoredTextBox.Font = new System.Drawing.Font(currentFont.FontFamily, 10, newFontStyle);
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
                fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
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

        private void reporteDeTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControladorXML.Instancia.ReporteTokenXML();
        }

        private void reporteDeErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControladorXML.Instancia.ReporteErrorXML();
        }

        private void reporteDeTokensToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ControladorReporte.Instancia.GetReportTokens();
        }

        private void reporteDeErrorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ControladorReporte.Instancia.GetReportTokensError();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            foreach (Control c in tabControl1.SelectedTab.Controls)
            {
                FastColoredTextBox rtb = c as FastColoredTextBox;
                if (rtb.Text != "")
                {
                    consola.Clear();
                    ControladorConjunto.Instancia.clearList();
                    ControladorNodo.Instancia.clearList();
                    ControladorEvaluador.Instancia.clearList();
                    ControladorToken.Instancia.Limpieza();
                    AnalizadorLexico.Instancia.Scanner(rtb.Text);

                    /*foreach(Token t in TokenController.Instancia.getArrayListTokens())
                    {
                        Console.WriteLine("ID: " + t.Description + " - " + t.Lexema);
                    }*/

                }
                else
                {
                    alertMessage("No existe texto para analizar D:");
                }
            }



            //////// PARTE DE LOS CONJUNTOS
            ControladorConjunto.Instancia.assemble_Sets();
            //SetController.Instancia.ShowSets();

            /////// Parte de la expresion regular
            ControladorExpresionRegular.Instancia.GetElements(Application.StartupPath);

            //LA CONSTRUCCION DE LOS AUTOMATAS SE PASO A 
            //RegularExpressionController-> Insertar, al final del metodo;


            //Evaluar la expresion
            GetString();
        }

        //METODO QUE BUSCA LA CADENA A EVALUAR;
        public void GetString()
        {
            int contador = 0;
            String expressionName = "";
            String strcadena = "";
            string cadena = "";
            string contenido = "";

            ArrayList l = ControladorToken.Instancia.ArrayListTokens;
            for (int i = 0; i < l.Count; i++)
            {
                Token t = (Token)l[i];
                if (t.Lexema.Equals(":"))
                {

                    //busca el nombre de la expresion
                    for (int j = i; j > 0; j--)
                    {
                        Token a = (Token)l[j];
                        if (a.Descripcion.Equals("Identificador"))
                        {
                            expressionName = a.Lexema;
                            break;
                        }
                    }
                    //itera en la expresion y guarda los elementos
                    for (int j = i + 1; j < l.Count; j++)
                    {
                        Token t2 = (Token)l[j];
                        if (!t2.Lexema.Equals(";")) //El limite de la expresion es el punto y coma
                        {
                            if (t2.Descripcion.Equals("Cadena"))
                            {
                                strcadena = t2.Lexema;
                            }
                        }
                        else
                        {
                            if (expressionName != "" && strcadena != "")
                            {

                                if (ControladorEvaluador.Instancia.SimulateExpression(expressionName, strcadena))
                                {

                                    consola.AppendText("* La Cadena " + strcadena + " de la Expresion " + expressionName + " fue Evaluada correctamente\n");
                                    contenido = "<tr>\n" +
                                       "     <td>" + "* La Cadena " + strcadena + " de la Expresion " + expressionName + " fue Evaluada correctamente\n" + "</td>\n" +
                                       "</tr>";
                                    cadena = cadena + contenido;
                                    ControladorEvaluador.Instancia.ReporteTokenXML(appPath, expressionName + "-" + contador);
                                }
                                /*else if (EvaluatorController.Instancia.SimulateExpressionWhitString(expressionName, strcadena))
                                {
                                    consola.AppendText("* La Cadena " + strcadena + " de la Expresion " + expressionName + " fue Evaluada correctamente\n");
                                }*/
                                else
                                {
                                    String error = ControladorEvaluador.Instancia.GetError();
                                    consola.AppendText(error);
                                    contenido = "<tr>\n" +
                                       "     <td>" + error + "</td>\n" +
                                       "</tr>";
                                    cadena = cadena + contenido;
                                    ControladorEvaluador.Instancia.ReporteErrorXML(appPath, expressionName + "-" + contador);
                                }
                                contador++;

                            }
                            i = j;
                            break;
                        }
                    }
                }


            }

            string cadena2 = "<th scope =\"col\">Evaluación</th>\n";
            ControladorReporte.Instancia.GetHTML("Consola" + fileName, cadena2, cadena, "Expresiones evaluadas en consola.");
        }

        public void alertMessage(String mensaje)
        {
            MessageBox.Show(mensaje, "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            int selectedIndex = comboBox1.SelectedIndex;
            Object selectedItem = comboBox1.SelectedItem;
            if(selectedItem!=null)
            {
                switch (selectedItem.ToString())
                {
                    case "Autómata Finito No Determinista":
                        tipo = "AFN";
                        foreach (Imagen imagen in ControladorImagen.Instancia.ArrayListImagen)
                        {
                            if (imagen.Tipo.Equals(tipo))
                            {
                                comboBox2.Items.Add(imagen);
                            }
                        }
                        break;
                    case "Tabla de Transiciones":
                        tipo = "Transicion";
                        foreach (Imagen imagen in ControladorImagen.Instancia.ArrayListImagen)
                        {
                            if (imagen.Tipo.Equals(tipo))
                            {
                                comboBox2.Items.Add(imagen);
                            }
                        }
                        break;
                    case "Autómata Finito Determinista":
                        tipo = "AFD";
                        foreach (Imagen imagen in ControladorImagen.Instancia.ArrayListImagen)
                        {
                            if (imagen.Tipo.Equals(tipo))
                            {
                                comboBox2.Items.Add(imagen);
                            }
                        }
                        break;
                    case "Token XML":
                        tipo = "Token";
                        foreach (Imagen imagen in ControladorImagen.Instancia.ArrayListImagen)
                        {
                            if (imagen.Tipo.Equals(tipo))
                            {
                                comboBox2.Items.Add(imagen);
                            }
                        }
                        break;
                    case "Token Error XML":
                        tipo = "Error";
                        foreach (Imagen imagen in ControladorImagen.Instancia.ArrayListImagen)
                        {
                            if (imagen.Tipo.Equals(tipo))
                            {
                                comboBox2.Items.Add(imagen);
                            }
                        }
                        break;
                    default:
                        tipo = "";
                        break;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBox2.SelectedIndex;
            Object selectedItem = comboBox2.SelectedItem;
            if(selectedItem!=null)
            {
                Imagen imagen1 = ControladorImagen.Instancia.BuscarImagen(selectedItem.ToString());
                if(imagen1.Tipo.Equals("Token"))
                {
                    System.Diagnostics.Process.Start(imagen1.Path);
                } else if (imagen1.Tipo.Equals("Error"))
                {
                    System.Diagnostics.Process.Start(imagen1.Path);
                } else
                {
                    this.pictureBox1.Image = System.Drawing.Image.FromFile(imagen1.Path + "\\" + imagen1.Nombre);
                }
            }
        }
    }
}
