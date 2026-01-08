import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  
  constructor() { }

  /**
   * Verifica se localStorage está disponível
   */
  private isLocalStorageAvailable(): boolean {
    try {
      const test = '__localStorage_test__';
      if (typeof localStorage !== 'undefined' && localStorage !== null) {
        localStorage.setItem(test, test);
        localStorage.removeItem(test);
        return true;
      }
    } catch (e) {
      return false;
    }
    return false;
  }

  /**
   * Salva um item no localStorage
   */
  setItem(key: string, value: string): void {
    try {
      if (this.isLocalStorageAvailable()) {
        localStorage.setItem(key, value);
      }
    } catch (error) {
      console.error('Erro ao salvar no localStorage:', error);
    }
  }

  /**
   * Obtém um item do localStorage
   */
  getItem(key: string): string | null {
    try {
      if (this.isLocalStorageAvailable()) {
        return localStorage.getItem(key);
      }
      return null;
    } catch (error) {
      console.error('Erro ao obter do localStorage:', error);
      return null;
    }
  }

  /**
   * Remove um item do localStorage
   */
  removeItem(key: string): void {
    try {
      if (this.isLocalStorageAvailable()) {
        localStorage.removeItem(key);
      }
    } catch (error) {
      console.error('Erro ao remover do localStorage:', error);
    }
  }

  /**
   * Limpa todo o localStorage
   */
  clear(): void {
    try {
      if (this.isLocalStorageAvailable()) {
        localStorage.clear();
      }
    } catch (error) {
      console.error('Erro ao limpar localStorage:', error);
    }
  }
}
