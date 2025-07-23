#!/bin/bash
set -euo pipefail

echo -e "\e[36m🔄 gh-pages dalından main dalına geçiş başlatılıyor...\e[0m"

# Proje klasörüne git
cd ~/Fullstack-Developer-Repo || { echo -e "\e[31m❌ Klasör bulunamadı\e[0m"; exit 1; }

# gh-pages dalına geç
git checkout gh-pages || { echo -e "\e[31m❌ gh-pages dalına geçilemedi\e[0m"; exit 1; }

# BE128 klasörünü geçici olarak yedekle
if [ -d BE128 ]; then
  cp -r BE128 ../BE128-ghpages-backup
  echo -e "\e[34m📁 BE128 klasörü yedeklendi.\e[0m"
else
  echo -e "\e[33mBE128 klasörü bulunamadı, yedekleme atlandı.\e[0m"
fi

# main dalına geç
git checkout main || { echo -e "\e[31m❌ main dalına geçilemedi\e[0m"; exit 1; }

# Yedek klasörü geri taşı
if [ -d ../BE128-ghpages-backup ]; then
  rm -rf ./BE128
  mv ../BE128-ghpages-backup ./BE128
  echo -e "\e[34m📦 BE128 klasörü main dalına taşındı.\e[0m"
else
  echo -e "\e[33mYedek klasör bulunamadı, taşıma atlandı.\e[0m"
fi

# GitHub’a gönder
git add BE128
COMMIT_DATE=$(date '+%Y-%m-%d %H:%M')
git commit -m "BE128 klasörü gh-pages dalından main dalına taşındı - $COMMIT_DATE"
git push origin main

echo -e "\e[32m✅ Taşıma işlemi tamamlandı ve GitHub'a gönderildi.\e[0m"
echo -e "\e[33m🌐 GitHub Pages ayarlarını main dalına göre güncellemeyi unutma!\e[0m"