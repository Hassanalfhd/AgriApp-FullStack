// src/shared/components/Layout/BaseDashboardLayout.tsx
import type { ReactNode } from "react";

export interface BaseDashboardLayoutProps {
  sidebar: ReactNode;
  header: ReactNode;
  children: ReactNode;
}

export default function BaseDashboardLayout({
  sidebar,
  header,
  children,
}: BaseDashboardLayoutProps) {
  return (
    <div className="flex min-h-screen bg-gray-50">
      {/* Sidebar (fixed) */}
      {sidebar}

      {/* Main area */}
      <div className="flex flex-col flex-1 lg:ml-64 transition-all duration-300">
        {/* Header */}
        {header}

        {/* Page content */}
        <div className="flex-1">{children}</div>
      </div>
    </div>
  );
}
