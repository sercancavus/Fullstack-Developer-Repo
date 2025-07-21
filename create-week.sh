#!/bin/bash

# === Ayarlar ===
BASE_DIR="BE128"
WEEKS_DIR="$BASE_DIR"
TEMPLATE="template.html"
PREFIX=".Hafta"
MAX_WEEK=16
WEEKS_JSON="weeks.json"

# === Haftaya özel bilgiler (rozet verileri)
STATUS="Tamamlandı"
TOPIC="WebAPI"
DATE="22 Temmuz"
LANG="tr"

# === Mevcut son haftayı bul
last_week=$(ls "$WEEKS_DIR" | grep -E '^[0-9]+\.Hafta$' | sed 's/\.Hafta//' | sort -n | tail -1)
new_week=$(( ${last_week:-0} + 1 ))

# === Sınır kontrolü
if (( new_week > MAX_WEEK )); then
  echo "🚫 $MAX_WEEK haftaya ulaşıldı."
  exit 1
fi

# === Yeni klasör ve index.html oluştur
new_folder="${new_week}${PREFIX}"
mkdir -p "$WEEKS_DIR/$new_folder"
cp "$TEMPLATE" "$WEEKS_DIR/$new_folder/index.html"
echo "📂 $new_folder klasörü oluşturuldu."

# === weeks.json güncelle (rozetli veri girişi)
new_entry="{\"week\": ${new_week}, \"path\": \"BE128/${new_folder}/index.html\", \"status\": \"${STATUS}\", \"topic\": \"${TOPIC}\", \"date\": \"${DATE}\", \"lang\": \"${LANG}\"}"

if [ ! -f "$WEEKS_JSON" ]; then
  echo "[$new_entry]" > "$WEEKS_JSON"
  echo "📄 Yeni weeks.json oluşturuldu."
else
  sed -i '$ s/\]/,\n  '"$new_entry"'\n]/' "$WEEKS_JSON"
  echo "📌 weeks.json güncellendi: Hafta ${new_week} eklendi."
fi

# === Git işlemleri
git add .
git commit -m "✨ ${new_folder} eklendi · ${STATUS}, ${TOPIC}, ${DATE}, ${LANG}"
git push origin main
echo "🚀 Git push başarılı!"