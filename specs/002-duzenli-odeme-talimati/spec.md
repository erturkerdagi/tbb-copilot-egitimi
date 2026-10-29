# Feature Specification: Düzenli Ödeme Talimatı

**Feature Branch**: `002-duzenli-odeme-talimati`

**Created**: 2026-11-05

**Status**: Draft

**Input**: User description: "Müşteri, her ay belirli bir günde, belirli bir hesaba, belirli tutarda tekrarlayan transfer tanımlayabilsin."

## Clarifications

### Session 2026-11-05

- Q: Talimat günü ayda yoksa (ör. 31 Şubat) ne olur? → A: Ayın son günü kullanılır.
- Q: Gerçekleşme tarihi hafta sonuna denk gelirse? → A: Sonraki pazartesi gerçekleşir; bu tarih bir sonraki aya taşabilir. Resmi tatiller bu sürümde kapsam dışı.
- Q: Talimat günlük transfer limitine tabi mi? → A: Evet, normal dış transfer gibi işlenir; bakiye ve limit kontrolleri geçerlidir.
- Q: Bakiye yetersizse? → A: Çalıştırma "başarısız" olarak kaydedilir ve o dönem içinde tekrar denenmez.
- Q: Alıcı hesap başka bir müşteriye ait olabilir mi? → A: Evet. Gönderen hesap ise mutlaka talimatı veren müşteriye ait olmalıdır.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Kira ödemesi talimatı (Priority: P1)

Ayşe Hanım her ayın 5'inde ev sahibi Mehmet Bey'e 1.500 TL kira göndermek istiyor ve bunu her ay elle yapmak istemiyor.

**Why this priority**: Mobil uygulamada en çok istenen ikinci özellik; kira, aidat, harçlık gibi düzenli ödemeler.

**Independent Test**: Talimat oluşturulur, ayın 5'i için gün sonu çalıştırması tetiklenir; transfer gerçekleşir ve bakiyeler güncellenir.

**Acceptance Scenarios**:

1. **Given** Ayşe'nin vadesiz hesabında 25.000 TL var, **When** her ayın 5'i için 1.500 TL talimat verir ve 5 Kasım 2026'da gün sonu çalıştırılır, **Then** Ayşe'nin hesabı 23.500 TL, Mehmet'in hesabı 6.500 TL olur.
2. **Given** talimat bugün zaten çalıştı, **When** gün sonu tekrar çalıştırılır, **Then** ikinci bir transfer yapılmaz.
3. **Given** talimat günü ayın 10'u, **When** ayın 5'inde gün sonu çalıştırılır, **Then** hiçbir transfer yapılmaz.

---

### User Story 2 - Ay sonu ve hafta sonu kuralları (Priority: P1)

Zeynep Hanım apartman aidatını her ayın 31'inde ödemek istiyor.

**Why this priority**: Yanlış tarihte para çekilmesi doğrudan müşteri şikâyeti ve itibar kaybıdır.

**Independent Test**: Gerçekleşme tarihi hesabı kısa aylar, hafta sonları ve artık yıllar için ayrı ayrı test edilir.

**Acceptance Scenarios**:

1. **Given** talimat günü 31, **When** Kasım 2026 (30 gün) için tarih hesaplanır, **Then** 30 Kasım 2026 olur.
2. **Given** talimat günü 30, **When** Mayıs 2026 için tarih hesaplanır (30 Mayıs Cumartesi), **Then** 1 Haziran 2026 Pazartesi olur.
3. **Given** talimat günü 31, **When** Ocak 2027 dönemi için (31 Ocak Pazar) 1 Şubat 2027'de gün sonu çalışır, **Then** Ocak dönemine ait transfer gerçekleşir.

---

### User Story 3 - Başarısız ödeme (Priority: P2)

Ayşe Hanım'ın hesabında talimat günü yeterli para yok.

**Why this priority**: Başarısız ödemeler kayıt altına alınmalı; aynı gün içinde defalarca denenmemeli.

**Independent Test**: Bakiyesi yetersiz bir hesap için çalıştırma yapılır; başarısız kayıt oluşur, bakiye değişmez.

**Acceptance Scenarios**:

1. **Given** bakiye 25.000 TL ve talimat 30.000 TL, **When** gün sonu çalışır, **Then** çalıştırma "Failed" ve hata kodu `transfer.insufficient_balance` olarak kaydedilir.
2. **Given** çalıştırma başarısız oldu, **When** aynı gün tekrar çalıştırılır, **Then** tekrar denenmez.

### Edge Cases

- Tutar sıfır veya negatifse talimat reddedilir.
- Gün 1–31 aralığı dışındaysa talimat reddedilir.
- Gönderen hesap müşteriye ait değilse "Gönderen hesap bulunamadı" döner.
- Hesapların para birimi farklıysa talimat reddedilir.
- Talimat tutarı günlük limiti aşıyorsa çalıştırma başarısız olur.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Müşteri, kendi hesabından herhangi bir hesaba aylık düzenli ödeme talimatı OLUŞTURABİLMELİDİR.
- **FR-002**: Sistem, tutarın sıfırdan büyük ve günün 1–31 aralığında olduğunu DOĞRULAMALIDIR.
- **FR-003**: Sistem, gönderen hesabın talimatı veren müşteriye ait olduğunu DOĞRULAMALIDIR.
- **FR-004**: Sistem, iki hesabın para biriminin aynı olduğunu DOĞRULAMALIDIR.
- **FR-005**: Talimat günü ayda yoksa ayın son günü KULLANILMALIDIR.
- **FR-006**: Gerçekleşme tarihi hafta sonuna denk gelirse sonraki pazartesiye KAYDIRILMALIDIR.
- **FR-007**: Talimat, normal dış transfer olarak işlenmeli; bakiye ve günlük limit kuralları UYGULANMALIDIR.
- **FR-008**: Başarısız çalıştırmalar hata koduyla KAYDEDİLMELİ ve aynı dönem içinde tekrar DENENMEMELİDİR.
- **FR-009**: Bir talimat aynı dönem (yıl/ay) için en fazla bir kez ÇALIŞMALIDIR.
- **FR-010**: Müşteri kendi talimatlarını LİSTELEYEBİLMELİDİR.

### Key Entities

- **Düzenli ödeme talimatı**: Müşteri, gönderen hesap, alıcı hesap, tutar, ayın günü, açıklama, aktiflik.
- **Talimat çalıştırma kaydı**: Talimat, dönem (yıl/ay), gerçekleşme tarihi, sonuç, hata kodu, oluşan transfer.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Talimatların tamamı doğru günde (kısa ay ve hafta sonu kuralları dahil) çalışır.
- **SC-002**: Hiçbir talimat aynı dönem için birden fazla transfer üretmez.
- **SC-003**: Başarısız ödemelerin tamamı hata koduyla izlenebilir.

## Assumptions

- Resmi tatiller kapsam dışıdır.
- Talimat iptali ve güncelleme bu sürümde yoktur.
- Gün sonu çalıştırması bir iş (job) tarafından günde bir kez tetiklenir; demoda bir uç üzerinden tetiklenir.
- Kimlik doğrulama kapsam dışıdır; müşteri kimliği URL'den alınır.
