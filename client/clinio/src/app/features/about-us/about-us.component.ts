// about-us.component.ts
import { Component, inject } from '@angular/core';
import { LanguageService } from '@core/services/language.service';

interface ValueItem {
  key: string;
  iconPath: string;
  title: string;
  description: string;
}

interface TeamMember {
  id: string;
  name: string;
  role: string;
  profileImageUrl?: string;
  linkedin?: string;
  twitter?: string;
}

@Component({
  selector: 'app-about-us',
  standalone: true,
  templateUrl: './about-us.component.html',
  styleUrl: './about-us.component.scss',
})
export class AboutUsComponent {
  protected ls = inject(LanguageService);

  // Static demo data for values (you can replace with translations later)
  readonly values: ValueItem[] = [
    {
      key: 'trust',
      iconPath: 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z',
      title: 'Trust & Integrity',
      description: 'We build transparent relationships with patients and doctors.',
    },
    {
      key: 'care',
      iconPath: 'M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z',
      title: 'Patient‑first Care',
      description: 'Every feature we build starts with patient well‑being.',
    },
    {
      key: 'innovation',
      iconPath: 'M13 10V3L4 14h7v7l9-11h-7z',
      title: 'Innovation',
      description: 'We use technology to simplify healthcare.',
    },
    {
      key: 'accessibility',
      iconPath: 'M12 22c5.523 0 10-4.477 10-10S17.523 2 12 2 2 6.477 2 12s4.477 10 10 10zm0-2a8 8 0 110-16 8 8 0 010 16zM8 12l2 2 4-4',
      title: 'Accessibility',
      description: 'We break barriers so everyone can get care.',
    },
  ];

  readonly teamMembers: TeamMember[] = [
    { id: '1', name: 'Dr. Ahmed Ali', role: 'CEO & Founder', linkedin: '#' },
    { id: '2', name: 'Dr. Mona Ibrahim', role: 'Medical Director', linkedin: '#' },
    { id: '3', name: 'Eng. Youssef Hassan', role: 'CTO', linkedin: '#' },
    { id: '4', name: 'Sarah Mohsen', role: 'Head of Patient Experience', linkedin: '#' },
  ];

  initials(fullName: string): string {
    return fullName
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  }
}