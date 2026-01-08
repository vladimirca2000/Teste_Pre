import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CategoryService } from '../../../services/category.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Category } from '../../../models/category.model';

@Component({
  selector: 'app-category-create-edit-dialog',
  templateUrl: './category-create-edit-dialog.component.html',
  styleUrls: ['./category-create-edit-dialog.component.css'],
  standalone: false
})
export class CategoryCreateEditDialogComponent {
  form: FormGroup;
  isLoading = false;
  isEditMode: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private categoryService: CategoryService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<CategoryCreateEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { mode: 'create' | 'edit', category?: Category }
  ) {
    this.isEditMode = data.mode === 'edit';

    this.form = this.formBuilder.group({
      name: [data.category?.name || '', [Validators.required, Validators.minLength(3)]]
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.snackBar.open('Nome da categoria é obrigatório', 'Fechar', { duration: 3000 });
      return;
    }

    this.isLoading = true;

    const formValue = this.form.value;

    if (this.isEditMode && this.data.category?.id) {
      this.categoryService.update(this.data.category.id, formValue).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error: any) => {
          console.error('Erro ao atualizar categoria:', error);
          this.snackBar.open('Erro ao atualizar categoria', 'Fechar', { duration: 3000 });
          this.isLoading = false;
        }
      });
    } else {
      this.categoryService.create(formValue).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error: any) => {
          console.error('Erro ao criar categoria:', error);
          this.snackBar.open('Erro ao criar categoria', 'Fechar', { duration: 3000 });
          this.isLoading = false;
        }
      });
    }
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  get name() { return this.form.get('name'); }
}
