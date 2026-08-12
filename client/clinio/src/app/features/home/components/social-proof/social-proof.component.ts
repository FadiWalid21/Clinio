import { Component, inject } from '@angular/core';
import { LanguageService } from '@core/services/language.service';

@Component({
  selector: 'app-social-proof',
  standalone: true,
  templateUrl: './social-proof.component.html',
  styleUrl: './social-proof.component.scss',
})
export class SocialProofComponent {

  protected ls = inject(LanguageService);

  testimonials = [
    {
      text: 'Found a cardiologist in Mansoura and booked the same day. No calls, no hold music.',
      author: 'Sara M.',
      location: 'Mansoura',
      rating: 5,
    },
    {
      text: 'My kids\' pediatrician was fully booked everywhere. Clinio showed me three alternatives with open slots tomorrow.',
      author: 'Ahmed K.',
      location: 'Dakahlia',
      rating: 5,
    },
    {
      text: 'The reminder SMS saved me from missing my appointment. Simple and works perfectly.',
      author: 'Nour H.',
      location: 'Cairo',
      rating: 5,
    },
  ];
}
