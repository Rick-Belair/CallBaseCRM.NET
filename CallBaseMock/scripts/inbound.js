function CheckIEVersion() {
    if (document.documentMode < 9) {
        window.location.replace("LegacyIE.aspx");
    }
}

function ShowOrderHistory() {
    var posleft = (screen.width - 400) / 2;
    var postop = (screen.height - 250) / 2;
    var page = "partials/order_history.aspx"
    var pageTitle = "OrderHistory";
    var width = 400;
    var height = 250;

    ShowPage(posleft, postop, page, pageTitle, width, height);

} //ShowOrderHistory

function addLabelFor(container, item) {
    $("#" + container.id).prepend("<label for='" + item.id + "' style='display: none'>Placeholder</label>");
}

function ShowScript() {
    var posleft = (screen.width - 400) / 2;
    var postop = (screen.height - 250) / 2;
    var page = "partials/view_script.aspx"
    var pageTitle = "Script";
    var width = 400;
    var height = 250;

    ShowPage(posleft, postop, page, pageTitle, width, height);

} //ShowScript

function ShowStatusReport() {
    var posleft = (screen.width - 500) / 2;
    var postop = (screen.height - 500) / 2;
    var page = "partials/status_report.aspx"
    var pageTitle = "StatusReport";
    var width = 750;
    var height = 500;

    ShowPage(posleft, postop, page, pageTitle, width, height);
} //ShowStatusReport



function setHighlightedText() {
    var txtIssues = $('#bodyContent_txtIssuesKB');
    var txtToSearch = (txtIssues.val()).substring(txtIssues[0].selectionStart, txtIssues[0].selectionEnd);
    $('#bodyContent_txtSearchKB').val(txtToSearch)

} //setHighlightedText

function EnterKeyFilter() {
    if (window.event.keyCode == 13) {
        event.returnValue = false;
        event.cancel = true;
    }
}

function updateProvState2DDL(ddlValue) {
    var provState2 = $("[id*='ddlProvState2']");
    provState2.val(ddlValue);
}

$('#bodyContent_txtLanguage').focusin(function () {
    var rblLang = $('#bodyContent_rblLanguage_2')
    rblLang.filter('[value=Other]').prop('checked', true);
});