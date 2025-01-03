using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace mainService.Models
{
    public class cConnect : IDisposable
    {
        private string mConnectionString = "";
        private SqlConnection mDB;
        public cConnect()
        {
            try
            {
                this.mConnectionString = @"Data Source=DESKTOP-F9FMAIR\SQLEXPRESS;Initial Catalog=MobileUserDB;User ID='SnashyConnect'; Password = 'Festus2032' ";

                //this.mConnectionString = Properties.Settings.Default.connString;

                this.mDB = new SqlConnection(this.mConnectionString);
                this.mDB.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SqlDataReader ReadDB(string vSQL)
        {
            SqlDataReader r = null;

            try
            {
                SqlCommand vCMD = new System.Data.SqlClient.SqlCommand(vSQL, this.mDB);
                r = vCMD.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                throw;
            }
            return r;
        }

        public SqlDataAdapter ReadDB2(string vSQL)
        {
            SqlDataAdapter r = null;

            try
            {
                r = new SqlDataAdapter(vSQL, this.mDB);
                r.AcceptChangesDuringFill = false;
                r.AcceptChangesDuringUpdate = false;

            }
            catch (Exception)
            {
                throw;
            }
            return r;
        }

        public void WriteDB(string vSQL)
        {
            DataSet vDS = new DataSet();

            try
            {
                vDS.EnforceConstraints = true;

                SqlDataAdapter vDA = new SqlDataAdapter
                (vSQL, this.mConnectionString);

                vDA.AcceptChangesDuringFill = true;
                vDA.Fill(vDS);
            }
            catch (Exception)
            {
                vDS.RejectChanges();
                vDS.Dispose();
                throw;
            }
            finally
            {
                this.mDB.Close();
            }
        }

        public void Dispose()
        {
            try
            {
                if (this.mDB != null)
                    if (this.mDB.State != ConnectionState.Open)
                        this.mDB.Close();

                this.mDB.Dispose();
                this.mDB = null;
            }
            catch (Exception ex)
            { ex.Data.Clear(); }
        }

    }
}