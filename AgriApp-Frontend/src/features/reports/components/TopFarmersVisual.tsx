import React, { useEffect } from "react";
import { Trophy, Medal, ArrowUpRight, Loader2 } from "lucide-react";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { getTopFarmers } from "../actThunks/getTopFarmers";

const TopFarmersVisual: React.FC = () => {
  const dispatch = useAppDispatch();
  const { topFarmers, loading, error } = useAppSelector(
    (state) => state.reports,
  );

  useEffect(() => {
    dispatch(getTopFarmers(5));
  }, [dispatch]);

  if (loading)
    return (
      <div className="h-64 flex flex-col items-center justify-center gap-2 text-slate-400">
        <Loader2 className="animate-spin text-amber-500" size={28} />
        <p className="text-xs font-medium">Loading top performers...</p>
      </div>
    );

  if (error)
    return (
      <div className="h-64 flex flex-col items-center justify-center gap-2 text-slate-400">
        <p className="text-xs font-medium">{error}</p>
      </div>
    );
  return (
    <div className="w-full h-full">
      {/* Header Section */}
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-sm font-bold text-slate-400 uppercase tracking-widest flex items-center gap-2">
          <Trophy className="text-amber-500" size={18} /> Platform Leaders
        </h2>
        <button className="text-indigo-600 text-xs font-bold hover:text-indigo-700 transition-colors">
          View All
        </button>
      </div>

      {/* Farmers List */}
      <div className="space-y-3">
        {topFarmers.map((farmer, index) => (
          <div
            key={farmer.farmerId}
            className={`group relative flex items-center p-3 rounded-xl border transition-all duration-300 ${
              index === 0
                ? "bg-gradient-to-r from-amber-50/50 to-transparent border-amber-100 shadow-sm"
                : "bg-white border-slate-100 hover:border-indigo-100 hover:shadow-md hover:shadow-indigo-50/50"
            }`}
          >
            {/* Rank Icon / Number */}
            <div className="mr-4 flex-shrink-0">
              {index === 0 ? (
                <div className="bg-amber-500 p-2 rounded-lg shadow-md shadow-amber-200">
                  <Trophy size={16} className="text-white" />
                </div>
              ) : index === 1 ? (
                <Medal size={22} className="text-slate-400" />
              ) : index === 2 ? (
                <Medal size={22} className="text-orange-400" />
              ) : (
                <div className="w-8 h-8 rounded-full bg-slate-50 flex items-center justify-center text-xs font-bold text-slate-400 group-hover:bg-indigo-50 group-hover:text-indigo-500 transition-colors">
                  {index + 1}
                </div>
              )}
            </div>

            {/* Farmer Identity */}
            <div className="flex-1 min-w-0">
              <h4 className="text-sm font-bold text-slate-800 truncate">
                {farmer.farmerName}
              </h4>
              <p className="text-[11px] text-slate-500 font-medium">
                {farmer.totalOrders} Orders{" "}
                <span className="mx-1 text-slate-300">•</span>{" "}
                {farmer.totalProductsSold} Products
              </p>
            </div>

            {/* Revenue Analytics */}
            <div className="text-right ml-2">
              <div className="flex items-center justify-end gap-1 text-emerald-600 font-bold text-sm">
                <span>${farmer.totalRevenue.toLocaleString()}</span>
                <ArrowUpRight size={14} className="opacity-70" />
              </div>
              <div className="text-[10px] text-slate-400 uppercase tracking-tight font-semibold">
                Total Revenue
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default TopFarmersVisual;
