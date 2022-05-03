using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhoHang.Model
{
    class BaoCao
    {
        public string IDProduct { get; set; }
        public string NameProduct { get; set; }
        public string NameStorage { get; set; }
        public int TotalInput { get; set; }
        public int TotalOutput { get; set; }
        public int TotalUnit { get; set; }
    }
    class BaoCaoTong
    {
        public string IDProduct { get; set; }
        public string NameProduct { get; set; }
        public int TotalInput { get; set; }
        public int TotalOutput { get; set; }
        public int TotalUnit { get; set; }
    }
}
