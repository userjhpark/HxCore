try {
	var selAntts = this.selectedAnnots;
	if (!selAntts) {
		app.alert('주석을 선택하지 않았습니다!\n(You have not selected an annotation)');
	}
	else {
		var annts = selAntts[0];
		var txt = annts.contents;

		var cResponse = app.response({
			cQuestion: '변경할 내용을 입력하세요!\n(Please enter your changes!)',
			cTitle: 'Change Comment Contents',
			cDefault: txt,
			cLabel: 'Contents :'
		});
		for (var i = 0; i < selAntts.length; i++) {
			annts = selAntts[i];
			if (cResponse) {
				annts.contents = cResponse;
			}
		}
	}
} catch (ex) {
	app.alert('주석을 선택하지 않았습니다!\n(You have not selected an annotation)\n\n' + ex);
}