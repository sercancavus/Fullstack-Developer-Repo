#!/bin/bash

cd /c/Users/Monster/Fullstack-Developer-Repo || { echo "❌ Klasör bulunamadı"; exit 1; }

if git status BE128 | grep -q "modified\|new file\|deleted"; then
  COMMIT_DATE=$(date '+%Y-%m-%d %H:%M')

  # 1) Tüm hafta dosyalarını sahnele
  find BE128 -type f \( -name "*.html" -o -name "*.css" -o -name "*.js" \
    -o -name "*.md" -o -name "*.sh" \) -exec git add {} \;

  # 2) BE128 README ve log
  echo "Son güncelleme: $COMMIT_DATE" > BE128/README.md
  echo "$COMMIT_DATE - BE128 klasörü güncellendi" >> update.log
  git add BE128/README.md update.log

  # 3) BE128/index.html → Haftalık liste
  cat <<EOF > BE128/index.html
<!DOCTYPE html>
<html lang="tr">
<head>
  <meta charset="UTF-8">
  <title>BE128 Haftalık Günlükler</title>
  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
  <style>
    body { font-family:sans-serif; background:#e6f7f4; text-align:center; padding:2rem }
    h1 { color:#2ac4a5; margin-bottom:1rem }
    ul { list-style:none; padding:0; max-width:400px; margin:auto }
    li { margin:0.8rem 0 }
    a { display:block; padding:0.6rem; border-radius:8px; background:#fff;
        box-shadow:0 2px 6px rgba(0,0,0,0.05); text-decoration:none;
        color:#2ac4a5; font-weight:bold; font-size:1.2rem;
        transition:background 0.2s, transform 0.2s; }
    a:hover { background:#d2f2ec; transform:scale(1.02) }
    footer { margin-top:2rem; color:#666; font-size:0.9rem }
  </style>
</head>
<body>
  <h1>📘 BE128 Haftalık Günlükler</h1>
  <ul>
EOF

  find BE128 -mindepth 1 -maxdepth 1 -type d -name "*Hafta*" | sort | head -n 4 \
    | while read -r weekdir; do
      WEEK=$(basename "$weekdir")
      echo "    <li><a href=\"./$WEEK/index.html\">$WEEK</a></li>" >> BE128/index.html
    done

  cat <<EOF >> BE128/index.html
  </ul>
  <footer>Oluşturuldu: $COMMIT_DATE</footer>
</body>
</html>
EOF

  git add BE128/index.html

  # 4) Kök index.html → Aynı listeyi root’a yaz
  cat <<EOF > index.html
<!DOCTYPE html>
<html lang="tr">
<head>
  <meta charset="UTF-8">
  <title>BE128 Haftalık Günlükler</title>
  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
  <style>
    body { font-family:sans-serif; background:#e6f7f4; text-align:center; padding:2rem }
    h1 { color:#2ac4a5; margin-bottom:1rem }
    ul { list-style:none; padding:0; max-width:400px; margin:auto }
    li { margin:0.8rem 0 }
    a { display:block; padding:0.6rem; border-radius:8px; background:#fff;
        box-shadow:0 2px 6px rgba(0,0,0,0.05); text-decoration:none;
        color:#2ac4a5; font-weight:bold; font-size:1.2rem;
        transition:background 0.2s, transform 0.2s; }
    a:hover { background:#d2f2ec; transform:scale(1.02) }
    footer { margin-top:2rem; color:#666; font-size:0.9rem }
  </style>
</head>
<body>
  <h1>📘 BE128 Haftalık Günlükler</h1>
  <ul>
EOF

  find BE128 -mindepth 1 -maxdepth 1 -type d -name "*Hafta*" | sort | head -n 4 \
    | while read -r weekdir; do
      WEEK=$(basename "$weekdir")
      echo "    <li><a href=\"./BE128/$WEEK/index.html\">$WEEK</a></li>" >> index.html
    done

  cat <<EOF >> index.html
  </ul>
  <footer>Oluşturuldu: $COMMIT_DATE</footer>
</body>
</html>
EOF

  git add index.html

  # 5) Her hafta için index.html (Dosya listesi + Commit Geçmişi)
  find BE128 -mindepth 1 -maxdepth 1 -type d -name "*Hafta*" | sort | head -n 4 \
    | while read -r weekdir; do
      WEEK=$(basename "$weekdir")
      P="BE128/$WEEK"

      cat <<EOF > "$P/index.html"
<!DOCTYPE html>
<html lang="tr">
<head>
  <meta charset="UTF-8">
  <title>$WEEK Günlükleri</title>
  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
  <style>
    body { font-family:sans-serif; background:#e6f7f4; padding:2rem }
    h1 { color:#2ac4a5 }
    table { width:100%; border-collapse:collapse; margin-bottom:2rem; background:#fff }
    th,td { padding:0.6rem; border-bottom:1px solid #ddd; text-align:left }
    th { background:#2ac4a5; color:#fff }
    .log-table { width:100%; border-collapse:collapse }
    .log-table th, .log-table td { padding:0.4rem; border:1px solid #ddd; font-size:0.9rem }
    .log-table th { background:#1a9e86; color:#fff }
    footer { margin-top:2rem; color:#666; font-size:0.8rem }
  </style>
</head>
<body>
  <h1>📒 $WEEK Günlükleri</h1>
  <h2>Dosya Listesi</h2>
  <table>
    <thead>
      <tr><th>Dosya</th><th>Tür</th><th>Boyut</th><th>Son Değişiklik</th></tr>
    </thead>
    <tbody>
EOF

      REPO_URL="https://github.com/sercancavus/Fullstack-Developer-Repo/blob/main/BE128/$WEEK"
      find "$P" -type f | while read -r f; do
        FN=$(basename "$f")
        [[ "$FN" != *.* ]] && continue
        EXT=${FN##*.}
        SIZE=$(du -k "$f" | cut -f1)
        MOD=$(date -r "$f" '+%d.%m.%Y %H:%M')
        ICON="📄"
        case "$EXT" in
          js) ICON="🟨";;
          html) ICON="🟥";;
          css) ICON="🟦";;
          md) ICON="📘";;
          sh) ICON="🟩";;
        esac
        echo "      <tr><td>$ICON <a href=\"$REPO_URL/$FN\" target=\"_blank\">$FN</a></td><td>${EXT^^}</td><td>${SIZE} KB</td><td>$MOD</td></tr>" \
          >> "$P/index.html"
      done

      cat <<EOF >> "$P/index.html"
    </tbody>
  </table>
  <h2>Commit Geçmişi</h2>
  <table class="log-table">
    <thead><tr><th>Hash</th><th>Tarih</th><th>Mesaj</th><th>Dosyalar</th></tr></thead>
    <tbody>
EOF

      git -C . log --pretty=format:'<tr><td><a href="https://github.com/sercancavus/Fullstack-Developer-Repo/commit/%H" target="_blank">%h</a></td><td>%ad</td><td>%s</td><td>' \
        --date=format:'%d.%m.%Y %H:%M' --name-only -- "$P" | \
      awk 'BEGIN{ORS="";} 
          /^[0-9a-f]{7}/{ printf "</td></tr>\n"%s, $0 }
          /^[A-Za-z0-9._-]+=*/{ printf $0"," }
          END{ print "</td></tr>\n" }' \
        >> "$P/index.html"

      cat <<EOF >> "$P/index.html"
    </tbody>
  </table>
  <footer>Oluşturuldu: $COMMIT_DATE</footer>
</body>
</html>
EOF

      git add "$P/index.html"
    done

  # 6) Commit & Push
  git commit -m "BE128 ilk 4 hafta güncellemesi - $COMMIT_DATE"
  git push origin main

  echo -e "\a\n✅ Başarıyla tamamlandı!"
else
  echo "🟢 BE128’de değişiklik yok."
fi