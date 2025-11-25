document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    form.addEventListener("submit", async function (event) {
        event.preventDefault();
        if (!validateForm()) return;
        const student = {
            firstName: form.firstName.value.trim(),
            lastName: form.lastName.value.trim(),
            studentNumber: parseInt(form.studentNumber.value),
            birthDate: form.birthDate.value,
            class: form.class.value.trim()
        };
        // Benzersizlik kontrolü
        const exists = await checkStudentNumber(student.studentNumber);
        if (exists) {
            alert("Bu öğrenci numarası zaten mevcut!");
            return;
        }
        // API'ye gönder
        const response = await fetch("https://localhost:5001/api/students", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(student)
        });
        if (response.ok) {
            alert("Öğrenci başarıyla eklendi!");
            form.reset();
        } else {
            const error = await response.json();
            alert(error.error || "Bir hata oluştu.");
        }
    });
});

function validateForm() {
    const firstName = document.getElementById("firstName").value.trim();
    const lastName = document.getElementById("lastName").value.trim();
    const studentNumber = document.getElementById("studentNumber").value;
    const birthDate = document.getElementById("birthDate").value;
    const classValue = document.getElementById("class")?.value.trim() || "";
    if (!firstName || !lastName || !studentNumber || !birthDate || !classValue) {
        alert("Tüm alanları doldurunuz.");
        return false;
    }
    if (firstName.length > 50 || lastName.length > 50) {
        alert("Ad ve Soyad en fazla 50 karakter olmalı.");
        return false;
    }
    if (classValue.length > 20) {
        alert("Sınıf en fazla 20 karakter olmalı.");
        return false;
    }
    if (parseInt(studentNumber) < 1) {
        alert("Öğrenci numarası 1'den büyük olmalı.");
        return false;
    }
    return true;
}

async function checkStudentNumber(studentNumber) {
    const response = await fetch("https://localhost:5001/api/students");
    if (!response.ok) return false;
    const students = await response.json();
    return students.some(s => s.studentNumber === parseInt(studentNumber));
}
