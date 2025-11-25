function toggleCart() {
    const cartSidebar = document.getElementById('cart-sidebar');
    const cartOverlay = document.getElementById('cart-overlay');
    cartSidebar.classList.toggle('active');
    cartOverlay.classList.toggle('active');
}

//sepete ürün eklemek sepette listelemek için kullandığımız script
function addToCart(productId, productName) {
    fetch(`/Home/GetProductPrice?productId=${productId}`)
        .then(response => response.json())
        .then(data => {
            const productPrice = parseFloat(data.price);
            const cart = getCart();
            const existingProduct = cart.find(p => p.productId === productId);

            if (existingProduct) {
                existingProduct.quantity++;
                existingProduct.totalPrice = (parseFloat(existingProduct.totalPrice) + productPrice).toFixed(2);
            } else {
                cart.push({
                    productId,
                    name: productName,
                    price: productPrice.toFixed(2),
                    quantity: 1,
                    totalPrice: productPrice.toFixed(2)
                });
            }

            setCart(cart);
            updateCartCount();
            renderCart();
        })
        .catch(error => {
            console.error('Error fetching product price:', error);
        });
}

function removeFromCart(productId) {
    const cart = getCart();
    const productIndex = cart.findIndex(p => p.productId === productId);

    if (productIndex > -1) {
        const product = cart[productIndex];
        const productPrice = parseFloat(product.price);

        if (product.quantity > 1) {
            product.quantity--;
            product.totalPrice = (parseFloat(product.totalPrice) - productPrice).toFixed(2);
        } else {
            cart.splice(productIndex, 1);
        }
    }

    setCart(cart);
    updateCartCount();
    renderCart();
}

function increaseQuantity(productId) {
    const cart = getCart();
    const product = cart.find(p => p.productId === productId);

    if (product) {
        const productPrice = parseFloat(product.price);
        product.quantity++;
        product.totalPrice = (parseFloat(product.totalPrice) + productPrice).toFixed(2);
        setCart(cart);
        renderCart();
    }
}

async function getData() {
    const response = await fetch('/userinfo');
    const data = await response.json(); // Veriyi JSON formatına çeviriyor
    return data; // JSON verisini döndürüyor
}

async function saveCart() {
    const cart = getCart();

    var userInfo = await getData();

    if (!userInfo || !userInfo.id) {
        console.error("User ID not found");
        return;
    }

    fetch(`SaveOrder/${userInfo.id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(cart)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(error => {
                    console.error('Error details:', error.details);
                    throw new Error(error.message);
                });
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                alert('Order saved successfully');
                toggleCart();
                updateCartCount();
                localStorage.removeItem('cart'); // localStorage'dan sepeti temizle
                renderCart(); // sepeti yeniden çiz
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error saving order: ' + error.message);
        });
}

function getCart() {
    return JSON.parse(localStorage.getItem('cart')) || [];
}

function setCart(cart) {
    localStorage.setItem('cart', JSON.stringify(cart));
}

function updateCartCount() {
    const cart = getCart();
    const cartCount = cart.reduce((total, product) => total + product.quantity, 0);
    document.getElementById('cart-count').innerText = cartCount;
}

function renderCart() {
    const cartItemsElement = document.getElementById('cart-items');
    const cartCountElement = document.getElementById('cart-count');
    const cartTotalElement = document.getElementById('cart-total');
    const cart = getCart();
    cartItemsElement.innerHTML = '';

    let totalAmount = 0;

    cart.forEach(product => {
        const itemElement = document.createElement('li');
        itemElement.className = 'list-group-item d-flex justify-content-between align-items-center';
        itemElement.dataset.productId = product.productId;
        itemElement.innerHTML = `
            <span>${product.name} - ${parseFloat(product.price).toFixed(2)}€ x ${product.quantity}</span>
            <div>
                <button class="btn btn-sm btn-danger" onclick="removeFromCart(${product.productId})">-</button>
                <button class="btn btn-sm btn-success" onclick="increaseQuantity(${product.productId})">+</button>
            </div>
        `;
        cartItemsElement.appendChild(itemElement);
        totalAmount += parseFloat(product.totalPrice);
    });

    cartCountElement.innerText = cart.reduce((total, product) => total + product.quantity, 0);
    cartTotalElement.innerText = `Toplam: ${totalAmount.toFixed(2)} €`;
}

document.querySelectorAll('.add-to-cart').forEach(button => {
    button.addEventListener('click', () => {
        const productId = parseInt(button.getAttribute('data-id'));
        const productName = button.getAttribute('data-name');

        addToCart(productId, productName);
    });
});

document.getElementById('cart-btn').addEventListener('click', () => {
    document.getElementById('cart-sidebar').classList.toggle('active');
    document.getElementById('cart-overlay').classList.toggle('active');
    renderCart();
});

document.querySelector('.close-cart').addEventListener('click', () => {
    document.getElementById('cart-sidebar').classList.remove('active');
    document.getElementById('cart-overlay').classList.remove('active');
});

window.addEventListener('load', updateCartCount);
