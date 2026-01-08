import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../shared/services/api.service';
import { CacheService } from '../shared/services/cache.service';
import { Product, ProductDTO } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private endpoint = 'products';
  private cacheKey = 'products_list';
  private cacheTTL = 10; // 10 minutos

  constructor(
    private apiService: ApiService,
    private cacheService: CacheService
  ) { }

  /**
   * Obter todos os produtos (com cache)
   */
  getAll(): Observable<Product[]> {
    return this.cacheService.getOrSet(
      this.cacheKey,
      () => this.apiService.get<Product[]>(this.endpoint),
      this.cacheTTL
    );
  }

  /**
   * Obter produto por ID
   */
  getById(id: number): Observable<Product> {
    return this.apiService.get<Product>(`${this.endpoint}/${id}`);
  }

  /**
   * Criar novo produto
   */
  create(product: ProductDTO): Observable<Product> {
    return this.apiService.post<Product>(this.endpoint, product);
  }

  /**
   * Atualizar produto
   */
  update(id: number, product: ProductDTO): Observable<Product> {
    return this.apiService.put<Product>(`${this.endpoint}/${id}`, product);
  }

  /**
   * Deletar produto
   */
  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }

  /**
   * Obter produtos paginados
   */
  getPaginated(page: number, pageSize: number): Observable<any> {
    return this.apiService.get<any>('products/paginated', {
      page,
      pageSize
    });
  }

  /**
   * Invalidar cache de produtos
   */
  invalidateCache(): void {
    this.cacheService.invalidate(this.cacheKey);
  }
}
