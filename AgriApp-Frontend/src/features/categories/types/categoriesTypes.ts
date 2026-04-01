import type { enLoadinStatus } from "../../../shared/types/enums/enLoadin";

export interface ICategory {
  id: number;
  name: string;
  imageFile?: string;
}

export interface ICategoryToAddEdit {
  id: number;
  name: string;
  imageFile?: File;
}

export interface CategoriesState {
  items: ICategory[];
  loading: enLoadinStatus;
  error: string | null;
  selectedItem: ICategory | null;
}
