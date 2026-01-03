import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { LucideAngularModule, Mail, ArrowRight, Lock, KeyRound } from 'lucide-angular';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, LucideAngularModule],
  template: `
    <div class="min-h-[80vh] flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div
        class="max-w-md w-full space-y-8 bg-white dark:bg-dark-surface p-8 rounded-2xl shadow-2xl border border-slate-100 dark:border-slate-800 transition-colors duration-300"
      >
        <!-- Step 1: Request Code -->
        <div *ngIf="step() === 1" class="animate-fade-in">
          <div class="text-center">
            <h2 class="mt-6 text-3xl font-bold tracking-tight text-slate-900 dark:text-white">
              Forgot Password?
            </h2>
            <p class="mt-2 text-sm text-slate-600 dark:text-slate-400">
              Enter your email and we'll send you a verification code.
            </p>
          </div>

          <form class="mt-8 space-y-6" (ngSubmit)="onRequestCode()">
            <div class="relative group">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <lucide-icon
                  [img]="icons.Mail"
                  class="h-5 w-5 text-slate-400 group-focus-within:text-primary-500 transition-colors"
                ></lucide-icon>
              </div>
              <input
                type="email"
                [(ngModel)]="email"
                name="email"
                required
                class="block w-full pl-10 pr-3 py-3 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
                placeholder="Email address"
              />
            </div>

            <button
              type="submit"
              [disabled]="isLoading()"
              class="w-full flex justify-center py-3 px-4 border border-transparent text-sm font-semibold rounded-xl text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all shadow-lg hover:shadow-primary-500/30"
            >
              <span *ngIf="!isLoading()">Send Code</span>
              <span *ngIf="isLoading()">Sending...</span>
            </button>
          </form>
        </div>

        <!-- Step 2: Verify Code -->
        <div *ngIf="step() === 2" class="animate-fade-in">
          <div class="text-center">
            <h2 class="mt-6 text-3xl font-bold tracking-tight text-slate-900 dark:text-white">
              Verify It's You
            </h2>
            <p class="mt-2 text-sm text-slate-600 dark:text-slate-400">
              Enter the 6-digit code sent to {{ email }}
            </p>
          </div>

          <form class="mt-8 space-y-6" (ngSubmit)="onVerifyCode()">
            <div class="relative group">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <lucide-icon
                  [img]="icons.KeyRound"
                  class="h-5 w-5 text-slate-400 group-focus-within:text-primary-500 transition-colors"
                ></lucide-icon>
              </div>
              <input
                type="text"
                [(ngModel)]="code"
                name="code"
                required
                maxlength="6"
                class="block w-full pl-10 pr-3 py-3 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all tracking-widest text-center text-xl font-mono"
                placeholder="000000"
              />
            </div>

            <button
              type="submit"
              [disabled]="isLoading()"
              class="w-full flex justify-center py-3 px-4 border border-transparent text-sm font-semibold rounded-xl text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all shadow-lg hover:shadow-primary-500/30"
            >
              <span *ngIf="!isLoading()">Verify Code</span>
              <span *ngIf="isLoading()">Verifying...</span>
            </button>
          </form>
        </div>

        <!-- Step 3: Reset Password -->
        <div *ngIf="step() === 3" class="animate-fade-in">
          <div class="text-center">
            <h2 class="mt-6 text-3xl font-bold tracking-tight text-slate-900 dark:text-white">
              Reset Password
            </h2>
            <p class="mt-2 text-sm text-slate-600 dark:text-slate-400">
              Create a new secure password.
            </p>
          </div>

          <form class="mt-8 space-y-6" (ngSubmit)="onResetPassword()">
            <div class="relative group">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <lucide-icon
                  [img]="icons.Lock"
                  class="h-5 w-5 text-slate-400 group-focus-within:text-primary-500 transition-colors"
                ></lucide-icon>
              </div>
              <input
                type="password"
                [(ngModel)]="newPassword"
                name="newPassword"
                required
                class="block w-full pl-10 pr-3 py-3 border border-slate-300 dark:border-slate-700 rounded-xl bg-white dark:bg-slate-900 text-slate-900 dark:text-white placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
                placeholder="New Password"
              />
            </div>

            <button
              type="submit"
              [disabled]="isLoading()"
              class="w-full flex justify-center py-3 px-4 border border-transparent text-sm font-semibold rounded-xl text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all shadow-lg hover:shadow-primary-500/30"
            >
              <span *ngIf="!isLoading()">Set New Password</span>
              <span *ngIf="isLoading()">Updating...</span>
            </button>
          </form>
        </div>

        <!-- Back to Login -->
        <div class="text-center mt-4">
          <a
            routerLink="/login"
            class="text-sm font-medium text-primary-600 hover:text-primary-500 flex items-center justify-center gap-2"
          >
            Back to Login
          </a>
        </div>
      </div>
    </div>
  `,
})
export class ForgotPasswordComponent {
  authService = inject(AuthService);
  private toastr = inject(ToastrService);
  private router = inject(Router);

  email = '';
  code = '';
  newPassword = '';
  step = signal(1); // 1: Email, 2: Code, 3: New Password
  isLoading = signal(false);

  readonly icons = { Mail, ArrowRight, Lock, KeyRound };

  onRequestCode() {
    if (!this.email) {
      this.toastr.warning('Please enter your email address', 'Validation');
      return;
    }

    this.isLoading.set(true);
    this.authService.forgotPassword(this.email).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.step.set(2);
        this.toastr.success('Verification code sent to your email', 'Code Sent');
        console.log('MOCK EMAIL SENT: Check backend console for code');
      },
      error: (err) => {
        this.isLoading.set(false);
        this.toastr.error('Failed to send code. Please try again.', 'Error');
        console.error(err);
      },
    });
  }

  onVerifyCode() {
    if (!this.code || this.code.length !== 6) {
      this.toastr.warning('Please enter a valid 6-digit code', 'Validation');
      return;
    }

    this.isLoading.set(true);
    this.authService.verifyCode(this.email, this.code).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.step.set(3);
        this.toastr.success('Code verified', 'Success');
      },
      error: (err) => {
        this.isLoading.set(false);
        this.toastr.error('Invalid or expired code', 'Error');
      },
    });
  }

  onResetPassword() {
    if (!this.newPassword) {
      this.toastr.warning('Please enter a new password', 'Validation');
      return;
    }

    this.isLoading.set(true);
    this.authService.resetPassword(this.email, this.code, this.newPassword).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.toastr.success('Password updated successfully! Please login.', 'Success');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.isLoading.set(false);
        this.toastr.error('Failed to reset password', 'Error');
      },
    });
  }
}
