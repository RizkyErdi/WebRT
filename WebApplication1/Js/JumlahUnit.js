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

function Filter() {
    var distrik = $("#dropDownDist").val();
    console.log(distrik);
    var periode = $("#txtPeriode").val();
    console.log(periode);

    var frame = document.getElementById("iframeReport");
    
    frame.src = $("#hd_id_url").val() + "/Report/ReportJumlahUnit.aspx?Distrik=" + $("#dropDownDist").val() + "&Periode=" + $("#txtPeriode").val();
    //frame.src = "https://kphosq102/ReportServer_RPTPROD/Pages/ReportViewer.aspx?/report/SHE_Report/rpt_jumlah_unit&DISTRIK=" + $("#dropDownDist").val() + "&PERIODE=" + $("#txtPeriode").val(); + "&rs:embed=true";
    
    console.log(frame.src);
}