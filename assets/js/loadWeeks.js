let currentLang = "tr";

function changeLang(lang) {
    currentLang = lang;
    loadWeeks();
}

function getStatusBadge(status, status_en) {
    const map = {
        "Tamamlandı": "bg-success",
        "Devam ediyor": "bg-warning text-dark",
        "Eksik": "bg-danger",
        "Planlanıyor": "bg-secondary"
    };

    const map_en = {
        "Completed": "bg-success",
        "In progress": "bg-warning text-dark",
        "Missing": "bg-danger",
        "Planned": "bg-secondary"
    };

    const label = currentLang === "en" ? status_en : status;
    const color = currentLang === "en" ? map_en[status_en] : map[status];

    return `<span class="badge ${color} me-2">${label}</span>`;
}

function loadWeeks() {
    fetch("../weeks.json")
        .then(res => res.json())
        .then(data => {
            const container = document.getElementById("week-cards");
            container.innerHTML = "";

            data.forEach(week => {
                const label = `Hafta ${week.week} · ${week.topic}`;
                const date = week.date;
                const desc = currentLang === "en" ? week.description_en : week.description_tr;

                const card = document.createElement("div");
                card.className = "col-md-6";

                card.innerHTML = `
          <div class="week-card p-3">
            <h4>${label}</h4>
            ${getStatusBadge(week.status, week.status_en)}
            <div class="description">${desc}</div>
            <a href="../${week.path}" class="btn btn-view btn-sm mt-3" target="_blank">Git</a>
          </div>
        `;

                container.appendChild(card);
            });
        })
        .catch(err => {
            document.getElementById("week-cards").innerHTML = `<div class="text-danger">❌ weeks.json yüklenemedi</div>`;
        });
}

loadWeeks();