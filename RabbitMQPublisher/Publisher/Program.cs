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

            //SendOneDocument(documentNumSeq: 7007);
            //SendManyDocuments(quantity: 10);*/

            //SendOneDocumentOccurrence(documentNumSeq: 7000);
            //SendManyDocumentOccurrences(documentNumSeq: 7002, quantity: 11);

            SendOneDocumentFiscalEvent(documentNumSeq: 7007);
        }

        /// <summary>
        /// Publicação de mensagem contendo o dto FiscalEventForCreateDto - (CreateForFiscalEvent)
        /// </summary>
        /// <param name="documentNumSeq">PK NF3ePack NF3eDocument</param>
        private static void SendOneDocumentFiscalEvent(long documentNumSeq)
        {
            FiscalEventForCreateDto fiscalEventForCreateDto = new FiscalEventForCreateDto()
            {
                Chave = "23545616546312642",
                Status = (int)ENF3eFiscalEventStatus.Criticized,
                NumeroSequencial = 1,
                TipoEvento = (int)ENF3eFiscalEventType.Cancelation,
                DataEvento = DateTime.Now,
                Descricao = "Teste de evento por msg",
            };

            PowerdocsMessage powerdocsMessage = PowerdocsMessage.CreateForFiscalEvent(documentNumSeq, EMessageOperationType.Create, JsonConvert.SerializeObject(fiscalEventForCreateDto));
            _mensageriaPowerdocs.Publicar(powerdocsMessage, "PowerDocs", "NF3ePack");
        }

        /// <summary>
        /// Publicação de mensagens contendo o dto NF3eDocumentForCreateDto - (CreateForDocument)
        /// </summary>
        /// <param name="quantity">Quantidade de documentos e respectivas mensagens que serão publicadas, caso não passado assume o valor default</param>
        /// /// <param name="documentNumSeq">PK NF3ePack NF3eDocument</param>
        private static void SendManyDocumentOccurrences(long documentNumSeq, int quantity = 11)
        {
            int occurrenceTypeEnumValue = 0;

            for (int i = 0; i <= quantity; i++)
            {
                occurrenceTypeEnumValue++;

                OccurrenceForCreateDto occurrence = new OccurrenceForCreateDto()
                {
                    Chave = $"3221051504812400380655002{documentNumSeq}1610660235",
                    DataOcorrencia = DateTime.Now,
                    Status = new Random().Next(0, 2),
                    TipoOcorrencia = occurrenceTypeEnumValue,
                    Mensagem = $"Teste de ocorrências... {Enum.GetName(typeof(ENF3eOccurrenceType), occurrenceTypeEnumValue)}."
                };

                PowerdocsMessage powerdocsMessage = PowerdocsMessage.CreateForDocumentOccurrence(documentNumSeq, EMessageOperationType.Create, JsonConvert.SerializeObject(occurrence));
                _mensageriaPowerdocs.Publicar(powerdocsMessage, "PowerDocs", "NF3ePack");

                Thread.Sleep(200);

                if (occurrenceTypeEnumValue == 11)
                {
                    occurrenceTypeEnumValue = 0;
                }

            }

        }

        /// <summary>
        /// Publicação de mensagem contendo o dto OccurrenceForCreateDto - (CreateForDocumentOccurrence)
        /// </summary>
        /// <param name="documentNumSeq">PK NF3ePack NF3eDocument</param>
        private static void SendOneDocumentOccurrence(long documentNumSeq)
        {
            OccurrenceForCreateDto occurrence = new OccurrenceForCreateDto()
            {
                Chave = $"3221051504812400380655002{documentNumSeq}1610660235",
                DataOcorrencia = DateTime.Now,
                Status = 0,
                TipoOcorrencia = (int)ENF3eOccurrenceType.OfflineContingency,
                Mensagem = "Teste de ocorrência do tipo de contingencia offline"
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
                    CNPJCPFEmitente = "1254124252352",
                    Serie = 21,
                    Numero = documentNumSeq,
                    NomeEmitente = "Emitente para teste do rabbitMq",
                    CNPJCPFDestinatario = "41221412432",
                    NomeDestinatario = "Destinatário para teste do rabbitMq",
                    DataEmissao = DateTime.Now,
                    Status = 4,
                    ValorTotal = 66.00m,
                    EmailsPDF = "guilherme.francisco@inventti.com.br",
                    EmailsXML = "guilherme.francisco@inventti.com.br",
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
                EmailsPDF = "guilherme.francisco@inventti.com.br",
                EmailsXML = "guilherme.francisco@inventti.com.br",
                DocumentoXML = ""
            };

            PowerdocsMessage powerdocsMessage = PowerdocsMessage.CreateForDocument(documentNumSeq, EMessageOperationType.Create, JsonConvert.SerializeObject(nf3eDocumentForCreateDto));
            _mensageriaPowerdocs.Publicar(powerdocsMessage, "PowerDocs", "NF3ePack");
        }
    }
}
