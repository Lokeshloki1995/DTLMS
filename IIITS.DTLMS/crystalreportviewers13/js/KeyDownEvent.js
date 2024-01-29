

function keydownfn(e, evntHdlrName, formName)
{	
    var nav4;
    var keyPressed;

    //check if the brower is Netscape Navigator 4 or not
    var nav4 = window.Event ? true : false;
	
	//if browser is Navigator 4, the key pressed is called <event object>.which else it's called <event object>.keyCode
	keyPressed = nav4 ? e.which : e.keyCode;
		
    if (keyPressed == 13)
    {	
        if (evntHdlrName != "")
		{	
			// append empty parentheses if none given
			if(evntHdlrName.substr(evntHdlrName.length-1) != ")")
				evntHdlrName += "()";
			eval(evntHdlrName);
        }
        else
        {
			if (formName == "")
				formName = 0;
            document.forms[formName].submit();
        }
    }
    return true;
}
