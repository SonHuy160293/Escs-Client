﻿
// Handle form submission (for demonstration purposes)
document.getElementById('createTokenForm').addEventListener('submit', function (e) {
    e.preventDefault();
    const tokenName = document.getElementById('tokenName').value;
    const allowances = Array.from(
        document.querySelectorAll('#createTokenForm .form-check-input:checked'),
    ).map((checkbox) => checkbox.value);

    console.log('Token Name:', tokenName);
    console.log('Allowances:', allowances);

    alert('Token created successfully!');
    // Reset the form and close the modal
    this.reset();
    const modal = bootstrap.Modal.getInstance(document.getElementById('createTokenModal'));
    modal.hide();
});


function openDeleteModal(tokenName) {
    // Set the token name dynamically in the modal
    const deleteMessage = document.getElementById('deleteMessage');
    deleteMessage.innerHTML = `Bạn có chắc chắn muốn xóa <strong>${tokenName}</strong> không?`;

    // Show the modal
    const deleteTokenModal = new bootstrap.Modal(document.getElementById('deleteTokenModal'));
    deleteTokenModal.show();
}

function confirmDelete() {
    // Perform the delete operation here
    alert('Token deleted successfully!');

    // Close the modal
    const deleteTokenModal = bootstrap.Modal.getInstance(
        document.getElementById('deleteTokenModal'),
    );
    deleteTokenModal.hide();
}

function openAuthModal() {
    // Show the modal
    const authModal = new bootstrap.Modal(document.getElementById('authModal'));
    authModal.show();
}

function togglePasswordVisibility() {
    const passwordInput = document.getElementById('passwordInput');
    const passwordIcon = document.getElementById('passwordIcon');

    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        passwordIcon.classList.remove('bi-eye-slash');
        passwordIcon.classList.add('bi-eye');
    } else {
        passwordInput.type = 'password';
        passwordIcon.classList.remove('bi-eye');
        passwordIcon.classList.add('bi-eye-slash');
    }
}

function authenticate() {
    const password = document.getElementById('passwordInput').value;

    if (password) {
        // Perform authentication logic here
        alert('Xác thực thành công!');
        const authModal = bootstrap.Modal.getInstance(document.getElementById('authModal'));
        authModal.hide();
    } else {
        alert('Vui lòng nhập mật khẩu!');
    }
}