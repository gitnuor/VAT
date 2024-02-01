using System;
using System.Collections.Generic;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmRdlcReportGenerateParam
{
    public VmRdlcReportGenerateParam()
    {
        StringParameters = new Dictionary<string, List<string>>();
        IntParameters = new Dictionary<string, List<int>>();
        BoolParameters = new Dictionary<string, List<bool>>();
        FloatParameters = new Dictionary<string, List<float>>();
        DateTimeParameters = new Dictionary<string, List<DateTime>>();
        ReportDataSets = new Dictionary<string, List<object>>();
    }

    public string RdlcFileUrl { get; set; }
    public string ExportFileFormat { get; private set; }
    public string ExportContentType { get; private set; }
    public string ExportContentExtension { get; private set; }
    public Dictionary<string, List<string>> StringParameters { get; }
    public Dictionary<string, List<int>> IntParameters { get;  }
    public Dictionary<string, List<bool>> BoolParameters { get;  }
    public Dictionary<string, List<float>> FloatParameters { get;  }
    public Dictionary<string, List<DateTime>> DateTimeParameters { get;  }
    public Dictionary<string, List<object>> ReportDataSets { get;  }

    public void SetExportFileFormat(string fromatName)
    {
        ExportFileFormat = fromatName;
    }

    public void AddStringParameter(string parameterName, List<string> parameterValues)
    {
        StringParameters.Add(parameterName, parameterValues);
    }

    private void SetFormatToExcel()
    {

    }
}