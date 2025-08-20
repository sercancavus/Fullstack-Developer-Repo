#!/bin/bash
set -euo pipefail

# cd /c/Users/Monster/Fullstack-Developer-Repo || { echo -e "\e[31m❌ Klasör bulunamadı\e[0m"; exit 1; }
# Yukarıdaki satırı kaldırdık, script bulunduğu dizinde çalışacak

# Load git helper functions for safe branch operations
source "$(dirname "$0")/git-helper.sh" 2>/dev/null || {
    echo -e "\e[33m⚠️  git-helper.sh not found, using basic git operations\e[0m"
}

if git status BE128 | grep -q "modified\|new file\|deleted"; then
  COMMIT_DATE=$(date '+%Y-%m-%d %H:%M')
  README="BE128/README.md"
  LOG="update.log"
  ROOT_INDEX="index.html"
  BE128_INDEX="BE128/index.html"
  MAIN_README="README.md"

  echo -e "\e[36m🔄 Değişiklikler algılandı, işlem başlıyor...\e[0m"

  # 1️⃣ Dosyaları sahnele (find yerine globstar)
  shopt -s globstar nullglob
  for FILE in BE128/**/*.{html,css,js,md,sh}; do
    [ -f "$FILE" ] || continue
    git add "$FILE"
  done
  shopt -u globstar nullglob

  # 2️⃣ BE128 README ve log
  echo "Son güncelleme: $COMMIT_DATE" > "$README"
  echo "$COMMIT_DATE - BE128 klasörü güncellendi" >> "$LOG"
  git add "$README" "$LOG"

  # 3️⃣ BE128 index.html
  echo "<!DOCTYPE html><html lang=\"tr\"><head><meta charset=\"UTF-8\"><title>BE128 Günlükler</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/><style>body{font-family:sans-serif;background:#e6f7f4;text-align:center;padding:2rem}h1{color:#2ac4a5}ul{list-style:none;padding:0;max-width:400px;margin:auto}li{margin:0.8rem 0}a{display:block;padding:0.6rem;border-radius:8px;background:#fff;box-shadow:0 2px 6px rgba(0,0,0,0.05);text-decoration:none;color:#2ac4a5;font-weight:bold;font-size:1.2rem;transition:0.2s}a:hover{background:#d2f2ec;transform:scale(1.02)}footer{margin-top:2rem;color:#666;font-size:0.9rem}</style></head><body><h1>📘 BE128 Haftalık Günlükler</h1><ul>" > "$BE128_INDEX"

  if [ ! -f weeks.json ]; then
    echo -e "\e[31m❌ weeks.json bulunamadı!\e[0m"
    exit 1
  fi

  jq -r '.[] | .path' weeks.json | cut -d'/' -f2 | while read -r WEEKDIR; do
    echo "<li><a href=\"./$WEEKDIR/index.html\">$WEEKDIR</a></li>" >> "$BE128_INDEX"
  done

  echo "</ul><footer>Oluşturuldu: $COMMIT_DATE</footer></body></html>" >> "$BE128_INDEX"
  git add "$BE128_INDEX"

  # 4️⃣ Root index.html
  echo "<!DOCTYPE html><html lang=\"tr\"><head><meta charset=\"UTF-8\"><title>BE128 Günlükler</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/><style>body{font-family:sans-serif;background:#e6f7f4;text-align:center;padding:2rem}h1{color:#2ac4a5}ul{list-style:none;padding:0;max-width:400px;margin:auto}li{margin:0.8rem 0}a{display:block;padding:0.6rem;border-radius:8px;background:#fff;box-shadow:0 2px 6px rgba(0,0,0,0.05);text-decoration:none;color:#2ac4a5;font-weight:bold;font-size:1.2rem;transition:0.2s}a:hover{background:#d2f2ec;transform:scale(1.02)}footer{margin-top:2rem;color:#666;font-size:0.9rem}</style></head><body><h1>📘 BE128 Günlükler</h1><ul>" > "$ROOT_INDEX"

  jq -r '.[] | .path' weeks.json | cut -d'/' -f2 | while read -r WEEKDIR; do
    echo "<li><a href=\"./BE128/$WEEKDIR/index.html\">$WEEKDIR</a></li>" >> "$ROOT_INDEX"
  done

  echo "</ul><footer>Oluşturuldu: $COMMIT_DATE</footer></body></html>" >> "$ROOT_INDEX"
  git add "$ROOT_INDEX"

  # 5️⃣ Hafta bazlı index.html (tamamen bash ile)
  jq -c '.[]' weeks.json | while read -r weekObj; do
    WEEK=$(echo "$weekObj" | jq -r '.week')
    PATH=$(echo "$weekObj" | jq -r '.path')
    DIR="${PATH%/*}"
    INDEX_FILE="$DIR/index.html"
    REPO_URL="https://github.com/sercancavus/Fullstack-Developer-Repo/blob/main/$DIR"

    echo "<!DOCTYPE html><html lang=\"tr\"><head><meta charset=\"UTF-8\"><title>Hafta $WEEK</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/><style>body{font-family:sans-serif;background:#e6f7f4;padding:2rem}h1{color:#2ac4a5}table{width:100%;border-collapse:collapse;margin-bottom:2rem;background:#fff}th,td{padding:0.6rem;border-bottom:1px solid #ddd;text-align:left}th{background:#2ac4a5;color:#fff}.log-table{width:100%;border-collapse:collapse}.log-table th,.log-table td{padding:0.4rem;border:1px solid #ddd;font-size:0.9rem}.log-table th{background:#1a9e86;color:#fff}footer{margin-top:2rem;color:#666;font-size:0.8rem}</style></head><body><h1>📒 Hafta $WEEK</h1><h2>Dosya Listesi</h2><table><thead><tr><th>Dosya</th><th>Tür</th><th>Boyut</th><th>Son Değişiklik</th></tr></thead><tbody>" > "$INDEX_FILE"

    shopt -s globstar nullglob
    for FILE in "$DIR"/**/*; do
      [ -f "$FILE" ] || continue
      FILENAME="${FILE##*/}"
      EXT="${FILENAME##*.}"
      # Dosya boyutunu KB cinsinden hesapla (tamamen bash, wc/stat yoksa okuma ile)
      SIZE=""
      if [ -e "$FILE" ]; then
        SIZE=$(stat -c %s "$FILE" 2>/dev/null || stat -f %z "$FILE" 2>/dev/null)
        if [ -z "$SIZE" ]; then
          exec 3<"$FILE"
          SIZE=0
          while IFS= read -r -n1 _ <&3; do
            SIZE=$((SIZE + 1))
          done
          exec 3<&-
        fi
        SIZE_KB=$(( (SIZE + 1023) / 1024 ))
      else
        SIZE_KB=0
      fi
      MOD=$(date -r "$FILE" '+%d.%m.%Y %H:%M')
      ICON="📄"; [ "$EXT" = "html" ] && ICON="🟥"
      [ "$EXT" = "js" ] && ICON="🟨"
      [ "$EXT" = "css" ] && ICON="🟦"
      [ "$EXT" = "md" ] && ICON="📘"
      [ "$EXT" = "sh" ] && ICON="🟩"

      echo "<tr><td>$ICON <a href=\"$REPO_URL/$FILENAME\" target=\"_blank\">$FILENAME</a></td><td>${EXT^^}</td><td>${SIZE_KB} KB</td><td>$MOD</td></tr>" >> "$INDEX_FILE"
    done
    shopt -u globstar nullglob

    echo "</tbody></table><h2>Commit Geçmişi</h2><table class=\"log-table\"><thead><tr><th>Hash</th><th>Tarih</th><th>Mesaj</th><th>Dosyalar</th></tr></thead><tbody>" >> "$INDEX_FILE"

    git log --pretty=format:'<tr><td><a href="https://github.com/sercancavus/Fullstack-Developer-Repo/commit/%H" target="_blank">%h</a></td><td>%ad</td><td>%s</td><td>' --date=format:'%d.%m.%Y %H:%M' --name-only -- "$DIR" | awk 'BEGIN{ORS=""} /^[0-9a-f]{7}/{print "</td></tr>"$0} /^[A-Za-z0-9._-]+/{print $0","} END{print "</td></tr>"}' >> "$INDEX_FILE"

    echo "</tbody></table><footer>Oluşturuldu: $COMMIT_DATE</footer></body></html>" >> "$INDEX_FILE"
    git add "$INDEX_FILE"
  done

  # 6️⃣ Ana README.md güncellemesi (uzaktan eğitim içeriği ile)
  REMOTE_URL="https://siliconmadeacademy.com/akademi/BE/BE128.html"
  TEMP_HTML="/tmp/be128_tmp.html"
  # İçeriği indir
  if command -v curl >/dev/null 2>&1; then
    curl -s "$REMOTE_URL" -o "$TEMP_HTML"
  elif command -v wget >/dev/null 2>&1; then
    wget -q "$REMOTE_URL" -O "$TEMP_HTML"
  else
    echo "curl veya wget bulunamadı, README.md güncellenemedi." >&2
    exit 1
  fi

  # Başlangıç ve bitiş tarihini çek (örnek: <b>Başlangıç Tarihi:</b> 1 Ocak 2024 ... <b>Bitiş Tarihi:</b> 1 Haziran 2024)
  BASLANGIC=$(grep -oP 'Başlangıç Tarihi:</b>\s*\K[^<]+' "$TEMP_HTML" | head -1)
  BITIS=$(grep -oP 'Bitiş Tarihi:</b>\s*\K[^<]+' "$TEMP_HTML" | head -1)

  # Hafta hafta içerik (örnek: <tr><td>1. Hafta</td><td>HTML ve CSS Temelleri</td></tr>)
  HAFTA_LIST=$(grep -oP '<tr>\s*<td>\K[0-9]+\. Hafta</td><td>[^<]+' "$TEMP_HTML" | sed 's#</td><td># - #g')

  echo "# BE128 Full Stack Programı" > "$MAIN_README"
  echo >> "$MAIN_README"
  if [ -n "$BASLANGIC" ] && [ -n "$BITIS" ]; then
    echo "**Başlangıç Tarihi:** $BASLANGIC  \n**Bitiş Tarihi:** $BITIS" >> "$MAIN_README"
    echo >> "$MAIN_README"
  fi
  echo "## Program Haftalık İçerik" >> "$MAIN_README"
  echo >> "$MAIN_README"
  if [ -n "$HAFTA_LIST" ]; then
    echo "$HAFTA_LIST" | while read -r line; do
      echo "- $line" >> "$MAIN_README"
    done
    echo >> "$MAIN_README"
  fi
  echo "---" >> "$MAIN_README"
  echo "Güncel haftalar listesi:" >> "$MAIN_README"
  jq -c '.[]' weeks.json | while read -r week; do
    WEEK=$(echo "$week" | jq -r '.week')
    PATH=$(echo "$week" | jq -r '.path')
    STATUS=$(echo "$week" | jq -r '.status')
    TOPIC=$(echo "$week" | jq -r '.topic')
    DATE=$(echo "$week" | jq -r '.date')
    case "$STATUS" in
      "Tamamlandı") EMOJI="✅";;
      "Devam ediyor") EMOJI="🔄";;
      "Eksik") EMOJI="❌";;
      "Planlanıyor") EMOJI="📝";;
      *) EMOJI="📄";;
    esac
    echo "- [Hafta $WEEK]($PATH) $EMOJI $TOPIC · $DATE" >> "$MAIN_README"
  done

  git add "$MAIN_README"

  # 7️⃣ Commit & Push
  git commit -m "📦 BE128 Güncellemesi - $COMMIT_DATE"

  # Use safe_push function if available, fallback to direct push
  if command -v safe_push >/dev/null 2>&1; then
    safe_push main || {
      echo -e "\e[33m⚠️  Failed to push to main, trying current branch...\e[0m"
      current_branch=$(git branch --show-current)
      safe_push "$current_branch"
    }
  else
    if git push origin main; then
      echo -e "\e[32m✅ Git push başarılı!\e[0m"
    else
      echo -e "\e[31m❌ Git push başarısız. Lütfen remote çatışmalarını kontrol et.\e[0m"
    fi
  fi

else
  echo -e "\e[32m🟢 BE128’de değişiklik yok.\e[0m"
fi

git add .
git commit -m "README ve otomasyon script güncellendi"

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