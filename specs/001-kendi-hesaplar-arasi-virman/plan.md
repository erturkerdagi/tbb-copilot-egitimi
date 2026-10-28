# Implementation Plan: Kendi Hesaplar Arası Virman

**Branch**: `001-kendi-hesaplar-arasi-virman` | **Date**: 2026-11-03 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `/specs/001-kendi-hesaplar-arasi-virman/spec.md`

## Summary

Müşterinin kendi hesapları arasında, günlük limitten bağımsız para aktarabilmesi için yeni bir uç (`POST /customers/{customerId}/internal-transfers`) eklenir.
Virmanlar mevcut `Transfer` tablosunda yeni bir `Kind = Internal` alanıyla tutulur; günlük limit hesabı yalnızca `Kind = External` kayıtları sayar.

## Technical Context

**Language/Version**: C# 14, .NET 10

**Primary Dependencies**: ASP.NET Core Minimal API, EF Core 10 (SQLite) — yeni paket yok

**Storage**: SQLite (`erdagibank.db`), mevcut `Transfers` tablosuna `Kind` sütunu

**Testing**: xUnit, `Fakes/` altındaki bellek içi depolar

**Target Platform**: Linux/Windows sunucu, container

**Project Type**: web-service

**Performance Goals**: Mevcut transfer ucuyla aynı

**Constraints**: Anayasa III (decimal), II (kişisel veri log'a yazılmaz)

**Scale/Scope**: Tek uç, bir servis, bir domain alanı

## Constitution Check

| İlke | Durum | Not |
| --- | --- | --- |
| I. Önce Test | Uygun | tasks.md'de testler uygulamadan önce |
| II. Kişisel Veri | Uygun | Log'da yalnızca hesap Id |
| III. Parasal Doğruluk | Uygun | `decimal`, bakiye `Withdraw`/`Deposit` ile |
| IV. Hata Yönetimi | Uygun | `InternalTransferErrors`, `Result<T>` |
| V. Sadelik | Uygun | Yeni tablo yerine mevcut `Transfer`'e alan eklenir |

## Project Structure

### Documentation (this feature)

```text
specs/001-kendi-hesaplar-arasi-virman/
├── spec.md
├── plan.md
├── contracts/
│   └── internal-transfers.md
└── tasks.md
```

### Source Code (repository root)

```text
src/Bank.Api/
├── Domain/Transfer.cs                 # TransferKind alanı
├── Contracts/Contracts.cs             # InternalTransferRequest
├── Services/InternalTransferService.cs
├── Services/InternalTransferErrors.cs
├── Data/Repositories.cs               # limit toplamında yalnızca External
├── Endpoints/CustomerEndpoints.cs     # yeni uç
└── Program.cs                         # servis kaydı ve MapCustomerEndpoints

tests/Bank.Api.Tests/
├── InternalTransferServiceTests.cs
└── Fakes/Fakes.cs                     # limit toplamında yalnızca External
```

**Structure Decision**: Mevcut tek proje yapısı korunur. Virman ayrı bir servis olarak yazılır; `TransferService` değiştirilmez, yalnızca depo sorgusu dış transferlerle sınırlanır.

## Complexity Tracking

Anayasa ihlali yok.
