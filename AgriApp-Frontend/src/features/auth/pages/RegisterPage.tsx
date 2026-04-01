import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";
import { registerUserThunk } from "../actThunk/registerUserThunk";
import { registerSchema } from "../validation/registerSchema";
import PasswordStrength from "../components/PasswordStrength";
import { Input } from "@/shared/components/ui/Input";
import toast from "react-hot-toast";
import { ROUTES } from "@/shared/constants/ROUTESLinks";

type RegisterForm = {
  id?: number;
  fullName: string;
  username: string;
  phone: string;
  email: string;
  password: string;
  userType: number;
};

const RegisterPage = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { loading, error } = useAppSelector((s) => s.auth);

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<RegisterForm>({
    resolver: yupResolver(registerSchema),
    defaultValues: {
      userType: 2,
    },
  });

  const passwordValue = watch("password");

  const onSubmit = async (data: RegisterForm) => {
    try {
      await dispatch(registerUserThunk(data)).unwrap();
      toast.success("Account created successfully");
      navigate(`${ROUTES.CLIENT.LoginIn}`);
    } catch (err: any) {
      toast.error(err || "Registration failed");
    }
  };

  useEffect(() => {
    if (error) toast.error(error);
  }, [error]);

  return (
    <>
      <main className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
        <section className="bg-white w-full max-w-md rounded-xl shadow p-8">
          <h1 className="text-2xl font-semibold text-center mb-6">
            Create Account
          </h1>

          <form onSubmit={handleSubmit(onSubmit)} noValidate>
            {/* Full Name */}
            <Input
              id="fullName"
              label="Full Name"
              error={errors.fullName?.message}
              {...register("fullName")}
            />

            {/* Phone */}
            <Input
              id="phoneNumber"
              label="Phone Number"
              type="text"
              error={errors.phone?.message}
              {...register("phone")}
            />

            {/* Username */}
            <Input
              id="userName"
              label="Username"
              error={errors.username?.message}
              {...register("username")}
            />

            {/* Email */}
            <Input
              id="email"
              label="Email"
              type="email"
              error={errors.email?.message}
              {...register("email")}
            />

            {/* Password */}
            <Input
              id="password"
              label="Password"
              type="password"
              error={errors.password?.message}
              {...register("password")}
            />
            <PasswordStrength password={passwordValue || ""} />

            {/* User Type */}
            <div className="mb-4">
              <label className="text-sm font-medium">Account Type</label>
              <select
                {...register("userType")}
                className="w-full border rounded-lg p-3 mt-1"
              >
                <option value={2}>Farmer</option>
                <option value={3}>Customer</option>
                <option value={4}>Agricultural Guide</option>
              </select>
            </div>

            <button
              type="submit"
              disabled={loading === "pending"}
              className="w-full bg-green-600 text-white py-3 rounded-lg
                         hover:bg-green-700 transition disabled:opacity-50"
            >
              {loading === "pending" ? "Creating..." : "Register"}
            </button>
          </form>
        </section>
      </main>
    </>
  );
};

export default RegisterPage;
