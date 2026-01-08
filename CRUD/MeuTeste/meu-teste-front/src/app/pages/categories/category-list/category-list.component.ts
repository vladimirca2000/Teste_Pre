import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CategoryService } from '../../../services/category.service';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Category } from '../../../models/category.model';
import { of } from 'rxjs';
import { CategoryCreateEditDialogComponent } from '../category-create-edit-dialog/category-create-edit-dialog.component';
import { CategoryDeleteDialogComponent } from '../category-delete-dialog/category-delete-dialog.component';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css'],
  standalone: false
})
export class CategoryListComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  categories: Category[] = [];
  dataSource = new MatTableDataSource<Category>([]);
  displayedColumns: string[] = ['id', 'name', 'actions'];
  isLoading = false;

  // Paginação
  pageSizeOptions = [10, 25, 50];
  showAll = false;

  constructor(
    private categoryService: CategoryService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  loadCategories(): void {
    this.isLoading = true;
    this.categoryService.getAll().subscribe({
      next: (categories: any) => {
        this.categories = categories.sort((a: any, b: any) => a.name.localeCompare(b.name));
        this.updateDataSource();
        this.isLoading = false;
      },
      error: (error: any) => {
        console.error('Erro ao carregar categorias:', error);
        this.snackBar.open('Erro ao carregar categorias', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  updateDataSource(): void {
    if (this.showAll) {
      this.dataSource.data = this.categories;
      if (this.paginator) {
        this.paginator.pageSize = this.categories.length;
      }
    } else {
      this.dataSource.data = this.categories;
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
      this.paginator.pageSize = this.categories.length;
    } else if (!this.showAll && this.paginator) {
      this.paginator.pageSize = 10;
    }
    this.updateDataSource();
  }

  createCategory(): void {
    const dialogRef = this.dialog.open(CategoryCreateEditDialogComponent, {
      width: '400px',
      data: { mode: 'create' }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.categoryService.invalidateCache();
        this.snackBar.open('Categoria criada com sucesso!', 'Fechar', { duration: 3000 });
        this.loadCategories();
      }
    });
  }

  editCategory(category: Category): void {
    const dialogRef = this.dialog.open(CategoryCreateEditDialogComponent, {
      width: '400px',
      data: { mode: 'edit', category }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.categoryService.invalidateCache();
        this.snackBar.open('Categoria atualizada com sucesso!', 'Fechar', { duration: 3000 });
        this.loadCategories();
      }
    });
  }

  deleteCategory(category: Category): void {
    const dialogRef = this.dialog.open(CategoryDeleteDialogComponent, {
      width: '400px',
      data: { category }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.categoryService.invalidateCache();
        this.snackBar.open('Categoria deletada com sucesso!', 'Fechar', { duration: 3000 });
        this.loadCategories();
      }
    });
  }
}
