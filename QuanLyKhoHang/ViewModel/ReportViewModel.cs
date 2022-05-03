using QuanLyKhoHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Syncfusion.XlsIO;
using System.Drawing;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace QuanLyKhoHang.ViewModel
{
    class ReportViewModel : BaseViewModel
    {
        #region ObservableCollection
        private ObservableCollection<BaoCao> _ListReport;
        public ObservableCollection<BaoCao> ListReport { get => _ListReport; set { _ListReport = value; OnPropertyChanged(); } }

        private ObservableCollection<BaoCaoTong> _ListExtract;
        public ObservableCollection<BaoCaoTong> ListExtract { get => _ListExtract; set { _ListExtract = value; OnPropertyChanged(); } }
        #endregion

        #region Create field 
        private string _IDProduct;
        public string IDProductName { get => _IDProduct; set { _IDProduct = value; OnPropertyChanged(); } }

        private string _NameProduct;
        public string NameProduct { get => _NameProduct; set { _NameProduct = value; OnPropertyChanged(); } }

        private string _NameStorage;
        public string NameStorage { get => _NameStorage; set { _NameStorage = value; OnPropertyChanged(); } }

        private int _TotalInput;
        public int TotalInput { get => _TotalInput; set { _TotalInput = value; OnPropertyChanged(); } }

        private int _TotalOutput;
        public int TotalOutput { get => _TotalOutput; set { _TotalOutput = value; OnPropertyChanged(); } }

        private int _TotalUnit;
        public int TotalUnit { get => _TotalUnit; set { _TotalUnit = value; OnPropertyChanged(); } }

        private DateTime? _DateStart;
        public DateTime? DateStart { get => _DateStart; set { _DateStart = value; OnPropertyChanged(); } }

        private DateTime? _DateEnd;
        public DateTime? DateEnd { get => _DateEnd; set { _DateEnd = value; OnPropertyChanged(); } }

        private int _SelectedSolution;
        public int SelectedSolution { get => _SelectedSolution; set { _SelectedSolution = value; OnPropertyChanged(); } }
        #endregion

        #region ListView's filter
        private string _Filter;
        public string Filter { get => _Filter; set { _Filter = value; OnFilterChanged(); } }

        void FilterListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListReport);
            view.Filter = UserFilter;
        }

        private void OnFilterChanged()
        {
            CollectionViewSource.GetDefaultView(ListReport).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Filter))
                return true;
            else
                return ((item as BaoCao).IDProduct.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as BaoCao).NameProduct.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion

        #region Group in ListView
        void GroupListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListReport);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("NameStorage");
            view.GroupDescriptions.Add(groupDescription);
        }
        #endregion

        #region Load data into ListView
        void LoadDataReport()
        {
            ListReport = new ObservableCollection<BaoCao>();

            var objectList = DataProvider.Ins.DB.Products;
            var storageList = DataProvider.Ins.DB.Storages;
            foreach (var storage in storageList)
            {
                foreach (var item in objectList)
                {
                    BaoCao bc = new BaoCao();
                    bc.IDProduct = item.IdProduct;
                    bc.NameProduct = item.NameProduct;
                    bc.NameStorage = storage.NameStorage;
                    //Total Unit
                    var inputList = DataProvider.Ins.DB.Inputs.Where(x => x.DateInput <= DateEnd);
                    foreach (var input in inputList)
                    {
                        /*if (storage.IdStorage == input.IdStorage)
                        {
                            bc.TotalUnit += (int)DataProvider.Ins.DB.InputInfoes.Where(p => p.IdInput == input.IdInput && p.IdProduct == item.IdProduct).Sum(p => p.Unit);
                            if (input.DateInput >= DateStart)
                            {
                                bc.TotalInput += (int)DataProvider.Ins.DB.InputInfoes.Where(p => p.IdInput == input.IdInput && p.IdProduct == item.IdProduct).Sum(p => p.Unit);
                            }
                        }*/
                        var inputInfoList = DataProvider.Ins.DB.InputInfoes.Where(x => x.IdInput == input.IdInput);
                        foreach(var inputInfo in inputInfoList)
                        {
                            var storageDetailList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == inputInfo.IdConsignment && x.IdStorage == storage.IdStorage && x.IdProduct == item.IdProduct);
                            foreach(var storageDetail in storageDetailList)
                            {
                                bc.TotalUnit += inputInfo.Unit.Value;
                                if(input.DateInput >= DateStart)
                                {
                                    bc.TotalInput += inputInfo.Unit.Value;
                                }
                            }
                        }
                    }
                    var outputList = DataProvider.Ins.DB.Outputs.Where(x => x.DateOutput <= DateEnd);
                    foreach (var output in outputList)
                    {
                        /*if (storage.IdStorage == output.IdStorage)
                        {
                            bc.TotalUnit -= (int)DataProvider.Ins.DB.OutputInfoes.Where(p => p.IdOutput == output.IdOutput && p.IdProduct == item.IdProduct).Sum(p => p.Unit);
                            if (output.DateOutput >= DateStart)
                            {
                                bc.TotalOutput += (int)DataProvider.Ins.DB.OutputInfoes.Where(p => p.IdOutput == output.IdOutput && p.IdProduct == item.IdProduct).Sum(p => p.Unit);
                            }
                        }*/
                        var outputInfoList = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IdOutput == output.IdOutput);
                        foreach (var outputInfo in outputInfoList)
                        {
                            var storageDetailList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == outputInfo.IdConsignment && x.IdStorage == storage.IdStorage && x.IdProduct == item.IdProduct);
                            foreach (var storageDetail in storageDetailList)
                            {
                                bc.TotalUnit -= outputInfo.Unit.Value;
                                if (output.DateOutput >= DateStart)
                                {
                                    bc.TotalOutput += outputInfo.Unit.Value;
                                }
                            }
                        }
                    }
                    ListReport.Add(bc);
                }
            }
            FilterListView();
            GroupListView();
            LoadDataListExtract();
        }
        #endregion

        #region Load data to list. This is used to write to sheet1
        void LoadDataListExtract()
        {
            ListExtract = new ObservableCollection<BaoCaoTong>();

            var objectList = DataProvider.Ins.DB.Products;
            foreach (var item in objectList)
            {
                BaoCaoTong bc = new BaoCaoTong();
                bc.IDProduct = item.IdProduct;
                bc.NameProduct = item.NameProduct;
                foreach(var item1 in ListReport)
                {
                    if(item.IdProduct == item1.IDProduct)
                    {
                        bc.TotalInput += item1.TotalInput;
                        bc.TotalOutput += item1.TotalOutput;
                        bc.TotalUnit += item1.TotalUnit;
                    }
                }
                ListExtract.Add(bc);
            }
        }
        #endregion

        #region (Function) ExtractFileExcel: Create and write to excel file
        void ExtractFileExcel(string filePath)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;
                //Create a workbook
                var storageList = DataProvider.Ins.DB.Storages;
                IWorkbook workbook = application.Workbooks.Create(storageList.Count() + 1);
                SheetAllStorage(workbook, "Tổng các kho", 0);
                int i = 1;
                foreach(var item in storageList)
                {
                    ListExtract.Clear();
                    foreach (var item1 in ListReport)
                    {
                        BaoCaoTong bc = new BaoCaoTong();
                        if (item.NameStorage == item1.NameStorage)
                        {
                            bc.IDProduct = item1.IDProduct;
                            bc.NameProduct = item1.NameProduct;
                            bc.TotalInput += item1.TotalInput;
                            bc.TotalOutput += item1.TotalOutput;
                            bc.TotalUnit += item1.TotalUnit;
                            ListExtract.Add(bc);
                        }
                    }
                    SheetOneStorage(workbook, item.NameStorage, i);
                    i++;
                }
                
                //Save the Excel document
                workbook.SaveAs(filePath);
            }
        }
        #endregion

        #region (Function) Write to excel file
        void SheetAllStorage(IWorkbook workbook, string sheetname, int i)
        {
            IWorksheet worksheet = workbook.Worksheets[i];
            
            workbook.Worksheets[i].Name = sheetname;
            //Disable gridlines in the worksheet
            worksheet.IsGridLinesVisible = false;
            //Enter report's information
            worksheet.Range["A1:F1"].Merge();
            worksheet.Range["A1"].Text = "PHIẾU BÁO CÁO NHẬP-XUẤT-TỒN KHO";
            worksheet.Range["A1"].CellStyle.Font.Bold = true;
            worksheet.Range["A1"].CellStyle.Font.Size = 20;
            worksheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
            worksheet.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            worksheet.Range["A3:C3"].Merge();
            worksheet.Range["A3"].Text = "Thông tin bảng báo cáo";
            worksheet.Range["A3:A8"].CellStyle.Font.Bold = true;
            worksheet.Range["A3"].CellStyle.Color = Color.FromArgb(42, 118, 189);
            worksheet.Range["A3"].CellStyle.Font.Color = ExcelKnownColors.White;

            worksheet.Range["A4"].Text = "Ngày lập:";
            worksheet.Range["B4:C4"].Merge();
            worksheet.Range["B4"].Text = DateTime.Now.ToString();

            worksheet.Range["A6:C6"].Merge();
            worksheet.Range["A6"].Text = "Phạm vi báo cáo";
            worksheet.Range["A6"].CellStyle.Color = Color.FromArgb(42, 118, 189);
            worksheet.Range["A6"].CellStyle.Font.Color = ExcelKnownColors.White;
            worksheet.Range["A7"].Text = "Từ ngày:";
            worksheet.Range["B7:C7"].Merge();
            worksheet.Range["B7"].Text = DateStart.ToString();
            worksheet.Range["A8"].Text = "Đến ngày:";
            worksheet.Range["B8:C8"].Merge();
            worksheet.Range["B8"].Text = DateEnd.ToString();

            //Header Table
            worksheet.Range["A10"].Text = "Mã hàng";
            worksheet.Range["B10"].Text = "Tên hàng";
            worksheet.Range["C10"].Text = "Tổng nhập";
            worksheet.Range["D10"].Text = "Tổng xuất";
            worksheet.Range["E10"].Text = "Tổng tồn";
            worksheet.Range["A10:E10"].CellStyle.Color = Color.FromArgb(42, 118, 189);
            worksheet.Range["A10:E10"].CellStyle.Font.Color = ExcelKnownColors.White;
            worksheet.Range["A10:E10"].CellStyle.Font.Size = 12;
            worksheet.Range["A10:E10"].CellStyle.Font.Bold = true;
            worksheet.Range["A10:E10"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

            //Import data from ListReport to Excel worksheet
            ExcelImportDataOptions importDataOptions = new ExcelImportDataOptions();
            importDataOptions.FirstRow = 11;
            importDataOptions.FirstColumn = 1;
            importDataOptions.IncludeHeader = false;
            worksheet.ImportData(ListExtract, importDataOptions);

            //Apply alignment
            int tmp = 10 + ListExtract.Count;
            string range = "C11:E" + tmp.ToString();
            worksheet.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            //Apply borders
            range = "A11:E" + tmp.ToString();
            worksheet.Range[range].CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[range].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[range].CellStyle.Borders[ExcelBordersIndex.EdgeTop].Color = ExcelKnownColors.Grey_25_percent;
            worksheet.Range[range].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Grey_25_percent;

            //Apply column width and row height
            worksheet.Range["A1"].ColumnWidth = 10;
            worksheet.Range["B1"].ColumnWidth = 15;
            worksheet.Range["C1"].ColumnWidth = 12;
            worksheet.Range["D1"].ColumnWidth = 12;
            worksheet.Range["E1"].ColumnWidth = 12;
            worksheet.Range["A10"].RowHeight = 22;
        }

        void SheetOneStorage(IWorkbook workbook, string sheetname, int i)
        {
            IWorksheet worksheet = workbook.Worksheets[i];

            workbook.Worksheets[i].Name = sheetname;
            //Disable gridlines in the worksheet
            worksheet.IsGridLinesVisible = false;
            //Enter report's information
            worksheet.Range["A1:F1"].Merge();
            worksheet.Range["A1"].Text = sheetname;
            worksheet.Range["A1"].CellStyle.Font.Bold = true;
            worksheet.Range["A1"].CellStyle.Font.Size = 20;
            worksheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(42, 118, 189);
            worksheet.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            //Header Table
            worksheet.Range["A2"].Text = "Mã hàng";
            worksheet.Range["B2"].Text = "Tên hàng";
            worksheet.Range["C2"].Text = "Tổng nhập";
            worksheet.Range["D2"].Text = "Tổng xuất";
            worksheet.Range["E2"].Text = "Tổng tồn";
            worksheet.Range["A2:E2"].CellStyle.Color = Color.FromArgb(42, 118, 189);
            worksheet.Range["A2:E2"].CellStyle.Font.Color = ExcelKnownColors.White;
            worksheet.Range["A2:E2"].CellStyle.Font.Size = 12;
            worksheet.Range["A2:E2"].CellStyle.Font.Bold = true;
            worksheet.Range["A2:E2"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

            //Import data from ListReport to Excel worksheet
            ExcelImportDataOptions importDataOptions = new ExcelImportDataOptions();
            importDataOptions.FirstRow = 3;
            importDataOptions.FirstColumn = 1;
            importDataOptions.IncludeHeader = false;
            worksheet.ImportData(ListExtract, importDataOptions);

            //Apply alignment
            int tmp = 2 + ListExtract.Count;
            string range = "C2:E" + tmp.ToString();
            worksheet.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            //Apply borders
            range = "A2:E" + tmp.ToString();
            worksheet.Range[range].CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[range].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            worksheet.Range[range].CellStyle.Borders[ExcelBordersIndex.EdgeTop].Color = ExcelKnownColors.Grey_25_percent;
            worksheet.Range[range].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Grey_25_percent;

            //Apply column width and row height
            worksheet.Range["A1"].ColumnWidth = 10;
            worksheet.Range["B1"].ColumnWidth = 15;
            worksheet.Range["C1"].ColumnWidth = 12;
            worksheet.Range["D1"].ColumnWidth = 12;
            worksheet.Range["E1"].ColumnWidth = 12;
            worksheet.Range["A2"].RowHeight = 22;
        }
        #endregion

        #region ICommand
        public ICommand ReportCommand { get; set; }
        public ICommand ExtractCommand { get; set; }
        #endregion

        public ReportViewModel()
        {
            #region Report Command
            ReportCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (string.IsNullOrEmpty(DateStart.ToString()) || string.IsNullOrEmpty(DateEnd.ToString()))
                        return false;
                    return true;
                },
                (p)=>
                {
                    LoadDataReport();
                });
            #endregion

            #region Extract Command
            ExtractCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (string.IsNullOrEmpty(DateStart.ToString()) || string.IsNullOrEmpty(DateEnd.ToString()))
                        return false;
                    return true;
                },
                (p) =>
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                if (dialog.ShowDialog() == true)
                    {
                        if (ListReport == null)
                            LoadDataReport();
                        string filePath = dialog.FileName;
                        ExtractFileExcel(filePath);
                        MessageBox.Show("Đã lưu file!");
                    }
                });
            #endregion
        }
    }
}
