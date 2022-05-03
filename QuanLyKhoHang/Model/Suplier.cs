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
    
    public partial class Suplier : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Suplier()
        {
            this.Inputs = new HashSet<Input>();
            this.Products = new HashSet<Product>();
        }

        private int _IdSuplier;
        public int IdSuplier { get => _IdSuplier; set { _IdSuplier = value; OnPropertyChanged(); } }

        private string _NameSuplier;
        public string NameSuplier { get => _NameSuplier; set { _NameSuplier = value; OnPropertyChanged(); } }

        private string _PhoneNumber;
        public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _AddressSup;
        public string AddressSup { get => _AddressSup; set { _AddressSup = value; OnPropertyChanged(); } }

        private Nullable<System.DateTime> _ContractDate;
        public Nullable<System.DateTime> ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Input> Inputs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
