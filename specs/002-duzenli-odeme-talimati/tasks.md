# Tasks: Düzenli Ödeme Talimatı

## Phase 1: Setup

- [x] T001 `PaymentInstruction` ve `PaymentInstructionRun` varlıkları — `Domain/PaymentInstruction.cs`
- [x] T002 EF Core yapılandırması ve benzersiz indeks — `Data/BankDbContext.cs`
- [x] T003 Depo arayüzü, EF ve bellek içi uygulamaları

## Phase 2: User Story 2 — Tarih kuralları (önce, çünkü US1 buna bağlı)

- [x] T004 [P] [US2] Hafta içi, cumartesi, pazar testleri — `ScheduleCalculatorTests.cs`
- [x] T005 [P] [US2] Kısa ay, aya taşma, artık yıl testleri
- [x] T006 [US2] `ScheduleCalculator.ExecutionDateFor`

## Phase 3: User Story 1 — Talimat oluşturma ve çalıştırma

- [x] T007 [P] [US1] Talimat oluşturma doğrulama testleri — `PaymentInstructionTests.cs`
- [x] T008 [P] [US1] Günü gelen / gelmeyen / ikinci kez çalıştırma testleri
- [x] T009 [US1] `PaymentInstructionService`
- [x] T010 [US1] `PaymentInstructionRunner` (önceki + mevcut dönem kontrolü)
- [x] T011 [US1] Uçlar ve `Program.cs` kayıtları

## Phase 4: User Story 3 — Başarısız ödeme

- [x] T012 [US3] Yetersiz bakiye ve limit aşımı testleri, tekrar denememe testi
- [x] T013 [US3] Başarısız çalıştırmanın hata koduyla kaydı

## Phase 5: Polish

- [x] T014 `Bank.Api.http` örnek istekleri
- [x] T015 `dotnet build`, `dotnet test`, güvenlik inceleyici agent ile inceleme
