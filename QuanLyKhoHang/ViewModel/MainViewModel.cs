using QuanLyKhoHang.Model;
using QuanLyKhoHang.UserControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyKhoHang.ViewModel
{
    
    public class MainViewModel : BaseViewModel
    {
        // Giao diện chính
        private UIElement _CurrentViewModel;
        public UIElement CurrentViewModel { get => _CurrentViewModel; set { _CurrentViewModel = value; OnPropertyChanged(); } }

        // Các thành phần trong giao diện
        Inventory inventoryUC;
        InventoryViewModel inventory;
        bool isOpenInventory = false;

        UserControl.Product productUC = new UserControl.Product();
        ProductViewModel product;
        bool isOpenProduct = false;

        InputProduct inputUC = new InputProduct();
        InputProductViewModel input;
        bool isOpenInput = false;

        OutputProduct outputUC = new OutputProduct();
        OutputProductViewModel output;
        bool isOpenOutput = false;

        UserControl.Employee employeeUC = new UserControl.Employee();
        bool isOpenEmployee = false;

        UserControl.Customer customerUC = new UserControl.Customer();
        bool isOpenCustomer = false;

        UserControl.Supplier supplierUC = new UserControl.Supplier();
        bool isOpenSupplier = false;

        UserControl.Report reportUC = new UserControl.Report();
        bool isOpenReport = false;
        // các nút gọi giao diện
        public ICommand OpenInventoryVM { get; set; }
        public ICommand OpenProductVM { get; set; }
        public ICommand OpenInputVM { get; set; }
        public ICommand OpenOutputVM { get; set; }
        public ICommand OpenEmployeeVM { get; set; }
        public ICommand OpenSupplierVM { get; set; }
        public ICommand OpenCustomerVM { get; set; }
        public ICommand OpenReportVM { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        // hàm khởi động lại các cờ giao diện
        void Reload()
        {
            isOpenInventory = isOpenProduct = isOpenInput = isOpenOutput = isOpenEmployee = isOpenCustomer = isOpenSupplier = isOpenReport = false;
        }
        public MainViewModel()
        {
            inventoryUC = new Inventory();
            CurrentViewModel = inventoryUC;
            isOpenInventory = true;

            #region Nút mở trang
            // nút mở trang tồn kho
            OpenInventoryVM = new RelayCommand<object>(
                (p) => {
                    if (isOpenInventory)
                        return false;
                    return true;
                },
                (p) => {
                    inventory = new InventoryViewModel();
                    inventoryUC.DataContext = inventory;
                    CurrentViewModel = inventoryUC;
                    Reload();
                    isOpenInventory = true;
                });
            // nút mở trang sản phẩm
            OpenProductVM = new RelayCommand<object>(
                (p) => {
                    if (isOpenProduct)
                        return false;
                    return true;
                },
                (p) => {
                    product = new ProductViewModel();
                    productUC.DataContext = product;
                    CurrentViewModel = productUC;
                    Reload();
                    isOpenProduct = true;
                });
            // nút mở trang nhập hàng
            OpenInputVM = new RelayCommand<object>(
                (p) => {
                    if (isOpenInput)
                        return false;
                    return true;
                },
                (p) => {
                    input = new InputProductViewModel();
                    inputUC.DataContext = input;
                    CurrentViewModel = inputUC;
                    Reload();
                    isOpenInput = true;
                });
            // nút mở trang xuất kho
            OpenOutputVM = new RelayCommand<object>(
                (p) => {
                    if (isOpenOutput)
                        return false;
                    return true;
                },
                (p) => {
                    output = new OutputProductViewModel();
                    outputUC.DataContext = output;
                    CurrentViewModel = outputUC;
                    Reload();
                    isOpenOutput = true;
                });
            // nút mở trang nhân viên
            OpenEmployeeVM = new RelayCommand<object>(
                (p) => {
                    if (isOpenEmployee)
                        return false;
                    return true;
                },
                (p) => {
                    CurrentViewModel = employeeUC;
                    Reload();
                    isOpenEmployee = true;
                });
            OpenSupplierVM = new RelayCommand<object>(
                (p) => {
                    if (isOpenSupplier)
                        return false;
                    return true;
                },
                (p) => {
                    CurrentViewModel = supplierUC;
                    Reload();
                    isOpenSupplier = true;
                });
            OpenCustomerVM = new RelayCommand<object>(
                (p) => {
                    if (isOpenCustomer)
                        return false;
                    return true;
                },
                (p) => {
                    CurrentViewModel = customerUC;
                    Reload();
                    isOpenCustomer = true;
                });
            OpenReportVM = new RelayCommand<object>(
                (p) => {
                    if (isOpenReport)
                        return false;
                    return true;
                },
                (p) => {
                    CurrentViewModel = reportUC;
                    Reload();
                    isOpenReport = true;
                });
            #endregion

            LoadedWindowCommand = new RelayCommand<Window>(
                (p) =>
                {
                    return true; //Xet dk
                },
                (p) =>
                {
                    if (p == null)
                        return;
                    p.Hide();
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                    if (loginWindow.DataContext == null)
                        return;
                    var loginVM = loginWindow.DataContext as LoginViewModel;
                    if (loginVM.IsLogin)
                    {
                        p.Show();
                    }
                    else
                        p.Close();
                });
        }
    }
}
