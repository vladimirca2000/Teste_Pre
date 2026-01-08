export interface User {
  id?: number;
  username: string;
  email: string;
  role: 'Admin' | 'User' | 'NotAccess';
  isActive: boolean;
  isDelete: boolean;
  createdAt?: Date;
  createdUser?: string;
  updatedAt?: Date;
  updatedUser?: string;
}

export interface UserDTO {
  username: string;
  email: string;
  password: string;
}
