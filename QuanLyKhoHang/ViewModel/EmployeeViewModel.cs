using Microsoft.Win32;
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
    class EmployeeViewModel : BaseViewModel
    {
        #region ObservableCollection
        private ObservableCollection<NhanVien> _ListEmployee;
        public ObservableCollection<NhanVien> ListEmployee { get => _ListEmployee; set { _ListEmployee = value; OnPropertyChanged(); } }

        private ObservableCollection<RankEmployee> _ListRank;
        public ObservableCollection<RankEmployee> ListRank { get => _ListRank; set { _ListRank = value; OnPropertyChanged(); } }

        private ObservableCollection<Storage> _ListStorage;
        public ObservableCollection<Storage> ListStorage { get => _ListStorage; set { _ListStorage = value; OnPropertyChanged(); } }
        #endregion

        #region SelectedItem
        private NhanVien _SelectedItem;
        public NhanVien SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    NameEmployee = SelectedItem.employee.NameEmployee;
                    Sex = SelectedItem.employee.Sex;
                    Birthday = SelectedItem.employee.Birthday;
                    DateOfStart = SelectedItem.employee.DateOfStart;
                    NameRank = SelectedItem.rank;
                    NameStorage = SelectedItem.storage;
                    PhoneNumber = SelectedItem.employee.PhoneNumber;
                    AddressEmp = SelectedItem.employee.AddressEmp;
                    ImageEmp = SelectedItem.employee.ImageEmp;
                    tmpImageEmp = SelectedItem.employee.ImageEmp;
                    if (string.IsNullOrEmpty(ImageEmp))
                    {
                        if (Sex == "Nam")
                            ImageEmp = "..\\ResourceXAML\\male.jpg";
                        else
                            ImageEmp = "..\\ResourceXAML\\female.jpg";
                    }
                }
            }
        }
        #endregion

        #region Create field to edit employee's information
        private string _NameEmployee;
        public string NameEmployee { get => _NameEmployee; set { _NameEmployee = value; OnPropertyChanged(); } }

        private string _Sex;
        public string Sex { get => _Sex; set { _Sex = value; OnPropertyChanged(); } }

        private DateTime? _Birthday;
        public DateTime? Birthday { get => _Birthday; set { _Birthday = value; OnPropertyChanged(); } }

        private DateTime? _DateOfStart;
        public DateTime? DateOfStart { get => _DateOfStart; set { _DateOfStart = value; OnPropertyChanged(); } }

        private string _NameRank;
        public string NameRank { get => _NameRank; set { _NameRank = value; OnPropertyChanged(); } }

        private string _NameStorage;
        public string NameStorage { get => _NameStorage; set { _NameStorage = value; OnPropertyChanged(); } }

        private string _PhoneNumber;
        public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }

        private string _AddressEmp;
        public string AddressEmp { get => _AddressEmp; set { _AddressEmp = value; OnPropertyChanged(); } }

        private string _ImageEmp;
        public string ImageEmp { get => _ImageEmp; set { _ImageEmp = value; OnPropertyChanged(); } }

        private string _tmpImageEmp;
        public string tmpImageEmp { get => _tmpImageEmp; set { _tmpImageEmp = value; OnPropertyChanged(); } }

        private string[] _ListSex;
        public string[] ListSex { get => _ListSex; set { _ListSex = value; OnPropertyChanged(); } }
        #endregion

        #region Create field to add new employee
        private string _NewName;
        public string NewName { get => _NewName; set { _NewName = value; OnPropertyChanged(); } }

        private string _NewSex;
        public string NewSex { get => _NewSex; set { _NewSex = value; OnPropertyChanged(); } }

        private DateTime? _NewBirthday;
        public DateTime? NewBirthday { get => _NewBirthday; set { _NewBirthday = value; OnPropertyChanged(); } }

        private DateTime? _NewDateOfStart;
        public DateTime? NewDateOfStart { get => _NewDateOfStart; set { _NewDateOfStart = value; OnPropertyChanged(); } }

        private string _NewIDRank;
        public string NewIDRank { get => _NewIDRank; set { _NewIDRank = value; OnPropertyChanged(); } }

        private string _NewIDStorage;
        public string NewIDStorage { get => _NewIDStorage; set { _NewIDStorage = value; OnPropertyChanged(); } }

        private string _NewPhoneNumber;
        public string NewPhoneNumber { get => _NewPhoneNumber; set { _NewPhoneNumber = value; OnPropertyChanged(); } }

        private string _NewAddress;
        public string NewAddress { get => _NewAddress; set { _NewAddress = value; OnPropertyChanged(); } }

        private string _NewImageEmp;
        public string NewImageEmp { get => _NewImageEmp; set { _NewImageEmp = value; OnPropertyChanged(); } }


        #endregion

        #region Group ListView
        void GroupListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListEmployee);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("storage");
            view.GroupDescriptions.Add(groupDescription);
            /*
            view = (CollectionView)CollectionViewSource.GetDefaultView(ListEmployee);
            groupDescription = new PropertyGroupDescription("rank");
            view.GroupDescriptions.Add(groupDescription);
            */
        }
        #endregion

        #region Search Item
        private string _Filter;
        public string Filter { get => _Filter; set { _Filter = value; OnFilterChanged(); } }

        void FilterListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListEmployee);
            view.Filter = UserFilter;
        }

        private void OnFilterChanged()
        {
            CollectionViewSource.GetDefaultView(ListEmployee).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Filter))
                return true;
            else
                return ((item as NhanVien).employee.NameEmployee.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as NhanVien).employee.Sex.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as NhanVien).employee.PhoneNumber.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as NhanVien).rank.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as NhanVien).storage.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion
        
        #region Load data employee into ListView
        void LoadDataEmployee()
        {
            ListEmployee = new ObservableCollection<NhanVien>();

            var objectList = DataProvider.Ins.DB.Employees;

            foreach (var item in objectList)
            {
                var rank = DataProvider.Ins.DB.RankEmployees.Where(p => p.IdRank == item.IdRank);
                var storage = DataProvider.Ins.DB.Storages.Where(p => p.IdStorage == item.IdStorage);
                NhanVien nv = new NhanVien();
                nv.employee = item;

                foreach (var item1 in rank)
                {
                    nv.rank = item1.NameRank;
                }
                foreach (var item1 in storage)
                {
                    nv.storage = item1.NameStorage;
                }
                ListEmployee.Add(nv);
            }
            FilterListView();
            GroupListView();
        }
        void LoadDataRankAndStorage()
        {
            ListRank = new ObservableCollection<RankEmployee>();
            var ranklist = DataProvider.Ins.DB.RankEmployees;
            foreach (var item in ranklist)
            {
                ListRank.Add(item);
            }
            ListStorage = new ObservableCollection<Storage>();
            var storagelist = DataProvider.Ins.DB.Storages;
            foreach(var item in storagelist)
            {
                ListStorage.Add(item);
            }
        }
        #endregion

        #region ICommand
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand BrowseCommand { get; set; }
        #endregion
        public EmployeeViewModel()
        {
            LoadDataEmployee();
            LoadDataRankAndStorage();
            ListSex = new string[] { "Nam", "Nu" };

            #region BrowseCommand
            BrowseCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem == null)
                        return false;
                    var idList = DataProvider.Ins.DB.Employees.Where(x => x.IdEmployee == SelectedItem.employee.IdEmployee);
                    if (idList != null && idList.Count() != 0)
                        return true;
                    return false;
                },
                (p) =>
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "Image File | *.png; *.jpg; *.jpeg";
                    if (dialog.ShowDialog() == true)
                    {
                        tmpImageEmp = dialog.FileName;
                    }
                });
            #endregion

            #region Add Command
            AddCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (string.IsNullOrEmpty(NewName) || string.IsNullOrEmpty(NewSex) || string.IsNullOrEmpty(NewPhoneNumber) || string.IsNullOrEmpty(NewAddress))
                        return false;
                    return true;
                },
                (p) =>
                {
                    var employee = new Employee() { NameEmployee = NewName, Sex = NewSex, Birthday = NewBirthday, DateOfStart = NewDateOfStart, IdRank = Convert.ToInt32(NewIDRank), IdStorage = Convert.ToInt32(NewIDStorage), PhoneNumber = NewPhoneNumber, AddressEmp = NewAddress, ImageEmp = NewImageEmp };

                    DataProvider.Ins.DB.Employees.Add(employee);
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Đã thêm một nhân viên mới!", "Thông báo");
                    var rank = DataProvider.Ins.DB.RankEmployees.Where(x => x.IdRank == employee.IdRank);
                    var storage = DataProvider.Ins.DB.Storages.Where(x => x.IdStorage == employee.IdStorage);
                    NhanVien nv = new NhanVien();
                    nv.employee = employee;

                    foreach (var item1 in rank)
                    {
                        nv.rank = item1.NameRank;
                    }
                    foreach (var item1 in storage)
                    {
                        nv.storage = item1.NameStorage;
                    }
                    ListEmployee.Add(nv);
                    NewName = NewSex = NewIDRank = NewIDStorage = NewPhoneNumber = NewAddress = NewImageEmp = null;
                    NewBirthday = NewDateOfStart = null;
                });
            #endregion

            #region Edit Command
            EditCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem == null)
                        return false;
                    var idList = DataProvider.Ins.DB.Employees.Where(x => x.IdEmployee == SelectedItem.employee.IdEmployee);
                    if (idList != null && idList.Count() != 0)
                        return true;
                    return false;
                },
                (p) =>
                {
                    var Employee = DataProvider.Ins.DB.Employees.Where(x => x.IdEmployee == SelectedItem.employee.IdEmployee).SingleOrDefault();
                    Employee.NameEmployee = NameEmployee;
                    Employee.Sex = Sex;
                    Employee.Birthday = Birthday;
                    Employee.DateOfStart = DateOfStart;
                    Employee.PhoneNumber = PhoneNumber;
                    Employee.AddressEmp = AddressEmp;
                    Employee.ImageEmp = tmpImageEmp;
                    Employee.IdRank = Convert.ToInt32(NewIDRank);
                    Employee.IdStorage = Convert.ToInt32(NewIDStorage);

                    DataProvider.Ins.DB.SaveChanges();
                    LoadDataEmployee();
                });
            #endregion

            #region Delete Command
            DeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem == null)
                        return false;
                    if (DataProvider.Ins.DB.InputInfoes.Where(x => x.IdEmployee == SelectedItem.employee.IdEmployee).Count() == 0 && DataProvider.Ins.DB.OutputInfoes.Where(x => x.IdEmployee == SelectedItem.employee.IdEmployee).Count() == 0)
                        return true;
                    return false;
                },
                (p) => 
                {
                    DataProvider.Ins.DB.Employees.Remove(SelectedItem.employee);
                    DataProvider.Ins.DB.SaveChanges();
                    NhanVien nv = new NhanVien();
                    nv.employee = SelectedItem.employee;
                    ListEmployee.Remove(ListEmployee.Where(x => x.employee.IdEmployee == nv.employee.IdEmployee).Single());
                    SelectedItem = null;
                });
            #endregion
        }
    }
}