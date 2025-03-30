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

$("#txttahun").kendoDatePicker({
    start: "decade",
    depth: "decade",
    format: "yyyy"
});


function Filter() {
    var distrik = $("#dropDownDist").val();
    console.log(distrik);
    var tahun = $("#txttahun").val();
    console.log(tahun);
    

    var frame = document.getElementById("iframeReport");

    frame.src = $("#hd_id_url").val() + "/Report/ReportDemage.aspx?Distrik=" + $("#dropDownDist").val() + "&Tahun=" + $("#txttahun").val();

    console.log(frame.src);
}