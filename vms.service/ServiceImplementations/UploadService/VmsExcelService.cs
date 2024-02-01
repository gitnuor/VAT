using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class VmsExcelService : IVmsExcelService
{
    public List<T> ReadExcel<T>(IFormFile excelFile) where T : class
    {
        var results = new List<T>();
        using var stream = excelFile.OpenReadStream();
        stream.Position = 0;
        var xssWorkbook = new XSSFWorkbook(stream);
        var sheet = xssWorkbook.GetSheetAt(0);

        var modelType = typeof(T);
        var modelProperties = modelType.GetProperties().Where(p => p.CanWrite).ToList();

        for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            if (row == null) continue;
            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

            var resultType = Activator.CreateInstance<T>();
            var modelPropertySl = 0;
            foreach (var modelProperty in modelProperties)
            {
                ++modelPropertySl;
                var workingSl = modelPropertySl - 1;
                if (row.GetCell(workingSl) == null) continue;
                if (string.IsNullOrEmpty(row.GetCell(workingSl).ToString()) || string.IsNullOrWhiteSpace(row.GetCell(workingSl).ToString()))
                    continue;
                try
                {
                    var converter = TypeDescriptor.GetConverter(modelProperty.PropertyType);
                    modelType.GetProperty(modelProperty.Name)?.SetValue(resultType, converter.ConvertFrom(row.GetCell(workingSl).ToString()));
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
            }
            results.Add(resultType);
        }
        return results;
    }

    public string ReadUnknownExcel(IFormFile excelFile)
    {
        var dtTable = new DataTable();
        using (var stream = excelFile.OpenReadStream())
        {
            stream.Position = 0;
            var xssWorkbook = new XSSFWorkbook(stream);
            var sheet = xssWorkbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (var j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                dtTable.Columns.Add(cell.ToString());
            }
            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                var tableRow = dtTable.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                        {
                            tableRow[j] = row.GetCell(j);
                        }
                    }
                }
                dtTable.Rows.Add(tableRow);
            }
        }
        return JsonConvert.SerializeObject(dtTable);
    }
}