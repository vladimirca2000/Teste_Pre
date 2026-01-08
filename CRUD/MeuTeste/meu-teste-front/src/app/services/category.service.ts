import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../shared/services/api.service';
import { CacheService } from '../shared/services/cache.service';
import { Category, CategoryDTO } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private endpoint = 'categories';
  private cacheKey = 'categories_list';
  private cacheTTL = 10; // 10 minutos

  constructor(
    private apiService: ApiService,
    private cacheService: CacheService
  ) { }

  /**
   * Obter todas as categorias (com cache)
   */
  getAll(): Observable<Category[]> {
    return this.cacheService.getOrSet(
      this.cacheKey,
      () => this.apiService.get<Category[]>(this.endpoint),
      this.cacheTTL
    );
  }

  /**
   * Obter categoria por ID
   */
  getById(id: number): Observable<Category> {
    return this.apiService.get<Category>(`${this.endpoint}/${id}`);
  }

  /**
   * Criar nova categoria
   */
  create(category: CategoryDTO): Observable<Category> {
    return this.apiService.post<Category>(this.endpoint, category);
  }

  /**
   * Atualizar categoria
   */
  update(id: number, category: CategoryDTO): Observable<Category> {
    return this.apiService.put<Category>(`${this.endpoint}/${id}`, category);
  }

  /**
   * Deletar categoria
   */
  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }

  /**
   * Invalidar cache de categorias
   */
  invalidateCache(): void {
    this.cacheService.invalidate(this.cacheKey);
  }
}
