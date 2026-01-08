import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { UserService, User } from '../../shared/services/user.service';
import { AuthService } from '../../shared/services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { of } from 'rxjs';

@Component({
  selector: 'app-approve-user',
  templateUrl: './approve-user.component.html',
  styleUrls: ['./approve-user.component.css'],
  standalone: false
})
export class ApproveUserComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  users: User[] = [];
  dataSource = new MatTableDataSource<User>([]);
  displayedColumns: string[] = ['id', 'username', 'email', 'role', 'createdAt', 'actions'];
  isLoading = false;

  // Paginação
  pageSizeOptions = [10, 25, 50];
  showAll = false;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    // Verificar se é admin
    if (!this.authService.isAdmin()) {
      this.snackBar.open('Acesso negado. Apenas administradores podem acessar.', 'Fechar', { duration: 3000 });
      return;
    }

    this.loadInactiveUsers();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  loadInactiveUsers(): void {
    this.isLoading = true;
    this.userService.getInactiveUsers().subscribe({
      next: (users: User[]) => {
        this.users = users;
        this.updateDataSource();
        this.isLoading = false;
      },
      error: (error: any) => {
        console.error('Erro ao carregar usuários:', error);
        this.snackBar.open('Erro ao carregar usuários', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  updateDataSource(): void {
    if (this.showAll) {
      this.dataSource.data = this.users;
      if (this.paginator) {
        this.paginator.pageSize = this.users.length;
      }
    } else {
      this.dataSource.data = this.users;
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
      this.paginator.pageSize = this.users.length;
    } else if (!this.showAll && this.paginator) {
      this.paginator.pageSize = 10;
    }
    this.updateDataSource();
  }

  approveUser(userId: number): void {
    if (confirm('Tem certeza que deseja aprovar este usuário?')) {
      this.userService.approveUser(userId).subscribe({
        next: () => {
          this.snackBar.open('Usuário aprovado com sucesso!', 'Fechar', { duration: 3000 });
          this.loadInactiveUsers();
        },
        error: (error) => {
          console.error('Erro ao aprovar usuário:', error);
          const message = error?.error?.message || 'Erro ao aprovar usuário';
          this.snackBar.open(message, 'Fechar', { duration: 3000 });
        }
      });
    }
  }

  formatDate(date: string | Date): string {
    if (!date) return '';
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return dateObj.toLocaleDateString('pt-BR', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
