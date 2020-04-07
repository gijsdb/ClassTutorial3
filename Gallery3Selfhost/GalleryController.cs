using System.Data;
using System.Collections.Generic;
using Gallery3WinForm;
using System;

namespace Gallery3Selfhost
{
    public class GalleryController:System.Web.Http.ApiController 
    {
        public List<string> GetArtistNames() {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Artist", null);
            List<string> lcNames = new List<string>();
            foreach (DataRow dr in lcResult.Rows) lcNames.Add((string)dr[0]);
            return lcNames;
        }

        public clsArtist GetArtist(string Name)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", Name);
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Artist WHERE Name = @Name", par);

            if (lcResult.Rows.Count > 0)
                return new clsArtist()
                {
                    Name = (string)lcResult.Rows[0]["Name"],
                    Speciality = (string)lcResult.Rows[0]["Speciality"],
                    Phone = (string)lcResult.Rows[0]["Phone"],
                    WorksList = getArtistsWork(Name)
                };
            else
                return null;
        }

        private List<clsAllWork> getArtistsWork(string prName)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", prName);
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Work WHERE ArtistName = @Name", par);
            List<clsAllWork> lcWorks = new List<clsAllWork>();

            foreach (DataRow dr in lcResult.Rows) lcWorks.Add(dataRow2AllWork(dr));
            return lcWorks;
        }

        private clsAllWork dataRow2AllWork(DataRow dr)
        {
            return new clsAllWork()
            {
                WorkType = Convert.ToChar(dr["WorkType"]),
                Name = Convert.ToString(dr["Name"]),
                Date = Convert.ToDateTime(dr["Date"]),
                Value = Convert.ToDecimal(dr["Value"]),
                Width = dr["Width"] is DBNull ? (float?)null : Convert.ToSingle(dr["Width"]),
                Height = dr["Height"] is DBNull ? (float?)null : Convert.ToSingle(dr["Height"]),
                Type = Convert.ToString(dr["Type"]),
                Weight = dr["Weight"] is DBNull ? (float?)null : Convert.ToSingle(dr["Weight"]),
                Material = Convert.ToString(dr["Material"]),
                ArtistName = Convert.ToString(dr["ArtistName"]),
            };
        }

        public string PutArtist(clsArtist prArtist)
        { 
            // update
            try {
                int lcRecCount = clsDbConnection.Execute(
                    "UPDATE Artist SET Speciality = @Speciality, Phone = @Phone WHERE Name = @Name",
                    prepareArtistParameters(prArtist));

                if (lcRecCount == 1)
                    return "One artist updated";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        private Dictionary<string, object> prepareArtistParameters(clsArtist prArtist)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(3);
            par.Add("Name", prArtist.Name);
            par.Add("Speciality", prArtist.Speciality);
            par.Add("Phone", prArtist.Phone);
           return par;
        }

        public string PostArtist(clsArtist prArtist)
        {
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "INSERT INTO Artist VALUES(@Name, @Speciality, @Phone)", prepareArtistParameters(prArtist));
                if (lcRecCount == 1)
                    return "One artist added";
                else
                    return "Unexpected artist count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

    }

}
