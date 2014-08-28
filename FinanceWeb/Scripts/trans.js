var incadataTable;
var topClassDataTable;
var alertBox;
function initElement() {

    /**************initialize jquery form********************/
    $.ajaxSetup({ cache: false });
    $("#btnTrans").attr("checked", "checked");
    $("#radios").buttonset();

    
   alertBox = $("#alertBox").dialog({
        'draggable' : false,
        'modal':true,
        'resizable':false,
        'autoOpen':false
    });

    var today = new Date();

   

    $("#dtInputFromDate").datepicker();
    $("#dtInputToDate").datepicker();
    $("#dtInputFromDate2").datepicker();
    $("#dtInputToDate2").datepicker();

    $("#dtInputFromDate").datepicker("option", "dateFormat", "dd/mm/yy");
    $("#dtInputToDate").datepicker("option", "dateFormat", "dd/mm/yy");
    $("#dtInputFromDate2").datepicker("option", "dateFormat", "dd/mm/yy");
    $("#dtInputToDate2").datepicker("option", "dateFormat", "dd/mm/yy");

    $("#syncTabs").tabs();
    incadataTable = $(".bizStagingList").dataTable({ "bJQueryUI": true,
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": globalSearch/*"/fintal/trans/search/IMPORTED/inca/01-06-2012"*/,
        "fnServerData": function (sSource, aoData, fnCallback) {

            aoData.push({ "name": "transStatus", "value": $("#selBizTransStatus").val() })
            aoData.push({ "name": "type", "value": "inca" })
            aoData.push({ "name": "fromDate", "value": today.getDate() + "/" + ("0" + (today.getMonth() + 1)).slice(-2) + "/" + today.getFullYear() })
            aoData.push({ "name": "toDate", "value": "" })

            $.getJSON(sSource, aoData, function (json) {
               
                fnCallback(json)
            });
        },
        "sPaginationType": "full_numbers",
        "bDestroy": true,
        "iDeferLoading":0,
        "bRetrieve": true,
        /* "bAutoWidth": false,*/
        "aoColumns": [
                                            { 'sName': "DOC_REF_ID", "bSortable": false },
                                            { 'sName': "STATUS", "bSortable": false },
                                            { 'sName': "DOCUMENT_TYPE", "bSortable": false },
                                            { 'sName': "MEMBER_ID", "bSortable": false },
                                            { 'sName': "DATE_CREATED", 'sType': "date", "bSortable": false },
                                            { 'sName': "AMOUNT", "bSortable": false },
                                            { 'sName': "RESET", "bSortable": false }
                             ]
    });
    topClassDataTable = $(".topclasslist").dataTable({ "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bProcessing": true,
        "bServerSide":true,
        "bDestroy": true,
        //"bAutoWidth": false,
        "sAjaxSource": globalSearch,
        "fnServerData": function (sSource, aoData, fnCallback) {

            aoData.push({ "name": "transStatus", "value": $("#selTopClassStatus").val() })
            aoData.push({ "name": "type", "value": "topclass" })
            aoData.push({ "name": "fromDate", "value": today.getDate() + "/" + ("0" + (today.getMonth() + 1)).slice(-2) + "/" + today.getFullYear() })
            aoData.push({ "name": "toDate", "value": "" })

            $.getJSON(sSource, aoData, function (json) {
               
                fnCallback(json)
            });
        },
         "aoColumns": [
        {'sName': "DOC_REF_ID", "bSortable": false },
        { "bSortable": false },
        { "bSortable": false },
        { "bSortable": false },
        { "bSortable": false },
        { "bSortable": false },
        { "bSortable": false }
        ]
    });
    $("input:button").button();

    $("input[type=radio]").click(function () {

        //$("input[type=radio]").attr("checked", "");
        window.location = $(this).attr("value");
        //$(this).attr("checked", "checked");


    });

    /******************************************************/



}
function getData(sUrl,sTransStatus,dtFromDate,dtToDate,sDivResult,sType,sTableName) {

    var isError = false;
    var oSettings;
    /***************validation*****************/

    if (!performValidation(sType, ""))
        return;

    /*******************************************/
    //alert("looking");
    
    oSettings = (sType == "inca")? incadataTable.fnSettings(): topClassDataTable.fnSettings();

    oSettings.bProcessing = true;
    oSettings.bServerSide = true;
    oSettings.sAjaxSource = globalSearch/*sUrl +"/"+ sTransStatus + "/" + sType + "/" + dtFromDate + "/" + dtToDate*/;
    oSettings.fnServerData = function (sSource, aoData, fnCallback) {
        //alert(sSource);
        aoData.push({ "name": "transStatus", "value": sTransStatus })
        aoData.push({ "name": "type", "value": sType })
        aoData.push({ "name": "fromDate", "value": dtFromDate })
        aoData.push({ "name": "toDate", "value": dtToDate })

        $.getJSON(sSource, aoData, function (json) {
            //incadataTable.fnAddData(json.aaData);
            fnCallback(json);

            $((sType == "inca") ? ".incarows" : ".rows").click(function (event) {
                //alert($(this).attr('href'));
                displayDetails($(this).attr('href'), sType);
                event.preventDefault();
            });
        });
    };
    
    //alert("ajax source: " + incadataTable.fnSettings().sAjaxSource);
    if (sType == "inca")
        incadataTable.fnDraw();
    else {
            topClassDataTable.fnDraw();

        }

    }

    function displayDetails(transId,stype) {

        $.getJSON(globalDetail,
                  { "transId": transId, "sType": stype },
                  function (json) {


                      var tableTemplate = "<div style='display:none;'>";
                      tableTemplate = tableTemplate.concat("<table><tr><td>Document ID:</td>");
                      tableTemplate = tableTemplate.concat("<td>");
                      tableTemplate = tableTemplate.concat((stype == "inca") ? json.DOC_ID : json.DocumentId);
                      tableTemplate = tableTemplate.concat("</td></tr>");
                      tableTemplate = tableTemplate.concat("<tr><td>Status:</td><td>");
                      tableTemplate = tableTemplate.concat((stype == "inca") ? json.ONYX_StagingStatus : json.Status);
                      tableTemplate = tableTemplate.concat("</td></tr>");
                      tableTemplate = tableTemplate.concat("<tr><td>Description:</td><td>");
                      tableTemplate = tableTemplate.concat((stype == "inca") ? json.ONYX_StagingErrorDescription : json.StatusDesc);
                      tableTemplate = tableTemplate.concat("</td></tr>");
                      tableTemplate = tableTemplate.concat("</table></div>");

                      //alert(tableTemplate);
                      //$(tableTemplate).appendTo("body");

                      var displayDialog = $(tableTemplate).dialog({
                      'draggable': false,
                      'modal': true,
                      'resizable': false,
                      'autoOpen': false,
                      'title': "Transaction Details",
                      //width: 450,
                      //height: 250,
                      close: function (event, ui) {

                        displayDialog.remove();

                      }


                      });

                      displayDialog.dialog("open");
                      /* if (stype == "inca") {
                      alert("Document ID:\n" + json.DOC_ID + "\n \n" + "Status:\n" + json.ONYX_StagingStatus + "\n \n" +
                      "Description:\n" + json.ONYX_StagingErrorDescription + "\n");
                      }
                      else {
                      alert("Document ID:\n" + json.DocumentId + "\n \n" + "Status:\n" + json.Status + "\n \n" +
                      "Description:\n" + json.StatusDesc);*/
                      //}
                      //alert((stype == "inca") ? json.DOC_ID : json.DocumentId);

                  });
    }

          function retrieveData(sSelector) {
              var $selected = $(sSelector);

              var selections = new Array($selected.size());
              var intCounter = 0;

              $.each($selected, function (i, input) {

                 
                  selections[intCounter] = {
                    transId : input.value,
                    status : $("#selResetStatus").val()
                  };


                  intCounter++;

              });

              return selections;
          }

          function sendData(sUrl, sSelector, sCurrentStatus, dtFromDate, dtToDate, sType, sDivRenderResult) {

              var data = retrieveData(sSelector);
              
              
              var auditData = {
                                auditId:0,
                                modifiedBy: "",
                                updateDate: "",
                                action: $("#selResetStatus").val(),
                                TransId:0,
                                system:sType,
                                reason:$("#txtConfirm").val()
                              };
                            //$(sDivRenderResult).html("Refreshing Data...");
                              ajaxSend(sUrl, $.toDictionary({ datas: data, transStatus: sCurrentStatus,
                                  fromDate: dtFromDate,
                                  toDate: dtToDate,
                                  type: sType,
                                  auditData: auditData
                              }), "json", true, function (result) {

                                  if (result.iTotalRecords == null) {
                                      showDialog(result.Error);
                                      return;
                                  }

                                  if (result.iTotalRecords == "0") {
                                      showDialog("No records returned");
                                  }

                                  if (sType == "inca") {
                                      /*incadataTable.fnClearTable();
                                      incadataTable.fnAddData(result.aaData);*/

                                      incadataTable.css("width", "100%")
                                      incadataTable.css("height", "100%")
                                  }
                                  else {

                                      topClassDataTable.fnClearTable();
                                      topClassDataTable.fnAddData(result.aaData);
                                      topClassDataTable.css("width", "100%")
                                      topClassDataTable.css("height", "100%")
                                  }

                              },
                   function (jhqr, textStatus) {


                       //testing
                       
                       if (jhqr.status != "200")
                       { showDialog(jhqr.responseText); return; }

                   });

                   if (sType == "inca") {
                       incadataTable.fnClearTable();
                       incadataTable.fnDraw();
                   }
                   else {
                       topClassDataTable.fnDraw();
                   }
                   //alert("end drawing");

                } 

        function ShowDialog(bType, sSelector, sCurrentStatus) {

            $("#notify").html("");
            $("#txtConfirm").html("");

            populateConfirmSelection(sCurrentStatus);

        /***************validation*****************/
           if (!performValidation(bType, sSelector))
               return;

         /*************************************/



            $("#frmConfirm").dialog({
                resizeable: false,
                height: 400,
                width: 400,
                modal: true,
                buttons: {
                    "Submit": function () {

                        if ($("#txtConfirm").val() == "") {
                            $("#notify").html(" * Please specify the reason.");
                            return;
                        }

                        if (bType == "biztalk") {

                            sendData("/Fintal/trans/submit", sSelector,
                                      $("#selBizTransStatus").val(),
                                      $("#dtInputFromDate").val(),
                                      $("#dtInputToDate").val(),
                                      "inca",
                                      "#divTransInCA");
                        }
                        else {

                            sendData("/Fintal/trans/submit", sSelector,
                                      $("#selTopClassStatus").val(),
                                      $("#dtInputFromDate2").val(),
                                      $("#dtInputToDate2").val(),
                                      "topclass",
                                      "#divTransTopClass");
                            //alert("Currently not supported");
                        }

                        $(this).dialog("close");
                    },
                    "Cancel": function () {

                        $(this).dialog("close");
                    }
                }
            });



        }

        function performValidation(sType,sSelector) {
            var sErrMsg = "";

            if (sSelector.length > 0) {
                if ($(sSelector).size() == 0) {
                    sErrMsg += "<li>No items selected</li>";
                }
            }

            if (sType == "biztalk" || sType=="inca") {

                if ($("#dtInputFromDate").val().length == 0 /*||
                    $("#dtInputToDate").val().length == 0*/) {

                    sErrMsg += "<li>Please specify the date from</li>";
                }

            }
            else {

                if ($("#dtInputFromDate2").val().length == 0 /*||
                    $("#dtInputToDate2").val().length == 0*/) {

                    sErrMsg += "<li>Please specify the date from</li>";
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

        function showDialog(sMessage)
        {
             $("#alertBox").html(sMessage);
             alertBox.dialog("open");

         }
         function populateConfirmSelection(sCurrentStatus) {

             $('#selResetStatus').html("");

             if (
                 (sCurrentStatus != "COMPLETE") ||
                 (sCurrentStatus != "IMPORTED") 
                
                ) {

                 $('#selResetStatus').append("<option value='READY'>READY</option>");
                 $('#selResetStatus').append("<option value='HOLD'>HOLD</option>");
             }
         }