using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketplace.Data.NoteDB
{
    public class NotesFilters
    {
        public static IDictionary<string, string> Types()
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var TypesInDB = context.NoteTypes.Where(t => t.IsActive == true).ToList();
                if (TypesInDB.Count != 0)
                {
                    IDictionary<string, string> AvailableTypes = new Dictionary<string, string>();
                    foreach (var Type in TypesInDB)
                    {
                        AvailableTypes.Add(Type.TypeName, Type.TypeName);
                    }
                    return AvailableTypes;
                }
            }
            return null;
        }

        public static IDictionary<string, string> Categories()
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var CategoriesInDB = context.NoteCategories.Where(c => c.IsActive == true).ToList();
                if (CategoriesInDB.Count != 0)
                {
                    IDictionary<string, string> AvailableCategories = new Dictionary<string, string>();
                    foreach (var Category in CategoriesInDB)
                    {
                        AvailableCategories.Add(Category.CategoryName, Category.CategoryName);
                    }
                    return AvailableCategories;
                }
            }
            return null;
        }

        public static IDictionary<string ,string> Universities()
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var UniversitiesInDB = context.Notes.Where(n => n.NoteStatus == 3).Select(n => n.University).Distinct().ToList();
                if (UniversitiesInDB.Count != 0)
                {
                    IDictionary<string, string> AvailableUniversities = new Dictionary<string, string>();
                    foreach (var University in UniversitiesInDB)
                    {
                        if (University == null)
                            continue;
                        AvailableUniversities.Add(University, University);
                    }
                    return AvailableUniversities;
                }
            }
            return null;
        }

        public static IDictionary<string ,string> Courses()
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var CoursesInDB = context.Notes.Where(n => n.NoteStatus == 3).Select(n => n.Course).Distinct().ToList();
                if (CoursesInDB.Count != 0)
                {
                    IDictionary<string, string> AvailableCourses = new Dictionary<string, string>();
                    foreach (var Course in CoursesInDB)
                    {
                        if (Course == null)
                            continue;
                        AvailableCourses.Add(Course,Course);
                    }
                    return AvailableCourses;
                }
            }
            return null;
        }

        public static IDictionary<string, string> Countries()
        {
            using (NotesMarketPlaceEntities context = new NotesMarketPlaceEntities())
            {
                var CountriesInDB = context.Countries.Where(c => c.IsActive == true).ToList();
                if (CountriesInDB.Count != 0)
                {
                    IDictionary<string , string> AvailableCountries = new Dictionary<string , string>();
                    foreach (var Country in CountriesInDB)
                    {
                        AvailableCountries.Add(Country.CountryName, Country.CountryName);
                    }
                    return AvailableCountries;
                }
            }
            return null;
        }

        public static IDictionary<int, string> Ratings()
        {
            return new Dictionary<int, string>()
            {
                { 5, "5 star"},
                { 4, "4 star"},
                { 3, "3 star"},
                { 2, "2 star"},
                { 1, "1 star"}
            };
        }

    }
}