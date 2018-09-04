using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DBToClass
{
    public class DbToClass : SingleTon<DbToClass>
    {
        public void Create()
        {
            GetDBOpertion opertion = new GetDBOpertion();
            var table_Names = opertion.GetTableName();
            var ModelPath = "D:\\DB\\" + opertion.GetDbName() + "\\Model";
            if (!Directory.Exists(ModelPath))
            {
                Directory.CreateDirectory(ModelPath);
            }
            foreach (var item in table_Names)
            {
                var csModelPath = ModelPath + "\\" + item + ".cs";
                var list = opertion.GetFieIdInfo(item);
                var text = ModelFunc.Instance.GetText(list, item);
                File.WriteAllLines(csModelPath, text.Split('\n'));
            }
            ModelPath = "D:\\DB\\" + opertion.GetDbName() + "\\ModelExtend";
            if (!Directory.Exists(ModelPath))
            {
                Directory.CreateDirectory(ModelPath);
            }
            foreach (var item in table_Names)
            {
                var csModelPath = ModelPath + "\\" + item + ".cs";
                var list = opertion.GetFieIdInfo(item);
                var text = ModelFuncEmpty.Instance.GetText(list, item);
                File.WriteAllLines(csModelPath, text.Split('\n'));
            }

            var OperPath = "D:\\DB\\" + opertion.GetDbName() + "\\Oper";
            if (!Directory.Exists(OperPath))
            {
                Directory.CreateDirectory(OperPath);
            }
            foreach (var item in table_Names)
            {
                var csOperPath = OperPath + "\\" + item + "Oper.cs";
                var list = opertion.GetFieIdInfo(item);
                var text = OperFunc.Instance.GetText(list, item);
                File.WriteAllLines(csOperPath, text.Split('\n'));
            }
            OperPath = "D:\\DB\\" + opertion.GetDbName() + "\\OperExtend";
            if (!Directory.Exists(OperPath))
            {
                Directory.CreateDirectory(OperPath);
            }
            foreach (var item in table_Names)
            {
                var csOperPath = OperPath + "\\" + item + "Oper.cs";
                var list = opertion.GetFieIdInfo(item);
                var text = OperFuncEmpty.Instance.GetText(list, item);
                File.WriteAllLines(csOperPath, text.Split('\n'));
            }


        }
    }
}
