using Edocs.Mensageria.Powerdocs;
using Edocs.Mensageria.Powerdocs.Message;
using Inventti.PowerDocs.Domain.Dtos.Commons;
using Inventti.PowerDocs.Domain.Dtos.NF3e.Document;
using Inventti.PowerDocs.Domain.Enums.NF3e;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace RabbitMQPublisher
{
    public class Program
    {
        private static readonly MensageriaPowerdocs _mensageriaPowerdocs = new MensageriaPowerdocs("127.0.0.1", 5672, "guest", "guest");
        static void Main(string[] args)
        {
            Console.WriteLine("Realizando a publicação das mensagens");

            /*SendOneDocument(documentNumSeq: 6027);
            SendManyDocuments(quantity: 10);*/

            SendDocumentOccurrence(documentNumSeq: 6027);
        }

        /// <summary>
        /// Publicação de mensagens contendo o dto OccurrenceForCreateDto - (CreateForDocumentOccurrence)
        /// </summary>
        /// <param name="documentNumSeq">PK NF3ePack NF3eDocument</param>
        private static void SendDocumentOccurrence(long documentNumSeq)
        {
            OccurrenceForCreateDto occurrence = new OccurrenceForCreateDto()
            {
                Chave = "127498112445124312",
                DataOcorrencia = DateTime.Now,
                Status = 0,
                TipoOcorrencia = (int)ENF3eOccurrenceType.Criticism,
                Mensagem = "Teste de ocorrência"
            };

            PowerdocsMessage powerdocsMessage = PowerdocsMessage.CreateForDocumentOccurrence(documentNumSeq, EMessageOperationType.Create, JsonConvert.SerializeObject(occurrence));
            _mensageriaPowerdocs.Publicar(powerdocsMessage, "PowerDocs", "NF3ePack");
        }

        /// <summary>
        /// Publicação de mensagens contendo o dto NF3eDocumentForCreateDto - (CreateForDocument)
        /// </summary>
        /// <param name="quantity">Quantidade de documentos e respectivas mensagens que serão publicadas</param>
        private static void SendManyDocuments(int quantity)
        {
            long documentNumSeq = 0;

            while (documentNumSeq < quantity)
            {
                documentNumSeq++;

                string seqChave = documentNumSeq.ToString().PadLeft(9, '0');
                string chave = $"3221051504812400380655002{seqChave}1610660235";

                NF3eDocumentForCreateDto nf3eDocumentForCreateDto = new NF3eDocumentForCreateDto()
                {
                    NumeroSequencial = documentNumSeq,
                    Chave = chave,
                    CNPJCPFEmitente = "11111111111111",
                    Serie = 1,
                    Numero = documentNumSeq,
                    NomeEmitente = "Primeiro Emitente Teste",
                    CNPJCPFDestinatario = "22222222222222",
                    NomeDestinatario = "Primeiro Destinatário Teste",
                    DataEmissao = DateTime.Now,
                    Status = 4,
                    ValorTotal = 10.00m,
                    EmailsPDF = "ruano@inventti.com.br",
                    EmailsXML = "ruano@inventti.com.br",
                    DocumentoXML = ""
                };

                PowerdocsMessage powerdocsMessage = PowerdocsMessage.CreateForDocument(documentNumSeq, EMessageOperationType.Create, JsonConvert.SerializeObject(nf3eDocumentForCreateDto));
                _mensageriaPowerdocs.Publicar(powerdocsMessage, "PowerDocs", "NF3ePack");

                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// Publicação de mensagens contendo o dto NF3eDocumentForCreateDto - (CreateForDocument)
        /// </summary>
        /// <param name="documentNumSeq">PK NF3ePack NF3eDocument</param>
        private static void SendOneDocument(long documentNumSeq)
        {
            string seqChave = documentNumSeq.ToString().PadLeft(9, '0');
            string chave = $"3221051504812400380655002{seqChave}1610660235";

            NF3eDocumentForCreateDto nf3eDocumentForCreateDto = new NF3eDocumentForCreateDto()
            {
                NumeroSequencial = documentNumSeq,
                Chave = chave,
                CNPJCPFEmitente = "11111111111111",
                Serie = 1,
                Numero = documentNumSeq,
                NomeEmitente = "Primeiro Emitente Teste",
                CNPJCPFDestinatario = "22222222222222",
                NomeDestinatario = "Primeiro Destinatário Teste",
                DataEmissao = DateTime.Now,
                Status = 4,
                ValorTotal = 10.00m,
                EmailsPDF = "ruano@inventti.com.br",
                EmailsXML = "ruano@inventti.com.br",
                DocumentoXML = ""
            };

            PowerdocsMessage powerdocsMessage = PowerdocsMessage.CreateForDocument(documentNumSeq, EMessageOperationType.Create, JsonConvert.SerializeObject(nf3eDocumentForCreateDto));
            _mensageriaPowerdocs.Publicar(powerdocsMessage, "PowerDocs", "NF3ePack");
        }
    }
}
