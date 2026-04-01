using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Enums
{
        public enum UserRole { Admin = 1, Farmer = 2, Customer = 3 , AgriculturalExpert  = 4, User = 5 }

    public enum OrderStatus
    {
        Pending = 1,        // الطلب تم إنشاؤه وبانتظار دفع أو مراجعة
        Confirmed = 2,      // تم التأكد من الطلب وبدأ المزارعون العمل
        PartiallyShipped = 3, // مزارع شحن ومنتجات أخرى لسه (مهم جداً في نظامك)
        Completed = 4,      // جميع المزارعين سلموا منتجاتهم
        Cancelled = 5       // العميل ألغى الطلب بالكامل
    }



    public enum ItemStatus
        {
            Pending = 1,        // المزارع لم يرى الطلب بعد
            Accepted = 2,       // المزارع وافق على تجهيز المنتج
            ReadyForPickup = 3, // المزارع جهز المحصول بانتظار شركة الشحن
            Shipped = 4,        // المنتج خرج من عند المزارع
            Cancelled = 5       // المزارع رفض الطلب (مثلاً المحصول غير متوفر)
        }
    public enum enProductStatus { Pending = 1, Approved = 2, Rejected = 3 };


}
