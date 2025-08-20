#!/bin/bash
set -euo pipefail

# Load git helper functions for safe branch operations
source "$(dirname "$0")/git-helper.sh" 2>/dev/null || {
    echo -e "\e[33m⚠️  git-helper.sh not found, using basic git operations\e[0m"
}

WEEK=$1
COURSE_DIR="BE128"
WEEK_DIR="$COURSE_DIR/${WEEK}.Hafta"
HTML_FILE="$WEEK_DIR/index.html"

if [ -z "$WEEK" ]; then
  echo -e "\e[31mKullanım: ./createWeek.sh <HaftaNumarası>\e[0m"
  exit 1
fi

if [ -d "$WEEK_DIR" ]; then
  echo -e "\e[33m❌ ${WEEK}.Hafta klasörü zaten mevcut!\e[0m"
  exit 1
fi

mkdir -p "$WEEK_DIR"

cat <<EOF > "$HTML_FILE"
<!DOCTYPE html>
<html lang="tr">
<head>
  <meta charset="UTF-8">
  <title>${WEEK}. Hafta</title>
</head>
<body>
  <h1>${WEEK}. Hafta</h1>
  <p>Ders içeriği buraya gelecek.</p>
</body>
</html>
EOF

# README.md ve update.log dosyalarını kontrol et, yoksa oluştur
[ -f README.md ] || echo -e "# Full Stack Öğrenme Yolculuğu 🚀\n\nGüncel haftalar listesi:\n" > README.md
[ -f update.log ] || touch update.log

# README.md'de haftalar listesini güncel ve sıralı tut
# Haftalar listesini oku, yeni haftayı ekle, sırala ve başlık altına yaz
awk -v week="- ${WEEK}.Hafta klasörü eklendi." 'BEGIN{found=0}{if(NR==1){print $0} else if(NR==2){print $0; found=1; print week} else if(found && $0 ~ /^- [0-9]+\.Hafta/){print $0} else if(!found){print $0}}' README.md | sort -u > README.tmp && mv README.tmp README.md

# update.log'a kayıt ekle
echo "$(date '+%Y-%m-%d %H:%M:%S') - ${WEEK}.Hafta klasörü ve index.html eklendi." >> update.log

git add "$WEEK_DIR" README.md update.log
git commit -m "$(date '+%Y-%m-%d') - ${WEEK}.Hafta: klasör, index.html, README.md ve update.log güncellendi"
git pull --rebase

# Use safe_push function if available, fallback to direct push
if command -v safe_push >/dev/null 2>&1; then
  safe_push main || {
    echo -e "\e[33m⚠️  Failed to push to main, trying current branch...\e[0m"
    current_branch=$(git branch --show-current)
    safe_push "$current_branch"
  }
else
  git push origin main
fi

echo -e "\e[32m✅ ${WEEK}.Hafta klasörü başarıyla oluşturuldu ve GitHub'a gönderildi!\e[0m"