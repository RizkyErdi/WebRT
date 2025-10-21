$(document).ready(function () {
    $.get("/Datawarga/GetDataWarga", function (data) {
        var tbody = $("#dataWargaTable tbody");
        tbody.empty();

        data.forEach(function (item) {
            var row = "<tr>" +
                "<td>" + item.NIK + "</td>" +
                "<td>" + item.NoKK + "</td>" +
                "<td>" + item.Nama + "</td>" +
                "<td>" + item.TempatLahir + "</td>" +
                "<td>" + formatDate(item.TanggalLahir) + "</td>" +
                "<td>" + item.JenisKelamin + "</td>" +
                "<td>" + item.Alamat + "</td>" +
                "<td>" + item.RT + "</td>" +
                "<td>" + item.RW + "</td>" +
                "<td>" + item.Kelurahan + "</td>" +
                "<td>" + item.Kecamatan + "</td>" +
                "<td>" + item.Agama + "</td>" +
                "<td>" + item.StatusPerkawinan + "</td>" +
                "<td>" + item.Pekerjaan + "</td>" +
                "<td>" + item.Kewarganegaraan + "</td>" +
                "<td>" + item.NoHP + "</td>" +
                "<td>" + item.Email + "</td>" +
                "<td>" + formatDate(item.TanggalTerdaftar) + "</td>" +
                "</tr>";
            tbody.append(row);
        });
    });
});

function formatDate(jsonDate) {
    const date = new Date(parseInt(jsonDate.substr(6)));
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Month is 0-based
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
}