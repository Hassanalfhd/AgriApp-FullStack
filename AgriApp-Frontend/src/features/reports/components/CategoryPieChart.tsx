import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { useEffect } from "react";
import {
  PieChart,
  Pie,
  Cell,
  ResponsiveContainer,
  Legend,
  Tooltip,
} from "recharts";
import { getCategoryAnalysis } from "../actThunks/getCategoryAnalysis";

// Modern, professional color palette
const COLORS = ["#6366f1", "#10b981", "#f59e0b", "#ef4444", "#8b5cf6"];

const CategoryPieChart = () => {
  const { categoryAnalysis } = useAppSelector((state) => state.reports);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(getCategoryAnalysis());
  }, [dispatch]);

  // Custom Tooltip for better readability
  const CustomTooltip = ({ active, payload }: any) => {
    if (active && payload && payload.length) {
      return (
        <div className="bg-white p-3 border border-slate-200 shadow-xl rounded-lg">
          <p className="text-sm font-bold text-slate-700">{payload[0].name}</p>
          <p className="text-sm text-indigo-600 font-semibold">
            ${payload[0].value.toLocaleString()}
          </p>
        </div>
      );
    }
    return null;
  };

  return (
    <div className="w-full h-full min-h-[350px] flex flex-col">
      {/* Note: We removed the Title, Background, and Padding here 
          because the parent DashboardCard handles it now.
      */}
      <div className="flex-1 w-full">
        <ResponsiveContainer width="100%" height={300}>
          <PieChart>
            <Pie
              data={categoryAnalysis}
              dataKey="totalRevenue"
              nameKey="categoryName"
              cx="50%"
              cy="50%"
              innerRadius={70} // Increased for a more modern "Donut" look
              outerRadius={90}
              paddingAngle={8}
              stroke="none"
            >
              {categoryAnalysis.map((_, index) => (
                <Cell
                  key={`cell-${index}`}
                  fill={COLORS[index % COLORS.length]}
                  className="hover:opacity-80 transition-opacity cursor-pointer"
                />
              ))}
            </Pie>
            <Tooltip content={<CustomTooltip />} />
            <Legend
              verticalAlign="bottom"
              align="center"
              iconType="circle"
              wrapperStyle={{ paddingTop: "20px" }}
            />
          </PieChart>
        </ResponsiveContainer>
      </div>

      {/* Small Detail: Total Summary Footer */}
      <div className="mt-4 text-center">
        <p className="text-xs text-slate-400 uppercase tracking-widest font-medium">
          Revenue Distribution
        </p>
      </div>
    </div>
  );
};

export default CategoryPieChart;
