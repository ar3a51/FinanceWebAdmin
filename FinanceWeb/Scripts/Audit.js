var oResultDebtorTable;
var oResultTransTable;
var alertBox;
function initPage() {

    $("input[type=submit], input[type=button]").button();
    $("#btnAudit").attr("checked", "checked");
    $("#radios").buttonset();

    $("#syncTabs").tabs();

    $("#txtInputFrom").datepicker();
    $("#txtInputTo").datepicker();
    $("#txtInputFromTrans").datepicker();
    $("#txtInputToTrans").datepicker();


    $("#txtInputFrom").datepicker("option", "dateFormat", "dd/mm/yy");
    $("#txtInputTo").datepicker("option", "dateFormat", "dd/mm/yy");
    $("#txtInputFromTrans").datepicker("option", "dateFormat", "dd/mm/yy");
    $("#txtInputToTrans").datepicker("option", "dateFormat", "dd/mm/yy");

    
    oResultDebtorTable = $("#divAuditDebtor .tblDatalist").dataTable({ "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bProcessing": true,
        "bDestroy": true,
        "bAutoWidth": false
       /* "aoColumns": [
                                            { sWidth: "50px" },
                                            { sWidth: "10px" },
                                            { sWidth: "50px" },
                                            { sWidth: "5px" },
                                            { sWidth: "100px" },
                                            { sWidth: "50px" },
                                            { sWidth: "50px" }
                             ]*/
    });

    oResultTransTable = $("#divAuditTrans .tblDatalist").dataTable({ "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bProcessing": true,
        "bDestroy": true,
        "bAutoWidth": false
       /* "aoColumns": [
                                            { sWidth: "50px" },
                                            { sWidth: "10px" },
                                            { sWidth: "50px" },
                                            { sWidth: "50px" },
                                            { sWidth: "50px" },
                                            { sWidth: "50px" },
                                            { sWidth: "50px" }
                             ]*/
    });

    alertBox = $("#alertBox").dialog({
        draggable: false,
        modal: true,
        resizable: false,
        autoOpen: false
    });



}

function sendRequest(sType, sEntityType, sFromDate, sToDate) {
    var sUrl;
    var isError = false;

    if (sType == "debtor")
        sUrl = "/Fintal/Audit/Debtor";
    else
        sUrl = "/Fintal/Audit/Trans";

    if (!performValidation(sType))
        return;

    ajaxSend(sUrl,
            { entityType: sEntityType, fromDate: sFromDate, toDate: sToDate },
            "json",
            true,
            function (result) {

                if (result.length == 0) {
                    showDialog("No Records found");
                    
                }

                if (result.Error != null) {

                    showDialog(result.Error);

                    isError = true;
                    return;
                }

                if (sType == "debtor") {
                    oResultDebtorTable.fnClearTable();
                    oResultDebtorTable.fnAddData(result);

                    oResultDebtorTable.css("width", "100%");
                    oResultDebtorTable.css("height", "100%");
                }
                else {
                    oResultTransTable.fnClearTable();
                    oResultTransTable.fnAddData(result);

                    oResultTransTable.css("width", "100%");
                    oResultTransTable.css("height", "100%");
                }


            },
            function (jhqr, status) {
                if (isError)
                    return;

                if (jhqr.status != "200")
                    alert(jhqr.responseText);
            });

        }

        function showDialog(sMessage) {
            $("#alertBox").html(sMessage);
            alertBox.dialog("open");

        }

        function performValidation(sType) {
            var sErrMsg = "";

          if (sType == "debtor") {

              if ($("#txtInputFrom").val().length == 0 ) /* ||
                   $("#txtInputTo").val().length == 0)*/
              {

                    sErrMsg += "<li>Please specify the date range</li>";
                }

            }
            else {

                if ($("#txtInputFromTrans").val().length == 0/* ||
                    $("#txtInputToTrans").val().length == 0*/) {

                    sErrMsg += "<li>Please specify the date range</li>";
                }
            }

            if (sErrMsg.length > 0) {

                $("#alertBox").html("<ul>" + sErrMsg + "</ul>");
                alertBox.dialog("open");
                return false;
            }
            else
                return true;
        }