export interface Product {
  id?: number;
  name: string;
  categoryId: number;
  price: number;
  isDelete: boolean;
  createdAt?: Date;
  createdUser?: string;
  updatedAt?: Date;
  updatedUser?: string;
}

export interface ProductDTO {
  name: string;
  categoryId: number;
  price: number;
}

export interface ProductWithCategory extends Product {
  categoryName?: string;
}
