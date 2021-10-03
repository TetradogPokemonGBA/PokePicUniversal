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
        public MainWindow Main { get; set; }
       
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
                imgFrontNormal.Tag = Pokemon.Sprites.Frontales.Sprites[picFront];
                imgFrontNormal.SetImage(bmp);

                bmp = Pokemon.Sprites.Frontales.Sprites[picFront] + Pokemon.Sprites.PaletaShiny;
                imgFrontShiny.Tag = Pokemon.Sprites.Frontales.Sprites[picFront];
                imgFrontShiny.SetImage(bmp);


                bmp = Pokemon.Sprites.Traseros.Sprites[picBack] + Pokemon.Sprites.PaletaNomal;
                imgBackNormal.Tag = Pokemon.Sprites.Traseros.Sprites[picBack];
                imgBackNormal.SetImage(bmp);

                bmp = Pokemon.Sprites.Traseros.Sprites[picBack] + Pokemon.Sprites.PaletaShiny;
                imgBackShiny.Tag = Pokemon.Sprites.Traseros.Sprites[picBack];
                imgBackShiny.SetImage(bmp);

            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Pic++;
            Refresh();

        }
        public void ImportOrExport(Sprites.Sprite sprite,bool? isImport=default,int frame = -1)
        {
            System.Windows.Controls.Image blSelected;
            bool importOn = ImportOn;
            int frameAct = Pic;

            if(frame>0)
                Pic = frame;
            if(isImport.HasValue)
                ImportOn = isImport.Value;

            if (sprite.HasFlag(Sprites.Sprite.Shiny))
            {
                if (sprite.HasFlag(Sprites.Sprite.Frontal))
                {
                    blSelected = imgFrontShiny;
                }
                else
                {
                    blSelected = imgBackShiny;
                }
            }
            else
            {
                if (sprite.HasFlag(Sprites.Sprite.Frontal))
                {
                    blSelected = imgFrontNormal;
                }
                else
                {
                    blSelected = imgBackNormal;
                }
            }

            img_MouseRightButtonDown(blSelected);


            ImportOn = importOn;
            Pic = frameAct;
        }

        private void img_MouseRightButtonDown(object sender, MouseButtonEventArgs e=default)
        {
            OpenFileDialog opnFileDialog;
            SaveFileDialog saveFileDialog;

            BloqueImagen blImg;

            int picFront;
            int picBack;

            if (!ImportOn)
            {
                blImg = ((System.Windows.Controls.Image)sender).Tag as BloqueImagen;
                if (!Equals(blImg, default))
                {
                    saveFileDialog = new SaveFileDialog();
                    saveFileDialog.DefaultExt = "bmp";
                    saveFileDialog.FileName = $"{Pokemon.Nombre}_{DateTime.UtcNow.Ticks} ";
                    if (saveFileDialog.ShowDialog().GetValueOrDefault())
                    {
                        blImg.ExportToBMP(saveFileDialog.FileName);
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
                    blImg = new Bitmap(opnFileDialog.FileName).ToBloqueImagen();

                    //quiere actualizar el pic
                    if (ReferenceEquals(sender, imgBackShiny))
                    {
                        Pokemon.Sprites.Traseros.Sprites[picBack].DatosDescomprimidos.Bytes = blImg.DatosDescomprimidos.Bytes;
                        Pokemon.Sprites.PaletaShiny = new PaletaShiny() { Paleta = blImg.Paletas[0] };
                    }
                    else if (ReferenceEquals(sender, imgBackNormal))
                    {
                        Pokemon.Sprites.Traseros.Sprites[picBack].DatosDescomprimidos.Bytes = blImg.DatosDescomprimidos.Bytes;
                        Pokemon.Sprites.PaletaNomal = new PaletaNormal() { Paleta = blImg.Paletas[0] };
                    }
                    else if (ReferenceEquals(sender, imgFrontNormal))
                    {
                        Pokemon.Sprites.Frontales.Sprites[picFront].DatosDescomprimidos.Bytes = blImg.DatosDescomprimidos.Bytes;
                        Pokemon.Sprites.PaletaNomal = new PaletaNormal() { Paleta = blImg.Paletas[0] };
                    }
                    else if (ReferenceEquals(sender, imgFrontShiny))
                    {
                        Pokemon.Sprites.Frontales.Sprites[picFront].DatosDescomprimidos.Bytes = blImg.DatosDescomprimidos.Bytes;
                        Pokemon.Sprites.PaletaShiny = new PaletaShiny() { Paleta = blImg.Paletas[0] };
                    }
                    Refresh();
                    //save rom
                    Sprites.Set(Main.Rom, Pokemon);
                    Main.Save();
                }
            }

        }

        private void swImportOrExport_Changed(object sender=default, EventArgs e=default)
        {
            if (swImportOrExport.EstaOn)
            {
                swImportOrExport.Label.Text = "Importar al hacer clic derecho";
                swImportOrExport.BrushOn = System.Windows.Media.Brushes.Salmon;
            }
            else
            {
                swImportOrExport.Label.Text = "Exportar al hacer clic derecho";
                swImportOrExport.BrushOn = System.Windows.Media.Brushes.GreenYellow;
            }
        }
    }
}
