using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Insurance_сompany
{
    public static class Manager
    {
        ///  Компьютер
        // public static string connectionString = "Data Source=DESKTOP-5CVQU3F\\SQLEXPRESS;Initial Catalog=InsuranceCompany;Integrated Security=True";
        
        /// Ноутбук
        public static string connectionString = "Data Source=DESKTOP-FEQVFS2\\SQLEXPRESS;Initial Catalog=InsuranceCompany;Integrated Security=True";
        public static Frame MainFrame { get; set; }
    }
}
