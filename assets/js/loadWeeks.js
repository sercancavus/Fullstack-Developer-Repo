// 🌐 Sayfa yüklendiğinde varsayılan dil ayarla ve haftaları yükle
window.onload = () => {
    changeLang('tr');
    loadWeeks();
};

// 🈳 Dil geçişini sağlar
function changeLang(lang) {
    document.querySelectorAll('[data-lang]').forEach(el => {
        el.classList.toggle('d-none', el.getAttribute('data-lang') !== lang);
    });
}

// 🏷️ Status rozetini koşullu renkle hazırlar
function getStatusBadge(status) {
    const map = {
        "Tamamlandı": "bg-success",
        "Devam ediyor": "bg-warning text-dark",
        "Eksik": "bg-danger",
        "Planlanıyor": "bg-secondary"
    };
    const color = map[status] || "bg-light text-dark";
    return `<span class="badge ${color} me-1">${status}</span>`;
}

// 🌍 Dil rozetini oluşturur (TR / EN)
function getLangBadge(lang) {
    const map = {
        "tr": "bg-primary",
        "en": "bg-dark"
    };
    const label = lang === "tr" ? "TR" : "EN";
    const color = map[lang] || "bg-light text-dark";
    return `<span class="badge ${color} me-1">${label}</span>`;
}

// 📦 Haftaları weeks.json'dan yükler
function loadWeeks() {
    fetch('weeks.json')
        .then(response => {
            if (!response.ok) throw new Error("weeks.json dosyası bulunamadı");
            return response.json();
        })
        .then(weeks => {
            const row = document.getElementById('week-cards');
            row.innerHTML = "";

            if (weeks.length === 0) {
                row.innerHTML = "<p class='text-warning'>Henüz eklenmiş hafta bulunmamaktadır.</p>";
                return;
            }

            weeks.forEach(weekObj => {
                row.innerHTML += `
          <div class="col-md-4">
            <div class="card week-card h-100">
              <div class="card-body">
                <h5 class="card-title">${weekObj.week}. Hafta</h5>
                <p>
                  ${getStatusBadge(weekObj.status)}
                  <span class="badge bg-info text-dark me-1">${weekObj.topic}</span>
                  <span class="badge bg-secondary me-1">${weekObj.date}</span>
                  ${getLangBadge(weekObj.lang)}
                </p>
                <a href="${weekObj.path}" class="btn btn-primary btn-view mt-2">Görüntüle</a>
              </div>
            </div>
          </div>
        `;
            });
        })
        .catch(error => {
            console.error("Haftalar yüklenemedi:", error);
            document.getElementById('week-cards').innerHTML = "<p class='text-danger'>Haftalar yüklenemedi veya weeks.json bozuk.</p>";
        });
}