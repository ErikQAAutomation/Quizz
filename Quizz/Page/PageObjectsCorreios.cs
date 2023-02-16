using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Quizz.Page
{
    public class PageObjectsCorreios
    {
        /// <summary>
        /// Carrega uma lista com os dados de Page Objetcs.
        /// </summary>
        /// <returns></returns>
        public List<PageCorreios> CarregarXMLPageObjects()
        {
            XDocument doc;
            List<PageCorreios> correiosObjetos = new List<PageCorreios>();

            string caminho = AppDomain.CurrentDomain.BaseDirectory.ToString();

            using (StreamReader sr = new StreamReader(caminho + "/CorreiosPageObjects.xml", Encoding.UTF8))
            {
                doc = XDocument.Load(sr);
            }

            var objetos = doc.Descendants("Objeto");

            foreach (var objeto in objetos)
            {
                if (objeto.HasElements)
                {
                    PageCorreios page = new PageCorreios();
                                                   
                    if (objeto.Element("nome") != null)
                        page.nomeLogico = objeto.Element("nome").Value;
                    if (objeto.Element("identificacaoFisica") != null)
                        page.identificacaoFisica = objeto.Element("identificacaoFisica").Value;
                    if (objeto.Element("seletorTipo") != null)
                        page.seletorTipo = objeto.Element("seletorTipo").Value;

                    correiosObjetos.Add(page);
                }
            }
            return correiosObjetos;
        }
    }


    public class PageCorreios
    {
        public string? nomeLogico { get; set; } = null;
        public string? seletorTipo { get; set; } = null;
        public string? identificacaoFisica { get; set; } = null;
    }

}
