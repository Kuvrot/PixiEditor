﻿using ChunkyImageLib.DataHolders;
using System.Windows;
using PixiEditor.DrawingApi.Core.Numerics;

namespace PixiEditor.Zoombox;

public class ViewportRoutedEventArgs : RoutedEventArgs
{
    public ViewportRoutedEventArgs(RoutedEvent e, VecD center, VecD size, VecD realSize, double angle) : base(e)
    {
        Center = center;
        Size = size;
        RealSize = realSize;
        Angle = angle;
    }

    public VecD Center { get; }
    public VecD Size { get; }
    public VecD RealSize { get; }
    public double Angle { get; }
}
