document.addEventListener("DOMContentLoaded", async function () {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get("id");
    if (!id) {
        alert("Geçersiz öğrenci!");
        window.location.href = "students.html";
        return;
    }
    const form = document.getElementById("updateForm");
    // Öğrenci bilgilerini doldur
    const response = await fetch(`https://localhost:5001/api/students/${id}`);
    if (!response.ok) {
        alert("Öğrenci bulunamadı!");
        window.location.href = "students.html";
        return;
    }
    const student = await response.json();
    form.id.value = student.id;
    form.firstName.value = student.firstName;
    form.lastName.value = student.lastName;
    form.studentNumber.value = student.studentNumber;
    form.birthDate.value = student.birthDate.substring(0,10);
    form.class.value = student.class;

    form.addEventListener("submit", async function (event) {
        event.preventDefault();
        if (!validateForm()) return;
        const updatedStudent = {
            id: parseInt(form.id.value),
            firstName: form.firstName.value.trim(),
            lastName: form.lastName.value.trim(),
            studentNumber: parseInt(form.studentNumber.value),
            birthDate: form.birthDate.value,
            class: form.class.value.trim()
        };
        // Benzersizlik kontrolü
        const exists = await checkStudentNumber(updatedStudent.studentNumber, updatedStudent.id);
        if (exists) {
            alert("Bu öğrenci numarası zaten mevcut!");
            return;
        }
        const response = await fetch(`https://localhost:5001/api/students/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updatedStudent)
        });
        if (response.ok) {
            alert("Öğrenci başarıyla güncellendi!");
            window.location.href = "students.html";
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
    const classValue = document.getElementById("class").value.trim();
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

async function checkStudentNumber(studentNumber, excludeId) {
    const response = await fetch("https://localhost:5001/api/students");
    if (!response.ok) return false;
    const students = await response.json();
    return students.some(s => s.studentNumber === parseInt(studentNumber) && s.id !== parseInt(excludeId));
}
