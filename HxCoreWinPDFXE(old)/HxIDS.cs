using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win.PDFXE
{
    public enum HxIDS
    {
        // cmdbars
        cmdbar_menubar,
        cmdbar_standard,
        cmdbar_file,
        cmdbar_view,
        cmdbar_pageZoom,
        cmdbar_pageNav,
        cmdbar_contentEditing,
        cmdbar_pageLayout,
        cmdbar_docOptions,
        cmdbar_commenting,
        cmdbar_measurement,
        cmdbar_properties,
        cmdbar_launchApp,
        cmdbar_addon,
        cmdbar_form,

        // panes/views
        pageThumbnailsView,
        bookmarksView,
        contentsView,
        attachmentsView,
        signaturesView,
        commentsView,
        layersView,
        pdfNamedDestsView,
        propertiesView,
        searchView,
        stampsView,
        commentStylesView,
        panzoomView,

        _op_begin_,

        // print
        op_document_printPages,

        // new doc
        op_newBlankDoc,
        op_imagesToDoc,
        op_textToDoc,
        op_combineDocs,

        // pages
        op_document_insertPages,
        op_document_insertEmptyPages,
        op_document_deletePages,
        op_document_extractPages,
        op_document_replacePages,
        op_document_cropPages,
        op_document_resizePages,
        op_document_addWatermarks,

        // export comments & fields
        op_document_summarizeAnnots,
        op_document_exportCommentsAndFields,

        // import comments & fields
        op_document_importCommentsAndFields,

        // export
        op_document_exportToImages,

        _op_end_,

        _e_begin_,

        // events
        e_activeDocChanged,
        e_document_modStateChanged,
        e_document_sourceChanged,
        e_pagesView_endLayoutChanging,
        e_uiLanguageChanged,

        _e_end_,

        _last_,
    };
}
