using System;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace FAST3_DataAccess
{
    public class ConfigFile
    {
        private string ConnectionWay { get; set; }

        public ConfigFile(string connWay) => this.ConnectionWay = connWay;

        public string GetConnectionString()
        {
            string connString = "";
            string uID = ConfigurationManager.AppSettings["UID"].ToString().Trim();
            string pwd = ConfigurationManager.AppSettings["PWD"].ToString().Trim();
            if (ConnectionWay.ToUpper() == "DIRECT")
            {
                //方式为直连
                string server = ConfigurationManager.AppSettings["Server"].ToString().Trim();
                string direct = ConfigurationManager.AppSettings["Direct"].ToString().Trim();
                string sid = ConfigurationManager.AppSettings["Sid"].ToString().Trim();
                string port = ConfigurationManager.AppSettings["Port"].ToString().Trim();

                connString = string.Format("User ID={0}; Password={1}; Server={2}; Direct={3}; SID={4}; Port={5}"
                    , uID, pwd, server, direct, sid, port);
            }
            else
            {
                //方式为非直连(预留)
                string dbName = ConfigurationManager.AppSettings["DBName"].ToString().Trim();

                connString = "Data Source=" + dbName + ";User ID=" + uID + ";Password=" + pwd + "";
            }
            return connString;
        }

    }
}
