using System;
using System.Collections.Generic;
using System.Drawing;
using Cosmos.Core.Multiboot;
using Cosmos.HAL.Drivers.Video;
using Cosmos.System.Graphics.Fonts;

namespace Cosmos.System.Graphics;

//
// Summary:
//     Defines a VBE (VESA Bios Extensions) canvas implementation. Please note that
//     this implementation of Cosmos.System.Graphics.Canvas only works on BIOS implementations,
//     meaning that it is not available on UEFI systems.
public class VBECanvas : Canvas
{
    private static readonly Mode defaultMode = new Mode(1024u, 768u, ColorDepth.ColorDepth32);

    private readonly VBEDriver driver;

    private Mode mode;

    public override Mode Mode
    {
        get
        {
            return mode;
        }
        set
        {
            mode = value;
            SetMode(mode);
        }
    }

    //
    // Summary:
    //     Available VBE supported video modes.
    //
    //     Low res:
    //
    //     • 320x240x32.
    //     • 640x480x32.
    //     • 800x600x32.
    //     • 1024x768x32.
    //
    //     HD:
    //
    //     • 1280x720x32.
    //     • 1280x1024x32.
    //
    //     HDR:
    //
    //     • 1366x768x32.
    //     • 1680x1050x32.
    //
    //     HDTV:
    //
    //     • 1920x1080x32.
    //     • 1920x1200x32.
    public override List<Mode> AvailableModes { get; } = new List<Mode>
    {
        new Mode(320u, 240u, ColorDepth.ColorDepth32),
        new Mode(640u, 480u, ColorDepth.ColorDepth32),
        new Mode(800u, 600u, ColorDepth.ColorDepth32),
        new Mode(1024u, 768u, ColorDepth.ColorDepth32),
        new Mode(1280u, 720u, ColorDepth.ColorDepth32),
        new Mode(1280u, 768u, ColorDepth.ColorDepth32),
        new Mode(1280u, 1024u, ColorDepth.ColorDepth32),
        new Mode(1366u, 768u, ColorDepth.ColorDepth32),
        new Mode(1680u, 1050u, ColorDepth.ColorDepth32),
        new Mode(1920u, 1080u, ColorDepth.ColorDepth32),
        new Mode(1920u, 1200u, ColorDepth.ColorDepth32)
    };


    public override Mode DefaultGraphicsMode => defaultMode;

    //
    // Summary:
    //     Initializes a new instance of the Cosmos.System.Graphics.VBECanvas class.
    public VBECanvas()
        : this(defaultMode)
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the Cosmos.System.Graphics.VBECanvas class.
    //
    // Parameters:
    //   mode:
    //     The display mode to use.
    public unsafe VBECanvas(Mode mode)
        : base(mode)
    {
        if (Multiboot2.IsVBEAvailable)
        {
            mode = new Mode(Multiboot2.Framebuffer->Width, Multiboot2.Framebuffer->Height, (ColorDepth)Multiboot2.Framebuffer->Bpp);
        }

        ThrowIfModeIsNotValid(mode);
        driver = new VBEDriver((ushort)mode.Width, (ushort)mode.Height, (ushort)mode.ColorDepth);
        Mode = mode;
    }

    public override void Disable()
    {
        driver.DisableDisplay();
    }

    public override string Name()
    {
        return "VBECanvas";
    }

    //
    // Summary:
    //     Sets the used display mode, disabling text mode if it is active.
    public void SetMode(Mode mode)
    {
        ThrowIfModeIsNotValid(mode);
        ushort xres = (ushort)Mode.Width;
        ushort yres = (ushort)Mode.Height;
        ushort bpp = (ushort)Mode.ColorDepth;
        driver.VBESet(xres, yres, bpp);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void Clear(int color)
    {
        throw new NotImplementedException();
    }

    public override void Clear(Color color)
    {
        base.Clear(color);
    }

    public override void DrawPoint(Color color, int x, int y)
    {
        throw new NotImplementedException();
    }

    public override void Display()
    {
        throw new NotImplementedException();
    }

    public override Color GetPointColor(int x, int y)
    {
        throw new NotImplementedException();
    }

    public override void DrawArray(Color[] colors, int x, int y, int width, int height)
    {
        base.DrawArray(colors, x, y, width, height);
    }

    public override void DrawLine(Color color, int x1, int y1, int x2, int y2)
    {
        base.DrawLine(color, x1, y1, x2, y2);
    }

    public override void DrawCircle(Color color, int xCenter, int yCenter, int radius)
    {
        base.DrawCircle(color, xCenter, yCenter, radius);
    }

    public override void DrawFilledCircle(Color color, int x0, int y0, int radius)
    {
        base.DrawFilledCircle(color, x0, y0, radius);
    }

    public override void DrawEllipse(Color color, int xCenter, int yCenter, int xR, int yR)
    {
        base.DrawEllipse(color, xCenter, yCenter, xR, yR);
    }

    public override void DrawFilledEllipse(Color color, int xCenter, int yCenter, int yR, int xR)
    {
        base.DrawFilledEllipse(color, xCenter, yCenter, yR, xR);
    }

    public override void DrawArc(int x, int y, int width, int height, Color color, int startAngle = 0, int endAngle = 360)
    {
        base.DrawArc(x, y, width, height, color, startAngle, endAngle);
    }

    public override void DrawPolygon(Color color, params Point[] points)
    {
        base.DrawPolygon(color, points);
    }

    public override void DrawSquare(Color color, int x, int y, int size)
    {
        base.DrawSquare(color, x, y, size);
    }

    public override void DrawRectangle(Color color, int x, int y, int width, int height)
    {
        base.DrawRectangle(color, x, y, width, height);
    }

    public override void DrawFilledRectangle(Color color, int xStart, int yStart, int width, int height)
    {
        base.DrawFilledRectangle(color, xStart, yStart, width, height);
    }

    public override void DrawTriangle(Color color, int v1x, int v1y, int v2x, int v2y, int v3x, int v3y)
    {
        base.DrawTriangle(color, v1x, v1y, v2x, v2y, v3x, v3y);
    }

    public override void DrawImage(Image image, int x, int y)
    {
        base.DrawImage(image, x, y);
    }

    public override void DrawImage(Image image, int x, int y, int w, int h)
    {
        base.DrawImage(image, x, y, w, h);
    }

    public override void DrawString(string str, Font font, Color color, int x, int y)
    {
        base.DrawString(str, font, color, x, y);
    }

    public override void DrawChar(char c, Font font, Color color, int x, int y)
    {
        base.DrawChar(c, font, color, x, y);
    }
}