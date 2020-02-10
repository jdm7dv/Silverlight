/** DOMMouseScroll is for mozilla. */
if (window.addEventListener)
{
	window.addEventListener('DOMMouseScroll', onMouseWheel, false);
}
else {
    window.onmousewheel = document.onmousewheel = onMouseWheel;
}

/** Event handler for mouse wheel event.
 */
function onMouseWheel(event)
{

    if (!event) /* For IE. */
	    event = window.event;
	
    var delta = 0;
    var x = (event.clientX);
    var y = (event.clientY);
	

    if (event.wheelDelta) { /* IE/Opera. */
		    delta = event.wheelDelta/120;   
		    /** In Opera 9, delta differs in sign as compared to IE.
		     */
		    if (window.opera)
				    delta = -delta;
    } else if (event.detail) { /** Mozilla case. */
		    /** In Mozilla, sign of delta is different than in IE.
		     * Also, delta is multiple of 3.
		     */
		    delta = -event.detail/3;
		    x /= 15;
		    y /= 15;
    }
	
    /** If delta is nonzero, handle it.
     * Basically, delta is now positive if wheel was scrolled up,
     * and negative, if wheel was scrolled down.
     */
    if (delta) {
        document.getElementById("<id>").content.Application.ScriptScrollChangedCallback(x, y, delta);
    }
		    //alert(delta + x + y);
    /** Prevent default actions caused by mouse wheel.
     * That might be ugly, but we handle scrolls somehow
     * anyway, so don't bother here..
     */
    if (event.preventDefault)
		    event.preventDefault();
	
    event.returnValue = false;
}