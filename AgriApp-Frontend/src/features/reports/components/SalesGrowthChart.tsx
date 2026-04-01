import React, { useEffect } from "react";
import {
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  AreaChart,
  Area,
} from "recharts";
import { Loader2 } from "lucide-react";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { getSalesGrowth } from "../actThunks/getSalesGrowth";

const SalesGrowthChart: React.FC = () => {
  const dispatch = useAppDispatch();
  const { salesGrowth, loading } = useAppSelector((state) => state.reports);

  useEffect(() => {
    dispatch(getSalesGrowth());
  }, [dispatch]);

  // Helper to convert month number to Short Name (English)
  const getMonthName = (monthNumber: number) => {
    const date = new Date();
    date.setMonth(monthNumber - 1);
    return date.toLocaleString("en-US", { month: "short" });
  };

  const chartData = salesGrowth.map((item) => ({
    name: `${getMonthName(item.salesMonth)} ${item.salesYear}`,
    revenue: item.currentMonthRevenue,
    growth: item.growthPercentage,
  }));

  if (loading)
    return (
      <div className="h-64 flex flex-col items-center justify-center gap-3 text-slate-400">
        <Loader2 className="animate-spin text-indigo-500" size={32} />
        <p className="text-sm font-medium">Generating growth analytics...</p>
      </div>
    );

  return (
    <div className="w-full h-full">
      {/* Header is handled by parent DashboardCard, 
          so we focus on the legend and chart here 
      */}
      <div className="flex items-center justify-end mb-4">
        <div className="flex items-center gap-4 text-xs font-bold uppercase tracking-wider">
          <span className="flex items-center gap-2 text-slate-500">
            <div className="w-3 h-1 bg-indigo-600 rounded-full"></div>
            Monthly Revenue
          </span>
        </div>
      </div>

      <div className="h-[300px] w-full">
        <ResponsiveContainer width="100%" height="100%">
          <AreaChart
            data={chartData}
            margin={{ top: 10, right: 10, left: -20, bottom: 0 }}
          >
            <defs>
              <linearGradient id="colorRevenue" x1="0" y1="0" x2="0" y2="1">
                <stop offset="5%" stopColor="#4f46e5" stopOpacity={0.2} />
                <stop offset="95%" stopColor="#4f46e5" stopOpacity={0} />
              </linearGradient>
            </defs>
            <CartesianGrid
              strokeDasharray="3 3"
              vertical={false}
              stroke="#f1f5f9"
            />
            <XAxis
              dataKey="name"
              axisLine={false}
              tickLine={false}
              tick={{ fill: "#94a3b8", fontSize: 11 }}
              dy={15}
            />
            <YAxis
              axisLine={false}
              tickLine={false}
              tick={{ fill: "#94a3b8", fontSize: 11 }}
              tickFormatter={(value) =>
                `$${value >= 1000 ? `${value / 1000}k` : value}`
              }
            />
            <Tooltip
              contentStyle={{
                borderRadius: "12px",
                border: "none",
                boxShadow: "0 20px 25px -5px rgb(0 0 0 / 0.1)",
                padding: "12px",
              }}
              formatter={(value: number) => [
                `$${value.toLocaleString()}`,
                "Revenue",
              ]}
              labelStyle={{
                fontWeight: "bold",
                marginBottom: "4px",
                color: "#1e293b",
              }}
            />
            <Area
              type="monotone"
              dataKey="revenue"
              stroke="#4f46e5"
              strokeWidth={3}
              fillOpacity={1}
              fill="url(#colorRevenue)"
              animationDuration={1500}
            />
          </AreaChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default SalesGrowthChart;
