import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LanguageService } from '@core/services/language.service';

@Component({
  selector: 'app-specialties',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './specialties.component.html',
  styleUrl: './specialties.component.scss',
})
export class SpecialtiesComponent {

  protected ls = inject(LanguageService);

  specialties = [
    { name: 'General Practice',    nameAr: 'طب عام',         icon: '🩺', count: 124 },
    { name: 'Cardiology',          nameAr: 'قلب وأوعية',     icon: '❤️', count: 48  },
    { name: 'Dermatology',         nameAr: 'جلدية',          icon: '✨', count: 63  },
    { name: 'Pediatrics',          nameAr: 'أطفال',          icon: '👶', count: 91  },
    { name: 'Orthopedics',         nameAr: 'عظام ومفاصل',    icon: '🦴', count: 55  },
    { name: 'Ophthalmology',       nameAr: 'عيون',           icon: '👁️', count: 44  },
    { name: 'Gynecology',          nameAr: 'نساء وتوليد',    icon: '🌸', count: 72  },
    { name: 'ENT',                 nameAr: 'أنف وأذن وحنجرة', icon: '👂', count: 38  },
    { name: 'Neurology',           nameAr: 'مخ وأعصاب',      icon: '🧠', count: 29  },
    { name: 'Urology',             nameAr: 'مسالك بولية',    icon: '🫘', count: 33  },
    { name: 'Psychiatry',          nameAr: 'نفسية',          icon: '🧘', count: 26  },
    { name: 'Dentistry',           nameAr: 'أسنان',          icon: '🦷', count: 88  },
  ];
}