using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Win.PDFXE
{
    public enum HxAnnotFormatType
    {
        FDF
        , XFDF
    }
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
        e_attachments_inserted,
        e_attachment_changed,
        e_attachments_deleted,
        e_uiLanguageChanged,

        _e_end_,

        _last_,
    };

    public enum HxPXC_AnnotFlag
    {
        AF_Invisible = 1,
        AF_Hidden = 2,
        AF_Print = 4,
        AF_NoZoom = 8,
        AF_NoRotate = 16,
        AF_NoView = 32,
        AF_ReadOnly = 64,
        AF_Locked = 128,
        AF_ToggleNoView = 256,
        AF_ContentLocked = 512,
    };

    public enum HxAnnotType
    {
        Link, Popup, Movie, Widget, Screen, PrinterMark, TrapNet, Watermark,
        n3D,
        RichMedia, Text, FreeText, Line, Square, Circle, Polygon, PolyLine,
        Highlight, Underline, Squiggly, StrikeOut, Stamp, Caret, Ink, FileAttachment,
        Sound, Redact, Projection
    }

}
