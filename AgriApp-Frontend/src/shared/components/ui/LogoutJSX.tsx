import { LogOut } from "lucide-react";

interface ILogOutProps {
  className?: string;
  onLogout?: () => void;
}
export default function LogoutJSX({ className, onLogout }: ILogOutProps) {
  /* 🟦 Footer */
  return (
    <div className="py-4 border-t border-green-600 sm:border-none">
      <button onClick={onLogout} className={className}>
        <LogOut />
        Logout
      </button>
    </div>
  );
}
