/* Yes, this is ugly, this was done in 2006 or 2007, before I knew proper
 * JavaScript. It's basically a copy and paste of some code from A List Apart.
 * If I were to rewrite this, it'd probably look nicer. :P
 *
 * Portions from http://www.alistapart.com/articles/alternate/
 */

function setActiveStyleSheet(title) {
  var i, a, main;
  for(i=0; (a = document.getElementsByTagName("link")[i]); i++) {
    if(a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title")) {
      a.disabled = true;
      if(a.getAttribute("title") == title) a.disabled = false;
    }
  }
}

function getActiveStyleSheet() {
  var i, a;
  for(i=0; (a = document.getElementsByTagName("link")[i]); i++) {
    if(a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title") && !a.disabled) return a.getAttribute("title");
  }
  return null;
}

function getPreferredStyleSheet() {
  var i, a;
  for(i=0; (a = document.getElementsByTagName("link")[i]); i++) {
    if(a.getAttribute("rel").indexOf("style") != -1
       && a.getAttribute("rel").indexOf("alt") == -1
       && a.getAttribute("title")
       ) return a.getAttribute("title");
  }
  return null;
}

function createCookie(name,value,days) {
  if (days) {
    var date = new Date();
    date.setTime(date.getTime()+(days*24*60*60*1000));
    var expires = "; expires="+date.toGMTString();
  }
  else expires = "";
  document.cookie = name+"="+value+expires+"; path=/";
}

function readCookie(name) {
  var nameEQ = name + "=";
  var ca = document.cookie.split(';');
  for(var i=0;i < ca.length;i++) {
    var c = ca[i];
    while (c.charAt(0)==' ') c = c.substring(1,c.length);
    if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
  }
  return null;
}

/*
window.onload = function(e) {
  var cookie = readCookie("style");
  var title = cookie ? cookie : getPreferredStyleSheet();
  setActiveStyleSheet(title);
}
*/

window.onunload = function(e) {
  var title = getActiveStyleSheet();
  createCookie("style3", title, 365);
}

var cookie = readCookie("style3");
var title = cookie ? cookie : 'Column on right'; //getPreferredStyleSheet();
setActiveStyleSheet(title);


/**************************************************/

function toggleStyleSheet()
{
	var current = getActiveStyleSheet();
	
	switch (current)
	{
		case 'Column on left':
			setActiveStyleSheet('No column');
			break;
		case 'Column on right':
			setActiveStyleSheet('Column on left');
			break;
		default:
			setActiveStyleSheet('Column on right');
			break;
	}
}

