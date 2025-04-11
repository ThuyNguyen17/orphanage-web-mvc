const dropdownBtn = document.getElementById("dropdownBtn");
const dropdownMenu = document.getElementById("dropdownMenu");
const selectedText = document.getElementById("selectedText");

// Toggle dropdown menu
dropdownBtn.addEventListener("click", () => {
    dropdownMenu.classList.toggle("hidden");
});

// Xử lý chọn tùy chọn
dropdownMenu.querySelectorAll("li").forEach(item => {
    item.addEventListener("click", () => {

        let imgElement = item.querySelector("img"); // Get the image
        let customText = item.getAttribute("data-text"); // Get custom text from data-value

        if (imgElement) {
            selectedText.innerHTML = `<span>${customText}</span> ${imgElement.outerHTML}`; // Set custom text + image
        }

        dropdownMenu.classList.add("hidden"); // Hide dropdown after selection
    });
});

// Ẩn dropdown khi click ra ngoài
document.addEventListener("click", (event) => {
    if (!dropdownBtn.contains(event.target) && !dropdownMenu.contains(event.target)) {
        dropdownMenu.classList.add("hidden");
    }
});

// ✅ Real-time search
function searchNguoiNhanNuoi() {
    let input = document.getElementById("search").value.toLowerCase();
    let rows = document.querySelectorAll("#nguoiNhanNuoiList tr");

    rows.forEach(row => {
        let name = row.querySelector(".name").textContent.toLowerCase();
        row.style.display = name.includes(input) ? "table-row" : "none";
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const dropdownItems = document.querySelectorAll("#dropdownMenu li");

    dropdownItems.forEach(item => {
        item.addEventListener("click", function () {
            let sortType = this.getAttribute("data-value"); // Lấy giá trị data-value
            sortList(sortType);
        });
    });

    function sortList(sortType) {
        let table = document.querySelector("table tbody"); // Chọn tbody trong bảng
        let rows = Array.from(table.querySelectorAll("tr")); // Lấy tất cả các hàng

        rows.sort((rowA, rowB) => {
            let cellA, cellB;

            switch (sortType) {
                case "maSo_asc": // Sắp xếp ID tăng dần
                    cellA = parseInt(rowA.children[0].textContent.trim());
                    cellB = parseInt(rowB.children[0].textContent.trim());
                    return cellA - cellB;

                case "maSo_desc": // Sắp xếp ID giảm dần
                    cellA = parseInt(rowA.children[0].textContent.trim());
                    cellB = parseInt(rowB.children[0].textContent.trim());
                    return cellB - cellA;

                case "hoTen_asc": // Sắp xếp tên A → Z
                    cellA = rowA.children[1].textContent.trim().toLowerCase();
                    cellB = rowB.children[1].textContent.trim().toLowerCase();
                    return cellA.localeCompare(cellB);

                case "hoTen_desc": // Sắp xếp tên Z → A
                    cellA = rowA.children[1].textContent.trim().toLowerCase();
                    cellB = rowB.children[1].textContent.trim().toLowerCase();
                    return cellB.localeCompare(cellA);
            }
        });

        // Xóa nội dung hiện tại và thêm lại các hàng đã sắp xếp
        table.innerHTML = "";
        rows.forEach(row => table.appendChild(row));
    }
});

// ✅ Export to CSV
function exportToCSV() {
    let table = document.getElementById("nguoiTable");
    let rows = table.querySelectorAll("tr");
    let csvContent = [];

    rows.forEach(row => {
        let cells = row.querySelectorAll("td, th");
        let rowData = Array.from(cells).map(cell => `"${cell.textContent.trim()}"`);
        csvContent.push(rowData.join(","));
    });

    let csvString = csvContent.join("\n");
    let blob = new Blob([csvString], { type: "text/csv" });
    let a = document.createElement("a");
    a.href = URL.createObjectURL(blob);
    a.download = "DanhSachNguoiNhanNuoi.csv";
    a.click();
}

function saveAdoption() {
    var formData = new FormData(document.getElementById("addAdoptionForm"));

    fetch("/Admin/Adopter/CreateAdoption", {
        method: "POST",
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert("Adoption added successfully!");
                closeAddAdoptionModal();
                location.reload();
            } else {
                alert("Failed to add adoption: " + data.message);
            }
        })
        .catch(error => console.error("Error:", error));
}

// Open the Add Adoption Modal
function openAddAdoptionModal() {
    document.getElementById("addAdoption").classList.remove("hidden");
}

function closeAddAdoptionModal() {
    let modal = document.getElementById("addAdoption");
    if (modal) {
        modal.classList.add("hidden");
    }
}

document.addEventListener("click", function (event) {
    let modal = document.getElementById("addAdoption");
    if (modal && event.target === modal) {
        closeAddAdoptionModal();
    }
});

function openEditModal(maSoNnn, hoTen, ngaySinh, sdt, diaChi, email,
    maSoTre, hoTenTre, ngaySinhTre, gioiTinhTre, ngayNhapTrungTam, trangThaiTre, anhDaiDienTre) {
    // Hiển thị modal
    document.getElementById("editModal").classList.remove("hidden");

    // Gán dữ liệu vào modal - Thông tin gốc
    document.getElementById("originalMaSoNnn").innerText = maSoNnn;
    document.getElementById("originalHoTen").innerText = hoTen;
    document.getElementById("originalNgaySinh").innerText = ngaySinh;
    document.getElementById("originalSdt").innerText = sdt;
    document.getElementById("originalDiaChi").innerText = diaChi;
    document.getElementById("originalEmail").innerText = email;

    document.getElementById("originalMaSoTre").innerText = maSoTre;
    document.getElementById("originalHoTenTre").innerText = hoTenTre;
    document.getElementById("originalNgaySinhTre").innerText = ngaySinhTre;
    document.getElementById("originalGioiTinhTre").innerText = gioiTinhTre;
    document.getElementById("originalNgayNhapTrungTam").innerText = ngayNhapTrungTam;
    document.getElementById("originalTrangThaiTre").innerText = trangThaiTre;
    document.getElementById("originalAnhDaiDienTre").src = anhDaiDienTre;

    // Gán dữ liệu vào modal - Cho phép chỉnh sửa
    document.getElementById("editMaSoNnn").value = maSoNnn;
    document.getElementById("editHoTen").value = hoTen;
    document.getElementById("editNgaySinh").value = ngaySinh;
    document.getElementById("editSdt").value = sdt;
    document.getElementById("editDiaChi").value = diaChi;
    document.getElementById("editEmail").value = email;

    document.getElementById("editHoTenTre").value = hoTenTre;
    document.getElementById("editNgaySinhTre").value = ngaySinhTre;
    document.getElementById("editTrangThaiTre").value = trangThaiTre;
}

function closeModal() {
    document.getElementById("editModal").classList.add("hidden");
}

// Hide modal on page load
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("addAdoption").classList.add("hidden");
});