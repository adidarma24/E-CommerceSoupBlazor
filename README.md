# 🍲 E-Commerce Soup Blazor

A modern, scalable, and fully interactive E-Commerce Web Application built with Blazor, .NET, MudBlazor, Swagger, and integrated with clean architecture principles.

Proyek ini dirancang sebagai platform e-commerce sederhana namun powerful, dengan fitur lengkap mulai dari manajemen produk, keranjang belanja, hingga autentikasi pengguna.
---

## 🚀 Tech Stack

Frontend: Blazor, MudBlazor (UI Components)
Backend: .NET, C#, Clean Architecture
API Tools: Swagger (API Documentation), SonarQube (Code Quality)
Database: SQL Server / PostgreSQL (opsional)
Tools: Git, GitHub, Visual Studio / VS Code

### 🧩 Fitur Utama

- 🖥️ **Blazor UI Modern** – Dibangun dengan **MudBlazor** & **Tailwind-style design**.
- ⚙️ **ASP.NET Core Web API** – Backend modular & efisien.
- 📊 **ApexCharts.Blazor** – Visualisasi data interaktif.
- 💾 **Blazored.LocalStorage** – Penyimpanan sisi klien.
- 🐳 **Docker Support** – Siap dijalankan di container.

---

## 🗂️ Struktur Direktori

```
src/
 ├── 06.WebAPI/        # Backend (ASP.NET Core Web API)
 ├── 08.BlazorUI/       # Frontend (Blazor UI)
 └── ...                # Folder pendukung lain
```

---

## 🧰 Prasyarat

Pastikan kamu sudah menginstal:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/)
- [Git](https://git-scm.com/)

Opsional:

- Visual Studio / VS Code untuk development.

---

## 💻 Cara Menjalankan

### 🔹 Backend (Web API)

```bash
cd src/06.WebAPI
dotnet restore
dotnet build
dotnet ef database update  # jika menggunakan migrasi EF Core
dotnet run
```

➡️ Akses di: `http://localhost:5099/swagger/index.html` 

### 🔹 Frontend (Blazor UI)

```bash
cd src/08.BlazorUI
dotnet restore
dotnet build
dotnet run
```

➡️ Akses di: `http://localhost:5099`

---

## ⚙️ Konfigurasi Environment

Buat file `appsettings.Development.json` di src/06.WebAPI:

```
ConnectionStrings__DefaultConnection=Server=localhost;Database=soupdb;User Id=sa;Password=YourPassword;
```

---

## 🧾 Perintah Migrasi Database (EF Core)

```bash
cd src/06.WebAPI
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Jika error “exclusive lock for migration”, pastikan tidak ada proses `dotnet` lain yang mengakses DB.

---

**Kelompok 4 – SOUP Project**
Kontributor utama: [ZhenanSky](https://github.com/zhenansky). [AdiDharma](https://github.com/adidarma24). [Dean](https://github.com/Dean-Tr)

---

## 📜 Lisensi

Proyek ini dilisensikan di bawah **MIT License** — bebas digunakan & dimodifikasi dengan tetap mencantumkan kredit.

---

> Dibuat dengan ❤️ oleh Kelompok 4
> 🚀 Powered by .NET, Blazor, dan Docker
