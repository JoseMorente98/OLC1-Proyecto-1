using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorXML
    {
        private readonly static ControladorXML instancia = new ControladorXML();
        string path = Application.StartupPath;

        private ControladorXML()
        {
        }

        public static ControladorXML Instancia
        {
            get
            {
                return instancia;
            }
        }

        public void ReporteTokenXML()
        {
            XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement xElement = new XElement("ListaToken");
            xDocument.Add(xElement);
            foreach(Token token in ControladorToken.Instancia.ArrayListTokens)
            {
                XElement elementToken = new XElement("Token",
                    new XElement("ID", token.IdToken),
                    new XElement("Nombre", token.Descripcion),
                    new XElement("Valor", token.Lexema),
                    new XElement("Fila", token.Fila),
                    new XElement("Columna", token.Columna)
                );
                xElement.Add(elementToken);
            }
            if (!Directory.Exists(path + "\\Reportes\\Tokens"))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\Reportes\\Tokens");
            }

            String tokenPath = path + "\\Reportes\\Tokens\\TokenXML.xml";
            xDocument.Save(tokenPath);
            System.Diagnostics.Process.Start(tokenPath);
        }

        public void ReporteErrorXML()
        {
            XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement xElement = new XElement("ListaErrores");
            xDocument.Add(xElement);
            foreach (Token token in ControladorToken.Instancia.ArrayListErrors)
            {
                XElement elementToken = new XElement("Error",
                    new XElement("ID", token.IdToken),
                    new XElement("Nombre", token.Descripcion),
                    new XElement("Valor", token.Lexema),
                    new XElement("Fila", token.Fila),
                    new XElement("Columna", token.Columna)
                );
                xElement.Add(elementToken);
            }
            if (!Directory.Exists(path + "\\Reportes\\Tokens"))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\Reportes\\Tokens");
            }

            String tokenPath = path + "\\Reportes\\Tokens\\TokenErrorXML.xml";
            xDocument.Save(tokenPath);
            System.Diagnostics.Process.Start(tokenPath);
        }
    }
}
