import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService, LoginDto } from '../../../core/services/auth.service';
import {
  LucideAngularModule,
  LogIn,
  Mail,
  Lock,
  Eye,
  EyeOff,
  Globe,
  Facebook,
} from 'lucide-angular';
// Note: Apple icon might not be in the set if 'Apple' is not exported. Using 'Github' as placeholder if valid.
// Checking lucide-angular exports: usually has Facebook, Github. Apple maybe not? Let's assume standard set.
// If Apple/Amazon missing, I'll use generic SVGs later. Safe bet: Facebook, Github, Globe.
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, LucideAngularModule], // Removed TranslateModule
  template: `
    <div class="min-h-[80vh] flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8 relative">
      <!-- Language Switcher (Temporarily Removed) -->
      <!-- <div class="absolute top-4 right-4 flex space-x-2 z-10"> ... </div> -->

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
              <a
                routerLink="/forgot-password"
                class="font-medium text-primary-600 hover:text-primary-500"
              >
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

          <!-- Social Login -->
          <div class="mt-6">
            <div class="relative">
              <div class="absolute inset-0 flex items-center">
                <div class="w-full border-t border-slate-300 dark:border-slate-700"></div>
              </div>
              <div class="relative flex justify-center text-sm">
                <span class="px-2 bg-white dark:bg-dark-surface text-slate-500">Or login with</span>
              </div>
            </div>

            <div class="mt-6 grid grid-cols-3 gap-3">
              <button
                type="button"
                (click)="socialLogin('Google')"
                class="flex justify-center items-center py-2.5 px-4 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors shadow-sm"
              >
                <!-- Google SVG -->
                <svg class="h-5 w-5" aria-hidden="true" viewBox="0 0 24 24">
                  <path
                    d="M12.0003 20.45c4.6667 0 8.45-3.7833 8.45-8.45 0-4.6667-3.7833-8.45-8.45-8.45-4.6667 0-8.45 3.7833-8.45 8.45 0 4.6667 3.7833 8.45 8.45 8.45Z"
                    fill="#fff"
                    fill-opacity="0"
                    stroke="none"
                  />
                  <path
                    d="M20.283 10.356h-8.327v3.451h4.792c-.446 2.193-2.313 3.453-4.792 3.453a5.27 5.27 0 0 1-5.279-5.28 5.27 5.27 0 0 1 5.279-5.279c1.259 0 2.397.447 3.29 1.178l2.6-2.599c-1.584-1.381-3.615-2.233-5.89-2.233a8.908 8.908 0 0 0-8.934 8.934 8.907 8.907 0 0 0 8.934 8.934c4.467 0 8.529-3.249 8.529-8.934 0-.528-.081-1.097-.202-1.625z"
                    fill="#EA4335"
                  />
                </svg>
              </button>

              <button
                type="button"
                (click)="socialLogin('Apple')"
                class="flex justify-center items-center py-2.5 px-4 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors shadow-sm"
              >
                <!-- Apple SVG -->
                <svg class="h-5 w-5" aria-hidden="true" viewBox="0 0 24 24" fill="currentColor">
                  <path
                    d="M12.152 6.896c-.948 0-2.415-1.078-3.96-1.04-2.04.027-3.91 1.183-4.961 3.014-2.117 3.675-.546 9.103 1.519 12.09 1.013 1.454 2.208 3.09 3.792 3.039 1.52-.065 2.09-.987 3.935-.987 1.831 0 2.35.987 3.96.948 1.637-.026 2.676-1.48 3.676-2.948 1.156-1.688 1.636-3.325 1.662-3.415-.039-.013-3.182-1.221-3.22-4.857-.026-3.04 2.48-4.494 2.597-4.559-1.429-2.09-3.623-2.324-4.415-2.376-2.003-.13-3.674 1.04-4.623 1.04zM15.532 3.23c.857-1.026 1.455-2.48 1.286-3.818-1.273.052-2.831.844-3.766 1.948-.909 1.052-1.688 2.714-1.494 3.974 1.42.117 2.87-.766 3.974-2.104z"
                  />
                </svg>
              </button>

              <button
                type="button"
                (click)="socialLogin('Facebook')"
                class="flex justify-center items-center py-2.5 px-4 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-800 hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors shadow-sm"
              >
                <lucide-icon [img]="icons.Facebook" class="h-5 w-5 text-[#1877F2]"></lucide-icon>
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  `,
})
export class LoginComponent {
  authService = inject(AuthService);
  private toastr = inject(ToastrService);

  formData: LoginDto = {
    email: '',
    password: '',
  };

  isLoading = signal(false);
  showPassword = signal(false);
  readonly icons = { LogIn, Mail, Lock, Eye, EyeOff, Globe, Facebook };

  togglePasswordVisibility() {
    this.showPassword.update((value) => !value);
  }

  socialLogin(provider: string) {
    this.isLoading.set(true);
    this.toastr.info(`${provider} ile bağlanılıyor...`, 'Bağlantı');

    // Simulate network delay for realistic feel
    setTimeout(() => {
      // Mock successful login by using the default user credentials
      // In a real app, this would redirect to OAuth provider
      const mockCredentials = {
        email: 'john@nexus.com',
        password: 'Password123!',
      };

      this.authService.login(mockCredentials).subscribe({
        next: () => {
          this.isLoading.set(false);
          this.toastr.success(`${provider} ile giriş başarılı!`, 'Hoşgeldiniz');
          // Green light indication as requested
        },
        error: (err) => {
          this.isLoading.set(false);
          this.toastr.error(`${provider} ile giriş yapılamadı.`, 'Hata');
        },
      });
    }, 1500);
  }

  onSubmit() {
    if (!this.formData.email || !this.formData.password) return;

    this.isLoading.set(true);
    this.authService.login(this.formData).subscribe({
      next: () => {
        // Navigation handled in AuthService
        this.isLoading.set(false);
        this.toastr.success('Giriş Başarılı', 'Hoşgeldiniz');
      },
      error: (err) => {
        console.error('Login failed', err);
        this.isLoading.set(false);

        if (err.status === 0) {
          this.toastr.error(
            'Sunucuya ulaşılamıyor. Lütfen bağlantınızı kontrol edin.',
            'Bağlantı Hatası'
          );
        } else if (err.status === 401) {
          this.toastr.error('E-posta adresi veya şifre hatalı.', 'Giriş Başarısız');
        } else {
          const errorMessage = err.error?.message || 'Bilinmeyen bir hata oluştu';
          this.toastr.error(errorMessage, 'Hata');
        }
      },
    });
  }
}
