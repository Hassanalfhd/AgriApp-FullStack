export interface CartState {
  userId: number;
  items: CartItems[];
  totalAmount: number;
  cartCount: number;
}

export interface CartItems {
  id: number;
  image: string | null;
  farmerName: string;
  ItemName: string;
  productId: number;
  quantity: number;
  status: number;
  price: number;
}
