$(document).ready(function () {

});

$("#dropDownDist").kendoComboBox({
    dataTextField: "DSTRCT_CODE",
    dataValueField: "DSTRCT_CODE",
    dataSource: {
        type: "json",
        transport: {
            read: {
                url: "/General/Get_Dsitrik",
                contentType: "application/json",
                type: "GET",
                cache: false
            }
        },
        schema: {
            data: "Data",
            total: "Total"
        }
    },
    optionLabel: "Pilih",
    filter: "contains",
    suggest: true,
    change: function (e) {
        var dataItem = e.sender.dataItem();
        console.log($("#dropDownDist").val());
        //$("#dropDownDept").data("kendoComboBox").dataSource.read();
        //var get = viewreportModel.get();
    }
});

$("#txttanggaldari").kendoDatePicker();
$("#txttanggalsampai").kendoDatePicker();


function Filter() {
    var distrik = $("#dropDownDist").val();
    console.log(distrik);
    var sampai = $("#txttanggalsampai").val();
    console.log(sampai);
    var dari = $("#txttanggaldari").val();
    console.log(dari);

    var frame = document.getElementById("iframeReport");

    frame.src = $("#hd_id_url").val() + "/Report/ReportManpower.aspx?Distrik=" + $("#dropDownDist").val() + "&dari=" + $("#txttanggaldari").val() + "&sampai=" + $("#txttanggalsampai").val();

    console.log(frame.src);
}