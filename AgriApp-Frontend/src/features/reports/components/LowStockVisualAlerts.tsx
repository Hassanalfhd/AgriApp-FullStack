import React, { useEffect } from "react";
import { AlertCircle, Package, ArrowRight, Loader2 } from "lucide-react";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { getLowStockAlerts } from "../actThunks/getLowStockAlerts";

const LowStockVisualAlerts: React.FC = () => {
  const dispatch = useAppDispatch();
  const { lowStockAlerts, loading } = useAppSelector((state) => state.reports);

  useEffect(() => {
    // Critical threshold is set to 10 units
    dispatch(getLowStockAlerts(10));
  }, [dispatch]);

  if (loading)
    return (
      <div className="h-48 flex flex-col items-center justify-center gap-3 text-slate-400">
        <Loader2 className="animate-spin text-red-500" size={28} />
        <p className="text-xs font-medium">Checking inventory levels...</p>
      </div>
    );

  return (
    <div className="w-full h-full">
      {/* Header Section */}
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-sm font-bold text-slate-400 uppercase tracking-widest flex items-center gap-2">
          <AlertCircle className="text-red-500 animate-pulse" size={18} />
          Stock Alerts
        </h2>
        <span className="text-[10px] font-bold bg-red-50 text-red-600 px-2 py-1 rounded-md uppercase border border-red-100">
          Action Required
        </span>
      </div>

      {/* Content Area */}
      <div className="space-y-6">
        {lowStockAlerts.length > 0 ? (
          lowStockAlerts.map((item) => {
            // Visual percentage calculation based on a threshold of 10
            const percentage = Math.min((item.currentStock / 10) * 100, 100);
            const isCritical = item.currentStock <= 2;

            return (
              <div key={item.productId} className="group cursor-pointer">
                <div className="flex justify-between items-end mb-2">
                  <div className="min-w-0 flex-1">
                    <h4 className="text-sm font-bold text-slate-800 group-hover:text-indigo-600 transition-colors truncate pr-2">
                      {item.productName}
                    </h4>
                    <p className="text-[10px] text-slate-400 font-medium">
                      {item.categoryName} <span className="mx-1">•</span>{" "}
                      {item.farmerName}
                    </p>
                  </div>
                  <div className="text-right">
                    <span
                      className={`text-xs font-black ${isCritical ? "text-red-600" : "text-slate-700"}`}
                    >
                      {item.currentStock}
                      <span className="text-[10px] text-slate-400 font-normal ml-1 lowercase">
                        units left
                      </span>
                    </span>
                  </div>
                </div>

                {/* Modern Progress Bar */}
                <div className="w-full h-2 bg-slate-100 rounded-full overflow-hidden">
                  <div
                    className={`h-full transition-all duration-1000 ease-out rounded-full ${
                      isCritical
                        ? "bg-gradient-to-r from-red-600 to-red-400"
                        : "bg-gradient-to-r from-orange-500 to-amber-400"
                    }`}
                    style={{ width: `${percentage}%` }}
                  ></div>
                </div>
              </div>
            );
          })
        ) : (
          /* Empty State */
          <div className="text-center py-10">
            <div className="bg-emerald-50 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
              <Package className="text-emerald-500" size={32} />
            </div>
            <p className="text-slate-500 text-sm font-semibold">
              Inventory Healthy
            </p>
            <p className="text-slate-400 text-xs mt-1">
              All products are well-stocked.
            </p>
          </div>
        )}
      </div>

      {/* Action Button */}
      <button className="w-full mt-8 py-3 bg-slate-50 border border-slate-100 text-slate-500 text-xs font-bold rounded-xl hover:bg-white hover:border-indigo-200 hover:text-indigo-600 flex items-center justify-center gap-2 transition-all group">
        View Inventory Manager
        <ArrowRight
          size={14}
          className="group-hover:translate-x-1 transition-transform"
        />
      </button>
    </div>
  );
};

export default LowStockVisualAlerts;
