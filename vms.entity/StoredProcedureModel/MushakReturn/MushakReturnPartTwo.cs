using System;

namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnPartTwo
{
    public string TermOfTax { get; set; }
    public string TermOfTaxEng { get; set; }
    public string TypeOfSubmission { get; set; }
    public string TypeOfSubmissionEng { get; set; }
    public bool? IsTransactionOccuredInLastTerm { get; set; }
    public string IsTransactionOccuredInLastTermInWords { get; set; }
    public string isTransactionOccuredInLastTermInWordsEng { get; set; }
    public DateTime? SubmissionDate { get; set; }
}