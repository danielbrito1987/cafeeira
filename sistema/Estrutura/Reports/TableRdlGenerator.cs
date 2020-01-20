using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Estrutura.Reports
{
    class TableRdlGenerator
    {
        private List<string> m_fields;

        public List<string> Fields
        {
            get { return m_fields; }
            set { m_fields = value; }
        }

        public TableType CreateTable()
        {
            TableType table = new TableType();
            table.Name = "Table1";
            table.Items = new object[]
                {
                    CreateTableColumns(),
                    CreateHeader(),
                    CreateDetails(),
                };
            table.ItemsElementName = new ItemsChoiceType21[]
                {
                    ItemsChoiceType21.TableColumns,
                    ItemsChoiceType21.Header,
                    ItemsChoiceType21.Details,
                };
            return table;
        }

        private HeaderType CreateHeader()
        {
            HeaderType header = new HeaderType();
            header.Items = new object[]
                {
                    CreateHeaderTableRows(),
                };
            header.ItemsElementName = new ItemsChoiceType20[]
                {
                    ItemsChoiceType20.TableRows,
                };
            return header;
        }

        private TableRowsType CreateHeaderTableRows()
        {
            TableRowsType headerTableRows = new TableRowsType();
            headerTableRows.TableRow = new TableRowType[] { CreateHeaderTableRow() };
            return headerTableRows;
        }

        private TableRowType CreateHeaderTableRow()
        {
            TableRowType headerTableRow = new TableRowType();
            headerTableRow.Items = new object[] { CreateHeaderTableCells(), "0.25in" };
            return headerTableRow;
        }

        private TableCellsType CreateHeaderTableCells()
        {
            TableCellsType headerTableCells = new TableCellsType();
            headerTableCells.TableCell = new TableCellType[m_fields.Count];
            for (int i = 0; i < m_fields.Count; i++)
            {
                headerTableCells.TableCell[i] = CreateHeaderTableCell(m_fields[i]);
            }
            return headerTableCells;
        }

        private TableCellType CreateHeaderTableCell(string fieldName)
        {
            TableCellType headerTableCell = new TableCellType();
            headerTableCell.Items = new object[] { CreateHeaderTableCellReportItems(fieldName) };
            return headerTableCell;
        }

        private ReportItemsType CreateHeaderTableCellReportItems(string fieldName)
        {
            ReportItemsType headerTableCellReportItems = new ReportItemsType();
            headerTableCellReportItems.Items = new object[] { CreateHeaderTableCellTextbox(fieldName) };
            return headerTableCellReportItems;
        }

        private TextboxType CreateHeaderTableCellTextbox(string fieldName)
        {
            TextboxType headerTableCellTextbox = new TextboxType();
            headerTableCellTextbox.Name = fieldName + "_Header";
            headerTableCellTextbox.Items = new object[] 
                {
                    fieldName,
                    CreateHeaderTableCellTextboxStyle(),
                    true,
                };
            headerTableCellTextbox.ItemsElementName = new ItemsChoiceType14[] 
                {
                    ItemsChoiceType14.Value,
                    ItemsChoiceType14.Style,
                    ItemsChoiceType14.CanGrow,
                };
            return headerTableCellTextbox;
        }

        private StyleType CreateHeaderTableCellTextboxStyle()
        {
            StyleType headerTableCellTextboxStyle = new StyleType();
            headerTableCellTextboxStyle.Items = new object[]
                {
                    "700",
                    "14pt",
                };
            headerTableCellTextboxStyle.ItemsElementName = new ItemsChoiceType5[]
                {
                    ItemsChoiceType5.FontWeight,
                    ItemsChoiceType5.FontSize,
                };
            return headerTableCellTextboxStyle;
        }

        private DetailsType CreateDetails()
        {
            DetailsType details = new DetailsType();
            details.Items = new object[] { CreateTableRows() };
            return details;
        }

        private TableRowsType CreateTableRows()
        {
            TableRowsType tableRows = new TableRowsType();
            tableRows.TableRow = new TableRowType[] { CreateTableRow() };
            return tableRows;
        }

        private TableRowType CreateTableRow()
        {
            TableRowType tableRow = new TableRowType();
            tableRow.Items = new object[] { CreateTableCells(), "0.25in" };
            return tableRow;
        }

        private TableCellsType CreateTableCells()
        {
            TableCellsType tableCells = new TableCellsType();
            tableCells.TableCell = new TableCellType[m_fields.Count];
            for (int i = 0; i < m_fields.Count; i++)
            {
                tableCells.TableCell[i] = CreateTableCell(m_fields[i]);
            }
            return tableCells;
        }

        private TableCellType CreateTableCell(string fieldName)
        {
            TableCellType tableCell = new TableCellType();
            tableCell.Items = new object[] { CreateTableCellReportItems(fieldName) };
            return tableCell;
        }

        private ReportItemsType CreateTableCellReportItems(string fieldName)
        {
            ReportItemsType reportItems = new ReportItemsType();
            reportItems.Items = new object[] { CreateTableCellTextbox(fieldName) };
            return reportItems;
        }

        private TextboxType CreateTableCellTextbox(string fieldName)
        {
            TextboxType textbox = new TextboxType();
            textbox.Name = fieldName;
            textbox.Items = new object[] 
                {
                    "=Fields!" + fieldName + ".Value",
                    CreateTableCellTextboxStyle(),
                    true,
                };
            textbox.ItemsElementName = new ItemsChoiceType14[] 
                {
                    ItemsChoiceType14.Value,
                    ItemsChoiceType14.Style,
                    ItemsChoiceType14.CanGrow,
                };
            return textbox;
        }

        private StyleType CreateTableCellTextboxStyle()
        {
            StyleType style = new StyleType();
            style.Items = new object[]
                {
                    "=iif(RowNumber(Nothing) mod 2, \"AliceBlue\", \"White\")",
                    "Left",
                };
            style.ItemsElementName = new ItemsChoiceType5[]
                {
                    ItemsChoiceType5.BackgroundColor,
                    ItemsChoiceType5.TextAlign,
                };
            return style;
        }

        private TableColumnsType CreateTableColumns()
        {
            TableColumnsType tableColumns = new TableColumnsType();
            tableColumns.TableColumn = new TableColumnType[m_fields.Count];
            for (int i = 0; i < m_fields.Count; i++)
            {
                tableColumns.TableColumn[i] = CreateTableColumn();
            }
            return tableColumns;
        }

        private TableColumnType CreateTableColumn()
        {
            TableColumnType tableColumn = new TableColumnType();
            tableColumn.Items = new object[] { "2in" };
            return tableColumn;
        }
    }
}
