using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PokemonGBAFramework.Core;
using Gabriel.Cat.S.Extension;
using System.Drawing;
using Microsoft.Win32;
using PokemonGBAFramework.Core.Extension;

namespace PokePic
{
    /// <summary>
    /// Lógica de interacción para VisorSprites.xaml
    /// </summary>
    public partial class VisorSprites : UserControl
    {
        public VisorSprites()
        {
            InitializeComponent();
            Pic = 0;
            swImportOrExport.Label.TextAlignment = TextAlignment.Center;
            ImportOn = false;
        }
        public Pokemon Pokemon { get; set; }
        public int Pic { get; set; }
        public bool ImportOn
        {
            get => swImportOrExport.EstaOn;
            set { 
                swImportOrExport.EstaOn = value;
                swImportOrExport_Changed();
            }
        }
        public void Refresh()
        {
            Bitmap bmp;
            int picFront = Pic;
            int picBack = Pic;
            if (!Equals(Pokemon, default))
            {
                if (picFront >= Pokemon.Sprites.Frontales.Sprites.Count)
                {
                    picFront = 0;
                    picBack = 0;
                    Pic = picFront;

                }
                else if (picFront < 0)
                {
                    picFront = Pokemon.Sprites.Frontales.Sprites.Count - 1;
                    Pic = picFront;
                }

                if (picBack >= Pokemon.Sprites.Traseros.Sprites.Count)
                {
                    picBack = 0;
                }
                else if (picBack < 0)
                {
                    picBack = Pokemon.Sprites.Traseros.Sprites.Count - 1;
                }

                bmp = Pokemon.Sprites.Frontales.Sprites[picFront] + Pokemon.Sprites.PaletaNomal;
                imgFrontNormal.Tag = bmp;
                imgFrontNormal.SetImage(bmp);

                bmp = Pokemon.Sprites.Frontales.Sprites[picFront] + Pokemon.Sprites.PaletaShiny;
                imgFrontShiny.Tag = bmp;
                imgFrontShiny.SetImage(bmp);


                bmp = Pokemon.Sprites.Traseros.Sprites[picBack] + Pokemon.Sprites.PaletaNomal;
                imgBackNormal.Tag = bmp;
                imgBackNormal.SetImage(bmp);

                bmp = Pokemon.Sprites.Traseros.Sprites[picBack] + Pokemon.Sprites.PaletaShiny;
                imgBackShiny.Tag = bmp;
                imgBackShiny.SetImage(bmp);

            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Pic++;
            Refresh();

        }

        private void img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog opnFileDialog;
            SaveFileDialog saveFileDialog;
            Bitmap bmp;
            int picFront;
            int picBack;

            if (!ImportOn)
            {
                bmp = ((System.Windows.Controls.Image)sender).Tag as Bitmap;
                if (!Equals(bmp, default))
                {
                    saveFileDialog = new SaveFileDialog();
                    saveFileDialog.DefaultExt = "png";
                    saveFileDialog.FileName = $"{Pokemon.Nombre}_{DateTime.UtcNow.Ticks} ";
                    if (saveFileDialog.ShowDialog().GetValueOrDefault())
                    {
                        bmp.Save(saveFileDialog.FileName);
                    }
                }
            }
            else
            {
                opnFileDialog = new OpenFileDialog();
                picFront = Pic;
                picBack = Pic;
                if (picFront >= Pokemon.Sprites.Frontales.Sprites.Count)
                {
                    picFront = 0;
                    picBack = 0;
                    Pic = picFront;

                }
                else if (picFront < 0)
                {
                    picFront = Pokemon.Sprites.Frontales.Sprites.Count - 1;
                    Pic = picFront;
                }

                if (picBack >= Pokemon.Sprites.Traseros.Sprites.Count)
                {
                    picBack = 0;
                }
                else if (picBack < 0)
                {
                    picBack = Pokemon.Sprites.Traseros.Sprites.Count - 1;
                }
                if (opnFileDialog.ShowDialog().GetValueOrDefault())
                {
                    bmp = new Bitmap(opnFileDialog.FileName);
                    //quiere actualizar el pic
                    if (ReferenceEquals(sender, imgBackShiny))
                    {
                        //Pokemon.Sprites.Traseros.Sprites[picBack].DatosDescomprimidos.Bytes = BloqueImagen.GetDatosDescomprimidos(bmp);
                        Pokemon.Sprites.PaletaShiny = new PaletaShiny() { Paleta = bmp.GetPaleta() };
                    }
                    else if (ReferenceEquals(sender, imgBackNormal))
                    {
                        //Pokemon.Sprites.Traseros.Sprites[picBack].DatosDescomprimidos.Bytes = BloqueImagen.GetDatosDescomprimidos(bmp);
                        Pokemon.Sprites.PaletaNomal = new PaletaNormal() { Paleta = bmp.GetPaleta() };
                    }
                    else if (ReferenceEquals(sender, imgFrontNormal))
                    {
                        //Pokemon.Sprites.Frontales.Sprites[picBack].DatosDescomprimidos.Bytes = BloqueImagen.GetDatosDescomprimidos(bmp);
                        Pokemon.Sprites.PaletaNomal = new PaletaNormal() { Paleta = bmp.GetPaleta() };
                    }
                    else if (ReferenceEquals(sender, imgFrontShiny))
                    {
                        //Pokemon.Sprites.Frontales.Sprites[picBack].DatosDescomprimidos.Bytes = BloqueImagen.GetDatosDescomprimidos(bmp);
                        Pokemon.Sprites.PaletaShiny = new PaletaShiny() { Paleta =  bmp.GetPaleta() };
                    }
                    Refresh();
                }
            }

        }

        private void swImportOrExport_Changed(object sender=default, EventArgs e=default)
        {
            if (swImportOrExport.EstaOn)
            {
                swImportOrExport.Label.Text = "Importar al hacer click derecho";
                swImportOrExport.BrushOn = System.Windows.Media.Brushes.Salmon;
            }
            else
            {
                swImportOrExport.Label.Text = "Exportar al hacer click derecho";
                swImportOrExport.BrushOn = System.Windows.Media.Brushes.GreenYellow;
            }
        }
    }
}
