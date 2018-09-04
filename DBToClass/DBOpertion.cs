using Common.Helper;
using Common.Result;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class GetDBOpertion
    {
        //private string connString = ConfigurationManager.AppSettings["ConnString"].ToString();
        public string connString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        private List<string> tableNames = new List<string>();
        private string dbName = null;
        private Dictionary<string, List<Fieid>> FieidDic = new Dictionary<string, List<Fieid>>();

        public GetDBOpertion()
        {
            GetTables();
            foreach (string tableName in tableNames)
            {
                GetFieId(tableName);
            }
        }

        /// <summary>
        /// 从数据库中获取表名
        /// </summary>
        private void GetTables()
        {
            DataTable table;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                table = conn.GetSchema(SqlClientMetaDataCollectionNames.Tables);
                conn.Close();
            }
            foreach (DataRow dr in table.Rows)
            {
                tableNames.Add(dr[2].ToString());
                dbName = dr[0].ToString();
            }
            tableNames = tableNames.OrderBy(p => p.ToString()).ToList();
        }

        /// <summary>
        /// 根据表名获取字段
        /// </summary>
        private void GetFieId(string tableName)
        {
            if (tableName == "ICChargeRecord")
                tableName = tableName;
            DataTable table = new DataTable();
            List<Fieid> fieids = new List<Fieid>();
            List<string> keyName = new List<string>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlDataAdapter adpter = new SqlDataAdapter("Select * from " + tableName + " where 1!=1", conn);
                //添加key
                adpter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adpter.Fill(table);
                conn.Close();
            }
            foreach (DataColumn dc in table.PrimaryKey)
            {
                keyName.Add(dc.ColumnName);
            }

            string str = $@"SELECT
col.name AS field,
isnull(prop.[value],'') AS remark
FROM
sys.columns col
left join sys.extended_properties prop
on (col.object_id = prop.major_id AND prop.minor_id = col.column_id)
WHERE
col.object_id =
(SELECT object_id FROM sys.tables WHERE name = '{tableName}')";
            var list = SqlHelper.Instance.ExecuteGetDt<Field_Remark>(str, new Dictionary<string, string>());

            foreach (DataColumn dc in table.Columns)
            {
                Fieid fieid = new Fieid();
                fieid.Fieid_Length = dc.MaxLength;
                fieid.Fieid_Name = dc.ColumnName;
                if (list.Where(p => p.field == dc.ColumnName).Count() > 0)
                    fieid.remark = list.Where(p => p.field == dc.ColumnName).First().remark;
                fieid.Fieid_Type = dc.DataType.Name;
                fieid.AllowDBNull = dc.AllowDBNull;
                if (keyName.Where(p => p == dc.ColumnName).FirstOrDefault() != null)
                {
                    fieid.PrimaryKey = true;
                }
                else
                {
                    fieid.PrimaryKey = false;
                }
                fieids.Add(fieid);
            }



            FieidDic.Add(tableName, fieids);
        }

        /// <summary>
        /// 获取表中字段信息
        /// </summary>
        /// <returns></returns>
        public List<Fieid> GetFieIdInfo(string tableName)
        {
            List<Fieid> fieids = FieidDic.Where(p => p.Key == tableName).FirstOrDefault().Value;
            return fieids;
        }

        /// <summary>
        /// 获取表中字段信息
        /// </summary>
        /// <returns></returns>
        public List<string> GetTableName()
        {
            return tableNames;
        }

        /// <summary>
        /// 获取库名
        /// </summary>
        /// <returns></returns>
        public string GetDbName()
        {
            return dbName;
        }
    }
}
