import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LucideAngularModule, Sun, Moon, Zap, Layers, FolderGit2 } from 'lucide-angular';
import { ThemeService } from './core/services/theme.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LucideAngularModule, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  // Inject ThemeService
  themeService = inject(ThemeService);

  // Expose Icons to Template
  readonly icons = { Sun, Moon, Zap, Layers, FolderGit2 };
}
