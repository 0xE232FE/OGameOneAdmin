// ==UserScript==
// @name           OGameOneAdmin Import Cookies
// @namespace      ogame.gameforge.celestos.net
// @description    Import session cookies from OGameOneAdmin tool to Firefox/Chrome
// @include        http*://*/load/cookies/v*/*/redir/*
// @version        2012.04.07
// ==/UserScript==
//** Copyright © 2012 by vodler **
//** May only be used by ogame staff. If you are not ogame staff, then delete this script immediately. **
//** Obtain permission before redistributing. **
(function() {
function createCookie(name, value, days) {
	var expires = "";
	var date;
	if (days) {
		date = new Date();
		date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
		expires = "; expires=" + date.toGMTString();
	}
	document.cookie = name + "=" + value + expires + "; path=/";
}
var Base64 = {
	// private property
	_keyStr : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
	// public method for encoding
	encode : function (input) {
		var output = "";
		var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
		var i = 0;
 
		input = Base64._utf8_encode(input);
 
		while (i < input.length) {
 
			chr1 = input.charCodeAt(i++);
			chr2 = input.charCodeAt(i++);
			chr3 = input.charCodeAt(i++);
 
			enc1 = chr1 >> 2;
			enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
			enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
			enc4 = chr3 & 63;
 
			if (isNaN(chr2)) {
				enc3 = enc4 = 64;
			} else if (isNaN(chr3)) {
				enc4 = 64;
			}
 
			output = output +
			this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
			this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);
 
		}
 
		return output;
	},
	// public method for decoding
	decode : function (input) {
		var output = "";
		var chr1, chr2, chr3;
		var enc1, enc2, enc3, enc4;
		var i = 0;
 
		input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
 
		while (i < input.length) {
 
			enc1 = this._keyStr.indexOf(input.charAt(i++));
			enc2 = this._keyStr.indexOf(input.charAt(i++));
			enc3 = this._keyStr.indexOf(input.charAt(i++));
			enc4 = this._keyStr.indexOf(input.charAt(i++));
 
			chr1 = (enc1 << 2) | (enc2 >> 4);
			chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
			chr3 = ((enc3 & 3) << 6) | enc4;
 
			output = output + String.fromCharCode(chr1);
 
			if (enc3 != 64) {
				output = output + String.fromCharCode(chr2);
			}
			if (enc4 != 64) {
				output = output + String.fromCharCode(chr3);
			}
 
		}
 
		output = Base64._utf8_decode(output);
 
		return output;
 
	},
	// private method for UTF-8 encoding
	_utf8_encode : function (string) {
		string = string.replace(/\r\n/g,"\n");
		var utftext = "";
 
		for (var n = 0; n < string.length; n++) {
 
			var c = string.charCodeAt(n);
 
			if (c < 128) {
				utftext += String.fromCharCode(c);
			}
			else if((c > 127) && (c < 2048)) {
				utftext += String.fromCharCode((c >> 6) | 192);
				utftext += String.fromCharCode((c & 63) | 128);
			}
			else {
				utftext += String.fromCharCode((c >> 12) | 224);
				utftext += String.fromCharCode(((c >> 6) & 63) | 128);
				utftext += String.fromCharCode((c & 63) | 128);
			}
 
		}
 
		return utftext;
	},
	// private method for UTF-8 decoding
	_utf8_decode : function (utftext) {
		var string = "";
		var i = 0;
		var c = c1 = c2 = 0;
 
		while ( i < utftext.length ) {
 
			c = utftext.charCodeAt(i);
 
			if (c < 128) {
				string += String.fromCharCode(c);
				i++;
			}
			else if((c > 191) && (c < 224)) {
				c2 = utftext.charCodeAt(i+1);
				string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
				i += 2;
			}
			else {
				c2 = utftext.charCodeAt(i+1);
				c3 = utftext.charCodeAt(i+2);
				string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
				i += 3;
			}
 
		}
		return string;
	}
}
document.getElementsByTagName('title')[0].innerHTML = "Loading";
document.getElementsByTagName('h1')[0].innerHTML = "Please wait, loading...";

var url = document.URL.substr(0, decodeURI(document.URL).indexOf('load/cookies/'));
url = url + document.URL.substr(decodeURI(document.URL).indexOf('/redir/') + 7);

document.getElementsByTagName('p')[0].innerHTML = "If you are not redirected to " + url + " within 5 seconds, contact <a href='mailto:vodler@gmail.com'>vodler</a>.";

if (decodeURI(document.URL).indexOf('load/cookies/v1') == -1)
{
	document.getElementsByTagName('h1')[0].innerHTML = "Error, you must update OGameOneAdmin Import Cookies (grease monkey script)";
	document.getElementsByTagName('p')[0].innerHTML = "A new version  is available at <a href='http://ogame.celestos.net'>http://ogame.celestos.net</a>.";
}
else
{
	try
	{
		var cookies = Base64.decode(document.URL.substr(decodeURI(document.URL).indexOf('load/cookies/v1/') + 16, decodeURI(document.URL).indexOf('/redir/') - (decodeURI(document.URL).indexOf('load/cookies/v1/') + 16)));

		var cookieList = cookies.split("|");

		for (cookie in cookieList)
		{
			if (cookieList[cookie].length > 0)
			{
				createCookie(cookieList[cookie].split("=")[0], cookieList[cookie].split("=")[1], 30);
			}
		}
	}
	catch(err)
	{
	  var txt = "There was an error on this page.\n\n";
	  txt += "Error description: " + err.message + "\n\n";
	  txt += "Click OK to continue.\n\n";
	  alert(txt);
	}
	document.location.href = url;
}
})();