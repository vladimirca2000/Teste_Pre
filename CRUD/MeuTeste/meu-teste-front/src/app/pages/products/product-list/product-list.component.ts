import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ProductService } from '../../../services/product.service';
import { CategoryService } from '../../../services/category.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Product } from '../../../models/product.model';
import { Category } from '../../../models/category.model';
import { of } from 'rxjs';
import { ProductCreateEditDialogComponent } from '../product-create-edit-dialog/product-create-edit-dialog.component';
import { ProductDeleteDialogComponent } from '../product-delete-dialog/product-delete-dialog.component';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
  standalone: false
})
export class ProductListComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  products: Product[] = [];
  dataSource = new MatTableDataSource<any>([]);
  categories: Category[] = [];
  displayedColumns: string[] = ['id', 'name', 'category', 'price', 'actions'];
  isLoading = false;

  // Paginação
  pageSizeOptions = [10, 25, 50];
  showAll = false;

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  loadData(): void {
    this.isLoading = true;

    // Carregar categorias primeiro para mapeamento
    this.categoryService.getAll().subscribe({
      next: (categories: any) => {
        this.categories = categories;
        this.loadProducts();
      },
      error: (error: any) => {
        console.error('Erro ao carregar categorias:', error);
        this.snackBar.open('Erro ao carregar categorias', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  loadProducts(): void {
    this.productService.getAll().subscribe({
      next: (products: any) => {
        this.products = products.map((product: any) => ({
          ...product,
          categoryName: this.getCategoryName(product.categoryId)
        }));
        this.updateDataSource();
        this.isLoading = false;
      },
      error: (error: any) => {
        console.error('Erro ao carregar produtos:', error);
        this.snackBar.open('Erro ao carregar produtos', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  updateDataSource(): void {
    if (this.showAll) {
      this.dataSource.data = this.products;
      if (this.paginator) {
        this.paginator.pageSize = this.products.length;
      }
    } else {
      this.dataSource.data = this.products;
    }
  }

  onPageChange(event: PageEvent): void {
    // Tratamento de mudança de página
    if (event.previousPageIndex !== undefined) {
      const previousPageSize = event.pageSize;
      if (event.pageSize !== previousPageSize) {
        if (this.paginator) {
          this.paginator.firstPage();
        }
      }
    }
  }

  toggleShowAll(): void {
    this.showAll = !this.showAll;
    if (this.showAll && this.paginator) {
      this.paginator.pageSize = this.products.length;
    } else if (!this.showAll && this.paginator) {
      this.paginator.pageSize = 10;
    }
    this.updateDataSource();
  }

  getCategoryName(categoryId: number): string {
    const category = this.categories.find(c => c.id === categoryId);
    return category ? category.name : 'Sem categoria';
  }

  createProduct(): void {
    const dialogRef = this.dialog.open(ProductCreateEditDialogComponent, {
      width: '500px',
      data: { mode: 'create', categories: this.categories }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.productService.invalidateCache();
        this.snackBar.open('Produto criado com sucesso!', 'Fechar', { duration: 3000 });
        this.loadProducts();
      }
    });
  }

  editProduct(product: Product): void {
    const dialogRef = this.dialog.open(ProductCreateEditDialogComponent, {
      width: '500px',
      data: { mode: 'edit', product, categories: this.categories }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.productService.invalidateCache();
        this.snackBar.open('Produto atualizado com sucesso!', 'Fechar', { duration: 3000 });
        this.loadProducts();
      }
    });
  }

  deleteProduct(product: Product): void {
    const dialogRef = this.dialog.open(ProductDeleteDialogComponent, {
      width: '400px',
      data: { product }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.productService.invalidateCache();
        this.snackBar.open('Produto deletado com sucesso!', 'Fechar', { duration: 3000 });
        this.loadProducts();
      }
    });
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(price);
  }
}
