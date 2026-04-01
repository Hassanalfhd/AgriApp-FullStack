import React, { useEffect } from "react";
import { Wallet, Percent, Calendar, CheckCircle2 } from "lucide-react";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { getFarmerMonthlySales } from "../actThunks/getFarmerMonthlySales";
import Loader from "@/shared/components/ui/Loader";

const FarmerFinancialDashboard: React.FC<{ farmerId: number }> = ({
  farmerId,
}) => {
  console.log(farmerId);
  const dispatch = useAppDispatch();
  const { farmerFinancialSummary, loading, error } = useAppSelector(
    (state) => state.reports,
  );

  useEffect(() => {
    const now = new Date();
    dispatch(
      getFarmerMonthlySales({
        farmerId,
        month: now.getMonth() + 1,
        year: now.getFullYear(),
      }),
    );
  }, [dispatch, farmerId]);

  if (loading)
    return (
      <div className="h-64 bg-slate-50 animate-pulse rounded-3xl">
        <Loader />
      </div>
    );
  if (!farmerFinancialSummary)
    return <div className="h-64 bg-slate-50 rounded-3xl">{error}</div>;
  // حساب نسبة الصافي من الإجمالي للعرض المرئي
  const netPercentage =
    (farmerFinancialSummary.netRevenue / farmerFinancialSummary.grossRevenue) *
    100;

  return (
    <div className="space-y-8">
      {/* 1. قسم البطاقات المالية الكبرى (The Hero Section) */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* الإيراد الكلي */}
        <div className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100 relative overflow-hidden">
          <div className="absolute -right-4 -top-4 bg-blue-50 w-20 h-20 rounded-full flex items-center justify-center">
            <Wallet size={30} className="text-blue-200 ml-4 mt-4" />
          </div>
          <p className="text-xs font-bold text-slate-400 mb-1 uppercase tracking-wider">
            إجمالي المبيعات
          </p>
          <h3 className="text-3xl font-black text-slate-800">
            ${farmerFinancialSummary.grossRevenue.toLocaleString()}
          </h3>
        </div>

        {/* عمولة المنصة */}
        <div className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100 relative overflow-hidden">
          <div className="absolute -right-4 -top-4 bg-orange-50 w-20 h-20 rounded-full flex items-center justify-center">
            <Percent size={30} className="text-orange-200 ml-4 mt-4" />
          </div>
          <p className="text-xs font-bold text-slate-400 mb-1 uppercase tracking-wider">
            عمولة المنصة
          </p>
          <h3 className="text-3xl font-black text-orange-500">
            -${farmerFinancialSummary.platformCommission.toLocaleString()}
          </h3>
        </div>

        {/* الصافي (The Star Card) */}
        <div className="bg-green-600 p-6 rounded-3xl shadow-xl shadow-green-100 relative overflow-hidden text-white">
          <div className="absolute -right-4 -top-4 bg-green-500 w-20 h-20 rounded-full flex items-center justify-center">
            <CheckCircle2 size={30} className="text-green-400 ml-4 mt-4" />
          </div>
          <p className="text-xs font-bold text-green-100 mb-1 uppercase tracking-wider">
            صافي أرباحك
          </p>
          <h3 className="text-3xl font-black">
            ${farmerFinancialSummary.netRevenue.toLocaleString()}
          </h3>

          {/* شريط توضيحي صغير */}
          <div className="mt-4 w-full bg-green-700/50 h-1.5 rounded-full">
            <div
              className="bg-white h-full rounded-full"
              style={{ width: `${netPercentage}%` }}
            ></div>
          </div>
        </div>
      </div>

      {/* 2. جدول التفاصيل (Product Breakdown) */}
      <div className="bg-white rounded-3xl shadow-sm border border-slate-100 overflow-hidden">
        <div className="p-6 border-b border-slate-50 flex justify-between items-center bg-slate-50/30">
          <h3 className="font-black text-slate-800 flex items-center gap-2">
            <Calendar size={20} className="text-blue-500" /> كشف مبيعات الشهر
          </h3>
          <span className="text-xs font-bold text-slate-400">
            {farmerFinancialSummary.reportDate}
          </span>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-right">
            <thead className="bg-slate-50/50 text-slate-400 text-[10px] uppercase font-bold tracking-widest">
              <tr>
                <th className="px-6 py-4">المنتج</th>
                <th className="px-6 py-4">الكمية</th>
                <th className="px-6 py-4">الإيراد</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-50 text-sm">
              {farmerFinancialSummary.monthlyData.map((item, index) => (
                <tr
                  key={index}
                  className="hover:bg-slate-50/50 transition-colors"
                >
                  <td className="px-6 py-4 font-bold text-slate-700">
                    {item.productName}
                  </td>
                  <td className="px-6 py-4 text-slate-500">
                    {item.totalQuantitySold} وحدة
                  </td>
                  <td className="px-6 py-4 font-black text-blue-600">
                    ${item.totalRevenue.toLocaleString()}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default FarmerFinancialDashboard;
