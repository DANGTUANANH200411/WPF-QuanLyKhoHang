using QuanLyKhoHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace QuanLyKhoHang.ViewModel
{
    public class OutputProductViewModel : BaseViewModel
    {
        #region Đỗ dữ liệu vào ListView
        private ObservableCollection<ThongTinXuatKho> _ListOutputInfo;
        public ObservableCollection<ThongTinXuatKho> ListOutputInfo { get => _ListOutputInfo; set { _ListOutputInfo = value; OnPropertyChanged(); } }

        void LoadDataOutputInfo()
        {
            ListOutputInfo = new ObservableCollection<ThongTinXuatKho>();
            var outputList = DataProvider.Ins.DB.Outputs;
            foreach (var output in outputList)
            {
                ThongTinXuatKho outputInfo = new ThongTinXuatKho();
                outputInfo.Output = output;
                var customer = DataProvider.Ins.DB.Customers.Where(x => x.IdCustomer == output.IdCustomer).SingleOrDefault();
                outputInfo.NameCustomer = customer.NameCustomer;
                ListOutputInfo.Add(outputInfo);
            }
        }
        #endregion

        #region Bộ lọc trong ListView
        private string _Filter;
        public string Filter { get => _Filter; set { _Filter = value; OnFilterChanged(); } }

        void FilterListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOutputInfo);
            view.Filter = UserFilter;
        }

        private void OnFilterChanged()
        {
            CollectionViewSource.GetDefaultView(ListOutputInfo).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Filter))
                return true;
            else
                return ((item as ThongTinXuatKho).NameCustomer.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as ThongTinXuatKho).Output.IdOutput.ToString().IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as ThongTinXuatKho).Output.DateOutput.ToString().IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion

        #region Chọn dữ liệu dưới ListView binding đến các Cart
        private ObservableCollection<Product> _ProductList1;
        public ObservableCollection<Product> ProductList1 { get => _ProductList1; set { _ProductList1 = value; OnPropertyChanged(); } }
        private ObservableCollection<Product> _ProductList2;
        public ObservableCollection<Product> ProductList2 { get => _ProductList2; set { _ProductList2 = value; OnPropertyChanged(); } }
        private ObservableCollection<Product> _ProductList3;
        public ObservableCollection<Product> ProductList3 { get => _ProductList3; set { _ProductList3 = value; OnPropertyChanged(); } }
        private string _Product1;
        public string Product1
        {
            get => _Product1; set
            {
                _Product1 = value; OnPropertyChanged();
                // lọc kho chứa sản phẩm
                StorageList1 = new ObservableCollection<Storage>();
                if (!string.IsNullOrEmpty(Product1))
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdProduct == Product1);
                    foreach (var item in consignmentList)
                    {
                        var storage = DataProvider.Ins.DB.Storages.Where(x => x.IdStorage == item.IdStorage).SingleOrDefault();
                        if (!StorageList1.Contains(storage)) StorageList1.Add(storage);
                    }
                }
            }
        }
        private string _Product2;
        public string Product2
        {
            get => _Product2; set
            {
                _Product2 = value; OnPropertyChanged();
                // lọc kho chứa sản phẩm
                StorageList2 = new ObservableCollection<Storage>();
                if (!string.IsNullOrEmpty(Product2))
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdProduct == Product2);
                    foreach (var item in consignmentList)
                    {
                        var storage = DataProvider.Ins.DB.Storages.Where(x => x.IdStorage == item.IdStorage).SingleOrDefault();
                        if (!StorageList2.Contains(storage)) StorageList2.Add(storage);
                    }
                }
            }
        }
        private string _Product3;
        public string Product3
        {
            get => _Product3; set
            {
                _Product3 = value; OnPropertyChanged();
                // lọc kho chứa sản phẩm
                StorageList3 = new ObservableCollection<Storage>();
                if (!string.IsNullOrEmpty(Product3))
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdProduct == Product3);
                    foreach (var item in consignmentList)
                    {
                        var storage = DataProvider.Ins.DB.Storages.Where(x => x.IdStorage == item.IdStorage).SingleOrDefault();
                        if (!StorageList3.Contains(storage)) StorageList3.Add(storage);
                    }
                }
            }
        }
        // 3 Kho chứa
        private ObservableCollection<Storage> _StorageList1;
        public ObservableCollection<Storage> StorageList1 { get => _StorageList1; set { _StorageList1 = value; OnPropertyChanged(); } }
        private ObservableCollection<Storage> _StorageList2;
        public ObservableCollection<Storage> StorageList2 { get => _StorageList2; set { _StorageList2 = value; OnPropertyChanged(); } }
        private ObservableCollection<Storage> _StorageList3;
        public ObservableCollection<Storage> StorageList3 { get => _StorageList3; set { _StorageList3 = value; OnPropertyChanged(); } }
        private int _Storage1;
        public int Storage1
        {
            get => _Storage1; set
            {
                _Storage1 = value; OnPropertyChanged();
                // duyệt nhân viên theo kho và theo rank
                EmployeeList1 = new ObservableCollection<Employee>();
                if (Storage1 != 0)
                {
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == Storage1 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        EmployeeList1.Add(item);
                    }
                }
                // duyệt lô hàng theo kho và theo sản phẩm
                ConsignmentList1 = new ObservableCollection<StorageDetail>();
                if (Storage1 != 0)
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdStorage == Storage1 && x.IdProduct == Product1);
                    foreach (var item in consignmentList)
                    {
                        if (item.IdConsignment != Consignment2 && item.IdConsignment != Consignment3)
                        {
                            ConsignmentList1.Add(item);
                        }
                    }
                }
            }
        }
        private int _Storage2;
        public int Storage2
        {
            get => _Storage2; set
            {
                _Storage2 = value; OnPropertyChanged();
                // duyệt nhân viên theo kho và theo rank
                EmployeeList2 = new ObservableCollection<Employee>();
                if (Storage2 != 0)
                {
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == Storage2 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        EmployeeList2.Add(item);
                    }
                }
                // duyệt lô hàng theo kho và theo sản phẩm
                ConsignmentList2 = new ObservableCollection<StorageDetail>();
                if (Storage2 != 0)
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdStorage == Storage2 && x.IdProduct == Product2);
                    foreach (var item in consignmentList)
                    {
                        if (item.IdConsignment != Consignment1 && item.IdConsignment != Consignment3)
                        {
                            ConsignmentList2.Add(item);
                        }
                    }
                }
            }
        }
        private int _Storage3;
        public int Storage3
        {
            get => _Storage3; set
            {
                _Storage3 = value; OnPropertyChanged();
                // duyệt nhân viên theo kho và theo rank
                EmployeeList3 = new ObservableCollection<Employee>();
                if (Storage3 != 0)
                {
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == Storage3 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        EmployeeList3.Add(item);
                    }
                }
                // duyệt lô hàng theo kho và theo sản phẩm
                ConsignmentList3 = new ObservableCollection<StorageDetail>();
                if (Storage3 != 0)
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdStorage == Storage3 && x.IdProduct == Product3);
                    foreach (var item in consignmentList)
                    {
                        if (item.IdConsignment != Consignment1 && item.IdConsignment != Consignment2)
                        {
                            ConsignmentList3.Add(item);
                        }
                    }
                }
            }
        }
        private string _TextStorage1;
        public string TextStorage1 { get => _TextStorage1; set { _TextStorage1 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextStorage1)) Storage1 = 0; } }
        private string _TextStorage2;
        public string TextStorage2 { get => _TextStorage2; set { _TextStorage2 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextStorage2)) Storage2 = 0; } }
        private string _TextStorage3;
        public string TextStorage3 { get => _TextStorage3; set { _TextStorage3 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextStorage3)) Storage3 = 0; } }
        // 3 lô hàng
        private ObservableCollection<StorageDetail> _ConsignmentList1;
        public ObservableCollection<StorageDetail> ConsignmentList1 { get => _ConsignmentList1; set { _ConsignmentList1 = value; OnPropertyChanged(); } }
        private ObservableCollection<StorageDetail> _ConsignmentList2;
        public ObservableCollection<StorageDetail> ConsignmentList2 { get => _ConsignmentList2; set { _ConsignmentList2 = value; OnPropertyChanged(); } }
        private ObservableCollection<StorageDetail> _ConsignmentList3;
        public ObservableCollection<StorageDetail> ConsignmentList3 { get => _ConsignmentList3; set { _ConsignmentList3 = value; OnPropertyChanged(); } }
        private int _Consignment1;
        public int Consignment1 { get => _Consignment1; set { _Consignment1 = value; OnPropertyChanged(); } }
        private int _Consignment2;
        public int Consignment2 { get => _Consignment2; set { _Consignment2 = value; OnPropertyChanged(); } }
        private int _Consignment3;
        public int Consignment3 { get => _Consignment3; set { _Consignment3 = value; OnPropertyChanged(); } }
        private string _TextConsignment1;
        public string TextConsignment1 { get => _TextConsignment1; set { _TextConsignment1 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextConsignment1)) Consignment1 = 0; } }
        private string _TextConsignment2;
        public string TextConsignment2 { get => _TextConsignment2; set { _TextConsignment2 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextConsignment2)) Consignment2 = 0; } }
        private string _TextConsignment3;
        public string TextConsignment3 { get => _TextConsignment3; set { _TextConsignment3 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextConsignment3)) Consignment3 = 0; } }
        // 3 nhân viên
        private ObservableCollection<Employee> _EmployeeList1;
        public ObservableCollection<Employee> EmployeeList1 { get => _EmployeeList1; set { _EmployeeList1 = value; OnPropertyChanged(); } }
        private ObservableCollection<Employee> _EmployeeList2;
        public ObservableCollection<Employee> EmployeeList2 { get => _EmployeeList2; set { _EmployeeList2 = value; OnPropertyChanged(); } }
        private ObservableCollection<Employee> _EmployeeList3;
        public ObservableCollection<Employee> EmployeeList3 { get => _EmployeeList3; set { _EmployeeList3 = value; OnPropertyChanged(); } }
        private int _Employee1;
        public int Employee1 { get => _Employee1; set { _Employee1 = value; OnPropertyChanged(); } }
        private int _Employee2;
        public int Employee2 { get => _Employee2; set { _Employee2 = value; OnPropertyChanged(); } }
        private int _Employee3;
        public int Employee3 { get => _Employee3; set { _Employee3 = value; OnPropertyChanged(); } }
        private string _TextEmployee1;
        public string TextEmployee1 { get => _TextEmployee1; set { _TextEmployee1 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextEmployee1)) Employee1 = 0; } }
        private string _TextEmployee2;
        public string TextEmployee2 { get => _TextEmployee2; set { _TextEmployee2 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextEmployee2)) Employee2 = 0; } }
        private string _TextEmployee3;
        public string TextEmployee3 { get => _TextEmployee3; set { _TextEmployee3 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextEmployee3)) Employee3 = 0; } }
        // 3 số lượng
        private Nullable<int> Unit1;
        private string _TextUnit1;
        public string TextUnit1 { get => _TextUnit1; set { _TextUnit1 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextUnit1)) Unit1 = 0; else Unit1 = Int16.Parse(TextUnit1); } }
        private Nullable<int> Unit2;
        private string _TextUnit2;
        public string TextUnit2 { get => _TextUnit2; set { _TextUnit2 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextUnit2)) Unit2 = 0; else Unit2 = Int16.Parse(TextUnit2); } }
        private Nullable<int> Unit3;
        private string _TextUnit3;
        public string TextUnit3 { get => _TextUnit3; set { _TextUnit3 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TextUnit3)) Unit3 = 0; else Unit3 = Int16.Parse(TextUnit3); } }
        // 3 ngày sản xuất
        private Nullable<DateTime> _MafDate1;
        public Nullable<DateTime> MafDate1 { get => _MafDate1; set { _MafDate1 = value; OnPropertyChanged(); } }
        private Nullable<DateTime> _MafDate2;
        public Nullable<DateTime> MafDate2 { get => _MafDate2; set { _MafDate2 = value; OnPropertyChanged(); } }
        private Nullable<DateTime> _MafDate3;
        public Nullable<DateTime> MafDate3 { get => _MafDate3; set { _MafDate3 = value; OnPropertyChanged(); } }
        // 3 hạn sử dụng
        private Nullable<DateTime> _ExpDate1;
        public Nullable<DateTime> ExpDate1 { get => _ExpDate1; set { _ExpDate1 = value; OnPropertyChanged(); } }
        private Nullable<DateTime> _ExpDate2;
        public Nullable<DateTime> ExpDate2 { get => _ExpDate2; set { _ExpDate2 = value; OnPropertyChanged(); } }
        private Nullable<DateTime> _ExpDate3;
        public Nullable<DateTime> ExpDate3 { get => _ExpDate3; set { _ExpDate3 = value; OnPropertyChanged(); } }
        // chọn item dưới ListView
        private ThongTinXuatKho _SelectedItem;
        public ThongTinXuatKho SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    // Xóa các dữ liệu trước đó
                    Consignment1 = Consignment2 = Consignment3 = 0;
                    Product1 = Product2 = Product3 = _CheckProduct1 = _CheckProduct2 = _CheckProduct3 = "";
                    _CheckUnit1 = _CheckUnit2 = _CheckUnit3 = null;
                    TextUnit1 = TextUnit2 = TextUnit3 = "";
                    Storage1 = Storage2 = Storage3 = _CheckStorage1 = _CheckStorage2 = _CheckStorage3 = 0;
                    MafDate1 = MafDate2 = MafDate3 = null;
                    ExpDate1 = ExpDate2 = ExpDate3 = null;
                    Employee1 = Employee2 = Employee3 = _CheckEmployee1 = _CheckEmployee2 = _CheckEmployee3 = 0;
                    // load dữ liệu lên combobox
                    ProductList1 = new ObservableCollection<Product>();
                    var productList = DataProvider.Ins.DB.Products;
                    foreach (var item in productList)
                    {
                        ProductList1.Add(item);
                    }
                    //binding
                    List<LoHangNhapXuat> list = new List<LoHangNhapXuat>();
                    var listOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IdOutput == SelectedItem.Output.IdOutput);
                    foreach (var item in listOutputInfo)
                    {
                        var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == item.IdConsignment).SingleOrDefault();
                        list.Add(new LoHangNhapXuat() { Consignment = consignment, Employee = item.IdEmployee, Unit = item.Unit });
                    }
                    // đổ lên Cart số 1
                    Product1 = _CheckProduct1 = list.ElementAt(0).Consignment.IdProduct;
                    Storage1 = _CheckStorage1 = list.ElementAt(0).Consignment.IdStorage;
                    Employee1 = _CheckEmployee1 = list.ElementAt(0).Employee;
                    Consignment1 = _CheckConsignment1 = list.ElementAt(0).Consignment.IdConsignment;
                    TextUnit1 = list.ElementAt(0).Unit.ToString();
                    _CheckUnit1 = list.ElementAt(0).Unit;
                    MafDate1 = list.ElementAt(0).Consignment.MafDate;
                    ExpDate1 = list.ElementAt(0).Consignment.ExpDate;
                    // đổ lên Cart số 2, 3 nếu có
                    if (list.Count() > 1)
                    {
                        // load dữ liệu lên combobox
                        ProductList2 = new ObservableCollection<Product>();
                        var productList2 = DataProvider.Ins.DB.Products;
                        foreach (var item in productList2)
                        {
                            ProductList2.Add(item);
                        }
                        Product2 = _CheckProduct2 = list.ElementAt(1).Consignment.IdProduct;
                        Storage2 = _CheckStorage2 = list.ElementAt(1).Consignment.IdStorage;
                        Employee2 = _CheckEmployee2 = list.ElementAt(1).Employee;
                        Consignment2 = _CheckConsignment2 = list.ElementAt(1).Consignment.IdConsignment;
                        TextUnit2 = list.ElementAt(1).Unit.ToString();
                        _CheckUnit2 = list.ElementAt(1).Unit;
                        MafDate2 = list.ElementAt(1).Consignment.MafDate;
                        ExpDate2 = list.ElementAt(1).Consignment.ExpDate;
                    }
                    if (list.Count() > 2)
                    {
                        // load dữ liệu lên combobox
                        ProductList3 = new ObservableCollection<Product>();
                        var productList3 = DataProvider.Ins.DB.Products;
                        foreach (var item in productList3)
                        {
                            ProductList3.Add(item);
                        }
                        Product3 = _CheckProduct3 = list.ElementAt(2).Consignment.IdProduct;
                        Storage3 = _CheckStorage3 = list.ElementAt(2).Consignment.IdStorage;
                        Employee3 = _CheckEmployee3 = list.ElementAt(2).Employee;
                        Consignment3 = _CheckConsignment3 = list.ElementAt(2).Consignment.IdConsignment;
                        TextUnit3 = list.ElementAt(2).Unit.ToString();
                        _CheckUnit3 = list.ElementAt(2).Unit;
                        MafDate3 = list.ElementAt(2).Consignment.MafDate;
                        ExpDate3 = list.ElementAt(2).Consignment.ExpDate;
                    }
                }
            }
        }
        #endregion

        #region Ba nút Sửa
        public ICommand EditCommand1 { get; set; }
        public ICommand EditCommand2 { get; set; }
        public ICommand EditCommand3 { get; set; }
        // các biến lưu lại dữ liệu ban đầu
        private string _CheckProduct1;
        private string _CheckProduct2;
        private string _CheckProduct3;
        private Nullable<int> _CheckUnit1;
        private Nullable<int> _CheckUnit2;
        private Nullable<int> _CheckUnit3;
        private int _CheckStorage1;
        private int _CheckStorage2;
        private int _CheckStorage3;
        private int _CheckEmployee1;
        private int _CheckEmployee2;
        private int _CheckEmployee3;
        private int _CheckConsignment1;
        private int _CheckConsignment2;
        private int _CheckConsignment3;
        #endregion

        #region Nút xóa
        public ICommand DeleteCommand { get; set; }
        void LoadCart()
        {
            ProductList1 = ProductList2 = ProductList3 = new ObservableCollection<Product>();
            MafDate1 = MafDate2 = MafDate3 = ExpDate1 = ExpDate2 = ExpDate3 = null;
            TextUnit1 = TextUnit2 = TextUnit3 = null;
        }
        #endregion

        #region Đổ dữ liệu vào các mục Xuất kho và nút Xuất
        // Khách hàng
        private ObservableCollection<Customer> _ListCustomer;
        public ObservableCollection<Customer> ListCustomer { get => _ListCustomer; set { _ListCustomer = value; OnPropertyChanged(); } }
        private int _NewCustomer;
        public int NewCustomer { get => _NewCustomer; set { _NewCustomer = value; OnPropertyChanged(); } }
        private string _TestCustomer;
        public string TestCustomer { get => _TestCustomer; set { _TestCustomer = value; OnPropertyChanged(); if (string.IsNullOrEmpty(_TestCustomer)) NewCustomer = 0; } }
        // 3 Sản phẩm
        private ObservableCollection<Product> _ListProduct;
        public ObservableCollection<Product> ListProduct { get => _ListProduct; set { _ListProduct = value; OnPropertyChanged(); } }
        private string _NewProduct1;
        public string NewProduct1
        {
            get => _NewProduct1; set
            {
                _NewProduct1 = value; OnPropertyChanged();
                // lọc kho chứa sản phẩm
                ListStorage1 = new ObservableCollection<Storage>();
                if (!string.IsNullOrEmpty(NewProduct1))
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdProduct == NewProduct1);
                    foreach (var item in consignmentList)
                    {
                        var storage = DataProvider.Ins.DB.Storages.Where(x => x.IdStorage == item.IdStorage).SingleOrDefault();
                        if (!ListStorage1.Contains(storage)) ListStorage1.Add(storage);
                    }
                }
            }
        }
        private string _NewProduct2;
        public string NewProduct2
        {
            get => _NewProduct2; set
            {
                _NewProduct2 = value; OnPropertyChanged();
                // lọc kho chứa sản phẩm
                ListStorage2 = new ObservableCollection<Storage>();
                if (!string.IsNullOrEmpty(NewProduct2))
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdProduct == NewProduct2);
                    foreach (var item in consignmentList)
                    {
                        var storage = DataProvider.Ins.DB.Storages.Where(x => x.IdStorage == item.IdStorage).SingleOrDefault();
                        if (!ListStorage2.Contains(storage)) ListStorage2.Add(storage);
                    }
                }
            }
        }
        private string _NewProduct3;
        public string NewProduct3
        {
            get => _NewProduct3; set
            {
                _NewProduct3 = value; OnPropertyChanged();
                // lọc kho chứa sản phẩm
                ListStorage3 = new ObservableCollection<Storage>();
                if (!string.IsNullOrEmpty(NewProduct3))
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdProduct == NewProduct3);
                    foreach (var item in consignmentList)
                    {
                        var storage = DataProvider.Ins.DB.Storages.Where(x => x.IdStorage == item.IdStorage).SingleOrDefault();
                        if (!ListStorage3.Contains(storage)) ListStorage3.Add(storage);
                    }
                }
            }
        }
        // 3 Kho chứa
        private ObservableCollection<Storage> _ListStorage1;
        public ObservableCollection<Storage> ListStorage1 { get => _ListStorage1; set { _ListStorage1 = value; OnPropertyChanged(); } }
        private ObservableCollection<Storage> _ListStorage2;
        public ObservableCollection<Storage> ListStorage2 { get => _ListStorage2; set { _ListStorage2 = value; OnPropertyChanged(); } }
        private ObservableCollection<Storage> _ListStorage3;
        public ObservableCollection<Storage> ListStorage3 { get => _ListStorage3; set { _ListStorage3 = value; OnPropertyChanged(); } }
        private string _TestStorage1;
        public string TestStorage1 { get => _TestStorage1; set { _TestStorage1 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestStorage1)) NewStorage1 = 0; } }
        private int _NewStorage1;
        public int NewStorage1
        {
            get => _NewStorage1; set
            {
                _NewStorage1 = value; OnPropertyChanged();
                // duyệt nhân viên theo kho và theo rank
                ListEmployee1 = new ObservableCollection<Employee>();
                if (NewStorage1 != 0)
                {
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == NewStorage1 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        ListEmployee1.Add(item);
                    }
                }
                // duyệt lô hàng theo kho và theo sản phẩm
                ListConsignment1 = new ObservableCollection<StorageDetail>();
                if (NewStorage1 != 0)
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdStorage == NewStorage1 && x.IdProduct == NewProduct1);
                    foreach (var item in consignmentList)
                    {
                        if (item.IdConsignment != NewConsignment2 && item.IdConsignment != NewConsignment3)
                        {
                            ListConsignment1.Add(item);
                        }
                    }
                }
            }
        }
        private string _TestStorage2;
        public string TestStorage2 { get => _TestStorage2; set { _TestStorage2 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestStorage2)) NewStorage2 = 0; } }
        private int _NewStorage2;
        public int NewStorage2
        {
            get => _NewStorage2; set
            {
                _NewStorage2 = value; OnPropertyChanged();
                // duyệt nhân viên theo kho và theo rank
                ListEmployee2 = new ObservableCollection<Employee>();
                if (NewStorage2 != 0)
                {
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == NewStorage2 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        ListEmployee2.Add(item);
                    }
                }
                // duyệt lô hàng theo kho và theo sản phẩm
                ListConsignment2 = new ObservableCollection<StorageDetail>();
                if (NewStorage2 != 0)
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdStorage == NewStorage2 && x.IdProduct == NewProduct2);
                    foreach (var item in consignmentList)
                    {
                        if (item.IdConsignment != NewConsignment1 && item.IdConsignment != NewConsignment3)
                        {
                            ListConsignment2.Add(item);
                        }
                    }
                }
            }
        }
        private string _TestStorage3;
        public string TestStorage3 { get => _TestStorage3; set { _TestStorage3 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestStorage3)) NewStorage3 = 0; } }
        private int _NewStorage3;
        public int NewStorage3
        {
            get => _NewStorage3; set
            {
                _NewStorage3 = value; OnPropertyChanged();
                // duyệt nhân viên theo kho và theo rank
                ListEmployee3 = new ObservableCollection<Employee>();
                if (NewStorage3 != 0)
                {
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == NewStorage3 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        ListEmployee3.Add(item);
                    }
                }
                // duyệt lô hàng theo kho và theo sản phẩm
                ListConsignment3 = new ObservableCollection<StorageDetail>();
                if (NewStorage3 != 0)
                {
                    var consignmentList = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdStorage == NewStorage3 && x.IdProduct == NewProduct3);
                    foreach (var item in consignmentList)
                    {
                        if (item.IdConsignment != NewConsignment1 && item.IdConsignment != NewConsignment2)
                        {
                            ListConsignment3.Add(item);
                        }
                    }
                }
            }
        }
        // 3 lô hàng
        private ObservableCollection<StorageDetail> _ListConsignment1;
        public ObservableCollection<StorageDetail> ListConsignment1 { get => _ListConsignment1; set { _ListConsignment1 = value; OnPropertyChanged(); } }
        private ObservableCollection<StorageDetail> _ListConsignment2;
        public ObservableCollection<StorageDetail> ListConsignment2 { get => _ListConsignment2; set { _ListConsignment2 = value; OnPropertyChanged(); } }
        private ObservableCollection<StorageDetail> _ListConsignment3;
        public ObservableCollection<StorageDetail> ListConsignment3 { get => _ListConsignment3; set { _ListConsignment3 = value; OnPropertyChanged(); } }
        private int _NewConsignment1;
        public int NewConsignment1
        {
            get => _NewConsignment1; set
            {
                _NewConsignment1 = value; OnPropertyChanged();
                if (NewConsignment1 != 0)
                {
                    var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == NewConsignment1).SingleOrDefault();
                    UnitConsignment1 = consignment.Unit;
                    NewMafDate1 = consignment.MafDate;
                    NewExpDate1 = consignment.ExpDate;
                }
                else
                {
                    NewMafDate1 = NewExpDate1 = null;
                    UnitConsignment1 = null;
                }
            }
        }
        private string _TestConsignment1;
        public string TestConsignment1 { get => _TestConsignment1; set { _TestConsignment1 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestConsignment1)) NewConsignment1 = 0; } }
        private int _NewConsignment2;
        public int NewConsignment2
        {
            get => _NewConsignment2; set
            {
                _NewConsignment2 = value; OnPropertyChanged();
                if (NewConsignment2 != 0)
                {
                    var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == NewConsignment2).SingleOrDefault();
                    UnitConsignment2 = consignment.Unit;
                    NewMafDate2 = consignment.MafDate;
                    NewExpDate2 = consignment.ExpDate;
                }
                else
                {
                    NewMafDate2 = NewExpDate2 = null;
                    UnitConsignment2 = null;
                }
            }
        }
        private string _TestConsignment2;
        public string TestConsignment2 { get => _TestConsignment2; set { _TestConsignment2 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestConsignment2)) NewConsignment2 = 0; } }
        private int _NewConsignment3;
        public int NewConsignment3
        {
            get => _NewConsignment3; set
            {
                _NewConsignment3 = value; OnPropertyChanged();
                if (NewConsignment3 != 0)
                {
                    var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == NewConsignment3).SingleOrDefault();
                    UnitConsignment3 = consignment.Unit;
                    NewMafDate3 = consignment.MafDate;
                    NewExpDate3 = consignment.ExpDate;
                }
                else
                {
                    NewMafDate3 = NewExpDate3 = null;
                    UnitConsignment3 = null;
                }
            }
        }
        private string _TestConsignment3;
        public string TestConsignment3 { get => _TestConsignment3; set { _TestConsignment3 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestConsignment3)) NewConsignment3 = 0; } }
        // 3 nhân viên
        private ObservableCollection<Employee> _ListEmployee1;
        public ObservableCollection<Employee> ListEmployee1 { get => _ListEmployee1; set { _ListEmployee1 = value; OnPropertyChanged(); } }
        private ObservableCollection<Employee> _ListEmployee2;
        public ObservableCollection<Employee> ListEmployee2 { get => _ListEmployee2; set { _ListEmployee2 = value; OnPropertyChanged(); } }
        private ObservableCollection<Employee> _ListEmployee3;
        public ObservableCollection<Employee> ListEmployee3 { get => _ListEmployee3; set { _ListEmployee3 = value; OnPropertyChanged(); } }
        private int _NewEmployee1;
        public int NewEmployee1 { get => _NewEmployee1; set { _NewEmployee1 = value; OnPropertyChanged(); } }
        private string _TestEmployee1;
        public string TestEmployee1 { get => _TestEmployee1; set { _TestEmployee1 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestEmployee1)) NewEmployee1 = 0; } }
        private int _NewEmployee2 = 0;
        public int NewEmployee2 { get => _NewEmployee2; set { _NewEmployee2 = value; OnPropertyChanged(); } }
        private string _TestEmployee2;
        public string TestEmployee2 { get => _TestEmployee2; set { _TestEmployee2 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestEmployee2)) NewEmployee2 = 0; } }
        private int _NewEmployee3 = 0;
        public int NewEmployee3 { get => _NewEmployee3; set { _NewEmployee3 = value; OnPropertyChanged(); } }
        private string _TestEmployee3;
        public string TestEmployee3 { get => _TestEmployee3; set { _TestEmployee3 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestEmployee3)) NewEmployee3 = 0; } }
        // 3 số lượng
        private Nullable<int> _UnitConsignment1;
        public Nullable<int> UnitConsignment1 { get => _UnitConsignment1; set { _UnitConsignment1 = value; OnPropertyChanged(); } }
        private Nullable<int> _UnitConsignment2;
        public Nullable<int> UnitConsignment2 { get => _UnitConsignment2; set { _UnitConsignment2 = value; OnPropertyChanged(); } }
        private Nullable<int> _UnitConsignment3;
        public Nullable<int> UnitConsignment3 { get => _UnitConsignment3; set { _UnitConsignment3 = value; OnPropertyChanged(); } }
        private string _TestUnit1;
        public string TestUnit1 { get => _TestUnit1; set { _TestUnit1 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestUnit1)) NewUnit1 = 0; else NewUnit1 = Int16.Parse(TestUnit1); } }
        private int NewUnit1;
        private string _TestUnit2;
        public string TestUnit2 { get => _TestUnit2; set { _TestUnit2 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestUnit2)) NewUnit2 = 0; else NewUnit2 = Int16.Parse(TestUnit2); } }
        private int NewUnit2;
        private string _TestUnit3;
        public string TestUnit3 { get => _TestUnit3; set { _TestUnit3 = value; OnPropertyChanged(); if (string.IsNullOrEmpty(TestUnit3)) NewUnit3 = 0; else NewUnit3 = Int16.Parse(TestUnit3); } }
        private int NewUnit3;
        // 3 ngày sản xuất
        private Nullable<DateTime> _NewMafDate1;
        public Nullable<DateTime> NewMafDate1 { get => _NewMafDate1; set { _NewMafDate1 = value; OnPropertyChanged(); } }
        private Nullable<DateTime> _NewMafDate2;
        public Nullable<DateTime> NewMafDate2 { get => _NewMafDate2; set { _NewMafDate2 = value; OnPropertyChanged(); } }
        private Nullable<DateTime> _NewMafDate3;
        public Nullable<DateTime> NewMafDate3 { get => _NewMafDate3; set { _NewMafDate3 = value; OnPropertyChanged(); } }
        // 3 hạn sử dụng
        private Nullable<DateTime> _NewExpDate1;
        public Nullable<DateTime> NewExpDate1 { get => _NewExpDate1; set { _NewExpDate1 = value; OnPropertyChanged(); } }
        private Nullable<DateTime> _NewExpDate2;
        public Nullable<DateTime> NewExpDate2 { get => _NewExpDate2; set { _NewExpDate2 = value; OnPropertyChanged(); } }
        private Nullable<DateTime> _NewExpDate3;
        public Nullable<DateTime> NewExpDate3 { get => _NewExpDate3; set { _NewExpDate3 = value; OnPropertyChanged(); } }
        void LoadDataOutput()
        {
            ListCustomer = new ObservableCollection<Customer>();
            var customerList = DataProvider.Ins.DB.Customers;
            foreach (var item in customerList)
            {
                ListCustomer.Add(item);
            }

            ListProduct = new ObservableCollection<Product>();
            var productList = DataProvider.Ins.DB.Products;
            foreach (var item in productList)
            {
                ListProduct.Add(item);
            }
        }
        // Command xuất kho
        public ICommand AddCommand { get; set; }
        // Các điều kiện của 3 sản phẩm
        private int conditionGroup1()
        {
            if (!string.IsNullOrEmpty(NewProduct1) && NewStorage1 != 0 && NewEmployee1 != 0 && NewUnit1 != 0 && NewUnit1 <= UnitConsignment1)
                return 2;
            if (string.IsNullOrEmpty(NewProduct1) && NewStorage1 == 0 && NewEmployee1 == 0 && (NewUnit1 == 0 || NewUnit1 > UnitConsignment1))
                return 0;
            return 1;
        }
        private int conditionGroup2()
        {
            if (!string.IsNullOrEmpty(NewProduct2) && NewStorage2 != 0 && NewEmployee2 != 0 && NewUnit2 != 0 && NewUnit2 <= UnitConsignment2)
                return 2;
            if (string.IsNullOrEmpty(NewProduct2) && NewStorage2 == 0 && NewEmployee2 == 0 && (NewUnit2 == 0 || NewUnit2 > UnitConsignment2))
                return 0;
            return 1;
        }
        private int conditionGroup3()
        {
            if (!string.IsNullOrEmpty(NewProduct3) && NewStorage3 != 0 && NewEmployee3 != 0 && NewUnit3 != 0 && NewUnit3 <= UnitConsignment3)
                return 2;
            if (string.IsNullOrEmpty(NewProduct3) && NewStorage3 == 0 && NewEmployee3 == 0 && (NewUnit3 == 0 || NewUnit3 > UnitConsignment3))
                return 0;
            return 1;
        }
        #endregion

        public OutputProductViewModel()
        {
            LoadDataOutputInfo();
            LoadDataOutput();
            FilterListView();

            // nút xuất kho
            AddCommand = new RelayCommand<object>(
                (p) => {
                    if (NewCustomer == 0)
                        return false;
                    if (conditionGroup1() == 1 || conditionGroup2() == 1 || conditionGroup3() == 1)
                        return false;
                    if (conditionGroup1() == 0 && conditionGroup2() == 0 && conditionGroup3() == 0)
                        return false;
                    return true;
                },
                (p) => {
                    var output = new Output() { DateOutput = DateTime.Now, IdCustomer = NewCustomer };
                    DataProvider.Ins.DB.Outputs.Add(output);
                    DataProvider.Ins.DB.SaveChanges();
                    int idOutput = DataProvider.Ins.DB.Outputs.Max(x => x.IdOutput);

                    if (conditionGroup1() == 2)
                    {
                        var updateUnitConsignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == NewConsignment1).SingleOrDefault();
                        updateUnitConsignment.Unit -= NewUnit1;
                        var outputInfo = new OutputInfo() { IdOutput = idOutput, IdConsignment = NewConsignment1, IdEmployee = NewEmployee1, Unit = NewUnit1 };
                        DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    if (conditionGroup2() == 2)
                    {
                        var updateUnitConsignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == NewConsignment2).SingleOrDefault();
                        updateUnitConsignment.Unit -= NewUnit2;
                        var outputInfo = new OutputInfo() { IdOutput = idOutput, IdConsignment = NewConsignment2, IdEmployee = NewEmployee2, Unit = NewUnit2 };
                        DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    if (conditionGroup3() == 2)
                    {
                        var updateUnitConsignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == NewConsignment3).SingleOrDefault();
                        updateUnitConsignment.Unit -= NewUnit2;
                        var outputInfo = new OutputInfo() { IdOutput = idOutput, IdConsignment = NewConsignment3, IdEmployee = NewEmployee3, Unit = NewUnit3 };
                        DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    LoadDataOutputInfo();
                    LoadDataOutput();
                    MessageBox.Show("Xuất kho thành công.\nXem lại chi tiết khi chọn phiếu xuất.", "Thông báo");
                });

            // nút sửa lô 1
            EditCommand1 = new RelayCommand<object>(
                (p) =>
                {
                    if (string.IsNullOrEmpty(Product1) || Storage1 == 0 || Employee1 == 0 || Consignment1 == 0 || Unit1 == 0)
                        return false;
                    var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment1).SingleOrDefault();
                    if (Consignment1 == _CheckConsignment1)
                    {
                        if (Unit1 == _CheckUnit1)
                            return false;
                        if (Unit1 > (_CheckUnit1 + consignment.Unit))
                            return false;
                        return true;
                    }
                    if (Consignment1 != _CheckConsignment1)
                    {
                        if (Unit1 > consignment.Unit)
                            return false;
                        return true;
                    }
                    if (_CheckProduct1 != Product1 || _CheckUnit1 != Unit1 || _CheckStorage1 != Storage1 || _CheckEmployee1 != Employee1)
                        return true;
                    return false;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn thay đổi thông tin chi tiết xuất kho?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var updateStorageDetail = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment1).SingleOrDefault();
                        var updateOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IdConsignment == _CheckConsignment1 && x.IdOutput == SelectedItem.Output.IdOutput).SingleOrDefault();
                        if (Consignment1 == _CheckConsignment1)
                        {
                            updateStorageDetail.Unit -= Unit1 - _CheckUnit1;
                            updateOutputInfo.IdEmployee = Employee1;
                            updateOutputInfo.Unit = Unit1;
                        }
                        else
                        {
                            var restore = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == _CheckConsignment1).SingleOrDefault();
                            restore.Unit += _CheckUnit1;
                            updateStorageDetail.Unit -= Unit1;
                            OutputInfo newOutputInfo = new OutputInfo() { IdOutput = SelectedItem.Output.IdOutput, IdConsignment = Consignment1, IdEmployee = Employee1, Unit = Unit1 };
                            DataProvider.Ins.DB.OutputInfoes.Remove(updateOutputInfo);
                            DataProvider.Ins.DB.OutputInfoes.Add(newOutputInfo);
                        }
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Cập nhật thành công", "Thông báo");
                        _CheckProduct1 = Product1;
                        _CheckStorage1 = Storage1;
                        _CheckConsignment1 = Consignment1;
                        _CheckUnit1 = Unit1;
                        _CheckEmployee1 = Employee1;
                    }
                });
            // nút sửa lô 2
            EditCommand2 = new RelayCommand<object>(
                (p) =>
                {
                    if (string.IsNullOrEmpty(Product2) || Storage2 == 0 || Employee2 == 0 || Consignment2 == 0 || Unit2 == 0)
                        return false;
                    var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment2).SingleOrDefault();
                    if (Consignment2 == _CheckConsignment2)
                    {
                        if (Unit2 == _CheckUnit2)
                            return false;
                        if (Unit2 > (_CheckUnit2 + consignment.Unit))
                            return false;
                        return true;
                    }
                    if (Consignment2 != _CheckConsignment2)
                    {
                        if (Unit2 > consignment.Unit)
                            return false;
                        return true;
                    }
                    if (_CheckProduct2 != Product2 || _CheckUnit2 != Unit2 || _CheckStorage2 != Storage2 || _CheckEmployee2 != Employee2)
                        return true;
                    return false;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn thay đổi thông tin chi tiết xuất kho?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var updateStorageDetail = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment2).SingleOrDefault();
                        var updateOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IdConsignment == _CheckConsignment2 && x.IdOutput == SelectedItem.Output.IdOutput).SingleOrDefault();
                        if (Consignment2 == _CheckConsignment2)
                        {
                            updateStorageDetail.Unit -= Unit2 - _CheckUnit2;
                            updateOutputInfo.IdEmployee = Employee2;
                            updateOutputInfo.Unit = Unit2;
                        }
                        else
                        {
                            var restore = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == _CheckConsignment2).SingleOrDefault();
                            restore.Unit += _CheckUnit2;
                            updateStorageDetail.Unit -= Unit2;
                            OutputInfo newOutputInfo = new OutputInfo() { IdOutput = SelectedItem.Output.IdOutput, IdConsignment = Consignment2, IdEmployee = Employee2, Unit = Unit2 };
                            DataProvider.Ins.DB.OutputInfoes.Remove(updateOutputInfo);
                            DataProvider.Ins.DB.OutputInfoes.Add(newOutputInfo);
                        }
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Cập nhật thành công", "Thông báo");
                        _CheckProduct2 = Product2;
                        _CheckStorage2 = Storage2;
                        _CheckConsignment2 = Consignment2;
                        _CheckUnit2 = Unit2;
                        _CheckEmployee2 = Employee2;
                    }
                });
            // nút sửa lô 3
            EditCommand3 = new RelayCommand<object>(
                (p) =>
                {
                    if (string.IsNullOrEmpty(Product3) || Storage3 == 0 || Employee3 == 0 || Consignment3 == 0 || Unit3 == 0)
                        return false;
                    var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment2).SingleOrDefault();
                    if (Consignment3 == _CheckConsignment3)
                    {
                        if (Unit3 == _CheckUnit3)
                            return false;
                        if (Unit3 > (_CheckUnit3 + consignment.Unit))
                            return false;
                        return true;
                    }
                    if (Consignment3 != _CheckConsignment3)
                    {
                        if (Unit3 > consignment.Unit)
                            return false;
                        return true;
                    }
                    if (_CheckProduct3 != Product3 || _CheckUnit3 != Unit3 || _CheckStorage3 != Storage3 || _CheckEmployee3 != Employee3)
                        return true;
                    return false;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn thay đổi thông tin chi tiết xuất kho?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var updateStorageDetail = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment3).SingleOrDefault();
                        var updateOutputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IdConsignment == _CheckConsignment3 && x.IdOutput == SelectedItem.Output.IdOutput).SingleOrDefault();
                        if (Consignment3 == _CheckConsignment3)
                        {
                            updateStorageDetail.Unit -= Unit3 - _CheckUnit3;
                            updateOutputInfo.IdEmployee = Employee3;
                            updateOutputInfo.Unit = Unit3;
                        }
                        else
                        {
                            var restore = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == _CheckConsignment3).SingleOrDefault();
                            restore.Unit += _CheckUnit3;
                            updateStorageDetail.Unit -= Unit3;
                            OutputInfo newOutputInfo = new OutputInfo() { IdOutput = SelectedItem.Output.IdOutput, IdConsignment = Consignment3, IdEmployee = Employee3, Unit = Unit3 };
                            DataProvider.Ins.DB.OutputInfoes.Remove(updateOutputInfo);
                            DataProvider.Ins.DB.OutputInfoes.Add(newOutputInfo);
                        }
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Cập nhật thành công", "Thông báo");
                        _CheckProduct3 = Product3;
                        _CheckStorage3 = Storage3;
                        _CheckConsignment3 = Consignment3;
                        _CheckUnit3 = Unit3;
                        _CheckEmployee3 = Employee3;
                    }
                });

            // nút xóa
            DeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem != null)
                        return true;
                    return false;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn xóa phiếu xuất này?\n(Xóa tất cả thông tin phiếu xuất và các lô hàng được ghi trong phiếu)", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Hand) == MessageBoxResult.Yes)
                    {
                        var outputInfo = DataProvider.Ins.DB.OutputInfoes.Where(x => x.IdOutput == SelectedItem.Output.IdOutput &&
                            (x.IdConsignment == Consignment1 || x.IdConsignment == Consignment2 || x.IdConsignment == Consignment3));
                        foreach (var item in outputInfo)
                        {
                            var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == item.IdConsignment).SingleOrDefault();
                            consignment.Unit += item.Unit;
                            DataProvider.Ins.DB.OutputInfoes.Remove(item);
                        }
                        var output = DataProvider.Ins.DB.Outputs.Where(x => x.IdOutput == SelectedItem.Output.IdOutput).SingleOrDefault();
                        DataProvider.Ins.DB.Outputs.Remove(output);
                        DataProvider.Ins.DB.SaveChanges();
                        LoadDataOutputInfo();
                        MessageBox.Show("Xóa phiếu xuất thành công", "Thông báo");
                        LoadCart();
                    }
                });
        }
    }
}
