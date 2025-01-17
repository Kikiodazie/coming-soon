﻿import 'zone.js';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { CoreModule } from '@coming-soon/core';

enableProdMode();

platformBrowserDynamic().bootstrapModule(CoreModule);
