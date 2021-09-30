using Gabriel.Cat.S.Extension;
using PokemonGBAFramework.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PokePic
{
    public static class Extension
    {
        public static Paleta GetPaleta(this Bitmap bmp,int sizePalette = 16)
        {//CImage->GetColorTable así es como obtiene la paleta en PokePic!! si se puede se pone  en la DLL principal sino pues en esta dependiendo de las dependencias si incluye o no interficie
            Color[] palette;
            Color colorFondo = Color.FromArgb(255, 0, 0, 0);
            BitmapImage image2 = new BitmapImage();
            image2.BeginInit();
            image2.StreamSource = bmp.ToStream();
            image2.EndInit();
            palette=new BitmapPalette(image2, sizePalette).Colors.Select(c => c.ToDrawingColor()).ToArray();
            //palette.Invertir();
            return new Paleta(palette);
        }
    }
}
