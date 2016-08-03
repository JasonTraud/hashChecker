using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Security.Cryptography;


namespace CheckHash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string fileDirectory = "";

        private void filePickerButton_Click(object sender, RoutedEventArgs e)
        {
            // Create the OpenFIleDialog object
            Microsoft.Win32.OpenFileDialog openPicker = new Microsoft.Win32.OpenFileDialog();

            // Display the OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openPicker.ShowDialog();

            // Check to see if we have a result 
            if (result == true)
            {
                filePicketTextBox.Text = openPicker.FileName.ToString();
                fileDirectory = openPicker.FileName.ToString();
                outputTextBox.Text = openPicker.FileName.ToString() + " selected." + "\n" + outputTextBox.Text;
            }
            else
            {
                outputTextBox.Text = "Operation cancelled." + "\n" + outputTextBox.Text;
            }
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            // Detect and handle the event of a void filename
            if (filePicketTextBox.Text == "")
            {
                outputTextBox.Text = "No file selected." + "\n" + outputTextBox.Text;
                return;
            }

            // Detect and handle the event of a non-valid filename
            try {
                var stream = File.OpenRead(filePicketTextBox.Text);
                stream.Close();
            }
            catch {
                outputTextBox.Text = "Invalid filename." + "\n" + outputTextBox.Text;
                return;
            }            

            // Detect event of no options selected
            if (!(bool)md5CheckBox.IsChecked && !(bool)sha1CheckBox.IsChecked)
            {
                outputTextBox.Text = "No options selected." + "\n" + outputTextBox.Text;
            }

            // MD5 Calculation and Display
            if ((bool)md5CheckBox.IsChecked)
            {
                md5OutputTextBox.Text = checkMD5(@filePicketTextBox.Text);
                outputTextBox.Text = "MD5: " + md5OutputTextBox.Text + "\n" + outputTextBox.Text;
            }

            // SHA1 Calculation and Display
            if((bool)sha1CheckBox.IsChecked)
            {
                sha1OutputTextBox.Text = checkSHA1(@filePicketTextBox.Text);
                outputTextBox.Text = "SHA1: " + sha1OutputTextBox.Text + "\n" + outputTextBox.Text;
            }
        }

        public string checkMD5(string filename)
        {
            string output;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder(hash.Length);

                    foreach (byte b in hash)
                    {
                        sb.AppendFormat("{0:X2}", b);
                    }
                    output = sb.ToString().ToLower();                    
                }
            }
            return output;
        }

        public string checkSHA1(string filename)
        {
            FileStream fs = new FileStream(@filename, FileMode.Open);
            string output;
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(fs);
                StringBuilder sb = new StringBuilder(hash.Length);
                foreach (byte b in hash)
                {
                    sb.AppendFormat("{0:X2}", b);
                }
                output = sb.ToString().ToLower();
            }
            fs.Close();
            return output;
        }
    }
}
