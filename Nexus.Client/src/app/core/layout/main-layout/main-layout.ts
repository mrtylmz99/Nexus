import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { TranslateService } from '@ngx-translate/core';
import { LucideAngularModule, Moon, Sun, LogOut } from 'lucide-angular';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, LucideAngularModule],
  templateUrl: './main-layout.component.html', // Fixed template path
  styleUrl: './main-layout.css',
})
export class MainLayoutComponent {
  authService = inject(AuthService);
  router = inject(Router);
  translate = inject(TranslateService);

  user = this.authService.currentUser;
  isDarkMode = signal(false);
  currentLang = signal('tr');

  readonly icons = { Moon, Sun, LogOut };

  constructor() {
    // Init theme
    this.isDarkMode.set(document.documentElement.classList.contains('dark'));
    // Init lang
    this.currentLang.set(this.translate.currentLang || 'tr');
  }

  toggleTheme() {
    this.isDarkMode.update((v) => !v);
    if (this.isDarkMode()) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }

  switchLanguage(lang: string) {
    this.translate.use(lang);
    this.currentLang.set(lang);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
