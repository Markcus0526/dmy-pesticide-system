# NongYao Pesticide Management System

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Legacy Project](https://img.shields.io/badge/status-legacy-orange.svg)](https://github.com)

## Overview

**NongYao** (农药 - Pesticide/Farm Medicine) is a comprehensive management system for pesticide shops and distributors. It provides tools for inventory tracking, sales, purchasing, financial reporting, and statistics. The system includes:

- **Android Mobile App**: Field sales, inventory, and reporting on mobile devices.
- **.NET Backend & Services**: Server-side logic, APIs, and data processing (C# / ASP.NET / WCF).
- **Web/Desktop Frontend**: Likely for office/admin use.
- **SQL Server Database**: Centralized data storage for catalogs, transactions, and reports.

Originally developed around 2014 (VS2010, Android API 16), this is a legacy enterprise application for the agricultural chemical distribution industry.

## Features

Inferred from Android app activities and data types:

### Sales & Inventory
- Sales catalog management (add, reject, search, pay).
- Buy/purchase catalog.
- Store operations: moving, search, usage tracking.
- Base data: Shops, users, stores.

### Financial Management
- Money reports and payments (history, details).
- Bank accounts, other payments.
- Manual reports.

### Statistics & Analytics
- Shop statistics.
- Pesticide sales by type/region/month.
- Policy statistics (details 1-3).

### Additional Tools
- QR/Barcode scanning (ZXing integration).
- Printing support (custom formats).
- Location services (Baidu GPS).
- Camera for photos.
- Charts (line, stacked bar for sales).
- Bluetooth device support.

### Permissions (Android)
Internet, storage, location, camera, phone state, Bluetooth, etc.

## Architecture

```
Android App (Java) ─── HTTP/JSON ───> .NET Backend/Services (C#) ───> SQL Server DB
                                                      │
                                               Web Frontend (ASP.NET?)
```

- **Client**: Android v1.6 (min SDK 11).
- **Server**: NongYaoBackend (API?), NongYaoService (WCF?), NongYaoFrontEnd (UI).
- **DB**: NongYao.mdf (entities: catalogs, sales, stores, users, etc.).

## Directory Structure

```
.
├── Android/                  # Android app source & resources
│   ├── AndroidManifest.xml   # App config
│   ├── src/com/damy/nongyao/ # Java activities & utils
│   └── res/                  # Layouts, drawables, values
├── Database/                 # SQL Server files
│   ├── NongYao.mdf
│   └── NongYao_log.ldf
├── NongYaoBackend/           # .NET backend
│   └── NongYaoBackend.sln
├── NongYaoFrontEnd/          # .NET frontend
│   └── NongYaoFrontEnd.sln
├── NongYaoService/           # .NET services
│   └── NongYaoService.sln
├── README.md
└── .gitignore
```

## Quick Start

### Prerequisites
- **Visual Studio 2010+** (for .NET projects).
- **SQL Server** (2008+ for .mdf attach).
- **Eclipse or Android Studio** (legacy: Eclipse with ADT for Android build).
- **JDK 6-7** (for Android).

### 1. Database Setup
```
1. Open SQL Server Management Studio.
2. Right-click Databases → Attach → Select Database/ → Add NongYao.mdf & log.ldf.
3. Update connection strings in .NET config files (likely app.config/web.config).
```

### 2. Backend & Services
```
1. Open NongYaoBackend/NongYaoBackend.sln in Visual Studio.
2. Restore NuGet packages (packages/ dir).
3. Build (Debug/Release).
4. Run services (WCF? Update endpoints).
```

### 3. Frontend
```
1. Open NongYaoFrontEnd/NongYaoFrontEnd.sln.
2. Build & run (web/desktop).
```

### 4. Android App
```
1. Import Android/ as Eclipse project or use Android Studio (gradle migrate).
2. Update build paths, libs (armeabi).
3. Build APK.
4. Install: adb install bin/NongYao-debug.apk
5. Configure server URL in Global.java or prefs.
```

## API Endpoints
Not directly readable from files. Likely JSON over HTTP (see `HttpConnUsingJSON.java`). Common: login, sync sales/catalogs. Inspect backend controllers/services post-build.

## Contributing
1. Fork & clone.
2. Create feature branch.
3. Modernization suggestions: Update Android to Kotlin/Jetpack, .NET to Core/8, DB to migrations.
4. PR to `main`.

## Status & TODO
- Legacy (2014-era).
- TODO:
  - Migrate Android to modern SDK.
  - Containerize backend (Docker).
  - Add API docs (Swagger).
  - Unit tests.
  - CI/CD (GitHub Actions).

## License
MIT License - see [LICENSE](LICENSE) (add if missing).

---

*Built with ❤️ for agricultural management.*

