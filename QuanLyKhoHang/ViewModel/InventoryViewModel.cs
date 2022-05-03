using QuanLyKhoHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLyKhoHang.ViewModel
{
    public class InventoryViewModel : BaseViewModel
    {
        #region Đổ dữ liệu vào ListView
        private ObservableCollection<TonKho> _List;
        public ObservableCollection<TonKho> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private void LoadDataInventory()
        {
            List = new ObservableCollection<TonKho>();

            var inventoryList = DataProvider.Ins.DB.StorageDetails;
            foreach (var item in inventoryList)
            {
                var product = DataProvider.Ins.DB.Products.Where(p => p.IdProduct == item.IdProduct);
                var storage = DataProvider.Ins.DB.Storages.Where(p => p.IdStorage == item.IdStorage);

                TonKho tk = new TonKho();
                tk.Inventory = item;

                foreach (var item1 in product)
                {
                    tk.NameProduct = item1.NameProduct;
                }

                foreach (var item2 in storage)
                {
                    tk.NameStorage = item2.NameStorage;
                }

                List.Add(tk);
            }
        }
        #endregion

        #region Nhóm trong ListView và Sắp xếp các sản phẩm theo HSD cận nhất
        void GroupListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(List);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("NameStorage");
            view.GroupDescriptions.Add(groupDescription);

            view.SortDescriptions.Add(new SortDescription("Inventory.ExpDate", ListSortDirection.Ascending));
        }
        #endregion

        #region Đổ dữ liệu vào 2 combobox lọc
        private ObservableCollection<Storage> _ListStorage;
        public ObservableCollection<Storage> ListStorage { get => _ListStorage; set { _ListStorage = value; OnPropertyChanged(); } }

        private ObservableCollection<Product> _ListProduct;
        public ObservableCollection<Product> ListProduct { get => _ListProduct; set { _ListProduct = value; OnPropertyChanged(); } }

        private void LoadDataStorageAndProduct()
        {
            ListStorage = new ObservableCollection<Storage>();
            var storageList = DataProvider.Ins.DB.Storages;
            foreach (var item in storageList)
            {
                ListStorage.Add(item);
            }

            ListProduct = new ObservableCollection<Product>();
            var productList = DataProvider.Ins.DB.Products;
            foreach (var item in productList)
            {
                ListProduct.Add(item);
            }
        }
        #endregion

        #region Bộ lọc trong ListView và Tính tổng tồn kho theo dữ liệu được lọc
        private string _FilterStorage;
        public string FilterStorage { get => _FilterStorage; set { _FilterStorage = value; OnFilterChanged(); } }

        private string _FilterProduct;
        public string FilterProduct { get => _FilterProduct; set { _FilterProduct = value; OnFilterChanged(); } }

        private int _Count;
        public int Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        void FilterListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(List);
            view.Filter = UserFilter;

            // tính tổng tồn kho lần đầu tiên khi chưa lọc
            Count = 0;
            var filteredItems = view.Cast<TonKho>();
            foreach (var item in filteredItems) Count += (int)item.Inventory.Unit;
        }

        private void OnFilterChanged()
        {
            CollectionViewSource.GetDefaultView(List).Refresh();

            // tính tổng tồn kho khi lọc
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(List);
            Count = 0;
            var filteredItems = view.Cast<TonKho>();
            foreach (var item in filteredItems) Count += (int)item.Inventory.Unit;
        }

        private bool UserFilter(object item)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(List);
            Count = 0;
            if (String.IsNullOrEmpty(FilterStorage) && String.IsNullOrEmpty(FilterProduct))
                return true;
            if (!String.IsNullOrEmpty(FilterStorage) && String.IsNullOrEmpty(FilterProduct))
                return ((item as TonKho).NameStorage.IndexOf(FilterStorage, StringComparison.OrdinalIgnoreCase) >= 0);
            if (!String.IsNullOrEmpty(FilterProduct) && String.IsNullOrEmpty(FilterStorage))
                return ((item as TonKho).NameProduct.IndexOf(FilterProduct, StringComparison.OrdinalIgnoreCase) >= 0);
            return ((item as TonKho).NameStorage.IndexOf(FilterStorage, StringComparison.OrdinalIgnoreCase) >= 0)
                    && ((item as TonKho).NameProduct.IndexOf(FilterProduct, StringComparison.OrdinalIgnoreCase) >= 0);

        }
        #endregion

        public void Reload()
        {
            LoadDataInventory();
            GroupListView();
            LoadDataStorageAndProduct();
            FilterListView();
        }

        public InventoryViewModel()
        {
            Reload();
        }
    }
}
