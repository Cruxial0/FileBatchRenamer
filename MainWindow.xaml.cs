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
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using RichTextBox = System.Windows.Controls.RichTextBox;

namespace FileBatchRenamerPerformance
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

            Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
            new Action(() =>  logElement("INFO", "#fff700", $"Disabling logging can vastly improve performance results! (Enabled by default)")));
        }

        private void btnFindFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                  new Action(() =>  logElement("Button clicked", "#fff700", $"'Select output folder' was clicked.")));

            if (ofd.ShowDialog() == true && ofd.FileNames.Length > 1)
            {
                inputFiles = ofd.FileNames.ToList();
                btnFindFiles.IsEnabled = false;

                Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                  new Action(() =>  logElement("Update", "#eb85ff", $"{ofd.FileNames.Length} files found.")));
                

                if (btnFindFiles.IsEnabled == false && btnOutputFolder.IsEnabled == false) btnConvert.IsEnabled = true;
                return;
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                  new Action(() =>  logElement("Notice", "#ffa412", $"Please select 1 or more picture(s).")));
            }
        }

        private void btnOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                new Action(() =>  logElement("Button clicked", "#fff700", $"'Select output folder' was clicked.")));
            
            using(var fbd = new CommonOpenFileDialog())
            {
                fbd.IsFolderPicker = true;
                
                var result = fbd.ShowDialog();

                if (result == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    outputFolder = fbd.FileName;
                    btnOutputFolder.IsEnabled = false;
                    
                    Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                        new Action(() =>  logElement("Update", "#eb85ff", $"Output directory set to: '{fbd.FileName}'")));
                    if (btnFindFiles.IsEnabled == false && btnOutputFolder.IsEnabled == false) btnConvert.IsEnabled = true;
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                        new Action(() =>  logElement("Error!", "#ff4a4a", $"Something went wrong when setting the output folder...")));
                }
            }
        }
        
        
        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            string stringFormat = txtOutputName.Text;
            Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                new Action(() => logElement("Button clicked", "#00ddff", "Conversion started")));
            sw.Start();
            inputFiles.Sort();
            //inputFiles.Reverse();+
            
            if(!Directory.Exists(outputFolder))
            {
                Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                    new Action(() =>  logElement("Error!", "#ff4a4a", $"Directory at '{outputFolder}' does not exist!")));
                return;
            }
            
            var i = 0;
            foreach (var oldPath in inputFiles)
            {
                i++;
                var newFileName = String.Format(stringFormat, i);
                File.Copy(oldPath, Path.Combine(outputFolder,newFileName));
                Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                    new Action(() => logElement("File Processed", "#00ddff", $"File {newFileName} has been processed.")));
            }

            var processTime = sw.ElapsedMilliseconds;
            sw.Stop();
            
            Dispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                new Action(() => logElement("INFO", "#fff700", $"Processing time: {processTime}ms")));
            
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
