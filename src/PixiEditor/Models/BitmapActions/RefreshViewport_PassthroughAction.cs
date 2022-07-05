﻿using PixiEditor.ChangeableDocument.Actions;
using PixiEditor.ChangeableDocument.ChangeInfos;
using PixiEditor.Models.Position;

namespace PixiEditor.Models.BitmapActions;

internal record class RefreshViewport_PassthroughAction(ViewportInfo Info) : IAction, IChangeInfo;
