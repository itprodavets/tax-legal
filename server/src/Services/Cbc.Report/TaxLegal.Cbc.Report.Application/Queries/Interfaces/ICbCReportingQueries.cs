// using System.IO;
// using System.Threading.Tasks;
// using TaxLegal.Cbc.Report.Application.Dto;
// using TaxLegal.Cbc.Report.Application.Schemas;
// using TaxLegal.Cbc.Report.Application.Xml;
// using TaxLegal.Cbc.Report.Application.Xml.Interfaces;
//
// namespace TaxLegal.Cbc.Report.Application.Queries.Interfaces
// {
//     public interface ICbCReportingQueries
//     {
//         Task<ReportData> XmlToModel(SupportedSchema schema, FileStream stream);
//         Task<IXmlFile> ModelToXml(SupportedSchema schema, ReportData data);
//     }
// }