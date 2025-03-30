$(document).ready(function () {
    loadiuran();

})

function loadiuran() {
    // Mengambil data dari /Order/GetOrders saat halaman selesai dimuat
    $.ajax({
        type: "POST",
        url: "/Home/GetOrders",
        dataType: "json",
        success: function (data) {
            let tableBody = $("#orderTableBody");
            tableBody.empty(); // Kosongkan tabel sebelum menambahkan data baru

            $.each(data, function (index, order) {
                let badgeClass = "";
                debugger;
                // If-else untuk menentukan warna badge berdasarkan status
                if (order.Status_Iuran == "Lunas") {
                    badgeClass = "badge badge-success"; // Hijau
                } else {
                    badgeClass = "badge badge-danger"; // Merah
                }

                let row = `<tr>
                            <td>${order.Id}</td>
                            <td>${order.Nama}</td>
                            <td>${order.Alamat}</td>
                            <td><span class="${badgeClass}">${order.Status_Iuran}</span></td>
                            <td>${order.Kontak}</td>
                        </tr>`;

                tableBody.append(row);
                console.log('datanya apa aja: ', row);
            });
            console.log("Data berhasil diambil:", data);
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", xhr.responseText);
        }
    });
}

//function loadiuran() {
//    $.ajax({
//        type: "GET",
//        url: "/Home/GetOrders",
//        dataType: "json",
//        success: function (data) {
//            let tableBody = $("#orderTableBody");
//            tableBody.empty(); // Kosongkan tabel sebelum menambahkan data baru
//            //console.log(data);
//            $.each(data, function (index, order) {
//                let badgeClass = "";
//                debugger;
//                // If-else untuk menentukan warna badge berdasarkan status
//                if (order.status === "Lunas") {
//                    badgeClass = "badge bg-success"; // Hijau
//                } else {
//                    badgeClass = "badge bg-danger"; // Merah
//                }

//                let row = `<tr>
//                            <td>${order.No}</td>
//                            <td>${order.Nama}</td>
//                            <td>${order.Alamat}</td>
//                            <td><span class="${order.badgeClass}">${order.status}</span></td>
//                            <td>${order.Kontak}</td>
//                        </tr>`;

//                tableBody.append(row);
//            });
//        },
//        error: function (xhr, status, error) {
//            console.error('Error fetching data:', error);
//        }
//    });
//}

