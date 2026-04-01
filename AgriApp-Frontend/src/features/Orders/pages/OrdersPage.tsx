import { useEffect, useMemo, useState } from "react";
import {
  Calendar,
  Hash,
  ShoppingCart,
  AlertCircle,
  Receipt,
  XCircle,
  Clock,
  CheckCircle2,
} from "lucide-react";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { getUserOrders } from "../actThunks/getUserOrders";
import { canceledOrderItem } from "../actThunks/canceledOrderItem";
import toast from "react-hot-toast";
import { formatCurrency } from "@/shared/utils/formatCurrency";
import ItemsGrid from "../components/ItemsGrid";
import Loader from "@/shared/components/ui/Loader";
import { canceledOrder } from "../actThunks/canceledOrder";

type TabType = "pending" | "cancelled" | "completed";

const OrdersPage = () => {
  const dispatch = useAppDispatch();
  const [activeTab, setActiveTab] = useState<TabType>("pending"); // البداية بـ pending

  const { orders, isLoading, error } = useAppSelector((state) => state.orders);

  // جلب البيانات من السيرفر بناءً على التبويب المختار
  useEffect(() => {
    dispatch(getUserOrders(activeTab));
  }, [dispatch, activeTab]);

  const displayOrders = useMemo(() => {
    if (!orders) return [];

    return orders.filter((order) => {
      const status = order.status.toLowerCase();
      if (activeTab === "pending") {
        return status !== "cancelled" && status !== "completed";
      }
      if (activeTab === "cancelled") {
        return status === "cancelled";
      }
      if (activeTab === "completed") {
        return status === "completed";
      }
      return true;
    });
  }, [orders, activeTab]);

  const handleCancelOrderItem = async (orderItemId: number) => {
    const resultAction = await dispatch(canceledOrderItem(orderItemId));
    if (resultAction.meta.requestStatus === "fulfilled") {
      toast.success("Order canceled successfully");
    }
  };

  const handleCancelOrder = async (orderId: number) => {
    const resultAction = await dispatch(canceledOrder(orderId));
    if (resultAction.meta.requestStatus === "fulfilled") {
      toast.success("Order canceled successfully");
    }
  };

  if (isLoading === "pending" && displayOrders.length === 0) return <Loader />;

  return (
    <div className="min-h-screen bg-[#F8FAFC] py-12 px-4" dir="ltr">
      <div className="max-w-5xl mx-auto">
        {/* Header Section */}
        <div className="mb-10 flex flex-col md:flex-row md:items-end justify-between gap-6">
          <div>
            <h1 className="text-4xl font-black text-slate-900 flex items-center gap-3">
              <Receipt className="text-emerald-600" size={36} />
              My Orders
            </h1>
            <p className="text-slate-500 mt-2">
              Manage your fresh farm purchases
            </p>
          </div>

          {/* Navigation Tabs - تجلب البيانات من السيرفر فور الضغط */}
          <div className="flex bg-white p-1.5 rounded-2xl border border-slate-200 shadow-sm">
            {[
              { id: "pending", label: "Active", icon: Clock },
              { id: "completed", label: "Completed", icon: CheckCircle2 },
              { id: "cancelled", label: "Canceled", icon: XCircle },
            ].map((tab) => (
              <button
                key={tab.id}
                onClick={() => setActiveTab(tab.id as TabType)}
                className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all ${
                  activeTab === tab.id
                    ? "bg-emerald-600 text-white shadow-lg shadow-emerald-200"
                    : "text-slate-500 hover:bg-slate-50"
                }`}
              >
                <tab.icon size={16} />
                {tab.label}
              </button>
            ))}
          </div>
        </div>

        {error && (
          <div className="mb-6 p-4 bg-red-50 border border-red-100 rounded-2xl flex items-center gap-3 text-red-700">
            <AlertCircle size={20} />
            <p className="font-medium">{error}</p>
          </div>
        )}

        {/* Orders List - نعرض المصفوفة مباشرة لأنها تأتي مفلترة من السيرفر */}
        {displayOrders.length === 0 ? (
          <div className="bg-white rounded-3xl p-16 text-center border border-dashed border-slate-300 shadow-sm">
            <ShoppingCart className="text-slate-200 mx-auto mb-4" size={64} />
            <h3 className="text-xl font-bold text-slate-800">
              No {activeTab} orders
            </h3>
            <p className="text-slate-500 mt-2">
              You don't have any orders in this category.
            </p>
          </div>
        ) : (
          <div className="space-y-8">
            {displayOrders.map((order) => (
              <div
                key={order.orderId}
                className="bg-white rounded-3xl shadow-sm border border-slate-200 overflow-hidden"
              >
                {/* Header Info */}
                <div className="bg-slate-50/80 p-6 border-b border-slate-100 flex flex-wrap justify-between items-center">
                  {/* ... نفس كود الـ Header السابق ... */}
                  <div className="flex gap-8">
                    <div className="space-y-1">
                      <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">
                        Order ID
                      </span>
                      <div className="flex items-center gap-2 font-bold text-slate-900">
                        <Hash size={14} className="text-emerald-500" />
                        {order.orderId}
                      </div>
                    </div>
                    <div className="space-y-1">
                      <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">
                        Date
                      </span>
                      <div className="flex items-center gap-2 text-slate-700 font-semibold">
                        <Calendar size={14} />
                        {new Date(order.createdAt).toLocaleDateString()}
                      </div>
                    </div>
                  </div>
                  <div className="text-right">
                    <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">
                      Total
                    </span>
                    <div className="text-2xl font-black text-emerald-600">
                      {formatCurrency(order.totalAmount)}
                    </div>
                  </div>
                </div>

                <ItemsGrid<orderItemsResponseDto>
                  mode="Customer"
                  items={order.items.flat()}
                  OnCanceled={(itemId) => handleCancelOrderItem(itemId)}
                />

                {/* Footer Status */}
                <div className="px-8 py-5 bg-slate-50/30 flex justify-between items-center border-t border-slate-100">
                  <div className="flex items-center gap-2">
                    <span
                      className={`w-2 h-2 rounded-full ${order.status === "Cancelled" ? "bg-red-500" : "bg-emerald-500"}`}
                    ></span>
                    <span className="text-sm font-bold text-slate-600 uppercase italic">
                      Status: {order.status}
                    </span>
                  </div>

                  {activeTab === "pending" && (
                    <button
                      onClick={() => handleCancelOrder(order.orderId)}
                      className="text-red-500 font-bold text-sm hover:underline"
                    >
                      <XCircle size={18} />
                    </button>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default OrdersPage;
