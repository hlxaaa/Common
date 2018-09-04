using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace Common.Helper
{
    public class FileHelper : SingleTon<FileHelper>
    {

        public string UpdateImgBase64(string base64, string newUrl, string oldUrl)
        {
            base64 = base64.Substring(base64.IndexOf(',') + 1);
            byte[] byteArray = Convert.FromBase64String(base64);
            //var name = DateTime.Now.ToString("yyyyMMddHHmmss");
            //string newUrl = $"UpLoadFile/SportImage/{name}-{RandHelper.Instance.Number(3)}.jpg";
            var path = HttpContext.Current.Server.MapPath($"../{newUrl}");
            var delPath = HttpContext.Current.Server.MapPath($"../{oldUrl}");
            //if (File.Exists(delPath) && oldUrl != "Images\\default.png")这个要不要看设计吧
            File.Delete(delPath);
            Stream s = new FileStream(path, FileMode.Append);
            s.Write(byteArray, 0, byteArray.Length);
            s.Close();
            return newUrl;
        }
        
        public string SaveBase64Img(string base64, string shortPath)
        {
            base64 = base64.Substring(base64.IndexOf(',') + 1);
            byte[] byteArray = Convert.FromBase64String(base64);
            //var name = DateTime.Now.ToString("yyyyMMddHHmmss");
            //string shortPath = $"UpLoadFile/AppHeadImage/{userId}-{name}-{RandHelper.Instance.Number(3)}.jpg";这个应该在func里自己写好传进来
            var path = HttpContext.Current.Server.MapPath($"../{shortPath}");
            Stream s = new FileStream(path, FileMode.Append);
            s.Write(byteArray, 0, byteArray.Length);
            s.Close();
            return shortPath;
        }

        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="codeNumber">要生成二维码的字符串</param>       
        /// <param name="size">大小尺寸</param>  
        /// <returns>二维码图片</returns>  
        public Bitmap Create_ImgCode(string codeNumber, int size)
        {
            //创建二维码生成类  
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度  
            qrCodeEncoder.QRCodeScale = size;
            //设置编码版本  
            qrCodeEncoder.QRCodeVersion = 0;
            //设置编码错误纠正  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片  
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(codeNumber);
            return image;
        }

        /// <summary>
        /// 将文件夹压缩
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="compressedFileName"></param>
        /// <param name="overrideExisting"></param>
        /// <returns></returns>
        public bool PackageFolder(string folderName, string compressedFileName, bool overrideExisting)
        {
            ZipFile.CreateFromDirectory(folderName, compressedFileName);
            return true;
        }


        /// <summary>
        /// 仅供参考，file这里莫明不能用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void GetExcel<T>() where T : class, new()
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("商品编号");
            //row1.CreateCell(1).SetCellValue("名称");
            //row1.CreateCell(2).SetCellValue("规格");
            //row1.CreateCell(3).SetCellValue("包装单位");
            //row1.CreateCell(4).SetCellValue("生产厂家");
            //row1.CreateCell(5).SetCellValue("日期");
            //row1.CreateCell(6).SetCellValue("单位名称");
            //row1.CreateCell(7).SetCellValue("数量");
            //row1.CreateCell(8).SetCellValue("单价");
            //row1.CreateCell(9).SetCellValue("金额");
            //row1.CreateCell(10).SetCellValue("批号");
            //row1.CreateCell(11).SetCellValue("有效期");
            //row1.CreateCell(12).SetCellValue("生产日期");
            //将数据逐步写入sheet1各个行
            var list = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].ToString());


            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");

            string fileName = "查询" + dateTime + ".xls";
            //return File(ms, "application/vnd.ms-excel", fileName);
        }

        /// <summary>
        /// 把Sheet中的数据转换为DataTable
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public DataTable ExportToDataTable(ISheet sheet)
        {
            DataTable dt = new DataTable();

            //默认，第一行是字段
            IRow headRow = sheet.GetRow(0);

            //设置datatable字段
            for (int i = headRow.FirstCellNum, len = headRow.LastCellNum; i < len; i++)
            {
                dt.Columns.Add(headRow.Cells[i].StringCellValue);
            }
            //遍历数据行
            for (int i = (sheet.FirstRowNum + 1), len = sheet.LastRowNum + 1; i < len; i++)
            {
                IRow tempRow = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                //遍历一行的每一个单元格
                for (int r = 0, j = tempRow.FirstCellNum, len2 = tempRow.LastCellNum; j < len2; j++, r++)
                {

                    ICell cell = tempRow.GetCell(j);

                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.String:
                                dataRow[r] = cell.StringCellValue;
                                break;
                            case CellType.Numeric:
                                dataRow[r] = cell.NumericCellValue;
                                break;
                            case CellType.Boolean:
                                dataRow[r] = cell.BooleanCellValue;
                                break;
                            default:
                                dataRow[r] = "ERROR";
                                break;
                        }
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

    }
}
