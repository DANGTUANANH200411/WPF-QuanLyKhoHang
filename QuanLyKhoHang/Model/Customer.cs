//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyKhoHang.Model
{
    using QuanLyKhoHang.ViewModel;
    using System;
    using System.Collections.Generic;
    
    public partial class Customer : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Outputs = new HashSet<Output>();
        }

        private int _IdCustomer;
        public int IdCustomer { get => _IdCustomer; set { _IdCustomer = value; OnPropertyChanged(); } }

        private string _NameCustomer;
        public string NameCustomer { get => _NameCustomer; set { _NameCustomer = value; OnPropertyChanged(); } }

        private string _PhoneNumber;
        public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _AddressCus;
        public string AddressCus { get => _AddressCus; set { _AddressCus = value; OnPropertyChanged(); } }

        private Nullable<System.DateTime> _ContractDate;
        public Nullable<System.DateTime> ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Output> Outputs { get; set; }

    }
}
