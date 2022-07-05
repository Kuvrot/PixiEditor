﻿using PixiEditor.ChangeableDocument;
using PixiEditor.ViewModels.SubViewModels.Document;

namespace PixiEditor.Models.DocumentModels;

internal class DocumentHelpers
{
    public DocumentHelpers(DocumentViewModel doc)
    {
        Tracker = new DocumentChangeTracker();
        StructureHelper = new DocumentStructureHelper(doc, this);
        Updater = new DocumentUpdater(doc, this);
        ActionAccumulator = new ActionAccumulator(doc, this);
        State = new DocumentState();
    }
    public ActionAccumulator ActionAccumulator { get; }
    public DocumentChangeTracker Tracker { get; }
    public DocumentStructureHelper StructureHelper { get; }
    public DocumentUpdater Updater { get; }
    public DocumentState State { get; }
}
