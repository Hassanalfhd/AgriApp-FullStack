export interface IProductToCreate {
  id?: number;
  name: string;
  description?: string;
  quantityInStock: number;
  cropTypeId: number;
  quantityTypeId: number;
  price: number;
  createdBy: number;
}
