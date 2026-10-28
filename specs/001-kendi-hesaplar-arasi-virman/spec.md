# Feature Specification: Kendi Hesaplar Arası Virman

**Feature Branch**: `001-kendi-hesaplar-arasi-virman`

**Created**: 2026-11-03

**Status**: Draft

**Input**: User description: "Müşteri kendi hesapları arasında, günlük limitten bağımsız olarak para aktarabilsin."

## Clarifications

### Session 2026-11-03

- Q: Virman, hesabın günlük transfer limitinden düşülecek mi? → A: Hayır. Kendi hesaplar arası virman günlük limite tabi değildir ve sonraki dış transferlerin limit hesabına da katılmaz.
- Q: Hesaplardan biri müşteriye ait değilse yanıt ne olmalı? → A: "Hesap bulunamadı" (404). Başkasına ait bir hesabın varlığı ele verilmez.
- Q: Farklı para birimli hesaplar arasında virman yapılabilir mi? → A: Bu sürümde hayır; kur dönüşümü kapsam dışı.
- Q: Virman için ücret alınacak mı? → A: Hayır.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Vadesizden birikime para aktarma (Priority: P1)

Ayşe Hanım vadesiz hesabındaki parayı aynı gün içinde birikim hesabına aktarmak istiyor. Bu aktarım, başka kişilere gönderdiği paralar için tanımlı günlük limitten etkilenmemeli.

**Why this priority**: Mobil uygulamada en sık talep edilen özellik; müşteri şikâyetlerinin önemli bir kısmı kendi hesapları arasında para aktaramamaktan kaynaklanıyor.

**Independent Test**: İki hesabı olan bir müşteri için virman isteği gönderilir; iki hesabın bakiyesi doğru güncellenir ve işlem geçmişte görünür.

**Acceptance Scenarios**:

1. **Given** Ayşe'nin vadesiz hesabında 25.000 TL ve birikim hesabında 100.000 TL var, **When** vadesizden birikime 5.000 TL virman yapar, **Then** vadesiz 20.000 TL, birikim 105.000 TL olur.
2. **Given** Ayşe'nin günlük limiti 10.000 TL, **When** kendi hesapları arasında 15.000 TL virman yapar, **Then** işlem başarılı olur.
3. **Given** Ayşe bugün kendi hesapları arasında 15.000 TL virman yaptı, **When** başka bir müşteriye 5.000 TL transfer yapar, **Then** limit kontrolü yalnızca dış transferleri sayar ve işlem başarılı olur.

---

### User Story 2 - Başkasının hesabını kullanma girişiminin engellenmesi (Priority: P1)

Bir müşteri, istek üzerinde oynayarak başka bir müşterinin hesabını virman kaynağı veya hedefi olarak göstermeye çalışıyor.

**Why this priority**: Güvenlik gereksinimi; başka müşterinin hesabından para çekilebilmesi kabul edilemez.

**Independent Test**: Müşteriye ait olmayan bir hesap Id'siyle istek gönderilir; işlem reddedilir ve bakiyeler değişmez.

**Acceptance Scenarios**:

1. **Given** Mehmet'in hesabı Ayşe'ye ait değil, **When** Ayşe adına Mehmet'in hesabından virman istenir, **Then** "Hesap bulunamadı" hatası döner ve hiçbir bakiye değişmez.

---

### Edge Cases

- Tutar sıfır veya negatifse işlem reddedilir.
- Kaynak ve hedef hesap aynıysa işlem reddedilir.
- Kaynak hesabın bakiyesi yetersizse işlem reddedilir.
- Hesapların para birimi farklıysa işlem reddedilir.
- Bakiyenin tamamı aktarılabilir (bakiye = tutar).

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Sistem, müşterinin kendi iki hesabı arasında para aktarımına izin VERMELİDİR.
- **FR-002**: Sistem, kaynak ve hedef hesabın isteği yapan müşteriye ait olduğunu DOĞRULAMALIDIR; değilse "Hesap bulunamadı" yanıtı VERMELİDİR.
- **FR-003**: Sistem, tutarın sıfırdan büyük olduğunu DOĞRULAMALIDIR.
- **FR-004**: Sistem, kaynak ve hedef hesabın farklı olduğunu DOĞRULAMALIDIR.
- **FR-005**: Sistem, kaynak hesap bakiyesinin tutarı karşıladığını DOĞRULAMALIDIR.
- **FR-006**: Sistem, iki hesabın para biriminin aynı olduğunu DOĞRULAMALIDIR.
- **FR-007**: Virman, günlük transfer limitine TABİ OLMAMALIDIR ve dış transferlerin limit hesabına KATILMAMALIDIR.
- **FR-008**: Virman, işlem geçmişinde dış transferlerden ayırt edilebilir şekilde KAYDEDİLMELİDİR.

### Key Entities

- **Virman**: Aynı müşteriye ait iki hesap arasındaki para aktarımı. Kaynak hesap, hedef hesap, tutar, açıklama ve işlem zamanı içerir.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Müşteri, kendi hesapları arasında para aktarımını tek bir istekle tamamlayabilir.
- **SC-002**: Başka müşteriye ait hesapla yapılan virman denemelerinin tamamı reddedilir.
- **SC-003**: Virmanlar, dış transfer limitini hiçbir koşulda tüketmez.

## Assumptions

- Kimlik doğrulama bu sürümün kapsamı dışındadır; müşteri kimliği URL'den alınır ve ileride kimlik doğrulama katmanından gelecektir.
- Kur dönüşümü kapsam dışıdır.
- Virman için ücret alınmaz.
