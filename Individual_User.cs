//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Insurance_сompany
{
    using System;
    using System.Collections.Generic;
    
    public partial class Individual_User
    {
        public int User_Id { get; set; }
        public string F_Name { get; set; }
        public string L_Name { get; set; }
        public string M_Name { get; set; }
        public long Passport_Num { get; set; }
        public int Burth_Date { get; set; }
    
        public virtual User User { get; set; }
    }
}
