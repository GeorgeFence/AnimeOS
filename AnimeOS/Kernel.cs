using Cosmos.Core.Memory;
using Cosmos.System.Graphics;
using CosmosTTF;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using Sys = Cosmos.System;

namespace AnimeOS
{
    public class Kernel : Sys.Kernel
    {
        public VBECanvas canvas;
        public TTFFont defaultfont;
        public CGSSurface surface;
        public int PositionY = 1;

        public bool IsFileSystem = true;


        #region Fonts
        [ManifestResourceStream(ResourceName = "AnimeOS.consola.ttf")] public static byte[] FontConsolas;
        #endregion

        [ManifestResourceStream(ResourceName = "AnimeOS.wallpaper.bmp")] public static byte[] rawWallpaper;
        [ManifestResourceStream(ResourceName = "AnimeOS.logo_small.bmp")] public static byte[] rawLogoSmall;

        protected override void BeforeRun()
        {
            Mode boot = new Mode(1280,720,ColorDepth.ColorDepth32);
            canvas = (VBECanvas)FullScreenCanvas.GetFullScreenCanvas(boot);
            canvas.Clear(Color.Black);
            canvas.DrawImageAlpha(new Bitmap(rawLogoSmall), 1280 / 2 - 64, 720 / 4);
            canvas.Display();
            defaultfont = new TTFFont(FontConsolas);
            surface = new CGSSurface(canvas);
            defaultfont.DrawToSurface(surface, 16, 0, PositionY * 16, "Starting AnimeOS ", Color.White);
            PositionY++;
            canvas.Display();
            Thread.Sleep(500);
            if (IsFileSystem)
            {
                defaultfont.DrawToSurface(surface, 16, 0, PositionY * 16, "Initialising FileSystem", Color.Yellow);
                PositionY++;
                canvas.Display();
                Files.Initialize();
                defaultfont.DrawToSurface(surface, 16, 0, PositionY * 16, "Done", Color.Green);
                PositionY++;
                canvas.Display();
                Thread.Sleep(500);
            }
            canvas.Disable();
            canvas.SetMode(new Mode(1920,1080,ColorDepth.ColorDepth32));
        }

        protected override void Run()
        {
            canvas.DrawImage(new Bitmap(rawWallpaper),0,0);
            defaultfont.DrawToSurface(surface,8,100,100,new Bitmap(rawWallpaper).RawData.ToString(), Color.Red);
            canvas.Display();
            Heap.Collect();
        }
    }
}
