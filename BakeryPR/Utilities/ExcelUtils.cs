using BakeryPR.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Linq;
using System.Windows;

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

        private void AddBold(IFont font,int size)
        {
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.FontHeightInPoints = (short)size;
        }

        public void Export(int prod_id, string filepart, ExcelModel excelmodel)
        {
            wb = new XSSFWorkbook();

            // create sheet
            sh = (XSSFSheet)wb.CreateSheet("Sheet1");

            sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 9));
            var header = sh.CreateRow(0);
            ICell cell = header.CreateCell(1);
            cell.SetCellValue("DAILY BAKERY ACTIVITY REGISTER");

            #region header Style
            ICellStyle headerStyle = wb.CreateCellStyle();
            headerStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            IFont font = wb.CreateFont();
            AddBold(font, 15);
            headerStyle.SetFont(font);
            cell.CellStyle = headerStyle; 
            #endregion

            sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 1, 9));
            var productionDateRow = sh.CreateRow(1);

            #region production Date Style
            ICell productionDateCell = productionDateRow.CreateCell(1);
            ICellStyle prodDateStyle = wb.CreateCellStyle();
            prodDateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            IFont fontprodStyle = wb.CreateFont();
            AddBold(fontprodStyle, 13);
            prodDateStyle.SetFont(fontprodStyle);
            productionDateCell.CellStyle = prodDateStyle;
            productionDateCell.SetCellValue($"Production Date: {excelmodel.productionDate}");
            #endregion

            sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 1, 4));
            sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 6, 9));

            sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 1, 4));
            sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 6, 9));

            var tableTitle = sh.CreateRow(2);
            ICellStyle tableTitleStyle = wb.CreateCellStyle();
            tableTitleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            IFont fontcell1 = wb.CreateFont();
            AddBold(fontcell1, 12);
            tableTitleStyle.SetFont(fontcell1);
            
            ICell titleTitleCell1 = tableTitle.CreateCell(1);
            titleTitleCell1.CellStyle = tableTitleStyle;
            titleTitleCell1.SetCellValue("PRODUCTION COST");

            ICell titleTitleCell2 = tableTitle.CreateCell(6);
            titleTitleCell2.CellStyle = tableTitleStyle;
            titleTitleCell2.SetCellValue("SALES/ REVENUE");

            var tableTitle2 = sh.CreateRow(3);
            ICellStyle tableTitleStyle2 = wb.CreateCellStyle();
            tableTitleStyle2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            IFont fontcell2 = wb.CreateFont();
            AddBold(fontcell2, 11);
            tableTitleStyle2.SetFont(fontcell2);

            ICell titleTitleCell3 = tableTitle2.CreateCell(1);
            titleTitleCell3.CellStyle = tableTitleStyle2;
            titleTitleCell3.SetCellValue("Section A: Material Cost");

            ICell titleTitleCell4 = tableTitle2.CreateCell(6);
            titleTitleCell4.CellStyle = tableTitleStyle2;
            titleTitleCell4.SetCellValue("Section D: Sales Value of Production Output");

            int row = 4;
            var tblHeader = sh.CreateRow(row++);
            tblHeader.CreateCell(1).SetCellValue("Ingredients");
            tblHeader.CreateCell(2).SetCellValue("Quantity ");
            tblHeader.CreateCell(3).SetCellValue("Cost/Unit");
            tblHeader.CreateCell(4).SetCellValue("Total Cost ");

            tblHeader.CreateCell(6).SetCellValue("Bread Size/Type");
            tblHeader.CreateCell(7).SetCellValue("Qty Produced");
            tblHeader.CreateCell(8).SetCellValue("Unit Price");
            tblHeader.CreateCell(9).SetCellValue("Total");

            ICellStyle rowStyle = wb.CreateCellStyle();
            IFont fontcell5 = wb.CreateFont();
            AddBold(fontcell5, 10);
            rowStyle.SetFont(fontcell5);
            tblHeader.RowStyle = rowStyle;

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
                MessageBox.Show("Completed");
            }

        }

    }
}