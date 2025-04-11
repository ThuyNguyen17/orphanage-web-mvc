function openAddChildModal() {
    document.getElementById("addChildModal").classList.remove("hidden");
}

function closeAddChildModal() {
    document.getElementById("addChildModal").classList.add("hidden");
    document.getElementById("addChildForm").reset();
    document.getElementById("caretakerId").textContent = "";
}

function toggleEntryDate() {
    let status = document.getElementById("childStatus").value;
    let entryDateContainer = document.getElementById("entryDateContainer");
    if (status === "Trung Tam") {
        entryDateContainer.classList.remove("hidden");
    } else {
        entryDateContainer.classList.add("hidden");
    }
}

function updateCaretakerId() {
    var select = document.getElementById("childCaretaker");
    var selectedOption = select.options[select.selectedIndex];
    var caretakerId = selectedOption.getAttribute("data-id") || "--";
    document.getElementById("caretakerId").textContent = caretakerId;
}

function updateCounts() {
    const adoptedCount = document.querySelectorAll("#adoptedChildren .child-card").length;
    const centerCount = document.querySelectorAll("#childrenInCenter .child-card").length;

    document.getElementById("adoptedCount").textContent = `${adoptedCount} trẻ`;
    document.getElementById("centerCount").textContent = `${centerCount} trẻ`;
}

async function saveChild(event) {
    event.preventDefault();

    const form = document.getElementById("addChildForm");
    const formData = new FormData(form);
    const submitButton = form.querySelector("button[type='submit']");

    submitButton.disabled = true;
    submitButton.textContent = "Đang lưu...";

    // Validate form data
    if (!validateFormData(formData)) {
        alert("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại các trường bắt buộc.");
        submitButton.disabled = false;
        submitButton.textContent = "Lưu";
        return;
    }

    try {
        const response = await fetch("/Admin/ManageChildren/CreateChild", {
            method: "POST",
            body: formData
        });

        const data = await response.json();

        if (data.success) {
            closeAddChildModal();
            alert("Thêm trẻ thành công!");
            await loadChildrenList(); // Cập nhật danh sách trẻ ngay lập tức
            updateCounts(); // Update the counts
        } else {
            throw new Error(data.message || "Lỗi không xác định");
        }
    } catch (error) {
        console.error("Chi tiết lỗi:", error);
        const errorMessage = error.message ? error.message : "An unknown error occurred.";
        alert(`Lỗi: ${errorMessage}`);
    }finally {
        submitButton.disabled = false;
        submitButton.textContent = "Lưu";
    }
}

function validateFormData(formData) {
    for (let [key, value] of formData.entries()) {
        if (!value) {
            return false; 
        }
    }
    return true;
}

async function deleteChild(childId) {
    if (!confirm("Bạn có chắc chắn muốn xóa trẻ này không?")) return;

    try {
        const response = await fetch(`/Admin/ManageChildren/DeleteChild/${childId}`, { method: "DELETE" });
        const data = await response.json();

        if (data.success) {
            alert("Xóa thành công!");
            window.location.href = "/Admin/ManageChildren"; 
        } else {
            throw new Error(data.message || "Lỗi không xác định");
        }
    } catch (error) {
        alert(`Lỗi: ${error.message}`);
    }
}

function setupInitialView() {
    document.querySelectorAll(".cards-container").forEach(container => {
        container.querySelectorAll(".child-card").forEach((card, index) => {
            if (index >= 4) {
                card.classList.add("hidden");
            }
        });
    });

    document.querySelectorAll(".view-all-link").forEach(link => {
        link.removeEventListener("click", handleViewAllClick);
        link.addEventListener("click", handleViewAllClick);
    });
}

function handleViewAllClick(event) {
    event.preventDefault();
    const sectionId = this.closest(".section").id;
    toggleViewAll(sectionId, this);
}

function toggleViewAll(sectionId, link) {
    var section = document.getElementById(sectionId);
    var cardsContainer = section.querySelector(".cards-container");
    var viewAllLink = link;

    if (cardsContainer.classList.contains("expanded")) {
        cardsContainer.classList.remove("expanded");
        viewAllLink.innerText = "Xem tất cả";
        section.querySelectorAll(".child-card").forEach((card, index) => {
            if (index >= 4) {
                card.classList.add("hidden");
            }
        });
    } else {
        cardsContainer.classList.add("expanded");
        viewAllLink.innerText = "Thu gọn";
        section.querySelectorAll(".child-card").forEach(card => {
            card.classList.remove("hidden");
        });
    }
}

function searchChildren() {
    var input = document.getElementById("search").value.toLowerCase();
    var cards = document.querySelectorAll(".child-card");
    cards.forEach(card => {
        var name = card.getAttribute("data-name").toLowerCase();
        card.style.display = name.includes(input) ? "block" : "none";
    });

    document.querySelectorAll(".view-all-link").forEach(link => {
        var sectionId = link.closest(".section").id;
        var cardsContainer = document.getElementById(sectionId).querySelector(".cards-container");

        if (cardsContainer.classList.contains("expanded")) {
            link.innerText = "Thu gọn";
        } else {
            link.innerText = "Xem tất cả";
        }

        link.removeEventListener("click", handleViewAllClick);
        link.addEventListener("click", handleViewAllClick);
    });
}

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("childCaretaker").addEventListener("change", updateCaretakerId);
    document.getElementById("addChildForm").addEventListener("submit", saveChild);
    document.getElementById("search").addEventListener("input", searchChildren);

    loadChildrenList();
    setupInitialView();

    document.getElementById("openAddChildModalBtn").addEventListener("click", openAddChildModal);

    document.getElementById("closeAddChildModalBtn").addEventListener("click", closeAddChildModal);

    window.addEventListener("click", function (event) {
        if (event.target === document.getElementById("addChildModal")) {
            closeAddChildModal();
        }
    });
});

