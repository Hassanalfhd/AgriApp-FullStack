import { useEffect, useMemo, useState } from "react";
import {
  ClipboardList,
  PackageCheck,
  Truck,
  History,
  AlertCircle,
  Inbox,
  RefreshCw,
  ShoppingCart,
} from "lucide-react";
import { getFramerOrders } from "../../actThunks/getFramerOrders";
import ItemsGrid from "../../components/ItemsGrid";
import { type farmerOrderDto } from "../../types/farmerOrderDtos";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import Loader from "@/shared/components/ui/Loader";
import toast from "react-hot-toast";
import { accetedOrderItem } from "../../actThunks/accetedOrderItem";
import { cancelledOrderItemByFarmer } from "../../actThunks/cancelledOrderItemByFarmer";
import { pickUpOrderItem } from "../../actThunks/pickUpOrderItem";

// تعريف أنواع التبويبات المتاحة في الواجهة
type FarmerTab =
  | "new"
  | "preparing"
  | "readyForPickUp"
  | "shipped"
  | "cancelled";

/**
 * خريطة الربط بين التبويب ورقم الحالة في الباك إند (Enum)
 * Pending = 0, Accepted = 1, ReadyForPickup = 2, Shipped = 3, Cancelled/Completed = 4
 */
const statusMapping: Record<FarmerTab, number> = {
  new: 1, // الطلبات الجديدة المنتظرة
  preparing: 2, // الطلبات التي تمت الموافقة عليها (مرحلة التجهيز)
  readyForPickUp: 3,
  shipped: 4, // الطلبات التي خرجت للشحن
  cancelled: 5,
};

const FarmerOrdersPage = () => {
  const dispatch = useAppDispatch();
  const { items, isLoading, error } = useAppSelector((s) => s.FarmerOrders);

  // حالة التبويب النشط حالياً
  const [activeTab, setActiveTab] = useState<FarmerTab>("new");

  // جلب البيانات من السيرفر في كل مرة يتغير فيها التبويب
  useEffect(() => {
    const statusId = statusMapping[activeTab];
    console.log(statusId);
    dispatch(getFramerOrders(statusId));
  }, [dispatch, activeTab]);

  // دالة لتحديث البيانات يدوياً
  const handleRefresh = () => {
    const statusId = statusMapping[activeTab];
    dispatch(getFramerOrders(statusId));
    toast.success("Orders updated");
  };

  const visibleItems = useMemo(() => {
    return items.filter((item) => {
      if (activeTab === "new") return item.status === "Pending"; // 1 = Pending
      if (activeTab === "preparing")
        return item.status === "Accepted" || item.status === "ReadyForPickup";
      if (activeTab === "readyForPickUp")
        return item.status === "ReadyForPickup";
      if (activeTab === "shipped") return item.status === "Shipped";
      if (activeTab === "cancelled") return item.status === "Cancelled";
      return true;
    });
  }, [items, activeTab]);

  const handleAcceptItem = (id: number) => {
    dispatch(accetedOrderItem(id));
  };

  const handlePickUpItem = (id: number) => {
    dispatch(pickUpOrderItem(id));
  };

  const handleCancelItem = (id: number) => {
    dispatch(cancelledOrderItemByFarmer(id));
  };

  if (isLoading === "pending" && items.length === 0) return <Loader />;

  return (
    <div
      className="max-w-6xl mx-auto py-12 px-4 md:px-8 bg-[#FBFDFA] min-h-screen"
      dir="ltr"
    >
      {/* Header Section */}
      <div className="mb-8 flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-black text-slate-800 flex items-center gap-3">
            <ClipboardList className="text-emerald-600" size={32} />
            Farmer Dashboard
          </h1>
          <p className="text-slate-500 mt-1">
            Track and manage your farm-to-table orders
          </p>
        </div>

        <button
          onClick={handleRefresh}
          className="flex items-center gap-2 text-sm font-bold text-emerald-700 bg-emerald-50 px-4 py-2 rounded-xl hover:bg-emerald-100 transition-colors"
        >
          <RefreshCw
            size={16}
            className={isLoading === "pending" ? "animate-spin" : ""}
          />
          Refresh Data
        </button>
      </div>

      {/* Tabs Navigation */}
      <div className="flex flex-wrap gap-2 mb-8 bg-white p-2 rounded-2xl border border-slate-200 shadow-sm">
        {[
          { id: "new", label: "New Requests", icon: Inbox },
          { id: "preparing", label: "Preparing", icon: PackageCheck },
          {
            id: "readyForPickUp",
            label: "ready for pickup",
            icon: ShoppingCart,
          },
          { id: "shipped", label: "On Way", icon: Truck },
          { id: "cancelled", label: "cancelled", icon: History },
        ].map((tab) => (
          <button
            key={tab.id}
            onClick={() => setActiveTab(tab.id as FarmerTab)}
            className={`flex items-center gap-2 px-6 py-3 rounded-xl text-sm font-bold transition-all ${
              activeTab === tab.id
                ? "bg-emerald-600 text-white shadow-lg shadow-emerald-100"
                : "text-slate-500 hover:bg-slate-50"
            }`}
          >
            <tab.icon size={18} />
            {tab.label}
          </button>
        ))}
      </div>

      {/* Error Message */}
      {error && (
        <div className="mb-6 p-4 bg-red-50 border border-red-100 rounded-2xl flex items-center gap-3 text-red-700">
          <AlertCircle size={20} />
          <p className="font-medium">{error}</p>
        </div>
      )}

      {/* Orders List Content */}
      <div className="bg-white rounded-3xl border border-slate-200 shadow-sm overflow-hidden min-h-[400px]">
        {items.length > 0 ? (
          <ItemsGrid<farmerOrderDto>
            mode="Farmer"
            items={visibleItems}
            OnAccepted={(id) => handleAcceptItem(id)}
            OnCanceled={(id) => handleCancelItem(id)}
            OnPickUp={(id) => handlePickUpItem(id)}
          />
        ) : (
          <div className="flex flex-col items-center justify-center py-24 text-center">
            <div className="w-20 h-20 bg-slate-50 rounded-full flex items-center justify-center mb-4">
              <Inbox size={40} className="text-slate-200" />
            </div>
            <h3 className="text-xl font-bold text-slate-800">
              No {activeTab} items
            </h3>
            <p className="text-slate-500 mt-2 max-w-xs">
              Everything is up to date. New orders will appear here once
              customers start buying.
            </p>
          </div>
        )}

        {/* Loading Overlay (يظهر عند التبديل بين التبويبات) */}
        {isLoading === "pending" && items.length > 0 && (
          <div className="absolute inset-0 bg-white/50 backdrop-blur-[1px] flex items-center justify-center">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-emerald-600"></div>
          </div>
        )}
      </div>
    </div>
  );
};

export default FarmerOrdersPage;
