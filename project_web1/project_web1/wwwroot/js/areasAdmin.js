document.addEventListener("DOMContentLoaded", function () {
    const toggler = document.getElementById("toggler");
    const sidebar = document.getElementById("sidebar");

    if (toggler && sidebar) {
        toggler.addEventListener("change", function () {
            sidebar.classList.toggle("expanded", toggler.checked);
        });
    }
});
const switchMode = document.getElementById('switch-mode');

if (switchMode) {
    switchMode.addEventListener('change', function () {
        if (this.checked) {
            document.body.classList.add('dark');
        } else {
            document.body.classList.remove('dark');
        }
    });
}
// This function is used to toggle the visibility of the sidebar
document.addEventListener('DOMContentLoaded', function () {
    const dropdownToggle = document.querySelector('#settingsDropdown');
    const dropdownMenu = dropdownToggle.nextElementSibling;

    // Bật/Tắt trạng thái show khi click vào dropdown
    dropdownToggle.addEventListener('click', function (e) {
        e.preventDefault(); // Ngăn ngừa hành động mặc định
        dropdownMenu.classList.toggle('show');
    });
});