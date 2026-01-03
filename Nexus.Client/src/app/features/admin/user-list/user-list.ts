import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, User } from '../../../core/services/user';
import { TranslateModule } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { LucideAngularModule, Ban, CheckCircle, Search, MoreVertical } from 'lucide-angular';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, TranslateModule, LucideAngularModule],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css',
})
export class UserList implements OnInit {
  userService = inject(UserService);
  toastr = inject(ToastrService);

  users = signal<User[]>([]);
  isLoading = signal(false);

  // Icons
  readonly icons = { Ban, CheckCircle, Search, MoreVertical };

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.isLoading.set(true);
    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.toastr.error('Kullanıcı listesi yüklenemedi', 'Hata');
        this.isLoading.set(false);
      },
    });
  }

  toggleStatus(user: User) {
    if (
      !confirm(
        `${user.username} adlı kullanıcıyı ${
          user.isActive ? 'yasaklamak' : 'aktif etmek'
        } istediğinize emin misiniz?`
      )
    )
      return;

    this.userService.toggleUserStatus(user.id).subscribe({
      next: () => {
        const action = user.isActive ? 'yasaklandı' : 'aktif edildi';
        this.toastr.success(`Kullanıcı başarıyla ${action}`, 'İşlem Başarılı');

        // Optimistic update
        this.users.update((currentUsers) =>
          currentUsers.map((u) => (u.id === user.id ? { ...u, isActive: !u.isActive } : u))
        );
      },
      error: (err) => {
        this.toastr.error('İşlem başarısız oldu', 'Hata');
      },
    });
  }
}
