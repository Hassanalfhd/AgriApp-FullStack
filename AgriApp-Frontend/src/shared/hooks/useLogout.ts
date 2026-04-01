import { persistor } from "../../app/store";
import { logout } from "../../features/auth/slices/authSlice";
import { ROUTES } from "../constants/ROUTESLinks";
import { useAppDispatch } from "./StoreHook";
import { useNavigate } from "react-router-dom";

export const useLogout = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const handleLogout = async () => {
    dispatch(logout());
    await persistor.purge();
    navigate(`${ROUTES.CLIENT.LoginIn}`);
  };
  return handleLogout;
};
