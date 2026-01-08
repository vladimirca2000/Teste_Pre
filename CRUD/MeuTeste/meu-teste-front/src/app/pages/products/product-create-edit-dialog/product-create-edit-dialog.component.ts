import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProductService } from '../../../services/product.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Product } from '../../../models/product.model';
import { Category } from '../../../models/category.model';

@Component({
  selector: 'app-product-create-edit-dialog',
  templateUrl: './product-create-edit-dialog.component.html',
  styleUrls: ['./product-create-edit-dialog.component.css'],
  standalone: false
})
export class ProductCreateEditDialogComponent {
  form: FormGroup;
  isLoading = false;
  isEditMode: boolean;
  categories: Category[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private productService: ProductService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<ProductCreateEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { mode: 'create' | 'edit', product?: Product, categories: Category[] }
  ) {
    this.isEditMode = data.mode === 'edit';
    this.categories = data.categories;

    this.form = this.formBuilder.group({
      name: [data.product?.name || '', [Validators.required, Validators.minLength(3)]],
      categoryId: [data.product?.categoryId || '', [Validators.required]],
      price: [data.product?.price || '', [Validators.required, Validators.min(0.01)]]
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.snackBar.open('Preencha todos os campos corretamente', 'Fechar', { duration: 3000 });
      return;
    }

    this.isLoading = true;

    const formValue = this.form.value;

    if (this.isEditMode && this.data.product?.id) {
      this.productService.update(this.data.product.id, formValue).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error: any) => {
          console.error('Erro ao atualizar produto:', error);
          this.snackBar.open('Erro ao atualizar produto', 'Fechar', { duration: 3000 });
          this.isLoading = false;
        }
      });
    } else {
      this.productService.create(formValue).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error: any) => {
          console.error('Erro ao criar produto:', error);
          this.snackBar.open('Erro ao criar produto', 'Fechar', { duration: 3000 });
          this.isLoading = false;
        }
      });
    }
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  get name() { return this.form.get('name'); }
  get categoryId() { return this.form.get('categoryId'); }
  get price() { return this.form.get('price'); }
}
