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
    public class ProductViewModel : BaseViewModel
    {
        #region Đổ dữ liệu vào ListView
        private ObservableCollection<SanPham> _ListProduct;
        public ObservableCollection<SanPham> ListProduct { get => _ListProduct; set { _ListProduct = value; OnPropertyChanged(); } }

        void LoadDataProduct()
        {
            ListProduct = new ObservableCollection<SanPham>();

            var productList = DataProvider.Ins.DB.Products;

            foreach (var item in productList)
            {
                var type = DataProvider.Ins.DB.TypeProducts.Where(p => p.IdType == item.IdType).SingleOrDefault();
                var suplier = DataProvider.Ins.DB.Supliers.Where(p => p.IdSuplier == item.IdSuplier).SingleOrDefault();
                SanPham sp = new SanPham() { Product = item, Type = type.NameType, Suplier = suplier.NameSuplier };
                ListProduct.Add(sp);
            }
        }
        #endregion

        #region Nhóm trong ListView
        void GroupListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListProduct);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
            view.GroupDescriptions.Add(groupDescription);
        }
        #endregion

        #region Bộ lọc trong ListView
        private string _Filter;
        public string Filter { get => _Filter; set { _Filter = value; OnFilterChanged(); } }

        void FilterListView()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListProduct);
            view.Filter = UserFilter;
        }

        private void OnFilterChanged()
        {
            CollectionViewSource.GetDefaultView(ListProduct).Refresh();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(Filter))
                return true;
            else
                return ((item as SanPham).Product.NameProduct.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as SanPham).Product.IdProduct.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as SanPham).Type.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as SanPham).Suplier.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion

        #region Chọn dữ liệu dưới ListView Binding đến các TextBlock
        private string _IdProduct;
        public string IdProduct { get => _IdProduct; set { _IdProduct = value; OnPropertyChanged(); } }

        private string _NameProduct;
        public string NameProduct { get => _NameProduct; set { _NameProduct = value; OnPropertyChanged(); } }

        private string _TypeProduct;
        public string TypeProduct { get => _TypeProduct; set { _TypeProduct = value; OnPropertyChanged(); } }

        private string _Suplier;
        public string Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }

        private SanPham _SelectedItem;
        public SanPham SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    IdProduct = SelectedItem.Product.IdProduct;
                    NameProduct = SelectedItem.Product.NameProduct;
                    TypeProduct = SelectedItem.Type;
                    Suplier = SelectedItem.Suplier;
                }
            }
        }
        #endregion

        #region Đổ dữ liệu vào 2 combobox mục Thêm
        private ObservableCollection<TypeProduct> _ListType;
        public ObservableCollection<TypeProduct> ListType { get => _ListType; set { _ListType = value; OnPropertyChanged(); } }

        private ObservableCollection<Suplier> _ListSuplier;
        public ObservableCollection<Suplier> ListSuplier { get => _ListSuplier; set { _ListSuplier = value; OnPropertyChanged(); } }
        void LoadDataTypeAndSuplier()
        {
            ListType = new ObservableCollection<TypeProduct>();
            var typeList = DataProvider.Ins.DB.TypeProducts;
            foreach (var item in typeList)
            {
                ListType.Add(item);
            }

            ListSuplier = new ObservableCollection<Suplier>();
            var typeSuplier = DataProvider.Ins.DB.Supliers;
            foreach (var item in typeSuplier)
            {
                ListSuplier.Add(item);
            }
        }
        #endregion

        #region Thêm một sản phẩm
        private string _NewIdProduct;
        public string NewIdProduct { get => _NewIdProduct; set { _NewIdProduct = value; OnPropertyChanged(); } }

        private string _NewNameProduct;
        public string NewNameProduct { get => _NewNameProduct; set { _NewNameProduct = value; OnPropertyChanged(); } }

        private int _NewTypeProduct = 0;
        public int NewTypeProduct { get => _NewTypeProduct; set { _NewTypeProduct = value; OnPropertyChanged(); } }

        private int _NewSuplier = 0;
        public int NewSuplier { get => _NewSuplier; set { _NewSuplier = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        #endregion

        #region Xóa một sản phẩm
        public ICommand DeleteCommand { get; set; }
        #endregion

        public void Reload()
        {
            LoadDataProduct();
            GroupListView();
            FilterListView();
            LoadDataTypeAndSuplier();
        }

        public ProductViewModel()
        {
            Reload();

            // Command thêm sản phẩm
            AddCommand = new RelayCommand<object>(
                (p) => {
                    if (string.IsNullOrEmpty(NewIdProduct) || string.IsNullOrEmpty(NewNameProduct) || NewSuplier == 0 || NewTypeProduct == 0)
                        return false;
                    var listIdProduct = DataProvider.Ins.DB.Products.Where(x => x.IdProduct == NewIdProduct);
                    if (listIdProduct.Count() != 0)
                        return false;
                    return true;
                },
                (p) => {
                    var product = new Product() { IdProduct = NewIdProduct, NameProduct = NewNameProduct, IdType = NewTypeProduct, IdSuplier = NewSuplier };
                    DataProvider.Ins.DB.Products.Add(product);
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Đã thêm một mặt hàng mới!", "Thông báo");
                    // Load lại dữ liệu
                    LoadDataProduct();
                    NewIdProduct = "";
                    NewNameProduct = "";
                    LoadDataTypeAndSuplier();
                    GroupListView();
                });

            // Command xóa sản phẩm
            DeleteCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedItem == null)
                        return false;
                    var product = DataProvider.Ins.DB.StorageDetails.Where(x => x.IdProduct == IdProduct).FirstOrDefault();
                    if (product != null)
                        return false;
                    return true;
                },
                (p) =>
                {
                    if (MessageBox.Show("Bạn có muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Hand) == MessageBoxResult.Yes)
                    {
                        var product = DataProvider.Ins.DB.Products.Where(x => x.IdProduct == IdProduct).SingleOrDefault();
                        DataProvider.Ins.DB.Products.Remove(product);
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Xóa sản phẩm thành công", "Thông báo");
                        // Load lại dữ liệu
                        LoadDataProduct();
                        GroupListView();
                        IdProduct = Suplier = NameProduct = TypeProduct = null;
                    }
                });
        }
    }
}
