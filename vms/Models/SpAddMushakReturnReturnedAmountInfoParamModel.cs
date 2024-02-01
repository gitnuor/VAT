using System;

namespace vms.Models;

public class SpAddMushakReturnReturnedAmountInfoParamModel
{
    public int MushakGenerationId { get; set; }
    public decimal? ReturnAmountFromClosingVat { get; set; }
    public int? ReturnFromClosingVatChequeBankId { get; set; }
    private string ReturnFromClosingVatChequeNo { get; set; }
    public DateTime? ReturnFromClosingVatChequeDate { get; set; }
    public decimal? ReturnAmountFromClosingSd { get; set; }
    public int? ReturnFromClosingSdChequeBankId { get; set; }
    private string ReturnFromClosingSdChequeNo { get; set; }
    public DateTime? ReturnFromClosingSdChequeDate { get; set; }
}