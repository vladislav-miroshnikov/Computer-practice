using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Drawing;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using ServiceReference;
using System.Drawing.Imaging;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServerContractCallback
    {
        ServerContractClientBase client;
        volatile bool isCancel;
        Bitmap image;
        byte[] filteredBytes;

        public MainWindow()
        {
            InitializeComponent();
            client = new ServerContractClientBase(new InstanceContext(this));
            isCancel = false;
            Loaded += FiltersInit;
        }

        private void FiltersInit(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> filters = client.GetListOfFilters();
                foreach (var f in filters)
                {
                    filtersList.Items.Add(f);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ReturnImage(byte[] bytes)
        {
            //because we only need to access UI elements from the main thread
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate {
                Bitmap filteredImage = null;
                filteredBytes = bytes;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    filteredImage = (Bitmap)System.Drawing.Image.FromStream(ms);
                }
                BitmapSource imgFormat = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(filteredImage.GetHbitmap(), IntPtr.Zero,
                         Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(image.Width, image.Height)); mainImage.Source = imgFormat;
                cancelButton.IsEnabled = false;
                cancelButton.Visibility = Visibility.Hidden;
                applyButton.IsEnabled = true;
                uploadButton.IsEnabled = true;
                uploadButton.Visibility = Visibility.Visible;
                bar.Value = 0;
                saveButton.Visibility = Visibility.Visible;
            }));
            
        }

        public void ReturnProgress(int progress)
        {
            if (!isCancel)
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate { bar.Value = progress; }));
            }
        }

        private void SelectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения (*.JPG;*.PNG;*.BMP)|*.JPG;*.PNG;*.BMP" + "|Все файлы|*.* ";
            string path = null;
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
            }
            try
            {
                image = new Bitmap(path);
                BitmapSource imgFormat = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), 
                    IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(image.Width, image.Height));
                mainImage.Source = imgFormat;
                applyButton.Visibility = Visibility.Visible;
                applyButton.IsEnabled = true;
                saveButton.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ApplyFilter(object sender, RoutedEventArgs e)
        {
            byte[] bytes = null;
            if (isCancel == true)
            {
                client = new ServerContractClientBase(new InstanceContext(this)); //if abort happened
            }
            isCancel = false;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bytes = ms.GetBuffer();
            }
            try
            {
                
                if(filtersList.SelectedItem == null)
                {
                    throw new ArgumentException("You don't choose a filter type!");
                }
                client.ApplyFilter(bytes, (string)filtersList.SelectedItem);
                cancelButton.IsEnabled = true;
                cancelButton.Visibility = Visibility.Visible;
                applyButton.IsEnabled = false;
                uploadButton.IsEnabled = false;
                uploadButton.Visibility = Visibility.Hidden;
                saveButton.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelFilter(object sender, RoutedEventArgs e)
        {
            bar.Value = 0;
            client.Abort();
            isCancel = true;
            cancelButton.IsEnabled = false;
            cancelButton.Visibility = Visibility.Hidden;
            uploadButton.IsEnabled = true;
            applyButton.IsEnabled = true;
            uploadButton.Visibility = Visibility.Visible;
        }

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveSelection = new SaveFileDialog();

            saveSelection.Title = "Save the image";
            saveSelection.Filter = "Все файлы| *.*";

            MessageBox.Show("Write the name with the extension, for example \"picture.jpg\"");

            if (saveSelection.ShowDialog() == true)
            {
                using (MemoryStream ms = new MemoryStream(filteredBytes))
                {
                    Bitmap savingImage = (Bitmap)System.Drawing.Image.FromStream(ms);
                    savingImage.Save(saveSelection.FileName);
                }            
            }
        }
    }
}
