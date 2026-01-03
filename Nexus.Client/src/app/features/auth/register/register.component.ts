import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService, RegisterDto } from '../../../core/services/auth.service';
import { LucideAngularModule, UserPlus, Mail, Lock, User } from 'lucide-angular';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, LucideAngularModule],
  template: `
    <div class="min-h-[80vh] flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div
        class="max-w-md w-full space-y-8 bg-white dark:bg-dark-surface p-8 rounded-2xl shadow-2xl border border-slate-100 dark:border-slate-800 transition-colors duration-300"
      >
        <div class="text-center">
          <h2 class="mt-6 text-3xl font-bold tracking-tight text-slate-900 dark:text-white">
            Create an account
          </h2>
          <p class="mt-2 text-sm text-slate-600 dark:text-slate-400">
            Already have an account?
            <a
              routerLink="/login"
              class="font-medium text-primary-600 hover:text-primary-500 transition-colors"
            >
              Sign in here
            </a>
          </p>
        </div>

        <form class="mt-8 space-y-6" (ngSubmit)="onSubmit()">
          <div class="space-y-4">
            <!-- Full Name -->
            <div class="relative group">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <lucide-icon
                  [img]="icons.User"
                  class="h-5 w-5 text-slate-400 group-focus-within:text-primary-500 transition-colors"
                ></lucide-icon>
              </div>
              <input
                id="fullName"
                name="fullName"
                type="text"
                [(ngModel)]="formData.fullName"
                required
                class="block w-full pl-10 pr-3 py-3 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500 sm:text-sm transition-all"
                placeholder="Full Name"
              />
            </div>

            <!-- Username -->
            <div class="relative group">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <lucide-icon
                  [img]="icons.User"
                  class="h-5 w-5 text-slate-400 group-focus-within:text-primary-500 transition-colors"
                ></lucide-icon>
              </div>
              <input
                id="username"
                name="username"
                type="text"
                [(ngModel)]="formData.username"
                required
                class="block w-full pl-10 pr-3 py-3 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500 sm:text-sm transition-all"
                placeholder="Username"
              />
            </div>

            <!-- Email -->
            <div class="relative group">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <lucide-icon
                  [img]="icons.Mail"
                  class="h-5 w-5 text-slate-400 group-focus-within:text-primary-500 transition-colors"
                ></lucide-icon>
              </div>
              <input
                id="email"
                name="email"
                type="email"
                [(ngModel)]="formData.email"
                required
                class="block w-full pl-10 pr-3 py-3 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500 sm:text-sm transition-all"
                placeholder="Email address"
              />
            </div>

            <!-- Password -->
            <div class="relative group">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <lucide-icon
                  [img]="icons.Lock"
                  class="h-5 w-5 text-slate-400 group-focus-within:text-primary-500 transition-colors"
                ></lucide-icon>
              </div>
              <input
                id="password"
                name="password"
                type="password"
                [(ngModel)]="formData.password"
                required
                class="block w-full pl-10 pr-3 py-3 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500 sm:text-sm transition-all"
                placeholder="Password"
              />
            </div>
          </div>

          <button
            type="submit"
            [disabled]="isLoading()"
            class="group relative w-full flex justify-center py-3 px-4 border border-transparent text-sm font-semibold rounded-xl text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500 disabled:opacity-70 disabled:cursor-not-allowed transition-all shadow-lg hover:shadow-primary-500/30"
          >
            <span class="absolute left-0 inset-y-0 flex items-center pl-3">
              <lucide-icon
                [img]="icons.UserPlus"
                class="h-5 w-5 text-primary-500 group-hover:text-primary-400 transition-colors"
              ></lucide-icon>
            </span>
            <span *ngIf="!isLoading()">Sign up</span>
            <span *ngIf="isLoading()">Creating account...</span>
          </button>
        </form>
      </div>
    </div>
  `,
})
export class RegisterComponent {
  authService = inject(AuthService);

  formData: RegisterDto = {
    username: '',
    fullName: '',
    email: '',
    password: '',
  };

  isLoading = signal(false);
  readonly icons = { UserPlus, Mail, Lock, User };

  onSubmit() {
    // Basic validation
    if (!this.formData.email || !this.formData.password || !this.formData.username) return;

    this.isLoading.set(true);
    this.authService.register(this.formData).subscribe({
      next: () => {
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Registration failed', err);
        alert('Registration failed: ' + (err.error?.message || 'Unknown error'));
        this.isLoading.set(false);
      },
    });
  }
}
