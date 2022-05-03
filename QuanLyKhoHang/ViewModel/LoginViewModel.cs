using QuanLyKhoHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyKhoHang.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private int failCount = 0;
        private int RemainningSeconds;
        private string _RemainningText;
        public string RemainningText { get => _RemainningText; set { _RemainningText = value; OnPropertyChanged(); } }

        private bool _IsEnableLogin;
        public bool IsEnableLogin { get => _IsEnableLogin; set { _IsEnableLogin = value; OnPropertyChanged(); } }

        #region Login
        public bool IsLogin { get; set; }
        private string _Username;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        public ICommand LoginCommand { get; set; }
        public ICommand PassWordChangedCommand { get; set; }
        #endregion
        public LoginViewModel()
        {
            IsLogin = false;
            IsEnableLogin = true;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            Username = "";
            Password = "";
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PassWordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }
        void Login(Window p)
        {
            if (p == null)
                return;

            string passEncode = MD5Hash(Base64Encode(Password));
            int account = DataProvider.Ins.DB.Accounts.Where(x => x.Username == Username && x.Passwords == passEncode).Count();
            if(account > 0)
            {
                IsLogin = true;
                p.Close();
            }
            else
            {
                IsLogin = false;
                failCount++;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                if(failCount == 3)
                {
                    RemainningSeconds = 300;
                    dispatcherTimer.Start();
                    IsEnableLogin = false;
                }
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RemainningSeconds--;
            RemainningText = "Thử lại sau " + RemainningSeconds + "s";
            if (RemainningSeconds == 0)
            {
                dispatcherTimer.Stop();
                failCount = 0;
                RemainningText = null;
                IsEnableLogin = true;
            }
        }
    }
}
