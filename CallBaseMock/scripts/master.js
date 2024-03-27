var sWindow = null;
function ShowPage(posleft, postop, page, pageTitle, width, height) {

    if (!isWindowOpen())
        sWindow = open(page, pageTitle, "menubar=no,resizable=yes,scrollbars=1,width=" + width + ",height=" + height + ",toolbar=no,left=" + posleft + ",top=" + postop);

    else {
        sWindow.focus();
        return;
    } //else

} //ShowPage(posleft, postop, page, pageTitle, width, height)

function isWindowOpen() {
    if (sWindow && !sWindow.closed) {
        //	   alert("<%=oLang.GetLabel("WindowOpen")%>")
        return true;
    } //if (sWindow && !sWindow.closed)

    else {
        return false;
    } //else

} //isWindowOpen
