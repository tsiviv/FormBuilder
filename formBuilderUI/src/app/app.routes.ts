import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'forms/new',
    loadComponent: () =>
      import('./features/forms/create-form/create-form.component').then((m) => m.CreateFormComponent)
  },
  { path: '', redirectTo: 'forms/new', pathMatch: 'full' },
  { path: '**', redirectTo: 'forms/new' }
];
