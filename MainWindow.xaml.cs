using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace FileBatchRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool enableLogging = true;
        public static bool AutoScroll = true;

        private static List<string> inputFiles = new List<string>();
        private static string outputFolder;
        public static Stopwatch sw = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();

            Dispatcher.Invoke(DispatcherPriority.Background,
            new Action(() =>  logElement("INFO", "#fff700", $"Disabling logging can vastly improve performance results! (Enabled by default)")));
        }

        private void btnFindFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>  logElement("Button clicked", "#fff700", $"'Select output folder' was clicked.")));

            if (ofd.ShowDialog() == true && ofd.FileNames.Length > 1)
            {
                inputFiles = ofd.FileNames.ToList();
                btnFindFiles.IsEnabled = false;

                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>  logElement("Update", "#eb85ff", $"{ofd.FileNames.Length} files found.")));
                

                if (btnFindFiles.IsEnabled == false && btnOutputFolder.IsEnabled == false) btnConvert.IsEnabled = true;
                return;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>  logElement("Notice", "#ffa412", $"Please select 1 or more picture(s).")));
            }
        }

        private void btnOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>  logElement("Button clicked", "#fff700", $"'Select output folder' was clicked.")));

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK && fbd.SelectedPath != null)
            {
                outputFolder = fbd.SelectedPath;
                btnOutputFolder.IsEnabled = false;

                Dispatcher.Invoke(DispatcherPriority.Background,
                 new Action(() =>  logElement("Update", "#eb85ff", $"Output directory set to: '{fbd.SelectedPath}'")));

                if (btnFindFiles.IsEnabled == false && btnOutputFolder.IsEnabled == false) btnConvert.IsEnabled = true;
                return;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>  logElement("Error!", "#ff4a4a", $"Something went wrong when setting the output folder...")));
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            string stringFormat = txtOutputName.Text;
            List<Bitmap> outputImages = new List<Bitmap>();

            try
            {
                outputImages = new List<Bitmap>();
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>  logElement("Button clicked", "#00ddff", $"Temporary list created.")));
            }
            catch(Exception ex)
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() =>  logElement("Error!", "#ff4a4a", $"{ex.Message}\n{ex.StackTrace}")));
                return;
            }

            sw.Start();
            foreach (var path in inputFiles)
            {
                Bitmap bmp;
                try
                {
                    bmp = new Bitmap(path);
                    Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() =>  logElement("Image found", "#00ddff", $"File in '{path}' was found and loaded.")));

                    outputImages.Add(bmp);
                    Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() =>  logElement("Image saved", "#00ddff", $"Image converted and saved to list. Local Image ID: {outputImages.Count}")));

                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() =>  logElement("Error!", "#ff4a4a", $"{ex.Message}\n{ex.StackTrace}")));
                    btnReset.IsEnabled = true;
                    return;
                } 
            }

            string conversionTime = sw.ElapsedMilliseconds.ToString();
            sw.Stop();

            Dispatcher.Invoke(DispatcherPriority.Background,
            new Action(() =>  logElement("Task completed", "#59ff38", $"All images found and saved.")));
            Dispatcher.Invoke(DispatcherPriority.Background,
            new Action(() =>  logElement("Executing task", "#fff700", $"Saving images to output folder...")));

            if(!Directory.Exists(outputFolder))
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(() =>  logElement("Error!", "#ff4a4a", $"Directory at '{outputFolder}' does not exist!")));
                return;
            }

            Dispatcher.Invoke(DispatcherPriority.Background,
            new Action(() =>  logElement("Directory found", "#00ddff", $"Directory at '{outputFolder}' found.")));

            int i = 0;

            sw.Reset();
            sw.Start();

            foreach (var image in outputImages)
            {
                try
                {
                    i++;
                    image.Save(System.IO.Path.Combine(outputFolder, string.Format(stringFormat, i)));
                    Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() =>  logElement("Image saved", "#00ddff", $"Image '{string.Format(stringFormat, i)}' saved in output folder.")));
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() =>  logElement("Error!", "#ff4a4a", $"{ex.Message}\n{ex.StackTrace}")));
                    btnReset.IsEnabled = true;
                    return;
                }
            }
            string elapsedSaved = sw.ElapsedMilliseconds.ToString();
            sw.Stop();

            Dispatcher.Invoke(DispatcherPriority.Background,
            new Action(() => logElement("Task completed", "#59ff38", $"All images found and saved.")));
            Dispatcher.Invoke(DispatcherPriority.Background,
            new Action(() => logElement("INFO", "#fff700", $"Time elapsed converting: {conversionTime}ms")));
            Dispatcher.Invoke(DispatcherPriority.Background,
            new Action(() => logElement("INFO", "#fff700", $"Time elapsed converting: {elapsedSaved}ms")));

            MessageBox.Show($"Task completed!\nConversion time: {conversionTime}ms\nSave time {elapsedSaved}ms");

            btnReset.IsEnabled = true;

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            inputFiles = null;
            outputFolder = null;

            btnReset.IsEnabled = false;
            btnConvert.IsEnabled = false;
            btnFindFiles.IsEnabled = true;
            btnOutputFolder.IsEnabled = true;

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            enableLogging = true;
            ctrlActionLog.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            enableLogging = false;
            ctrlActionLog.IsEnabled = false;
        }

        private void chkAutoScroll_Checked(object sender, RoutedEventArgs e)
        {
            AutoScroll = true;
        }

        private void chkAutoScroll_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoScroll = false;
        }

        private async Task logElement(string header, string headerColorHex, string content)
        {
            if (!enableLogging) return;

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

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {   
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                {   
                    AutoScroll = true;
                }
                else
                {   
                    AutoScroll = false;
                }
            }

            if (AutoScroll && e.ExtentHeightChange != 0)
            {   
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
            }
        }
    }
}
