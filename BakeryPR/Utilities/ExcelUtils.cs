using BakeryPR.Models;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Linq;

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
            int row = 5;
            var tblHeader = sh.CreateRow(row++);
            tblHeader.CreateCell(1).SetCellValue("Ingredients");
            tblHeader.CreateCell(2).SetCellValue("Quantity ");
            tblHeader.CreateCell(3).SetCellValue("Cost/Unit");
            tblHeader.CreateCell(4).SetCellValue("Total Cost ");

            tblHeader.CreateCell(6).SetCellValue("Bread Size/Type");
            tblHeader.CreateCell(7).SetCellValue("Qty Produced");
            tblHeader.CreateCell(8).SetCellValue("Unit Price");
            tblHeader.CreateCell(9).SetCellValue("Total");

            int totalRow = excelmodel.SalesCost.Count > excelmodel.ProductionCost.Count ? excelmodel.SalesCost.Count : excelmodel.ProductionCost.Count;

            for (int i = 0; i < excelmodel.ProductionCost.Count; i++)
            {
                var r = sh.CreateRow(row + i);
                r.CreateCell(1).SetCellValue(excelmodel.ProductionCost[i].ingredentName);
                r.CreateCell(2).SetCellValue(excelmodel.ProductionCost[i].quantity);
                r.CreateCell(3).SetCellValue(excelmodel.ProductionCost[i].UnitCost);
                r.CreateCell(4).SetCellValue(excelmodel.ProductionCost[i].totalCost);
            }

           for (int i = 0; i < excelmodel.SalesCost.Count; i++)
            {
                var r = sh.CreateRow(row + i);
                r.CreateCell(6).SetCellValue(excelmodel.SalesCost[i].ProductName);
                r.CreateCell(7).SetCellValue(excelmodel.SalesCost[i].QuantityProduced);
                r.CreateCell(8).SetCellValue(excelmodel.SalesCost[i].UnitPrice);
                r.CreateCell(9).SetCellValue(excelmodel.SalesCost[i].totalPrice);
            }
            row = row +totalRow;
            var rT = sh.CreateRow(row++);
            if (excelmodel.ProductionCost.Count > 0)
            {
                rT.CreateCell(2).SetCellValue(excelmodel.ProductionCost.Sum(x=>x.quantity));
                rT.CreateCell(3).SetCellValue(excelmodel.ProductionCost.Sum(x => x.UnitCost));
                rT.CreateCell(4).SetCellValue(excelmodel.ProductionCost.Sum(x => x.totalCost)); 
            }

            if (excelmodel.SalesCost.Count > 0)
            {
                rT.CreateCell(7).SetCellValue(excelmodel.SalesCost.Sum(x => x.QuantityProduced));
                rT.CreateCell(8).SetCellValue(excelmodel.SalesCost.Sum(x => x.UnitPrice));
                rT.CreateCell(9).SetCellValue(excelmodel.SalesCost.Sum(x => x.totalPrice));
            }
            row++;
            var dlpOverhead = sh.CreateRow(row++);

            var ntAdministative = excelmodel.OverrheadsCost.Where(x => x.overheadType != OverheadType.Administrative.ToString()).ToList();

            if (ntAdministative.Count > 0)
            {
                for (int i = 0; i < ntAdministative.Count; i++)
                {
                    var ntRow = sh.CreateRow(row++);
                    ntRow.CreateCell(1).SetCellValue(ntAdministative[i].name);
                    ntRow.CreateCell(4).SetCellValue(ntAdministative[i].unitCost); 
                }
            }


            using (var fs = new FileStream(filepart, FileMode.Create, FileAccess.Write))
            {
                wb.Write(fs);
            }

        }

    }
}