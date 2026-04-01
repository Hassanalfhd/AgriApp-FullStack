import { formatCurrency } from "@/shared/utils/formatCurrency";
import {
  Package,
  Tag,
  User,
  XCircle,
  CheckCircle,
  Calendar,
  PackageCheck,
} from "lucide-react";
import type { itemStatus } from "../types/orderDto";

// 1. تعريف الحقول المشتركة الإجبارية للطرفين
interface BaseItem {
  orderItemId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  total: number;
  status: itemStatus;
}

// 2. تعريف الـ Props لتكون Generic (T)
interface ItemsGridProps<T extends BaseItem> {
  items: T[];
  mode: "Customer" | "Farmer";
  OnCanceled?: (id: number) => void;
  OnAccepted?: (id: number) => void;
  OnPickUp?: (id: number) => void;
}

const ItemsGrid = <T extends BaseItem>({
  items,
  mode,
  OnCanceled,
  OnAccepted,
  OnPickUp,
}: ItemsGridProps<T>) => {
  // حماية من القيم الفارغة أو null
  if (!items || items.length === 0) {
    return (
      <div className="p-10 text-center text-slate-400">No items available.</div>
    );
  }

  return (
    <div className="p-4 md:p-6 bg-white/50">
      <h4 className="text-[10px] font-black text-slate-400 mb-4 uppercase tracking-[0.2em] flex items-center gap-2">
        <Package size={14} className="text-emerald-500" />
        {mode === "Farmer" ? "Order Inventory" : "Your Selected Products"}
      </h4>

      <div className="grid grid-cols-1 gap-3">
        {items.map((item) => (
          <div
            key={item.orderItemId}
            className="flex flex-col md:flex-row md:items-center justify-between p-4 bg-white border border-slate-100 rounded-2xl hover:shadow-md transition-shadow"
          >
            {/* القسم الأيمن: معلومات المنتج */}
            <div className="flex items-center gap-4">
              <div className="w-10 h-10 bg-slate-50 rounded-xl flex items-center justify-center text-slate-400">
                <Tag size={18} />
              </div>
              <div>
                <h5 className="font-bold text-slate-900 text-sm">
                  {item.productName}
                </h5>
                <div className="flex items-center gap-3 mt-1">
                  <span className="text-[11px] font-bold text-emerald-600 bg-emerald-50 px-2 py-0.5 rounded-md">
                    {item.status}
                  </span>
                  {/* إظهار اسم العميل فقط في وضع المزارع */}
                  {mode === "Farmer" && (
                    <>
                      {"customerName" in item && (
                        <span className="text-[11px] text-slate-500 flex items-center gap-1 font-medium border-l pl-3 border-slate-200">
                          <User size={12} /> {(item as any).customerName}
                        </span>
                      )}

                      {"orderDate" in item && (
                        <span className="text-[11px] text-slate-400 flex items-center gap-1 font-medium border-l pl-3 border-slate-200">
                          <Calendar size={12} />
                          {new Date(
                            (item as any).orderDate,
                          ).toLocaleDateString()}
                        </span>
                      )}
                    </>
                  )}

                  {mode === "Customer" && (
                    <>
                      {"farmerName" in item && (
                        <span className="text-[11px] text-slate-500 flex items-center gap-1 font-medium border-l pl-3 border-slate-200">
                          <User size={12} /> {(item as any).farmerName}
                        </span>
                      )}
                    </>
                  )}
                </div>
              </div>
            </div>

            {/* القسم الأيسر: الأرقام والأكشن */}
            <div className="flex items-center justify-between md:gap-10 mt-4 md:mt-0 pt-3 md:pt-0 border-t md:border-t-0 border-slate-50">
              <div className="text-center">
                <p className="text-[10px] text-slate-400 uppercase font-bold">
                  Qty
                </p>
                <p className="text-sm font-black text-slate-700">
                  {item.quantity}
                </p>
              </div>
              <div className="text-center">
                <p className="text-[10px] text-slate-400 uppercase font-bold">
                  Price
                </p>
                <p className="text-sm font-black text-slate-900">
                  {formatCurrency(item.total)}
                </p>
              </div>

              {/* الأزرار التفاعلية */}
              <div className="flex items-center gap-2 ml-4">
                {/* زر القبول للمزارع فقط */}
                {mode === "Farmer" &&
                  item.status.toLowerCase() === "pending" && (
                    <>
                      <button
                        onClick={() => OnAccepted?.(item.orderItemId)}
                        className="p-2 bg-emerald-600 text-white rounded-lg hover:bg-emerald-700 shadow-sm"
                        title="Accept Item"
                      >
                        <CheckCircle size={16} />
                      </button>
                    </>
                  )}
                {mode === "Farmer" &&
                  (item.status === "Pending" || item.status === "Accepted") && (
                    <button
                      onClick={() => OnPickUp?.(item.orderItemId)}
                      className="p-2 bg-emerald-600 text-white rounded-lg hover:bg-emerald-700 shadow-sm"
                      title="Pick up Item"
                    >
                      <PackageCheck size={16} />
                    </button>
                  )}

                {/* زر الإلغاء (يظهر للطرفين إذا كانت الحالة تسمح) */}
                {item.status !== "Cancelled" &&
                  item.status !== "Shipped" &&
                  item.status !== "ReadyForPickup" && (
                    <button
                      onClick={() => OnCanceled?.(item.orderItemId)}
                      className="p-2 text-red-600 hover:text-red-900 hover:bg-red-50 rounded-lg transition-colors"
                      title="Cancel Item"
                    >
                      <XCircle size={18} />
                    </button>
                  )}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ItemsGrid;
