import type { enLoadinStatus } from "@/shared/types/enums/enLoadin";

export interface ICrops {
  id: number;
  name: string;
  ownerId: number;
  categoryId: number;
  createdAt: Date;
  username?: string;
  categoryName?: string;
  imagePath?: string;
  imageFile?: File;
}


export interface CropsState {
  items: ICrops[];
  loading: enLoadinStatus;
  error: string | null;
  selectedItem: ICrops | null;
}
