using System;

namespace TaxLegal.Cbc.Report.Application.Dto
{
    public class Report : BaseDto
    {
        public ReportingEntity ReportingEntity { get; set; } = new ReportingEntity();
        public ConstituentEntity[] ConstituentEntities { get; set; } = Array.Empty<ConstituentEntity>();
        public AdditionalInfo[] AdditionalInfo { get; set; } = Array.Empty<AdditionalInfo>();
        public ReportBody[] Reports { get; set; } = Array.Empty<ReportBody>();
    }
}