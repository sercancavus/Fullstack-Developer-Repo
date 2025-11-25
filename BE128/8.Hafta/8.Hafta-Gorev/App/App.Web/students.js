document.addEventListener("DOMContentLoaded", function () {
    loadStudents();
});

async function loadStudents() {
    const response = await fetch("https://localhost:5001/api/students");
    if (!response.ok) return;
    const students = await response.json();
    const tbody = document.querySelector("#studentsTable tbody");
    tbody.innerHTML = "";
    students.forEach(student => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${student.id}</td>
            <td>${student.firstName}</td>
            <td>${student.lastName}</td>
            <td>${student.studentNumber}</td>
            <td>${student.birthDate ? student.birthDate.substring(0,10) : ""}</td>
            <td>${student.class}</td>
            <td>
                <button onclick="showUpdateForm(${student.id})">Güncelle</button>
                <button onclick="deleteStudent(${student.id})">Sil</button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

async function deleteStudent(id) {
    if (!confirm("Bu öðrenciyi silmek istediðinize emin misiniz?")) return;
    const response = await fetch(`https://localhost:5001/api/students/${id}`, { method: "DELETE" });
    if (response.ok) {
        alert("Öðrenci silindi.");
        loadStudents();
    } else {
        alert("Silme iþlemi baþarýsýz.");
    }
}

function showUpdateForm(id) {
    window.location.href = `update.html?id=${id}`;
}
