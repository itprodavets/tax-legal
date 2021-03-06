using System;

namespace TaxLegal.Cbc.Report.Application.Dto
{
    public class ReportData : BaseDto
    {
        public SupportedSchema Version { get; set; }
        public Message Message { get; set; } = new Message();
        public Report[] Reports { get; set; } = Array.Empty<Report>();
    }
}