const images = [
  {
    id: "modal1",
    src: "https://images.pexels.com/photos/27182510/pexels-photo-27182510.jpeg",
    alt: "Doða Manzarasý",
    category: "Doða"
  },
  {
    id: "modal2",
    src: "https://images.pexels.com/photos/5044212/pexels-photo-5044212.jpeg",
    alt: "Sanatsal Görsel",
    category: "Sanat"
  },
  {
    id: "modal3",
    src: "https://images.pexels.com/photos/31995895/pexels-photo-31995895.jpeg",
    alt: "Þehir Manzarasý",
    category: "Þehir"
  }
];

const galleryRow = document.querySelector("#gallery-row");
const modalContainer = document.querySelector("#modal-container");

images.forEach(img => {
  // Galeriye resim ekle
  galleryRow.innerHTML += `
    <div class="col-md-4">
      <img src="${img.src}" class="gallery-img" data-bs-toggle="modal" data-bs-target="#${img.id}" alt="${img.alt}">
    </div>
  `;

  // Modal oluþtur
  modalContainer.innerHTML += `
    <div class="modal fade" id="${img.id}" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content">
          <img src="${img.src}" class="img-fluid" alt="${img.alt}">
        </div>
      </div>
    </div>
  `;
});