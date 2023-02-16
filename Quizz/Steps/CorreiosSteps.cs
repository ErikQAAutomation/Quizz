using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Quizz.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Quizz.Steps
{
    [Binding]
    public class CorreiosSteps
    {
        public static IWebDriver _driver = null;        
        public static string _url = "https://buscacepinter.correios.com.br/app/endereco/index.php";
        public static List<PageCorreios>? _pageCorrerio = new List<PageCorreios>();

        [BeforeFeature]
        public static void BeforeFeature()
        {
            PageObjectsCorreios page = new PageObjectsCorreios();            
            _pageCorrerio = page.CarregarXMLPageObjects();
                        
            _driver = StartDriver(_url);
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            ConsutaRastreamento();
            _driver.Dispose();
        }

        [Given(@"Que possuo acesso ao site dos correios")]
        public void GivenQuePossuoAcessoAoSiteDosCorreios()
        {            
            Assert.IsTrue(_driver.Url.Contains(_url));
        }

        [When(@"Procuro pelo CEP (.*)")]
        public void QuandoProcuroPeloCEP(string p0)
        {
            var executaAcao1= PreparaAcao(ConsultaObjeto("Endereco"));
            executaAcao1.SendKeys(p0);

            var executaAcao2 = PreparaAcao(ConsultaObjeto("Pesquisar"));
            executaAcao2.Click();
        }

        [Then(@"Valido endereco (.*)")]
        public void EntaoValidoEndereco(string endereco)
        {
            if (endereco.Equals("Dados não encontrado"))
            {
                var executaAcao = PreparaAcao(ConsultaObjeto("Dados nao encontrados"));                
                Assert.IsTrue(executaAcao.Text.Contains(endereco));                
            }
            else
            {
                var executaAcao = PreparaAcao(ConsultaObjeto("Valida endereco"));
                Assert.IsTrue(executaAcao.Text.Contains(endereco));                
            }
                                                        
        }

        [Then(@"localildade (.*)")]
        public void EntaoLocalildade(string localidade)
        {
            if (!String.IsNullOrWhiteSpace(localidade))
            {
                var executaAcao = PreparaAcao(ConsultaObjeto("Localidade"));
                Assert.IsTrue(executaAcao.Text.Contains(localidade));                
            }                
            else
                Assert.IsTrue(true);

            var executaAcao2 = PreparaAcao(ConsultaObjeto("Nova busca"));
            executaAcao2.Click();            
        }        

        /// <summary>
        /// Starta a execução do Driver com a URL informada.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static IWebDriver StartDriver(string url)
        {
            IWebDriver driver = null;

            try
            {
                ChromeOptions chrOpt = new ChromeOptions();
                chrOpt.AddArgument("start-maximized");                

                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driver = new ChromeDriver(driverService, chrOpt);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                //_wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(5));

                driver.Navigate().GoToUrl(url);
            }
            catch (Exception erro)
            {
                return null;
            }

            return driver;
        }

        /// <summary>
        /// Consulta o código de rastreamento.
        /// </summary>
        private static void ConsutaRastreamento()
        {
            string codigoRastreamento = "SS987654321BR";
            string mensagemCaptcha = "Preencha o campo captcha";
                        
            var executaAcao1 = PreparaAcao(ConsultaObjeto("Link correios"));
            executaAcao1.Click();
           
            var executaAcao2 = PreparaAcao(ConsultaObjeto("Aceito cookies"));
            executaAcao2.Click();
            
            var executaAcao3 = PreparaAcao(ConsultaObjeto("Codigo rastreamento"));
            executaAcao3.SendKeys(codigoRastreamento);
        
            var executaAcao4= PreparaAcao(ConsultaObjeto("Buscar rastreamento"));
            executaAcao4.Click();
            
            _driver.SwitchTo().Window(_driver.WindowHandles[1]);
           
            var executaAcao5 = PreparaAcao(ConsultaObjeto("Pesquisar rastreamento"));
            executaAcao5.Click();
            
            var executaAcao6 = PreparaAcao(ConsultaObjeto("Mensagem captcha"));
            Assert.IsTrue(executaAcao6.Text.Contains(mensagemCaptcha));
        }

        /// <summary>
        /// Consulta os dados para o nome logico informado.
        /// </summary>
        /// <param name="nomeObjeto"></param>
        /// <returns></returns>
        private static PageCorreios ConsultaObjeto(string nomeObjeto)
        {
            var retorno = _pageCorrerio.Where(a => a.nomeLogico.Equals(nomeObjeto)).LastOrDefault();
            PageCorreios pg = new PageCorreios
            {
                identificacaoFisica = retorno.identificacaoFisica,
                nomeLogico = retorno.nomeLogico,
                seletorTipo = retorno.seletorTipo
            };

            return pg;
        }

        /// <summary>
        /// Devolve o elemento pronto para receber ação.
        /// </summary>
        /// <param name="elemento"></param>
        /// <returns></returns>
        private static IWebElement PreparaAcao(PageCorreios elemento)
        {
            switch (elemento.seletorTipo)
            {
                case "id":
                   return _driver.FindElement(By.Id(elemento.identificacaoFisica));                    
                case "name":
                    return _driver.FindElement(By.Name(elemento.identificacaoFisica));                    
                case "xpath":
                    return _driver.FindElement(By.XPath(elemento.identificacaoFisica));
                case "css":
                    return _driver.FindElement(By.CssSelector(elemento.identificacaoFisica));
                default:
                    return null;
            }
        }
    }
}
