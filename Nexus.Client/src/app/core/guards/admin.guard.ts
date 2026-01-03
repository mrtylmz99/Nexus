import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  const user = authService.currentUser();

  if (user && user.role === 'Admin') {
    return true;
  }

  toastr.error('Bu alana erişim yetkiniz yok!', 'Erişim Reddedildi');
  router.navigate(['/']);
  return false;
};
