﻿using ChunkyImageLib;
using ChunkyImageLib.DataHolders;
using PixiEditor.DrawingApi.Core.ColorsImpl;
using PixiEditor.DrawingApi.Core.Numerics;
using PixiEditor.DrawingApi.Core.Surface;
using Xunit;

namespace ChunkyImageLibTest;
public class ChunkyImageTests
{
    public static Surface ImportImage(string path, VecI size)
    {
        Surface original = Surface.Load(path);
        if (original.Size != size)
        {
            Surface resized = original.ResizeNearestNeighbor(size);
            original.Dispose();
            return resized;
        }
        return original;
    }

    [Fact]
    public void LoadDemo()
    {
        var path = @"C:\Users\egor0\Desktop\SpazzS1.png";
        ImportImage(path, new VecI(5, 5));
    }

    [Fact]
    public void Dispose_ComplexImage_ReturnsAllChunks()
    {
        ChunkyImage image = new ChunkyImage(new(ChunkyImage.FullChunkSize, ChunkyImage.FullChunkSize));
        image.EnqueueDrawRectangle(new(new(5, 5), new(80, 80), 0, 2, Colors.AliceBlue, Colors.Snow));
        using (Chunk target = Chunk.Create())
        {
            image.DrawMostUpToDateChunkOn(new(0, 0), ChunkResolution.Full, target.Surface.DrawingSurface, VecI.Zero);
            image.CancelChanges();
            image.EnqueueResize(new(ChunkyImage.FullChunkSize * 4, ChunkyImage.FullChunkSize * 4));
            image.EnqueueDrawRectangle(new(VecD.Zero, image.CommittedSize, 0, 2, Colors.AliceBlue, Colors.Snow, BlendMode.Multiply));
            image.CommitChanges();
            image.SetBlendMode(BlendMode.Overlay);
            image.EnqueueDrawRectangle(new(VecD.Zero, image.CommittedSize, 0, 2, Colors.AliceBlue, Colors.Snow, BlendMode.Multiply));
            image.EnqueueDrawRectangle(new(VecD.Zero, image.CommittedSize, 0, 2, Colors.AliceBlue, Colors.Snow));
            image.CommitChanges();
            image.SetBlendMode(BlendMode.Screen);
            image.EnqueueDrawRectangle(new(VecD.Zero, image.CommittedSize, 0, 2, Colors.AliceBlue, Colors.Snow));
            image.CancelChanges();
            image.SetBlendMode(BlendMode.SrcOver);
            image.EnqueueDrawRectangle(new(VecD.Zero, image.CommittedSize, 0, 2, Colors.AliceBlue, Colors.Snow));
        }
        image.Dispose();

        Assert.Equal(0, Chunk.ChunkCounter);
    }

    [Fact]
    public void GetCommittedPixel_RedImage_ReturnsRedPixel()
    {
        const int chunkSize = ChunkyImage.FullChunkSize;
        ChunkyImage image = new ChunkyImage(new VecI(chunkSize * 2));
        image.EnqueueDrawRectangle
            (new ShapeData(new VecD(chunkSize), new VecD(chunkSize * 2), 0, 0, Colors.Transparent, Colors.Red));
        image.CommitChanges();
        Assert.Equal(Colors.Red, image.GetCommittedPixel(new VecI(chunkSize + chunkSize / 2)));
        image.Dispose();
        Assert.Equal(0, Chunk.ChunkCounter);
    }

    [Fact]
    public void GetMostUpToDatePixel_BlendModeSrc_ReturnsCorrectPixel()
    {
        const int chunkSize = ChunkyImage.FullChunkSize;
        ChunkyImage image = new ChunkyImage(new VecI(chunkSize * 2));
        image.EnqueueDrawRectangle
            (new ShapeData(new VecD(chunkSize), new VecD(chunkSize * 2), 0, 0, Colors.Transparent, Colors.Red));
        Assert.Equal(Colors.Red, image.GetMostUpToDatePixel(new VecI(chunkSize + chunkSize / 2)));
        image.Dispose();
        Assert.Equal(0, Chunk.ChunkCounter);
    }
    
    [Fact]
    public void GetMostUpToDatePixel_BlendModeSrcOver_ReturnsCorrectPixel()
    {
        const int chunkSize = ChunkyImage.FullChunkSize;
        ChunkyImage image = new ChunkyImage(new VecI(chunkSize * 2));
        image.EnqueueDrawRectangle
            (new ShapeData(new VecD(chunkSize), new VecD(chunkSize * 2), 0, 0, Colors.Transparent, Colors.Red));
        image.CommitChanges();
        image.SetBlendMode(BlendMode.SrcOver);
        image.EnqueueDrawRectangle(new ShapeData(
            new VecD(chunkSize),
            new VecD(chunkSize * 2),
            0, 
            0,
            Colors.Transparent,
            new Color(0, 255, 0, 128)));
        Assert.Equal(new Color(127, 128, 0), image.GetMostUpToDatePixel(new VecI(chunkSize + chunkSize / 2)));
        image.Dispose();
        Assert.Equal(0, Chunk.ChunkCounter);
    }

    [Fact]
    public void EnqueueDrawRectangle_OutsideOfImage_PartsAreNotDrawn()
    {
        const int chunkSize = ChunkyImage.FullChunkSize;
        using ChunkyImage image = new(new VecI(chunkSize));
        image.EnqueueDrawRectangle(new ShapeData(
                VecD.Zero,
                new VecD(chunkSize * 10),
                0,
                0,
                Colors.Transparent,
                Colors.Red));
        image.CommitChanges();
        Assert.Collection(
            image.FindAllChunks(), 
            elem => Assert.Equal(VecI.Zero, elem));
    }
}
