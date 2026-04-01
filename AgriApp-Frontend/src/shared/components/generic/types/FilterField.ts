export type FilterType = "text" | "select" | "number" | "date";

export interface FilterField {
  key: string;
  label: string;
  type: FilterType;
  options?: { label: string; value: any }[];
  placeholder?: string;
  searchMode?: "change" | "enter";
  suggestions?: any[];
  isLoading?: boolean;
}
