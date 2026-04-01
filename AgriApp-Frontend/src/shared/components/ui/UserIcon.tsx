import { ROUTES } from "@/shared/constants/ROUTESLinks";
import { Link } from "react-router-dom";

interface UserIconProps {
  userImage?: string;
  userName?: string;
  userId?: number;
}

export default function UserIcon({
  userImage,
  userName,
  userId,
}: UserIconProps) {
  return (
    <Link
      to={`${ROUTES.CLIENT.PROFILE}/${userId}`}
      className="flex items-center gap-2  text-white px-3 py-2  text-sm font-medium hover:text-green-800 transition"
    >
      <span className="text-sm  text-green-600 ">@{userName}</span>
      <img
        className="rounded-full w-10 h-10  "
        src={userImage}
        alt={`Avatar of ${userName || "User"}`}
        loading="lazy"
      />
    </Link>
  );
}
