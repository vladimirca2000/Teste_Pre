export interface Category {
  id?: number;
  name: string;
  isDelete: boolean;
  createdAt?: Date;
  createdUser?: string;
  updatedAt?: Date;
  updatedUser?: string;
}

export interface CategoryDTO {
  name: string;
}
