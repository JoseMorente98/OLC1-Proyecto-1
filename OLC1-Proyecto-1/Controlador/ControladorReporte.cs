using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorReporte
    {
        private readonly static ControladorReporte instancia = new ControladorReporte();
        private ControladorReporte()
        {
        }

        public static ControladorReporte Instancia
        {
            get
            {
                return instancia;
            }
        }

        /**
         * REPORTE DE TOKEN 
         */
        #region REPORTE DE TOKEN
        public void GetReportTokens(String nombre)
        {
            string tbody = "";
            string content = "";
            foreach (Token t in ControladorToken.Instancia.ArrayListTokens)
            {
                string lexema = t.Lexema;
                if (t.Descripcion.Equals("ComentarioMultilinea"))
                {
                    lexema = lexema.Replace("<!", " ");
                    lexema = lexema.Replace("!>", " ");

                }
                if (t.Descripcion.Equals("Cadena_TODO"))
                {
                    lexema = "[:" + lexema + ":]";
                }
                content = "<tr>\n" +
                    "     <th scope=\"row\">" + t.IdToken + "</th>\n" +
                    "     <td>" + lexema + "</td>\n" +
                    "     <td>" + t.Descripcion + "</td>\n" +
                    "     <td>" + t.Fila + "</td>\n" +
                    "     <td>" + t.Columna + "</td>\n" +
                    "</tr>";
                tbody = tbody + content;
            }
            string thead = "<th scope=\"col\">ID Token</th>\n" +
            "<th scope=\"col\">Token</th>\n" +
            "<th scope=\"col\">Descripcion</th>" +
            "<th scope=\"col\">Fila</th>\n" +
            "<th scope=\"col\">Columna</th>";
            GetHTML("Tokens" + nombre, thead, tbody, "Listado de Tokens detectados por el analizador");
        }
        #endregion

        /**
         * REPORTE DE TOKEN ERROR
         */
        #region REPORTE DE TOKEN ERROR
        public void GetReportTokensError(String nombre)
        {
            string tbody = "";
            string content = "";
            foreach (Token t in ControladorToken.Instancia.ArrayListErrors)
            {
                content = "<tr>\n" +
                    "     <th scope=\"row\">" + t.IdToken + "</th>\n" +
                    "     <td>" + t.Lexema + "</td>\n" +
                    "     <td>" + t.Descripcion + "</td>\n" +
                    "     <td>" + t.Fila + "</td>\n" +
                    "     <td>" + t.Columna + "</td>\n" +
                    "</tr>";
                tbody = tbody + content;
            }
            string thead = "<th scope=\"col\">ID Token</th>\n" +
            "<th scope=\"col\">Token</th>\n" +
            "<th scope=\"col\">Descripcion</th>\n" +
            "<th scope=\"col\">Fila</th>\n" +
            "<th scope=\"col\">Columna</th>";
            GetHTML("Tokens Error" + nombre, thead, tbody, "Listado de Tokens Error detectados por el analizador");
        }
        #endregion
        
        /**
         * CUERPO HTML
         */
        #region CUERPO HTML
        public void GetHTML(string title, string thead, string tbody, string descripcion)
        {
            string html = "<html lang=\"en\">\n" +
            "<head>\n" +
            "    <meta charset=\"UTF-8\">\n" +
            "    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n" +
            "    <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\">\n" +
            "    <link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css\" integrity=\"sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T\" crossorigin=\"anonymous\">\n" +
            "    <title>" + title + "</title>\n" +
            "</head>\n" +
            "<body>\n" +
            "    <nav class=\"navbar navbar-dark bg-dark\">\n" +
            "    <a class=\"navbar-brand\" href=\"#\">\n" +
            "        <img src=\"https://www.usac.edu.gt/g/escudo10.png\" width=\"30\" height=\"30\" class=\"d-inline-block align-top\" alt=\"\">\n" +
            "        Organización de Lenguajes y Compiladores 1\n" +
            "    </a>\n" +
            "    <form class=\"form-inline my-2 my-lg-0\">\n" +
            "      <input class=\"form-control mr-sm-2\" id=\"search\" type=\"search\" placeholder=\"Search\" aria-label=\"Search\">\n" +
            "    </form>\n" +
            "    </nav>\n" +
            "    <div class=\"container\">\n" +//DateTime.Now.ToString("G");
            "    <div class=\"jumbotron jumbotron-fluid\">\n" +
            "    <div class=\"container\">\n" +
            "        <h1 class=\"display-2\">" + title + "</h1>\n" +
            "        <p class=\"lead\">"+descripcion+"</p>\n" +
            "        <p class=\"lead\"><strong>" + DateTime.Now.ToString("G") + "</strong></p>\n" +
            "    </div>\n" +
            "    </div>\n" +
            "    <table class=\"table\" id=\"mytable\">\n" +
            "    <thead class=\"thead-dark\">\n" +
            "        <tr>\n" +
            thead +
            "        </tr>\n" +
            "    </thead>\n" +
            "    <tbody>\n" +
            tbody +
            "    </tbody>\n" +
            "    </table>\n" +
            "    </div>\n" +
            "\n" +
            "    <script src=\"https://code.jquery.com/jquery-3.3.1.slim.min.js\" integrity=\"sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo\" crossorigin=\"anonymous\"></script>\n" +
            "    <script>\n" +
            "    // Write on keyup event of keyword input element\n" +
            "    $(document).ready(function(){\n" +
            "    $(\"#search\").keyup(function(){\n" +
            "    _this = this;\n" +
            "    // Show only matching TR, hide rest of them\n" +
            "    $.each($(\"#mytable tbody tr\"), function() {\n" +
            "    if($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) === -1)\n" +
            "    $(this).hide();\n" +
            "    else\n" +
            "    $(this).show();\n" +
            "    });\n" +
            "    });\n" +
            "    });\n" +
            "    </script>\n" +
            "    <script src=\"https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js\" integrity=\"sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1\" crossorigin=\"anonymous\"></script>\n" +
            "    <script src=\"https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js\" integrity=\"sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM\" crossorigin=\"anonymous\"></script>\n" +
            "</body>\n" +
            "</html>";
            File.WriteAllText("Reporte de " + title + ".html", html);
            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.RenderHtmlAsPdf(html).SaveAs("Reporte de " + title + ".pdf");
            System.Diagnostics.Process.Start("Reporte de " + title + ".html");
            System.Diagnostics.Process.Start("Reporte de " + title + ".pdf");
        }
        #endregion
    }
}
