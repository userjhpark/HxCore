var cIconsPath = app.getPath("app", "root");
var iconPrintAll = util.iconStreamFromImage(cIconsPath + "/Resources/Icon-Print_All_24x24.png");
app.addToolButton({
    cName: "js.custom.PrintAll",   
    oIcon: iconPrintAll,
    cExec: "printAllOpenedDocs();",   
    cTooltext: "Print All Documents",   
    cEnable: true,
    nPos: 0  
});


function printAllOpenedDocs(){
    var ad = app.activeDocs;
    for (var i = 0; i < ad.length; i++)
    {
        var pp = ad[i].getPrintParams();
        // uncomment the next line to print without dialog for each document
        // pp.interactive = pp.constants.interactionLevel.silent;
        ad[i].print(pp);
    }
}