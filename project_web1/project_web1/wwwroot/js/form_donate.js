//document.addEventListener("DOMContentLoaded", function () {
//    // Kiểm tra xem thư viện AOS đã được tải chưa
//    if (typeof AOS !== "undefined") {
//        AOS.init({
//            duration: 1000,
//            easing: 'ease-in-out',
//            once: true,
//        });
//    } else {
//        console.error("AOS library is not loaded. Check your script imports.");
//    }

//    initializeCards();
//    setupGlobalEvents();
//});

//// ===================== Global Event Handlers ===================== //

//function setupGlobalEvents() {
//    const donateBtn = document.getElementById("donateBtn");
//    if (donateBtn) {
//        donateBtn.addEventListener("click", function (event) {
//            event.preventDefault();
//            loadDonationModal();
//        });
//    } else {
//        console.error("Donate button not found.");
//    }
//}

//// ===================== Modal Handling ===================== //

//function loadDonationModal() {
//    const donationContainer = document.getElementById("donation-container");
//    if (!donationContainer) {
//        console.error("Donation container not found.");
//        return;
//    }

//    // Kiểm tra nếu modal đã được tải trước đó
//    if (document.getElementById("donationModal")) {
//        document.getElementById("donationModal").classList.remove("hidden");
//        return;
//    }

//    fetch("/Views/Home/donation_modal.cshtml")
//        .then(response => response.text())
//        .then(data => {
//            donationContainer.innerHTML = data;
//            setupModalEvents();
//            setupDonationEvents();
//            setupPaymentMethodEvents();
//            document.getElementById("donationModal").classList.remove("hidden");
//        })
//        .catch(error => console.error("Error loading the donation modal:", error));
//}

//function setupModalEvents() {
//    setTimeout(() => {
//        const modal = document.getElementById("donationModal");
//        const closeModalBtn = document.querySelector("[data-modal-close='#donationModal']");

//        if (!modal || !closeModalBtn) {
//            console.error("Modal elements not found.");
//            return;
//        }

//        closeModalBtn.addEventListener("click", closeDonationModal);
//        window.addEventListener("click", function (e) {
//            if (e.target === modal) {
//                closeDonationModal();
//            }
//        });
//    }, 300);
//}

//function closeDonationModal() {
//    const modal = document.getElementById("donationModal");
//    if (modal) {
//        modal.classList.add("hidden");
//    } else {
//        console.error("Donation modal not found.");
//    }
//}

//// ===================== Card Swinging Effect ===================== //

//let currentCardIndex = 0;
//let cards = [];

//function initializeCards() {
//    cards = document.querySelectorAll(".swing-card");
//    if (cards.length > 0) {
//        showCard(currentCardIndex);
//    }
//}

//function showCard(index) {
//    cards.forEach((card, i) => {
//        card.classList.toggle("active", i === index);
//    });
//}

//// ===================== Donation Amount & Currency Handling ===================== //

//async function setupDonationEvents() {
//    const amountSelect = document.getElementById("amountSelect");
//    const customAmountInput = document.getElementById("customAmount");
//    const currencySelect = document.getElementById("currencySelect");
//    const currencySymbol = document.getElementById("currencySymbol");

//    if (!amountSelect || !customAmountInput || !currencySelect || !currencySymbol) {
//        console.error("Donation elements not found.");
//        return;
//    }

//    const currencySymbols = { "VND": "₫", "USD": "$", "EUR": "€", "JPY": "¥" };
//    let exchangeRates = { "USD": 1, "EUR": 0.85, "JPY": 110, "VND": 23000 };

//    try {
//        const response = await fetch("https://api.exchangerate-api.com/v4/latest/USD");
//        const data = await response.json();
//        exchangeRates = data.rates;
//    } catch (error) {
//        console.error("Error fetching exchange rates:", error);
//    }

//    function updateCurrency() {
//        const selectedCurrency = currencySelect.value;
//        currencySymbol.textContent = currencySymbols[selectedCurrency] || "$";
//    }

//    currencySelect.addEventListener("change", updateCurrency);
//    updateCurrency();
//}

//// ===================== Payment Method Handling ===================== //

//function setupPaymentMethodEvents() {
//    const momoRadio = document.getElementById("momo");
//    const bankRadio = document.getElementById("bank");
//    const paymentDetailsContainer = document.getElementById("paymentDetails");
//    const amountSelect = document.getElementById("amountSelect");
//    const customAmountInput = document.getElementById("customAmount");

//    if (!momoRadio || !bankRadio || !paymentDetailsContainer || !amountSelect) {
//        console.error("Payment method elements not found.");
//        return;
//    }

//    function generateVietQR(amount) {
//        return generatePaymentHTML("Vietcombank", "0881000478903", "Đặng Lâm Thế Nhân", "VCB", amount);
//    }

//    function generateMoMoQR(amount) {
//        return generatePaymentHTML("MoMo", "0915221018", "MoMo", "MOMO", amount);
//    }

//    function generatePaymentHTML(bankName, accountNumber, accountHolder, bankCode, amount) {
//        const qrUrl = `https://img.vietqr.io/image/${bankCode}-${accountNumber}-qr_only.png?amount=${amount}&addInfo=Donation`;
//        return `
//            <p class="text-lg font-semibold text-gray-700 text-center">${bankName}</p>
//            <p class="text-lg font-semibold text-gray-700 text-center">Số tài khoản: ${accountNumber}</p>
//            <p class="text-lg font-semibold text-gray-700 text-center">Chủ tài khoản: ${accountHolder}</p>
//            <img src="${qrUrl}" alt="QR Chuyển khoản" class="w-40 h-40 mx-auto mt-4">
//        `;
//    }

//    function updatePaymentFields() {
//        let selectedAmount = amountSelect.value === "custom" ? customAmountInput.value : amountSelect.value;
//        if (!selectedAmount || isNaN(selectedAmount) || selectedAmount <= 0) {
//            paymentDetailsContainer.innerHTML = `<p class="text-red-500 text-center">Vui lòng nhập số tiền hợp lệ.</p>`;
//            return;
//        }
//        paymentDetailsContainer.innerHTML = momoRadio.checked
//            ? generateMoMoQR(selectedAmount)
//            : generateVietQR(selectedAmount);
//    }

//    momoRadio.addEventListener("change", updatePaymentFields);
//    bankRadio.addEventListener("change", updatePaymentFields);
//    amountSelect.addEventListener("change", updatePaymentFields);
//    customAmountInput.addEventListener("input", updatePaymentFields);
//}