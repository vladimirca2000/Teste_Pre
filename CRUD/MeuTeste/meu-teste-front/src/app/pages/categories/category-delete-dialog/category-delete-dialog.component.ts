import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CategoryService } from '../../../services/category.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Category } from '../../../models/category.model';

@Component({
  selector: 'app-category-delete-dialog',
  templateUrl: './category-delete-dialog.component.html',
  styleUrls: ['./category-delete-dialog.component.css'],
  standalone: false
})
export class CategoryDeleteDialogComponent {
  isLoading = false;

  constructor(
    private categoryService: CategoryService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<CategoryDeleteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { category: Category }
  ) {}

  confirm(): void {
    if (!this.data.category?.id) {
      return;
    }

    this.isLoading = true;

    this.categoryService.delete(this.data.category.id).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Erro ao deletar categoria:', error);
        this.snackBar.open('Erro ao deletar categoria', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
