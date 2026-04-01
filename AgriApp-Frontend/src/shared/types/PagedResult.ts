// Generic PagedResult لدعم أي نوع بيانات
export interface PagedResult<T> {
  items: T[]; // قائمة العناصر في الصفحة الحالية
  page: number; // الصفحة الحالية
  pageSize: number; // عدد العناصر في الصفحة
  totalCount: number; // إجمالي عدد العناصر في جميع الصفحات
  totalPages: number; // إجمالي عدد الصفحات
}

