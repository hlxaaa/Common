using Common.Result;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DBToClass
{
    public class OperFunc : SingleTon<OperFunc>
    {
        StringBuilder strBuilderHead = new StringBuilder();
        string Wrap = "\n";
        string tab = "    ";
        string leftbra = "{";
        string rightbra = "}";
        private string space = "DbOpertion";
        public OperFunc()
        {
            Head();
        }

        /// <summary>
        /// 头部
        /// </summary>
        private void Head()
        {
            strBuilderHead
                          //.Append(Wrap).Append("using System.Data.SqlClient;")
                          //.Append(Wrap).Append("using System.Configuration;")
                          .Append("using System.Text;")
                          .Append(Wrap).Append("using Common.Helper;")
                          .Append(Wrap).Append("using System;")
                          //.Append(Wrap).Append("using takeAway.Helper;")
                          //.Append(Wrap).Append("using System.Linq;")
                          .Append(Wrap).Append("using System.Collections.Generic;")
                             .Append(Wrap).Append("using Common;")
                               .Append(Wrap).Append("using System.Linq;")
                          .Append(Wrap).Append("using ").Append(space).Append(".Models;")
                          .Append(Wrap).Append("using System.Data.SqlClient;")
                          .Append(Wrap).Append(Wrap).Append("namespace ").Append(space).Append(".DBoperation")
                          .Append(Wrap).Append(leftbra).Append(Wrap);
        }

        /// <summary>
        /// 内容文本
        /// </summary>
        /// <param name="Fieids">对应字段</param>
        /// <param name="TableName">对应表名</param>
        private StringBuilder Content(List<Fieid> Fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderPrimaryKey = new StringBuilder();
            strBuliderContent.Append(tab).Append("public partial class ").Append(TableName).Append($"Oper : SingleTon<{TableName}Oper>")
                             .Append(Wrap).Append(tab).Append(leftbra);
            var insert = InsertSql(Fieids, TableName);
            var delete = DeleteSql(Fieids, TableName);
            var update = UpdataSql(Fieids, TableName);

            var getParameters = GetParameters(Fieids, TableName);

            var getParameterItem = GetParametersItem(Fieids, TableName);
            var getUpdateStrItem = UpdataSqlItem(Fieids, TableName);
            var updateList = UpdateList(Fieids, TableName);


            var getAllList = GetAllList(Fieids, TableName);
            var getExist = GetExist(Fieids, TableName);
            var add = Add(Fieids, TableName);
            var updateMine = Update(Fieids, TableName);
            var deleteMine = Delete1(Fieids, TableName);
            var getById = GetById(Fieids, TableName);

            var select = SelectSql(Fieids, TableName);
            var selectByPage = SelectSqlByPage(Fieids, TableName);
            var selectByIds = SelectSqlByIds(Fieids, TableName);
            strBuliderContent.Append(getParameters).Append(insert).Append(delete).Append(update).Append(add).Append(updateMine).Append(deleteMine).Append(getParameterItem).Append(getUpdateStrItem).Append(updateList).Append(getById).Append(getAllList).Append(tab).Append(rightbra).Append(Wrap).Append(rightbra);
            return strBuliderContent;
        }

        /// <summary>
        /// 更新sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder UpdataSql(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            var tableName = TableName.ToLower();
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 更新")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public string GetUpdateStr(").Append(TableName).Append(" ").Append(tableName).Append(")")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();").Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"part1.Append(\"update {tableName} set \");").Append(Wrap);

            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey != true)
                {
                    strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")

                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append(",\");")
                                 .Append(Wrap);
                }

            }
            strBuliderContent.Append(strBuliderSql);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderContent.Append(tab).Append(tab).Append(tab).Append("int n = part1.ToString().LastIndexOf(\",\");").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Remove(n, 1);").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" where " + fieid.Fieid_Name + "= @" + fieid.Fieid_Name + "  \");");
                }
            }
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return part1.ToString();");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder UpdataSqlItem(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            var tableName = TableName.ToLower();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 更新")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public string GetUpdateStrItem(").Append(TableName).Append(" ").Append(tableName).Append(", int i)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();").Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"part1.Append(\"update {tableName} set \");").Append(Wrap);

            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey != true)
                {
                    strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")

                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append($\"").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("{i},\");")
                                 .Append(Wrap);
                }

            }
            strBuliderContent.Append(strBuliderSql);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderContent.Append(tab).Append(tab).Append(tab).Append("int n = part1.ToString().LastIndexOf(\",\");").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Remove(n, 1);").Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"part1.Append($\" where {fieid.Fieid_Name}= @" + fieid.Fieid_Name + "{i}  \");");
                }
            }
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return part1.ToString();");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder GetParameters(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            var tableName = TableName.ToLower();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 获取参数")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public Dictionary<string, string> GetParameters(").Append(TableName).Append(" ").Append(tableName).Append(")")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("Dictionary<string, string> dict = new Dictionary<string, string>();").Append(Wrap);
            //.Append(tab).Append(tab).Append(tab).Append("part1.Append(\"update " + tableName + " set \");").Append(Wrap);

            foreach (Fieid fieid in fieids)
            {
                //if (fieid.PrimaryKey != true)
                //{
                strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")

                              //.Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = '\"+").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append("+\"',\");")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append($"dict.Add(\"@{fieid.Fieid_Name}\", {tableName}.{fieid.Fieid_Name}.ToString());")
                             .Append(Wrap);
                //}

            }
            strBuliderContent.Append(strBuliderSql);
            //foreach (Fieid fieid in fieids)
            //{
            //    if (fieid.PrimaryKey == true)
            //    {
            //        strBuliderContent.Append(tab).Append(tab).Append(tab).Append("int n = part1.ToString().LastIndexOf(\",\");").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Remove(n, 1);").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" where " + fieid.Fieid_Name + "=\" +" + tableName + "." + fieid.Fieid_Name + " + \"\");");
            //    }
            //}
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return dict;");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra);
            return strBuliderContent;
        }

        private StringBuilder GetParametersItem(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            var tableName = TableName.ToLower();
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 获取参数")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public Dictionary<string, string> GetParametersItem(").Append(TableName).Append(" ").Append(tableName).Append(",int i)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("Dictionary<string, string> dict = new Dictionary<string, string>();").Append(Wrap);
            //.Append(tab).Append(tab).Append(tab).Append("part1.Append(\"update " + tableName + " set \");").Append(Wrap);

            foreach (Fieid fieid in fieids)
            {
                //if (fieid.PrimaryKey != true)
                //{
                strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")

                              //.Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = '\"+").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append("+\"',\");")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append($"dict.Add(\"@{fieid.Fieid_Name}\" + i, {tableName}.{fieid.Fieid_Name}.ToString());")
                             .Append(Wrap);
                //}

            }
            strBuliderContent.Append(strBuliderSql);
            //foreach (Fieid fieid in fieids)
            //{
            //    if (fieid.PrimaryKey == true)
            //    {
            //        strBuliderContent.Append(tab).Append(tab).Append(tab).Append("int n = part1.ToString().LastIndexOf(\",\");").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Remove(n, 1);").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" where " + fieid.Fieid_Name + "=\" +" + tableName + "." + fieid.Fieid_Name + " + \"\");");
            //    }
            //}
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return dict;");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra);
            return strBuliderContent;
        }



        private StringBuilder GetAllList(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// GET")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append($"public List<{tableName}> GetAllList()")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"return SqlHelper.Instance.GetListFromDb<{tableName}>();")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }


        private StringBuilder GetExist(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// GETExist")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append($"public List<{tableName}> GetExist()")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return Get().Where(p => p.isDeleted == false).ToList();")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder Add(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// add")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append($"public int Add({tableName} model, SqlConnection connection = null, SqlTransaction transaction = null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var str = GetInsertStr(model)+\" select @@identity\";")
                                                           .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("  var dict = GetParameters(model);")

                               .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder Update(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// update")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public int Update(" + tableName + " model, SqlConnection connection = null, SqlTransaction transaction = null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)

                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var str = GetUpdateStr(model);")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("  var dict = GetParameters(model);")
                               .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return SqlHelper.Instance.ExcuteNonQuery(str, dict, connection, transaction);")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder UpdateList(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// update")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public void UpdateList(List<" + tableName + "> list, SqlConnection connection = null, SqlTransaction transaction = null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)

                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var str = \"\";")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var dict = new Dictionary<string,string>();")

                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("for(int i=0;i<list.Count;i++)")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("{")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var tempDict=GetParametersItem(list[i],i);")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("foreach(var item in tempDict)")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("{")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("dict.Add(item.Key,item.Value);")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("}")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("str+=GetUpdateStrItem(list[i],i);")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("}")
                               .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder Delete0(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            return strBuliderContent;
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// del")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public int Delete0(int id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("" + tableName + " model = new " + tableName + "();")


                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder Delete1(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            //return strBuliderContent;
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// del")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public int Delete1(int id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"var str = \"delete from {tableName} where id = \" + id;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"var r = SqlHelper.Instance.ExecuteScalar(str);")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"return Convert.ToInt32(r);")


                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder GetById(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// getById")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append($"public {tableName} GetById(int id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"return SqlHelper.Instance.GetById<{tableName}>(id);")


                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        /// <summary>
        /// 删除sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder DeleteSql(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            return strBuliderContent;
            StringBuilder strBuliderSql = new StringBuilder();
            StringBuilder strBuliderSqlSet = new StringBuilder();
            StringBuilder strBuliderSqlWhere = new StringBuilder();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 删除")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"Id\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public bool Delete(int id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra);
            strBuliderSql.Append("Delete From ").Append(TableName);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderSqlWhere.Append(fieid.Fieid_Name).Append("=@").Append(fieid.Fieid_Name);
                    strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("object parm = new { ").Append(fieid.Fieid_Name).Append(" = id };");
                }
            }
            strBuliderSql.Append(" where ").Append(strBuliderSqlWhere);
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("using (var conn = new SqlConnection(ConnString))")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Open();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("var r = conn.Execute(@").Append("\"")
                             .Append(strBuliderSql).Append("\", parm);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Close();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("return r > 0;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);

            return strBuliderContent;
        }

        /// <summary>
        /// 插入sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder InsertSql(List<Fieid> fieids, string TableName)
        {
            var tableName = TableName.ToLower();
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 插入")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public string GetInsertStr(").Append(TableName).Append(" ").Append(tableName).Append(")")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra).Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part2 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab)
                             .Append(Wrap);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey != true)
                {
                    strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(",\");")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part2.Append(\"").Append("@" + fieid.Fieid_Name).Append(",\");")

                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra).Append(Wrap);


                }
            }
            strBuliderContent.Append(strBuliderSql)
                             .Append(tab).Append(tab).Append(tab).Append("StringBuilder sql = new StringBuilder();").Append(Wrap).Append(tab).Append(tab).Append(tab);

            strBuliderContent.Append("sql.Append(\"insert into " + tableName + "(\").Append(part1.ToString().Remove(part1.Length - 1)).Append(\") values (\").Append(part2.ToString().Remove(part2.Length-1)).Append(\")\");");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return sql.ToString();");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        /// <summary>
        /// 查询sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder SelectSql(List<Fieid> fieids, string TableName)
        {
            var tableName = TableName.ToLower();
            StringBuilder strBuliderContent = new StringBuilder();
            //return strBuliderContent;
            StringBuilder strBuliderSql = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 查询")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>对象列表</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public List<").Append(TableName).Append("> Select(").Append(TableName).Append(" ").Append(tableName).Append(")")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder sql = new StringBuilder(\"Select \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".Field.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(").Append(tableName).Append(".Field);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("else")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(\"*\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(\" from ").Append(TableName).Append(" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var parm = new DynamicParameters();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("bool flag = true;").Append(Wrap);
            foreach (Fieid fieid in fieids)
            {
                strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(".IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (flag)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("else")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" and ").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("parm.Add(\"").Append(fieid.Fieid_Name).Append("\", ").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            }
            strBuliderContent.Append(strBuliderSql)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".GroupBy.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" Group By \").Append(").Append(tableName).Append(".GroupBy).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".OrderBy.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" Order By \").Append(").Append(tableName).Append(".OrderBy).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!flag)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(\" where \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(part1);");

            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("using (var conn = new SqlConnection(ConnString))")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Open();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("var r = (List<").Append(TableName).Append(">)conn.Query<").Append(TableName).Append(">(sql.ToString(), parm);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Close();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (r == null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("r = new List<").Append(TableName).Append(">();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("return r;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        /// <summary>
        /// 分页查询sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder SelectSqlByPage(List<Fieid> fieids, string TableName)
        {
            var tableName = TableName.ToLower();
            StringBuilder strBuliderContent = new StringBuilder();
            //return strBuliderContent;
            StringBuilder strBuliderSql = new StringBuilder();
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 分页查询")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"pageSize\">页面大小</param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"pageNo\">页面编号</param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>对象列表</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public List<").Append(TableName).Append("> SelectByPage(").Append(TableName).Append(" ").Append(tableName).Append(", int pageSize, int pageNo)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder sql = new StringBuilder(\"Select Top \").Append(pageSize).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".Field.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(").Append(tableName).Append(".Field);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("else")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(\"*\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(\" from ").Append(TableName).Append(" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part2 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder strBuliderPage = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var parm = new DynamicParameters();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("bool flag = true;").Append(Wrap);
            foreach (Fieid fieid in fieids)
            {
                strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(".IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (flag)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("else")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" and ").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("parm.Add(\"").Append(fieid.Fieid_Name).Append("\", ").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            }
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (!flag)")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" and\");")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra);
                    strBuliderSql.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" ").Append(fieid.Fieid_Name)
                                 .Append(" not in (\").Append(\"Select Top \").Append(pageSize * (pageNo - 1)).Append(\" ").Append(fieid.Fieid_Name).Append(" from ").Append(TableName).Append(" \");");
                    strBuliderSql.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".GroupBy.IsNullOrEmpty())")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" Group By \").Append(").Append(tableName).Append(".GroupBy).Append(\" \");")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".OrderBy.IsNullOrEmpty())")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" Order By \").Append(").Append(tableName).Append(".OrderBy).Append(\" \");")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra);
                    strBuliderSql.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" )\");");
                }
            }
            strBuliderContent.Append(strBuliderSql)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!flag)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(\" where \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(part1).Append(strBuliderPage).Append(part1);");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".GroupBy.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part2.Append(\" Group By \").Append(").Append(tableName).Append(".GroupBy).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".OrderBy.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part2.Append(\" Order By \").Append(").Append(tableName).Append(".OrderBy).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(part2);");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("using (var conn = new SqlConnection(ConnString))")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Open();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("var r = (List<").Append(TableName).Append(">)conn.Query<").Append(TableName).Append(">(sql.ToString(), parm);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Close();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (r == null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("r = new List<").Append(TableName).Append(">();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("return r;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra);
            return strBuliderContent;
        }

        /// <summary>
        /// 查询sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder SelectSqlByIds(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            //return strBuliderContent;
            StringBuilder strBuliderSql = new StringBuilder();
            StringBuilder strBuliderSqlSet = new StringBuilder();
            StringBuilder strBuliderSqlWhere = new StringBuilder();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 根据Id查询")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"Id\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public List<").Append(TableName).Append("> SelectByIds(List<string> List_Id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra);
            strBuliderSql.Append("Select * From ").Append(TableName);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderSqlWhere.Append(fieid.Fieid_Name).Append(" in @").Append(fieid.Fieid_Name);
                    strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("object parm = new { ").Append(fieid.Fieid_Name).Append(" = List_Id.ToArray() };");
                }
            }
            strBuliderSql.Append(" where ").Append(strBuliderSqlWhere);
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("using (var conn = new SqlConnection(ConnString))")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Open();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("var r = (List<").Append(TableName).Append(">)conn.Query<").Append(TableName).Append(">(\"").Append(strBuliderSql).Append("\", parm);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Close();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (r == null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("r = new List<").Append(TableName).Append(">();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("return r;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);

            return strBuliderContent;
        }

        /// <summary>
        /// 获得字符串
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public string GetText(List<Fieid> fieids, string TableName)
        {
            var strBuilder = Content(fieids, TableName);
            return strBuilderHead.ToString() + strBuilder.ToString();
        }
    }

    public class OperFuncEmpty : SingleTon<OperFuncEmpty>
    {
        StringBuilder strBuilderHead = new StringBuilder();
        string Wrap = "\n";
        string tab = "    ";
        string leftbra = "{";
        string rightbra = "}";
        private string space = "DbOpertion";
        public OperFuncEmpty()
        {
            Head();
        }

        /// <summary>
        /// 头部
        /// </summary>
        private void Head()
        {
            strBuilderHead
                          //.Append(Wrap).Append("using System.Data.SqlClient;")
                          //.Append(Wrap).Append("using System.Configuration;")
                          .Append("using System.Text;")
                          .Append(Wrap).Append("using Common.Helper;")
                          .Append(Wrap).Append("using System;")
                          //.Append(Wrap).Append("using takeAway.Helper;")
                          //.Append(Wrap).Append("using System.Linq;")
                          .Append(Wrap).Append("using System.Collections.Generic;")
                             .Append(Wrap).Append("using Common;")
                               .Append(Wrap).Append("using System.Linq;")
                          .Append(Wrap).Append("using ").Append(space).Append(".Models;")
                          .Append(Wrap).Append("using System.Data.SqlClient;")
                          .Append(Wrap).Append(Wrap).Append("namespace ").Append(space).Append(".DBoperation")
                          .Append(Wrap).Append(leftbra).Append(Wrap);
        }

        /// <summary>
        /// 内容文本
        /// </summary>
        /// <param name="Fieids">对应字段</param>
        /// <param name="TableName">对应表名</param>
        private StringBuilder Content(List<Fieid> Fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderPrimaryKey = new StringBuilder();
            strBuliderContent.Append(tab).Append("public partial class ").Append(TableName).Append($"Oper : SingleTon<{TableName}Oper>")
                             .Append(Wrap).Append(tab).Append(leftbra);

            strBuliderContent.Append(Wrap).Append(Wrap);
            strBuliderContent.Append(tab).Append(rightbra).Append(Wrap).Append(rightbra);
            return strBuliderContent;
        }

        /// <summary>
        /// 更新sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder UpdataSql(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            var tableName = TableName.ToLower();
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 更新")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public string GetUpdateStr(").Append(TableName).Append(" ").Append(tableName).Append(")")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();").Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"part1.Append(\"update {tableName} set \");").Append(Wrap);

            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey != true)
                {
                    strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")

                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append(",\");")
                                 .Append(Wrap);
                }

            }
            strBuliderContent.Append(strBuliderSql);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderContent.Append(tab).Append(tab).Append(tab).Append("int n = part1.ToString().LastIndexOf(\",\");").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Remove(n, 1);").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" where " + fieid.Fieid_Name + "= @" + fieid.Fieid_Name + "  \");");
                }
            }
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return part1.ToString();");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder UpdataSqlItem(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            var tableName = TableName.ToLower();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 更新")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public string GetUpdateStrItem(").Append(TableName).Append(" ").Append(tableName).Append(", int i)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();").Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"part1.Append(\"update {tableName} set \");").Append(Wrap);

            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey != true)
                {
                    strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")

                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append($\"").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("{i},\");")
                                 .Append(Wrap);
                }

            }
            strBuliderContent.Append(strBuliderSql);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderContent.Append(tab).Append(tab).Append(tab).Append("int n = part1.ToString().LastIndexOf(\",\");").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Remove(n, 1);").Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"part1.Append($\" where {fieid.Fieid_Name}= @" + fieid.Fieid_Name + "{i}  \");");
                }
            }
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return part1.ToString();");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder GetParameters(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            var tableName = TableName.ToLower();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 获取参数")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public Dictionary<string, string> GetParameters(").Append(TableName).Append(" ").Append(tableName).Append(")")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("Dictionary<string, string> dict = new Dictionary<string, string>();").Append(Wrap);
            //.Append(tab).Append(tab).Append(tab).Append("part1.Append(\"update " + tableName + " set \");").Append(Wrap);

            foreach (Fieid fieid in fieids)
            {
                //if (fieid.PrimaryKey != true)
                //{
                strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")

                              //.Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = '\"+").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append("+\"',\");")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append($"dict.Add(\"@{fieid.Fieid_Name}\", {tableName}.{fieid.Fieid_Name}.ToString());")
                             .Append(Wrap);
                //}

            }
            strBuliderContent.Append(strBuliderSql);
            //foreach (Fieid fieid in fieids)
            //{
            //    if (fieid.PrimaryKey == true)
            //    {
            //        strBuliderContent.Append(tab).Append(tab).Append(tab).Append("int n = part1.ToString().LastIndexOf(\",\");").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Remove(n, 1);").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" where " + fieid.Fieid_Name + "=\" +" + tableName + "." + fieid.Fieid_Name + " + \"\");");
            //    }
            //}
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return dict;");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra);
            return strBuliderContent;
        }

        private StringBuilder GetParametersItem(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            var tableName = TableName.ToLower();
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 获取参数")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public Dictionary<string, string> GetParametersItem(").Append(TableName).Append(" ").Append(tableName).Append(",int i)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("Dictionary<string, string> dict = new Dictionary<string, string>();").Append(Wrap);
            //.Append(tab).Append(tab).Append(tab).Append("part1.Append(\"update " + tableName + " set \");").Append(Wrap);

            foreach (Fieid fieid in fieids)
            {
                //if (fieid.PrimaryKey != true)
                //{
                strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")

                              //.Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = '\"+").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append("+\"',\");")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append($"dict.Add(\"@{fieid.Fieid_Name}\" + i, {tableName}.{fieid.Fieid_Name}.ToString());")
                             .Append(Wrap);
                //}

            }
            strBuliderContent.Append(strBuliderSql);
            //foreach (Fieid fieid in fieids)
            //{
            //    if (fieid.PrimaryKey == true)
            //    {
            //        strBuliderContent.Append(tab).Append(tab).Append(tab).Append("int n = part1.ToString().LastIndexOf(\",\");").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Remove(n, 1);").Append(Wrap).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" where " + fieid.Fieid_Name + "=\" +" + tableName + "." + fieid.Fieid_Name + " + \"\");");
            //    }
            //}
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return dict;");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra);
            return strBuliderContent;
        }



        private StringBuilder GetAllList(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// GET")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append($"public List<{tableName}> GetAllList()")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"return SqlHelper.Instance.GetListFromDb<{tableName}>();")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }


        private StringBuilder GetExist(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// GETExist")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append($"public List<{tableName}> GetExist()")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return Get().Where(p => p.isDeleted == false).ToList();")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder Add(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// add")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append($"public int Add({tableName} model, SqlConnection connection = null, SqlTransaction transaction = null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var str = GetInsertStr(model)+\" select @@identity\";")
                                                           .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("  var dict = GetParameters(model);")

                               .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return Convert.ToInt32(SqlHelper.Instance.ExecuteScalar(str, dict, connection, transaction));")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder Update(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// update")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public int Update(" + tableName + " model, SqlConnection connection = null, SqlTransaction transaction = null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)

                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var str = GetUpdateStr(model);")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("  var dict = GetParameters(model);")
                               .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return SqlHelper.Instance.ExcuteNonQuery(str, dict, connection, transaction);")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder UpdateList(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// update")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public void UpdateList(List<" + tableName + "> list, SqlConnection connection = null, SqlTransaction transaction = null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)

                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var str = \"\";")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var dict = new Dictionary<string,string>();")

                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("for(int i=0;i<list.Count;i++)")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("{")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var tempDict=GetParametersItem(list[i],i);")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("foreach(var item in tempDict)")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("{")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("dict.Add(item.Key,item.Value);")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("}")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("str+=GetUpdateStrItem(list[i],i);")
                                .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("}")
                               .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("SqlHelper.Instance.ExcuteNon(str, dict, connection, transaction);")

                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder Delete0(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            return strBuliderContent;
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// del")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public int Delete0(int id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("" + tableName + " model = new " + tableName + "();")


                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder Delete1(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            //return strBuliderContent;
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// del")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public int Delete1(int id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"var str = \"delete from {tableName} where id = \" + id;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"var r = SqlHelper.Instance.ExecuteScalar(str);")
                              .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"return Convert.ToInt32(r);")


                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        private StringBuilder GetById(List<Fieid> fieids, string tableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// getById")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns></returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append($"public {tableName} GetById(int id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append($"return SqlHelper.Instance.GetById<{tableName}>(id);")


                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        /// <summary>
        /// 删除sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder DeleteSql(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            return strBuliderContent;
            StringBuilder strBuliderSql = new StringBuilder();
            StringBuilder strBuliderSqlSet = new StringBuilder();
            StringBuilder strBuliderSqlWhere = new StringBuilder();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 删除")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"Id\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public bool Delete(int id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra);
            strBuliderSql.Append("Delete From ").Append(TableName);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderSqlWhere.Append(fieid.Fieid_Name).Append("=@").Append(fieid.Fieid_Name);
                    strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("object parm = new { ").Append(fieid.Fieid_Name).Append(" = id };");
                }
            }
            strBuliderSql.Append(" where ").Append(strBuliderSqlWhere);
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("using (var conn = new SqlConnection(ConnString))")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Open();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("var r = conn.Execute(@").Append("\"")
                             .Append(strBuliderSql).Append("\", parm);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Close();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("return r > 0;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);

            return strBuliderContent;
        }

        /// <summary>
        /// 插入sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder InsertSql(List<Fieid> fieids, string TableName)
        {
            var tableName = TableName.ToLower();
            StringBuilder strBuliderContent = new StringBuilder();
            StringBuilder strBuliderSql = new StringBuilder();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 插入")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public string GetInsertStr(").Append(TableName).Append(" ").Append(tableName).Append(")")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra).Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part2 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab)
                             .Append(Wrap);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey != true)
                {
                    strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(" != null)")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(",\");")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part2.Append(\"").Append("@" + fieid.Fieid_Name).Append(",\");")

                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra).Append(Wrap);


                }
            }
            strBuliderContent.Append(strBuliderSql)
                             .Append(tab).Append(tab).Append(tab).Append("StringBuilder sql = new StringBuilder();").Append(Wrap).Append(tab).Append(tab).Append(tab);

            strBuliderContent.Append("sql.Append(\"insert into " + tableName + "(\").Append(part1.ToString().Remove(part1.Length - 1)).Append(\") values (\").Append(part2.ToString().Remove(part2.Length-1)).Append(\")\");");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("return sql.ToString();");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        /// <summary>
        /// 查询sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder SelectSql(List<Fieid> fieids, string TableName)
        {
            var tableName = TableName.ToLower();
            StringBuilder strBuliderContent = new StringBuilder();
            //return strBuliderContent;
            StringBuilder strBuliderSql = new StringBuilder();

            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 查询")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>对象列表</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public List<").Append(TableName).Append("> Select(").Append(TableName).Append(" ").Append(tableName).Append(")")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder sql = new StringBuilder(\"Select \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".Field.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(").Append(tableName).Append(".Field);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("else")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(\"*\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(\" from ").Append(TableName).Append(" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var parm = new DynamicParameters();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("bool flag = true;").Append(Wrap);
            foreach (Fieid fieid in fieids)
            {
                strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(".IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (flag)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("else")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" and ").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("parm.Add(\"").Append(fieid.Fieid_Name).Append("\", ").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            }
            strBuliderContent.Append(strBuliderSql)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".GroupBy.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" Group By \").Append(").Append(tableName).Append(".GroupBy).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".OrderBy.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" Order By \").Append(").Append(tableName).Append(".OrderBy).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!flag)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(\" where \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(part1);");

            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("using (var conn = new SqlConnection(ConnString))")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Open();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("var r = (List<").Append(TableName).Append(">)conn.Query<").Append(TableName).Append(">(sql.ToString(), parm);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Close();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (r == null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("r = new List<").Append(TableName).Append(">();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("return r;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            return strBuliderContent;
        }

        /// <summary>
        /// 分页查询sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder SelectSqlByPage(List<Fieid> fieids, string TableName)
        {
            var tableName = TableName.ToLower();
            StringBuilder strBuliderContent = new StringBuilder();
            //return strBuliderContent;
            StringBuilder strBuliderSql = new StringBuilder();
            strBuliderContent.Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 分页查询")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"").Append(tableName).Append("\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"pageSize\">页面大小</param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"pageNo\">页面编号</param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>对象列表</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public List<").Append(TableName).Append("> SelectByPage(").Append(TableName).Append(" ").Append(tableName).Append(", int pageSize, int pageNo)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder sql = new StringBuilder(\"Select Top \").Append(pageSize).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".Field.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(").Append(tableName).Append(".Field);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("else")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(\"*\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(\" from ").Append(TableName).Append(" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part1 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder part2 = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("StringBuilder strBuliderPage = new StringBuilder();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("var parm = new DynamicParameters();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("bool flag = true;").Append(Wrap);
            foreach (Fieid fieid in fieids)
            {
                strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(".IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (flag)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\"").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("else")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("part1.Append(\" and ").Append(fieid.Fieid_Name).Append(" = @").Append(fieid.Fieid_Name).Append("\");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("parm.Add(\"").Append(fieid.Fieid_Name).Append("\", ").Append(tableName).Append(".").Append(fieid.Fieid_Name).Append(");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra).Append(Wrap);
            }
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderSql.Append(tab).Append(tab).Append(tab).Append("if (!flag)")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" and\");")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra);
                    strBuliderSql.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" ").Append(fieid.Fieid_Name)
                                 .Append(" not in (\").Append(\"Select Top \").Append(pageSize * (pageNo - 1)).Append(\" ").Append(fieid.Fieid_Name).Append(" from ").Append(TableName).Append(" \");");
                    strBuliderSql.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".GroupBy.IsNullOrEmpty())")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" Group By \").Append(").Append(tableName).Append(".GroupBy).Append(\" \");")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".OrderBy.IsNullOrEmpty())")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" Order By \").Append(").Append(tableName).Append(".OrderBy).Append(\" \");")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("flag = false;")
                                 .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra);
                    strBuliderSql.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("strBuliderPage.Append(\" )\");");
                }
            }
            strBuliderContent.Append(strBuliderSql)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!flag)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("sql.Append(\" where \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(part1).Append(strBuliderPage).Append(part1);");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".GroupBy.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part2.Append(\" Group By \").Append(").Append(tableName).Append(".GroupBy).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("if (!").Append(tableName).Append(".OrderBy.IsNullOrEmpty())")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("part2.Append(\" Order By \").Append(").Append(tableName).Append(".OrderBy).Append(\" \");")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append("sql.Append(part2);");
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("using (var conn = new SqlConnection(ConnString))")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Open();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("var r = (List<").Append(TableName).Append(">)conn.Query<").Append(TableName).Append(">(sql.ToString(), parm);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Close();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (r == null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("r = new List<").Append(TableName).Append(">();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("return r;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra);
            return strBuliderContent;
        }

        /// <summary>
        /// 查询sql
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private StringBuilder SelectSqlByIds(List<Fieid> fieids, string TableName)
        {
            StringBuilder strBuliderContent = new StringBuilder();
            //return strBuliderContent;
            StringBuilder strBuliderSql = new StringBuilder();
            StringBuilder strBuliderSqlSet = new StringBuilder();
            StringBuilder strBuliderSqlWhere = new StringBuilder();
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append("/// <summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// 根据Id查询")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// </summary>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <param name=\"Id\"></param>")
                             .Append(Wrap).Append(tab).Append(tab).Append("/// <returns>是否成功</returns>")
                             .Append(Wrap).Append(tab).Append(tab).Append("public List<").Append(TableName).Append("> SelectByIds(List<string> List_Id)")
                             .Append(Wrap).Append(tab).Append(tab).Append(leftbra);
            strBuliderSql.Append("Select * From ").Append(TableName);
            foreach (Fieid fieid in fieids)
            {
                if (fieid.PrimaryKey == true)
                {
                    strBuliderSqlWhere.Append(fieid.Fieid_Name).Append(" in @").Append(fieid.Fieid_Name);
                    strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("object parm = new { ").Append(fieid.Fieid_Name).Append(" = List_Id.ToArray() };");
                }
            }
            strBuliderSql.Append(" where ").Append(strBuliderSqlWhere);
            strBuliderContent.Append(Wrap).Append(tab).Append(tab).Append(tab).Append("using (var conn = new SqlConnection(ConnString))")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Open();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("var r = (List<").Append(TableName).Append(">)conn.Query<").Append(TableName).Append(">(\"").Append(strBuliderSql).Append("\", parm);")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("conn.Close();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("if (r == null)")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(leftbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(tab).Append("r = new List<").Append(TableName).Append(">();")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(tab).Append("return r;")
                             .Append(Wrap).Append(tab).Append(tab).Append(tab).Append(rightbra)
                             .Append(Wrap).Append(tab).Append(tab).Append(rightbra).Append(Wrap);

            return strBuliderContent;
        }

        /// <summary>
        /// 获得字符串
        /// </summary>
        /// <param name="fieids"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public string GetText(List<Fieid> fieids, string TableName)
        {
            var strBuilder = Content(fieids, TableName);
            return strBuilderHead.ToString() + strBuilder.ToString();
        }
    }
}
