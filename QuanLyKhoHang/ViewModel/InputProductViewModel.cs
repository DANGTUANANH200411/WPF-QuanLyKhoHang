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
    public class InputProductViewModel : BaseViewModel
    {
        #region Đỗ dữ liệu vào ListView
        private ObservableCollection<ThongTinNhapHang> _ListInputInfo;
        public ObservableCollection<ThongTinNhapHang> ListInputInfo { get => _ListInputInfo; set { _ListInputInfo = value; OnPropertyChanged(); } }

        void LoadDataInputInfo()
        {
            ListInputInfo = new ObservableCollection<ThongTinNhapHang>();
            var inputList = DataProvider.Ins.DB.Inputs;
            foreach (var input in inputList)
            {
                ThongTinNhapHang inputInfo = new ThongTinNhapHang();
                inputInfo.Input = input;
                var suplier = DataProvider.Ins.DB.Supliers.Where(x => x.IdSuplier == input.IdSuplier).SingleOrDefault();
                inputInfo.NameSuplier = suplier.NameSuplier;
                ListInputInfo.Add(inputInfo);
            }
        }
        #endregion

        #region Bộ lọc trong ListView
        private string _Filter;
        public string Filter { get => _Filter; set { _Filter = value; OnFilterChanged(); } }

        void FilterListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListInputInfo);
            view.Filter = UserFilter;
        }

        private void OnFilterChanged()
        {
            CollectionViewSource.GetDefaultView(ListInputInfo).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Filter))
                return true;
            else
                return ((item as ThongTinNhapHang).NameSuplier.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as ThongTinNhapHang).Input.IdInput.ToString().IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as ThongTinNhapHang).Input.DateInput.ToString().IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion

        #region Chọn dữ liệu dưới ListView Binding đến các Cart
        // 3 mã lô hàng
        private int _Consignment1;
        public int Consignment1 { get => _Consignment1; set { _Consignment1 = value; OnPropertyChanged(); } }
        private int _Consignment2;
        public int Consignment2 { get => _Consignment2; set { _Consignment2 = value; OnPropertyChanged(); } }
        private int _Consignment3;
        public int Consignment3 { get => _Consignment3; set { _Consignment3 = value; OnPropertyChanged(); } }
        // 3 sản phẩm
        private ObservableCollection<Product> _ProductList;
        public ObservableCollection<Product> ProductList { get => _ProductList; set { _ProductList = value; OnPropertyChanged(); } }
        private string _Product1;
        public string Product1 { get => _Product1; set { _Product1 = value; OnPropertyChanged(); } }
        private string _Product2;
        public string Product2 { get => _Product2; set { _Product2 = value; OnPropertyChanged(); } }
        private string _Product3;
        public string Product3 { get => _Product3; set { _Product3 = value; OnPropertyChanged(); } }
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
        // 3 kho chứa
        private ObservableCollection<Storage> _StorageList;
        public ObservableCollection<Storage> StorageList { get => _StorageList; set { _StorageList = value; OnPropertyChanged(); } }
        private int _Storage1;
        public int Storage1
        {
            get => _Storage1; set
            {
                _Storage1 = value; OnPropertyChanged();
                // duyệt nhân viên theo kho và theo rank
                if (Storage1 != 0)
                {
                    EmployeeList1 = new ObservableCollection<Employee>();
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == Storage1 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        EmployeeList1.Add(item);
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
                if (Storage2 != 0)
                {
                    EmployeeList2 = new ObservableCollection<Employee>();
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == Storage2 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        EmployeeList2.Add(item);
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
                if (Storage3 != 0)
                {
                    EmployeeList3 = new ObservableCollection<Employee>();
                    var employeeList = DataProvider.Ins.DB.Employees.Where(x => x.IdStorage == Storage3 && (x.IdRank == 1 || x.IdRank == 5));
                    foreach (var item in employeeList)
                    {
                        EmployeeList3.Add(item);
                    }
                }
            }
        }
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
        // 3 nhân viên
        private ObservableCollection<Employee> _EmployeeList1;
        public ObservableCollection<Employee> EmployeeList1 { get => _EmployeeList1; set { _EmployeeList1 = value; OnPropertyChanged(); } }
        private int _Employee1;
        public int Employee1 { get => _Employee1; set { _Employee1 = value; OnPropertyChanged(); } }
        private ObservableCollection<Employee> _EmployeeList2;
        public ObservableCollection<Employee> EmployeeList2 { get => _EmployeeList2; set { _EmployeeList2 = value; OnPropertyChanged(); } }
        private int _Employee2;
        public int Employee2 { get => _Employee2; set { _Employee2 = value; OnPropertyChanged(); } }
        private ObservableCollection<Employee> _EmployeeList3;
        public ObservableCollection<Employee> EmployeeList3 { get => _EmployeeList3; set { _EmployeeList3 = value; OnPropertyChanged(); } }
        private int _Employee3;
        public int Employee3 { get => _Employee3; set { _Employee3 = value; OnPropertyChanged(); } }
        // chọn item dưới ListView
        private ThongTinNhapHang _SelectedItem;
        public ThongTinNhapHang SelectedItem
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
                    MafDate1 = MafDate2 = MafDate3 = _CheckMafDate1 = _CheckMafDate2 = _CheckMafDate3 = null;
                    ExpDate1 = ExpDate2 = ExpDate3 = _CheckExpDate1 = _CheckExpDate2 = _CheckExpDate3 = null;
                    Employee1 = Employee2 = Employee3 = _CheckEmployee1 = _CheckEmployee2 = _CheckEmployee3 = 0;
                    // load dữ liệu lên các combobox
                    ProductList = new ObservableCollection<Product>();
                    var productList = DataProvider.Ins.DB.Products.Where(x => x.IdSuplier == SelectedItem.Input.IdSuplier);
                    foreach (var item in productList)
                    {
                        ProductList.Add(item);
                    }
                    StorageList = new ObservableCollection<Storage>();
                    var storageList = DataProvider.Ins.DB.Storages;
                    foreach (var item in storageList)
                    {
                        StorageList.Add(item);
                    }
                    // binding
                    List<LoHangNhapXuat> list = new List<LoHangNhapXuat>();
                    var listInputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.IdInput == SelectedItem.Input.IdInput);
                    foreach (var item in listInputInfo)
                    {
                        var consignment = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == item.IdConsignment).SingleOrDefault();
                        list.Add(new LoHangNhapXuat() { Consignment = consignment, Employee = item.IdEmployee, Unit = item.Unit });
                    }
                    // đổ lên Cart số 1
                    Consignment1 = list.ElementAt(0).Consignment.IdConsignment;
                    Product1 = _CheckProduct1 = list.ElementAt(0).Consignment.IdProduct;
                    TextUnit1 = list.ElementAt(0).Unit.ToString();
                    _CheckUnit1 = list.ElementAt(0).Unit;
                    Storage1 = _CheckStorage1 = list.ElementAt(0).Consignment.IdStorage;
                    MafDate1 = _CheckMafDate1 = list.ElementAt(0).Consignment.MafDate;
                    ExpDate1 = _CheckExpDate1 = list.ElementAt(0).Consignment.ExpDate;
                    Employee1 = _CheckEmployee1 = list.ElementAt(0).Employee;
                    // đổ lên Cart số 2, 3 nếu có
                    if (list.Count() > 1)
                    {
                        Consignment2 = list.ElementAt(1).Consignment.IdConsignment;
                        Product2 = _CheckProduct2 = list.ElementAt(1).Consignment.IdProduct;
                        TextUnit2 = list.ElementAt(1).Unit.ToString();
                        _CheckUnit2 = list.ElementAt(1).Unit;
                        Storage2 = _CheckStorage2 = list.ElementAt(1).Consignment.IdStorage;
                        MafDate2 = _CheckMafDate2 = list.ElementAt(1).Consignment.MafDate;
                        ExpDate2 = _CheckExpDate2 = list.ElementAt(1).Consignment.ExpDate;
                        Employee2 = _CheckEmployee2 = list.ElementAt(1).Employee;
                    }
                    if (list.Count() > 2)
                    {
                        Consignment3 = list.ElementAt(2).Consignment.IdConsignment;
                        Product3 = _CheckProduct3 = list.ElementAt(2).Consignment.IdProduct;
                        TextUnit3 = list.ElementAt(2).Unit.ToString();
                        _CheckUnit3 = list.ElementAt(2).Unit;
                        Storage3 = _CheckStorage3 = list.ElementAt(2).Consignment.IdStorage;
                        MafDate3 = _CheckMafDate3 = list.ElementAt(2).Consignment.MafDate;
                        ExpDate3 = _CheckExpDate3 = list.ElementAt(2).Consignment.ExpDate;
                        Employee3 = _CheckEmployee3 = list.ElementAt(2).Employee;
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
        private Nullable<DateTime> _CheckMafDate1;
        private Nullable<DateTime> _CheckMafDate2;
        private Nullable<DateTime> _CheckMafDate3;
        private Nullable<DateTime> _CheckExpDate1;
        private Nullable<DateTime> _CheckExpDate2;
        private Nullable<DateTime> _CheckExpDate3;
        private int _CheckEmployee1;
        private int _CheckEmployee2;
        private int _CheckEmployee3;
        #endregion

        #region Nút Xóa
        public ICommand DeleteCommand { get; set; }
        void LoadCart()
        {
            Consignment1 = Consignment2 = Consignment3 = 0;
            ProductList = new ObservableCollection<Product>();
            StorageList = new ObservableCollection<Storage>();
            TextUnit1 = TextUnit2 = TextUnit3 = null;
            MafDate1 = MafDate2 = MafDate3 = ExpDate1 = ExpDate2 = ExpDate3 = null;
        }
        #endregion

        #region Đổ dữ liệu vào các mục Nhập hàng và nút Nhập
        // Nhà cung cấp
        private ObservableCollection<Suplier> _ListSuplier;
        public ObservableCollection<Suplier> ListSuplier { get => _ListSuplier; set { _ListSuplier = value; OnPropertyChanged(); } }
        private int _NewSuplier = 0;
        public int NewSuplier
        {
            get => _NewSuplier; set
            {
                _NewSuplier = value; OnPropertyChanged();
                // duyệt sản phẩm theo nhà cung ứng
                ListProduct = new ObservableCollection<Product>();
                if (NewSuplier != 0)
                {
                    var product = DataProvider.Ins.DB.Products.Where(x => x.IdSuplier == NewSuplier);
                    foreach (var item in product)
                    {
                        ListProduct.Add(item);
                    }
                }
            }
        }
        // 3 Sản phẩm
        private ObservableCollection<Product> _ListProduct;
        public ObservableCollection<Product> ListProduct { get => _ListProduct; set { _ListProduct = value; OnPropertyChanged(); } }
        private string _NewProduct1;
        public string NewProduct1 { get => _NewProduct1; set { _NewProduct1 = value; OnPropertyChanged(); } }
        private string _NewProduct2;
        public string NewProduct2 { get => _NewProduct2; set { _NewProduct2 = value; OnPropertyChanged(); } }
        private string _NewProduct3;
        public string NewProduct3 { get => _NewProduct3; set { _NewProduct3 = value; OnPropertyChanged(); } }
        // 3 Kho chứa
        private ObservableCollection<Storage> _ListStorage;
        public ObservableCollection<Storage> ListStorage { get => _ListStorage; set { _ListStorage = value; OnPropertyChanged(); } }
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
            }
        }
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
        void LoadDataInput()
        {
            ListSuplier = new ObservableCollection<Suplier>();
            var suplierList = DataProvider.Ins.DB.Supliers;
            foreach (var item in suplierList)
            {
                ListSuplier.Add(item);
            }

            ListStorage = new ObservableCollection<Storage>();
            var storageList = DataProvider.Ins.DB.Storages;
            foreach (var item in storageList)
            {
                ListStorage.Add(item);
            }
        }
        // Command nhập hàng
        public ICommand AddCommand { get; set; }
        // Các điều kiện của 3 sản phẩm
        private int conditionGroup1()
        {
            if (!string.IsNullOrEmpty(NewProduct1) && NewStorage1 != 0 && NewEmployee1 != 0 && NewUnit1 != 0 && !string.IsNullOrEmpty(NewMafDate1.ToString()) && !string.IsNullOrEmpty(NewExpDate1.ToString()))
                return 2;
            if (string.IsNullOrEmpty(NewProduct1) && NewStorage1 == 0 && NewEmployee1 == 0 && NewUnit1 == 0 && string.IsNullOrEmpty(NewMafDate1.ToString()) && string.IsNullOrEmpty(NewExpDate1.ToString()))
                return 0;
            return 1;
        }
        private int conditionGroup2()
        {
            if (!string.IsNullOrEmpty(NewProduct2) && NewStorage2 != 0 && NewEmployee2 != 0 && NewUnit2 != 0 && !string.IsNullOrEmpty(NewMafDate2.ToString()) && !string.IsNullOrEmpty(NewExpDate2.ToString()))
                return 2;
            if (string.IsNullOrEmpty(NewProduct2) && NewStorage2 == 0 && NewEmployee2 == 0 && NewUnit2 == 0 && string.IsNullOrEmpty(NewMafDate2.ToString()) && string.IsNullOrEmpty(NewExpDate2.ToString()))
                return 0;
            return 1;
        }
        private int conditionGroup3()
        {
            if (!string.IsNullOrEmpty(NewProduct3) && NewStorage3 != 0 && NewEmployee3 != 0 && NewUnit3 != 0 && !string.IsNullOrEmpty(NewMafDate3.ToString()) && !string.IsNullOrEmpty(NewExpDate3.ToString()))
                return 2;
            if (string.IsNullOrEmpty(NewProduct3) && NewStorage3 == 0 && NewEmployee3 == 0 && NewUnit3 == 0 && string.IsNullOrEmpty(NewMafDate3.ToString()) && string.IsNullOrEmpty(NewExpDate3.ToString()))
                return 0;
            return 1;
        }
        #endregion

        public InputProductViewModel()
        {
            LoadDataInput();
            LoadDataInputInfo();
            FilterListView();

            // nút nhập hàng
            AddCommand = new RelayCommand<object>(
                (p) => {
                    if (conditionGroup1() == 1 || conditionGroup2() == 1 || conditionGroup3() == 1)
                        return false;
                    if (conditionGroup1() == 0 && conditionGroup2() == 0 && conditionGroup3() == 0)
                        return false;
                    return true;
                },
                (p) => {
                    var input = new Input() { DateInput = DateTime.Now, IdSuplier = NewSuplier };
                    DataProvider.Ins.DB.Inputs.Add(input);
                    DataProvider.Ins.DB.SaveChanges();
                    int idInput = DataProvider.Ins.DB.Inputs.Max(x => x.IdInput);

                    if (conditionGroup1() == 2)
                    {
                        var consignment = new StorageDetail() { IdStorage = NewStorage1, IdProduct = NewProduct1, Unit = NewUnit1, MafDate = NewMafDate1, ExpDate = NewExpDate1 };
                        DataProvider.Ins.DB.StorageDetails.Add(consignment);
                        DataProvider.Ins.DB.SaveChanges();
                        int idConsignment = DataProvider.Ins.DB.StorageDetails.Max(x => x.IdConsignment);
                        var inputInfo = new InputInfo() { IdInput = idInput, IdConsignment = idConsignment, IdEmployee = NewEmployee1, Unit = NewUnit1 };
                        DataProvider.Ins.DB.InputInfoes.Add(inputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    if (conditionGroup2() == 2)
                    {
                        var consignment = new StorageDetail() { IdStorage = NewStorage2, IdProduct = NewProduct2, Unit = NewUnit2, MafDate = NewMafDate2, ExpDate = NewExpDate2 };
                        DataProvider.Ins.DB.StorageDetails.Add(consignment);
                        DataProvider.Ins.DB.SaveChanges();
                        int idConsignment = DataProvider.Ins.DB.StorageDetails.Max(x => x.IdConsignment);
                        var inputInfo = new InputInfo() { IdInput = idInput, IdConsignment = idConsignment, IdEmployee = NewEmployee2, Unit = NewUnit2 };
                        DataProvider.Ins.DB.InputInfoes.Add(inputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    if (conditionGroup3() == 2)
                    {
                        var consignment = new StorageDetail() { IdStorage = NewStorage3, IdProduct = NewProduct3, Unit = NewUnit3, MafDate = NewMafDate3, ExpDate = NewExpDate3 };
                        DataProvider.Ins.DB.StorageDetails.Add(consignment);
                        DataProvider.Ins.DB.SaveChanges();
                        int idConsignment = DataProvider.Ins.DB.StorageDetails.Max(x => x.IdConsignment);
                        var inputInfo = new InputInfo() { IdInput = idInput, IdConsignment = idConsignment, IdEmployee = NewEmployee3, Unit = NewUnit3 };
                        DataProvider.Ins.DB.InputInfoes.Add(inputInfo);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                    MessageBox.Show("Nhập hàng thành công.\nXem lại chi tiết khi chọn phiếu nhập.", "Thông báo");
                    // Load dữ liệu vào clear các box
                    LoadDataInputInfo();
                    LoadDataInput();
                    ListProduct = new ObservableCollection<Product>();
                    TestUnit1 = TestUnit2 = TestUnit3 = null;
                    NewMafDate1 = NewMafDate2 = NewMafDate3 = null;
                    NewExpDate1 = NewExpDate2 = NewExpDate3 = null;
                });

            // nút sửa lô 1
            EditCommand1 = new RelayCommand<object>(
                (p) =>
                {
                    if (Consignment1 == 0 || Unit1 == 0)
                        return false;
                    if (_CheckProduct1 != Product1 || _CheckUnit1 != Unit1 || _CheckStorage1 != Storage1
                        || _CheckMafDate1 != MafDate1 || _CheckExpDate1 != ExpDate1 || _CheckEmployee1 != Employee1)
                        return true;
                    return false;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn thay đổi thông tin lô hàng này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var updateStorageDetail = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment1).SingleOrDefault();
                        updateStorageDetail.IdProduct = _CheckProduct1 = Product1;
                        updateStorageDetail.IdStorage = _CheckStorage1 = Storage1;
                        updateStorageDetail.Unit += Unit1 - _CheckUnit1;
                        _CheckUnit1 = Unit1;
                        updateStorageDetail.MafDate = _CheckMafDate1 = MafDate1;
                        updateStorageDetail.ExpDate = _CheckExpDate1 = ExpDate1;
                        var updateInputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.IdConsignment == Consignment1 && x.IdInput == SelectedItem.Input.IdInput).SingleOrDefault();
                        updateInputInfo.IdEmployee = _CheckEmployee1 = Employee1;
                        updateInputInfo.Unit = Unit1;
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Cập nhật thành công", "Thông báo");
                    }
                });
            // nút sửa lô 2
            EditCommand2 = new RelayCommand<object>(
                (p) =>
                {
                    if (Consignment2 == 0 || Unit2 == 0)
                        return false;
                    if (_CheckProduct2 != Product2 || _CheckUnit2 != Unit2 || _CheckStorage2 != Storage2
                        || _CheckMafDate2 != MafDate2 || _CheckExpDate2 != ExpDate2 || _CheckEmployee2 != Employee2)
                        return true;
                    return false;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn thay đổi thông tin lô hàng này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var updateStorageDetail = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment2).SingleOrDefault();
                        updateStorageDetail.IdProduct = _CheckProduct2 = Product2;
                        updateStorageDetail.IdStorage = _CheckStorage2 = Storage2;
                        updateStorageDetail.Unit += Unit2 - _CheckUnit2;
                        _CheckUnit2 = Unit2;
                        updateStorageDetail.MafDate = _CheckMafDate2 = MafDate2;
                        updateStorageDetail.ExpDate = _CheckExpDate2 = ExpDate2;
                        var updateInputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.IdConsignment == Consignment2 && x.IdInput == SelectedItem.Input.IdInput).SingleOrDefault();
                        updateInputInfo.IdEmployee = _CheckEmployee2 = Employee2;
                        updateInputInfo.Unit = Unit2;
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Cập nhật thành công", "Thông báo");
                    }
                });
            // nút sửa lô 3
            EditCommand3 = new RelayCommand<object>(
                (p) =>
                {
                    if (Consignment3 == 0 || Unit3 == 0)
                        return false;
                    if (_CheckProduct3 != Product3 || _CheckUnit3 != Unit3 || _CheckStorage3 != Storage3
                        || _CheckMafDate3 != MafDate3 || _CheckExpDate3 != ExpDate3 || _CheckEmployee3 != Employee3)
                        return true;
                    return false;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn thay đổi thông tin lô hàng này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var updateStorageDetail = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment3).SingleOrDefault();
                        updateStorageDetail.IdProduct = _CheckProduct3 = Product3;
                        updateStorageDetail.IdStorage = _CheckStorage3 = Storage3;
                        updateStorageDetail.Unit += Unit3 - _CheckUnit3;
                        _CheckUnit3 = Unit3;
                        updateStorageDetail.MafDate = _CheckMafDate3 = MafDate3;
                        updateStorageDetail.ExpDate = _CheckExpDate3 = ExpDate3;
                        var updateInputInfo = DataProvider.Ins.DB.InputInfoes.Where(x => x.IdConsignment == Consignment3 && x.IdInput == SelectedItem.Input.IdInput).SingleOrDefault();
                        updateInputInfo.IdEmployee = _CheckEmployee3 = Employee3;
                        updateInputInfo.Unit = Unit3;
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Cập nhật thành công", "Thông báo");
                    }
                });

            // nút xóa
            DeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (Consignment1 == 0)
                        return false;
                    var consignment1 = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment1).SingleOrDefault();
                    if (consignment1 == null) return false;
                    if (consignment1.Unit != Unit1) return false;
                    if (Consignment2 != 0)
                    {
                        var consignment2 = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment2).SingleOrDefault();
                        if (consignment2.Unit != Unit2) return false;
                    }
                    if (Consignment3 != 0)
                    {
                        var consignment3 = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment3).SingleOrDefault();
                        if (consignment3.Unit != Unit3) return false;
                    }
                    return true;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn xóa phiếu nhập này?\n(Xóa tất cả thông tin phiếu nhập và các lô hàng được ghi trong phiếu)", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Hand) == MessageBoxResult.Yes)
                    {
                        var inputInfoList = DataProvider.Ins.DB.InputInfoes.Where(x => x.IdInput == SelectedItem.Input.IdInput);
                        foreach (var item in inputInfoList)
                        {
                            DataProvider.Ins.DB.InputInfoes.Remove(item);
                        }
                        var consignment1 = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment1).SingleOrDefault();
                        DataProvider.Ins.DB.StorageDetails.Remove(consignment1);
                        if (Consignment2 != 0)
                        {
                            var consignment2 = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment2).SingleOrDefault();
                            DataProvider.Ins.DB.StorageDetails.Remove(consignment2);
                        }
                        if (Consignment3 != 0)
                        {
                            var consignment3 = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdConsignment == Consignment3).SingleOrDefault();
                            DataProvider.Ins.DB.StorageDetails.Remove(consignment3);
                        }
                        var input = DataProvider.Ins.DB.Inputs.Where(x => x.IdInput == SelectedItem.Input.IdInput).SingleOrDefault();
                        DataProvider.Ins.DB.Inputs.Remove(input);
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Xóa phiếu nhập thành công", "Thông báo");
                        LoadDataInputInfo();
                        LoadCart();
                    }
                });
        }
    }
}
