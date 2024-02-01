using System.Collections.Generic;
using vms.entity.StoredProcedureModel.MushakReturn;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmMushakReturn
{
    public MushakReturnPartOne MushakReturnPartOne { get; set; }
    public MushakReturnPartTwo MushakReturnPartTwo { get; set; }
    public MushakReturnPartThree MushakReturnPartThree { get; set; }
    public MushakReturnPartFour MushakReturnPartFour { get; set; }
    public MushakReturnPartFive MushakReturnPartFive { get; set; }
    public MushakReturnPartSix MushakReturnPartSix { get; set; }
    public MushakReturnPartSeven MushakReturnPartSeven { get; set; }
    public MushakReturnPartEight MushakReturnPartEight { get; set; }
    public IEnumerable<MushakReturnPartNine> MushakReturnPartNineList { get; set; }
    public MushakReturnPartTen MushakReturnPartTen { get; set; }
    public MushakReturnPartEleven MushakReturnPartEleven { get; set; }
    public IEnumerable<MushakReturnSubFormKa> MushakReturnSubFormKaList { get; set; }
    public IEnumerable<MushakReturnSubFormKha> MushakReturnSubFormKhaList { get; set; }
    public IEnumerable<MushakReturnSubFormGa> MushakReturnSubFormGaList { get; set; }
    public IEnumerable<MushakReturnSubFormGha> MushakReturnSubFormGhaList { get; set; }
    public IEnumerable<MushakReturnSubFormUma> MushakReturnSubFormUmaList { get; set; }
    public IEnumerable<MushakReturnSubFormCha> MushakReturnSubFormChaList { get; set; }
    public IEnumerable<MushakReturnSubFormChha> MushakReturnSubFormChhaList { get; set; }
}