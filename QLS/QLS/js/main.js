// main.js
document.addEventListener('DOMContentLoaded', () => {
    loadBooks(); // Load danh sách sách khi trang được tải
});

// Load danh sách sản phẩm từ backend
function loadBooks() {
    const apiUrl = 'https://localhost:7217/api/ProductApi';
    
    console.log("Gọi API:", apiUrl);
    
    fetch(apiUrl)
        .then(response => {
            console.log("Status:", response.status);
            if (!response.ok) {
                throw new Error(`Lỗi: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            console.log("Dữ liệu:", data);
            const bookTableBody = document.querySelector('#productList');
            if (!bookTableBody) {
                console.error("Không tìm thấy #productList");
                return;
            }
            bookTableBody.innerHTML = ''; // Xóa nội dung cũ

            data.forEach(book => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${book.id}</td>
                    <td>${book.name}</td>
                    <td>${book.price}</td>
                    <td>${book.description}</td>
                    <td>
                        <button onclick="viewDetail(${book.id})" class="btn btn-info btn-sm text-white"><i class="fa-solid fa-eye"></i></button>
                        <button onclick="editBook(${book.id})" class="btn btn-warning btn-sm text-white"><i class="fa-solid fa-pen-to-square"></i></button>
                        <button onclick="deleteBook(${book.id})" class="btn btn-danger btn-sm"><i class="fa-solid fa-trash"></i></button>
                    </td>
                `;
                bookTableBody.appendChild(row);
            });
        })
        .catch(error => {
            console.error("Chi tiết lỗi:", error);
            Swal.fire('Lỗi', 'Không thể tải danh sách sách!', 'error');
        });
}

// Xem chi tiết sản phẩm
function viewDetail(id) {
    fetch(`https://localhost:7217/api/ProductApi/${id}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) throw new Error('Không tìm thấy sản phẩm');
            return response.json();
        })
        .then(book => {
            const fullNameElement = document.querySelector('.fullName');
            const codeElement = document.querySelector('.code');
            const dateOfBirthElement = document.querySelector('.dateOfBirth');
            const genderElement = document.querySelector('.gender');

            if (fullNameElement) fullNameElement.textContent = book.name;
            if (codeElement) codeElement.textContent = book.id;
            if (dateOfBirthElement) dateOfBirthElement.textContent = book.name;
            if (genderElement) genderElement.textContent = book.description;

            if ($('#modalViewDetailInfo').length) {
                $('#modalViewDetailInfo').modal('show');
            } else {
                console.warn('Modal #modalViewDetailInfo không tồn tại trong HTML');
            }
        })
        .catch(error => {
            console.error('Error viewing book:', error);
            Swal.fire('Lỗi', 'Không thể xem chi tiết sách!', 'error');
        });
}

// Thêm sản phẩm
document.getElementById('btnAdd').addEventListener('click', () => {
    const name = document.getElementById('bookName').value;
    const price = parseFloat(document.getElementById('price').value);
    const description = document.getElementById('description').value;

    if (!name || isNaN(price)) {
        Swal.fire('Lỗi', 'Vui lòng điền đầy đủ tên sách và giá (số)!', 'error');
        return;
    }

    fetch('https://localhost:7217/api/ProductApi', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify({ name, price, description })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => {
        Swal.fire('Thành công', 'Thêm sách thành công!', 'success');
        loadBooks();
        resetForm();
    })
    .catch(error => {
        console.error('Error adding book:', error);
        Swal.fire('Lỗi', 'Không thể thêm sách!', 'error');
    });
});

// Cập nhật sản phẩm
let currentBookId = null;
document.getElementById('btnUpdate').addEventListener('click', () => {
    if (!currentBookId) {
        Swal.fire('Lỗi', 'Vui lòng chọn sách để cập nhật!', 'error');
        return;
    }

    const name = document.getElementById('bookName').value;
    const price = parseFloat(document.getElementById('price').value);
    const description = document.getElementById('description').value;

    if (!name || isNaN(price)) {
        Swal.fire('Lỗi', 'Vui lòng điền đầy đủ tên sách và giá (số)!', 'error');
        return;
    }

    fetch(`https://localhost:7217/api/ProductApi/${currentBookId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body: JSON.stringify({ id: currentBookId, name, price, description })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    })
    .then(data => {
        Swal.fire('Thành công', 'Cập nhật sách thành công!', 'success');
        loadBooks();
        resetForm();
        currentBookId = null;
        document.getElementById('btnClear').style.display = 'none';
    })
    .catch(error => {
        console.error('Error updating book:', error);
        Swal.fire('Lỗi', 'Không thể cập nhật sách!', 'error');
    });
});

// Chỉnh sửa sản phẩm
function editBook(id) {
    fetch(`https://localhost:7217/api/ProductApi/${id}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
    })
    .then(response => {
        if (!response.ok) throw new Error('Không tìm thấy sản phẩm');
        return response.json();
    })
    .then(book => {
        document.getElementById('bookName').value = book.name;
        document.getElementById('price').value = book.price;
        document.getElementById('description').value = book.description;
        currentBookId = id;
        document.getElementById('btnClear').style.display = 'inline-block';
    })
    .catch(error => {
        console.error('Error editing book:', error);
        Swal.fire('Lỗi', 'Không thể tải thông tin sách để chỉnh sửa!', 'error');
    });
}

// Xóa sản phẩm
function deleteBook(id) {
    Swal.fire({
        title: 'Bạn có chắc không?',
        text: 'Bạn sẽ không thể khôi phục lại sách này!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then(result => {
        if (result.isConfirmed) {
            fetch(`https://localhost:7217/api/ProductApi/${id}`, {
                method: 'DELETE',
                headers: {
                    'Accept': 'application/json',
                },
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(() => {
                Swal.fire('Thành công', 'Xóa sách thành công!', 'success');
                loadBooks();
            })
            .catch(error => {
                console.error('Error deleting book:', error);
                Swal.fire('Lỗi', 'Không thể xóa sách!', 'error');
            });
        }
    });
}

// Reset form
document.getElementById('btnReset').addEventListener('click', () => {
    resetForm();
    currentBookId = null;
    document.getElementById('btnClear').style.display = 'none';
});

function resetForm() {
    document.getElementById('studentForm').reset();
}

// Xóa sản phẩm từ form
document.getElementById('btnClear').addEventListener('click', () => {
    if (currentBookId) {
        deleteBook(currentBookId);
        resetForm();
        currentBookId = null;
        document.getElementById('btnClear').style.display = 'none';
    }
});