$(document).ready(function () {
    dropdowndistrik();
    dropdownbulan();
    //$("#dropDownTahun").datepicker({
    //    format: "yyyy",
    //    viewMode: "years",
    //    minViewMode: "years",
    //    autoclose: true //to close picker once year is selected
    //});
});



function SaveInjury() {
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
        PeriodeBulan: $("#periodeBulan").val(),
        PeriodeTahun: $("#periodeTahun").val(),
        Distrik: $("#dropdownDist").val(),

        NmbrEmpMa: $("#txtNmbrEmployeeMA").val(),
        NmbrEmpApy: $("#txtNmbrEmployeeAPY").val(),
        TotalMswMa: $("#txtTotalMSWMA").val(),
        TotalMswApy: $("#txtTotalMSWAPY").val(),
        NmbrProdSwMa: $("#txtNmbrProdSWMA").val(),
        NmbrProdSwApy: $("#txtNmbrProdSWAPY").val(),

        NumberMinInjMa: $("#txtNmbrMnrInjMA").val(),
        NumberMinInjApy: $("#txtNmbrMnrInjAPY").val(),
        NumberFatigueIncMa: $("#txtNmbrFatigueIncdMA").val(),
        NumberFatigueIncApy: $("#txtNmbrFatigueIncdAPY").val(),
        NmbrLossTimeInjMa: $("#txtNmbrLossTimeInjMA").val(),
        NmbrLossTimeInjApy: $("#txtNmbrLossTimeInjAPY").val(),

        NumberLossShiftslostMa: $("#txtNmbrLossShiftsLostMA").val(),
        NumberLossShiftslostApy: $("#txtNmbrLossShiftsLostAPY").val(),
        LtifrProdShiftsMa: $("#txtLtifrProdShiftsMA").val(),
        LtifrProdShiftsApy: $("#txtLtifrProdShiftsAPY").val(),
        LtifrManHourMa: $("#txtLtiFrManHourMA").val(),
        LtifrManHourApy: $("#txtLtiFrManHourAPY").val(),

        LtifrManShiftsMa: $("#txtLtifrmanshiftsMA").val(),
        LtifrManShiftsApy: $("#txtLtifrmanshiftsAPY").val(),
        LtifrMillHoursMa: $("#txtLtifrMillHoursMA").val(),
        LtifrMillHoursApy: $("#txtLtifrMillHoursAPY").val(),
        DirPer2hoursMa: $("#txtDifrPer2HoursMA").val(),
        DirPer2hoursApy: $("#txtDifrPer2HoursApy").val(),

        SevRateMillhMa: $("#txtSevRatePerMillHMa").val(),
        SevRateMillhApy: $("#txtSevRatePerMillHApy").val(),
        NmbrPotentFatalMa: $("#txtNmbrPotentFatalMa").val(),
        NmbrPotentFatalApy: $("#txtNmbrPotentFatalApy").val(),
        NmbrFatalInjrMa: $("#txtNmbrFatalInjrMa").val(),
        NmbrFatalInjrApy: $("#txtNmbrFatalInjrApy").val(),

        NMbrShiftsLostMa: $("#txtNmbrofShiftsLostMa").val(),
        NMbrShiftsLostApy: $("#txtNmbrofShiftsLostApy").val(),
        FatigueIncdntFreqMa: $("#txtFatigueIncFreqRateMA").val(),
        FatigueIncdntFreqApy: $("#txtFatigueIncFreqRateApy").val(),
        FatalityFreeManshMa: $("#txtFatalityFreemanShMa").val(),
        FatalityFreeManshApy: $("#txtFatalityFreemanShApy").val(),

        FreemanFatalMhMa: $("#txtFreemanfatalMhMa").val(),
        FreemanFatalMhApy: $("#txtFreemanfatalMhApy").val(),

    };
    $.ajax({
        type: "POST",
        url: "/Injury/InsertInjuri",
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

function dropdownbulan() {
    $.ajax({
        type: "POST",
        url: "/Injury/pb_cust_readBulan",
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            if (response.Remarks == true) {
                var items = "";
                items += "<option value='' selected>Silahkan pilih</option>";
                $.each(response.Data, function (key, item) {
                    //console.log(item)
                    items += "<option value='" + item.MONTH + "'>" + (item.MONTH) + "</option>";
                });
                $("#periodeBulan").append(items);

            }
        },
        //complete: function () {
        //    $("#loader").removeClass("is-active");
        //}
    })
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