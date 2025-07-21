#!/bin/bash

echo "🔄 gh-pages dalından main dalına geçiş başlatılıyor..."

# Proje klasörüne git
cd ~/Fullstack-Developer-Repo || { echo "❌ Klasör bulunamadı"; exit 1; }

# gh-pages dalına geç
git checkout gh-pages || { echo "❌ gh-pages dalına geçilemedi"; exit 1; }

# BE128 klasörünü geçici olarak yedekle
cp -r BE128 ../BE128-ghpages-backup
echo "📁 BE128 klasörü yedeklendi."

# main dalına geç
git checkout main || { echo "❌ main dalına geçilemedi"; exit 1; }

# Yedek klasörü geri taşı
mv ../BE128-ghpages-backup ./BE128
echo "📦 BE128 klasörü main dalına taşındı."

# GitHub’a gönder
git add BE128
COMMIT_DATE=$(date '+%Y-%m-%d %H:%M')
git commit -m "BE128 klasörü gh-pages dalından main dalına taşındı - $COMMIT_DATE"
git push origin main

echo "✅ Taşıma işlemi tamamlandı ve GitHub'a gönderildi."
echo "🌐 GitHub Pages ayarlarını main dalına göre güncellemeyi unutma!"