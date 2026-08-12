import { Component } from '@angular/core';
import { HeroComponent } from './components/hero/hero.component';
import { HowItWorksComponent } from './components/how-it-works/how-it-works.component';
import { SpecialtiesComponent } from './components/specialties/specialties.component';
import { FeaturedDoctorsComponent } from './components/featured-doctors/featured-doctors.component';
import { SocialProofComponent } from './components/social-proof/social-proof.component';

@Component({
  selector: 'app-home',
  imports: [
    HeroComponent,
    HowItWorksComponent,
    SpecialtiesComponent,
    FeaturedDoctorsComponent,
    SocialProofComponent
],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {}
