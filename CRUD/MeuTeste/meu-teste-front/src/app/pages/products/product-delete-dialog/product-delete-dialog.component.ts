import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProductService } from '../../../services/product.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Product } from '../../../models/product.model';

@Component({
  selector: 'app-product-delete-dialog',
  templateUrl: './product-delete-dialog.component.html',
  styleUrls: ['./product-delete-dialog.component.css'],
  standalone: false
})
export class ProductDeleteDialogComponent {
  isLoading = false;

  constructor(
    private productService: ProductService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<ProductDeleteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { product: Product }
  ) {}

  confirm(): void {
    if (!this.data.product?.id) {
      return;
    }

    this.isLoading = true;

    this.productService.delete(this.data.product.id).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Erro ao deletar produto:', error);
        this.snackBar.open('Erro ao deletar produto', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
