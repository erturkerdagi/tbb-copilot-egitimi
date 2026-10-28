# Implementation Plan: Düzenli Ödeme Talimatı

**Branch**: `002-duzenli-odeme-talimati` | **Date**: 2026-11-05 | **Spec**: [spec.md](spec.md)

## Summary

İki yeni varlık (`PaymentInstruction`, `PaymentInstructionRun`), bir tarih hesaplayıcı (`ScheduleCalculator`), talimat servisi ve gün sonu çalıştırıcısı eklenir.
Çalıştırıcı, transferleri mevcut `TransferService` üzerinden yapar; böylece bakiye ve günlük limit kuralları yeniden yazılmaz (FR-007).

## Technical Context

**Language/Version**: C# 14, .NET 10

**Primary Dependencies**: ASP.NET Core Minimal API, EF Core 10 (SQLite) — yeni paket yok

**Storage**: İki yeni tablo; `PaymentInstructionRuns` üzerinde (InstructionId, PeriodYear, PeriodMonth) benzersiz indeksi (FR-009)

**Testing**: xUnit, bellek içi depolar

**Project Type**: web-service

## Constitution Check

| İlke | Durum | Not |
| --- | --- | --- |
| I. Önce Test | Uygun | Tarih kuralları ve çalıştırıcı için testler önce |
| II. Kişisel Veri | Uygun | Log'da yalnızca talimat ve hesap Id |
| III. Parasal Doğruluk | Uygun | `decimal`, transfer mevcut servis üzerinden |
| IV. Hata Yönetimi | Uygun | `PaymentInstructionErrors`, çalıştırmada hata kodu |
| V. Sadelik | Uygun | Transfer mantığı yeniden kullanılır; zamanlayıcı yerine demo ucu |

## Project Structure

```text
src/Bank.Api/
├── Domain/PaymentInstruction.cs
├── Contracts/PaymentInstructionContracts.cs
├── Services/ScheduleCalculator.cs
├── Services/PaymentInstructionAbstractions.cs
├── Services/PaymentInstructionService.cs
├── Services/PaymentInstructionRunner.cs
├── Data/PaymentInstructionRepository.cs
├── Data/BankDbContext.cs              # yeni DbSet'ler ve indeks
├── Endpoints/PaymentInstructionEndpoints.cs
└── Program.cs

tests/Bank.Api.Tests/
├── ScheduleCalculatorTests.cs
├── PaymentInstructionTests.cs
└── Fakes/PaymentFakes.cs
```

## Riskler

| Risk | Azaltma |
| --- | --- |
| Hafta sonu kaydırması talimatı bir sonraki aya taşır, o ay atlanır | Çalıştırıcı önceki ve mevcut dönemi birlikte kontrol eder |
| Aynı gün çift çalıştırma | Dönem bazlı kayıt + benzersiz indeks |
| Gün sonu ucu dışarıya açık | Demo amaçlı; üretimde zamanlanmış iş ve yetkilendirme |