async function loadChildrenList() {
    try {
        const response = await fetch("/Admin/ManageChildren/GetChildrenList");
        const data = await response.json();

        if (data.success) {
            const centerSection = document.getElementById("childrenInCenter");
            const adoptedSection = document.getElementById("adoptedChildren");

            centerSection.innerHTML = "";
            adoptedSection.innerHTML = "";

            const createCardsContainer = () => {
                const container = document.createElement("div");
                container.className = "cards-container grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4";
                return container;
            };

            const createViewAllLink = (sectionId) => {
                const link = document.createElement("a");
                link.href = "#";
                link.className = "view-all-link text-blue-500 hover:text-blue-700 cursor-pointer block text-center mt-4";
                link.textContent = "Xem tất cả";
                link.onclick = (e) => {
                    e.preventDefault();
                    toggleViewAll(sectionId, link);
                };
                return link;
            };

            const centerContainer = createCardsContainer();
            const centerChildren = data.children.filter(child => child.trangThai === "Trung Tam");

            centerChildren.forEach((child, index) => {
                const card = createChildCard(child, child.imageUrl);
                if (index >= 4) {
                    card.classList.add("hidden");
                }
                centerContainer.appendChild(card);
            });

            centerSection.appendChild(centerContainer);
            if (centerChildren.length > 4) {
                centerSection.appendChild(createViewAllLink("childrenInCenter"));
            }

            const adoptedContainer = createCardsContainer();
            const adoptedChildren = data.children.filter(child => child.trangThai !== "Trung Tam");

            adoptedChildren.forEach((child, index) => {
                const card = createChildCard(child, child.imageUrl);
                if (index >= 4) {
                    card.classList.add("hidden");
                }
                adoptedContainer.appendChild(card);
            });

            adoptedSection.appendChild(adoptedContainer);
            if (adoptedChildren.length > 4) {
                adoptedSection.appendChild(createViewAllLink("adoptedChildren"));
            }

            setupInitialView(); 

        } else {
            console.error("Không thể tải danh sách trẻ:", data.message);
        }
    } catch (error) {
        console.error("Lỗi khi tải danh sách trẻ:", error);
    }
}

function createChildCard(child, imageUrl) {
    const card = document.createElement("div");
    card.className = "child-card relative bg-gray-100 rounded-lg shadow-md p-4 text-center hover:shadow-lg transition duration-300";
    card.setAttribute("data-name", child.hoTen);
    card.setAttribute("data-id", child.maSoTre);

    const birthDate = new Date(child.ngaySinh);
    const formattedDate = `${birthDate.getDate().toString().padStart(2, '0')}/${(birthDate.getMonth() + 1).toString().padStart(2, '0')}/${birthDate.getFullYear()}`;

    card.innerHTML = `
        <img src="${imageUrl}" alt="Ảnh đại diện" 
             class="w-full h-64 mx-auto object-cover mb-3 border-2 border-gray-300" />
        <h3 class="text-lg font-semibold text-gray-700">${child.hoTen}</h3>
        <p class="text-gray-500 text-sm">Ngày sinh: ${formattedDate}</p>
    `;

  
    const tooltip = document.createElement("div");
    tooltip.className = "tooltip hidden fixed bg-white shadow-lg rounded p-2 z-50 border border-gray-300";
    tooltip.style.minWidth = "120px";
    tooltip.innerHTML = `
        <button class="block w-full text-left px-2 py-1 hover:bg-gray-100 details-btn" data-id="${child.maSoTre}">Chi tiết</button>
        <button class="block w-full text-left px-2 py-1 hover:bg-gray-100 edit-btn" data-id="${child.maSoTre}">Sửa</button>
        <button class="block w-full text-left px-2 py-1 hover:bg-gray-100 text-red-500 delete-btn" data-id="${child.maSoTre}">Xóa</button>
    `;

    document.body.appendChild(tooltip); 

    card.addEventListener("contextmenu", function (event) {
        event.preventDefault(); 
        event.stopPropagation();

        let x = event.clientX + window.scrollX;
        let y = event.clientY + window.scrollY;

        if (x + tooltip.offsetWidth > window.innerWidth) {
            x = window.innerWidth - tooltip.offsetWidth - 10;
        }
        if (y + tooltip.offsetHeight > window.innerHeight) {
            y = window.innerHeight - tooltip.offsetHeight - 10;
        }

        tooltip.style.left = `${x}px`;
        tooltip.style.top = `${y}px`;
        tooltip.classList.remove("hidden");

        document.querySelectorAll(".tooltip").forEach(tip => {
            if (tip !== tooltip) tip.classList.add("hidden");
        });
    });

    tooltip.addEventListener("click", function (event) {
        event.stopPropagation();
    });

    tooltip.querySelector(".details-btn").addEventListener("click", function (event) {
        const id = event.target.getAttribute("data-id");
        detailsChild(id);
    });

    tooltip.querySelector(".edit-btn").addEventListener("click", function (event) {
        const id = event.target.getAttribute("data-id");
        editChild(id);
    });

    tooltip.querySelector(".delete-btn").addEventListener("click", function (event) {
        const id = event.target.getAttribute("data-id");
        deleteChild(id);
    });

    document.addEventListener("click", function () {
        document.querySelectorAll(".tooltip").forEach(tip => tip.classList.add("hidden"));
    });

    return card;
}

function detailsChild(maSoTre) {
    window.location.href = `/Admin/ManageChildren/ChildDetails/${maSoTre}`;
}
function editChild(maSoTre) {
    window.location.href = `/Admin/ManageChildren/Update/${maSoTre}`;
}