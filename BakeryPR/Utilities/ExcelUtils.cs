using BakeryPR.Models;
using NPOI.XSSF.UserModel;
using System.IO;

namespace BakeryPR.Utilities
{
    public class ExcelUtils
    {
        XSSFWorkbook wb;
        XSSFSheet sh;

        private string _subject;
        public string subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public void Export(int prod_id, string filepart, ExcelModel excelmodel)
        {
            wb = new XSSFWorkbook();

            // create sheet
            sh = (XSSFSheet)wb.CreateSheet("Sheet1");

            var tblHeader = sh.CreateRow(5);
            tblHeader.CreateCell(1).SetCellValue("Ingredients");
            tblHeader.CreateCell(2).SetCellValue("Quantity ");
            tblHeader.CreateCell(3).SetCellValue("Cost/Unit");
            tblHeader.CreateCell(4).SetCellValue("Total Cost ");

            tblHeader.CreateCell(6).SetCellValue("Bread Size/Type");
            tblHeader.CreateCell(7).SetCellValue("Qty Produced");
            tblHeader.CreateCell(8).SetCellValue("Unit Price");
            tblHeader.CreateCell(9).SetCellValue("Total");


            for (int i = 0; i < excelmodel.ProductionCost.Count; i++)
            {
                var r = sh.CreateRow(i);
                r.CreateCell(1).SetCellValue(excelmodel.ProductionCost[i].ingredentName);
                r.CreateCell(2).SetCellValue(excelmodel.ProductionCost[i].quantity);
                r.CreateCell(3).SetCellValue(excelmodel.ProductionCost[i].UnitCost);
                r.CreateCell(4).SetCellValue(excelmodel.ProductionCost[i].totalCost);
            }

            for (int i = 0; i < excelmodel.SalesCost.Count; i++)
            {
                var r = sh.CreateRow(i);
                r.CreateCell(1).SetCellValue(excelmodel.SalesCost[i].ProductName);
                r.CreateCell(2).SetCellValue(excelmodel.SalesCost[i].QuantityProduced);
                r.CreateCell(3).SetCellValue(excelmodel.SalesCost[i].UnitPrice);
                r.CreateCell(4).SetCellValue(excelmodel.SalesCost[i].totalPrice);
            }



            using (var fs = new FileStream(filepart, FileMode.Create, FileAccess.Write))
            {
                wb.Write(fs);
            }

        }

    }
}