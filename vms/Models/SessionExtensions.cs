using Newtonsoft.Json;
using vms.entity.viewModels;

namespace Microsoft.AspNetCore.Http;

public static class SessionExtensions
{
    public static void SetObject(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
    public static VmUserSession GetSesstion(this ISession session, string key)
    {
        var value = session.GetString(key);
        return  JsonConvert.DeserializeObject<VmUserSession>(value);
    }

}
public class GolbalSession
{
    public void SetInSession(ISession session,string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
    public VmUserSession GetFromSession(ISession session,string key)
    {
        var value = session.GetString(key);
        var sessionobject =  JsonConvert.DeserializeObject<VmUserSession>(value);
        return sessionobject;
    }

        
}