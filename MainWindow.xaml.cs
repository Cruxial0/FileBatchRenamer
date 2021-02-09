using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace FileBatchRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<string> inputFiles = new List<string>();
        private static string outputFolder;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFindFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            logElement("Button clicked", "#fff700",  $"'Select input files' was clicked.");

            if(ofd.ShowDialog() == true && ofd.FileNames.Length > 0)
            {
                inputFiles = ofd.FileNames.ToList();
                btnFindFiles.IsEnabled = false;

                if (btnFindFiles.IsEnabled == false && btnOutputFolder.IsEnabled == false) btnConvert.IsEnabled = true;
            }
        }

        private void btnOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            logElement("Button clicked", "#fff700", $"'Select output folder' was clicked.");

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK && fbd.SelectedPath != null)
            {
                outputFolder = fbd.SelectedPath;
                btnOutputFolder.IsEnabled = false;

                if (btnFindFiles.IsEnabled == false && btnOutputFolder.IsEnabled == false) btnConvert.IsEnabled = true;
            }
        }

        private void logElement(string header, string headerColorHex, string content)
        {
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Background = null;
            richTextBox.BorderBrush = null;

            System.Windows.Media.Color color = new System.Windows.Media.Color();
            System.Windows.Media.Color dColor = new System.Windows.Media.Color();
            color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(headerColorHex);
            dColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFFFF");
            SolidColorBrush colorBrush = new SolidColorBrush(color);
            SolidColorBrush defaultBrush = new SolidColorBrush(dColor);

            var paragraph = new Paragraph();
            richTextBox.Document = new FlowDocument(paragraph);
            richTextBox.FontSize = 12;

            paragraph.Inlines.Add(new Bold(new Run(header + ": "))
            {
                Foreground = colorBrush
            });
            paragraph.Inlines.Add(new Bold(new Run(content))
            {
                Foreground = defaultBrush
            });

            ctrlActionLog.Children.Add(richTextBox);
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            string stringFormat = txtOutputName.Text;
            List<Bitmap> outputImages = new List<Bitmap>();

            try
            {
                outputImages = new List<Bitmap>();
                logElement("Button clicked", "#00ddff", $"Temporary list created.");
            }
            catch(Exception ex)
            {
                logElement("Error!", "#ff4a4a", $"{ex.Message}\n{ex.StackTrace}");
                return;
            }

            foreach (var path in inputFiles)
            {
                Bitmap bmp;
                try
                {
                    bmp = new Bitmap(path);
                    logElement("Image found", "#00ddff", $"File in '{path}' was found and loaded.");

                    outputImages.Add(bmp);
                    logElement("Image saved", "#00ddff", $"Image converted and saved to list. Local Image ID: {outputImages.Count}");

                }
                catch (Exception ex)
                {
                    logElement("Error!", "#ff4a4a", $"{ex.Message}\n{ex.StackTrace}");
                    return;
                } 
            }

            logElement("Task completed", "#59ff38", $"All images found and saved.");
            logElement("Executing task", "#fff700", $"Saving images to output folder...");

            if(!Directory.Exists(outputFolder))
            {
                logElement("Error!", "#ff4a4a", $"Directory at '{outputFolder}' does not exist!");
                return;
            }

            logElement("Directory found", "#00ddff", $"Directory at '{outputFolder}' found.");

            int i = 0;

            foreach (var image in outputImages)
            {
                try
                {
                    i++;
                    image.Save(System.IO.Path.Combine(outputFolder, string.Format(stringFormat, i)));
                    logElement("Image saved", "#00ddff", $"Image '{string.Format(stringFormat, i)}' saved in output folder.");
                }
                catch (Exception ex)
                {
                    logElement("Error!", "#ff4a4a", $"{ex.Message}\n{ex.StackTrace}");
                    return;
                }
            }

            logElement("Taks completed", "#59ff38", $"All images found and saved.");

            MessageBox.Show("Task completed!");
        }
    }
}
