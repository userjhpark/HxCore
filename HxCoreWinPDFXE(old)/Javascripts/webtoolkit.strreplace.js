/**
*
*  Javascript string replace
*  http://www.webtoolkit.info/
*
**/

// standart string replace functionality
function str_replace(haystack, needle, replacement) {
	var temp = haystack.split(needle);
	return temp.join(replacement);
}

// needle may be a regular expression
function str_replace_reg(haystack, needle, replacement) {
	var r = new RegExp(needle, 'g');
	return haystack.replace(r, replacement);
}