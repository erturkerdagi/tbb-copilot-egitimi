# Tasks: Kendi Hesaplar Arası Virman

**Input**: Design documents from `/specs/001-kendi-hesaplar-arasi-virman/`

**Prerequisites**: plan.md, spec.md, contracts/internal-transfers.md

## Phase 1: Setup

- [x] T001 `Transfer` varlığına `TransferKind` (External/Internal) alanı ekle — `src/Bank.Api/Domain/Transfer.cs`
- [x] T002 Günlük limit toplamını yalnızca `External` transferlerle sınırla — `src/Bank.Api/Data/Repositories.cs`, `tests/Bank.Api.Tests/Fakes/Fakes.cs`

## Phase 2: User Story 1 — Vadesizden birikime aktarma (P1)

### Tests (önce yazılır, önce kırmızı görülür)

- [x] T003 [P] [US1] Başarılı virmanda iki bakiyenin güncellenmesi testi — `tests/Bank.Api.Tests/InternalTransferServiceTests.cs`
- [x] T004 [P] [US1] Günlük limitin üstündeki virmanın kabul edilmesi testi
- [x] T005 [P] [US1] Virmandan sonra dış transferin limitten etkilenmemesi testi
- [x] T006 [P] [US1] Sıfır/negatif tutar, aynı hesap, yetersiz bakiye, para birimi farkı testleri
- [x] T007 [P] [US1] Bakiyenin tamamının aktarılabilmesi testi

### Implementation

- [x] T008 [US1] `InternalTransferRequest` sözleşmesi — `src/Bank.Api/Contracts/Contracts.cs`
- [x] T009 [US1] `InternalTransferErrors` — `src/Bank.Api/Services/InternalTransferErrors.cs`
- [x] T010 [US1] `InternalTransferService` — `src/Bank.Api/Services/InternalTransferService.cs`
- [x] T011 [US1] `POST /customers/{customerId}/internal-transfers` — `src/Bank.Api/Endpoints/CustomerEndpoints.cs`, `Program.cs`

## Phase 3: User Story 2 — Başkasının hesabının engellenmesi (P1)

- [x] T012 [US2] Müşteriye ait olmayan kaynak hesap testi (404, bakiyeler değişmez)
- [x] T013 [US2] Müşteriye ait olmayan hedef hesap testi
- [x] T014 [US2] Sahiplik kontrolünün servise eklenmesi

## Phase 4: Polish

- [x] T015 `Bank.Api.http` dosyasına örnek istekler
- [x] T016 `dotnet build` ve `dotnet test` ile doğrulama
