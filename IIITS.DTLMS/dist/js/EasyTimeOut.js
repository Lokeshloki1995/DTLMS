var myTimeOut;
var myVar = 0;
function SessionExpireAlert(timeout) {
    var seconds = timeout / 1000;

    document.getElementsByName("seconds").innerHTML = seconds;

    setInterval(function () {
        seconds--;
        document.getElementById("seconds").innerHTML = seconds;
    }, 1000);

    setTimeout(function () {
        //Show Popup for 10 seconds.
        $('#EASySessionTimeOutModal').modal({ backdrop: 'static', keyboard: false }); 
    }, timeout - 60 * 1000);

    myTimeOut = setTimeout(function () {
        window.location = "/Login";
    }, timeout);
}
function myStopFunction() {
    clearTimeout(myTimeOut);
}
function LogOut() {
    //Redirect to refresh Session.
    clearTimeout(myTimeOut);
    window.location = "/Login";
};