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
    class CustomerViewModel : BaseViewModel
    {
        #region ObservableCollection
        private ObservableCollection<KhachHang> _ListCustomer;
        public ObservableCollection<KhachHang> ListCustomer { get => _ListCustomer; set { _ListCustomer = value; OnPropertyChanged(); } }
        #endregion

        #region SelectedItem
        private KhachHang _SelectedItem;
        public KhachHang SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    ID = SelectedItem.customer.IdCustomer;
                    Name = SelectedItem.customer.NameCustomer;
                    PhoneNumber = SelectedItem.customer.PhoneNumber;
                    Email = SelectedItem.customer.Email;
                    Address = SelectedItem.customer.AddressCus;
                    ContractDate = SelectedItem.customer.ContractDate;
                }
            }
        }
        #endregion

        #region Create field to binding Customer's Information
        private int _ID;
        public int ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private string _PhoneNumber;
        public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private DateTime? _ContractDate;
        public DateTime? ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }
        #endregion

        #region Create field to add new Customer
        private string _NewName;
        public string NewName { get => _NewName; set { _NewName = value; OnPropertyChanged(); } }

        private string _NewPhoneNumber;
        public string NewPhoneNumber { get => _NewPhoneNumber; set { _NewPhoneNumber = value; OnPropertyChanged(); } }

        private string _NewEmail;
        public string NewEmail { get => _NewEmail; set { _NewEmail = value; OnPropertyChanged(); } }

        private string _NewAddress;
        public string NewAddress { get => _NewAddress; set { _NewAddress = value; OnPropertyChanged(); } }

        private DateTime? _NewContractDate;
        public DateTime? NewContractDate { get => _NewContractDate; set { _NewContractDate = value; OnPropertyChanged(); } }
        #endregion

        #region Filter on ListView
        private string _Filter;
        public string Filter { get => _Filter; set { _Filter = value; OnFilterChanged(); } }

        void FilterListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListCustomer);
            view.Filter = UserFilter;
        }

        private void OnFilterChanged()
        {
            CollectionViewSource.GetDefaultView(ListCustomer).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Filter))
                return true;
            else
                return ((item as KhachHang).customer.NameCustomer.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as KhachHang).customer.PhoneNumber.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as KhachHang).customer.Email.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as KhachHang).customer.AddressCus.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion

        #region Load Data into ListView
        void LoadDataCustomer()
        {
            ListCustomer = new ObservableCollection<KhachHang>();

            var objectList = DataProvider.Ins.DB.Customers;

            foreach (var item in objectList)
            {
                KhachHang kh = new KhachHang();
                kh.customer = item;
                ListCustomer.Add(kh);
            }
        }
        #endregion

        #region ICommand
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion

        public CustomerViewModel()
        {
            LoadDataCustomer();
            FilterListView();

            #region AddCommand
            AddCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (string.IsNullOrEmpty(NewName) || string.IsNullOrEmpty(NewPhoneNumber) || string.IsNullOrEmpty(NewEmail) || string.IsNullOrEmpty(NewAddress) || NewContractDate == null)
                        return false;
                    return true;
                },
                (p) =>
                {
                    var customer = new Customer() { NameCustomer = NewName, PhoneNumber = NewPhoneNumber, Email = NewEmail, AddressCus = NewAddress, ContractDate = NewContractDate };
                    DataProvider.Ins.DB.Customers.Add(customer);
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Đã thêm một khách hàng mới!", "Thông báo");
                    KhachHang kh = new KhachHang();
                    kh.customer = customer;
                    ListCustomer.Add(kh);
                    NewName = NewPhoneNumber = NewEmail = NewAddress = null;
                    NewContractDate = null;
                });
            #endregion

            #region EditCommand
            EditCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem == null)
                        return false;
                    var idList = DataProvider.Ins.DB.Customers.Where(x => x.IdCustomer == SelectedItem.customer.IdCustomer);
                    if (idList != null && idList.Count() != 0)
                        return true;
                    return false;
                },
                (p) =>
                {
                    var Customer = DataProvider.Ins.DB.Customers.Where(x => x.IdCustomer == SelectedItem.customer.IdCustomer).SingleOrDefault();
                    Customer.NameCustomer = Name;
                    Customer.PhoneNumber = PhoneNumber;
                    Customer.Email = Email;
                    Customer.AddressCus = Address;
                    Customer.ContractDate = ContractDate;
                    DataProvider.Ins.DB.SaveChanges();
                });
            #endregion

            #region DeleteCommand
            DeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem == null)
                        return false;
                    if (DataProvider.Ins.DB.Outputs.Where(x => x.IdCustomer == SelectedItem.customer.IdCustomer).Count() == 0)
                        return true;
                    return false;
                },
                (p) => 
                {
                    DataProvider.Ins.DB.Customers.Remove(SelectedItem.customer);
                    DataProvider.Ins.DB.SaveChanges();
                    KhachHang kh = new KhachHang();
                    kh.customer = SelectedItem.customer;
                    ListCustomer.Remove(ListCustomer.Where(x => x.customer.IdCustomer == kh.customer.IdCustomer).Single());
                    SelectedItem = null;
                });
            #endregion
        }
    }
}
