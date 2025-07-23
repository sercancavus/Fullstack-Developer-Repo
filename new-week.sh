#!/bin/bash
set -euo pipefail

# Ayarlar
BASE_DIR="BE128/Haftalar"
MAIN_HTML="Fullstack-Developer-Repo/index.html"
TEMPLATE_HTML="template.html"
CARD_TEMPLATE="card.html" # Hafta kartı şablonu varsa
PREFIX="Hafta"

# Son haftayı bul
last_week=$(ls "$BASE_DIR" | grep "$PREFIX" | sed "s/$PREFIX//" | sort -n | tail -1)
new_week=$((last_week + 1))
new_folder="${PREFIX}${new_week}"

# Şablon dosya kontrolü
test -f "$TEMPLATE_HTML" || { echo -e "\e[31m❌ $TEMPLATE_HTML bulunamadı!\e[0m"; exit 1; }

# Yeni klasörü oluştur
mkdir -p "$BASE_DIR/$new_folder"
cp "$TEMPLATE_HTML" "$BASE_DIR/$new_folder/index.html"

echo -e "\e[32m✅ $new_folder klasörü ve index.html oluşturuldu.\e[0m"

# Yeni kart içeriğini oluştur
new_card=$(cat <<EOF
<div class="col">
  <div class="card h-100 shadow-sm">
    <div class="card-body">
      <h5 class="card-title">Hafta ${new_week}</h5>
      <p class="card-text">İçeriği görmek için tıklayın.</p>
      <a href="BE128/Haftalar/${new_folder}/index.html" class="btn btn-primary">Git</a>
    </div>
  </div>
</div>
EOF
)

# Ana index.html dosyasını güncelle
awk -v card="$new_card" '
/<!-- WEEK-CARDS-START -->/ { print; in_block=1; next }
/<!-- WEEK-CARDS-END -->/ && in_block { print card; print; in_block=0; next }
{ print }
' "$MAIN_HTML" > temp.html && mv temp.html "$MAIN_HTML"

echo -e "\e[32m✅ Ana sayfa güncellendi.\e[0m"