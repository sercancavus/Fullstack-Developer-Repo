#!/bin/bash

cd /c/Users/Monster/Fullstack-Developer-Repo || { echo "❌ Klasör bulunamadı"; exit 1; }

if git status BE128 | grep -q "modified\|new file\|deleted"; then
  COMMIT_DATE=$(date '+%Y-%m-%d %H:%M')
  README="BE128/README.md"
  LOG="update.log"
  ROOT_INDEX="index.html"
  BE128_INDEX="BE128/index.html"
  MAIN_README="README.md"

  echo "🔄 Değişiklikler algılandı, işlem başlıyor..."

  # 1️⃣ Dosyaları sahnele
  find BE128 -type f \( -name "*.html" -o -name "*.css" -o -name "*.js" -o -name "*.md" -o -name "*.sh" \) -exec git add {} \;

  # 2️⃣ BE128 README ve log
  echo "Son güncelleme: $COMMIT_DATE" > "$README"
  echo "$COMMIT_DATE - BE128 klasörü güncellendi" >> "$LOG"
  git add "$README" "$LOG"

  # 3️⃣ BE128 index.html
  echo "<!DOCTYPE html><html lang=\"tr\"><head><meta charset=\"UTF-8\"><title>BE128 Günlükler</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/><style>body{font-family:sans-serif;background:#e6f7f4;text-align:center;padding:2rem}h1{color:#2ac4a5}ul{list-style:none;padding:0;max-width:400px;margin:auto}li{margin:0.8rem 0}a{display:block;padding:0.6rem;border-radius:8px;background:#fff;box-shadow:0 2px 6px rgba(0,0,0,0.05);text-decoration:none;color:#2ac4a5;font-weight:bold;font-size:1.2rem;transition:0.2s}a:hover{background:#d2f2ec;transform:scale(1.02)}footer{margin-top:2rem;color:#666;font-size:0.9rem}</style></head><body><h1>📘 BE128 Haftalık Günlükler</h1><ul>" > "$BE128_INDEX"

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

  # 5️⃣ Hafta bazlı index.html
  jq -c '.[]' weeks.json | while read -r weekObj; do
    WEEK=$(echo "$weekObj" | jq -r '.week')
    PATH=$(echo "$weekObj" | jq -r '.path')
    DIR=$(dirname "$PATH")
    INDEX_FILE="$DIR/index.html"
    REPO_URL="https://github.com/sercancavus/Fullstack-Developer-Repo/blob/main/$DIR"

    echo "<!DOCTYPE html><html lang=\"tr\"><head><meta charset=\"UTF-8\"><title>Hafta $WEEK</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/><style>body{font-family:sans-serif;background:#e6f7f4;padding:2rem}h1{color:#2ac4a5}table{width:100%;border-collapse:collapse;margin-bottom:2rem;background:#fff}th,td{padding:0.6rem;border-bottom:1px solid #ddd;text-align:left}th{background:#2ac4a5;color:#fff}.log-table{width:100%;border-collapse:collapse}.log-table th,.log-table td{padding:0.4rem;border:1px solid #ddd;font-size:0.9rem}.log-table th{background:#1a9e86;color:#fff}footer{margin-top:2rem;color:#666;font-size:0.8rem}</style></head><body><h1>📒 Hafta $WEEK</h1><h2>Dosya Listesi</h2><table><thead><tr><th>Dosya</th><th>Tür</th><th>Boyut</th><th>Son Değişiklik</th></tr></thead><tbody>" > "$INDEX_FILE"

    find "$DIR" -type f ! -path "*/.git/*" | while read -r FILE; do
      FN=$(basename "$FILE")
      EXT=${FN##*.}
      SIZE=$(du -k "$FILE" | cut -f1)
      MOD=$(date -r "$FILE" '+%d.%m.%Y %H:%M')
      ICON="📄"; [ "$EXT" = "html" ] && ICON="🟥"
      [ "$EXT" = "js" ] && ICON="🟨"
      [ "$EXT" = "css" ] && ICON="🟦"
      [ "$EXT" = "md" ] && ICON="📘"
      [ "$EXT" = "sh" ] && ICON="🟩"

      echo "<tr><td>$ICON <a href=\"$REPO_URL/$FN\" target=\"_blank\">$FN</a></td><td>${EXT^^}</td><td>${SIZE} KB</td><td>$MOD</td></tr>" >> "$INDEX_FILE"
    done

    echo "</tbody></table><h2>Commit Geçmişi</h2><table class=\"log-table\"><thead><tr><th>Hash</th><th>Tarih</th><th>Mesaj</th><th>Dosyalar</th></tr></thead><tbody>" >> "$INDEX_FILE"

    git log --pretty=format:'<tr><td><a href="https://github.com/sercancavus/Fullstack-Developer-Repo/commit/%H" target="_blank">%h</a></td><td>%ad</td><td>%s</td><td>' --date=format:'%d.%m.%Y %H:%M' --name-only -- "$DIR" | awk 'BEGIN{ORS=""} /^[0-9a-f]{7}/{print "</td></tr>"$0} /^[A-Za-z0-9._-]+/{print $0","} END{print "</td></tr>"}' >> "$INDEX_FILE"

    echo "</tbody></table><footer>Oluşturuldu: $COMMIT_DATE</footer></body></html>" >> "$INDEX_FILE"
    git add "$INDEX_FILE"
  done

  # 6️⃣ Ana README.md güncellemesi
  echo "# Full Stack Öğrenme Yolculuğu 🚀" > "$MAIN_README"
  echo "" >> "$MAIN_README"
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

  if git push origin main; then
    echo "✅ Git push başarılı!"
  else
    echo "❌ Git push başarısız. Lütfen remote çatışmalarını kontrol et."
  fi

else
  echo "🟢 BE128’de değişiklik yok."
fi