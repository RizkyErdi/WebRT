$(document).ready(function () {
    dropdowndistrik();
});

function SaveSubcount() {
    $("#loader").addClass("is-active");
    //var isValid = validateInput();
    //if (!isValid) {
    //    $("#message").show();
    //    $("#loader").removeClass("is-active");
    //    return;
    //} else {
    //    $("#message").hide();
    //}

    var ClsSave = {
        NamaSubcount: $("#txtNamaSc").val(),
        JumlahSubcount: $("#txtDataSc").val(),
        DistrikSubcount: $("#dropdownDist").val(),
        TanggalAwal: $("#tanggalKerjaAwal").val(),
        TanggalAkhir: $("#tanggalKerjaAkhir").val(),

    };
    $.ajax({
        type: "POST",
        url: "/Home/InsertSubcount",
        data: JSON.stringify(ClsSave),
        contentType: "application/json",
        success: function (response) {
            //console.log(data);
            if (response.Remarks != true) {
                alert(response.Message);
                //window.location.href = "/FormKetKerja/DataKetKerja";
                return;
            }
            else {
                alert(response.Message);
                location.reload();
                return;
            }
        },
        //failure: function (errMsg) {
        //    alert(errMsg);
        //},
        complete: function () {
            $("#loader").removeClass("is-active");
        }
    });
}

function dropdowndistrik() {
    $.ajax({
        type: "POST",
        url: "/Home/pb_cust_readDistrik",
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            if (response.Remarks == true) {
                var items = "";
                items += "<option value='' selected>Silahkan pilih</option>";
                $.each(response.Data, function (key, item) {
                    //console.log(item)
                    items += "<option value='" + item.DSTRCT_CODE + "'>" + (item.DSTRCT_CODE) + "</option>";
                });
                $("#dropdownDist").append(items);

            }
        },
        //complete: function () {
        //    $("#loader").removeClass("is-active");
        //}
    })
}