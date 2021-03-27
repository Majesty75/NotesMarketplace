using System.Linq;
using NotesMarketplace.Models.SystemConfigModels;
namespace NotesMarketplace.Data.SystemConfigDB
{
    public class SystemConfigData
    {
        public static SystemConfigModel GetSystemConfigData(string DataKey)
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                SystemConfiguration sc = context.SystemConfigurations.FirstOrDefault(s => s.ConfigKey == DataKey && s.IsActive);
                if (sc != null)
                {
                    return new SystemConfigModel()
                    {
                        DataKey = sc.ConfigKey,
                        DataValue = sc.ConfigValue
                    };
                }
                else
                    return null;
            }
        }
    }
}