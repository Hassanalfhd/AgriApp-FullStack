import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { useForm } from "react-hook-form";
import { toast } from "react-hot-toast";
import { Edit, User, Calendar, Mail, Type, Phone } from "lucide-react";
import { formatDate } from "@/shared/utils/formatDate";
import Loader from "@/shared/components/ui/Loader";
import { updateProfileThunk } from "../actThunk/updateProfileThunk";
import { profile } from "../actThunk/getUserProfile";
import { Input } from "@/shared/components/ui/Input";
import type { IUserProfile } from "../types/IUserProfile";

const ProfilePage = () => {
  const dispatch = useAppDispatch();
  const { user, loading } = useAppSelector((s) => s.auth);
  const [previewImage, setPreviewImage] = useState<string | null>(null);

  const { register, handleSubmit, watch, reset } = useForm<IUserProfile>({
    defaultValues: {
      fullName: "",
      username: "",
      phone: "",
    },
  });

  const imageWatch = watch("imageFile");

  // Fetch profile on mount
  useEffect(() => {
    dispatch(profile());
  }, [dispatch]);

  // Reset form when user changes
  useEffect(() => {
    if (user) {
      reset({
        fullName: user.fullName,
        username: user.username,
        phone: user.phone,
      });
      setPreviewImage(user.imageFile || "/default-avatar.png");
    }
  }, [user, reset]);

  // Preview image when selecting new file
  useEffect(() => {
    if (imageWatch && imageWatch.length > 0) {
      const file = imageWatch[0];
      setPreviewImage(URL.createObjectURL(file));
    }
  }, [imageWatch]);

  const onSubmit = async (data: IUserProfile) => {
    try {
      await dispatch(
        updateProfileThunk({
          fullName: data.fullName,
          username: data.username,
          imageFile: data.imageFile,
          phone: data.phone,
        }),
      ).unwrap();
      toast.success("Profile updated successfully!");
      dispatch(profile()); // Refresh profile
    } catch (err: any) {
      toast.error(err?.message || "Update failed!");
    }
  };

  if (!user) return <Loader />;

  return (
    <main className="min-h-screen flex justify-center items-start bg-gray-50 p-4">
      <section className="bg-white w-full max-w-2xl rounded-xl shadow-lg p-8 space-y-6">
        {/* Avatar */}
        <div className="flex flex-col items-center relative">
          <img
            src={previewImage!}
            alt="User Avatar"
            loading="lazy"
            className="w-32 h-32 rounded-full object-cover border border-gray-300"
          />
          <label
            htmlFor="imageFile"
            className="absolute bottom-0  bg-green-600 p-2 rounded-full text-white cursor-pointer hover:bg-green-700 transition"
            aria-label="Change profile picture"
          >
            <Edit className="w-3 h-3" />
          </label>
          <input
            type="file"
            id="imageFile"
            {...register("imageFile")}
            className="hidden"
            accept="image/*"
          />
        </div>

        {/* Profile Info */}
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-4">
            <div className="flex flex-col">
              <label className="text-sm font-medium w-full md:w-40 flex items-center gap-2">
                <User className="w-4 h-4" /> Full Name
              </label>
              <Input
                type="text"
                {...register("fullName")}
                className="flex-1 border rounded p-2 focus:ring focus:ring-green-300 focus:outline-none"
              />
            </div>

            <div className="flex flex-col ">
              <label className="text-sm font-medium w-full md:w-40 flex items-center gap-2">
                <User className="w-4 h-4" /> Username
              </label>
              <Input
                type="text"
                {...register("username")}
                className="flex-1 border rounded p-2 focus:ring focus:ring-green-300 focus:outline-none"
              />
            </div>
            <div className="flex flex-col ">
              <label className="text-sm font-medium flex items-center gap-2">
                <Mail className="w-4 h-4" /> Email
              </label>
              <p className="p-2 border rounded bg-gray-50">{user.email}</p>
            </div>

            <div className="flex flex-col ">
              <label className="text-sm font-medium w-full md:w-40 flex items-center gap-2">
                <Phone className="w-4 h-4" /> Phone
              </label>
              <Input
                maxLength={14}
                minLength={9}
                type="text"
                {...register("phone")}
                className="flex-1 border rounded p-2 focus:ring focus:ring-green-300 focus:outline-none"
              />
            </div>

            <div className="flex flex-col">
              <label className="text-sm font-medium flex items-center gap-2">
                <Type className="w-4 h-4" /> UserType
              </label>
              <p className="p-2 border rounded bg-gray-50">{user.role}</p>
            </div>

            <div className="flex flex-col">
              <label className="text-sm font-medium flex items-center gap-2">
                <Calendar className="w-4 h-4" /> Created At
              </label>
              <p className="p-2 border rounded bg-gray-50">
                {/* {new Date(user.createdAt!).toLocaleDateString()} */}
                {formatDate(user.createdAt!)}
              </p>
            </div>
          </div>

          {/* Single Update Button */}
          <button
            type="submit"
            disabled={loading === "pending"}
            className="w-full bg-green-600 text-white py-3 rounded-lg hover:bg-green-700 transition disabled:opacity-50 focus:outline-none focus:ring-2 focus:ring-green-400 focus:ring-offset-2 mt-4"
          >
            {loading === "pending" ? "Updating..." : "Update Profile"}
          </button>
        </form>
      </section>
    </main>
  );
};

export default ProfilePage;
