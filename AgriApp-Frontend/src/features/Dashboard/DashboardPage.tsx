import React from "react";
import { LayoutDashboard, UserCircle, Lightbulb, Wallet } from "lucide-react";
import { useAppSelector } from "@/shared/hooks/StoreHook";
import TopFarmersList from "../reports/components/TopFarmersVisual";
import SalesGrowthChart from "../reports/components/SalesGrowthChart";
import CategoryPieChart from "../reports/components/CategoryPieChart";
import LowStockVisualAlerts from "../reports/components/LowStockVisualAlerts";
import FarmerFinancialDashboard from "../reports/components/FarmerFinancialDashboard";

const Dashboard: React.FC = () => {
  const { user } = useAppSelector((state) => state.auth);

  // Reusable Card with better padding and clear headers
  const DashboardCard = ({
    children,
    title,
    icon: Icon,
    className = "",
  }: {
    children: React.ReactNode;
    title?: string;
    icon?: any;
    className?: string;
  }) => (
    <div
      className={`bg-white rounded-xl border border-slate-200 shadow-sm p-6 ${className}`}
    >
      {title && (
        <div className="flex items-center gap-2 mb-6 border-b border-slate-50 pb-4">
          {Icon && <Icon className="text-indigo-500" size={20} />}
          <h3 className="text-lg font-bold text-slate-800 uppercase tracking-tight">
            {title}
          </h3>
        </div>
      )}
      <div className="w-full overflow-x-auto">{children}</div>
    </div>
  );

  return (
    <div className="min-h-screen bg-[#f8fafc] p-4 md:p-8 lg:p-10" dir="ltr">
      {/* Header Section */}
      <header className="mb-10">
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-6">
          <div>
            <div className="flex items-center gap-3 mb-2">
              <div className="bg-indigo-600 p-2 rounded-lg">
                <LayoutDashboard className="text-white" size={24} />
              </div>
              <h1 className="text-2xl md:text-3xl font-black text-slate-900">
                {user?.role === "Admin"
                  ? "System Administration"
                  : "Farmer Overview"}
              </h1>
            </div>
            <p className="text-slate-500 flex items-center gap-2 ml-1">
              <UserCircle size={18} />
              Welcome,{" "}
              <span className="font-semibold text-indigo-600">
                {user?.fullName}
              </span>
            </p>
          </div>

          <div className="hidden md:block bg-white px-4 py-2 rounded-lg border border-slate-200 shadow-sm">
            <span className="text-sm text-slate-400 font-medium uppercase tracking-wider">
              Status:{" "}
            </span>
            <span className="text-sm text-green-600 font-bold italic">
              Online
            </span>
          </div>
        </div>
      </header>

      {/* Main Content Layout */}
      <div className="flex flex-col gap-8">
        {/* --- ROLE: ADMIN --- */}
        {user?.role === "Admin" && (
          <>
            {/* Top Row: Financial & Sales (The heavy data) */}
            <section className="grid grid-cols-1 lg:grid-cols-3 gap-8">
              <div className="lg:col-span-2">
                <DashboardCard
                  title="Revenue & Sales Growth"
                  icon={SalesGrowthChart}
                >
                  <SalesGrowthChart />
                </DashboardCard>
              </div>
              <div className="lg:col-span-1">
                <DashboardCard title="Sales by Category">
                  <CategoryPieChart />
                </DashboardCard>
              </div>
            </section>

            {/* Middle Row: The Financial Dashboard (Given Wide Space Now) */}
            <section>
              <DashboardCard
                title="System-Wide Financial Performance"
                icon={Wallet}
              >
                <FarmerFinancialDashboard farmerId={user.id!} />
              </DashboardCard>
            </section>

            {/* Bottom Row: Lists and Alerts */}
            <section className="grid grid-cols-1 lg:grid-cols-2 gap-8">
              <DashboardCard title="Top Performing Farmers">
                <TopFarmersList />
              </DashboardCard>
              <DashboardCard title="Inventory & Stock Alerts">
                <LowStockVisualAlerts />
              </DashboardCard>
            </section>
          </>
        )}

        {/* --- ROLE: FARMER --- */}
        {user?.role === "Farmer" && (
          <>
            {/* Primary Section: Financial (Full Width for maximum readability) */}
            <section>
              <DashboardCard
                title="Detailed Financial Report"
                icon={Wallet}
                className="border-t-4 border-t-indigo-500"
              >
                <FarmerFinancialDashboard farmerId={user.id!} />
              </DashboardCard>
            </section>

            {/* Secondary Section: Alerts and Tips */}
            <section className="grid grid-cols-1 lg:grid-cols-3 gap-8">
              <div className="lg:col-span-2">
                <DashboardCard title="Stock Status Alerts">
                  <LowStockVisualAlerts />
                </DashboardCard>
              </div>

              <div className="lg:col-span-1 flex flex-col gap-6">
                {/* Information Card */}
                <div className="bg-gradient-to-br from-indigo-600 to-blue-700 p-8 rounded-2xl text-white shadow-lg relative overflow-hidden h-full">
                  <div className="relative z-10">
                    <div className="bg-white/20 w-12 h-12 rounded-lg flex items-center justify-center mb-4">
                      <Lightbulb size={28} className="text-yellow-300" />
                    </div>
                    <h4 className="font-bold text-xl mb-3">Smart Insights</h4>
                    <p className="text-indigo-50 leading-relaxed opacity-90">
                      Regularly updating your stock levels helps our system
                      optimize your visibility to potential buyers. Aim for
                      weekly updates!
                    </p>
                  </div>
                  {/* Abstract Background Circle */}
                  <div className="absolute -right-10 -bottom-10 w-40 h-40 bg-white/5 rounded-full"></div>
                </div>
              </div>
            </section>
          </>
        )}
      </div>
    </div>
  );
};

export default Dashboard;
