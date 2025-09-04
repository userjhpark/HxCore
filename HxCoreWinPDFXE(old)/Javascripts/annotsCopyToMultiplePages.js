//var cIconsPath = app.getPath('app', 'root');
//var iconPrintAll = util.iconStreamFromImage(cIconsPath + '/Resources/Icon-CopyAnnotsToPages_24x24.png');
//app.addToolButton({
//    cName: 'js.custom.CopyAnnotsToPages',
//    oIcon: iconPrintAll,
//    cExec: 'CopyAnnotsToPages();',
//    cTooltext: 'Copy Comment To Pages',
//    cEnable: true,
//    nPos: 0
//});

CopyAnnotsToPages();

function CopyAnnotsToPages() {
	//참고: <https://forum.tracker-software.com/viewtopic.php?f=62&t=30092&p=118915&hilit=stamp+multiple+page#p118915>
    
    //Title: COPY ANNOTATION TO USER DEFINED PAGE RANGE
    //Author:  John Statler
    //Purpose: Prompt user to enter required page
    //         range to copy annotation to other pages, e.g. 1-5,7,9-20
    
	//Get user response
	var selAnnts = null;
	try {
		selAnnts = this.selectedAnnots;
	} catch (e) {
		selAnnts = null;
		//app.alert('복제 할 주석을 선택하지 않았습니다!\n(You have not selected an annotation to duplicate)\n\n' + e);
	}
	if (!selAnnts || selAnnts.length <= 0) {
		app.alert('복제 할 주석을 선택하지 않았습니다!\n(You have not selected an annotation to duplicate)');
	}
	else {
		try {
			var nSelAnnts = selAnnts.length;

			if (nSelAnnts > 0) {
				var annt = this.selectedAnnots[0];
				var props = annt.getProps();
				var pagesAll = '';

				var currPageIndex = props.page;
				var currPageNum = currPageIndex + 1;
				var totalPageNum = this.numPages;
				var totalPageIndex = totalPageNum - 1;
				//app.alert(currPageIndex + ' / ' + totalPageIndex);

				if (currPageIndex >= totalPageIndex) {
					app.alert('복제할 페이지와 마지막 페이지가 같습니다!(Source Page is Last Page Equals)');
				}
				else {
					app.alert('본 작업은 실행취소(UNDO, Ctrl + Z)가 지원되지 않습니다.\n\n사용에 주의를 요구합니다.\n\n(Undo (UNDO, Ctrl + Z) is not supported for this operation. It requires careful use.)');
					var cResponse = app.response({
						cQuestion: '선택한 주석을 반복할 페이지를 입력하세요,\n(Enter in the pages where you wish to repeat the comment)\n' +
							'ex) 1,5,10-19,25\n\n' +
							//'All pages are chosen by default.\n\n' +
							//'For large documents it may take a second to do 8 pages.\n' +
							//'So 120 pages would take 15 seconds to finish.\n\n' +
							'세로 스탬프의 경우 먼저 문서의 방향과 스탬프의 방향을 먼저 확인후 적용 하세요.\n' +
							'(For vertical stamps, first rotate the document, apply stamp, then rotate back)\n\n' +
							'본 작업은 실행취소(UNDO, Ctrl + Z)가 지원되지 않습니다.\n' +
							'(Undo (UNDO, Ctrl + Z) is not supported for this operation.)'
						,
						cTitle: 'Copy To Pages',
						cDefault: currPageNum + '-' + totalPageNum,
						cLabel: 'Pages:'
					});


					if (!cResponse) {
						//app.alert('입력 한 페이지가 없습니다.\n(No pages entered)');
					}
					else {
						app.alert('복제할 페이지와 마지막 페이지가 같습니다!(Source Page is Last Page Equals)');
						var d1 = new Date();
						//var anntPage = this.pageNum + 1;
						var strInput = cResponse;
						var strChar;
						var arPrint = new Array(10);
						var arCount = 0;
						arPrint[arCount] = '';

						for (var i = 0; i < strInput.length; i++) {

							strChar = strInput.substr(i, 1);

							//Check character and form page group
							if (IsInteger(strChar) == 0) {
								arPrint[arCount] = arPrint[arCount] + strChar;
							}

							if (IsDash(strChar) == 0) {
								arPrint[arCount] = arPrint[arCount] + strChar;
							}

							if (IsComma(strChar) == 0) {
								arCount++;
								arPrint[arCount] = '';
							}

						}

						for (i = 0; i < (arCount + 1); i++) {

							if (arPrint[i].indexOf('-') > 0) {
								var dashPos;
								dashPos = (arPrint[i].indexOf('-'));

								var pageStart = arPrint[i].substr(0, dashPos);
								var pageEnd = arPrint[i].substr(arPrint[i].indexOf('-') + 1,
									(arPrint[i].length - dashPos + 1));
								pagesAll = pagesAll + range(Number(pageStart), pageEnd - pageStart + 1) + ',';
							} else {
								pagesAll = pagesAll + arPrint[i] + ',';
							}
						}
						pagesAll = pagesAll.replace(/,\s *$/, '');
						var arPage = pagesAll.split(',');
						//app.alert( pagesAll + ' / ' + (arPage.length - 1));
						var nArPage = arPage.length - 1;
						for (var k = 0; k < nSelAnnts; k++) {
							props = selAnnts[k].getProps();

							for (var i = 0; i < nArPage; i++) {
								var iPageNum = arPage[i];
								var iPageIndex = arPage[i] - 1;

								props.page = iPageIndex;
								if (props != null) {
									if (props.page != currPageIndex) {
										this.addAnnot(props);
										//for ( o in props ) console.println( o + ' : ' + props[o] ); 
									}
								}
							}
						}

						var d2 = new Date();
						var SecsElapsed = (d2 - d1) / 1000;
						var SecsElapsed2 = SecsElapsed.toFixed(0);
						var MinsElapsed2 = SecsElapsed2 / 60;
						app.alert('완료! / 경과 시간(Minutes elapsed) : ' + MinsElapsed2.toFixed(2));
					}
				}
			}
		} catch (ex) {
			app.alert(ex);
		}
	}
}

function range(start, count) {
	return Array.apply(0, Array(count))
		.map(function (element, index) {
			return index + start;
		});
}

function IsComma(strChar) {

	if (strChar == ',') {
		return 0;
	}
	else {
		return -1;
	}
}

function IsSpace(strChar) {
	if (strChar == ' ') {
		return 0;
	}
	else {
		return -1;
	}
}

function IsDash(strChar) {
	if (strChar == '-') {
		return 0;
	}
	else {
		return -1;
	}
}

function IsInteger(strChar) {
	if (strChar >= 0 || strChar <= 9) {
		return 0;
	}
	else {
		return -1;
	}
}