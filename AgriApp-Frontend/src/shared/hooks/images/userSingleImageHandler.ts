import { useEffect, useMemo, useState } from "react";

export function useSingleImage(initialImage?: string) {
  /* ------------ STATE ------------ */

  const [imageFile, setImageFile] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(initialImage ?? null);

  /* ------------ INIT ------------ */

  useEffect(() => {
    setPreview(initialImage ?? null);
    setImageFile(null);
  }, [initialImage]);

  /* ------------ SELECT ------------ */

  const selectImage = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files || !e.target.files[0]) return;

    const file = e.target.files[0];
    setImageFile(file);
    setPreview(URL.createObjectURL(file));
  };

  /* ------------ REMOVE ------------ */

  const removeImage = () => {
    setImageFile(null);
    setPreview(null);
  };

  /* ------------ CHANGES ------------ */

  const hasChange = useMemo(() => {
    return imageFile !== null;
  }, [imageFile]);

  /* ------------ CLEANUP ------------ */

  useEffect(() => {
    return () => {
      if (preview?.startsWith("blob:")) {
        URL.revokeObjectURL(preview);
      }
    };
  }, [preview]);

  return {
    imageFile, // ترسلها للسيرفر
    preview, // للعرض
    selectImage,
    removeImage,
    hasChange,
  };
}
