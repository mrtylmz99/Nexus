import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService, LoginDto } from '../../../core/services/auth.service';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { LucideAngularModule, LogIn, Mail, Lock, Eye, EyeOff, Globe } from 'lucide-angular';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, LucideAngularModule, TranslateModule],
  template: `
    <div class="min-h-[80vh] flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8 relative">
      <!-- Language Switcher -->
      <div class="absolute top-4 right-4 flex space-x-2 z-10">
        <button
          (click)="switchLanguage('en')"
          class="px-3 py-1 text-sm bg-white dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors"
        >
          EN
        </button>
        <button
          (click)="switchLanguage('tr')"
          class="px-3 py-1 text-sm bg-white dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors"
        >
          TR
        </button>
      </div>
      <div
        class="max-w-md w-full space-y-8 bg-white dark:bg-dark-surface p-8 rounded-2xl shadow-2xl border border-slate-100 dark:border-slate-800 transition-colors duration-300"
      >
        <!-- Header -->
        <div class="text-center">
          <h2 class="mt-6 text-3xl font-bold tracking-tight text-slate-900 dark:text-white">
            Welcome back
          </h2>
          <p class="mt-2 text-sm text-slate-600 dark:text-slate-400">
            Or
            <a
              routerLink="/register"
              class="font-medium text-primary-600 hover:text-primary-500 transition-colors"
            >
              create a new account
            </a>
          </p>
        </div>

        <form class="mt-8 space-y-6" (ngSubmit)="onSubmit()">
          <div class="space-y-4">
            <!-- Email Input -->
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
                class="block w-full pl-10 pr-3 py-3 border border-slate-300 dark:border-slate-700 rounded-xl leading-5 bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500 sm:text-sm transition-all shadow-sm"
                placeholder="Email address"
              />
            </div>

            <!-- Password Input -->
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
                [type]="showPassword() ? 'text' : 'password'"
                [(ngModel)]="formData.password"
                required
                class="block w-full pl-10 pr-10 py-3 border border-slate-300 dark:border-slate-700 rounded-xl leading-5 bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-primary-500 sm:text-sm transition-all shadow-sm"
                placeholder="Password"
              />
              <button
                type="button"
                (click)="togglePasswordVisibility()"
                class="absolute inset-y-0 right-0 pr-3 flex items-center cursor-pointer text-slate-400 hover:text-slate-600 dark:hover:text-slate-300 transition-colors focus:outline-none"
              >
                <lucide-icon
                  [img]="showPassword() ? icons.EyeOff : icons.Eye"
                  class="h-5 w-5"
                ></lucide-icon>
              </button>
            </div>
          </div>

          <div class="flex items-center justify-between">
            <div class="flex items-center">
              <input
                id="remember-me"
                name="remember-me"
                type="checkbox"
                class="h-4 w-4 text-primary-600 focus:ring-primary-500 border-slate-300 rounded"
              />
              <label
                for="remember-me"
                class="ml-2 block text-sm text-slate-900 dark:text-slate-300"
              >
                Remember me
              </label>
            </div>

            <div class="text-sm">
              <a href="#" class="font-medium text-primary-600 hover:text-primary-500">
                Forgot your password?
              </a>
            </div>
          </div>

          <div>
            <button
              type="submit"
              [disabled]="isLoading()"
              class="group relative w-full flex justify-center py-3 px-4 border border-transparent text-sm font-semibold rounded-xl text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500 disabled:opacity-70 disabled:cursor-not-allowed transition-all shadow-lg hover:shadow-primary-500/30"
            >
              <span class="absolute left-0 inset-y-0 flex items-center pl-3">
                <lucide-icon
                  [img]="icons.LogIn"
                  class="h-5 w-5 text-primary-500 group-hover:text-primary-400 transition-colors"
                ></lucide-icon>
              </span>
              <span *ngIf="!isLoading()">Sign in</span>
              <span *ngIf="isLoading()">Signing in...</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `,
})
export class LoginComponent {
  authService = inject(AuthService);
  private translate = inject(TranslateService);

  formData: LoginDto = {
    email: '',
    password: '',
  };

  isLoading = signal(false);
  showPassword = signal(false);
  readonly icons = { LogIn, Mail, Lock, Eye, EyeOff };

  togglePasswordVisibility() {
    this.showPassword.update((value) => !value);
  }

  switchLanguage(lang: string) {
    this.translate.use(lang);
  }

  onSubmit() {
    if (!this.formData.email || !this.formData.password) return;

    this.isLoading.set(true);
    this.authService.login(this.formData).subscribe({
      next: () => {
        // Navigation handled in AuthService
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Login failed', err);
        const errorMessage = err.error?.message || err.statusText || 'Unknown error';
        alert(`Login failed: ${errorMessage} (${err.status})`);
        this.isLoading.set(false);
      },
    });
  }
}
