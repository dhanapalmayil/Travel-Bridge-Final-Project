import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withFetch } from '@angular/common/http'; // Import HttpClient and fetch option
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

bootstrapApplication(AppComponent, {
  ...appConfig,  // Spread the existing appConfig
  providers: [
    provideHttpClient(withFetch()), // Enable fetch for HttpClient
    ...(appConfig.providers || [])  // Include any existing providers from appConfig
  ]
})
.catch((err) => console.error(err));
