using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Model
{
    public class LoHangNhapXuat
    {
        public StorageDetail Consignment { get; set; }
        public int Employee { get; set; }
        public Nullable<int> Unit { get; set; }
    }
}
