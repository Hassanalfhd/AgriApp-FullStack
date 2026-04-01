import type { ReactNode } from "react";
import { useAppSelector } from "../../hooks/StoreHook";

interface FabProps {
  onClick: () => void;
  icon: ReactNode;
}

export function Fab({ onClick, icon }: FabProps) {
  const userType = useAppSelector((s) => s.auth.user?.role);

  if (userType === "Admin" || userType === "Farmer") {
    return (
      <button
        onClick={onClick}
        className="
        fixed bottom-6 right-6
        w-16 h-16 rounded-full
        bg-green-600 text-white
        flex items-center justify-center
        shadow-lg hover:bg-green-700
        active:scale-95 transition
        z-10
      "
      >
        {icon}
      </button>
    );
  } else return <></>;
}
