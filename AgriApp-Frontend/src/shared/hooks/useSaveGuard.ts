import { useMemo } from "react";
interface UseSaveGuardOptions {
  isSubmitting: boolean;
  isLoading: boolean;
  isDirty?: boolean;
  hasChanges?: boolean;
}

export function useSaveGuard({
  isSubmitting,
  isLoading,
  isDirty = true,
  hasChanges = true,
}: UseSaveGuardOptions) {
  const isDisabled = useMemo(() => {
    if (isSubmitting) return true;
    if (isLoading) return true;
    if (!isDirty && !hasChanges) return true;

    return false;
  }, [isSubmitting, isLoading, isDirty, hasChanges]);

  return {
    isDisabled,
  };
}
