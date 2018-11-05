using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sage.Core.Archive;

namespace ApiDigitalArquive
{
    class Program
    {

        // connection data
        const string sServer = "PTPWKSPDJNOBRE\\MSSQLSERVER2014";
        const string sDb = "DEMO_1GCO";
        const string sUser = "sa";
        const string sPwd = "myPassword";

        // Connection String para o Servidor e Bd onde reside o Arquivo Digital
        const string CNN_STRING = @"Data Source=" + sServer + ";Initial Catalog=" + sDb + ";User id=" + sUser + ";Password=" + sPwd;

        // login data
        const string pProduct = "1GCO";
        const string pUser = "SAGE";
        const string pName = "Sage User";
  
        // Arquive Data
        const string aDataFile = @"C:\Temp\test.jpg";

        // StoreID = Sigla da empresa (Necessário num cenário onde uma base de dados armazena arquivos digitais de várias empresas)
        const string CompanyID = "DEMO_1GCO";
        const string CompanyDescription = "Gestão Comercial";


        static void Main(string[] args)
        {

            // Exemplo 1
            novoItemArquivoExistente();

            // Exemplo 2
            fullStoreContainderCicle();

            Console.WriteLine("press any key...");
            Console.ReadLine();

        }

        /**
         * Exemplo:
         * Inserir Novo Item em Arquivo Existente
         * 
         * Pode ser utilizado para adicicionar um item a um arquivo criado de base na 
         * bd da aplicação Sage100c
         * 
         * Executar em modo Debug e verificar passo a passo a execução na BD
         * 
         */
        static void novoItemArquivoExistente()
        {

            // Informação de container identifica a entidade da aplicação que se pretende associar aos documentos a manipular
            const string ContainerID = "CLIENTES";              // Ex.:  usa a vista. CLIENTES = Código da vista de Clientes
            const string ContainerDescription = "Clientes";     // Ex.:  usa a descrição associada à vista "CLIENTES" = "Clientes"
  
            // Importar um documento especifico para uma entidade do arquivo digital
            // NOTA: O container para o item será criado se ainda não existir.
            // Inserir Ficheiro para o Cliente com Chave = "999"
            const string CorrelationID = "001";
            const string CorrelationDescription = "Clientes Nr. 001";   // Ex.:  usa a descrição associada à vista "CLIENTES" = "Clientes"

            IDocStoreServices Services = new DocStoreServices();

            // Informação da sessão (opcional). Usa o utilizador do sistema se não for especificado
            Services.SetSession(pProduct, pUser, pName, true);

            Console.WriteLine(Services.ImportItem(
                                                    0,                      // hostWnd
                                                    CNN_STRING,             // data source
                                                    CompanyID,              // store correlation ID
                                                    CompanyDescription,     // store correlation Description
                                                    ContainerID,            // container correlation ID
                                                    ContainerDescription,   // container correlation Description
                                                    CorrelationID,          // item Correlation ID
                                                    CorrelationDescription, // item Correlation Description
                                                    aDataFile,              // file name
                                                    aDataFile               // original File Name
                                                    ));
        }

        /**
         * Exemplo:
         * 
         * Cria um Novo Container
         * Importa um Item para o Container
         * Remove Item do Container
         * Remove o Container
         * 
         * Executar em modo Debug e verificar passo a passo a execução na BD
         * 
         */
        static void fullStoreContainderCicle()
        {
           // Informação de container identifica a entidade da aplicação que se pretende associar aos documentos a manipular
            const string ContainerID = "MyContainer";
            const string ContainerDescription = "MyContainerToStoreStuff";

            // Importar um documento especifico para uma entidade do arquivo digital
            // NOTA: O container para o item será criado se ainda não existir.
            // Inserir Ficheiro para o item com Chave = "ABC"
            const string CorrelationID = "ABC";
            const string CorrelationDescription = "ABC Stuff";

            IDocStoreServices Services = new DocStoreServices();

            // Informação da sessão (opcional). Usa o utilizador do sistema se não for especificado
            Services.SetSession(pProduct, pUser, pName, true);

            // Criar um container para armazenar informação de funcionários (As aplicações criam automáticamente os seus containers)
            // NOTA: O container será criado se ainda não existir, caso contrário não cria e devolve false
            Console.WriteLine(Services.CreateContainer(
                                                        0, 
                                                        CNN_STRING, 
                                                        CompanyID, 
                                                        ContainerID, 
                                                        ContainerDescription)
                                                        );

            // Importar um documento especifico para uma entidade do arquivo digital
            // NOTA: O container para o item será criado se ainda não existir.
            Console.WriteLine(Services.ImportItem(
                                                    0, 
                                                    CNN_STRING, 
                                                    CompanyID, 
                                                    CompanyDescription, 
                                                    ContainerID, 
                                                    ContainerDescription, 
                                                    CorrelationID, 
                                                    CorrelationDescription, 
                                                    aDataFile, 
                                                    aDataFile
                                                    ));

            // Remover informação de uma entidade especifica de um container (Ex.: Eliminar a informação do funcionário "00001" porque o funcionário foi eliminado)            
            Console.WriteLine(Services.RemoveItems(0, CNN_STRING, CompanyID, ContainerID, CorrelationID));

            // Remover um container bem como todos os items e histórico associados
            Console.WriteLine(Services.RemoveContainer(0, CNN_STRING, CompanyID, ContainerID));
        }
    }
}
