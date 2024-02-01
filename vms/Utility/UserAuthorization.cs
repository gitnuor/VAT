using System.Collections.Generic;
using System.Linq;
using vms.entity.models;

namespace vms.Utility;

public static class UserAuthorization
{
    public static bool Check(string featureName, IList<Right> roleData)
    {
        return roleData.Any(rf => rf.RightName.Equals(featureName));
    }
}