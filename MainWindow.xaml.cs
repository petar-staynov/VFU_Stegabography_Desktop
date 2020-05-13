using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using VFU_Stegabography_Desktop.Helpers;
using Image = System.Drawing.Image;

namespace VFU_Stegabography_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage DisplayImage;
        private Image LoadedImage;
        private SteganographyHelper SteganographyHelper;
        private ImageConverterCustom ImageConverter;
        private CryptographyHelper CryptographyHelper;

        public MainWindow()
        {
            InitializeComponent();
            this.DisplayImage = new BitmapImage();
            this.LoadedImage = null;
            this.SteganographyHelper = new SteganographyHelper();
            this.ImageConverter = new ImageConverterCustom();
            this.CryptographyHelper = new CryptographyHelper();
        }

        private void BrowseButtonHandler(object sender, RoutedEventArgs e)
        {
            /****** Open and read file ********/
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            dlg.Filter = "Bitmap image (*.bmp)|*.bmp";
            dlg.RestoreDirectory = true;
            dlg.ShowDialog();

            //var bool2 = System.Windows.Forms.DialogResult.OK;

            /****** Upate filename in main window ********/
            string filename = dlg.SafeFileName;
            FilenameLabel.Content = filename;
            if (String.IsNullOrEmpty(filename))
            {
                return;
            }

            /****** Load bitmap image ********/
            string imagePath = dlg.FileName;
            if (this.DisplayImage.UriSource == null)
            {
                this.DisplayImage.BeginInit();
                this.DisplayImage.UriSource = new Uri(imagePath);
                this.DisplayImage.EndInit();
            }
            else
            {
                this.DisplayImage = new BitmapImage();
                this.DisplayImage.BeginInit();
                this.DisplayImage.UriSource = new Uri(imagePath);
                this.DisplayImage.EndInit();
            }

            /****** Show bitmapimage in main window ********/
            ImageDisplay.Source = this.DisplayImage;

            /****** Store bitmap image as Image ********/
            this.LoadedImage = Image.FromFile(imagePath);
        }
        private void DecryptButtonHandler(object sender, RoutedEventArgs e)
        {
            // check if image has been loaded
            if (this.DisplayImage.UriSource == null)
            {
                return;
            }

            var nonIndexedBitmap = this.ImageConverter.CreateNonIndexedBitmap(this.LoadedImage);

            string extractedText = this.SteganographyHelper.ExtractText(nonIndexedBitmap);

            if (DecryptPasswordInput.Password.Length > 0)
            {
                string password = DecryptPasswordInput.Password;
                try
                {
                    extractedText = this.CryptographyHelper.Decrypt(extractedText, password);
                }
                catch
                {
                    extractedText = "Wrong password";
                }
            }

            MessageInput.Text = extractedText;
        }

        private void SaveImageHandler(object sender, RoutedEventArgs e)
        {
            // check if image has been loaded
            if (this.DisplayImage.UriSource == null)
            {
                return;
            }

            Bitmap nonIndexedBitmap = this.ImageConverter.CreateNonIndexedBitmap(this.LoadedImage);

            string textToEmbed = MessageInput.Text;

            if (EncryptIsChecked.IsChecked == true)
            {
                string password = EncryptPasswordInput.Password;
                textToEmbed = this.CryptographyHelper.Encrypt(textToEmbed, password);
            }

            Bitmap imageWithText = this.SteganographyHelper.EmbedText(textToEmbed, nonIndexedBitmap);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            saveFileDialog.Filter = "Image files (*.bmp)|*.bmp";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;


                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    imageWithText.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
        }
    }
}
