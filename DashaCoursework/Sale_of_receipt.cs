//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashaCoursework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sale_of_receipt
    {
        public int Id { get; set; }
        public Nullable<int> Id_Receipt { get; set; }
        public Nullable<int> Id_Sale { get; set; }
    
        public virtual Receipt Receipt { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
