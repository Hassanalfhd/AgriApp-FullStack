import React, { useEffect, useMemo, useState } from "react";

export interface ImageFileToAdd {
  id?: number;
  imageFile: File;
  imageOrder?: number;
}

interface UseImagesManagerProps<T> {
  initialImages: T[];
  getImageSrc: (img: T) => string;
  getImageId: (img: T) => number;
  single?: boolean;
}

export function useImagesManager<T>({
  initialImages = [],
  getImageSrc,
  getImageId,
  single = false,
}: UseImagesManagerProps<T>) {
  const [oldImages, setOldImages] = useState<T[]>(initialImages);

  const [newImages, setNewImages] = useState<ImageFileToAdd[]>([]);

  const [deleteIds, setDeleteIds] = useState<number[]>([]);

  useEffect(() => {
    setOldImages(initialImages);
  }, [initialImages]);

  const previews = useMemo(() => {
    const oldPreviews = oldImages.map(getImageSrc);
    const newPreviews = newImages.map((i) => URL.createObjectURL(i.imageFile));

    return [...oldPreviews, ...newPreviews];
  }, [oldImages, newImages, getImageSrc]);

  // clean the URLs
  useEffect(() => {
    return () => {
      previews.forEach((url) => URL.revokeObjectURL(url));
    };
  }, [previews]);

  // select new images
  const selectImages = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files) return;

    const files = Array.from(e.target.files).map((file, index) => ({
      imageFile: file,
      imagesOrder: newImages.length + index + 1,
    }));

    if (single) {
      setNewImages(files.slice(0, 1));
      setOldImages([]);
      return;
    }

    setNewImages((prev) => [...prev, ...files]);
  };

  //   Remove Images

  const removeImage = (index: number) => {
    if (index < oldImages.length) {
      const img = oldImages[index];

      if (getImageId) {
        const id = getImageId(img);
        setDeleteIds((prev) => [...prev, id]);
      }

      setOldImages((prev) => prev.filter((_, i) => i !== index));
      return;
    }

    const newIndex = index - oldImages.length;

    setNewImages((prev) => prev.filter((_, i) => i !== newIndex));
  };

  const hasChanges = useMemo(() => {
    return newImages.length > 0 || deleteIds.length > 0;
  }, [newImages, deleteIds]);

  return {
    previews,
    selectImages,
    removeImage,
    newImages,
    hasChanges,

    deleteIds,
    oldImages,
  };
}
