import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface CacheEntry<T> {
  data: T;
  timestamp: number;
  expiresAt?: number;
}

@Injectable({
  providedIn: 'root'
})
export class CacheService {
  private cachePrefix = 'app_cache_';
  private cacheMap = new Map<string, any>();
  private cacheInvalidated$ = new BehaviorSubject<string | null>(null);

  constructor() {
    this.loadFromLocalStorage();
  }

  /**
   * Obtém um item do cache
   */
  get<T>(key: string): T | null {
    const entry = this.cacheMap.get(key) as CacheEntry<T>;
    
    if (!entry) {
      return null;
    }

    // Verificar se expirou
    if (entry.expiresAt && new Date().getTime() > entry.expiresAt) {
      this.invalidate(key);
      return null;
    }

    return entry.data;
  }

  /**
   * Define um item no cache
   */
  set<T>(key: string, data: T, ttlMinutes?: number): void {
    const entry: CacheEntry<T> = {
      data,
      timestamp: new Date().getTime(),
      expiresAt: ttlMinutes ? new Date().getTime() + (ttlMinutes * 60 * 1000) : undefined
    };

    this.cacheMap.set(key, entry);
    this.saveToLocalStorage(key, entry);
  }

  /**
   * Obtém do cache ou executa função se não existir
   */
  getOrSet<T>(
    key: string,
    fn: () => Observable<T>,
    ttlMinutes?: number
  ): Observable<T> {
    const cached = this.get<T>(key);
    
    if (cached) {
      return new Observable(observer => {
        observer.next(cached);
        observer.complete();
      });
    }

    return new Observable(observer => {
      fn().subscribe({
        next: (data) => {
          this.set(key, data, ttlMinutes);
          observer.next(data);
          observer.complete();
        },
        error: (error) => observer.error(error)
      });
    });
  }

  /**
   * Invalida um item do cache
   */
  invalidate(key: string): void {
    this.cacheMap.delete(key);
    this.removeFromLocalStorage(key);
    this.cacheInvalidated$.next(key);
  }

  /**
   * Invalida todos os itens que começam com um prefixo
   */
  invalidatePattern(pattern: string): void {
    const keysToDelete: string[] = [];
    
    this.cacheMap.forEach((value, key) => {
      if (key.includes(pattern)) {
        keysToDelete.push(key);
      }
    });

    keysToDelete.forEach(key => {
      this.invalidate(key);
    });
  }

  /**
   * Limpa todo o cache
   */
  clear(): void {
    this.cacheMap.clear();
    this.clearAllFromLocalStorage();
  }

  /**
   * Observable para monitorar invalidações
   */
  onInvalidated(): Observable<string | null> {
    return this.cacheInvalidated$.asObservable();
  }

  /**
   * Salva cache no localStorage
   */
  private saveToLocalStorage(key: string, entry: any): void {
    try {
      const fullKey = this.cachePrefix + key;
      localStorage.setItem(fullKey, JSON.stringify(entry));
    } catch (error) {
      console.error('Erro ao salvar cache no localStorage:', error);
    }
  }

  /**
   * Carrega cache do localStorage na inicialização
   */
  private loadFromLocalStorage(): void {
    try {
      for (let i = 0; i < localStorage.length; i++) {
        const key = localStorage.key(i);
        if (key && key.startsWith(this.cachePrefix)) {
          const cleanKey = key.substring(this.cachePrefix.length);
          const value = localStorage.getItem(key);
          if (value) {
            this.cacheMap.set(cleanKey, JSON.parse(value));
          }
        }
      }
    } catch (error) {
      console.error('Erro ao carregar cache do localStorage:', error);
    }
  }

  /**
   * Remove item do localStorage
   */
  private removeFromLocalStorage(key: string): void {
    try {
      const fullKey = this.cachePrefix + key;
      localStorage.removeItem(fullKey);
    } catch (error) {
      console.error('Erro ao remover do localStorage:', error);
    }
  }

  /**
   * Limpa todos os itens de cache do localStorage
   */
  private clearAllFromLocalStorage(): void {
    try {
      const keysToDelete: string[] = [];
      
      for (let i = 0; i < localStorage.length; i++) {
        const key = localStorage.key(i);
        if (key && key.startsWith(this.cachePrefix)) {
          keysToDelete.push(key);
        }
      }

      keysToDelete.forEach(key => localStorage.removeItem(key));
    } catch (error) {
      console.error('Erro ao limpar cache do localStorage:', error);
    }
  }

  /**
   * Retorna informações do cache (DEBUG)
   */
  debug(): void {
    console.log('Cache Debug:');
    this.cacheMap.forEach((value, key) => {
      console.log(`  ${key}:`, value);
    });
  }
}
