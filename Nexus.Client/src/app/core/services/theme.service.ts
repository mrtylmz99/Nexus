import { Injectable, signal, effect } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  // Signal to track theme state (true = dark, false = light)
  darkMode = signal<boolean>(true); // Default to Dark Mode as requested "Premium"

  constructor() {
    // Load saved preference
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
      this.darkMode.set(savedTheme === 'dark');
    } else {
      // Check system preference
      const systemDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      this.darkMode.set(systemDark);
    }

    // Effect to apply class to html element
    effect(() => {
      const isDark = this.darkMode();
      if (isDark) {
        document.documentElement.classList.add('dark');
        localStorage.setItem('theme', 'dark');
      } else {
        document.documentElement.classList.remove('dark');
        localStorage.setItem('theme', 'light');
      }
    });
  }

  toggleTheme() {
    this.darkMode.update((prev) => !prev);
  }
}
