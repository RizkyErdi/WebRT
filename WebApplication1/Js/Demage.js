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



function SaveDemage() {
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
        PeriodeTahun: $("#dropDownTahun").val(),
        Distrik: $("#dropdownDist").val(),
        NmbrVhclsMchnSite: $("#nmbrvhcmchnsite").val(),
        NmbrVhcMchnHrSiteMa: $("#txtNmbrVhcMchnHrSiteMa").val(),
        NmbrVhcMchnHrSitePy: $("#txtNmbrVhcMchnHrSitePy").val(),
        NmbrTraffIncdntMa: $("#txtNmbrTraffIncdntMa").val(),
        NmbrTraffIncdntPy: $("#txtNmbrTraffIncdntPy").val(),
        PropDmgIncd999Ma: $("#txtPropDmgIncd999Ma").val(),
        PropDmgIncd999Py: $("#txtPropDmgIncd999Py").val(),
        PropDmgIncd1001Ma: $("#txtPropDmgIncd1001Ma").val(),
        PropDmgIncd1001Py: $("#txtPropDmgIncd1001Py").val(),

        EstTotalValDmgUsMa: $("#txtEstTotValDmgUsma").val(),
        EstTotalValDmgUsPy: $("#txtEstTotValDmgUsPy").val(),
        PropDmgFreqRateMa: $("#txtPropDmgFreqRateMa").val(),
        PropDmgFreqRatePy: $("#txtPropDmgFreqRatePy").val(),
        TraffIncdFreqRateMa: $("#txtTraffIncdFreqRateMa").val(),
        TraffIncdFreqRatePy: $("#txtTraffIncdFreqRatePy").val(),

        LossDtPremFail999TyMa: $("#txtLossDtPremFail999TyMa").val(),
        LossDtPremFail999TyPy: $("#txtLossDtPremFail999TyPy").val(),
        LossDtPremFail1001TyMa: $("#txtLossDtPremFail1001TyMa").val(),
        LossDtPremFail1001TyPy: $("#txtLossDtPremFail1001TyPy").val(),
        EstTotValLossUsTyMa: $("#txtEstTotValLossUsTyMa").val(),
        EstTotValLossUsTyPy: $("#txtEstTotValLossUsTyPy").val(),
        
        LossDtPremFailFreqRateTyMa: $("#txtLossDtPremFailFreqRateTyMa").val(),
        LossDtPremFailFreqRateTyPy: $("#txtLossDtPremFailFreqRateTyPy").val(),
        LossDtPremFailInc999EqpMa: $("#txtLossDtPremFailInc999EquipMa").val(),
        LossDtPremFailInc999EqpPy: $("#txtLossDtPremFailInc999EquipPy").val(),
        LossDtPremFailIncd1001EqMa: $("#txtLossDtPremFailInc1001EquipMa").val(),
        LossDtPremFailIncd1001EqPy: $("#txtLossDtPremFailInc1001EquipPy").val(),
        
        EstTotalValLossEqpMa: $("#txtEstTotValLossUsEquipMa").val(),
        EstTotalValLossEqpPy: $("#txtEstTotValLossUsEquipPy").val(),
        LossPremFailFreqMa: $("#txtLossDtPremFailFreqEqMa").val(),
        LossPremFailFreqPy: $("#txtLossDtPremFailFreqEqPy").val(),
        

    };
    $.ajax({
        type: "POST",
        url: "/Demage/InsertDemage",
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