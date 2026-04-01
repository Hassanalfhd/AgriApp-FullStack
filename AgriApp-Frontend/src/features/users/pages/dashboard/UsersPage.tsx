import GenericListPage from "@/shared/components/generic/ListPage/GenericListPage";
import type { Column } from "@/shared/components/generic/types/Column";
import type { FilterField } from "@/shared/components/generic/types/FilterField";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { useCallback, useEffect } from "react";
import { Fab } from "@/shared/components/ui/Fab";
import { ROUTES } from "@/shared/constants/ROUTESLinks";
import { useNavigate } from "react-router-dom";
import { Plus, UserCheck, UserX } from "lucide-react";
import Loader from "@/shared/components/ui/Loader";
import { getAllUsersThunk } from "../../actThunks/getAllUsersThunk";
import type { UserResponseDto } from "../../types/userTypes";
import Button from "@/shared/components/ui/Button";
import { toggleUserStatusThunk } from "../../actThunks/toggleUserStatusThunk";
// استورد الـ Thunk الخاص بتغيير الحالة هنا
// import { toggleUserStatusThunk } from "../../actThunks/toggleUserStatusThunk";

export default function UsersBoard() {
  const dispatch = useAppDispatch();
  const navigator = useNavigate();

  const { loading } = useAppSelector((state) => state.users);
  const {
    items: users,
    page,
    pageSize,
    totalPages,
  } = useAppSelector((s) => s.users.data);
  const userRole = useAppSelector((s) => s.auth.user?.role);

  const getUsers = useCallback(
    (newPage: number) => {
      if (userRole === "Admin") {
        dispatch(
          getAllUsersThunk({
            page: newPage,
            pageSize,
          }),
        );
      }
    },
    [dispatch, userRole, pageSize],
  );

  useEffect(() => {
    getUsers(page);
  }, [getUsers, page]);

  const handlePageChange = (newPage: number) => {
    getUsers(newPage);
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  const columns: Column<UserResponseDto>[] = [
    { key: "fullName", label: "Full Name" },
    { key: "username", label: "Username" },
    { key: "email", label: "Email" },
    {
      key: "isActive",
      label: "Status",
      render: (user) => (
        <span
          className={`px-2 py-1 rounded-full text-xs ${user.isActive ? "bg-green-100 text-green-700" : "bg-red-100 text-red-700"}`}
        >
          {user.isActive ? "Active" : "Inactive"}
        </span>
      ),
    },
    { key: "userType", label: "User Type" },
    { key: "createdAt", label: "Date" },
  ];

  const usersFilters: FilterField[] = [
    {
      key: "search",
      label: "Search",
      type: "text",
      placeholder: "Search by name or email...",
      searchMode: "enter",
    },
  ];

  if (loading === "pending" && users.length === 0) return <Loader />;

  return (
    <>
      <Fab
        icon={<Plus />}
        onClick={() => navigator(`${ROUTES.ADMIN.USERS.ADD}`)}
      />

      <GenericListPage<UserResponseDto>
        title="Users Management"
        columns={columns}
        data={users}
        filters={usersFilters}
        filterState={() => {}}
        // تعديل المستخدم
        onEdit={(id) => navigator(`${ROUTES.ADMIN.USERS.LIST}/${id}/edit`)}
        // تفعيل/إيقاف الحساب (الزر الإضافي المخصص)
        renderExtraActions={(user) => (
          <Button
            className={`p-1.5 rounded-md transition-all shadow-sm ${
              user.isActive
                ? "bg-amber-600 hover:bg-amber-700 text-white"
                : "bg-indigo-600 hover:bg-indigo-700 text-white"
            }`}
            onClick={() => {
              // هنا تنفذ الـ Action الخاص بتغيير الحالة
              console.log("Toggle status for:", user.id);
              dispatch(
                toggleUserStatusThunk({
                  userId: user.id,
                  isActive: !user.isActive,
                }),
              );
            }}
            title={user.isActive ? "Deactivate User" : "Activate User"}
          >
            {user.isActive ? <UserX size={16} /> : <UserCheck size={16} />}
          </Button>
        )}
        // منطق الحذف (الذي يعمل كـ InActive)
        onDeleteAction={(id) => {}}
        // منطق البحث والفلترة
        onFilterChangeAction={(key) => {
          // dispatch(setUserFilter({ name: key, value }));
        }}
        onSearchAction={() => getUsers(1)}
        onResetAction={() => {
          // dispatch(resetUserFilters());
          getUsers(1);
        }}
        deleteMessage="Are you sure you want to change this user's status?"
        actions={{
          showDelete: false,
          showEdit: false,
          showToggleStatus: true,
        }}
        currentPage={page}
        totalPages={totalPages}
        handlePageChange={handlePageChange}
      />
    </>
  );
}
