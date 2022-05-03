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
    class SupplierViewModel : BaseViewModel
    {
        #region ObservableCollection
        private ObservableCollection<NhaCungCap> _ListSupplier;
        public ObservableCollection<NhaCungCap> ListSupplier { get => _ListSupplier; set { _ListSupplier = value; OnPropertyChanged(); } }
        #endregion

        #region SelectedItem
        private NhaCungCap _SelectedItem;
        public NhaCungCap SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    ID = SelectedItem.supplier.IdSuplier;
                    Name = SelectedItem.supplier.NameSuplier;
                    PhoneNumber = SelectedItem.supplier.PhoneNumber;
                    Email = SelectedItem.supplier.Email;
                    Address = SelectedItem.supplier.AddressSup;
                    ContractDate = SelectedItem.supplier.ContractDate;
                }
            }
        }
        #endregion

        #region Create field to binding Supplier's Information
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

        #region Create field to add new Supplier
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

        #region Bộ lọc trong ListView
        private string _Filter;
        public string Filter { get => _Filter; set { _Filter = value; OnFilterChanged(); } }

        void FilterListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListSupplier);
            view.Filter = UserFilter;
        }

        private void OnFilterChanged()
        {
            CollectionViewSource.GetDefaultView(ListSupplier).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Filter))
                return true;
            else
                return ((item as NhaCungCap).supplier.NameSuplier.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as NhaCungCap).supplier.PhoneNumber.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as NhaCungCap).supplier.Email.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as NhaCungCap).supplier.AddressSup.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion

        #region LoadData cho ListView
        void LoadDataSupplier()
        {
            ListSupplier = new ObservableCollection<NhaCungCap>();

            var objectList = DataProvider.Ins.DB.Supliers;

            foreach (var item in objectList)
            {
                NhaCungCap ncc = new NhaCungCap();
                ncc.supplier = item;
                ListSupplier.Add(ncc);
            }
        }
        #endregion

        #region ICommand
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion
        public SupplierViewModel()
        {
            LoadDataSupplier();
            FilterListView();
            #region Add Command
            AddCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (string.IsNullOrEmpty(NewName) || string.IsNullOrEmpty(NewPhoneNumber) || string.IsNullOrEmpty(NewEmail) || string.IsNullOrEmpty(NewAddress))
                        return false;
                    return true;
                },
                (p) =>
                {
                    var supplier = new Suplier() { NameSuplier = NewName, PhoneNumber = NewPhoneNumber, Email = NewEmail, AddressSup = NewAddress, ContractDate = NewContractDate};
                    DataProvider.Ins.DB.Supliers.Add(supplier);
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Đã thêm một nhà cung cấp mới!", "Thông báo");
                    NhaCungCap ncc = new NhaCungCap();
                    ncc.supplier = supplier;
                    ListSupplier.Add(ncc);
                    NewName = NewPhoneNumber = NewEmail = NewAddress = null;
                    NewContractDate = null;
                });
            #endregion

            #region Edit Command
            EditCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem == null)
                        return false;
                    var idList = DataProvider.Ins.DB.Supliers.Where(x => x.IdSuplier == SelectedItem.supplier.IdSuplier);
                    if (idList != null && idList.Count() != 0)
                        return true;
                    return false;
                },
                (p) =>
                {
                    var Supplier = DataProvider.Ins.DB.Supliers.Where(x => x.IdSuplier == SelectedItem.supplier.IdSuplier).SingleOrDefault();
                    Supplier.NameSuplier = Name;
                    Supplier.PhoneNumber = PhoneNumber;
                    Supplier.Email = Email;
                    Supplier.AddressSup = Address;
                    Supplier.ContractDate = ContractDate;
                    DataProvider.Ins.DB.SaveChanges();
                });
            #endregion

            #region Delete Command
            DeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem == null)
                        return false;
                    if (DataProvider.Ins.DB.Inputs.Where(x => x.IdSuplier == SelectedItem.supplier.IdSuplier).Count() == 0 && DataProvider.Ins.DB.Products.Where(x => x.IdSuplier == SelectedItem.supplier.IdSuplier).Count() == 0)
                        return true;
                    return false;
                },
                (p) =>
                {
                    DataProvider.Ins.DB.Supliers.Remove(SelectedItem.supplier);
                    DataProvider.Ins.DB.SaveChanges();
                    NhaCungCap ncc = new NhaCungCap();
                    ncc.supplier = SelectedItem.supplier;
                    ListSupplier.Remove(ListSupplier.Where(x => x.supplier.IdSuplier == ncc.supplier.IdSuplier).Single());
                    SelectedItem = null;
                });
            #endregion

        }
    }
}
