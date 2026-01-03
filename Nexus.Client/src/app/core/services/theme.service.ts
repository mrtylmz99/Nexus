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
        // Update Favicon: Dark Screen -> White Icon (favicon-light)
        const favicon = document.getElementById('app-favicon') as HTMLLinkElement;
        if (favicon) favicon.href = 'favicon-light.svg';
      } else {
        document.documentElement.classList.remove('dark');
        localStorage.setItem('theme', 'light');
        // Update Favicon: Light Screen -> Black Icon (favicon-dark)
        const favicon = document.getElementById('app-favicon') as HTMLLinkElement;
        if (favicon) favicon.href = 'favicon-dark.svg';
      }
    });
  }

  toggleTheme() {
    this.darkMode.update((prev) => !prev);
  }
}
