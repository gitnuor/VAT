using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel.MushakReturn;
using vms.entity.viewModels.ReportsViewModel;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class MushakReturnRepository : IMushakReturnRepository
{
    private readonly DbContext _context;

    public MushakReturnRepository(DbContext context)
    {
        _context = context;
    }

    public VmMushakReturn GetMushakReturn(int organizationId, int year, int month)
    {
        var mushakReturn = new VmMushakReturn();
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", organizationId);
        parameter.Add("@Year", year);
        parameter.Add("@Month", month);
        using (var queryMultiple = _context.Database.GetDbConnection().QueryMultiple("SpGetMushakReturn @OrganizationId, @Year, @Month", parameter, commandTimeout: 500))
        {
            mushakReturn.MushakReturnPartOne = queryMultiple.ReadFirst<MushakReturnPartOne>();
            mushakReturn.MushakReturnPartTwo = queryMultiple.ReadFirst<MushakReturnPartTwo>();
            mushakReturn.MushakReturnPartThree = queryMultiple.ReadFirst<MushakReturnPartThree>();
            mushakReturn.MushakReturnPartFour = queryMultiple.ReadFirst<MushakReturnPartFour>();
            mushakReturn.MushakReturnPartFive = queryMultiple.ReadFirst<MushakReturnPartFive>();
            mushakReturn.MushakReturnPartSix = queryMultiple.ReadFirst<MushakReturnPartSix>();
            mushakReturn.MushakReturnPartSeven = queryMultiple.ReadFirst<MushakReturnPartSeven>();
            mushakReturn.MushakReturnPartEight = queryMultiple.ReadFirst<MushakReturnPartEight>();
            mushakReturn.MushakReturnPartNineList = queryMultiple.Read<MushakReturnPartNine>();
            mushakReturn.MushakReturnPartTen = queryMultiple.ReadFirst<MushakReturnPartTen>();
            mushakReturn.MushakReturnPartEleven = queryMultiple.ReadFirst<MushakReturnPartEleven>();
            mushakReturn.MushakReturnSubFormKaList = queryMultiple.Read<MushakReturnSubFormKa>(); ;
            mushakReturn.MushakReturnSubFormKhaList = queryMultiple.Read<MushakReturnSubFormKha>(); ;
            mushakReturn.MushakReturnSubFormGaList = queryMultiple.Read<MushakReturnSubFormGa>(); ;
            mushakReturn.MushakReturnSubFormGhaList = queryMultiple.Read<MushakReturnSubFormGha>(); ;
            mushakReturn.MushakReturnSubFormUmaList = queryMultiple.Read<MushakReturnSubFormUma>(); ;
            mushakReturn.MushakReturnSubFormChaList = queryMultiple.Read<MushakReturnSubFormCha>(); ;
            mushakReturn.MushakReturnSubFormChhaList = queryMultiple.Read<MushakReturnSubFormChha>(); ;
        }
        return mushakReturn;
    }

    public async Task<bool> InsertMushakSubmision(MushakSubmission model)
    {

        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertMushakSubmission]" +
                $"@MushakSubmissionTypeId" +
                $",@OrganizationId" +
                $",@MushakForYear" +
                $",@MushakForMonth" +
                $",@GenerateDate" +
                $",@SubmissionDate" +
                $",@VatResponsiblePersonId" +
                $",@IsWantToGetBackClosingBalance" +
                $",@CreatedBy" +
                $",@CreatedTime"
                , new SqlParameter("@MushakSubmissionTypeId", model.MushakSubmissionTypeId)
                , new SqlParameter("@OrganizationId", model.OrganizationId)
                , new SqlParameter("@MushakForYear", (object)model.MushakForYear ?? DBNull.Value)
                , new SqlParameter("@MushakForMonth", (object)model.MushakForMonth ?? DBNull.Value)
                , new SqlParameter("@GenerateDate", (object)model.GenerateDate ?? DBNull.Value)
                , new SqlParameter("@SubmissionDate", (object)model.SubmissionDate ?? DBNull.Value)
                , new SqlParameter("@VatResponsiblePersonId", (object)model.VatResponsiblePersonId ?? DBNull.Value)
                , new SqlParameter("@IsWantToGetBackClosingBalance", (object)model.IsWantToGetBackClosingBalance ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)model.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)model.CreatedTime ?? DBNull.Value)

            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }
}