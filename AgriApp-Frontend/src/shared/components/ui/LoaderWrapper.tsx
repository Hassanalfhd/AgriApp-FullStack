// src/shared/components/LoaderWrapper.tsx
import { type ReactNode, Suspense } from "react";
import Loader from "@/shared/components/ui/Loader";

interface LoaderWrapperProps {
  children: ReactNode;
}

export default function LoaderWrapper({ children }: LoaderWrapperProps) {
  return <Suspense fallback={<Loader />}>{children}</Suspense>;
}
