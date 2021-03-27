using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Data.UserDB
{
    public class DropdownItems
    {
        public static IDictionary<string, string> Genders()
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var GendersInDB = context.ReferenceDatas.Where(r => r.IsActive == true && r.RefCategory == "gender").ToList();
                if (GendersInDB.Count != 0)
                {
                    IDictionary<string, string> AvailableGenders = new Dictionary<string, string>();
                    foreach (var Gender in GendersInDB)
                    {
                        AvailableGenders.Add(Gender.DataKey, Gender.DataValue);
                    }
                    return AvailableGenders;
                }
            }
            return null;
        }

        public static IDictionary<string, string> CountryCodes()
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var CountryCodesInDB = context.Countries.Where(c => c.IsActive == true).ToList();
                if (CountryCodesInDB.Count != 0)
                {
                    IDictionary<string, string> AvailableCountryCodes = new Dictionary<string, string>();
                    foreach (var CountryCode in CountryCodesInDB)
                    {
                        AvailableCountryCodes.Add(CountryCode.CountryCode, CountryCode.CountryCode);
                    }
                    return AvailableCountryCodes;
                }
            }
            return null;
        }

    }
}