import React, { useState, useEffect } from "react";

import { useAppDispatch, useAppSelector } from "@/shared/hooks/StoreHook";

import { Input } from "@/shared/components/ui/Input";
import Button from "@/shared/components/ui/Button";

import toast from "react-hot-toast";
import { login } from "../actThunk/loginThunk";
import { profile } from "../actThunk/getUserProfile";
import logo from "@assets/icons/logo.png";

export default function LoginPage() {
  const dispatch = useAppDispatch();

  const { loading, error } = useAppSelector((state) => state.auth);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  useEffect(() => {
    if (error) toast.error(error);
  }, [error]);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();

    const res = await dispatch(login({ email, password }))
      .unwrap()
      .then(() => {
        dispatch(profile());
      });

    if (login.fulfilled.match(res)) toast.success("Login Successfuly");
  };

  return (
    <>
      <div className="flex items-center justify-center min-h-screen bg-gradient-to-br from-green-100 to-green-300">
        <div className="bg-white shadow-2xl rounded-3xl p-10 w-full max-w-md">
          <div className="flex flex-col items-center mb-6">
            <img src={logo} className="w-20 h-20 mb-2" alt="logo" />
            <h1 className="text-2xl font-bold text-green-700">Login</h1>
          </div>
          <form onSubmit={submit} className="space-y-4">
            <Input
              label="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <Input
              label="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
            <Button
              type="submit"
              className="w-full bg-green-600"
              disabled={!loading}
            >
              {!loading ? "login..." : "Login"}
            </Button>
          </form>
          <p className="text-center  text-sm mt-4">
            Don't Have Account?{" "}
            <a href="/register" className="text-green-600 font-semibold">
              Create
            </a>
          </p>
        </div>
      </div>
    </>
  );
}
