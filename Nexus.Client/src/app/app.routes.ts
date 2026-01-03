import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';
import { UserList } from './features/admin/user-list/user-list';
import { NotFoundComponent } from './core/errors/not-found/not-found';

export const routes: Routes = [
  // Public Routes (Auth)
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // Admin Routes (Admin Layout)
  {
    path: 'admin',
    component: MainLayoutComponent, // Reusing MainLayout structure for now
    canActivate: [adminGuard],
    children: [
      { path: 'users', component: UserList },
      { path: '', redirectTo: 'users', pathMatch: 'full' },
    ],
  },

  // User Routes (Main Layout)
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      // Redirect root to admin/users for now (since we logged in as admin)
      // Later this will point to Projects/Dashboard
      { path: '', redirectTo: 'admin/users', pathMatch: 'full' },
    ],
  },
  { path: '**', component: NotFoundComponent },
];
