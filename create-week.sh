#!/bin/bash

cd /c/Users/Monster/Fullstack-Developer-Repo || { echo "❌ Repo klasörü bulunamadı."; exit 1; }

read -p "Yeni hafta numarasını girin (örn: 5): " WEEK
read -p "Konu başlığı (örn: Backend): " TOPIC
read -p "Tarih (örn: 29 Temmuz): " DATE
read -p "Durum (Tamamlandı / Devam ediyor / Eksik / Planlanıyor): " STATUS
read -p "Açıklama (TR): " DESC_TR
read -p "Açıklama (EN): " DESC_EN

WEEK_DIR="BE128/${WEEK}.Hafta"
INDEX_PATH="${WEEK_DIR}/index.html"
TEMPLATE="template.html"

# 📁 Klasörü oluştur
mkdir -p "$WEEK_DIR"

# 📝 template.html'yi kopyala
if [ -f "$TEMPLATE" ]; then
  cp "$TEMPLATE" "$INDEX_PATH"
else
  echo "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Hafta $WEEK</title></head><body><h1>Hafta $WEEK - $TOPIC</h1></body></html>" > "$INDEX_PATH"
fi

# 🧠 weeks.json'a yeni içerik ekle
jq --arg week "$WEEK" \
   --arg path "$INDEX_PATH" \
   --arg status "$STATUS" \
   --arg status_en "$(case "$STATUS" in "Tamamlandı") echo "Completed";;
                                      "Devam ediyor") echo "In progress";;
                                      "Eksik") echo "Missing";;
                                      "Planlanıyor") echo "Planned";;
                                      *) echo "$STATUS";; esac)" \
   --arg topic "$TOPIC" \
   --arg date "$DATE" \
   --arg lang "tr" \
   --arg desc_tr "$DESC_TR" \
   --arg desc_en "$DESC_EN" \
'. += [{
  week: ($week | tonumber),
  path: $path,
  status: $status,
  status_en: $status_en,
  topic: $topic,
  date: $date,
  lang: $lang,
  description_tr: $desc_tr,
  description_en: $desc_en
}]' weeks.json > tmp.json && mv tmp.json weeks.json

echo "✅ Hafta $WEEK başarıyla oluşturuldu: $INDEX_PATH"