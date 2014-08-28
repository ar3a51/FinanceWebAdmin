var oTable;
var alertBox;

function initPage() {

    $("input[type=submit]").button();
    $("#btnDebtor").attr("checked", "checked");
    $("#radios").buttonset();

   

    $(".syncTabs, #indSync, #compSync").tabs({
        select: function (event, ui) {
            refreshDatalist();
        }
    });

    alertBox = $("#alertBox").dialog({
        draggable: false,
        modal: true,
        resizable: false,
        autoOpen: false
    });
}
function submitPage(bType) {

    $("#dialogConfirm").dialog({
        resizeable: false,
        height: 300,
        width: 400,
        modal: true,
        buttons: {
            "Submit": function () {
                //send the record
                /*processSending();
                $(this).dialog("close");
                $("#txtMemberId").val("Member ID");*/


                if (bType == "individual") {

                    var indivdata = {
                        sync_id: null,
                        batch_id: null,
                        individual_id: $("#txtMemberId").val(),
                        insert_update: $("#slctInsertUpdate").val(),
                        update_date: null,
                        recordStatus: 1
                    };

                    var auditDataInd = {
                        modifiedBy: "",
                        updateDate: "",
                        action: $("#slctInsertUpdate").val(),
                        MemberId: $("#txtMemberId").val(),
                        entityType: "Individual",
                        reason: $("#txtConfirm").val()
                    };

                    processSending(indivdata, auditDataInd,
                                   "/Fintal/SyncTableAdmin/submitData",
                                   "#divSyncTableInd");
                }
                else {

                    var compdata = {
                        sync_id: null,
                        batch_id: null,
                        individual_id: $("#txtCompanyId").val(),
                        insert_update: $("#slctInsertUpdateCompany").val(),
                        update_date: null,
                        recordStatus: 1
                    };
                   

                    var auditDataComp = {
                        modifiedBy: "",
                        updateDate: "",
                        action: $("#slctInsertUpdateCompany").val(),
                        MemberId: $("#txtCompanyId").val(),
                        entityType: "Company",
                        reason: $("#txtConfirm").val()
                    };

                    processSending(compdata, auditDataComp,
                                   "/Fintal/SyncTableAdmin/submitCompanyData",
                                   "#divSyncTableCompany");

                }

                $(this).dialog("close");
            },
            "Cancel": function () {

                $(this).dialog("close");
            }
        }
    });

}

function processSending(data, auditData, url, sDivRenderResult) {

    var isError = false;

    ajaxSend(url, $.toDictionary({ indData: data, auditData: auditData }), "json", true,
                     function (result) {
                         if (result.Error != null) {
                             isError = true;
                             showDialog(result.Error);
                         }
                       
                     },
                     function (jhqr, result) {
                        
                         if (!isError) {
                             $(sDivRenderResult).html(jhqr.responseText);
                             $(".tblDatalist").dataTable({ "bDestroy": true });
                         }
                        
                     });
}


function refreshDatalist() {

    /* ajaxSend("/EventMonitoringAdmin/user/getSuppressedLog",
    "html",
    false,
    null,
    function (jhqr, result) {

    if (result == "success")
    $("#divSuppressedLog").html(jhqr);
    else {
    $("#idNotification").html(jhqr);
    $("#idNotification").dialog("open");
    }

    });*/

    ajaxSend("/Fintal/SyncTableAdmin/getCompanySyncList", "html", false, null, function (jhqr, result) {
        $("#divSyncTableCompany").html(jhqr);

        $(".tblDatalist").dataTable({ "bDestroy": true });

    });

    ajaxSend("/Fintal/SyncTableAdmin/getIndividualSyncList", "html", false, null, function (jhqr, result) {
        $("#divSyncTableInd").html(jhqr);

        $(".tblDatalist").dataTable({ "bDestroy": true });
    });
}

function showDialog(sMessage) {
    $("#alertBox").html(sMessage);
    alertBox.dialog("open");

}

