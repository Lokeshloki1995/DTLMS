
function IsValidDate(idate) {
    var dateReg = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    if (dateReg.test(idate)) {
        return true;
    } else {
        return false;
    }
}

function ValidateDate(fromDate, currentDate) {
    var bResult;
    console.log(fromDate);
    bResult = IsValidDate(currentDate);
    if (bResult == false) {
        return false;
    }    
    
    var toDate = new Date();

    var dd = toDate.getDate();

    var mm = toDate.getMonth() + 1;
    var yyyy = toDate.getFullYear();  
    if(dd<10)   
    {  
        dd='0'+dd;
    }  
    if(mm<10)
    {  
        mm='0'+mm;  
    }
    toDate = dd + '/' + mm + '/' + yyyy;

    var d1 = fromDate.split("/");
    var d2 = toDate.split("/");
    var c = currentDate.split("/");

    var from = new Date(d1[2], parseInt(d1[1]) - 1, d1[0]);  // -1 because months are from 0 to 11
    var to = new Date(d2[2], parseInt(d2[1]) - 1, d2[0]);
    var check = new Date(c[2], parseInt(c[1]) - 1, c[0]);
    
    console.log("Date Validation");
    console.log(check >= from && check <= to)
    if (check >= from && check <= to) {
        return true;
    }
    else {
        return false;
    }
}