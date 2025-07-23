#!/bin/bash
set -euo pipefail

cd ~/Fullstack-Developer-Repo || { echo -e "\e[31m❌ Klasör bulunamadı\e[0m"; exit 1; }

if git status | grep -q "BE128"; then
    echo -e "\e[36m🔄 BE128 klasöründe değişiklikler algılandı. Güncelleniyor...\e[0m"

    # Sadece belirli dosya türlerini sahneye al
    find BE128 -type f \( -name "*.html" -o -name "*.js" -o -name "*.java" -o -name "*.css" \) -exec git add {} \;

    # Güncelleme tarihi
    UPDATE_DATE=$(date '+%Y-%m-%d %H:%M')

    # Dosya listesini oluştur
    TABLE_ROWS=""
    while IFS= read -r file; do
        FILENAME=$(basename "$file")
        EXT="${FILENAME##*.}"
        MODIFIED=$(date -r "$file" '+%Y-%m-%d %H:%M')
        TABLE_ROWS+="<tr><td>$FILENAME</td><td>${EXT^^}</td><td>$MODIFIED</td></tr>\n"
    done < <(find BE128 -type f)

    # Güncelleme geçmişi satırı
    echo "$UPDATE_DATE - BE128 klasörü güncellendi" >> update.log

    # Güncelleme geçmişini oku
    LOG_ENTRIES=""
    while IFS= read -r line; do
        LOG_ENTRIES+="<li>$line</li>\n"
    done < update.log

    # Mevcut index.html dosyasını güncelle
    sed -i "/<tbody>/,/<\/tbody>/c\<tbody>\n$TABLE_ROWS<\/tbody>" BE128/index.html
    sed -i "/<ul>/,/<\/ul>/c\<ul>\n$LOG_ENTRIES<\/ul>" BE128/index.html
    sed -i "s|<span id=\"date\"></span>|$UPDATE_DATE|" BE128/index.html

    # README güncelle
    echo "Son güncelleme: $UPDATE_DATE" > BE128/README.md
    git add BE128/index.html BE128/README.md

    # Commit ve push
    git commit -m "BE128 tablo ve log güncellemesi - $UPDATE_DATE"
    git push origin main

    echo -e "\a"
    echo -e "\e[32m✅ index.html başarıyla güncellendi!\e[0m"
else
    echo -e "\e[32m🟢 BE128 klasöründe değişiklik yok. Güncelleme yapılmadı.\e[0m"
fi