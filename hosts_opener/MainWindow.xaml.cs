using System;
using System.Collections.Generic;
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

namespace hosts_opener
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public string filepath = @"C:\WINDOWS\system32\drivers\etc\hosts";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            refreshFile();
        }

        private void saveFile()
        {
            string allText = commentsBox.Text + Environment.NewLine + Environment.NewLine + mainBox.Text;

            using (StreamWriter sw = new StreamWriter(filepath, false))
            {
                sw.WriteLine(allText);
                sw.Close();
            }
        }

        private void refreshFile()
        {
            commentsBox.Text = "";
            mainBox.Text = "";

            var fileContent = File.ReadAllLines(filepath);

            for (int i = 0; i < fileContent.Length; i++)
            {
                string curStr = fileContent[i];
                if(curStr != "")
                    if (curStr[0] == '#')
                    {
                        if (commentsBox.Text == "")
                            commentsBox.Text += curStr;
                        else
                            commentsBox.Text += Environment.NewLine + curStr;
                    }
                    else
                    {
                        if (mainBox.Text == "")
                            mainBox.Text += curStr;
                        else
                            mainBox.Text += Environment.NewLine + curStr;
                    }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            saveFile();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            refreshFile();
        }

        bool closeSave = true;


        private void CloseWithoutSave_Click(object sender, RoutedEventArgs e)
        {
            closeSave = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (closeSave)
                saveFile();
        }

        private void mainBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.RightCtrl)
            {
                //MessageBox.Show(mainBox.SelectionStart.ToString());
                var currentSel = mainBox.SelectionStart;
                var currentLine = mainBox.GetLineIndexFromCharacterIndex(currentSel);

                List<string> newText = new List<string>();

                for(var i = 0; i < mainBox.LineCount; i++)
                {
                    if(currentLine != i)
                    {
                        newText.Add(mainBox.GetLineText(i));
                    }
                    else
                    {
                        newText.Add(mainBox.GetLineText(i));

                        if (i == mainBox.LineCount - 1)
                            newText.Add(Environment.NewLine + mainBox.GetLineText(i));
                        else
                            newText.Add(mainBox.GetLineText(i));
                    }
                }

                mainBox.Clear();
                for(var i = 0; i < newText.Count; i++)
                {
                    mainBox.Text += newText[i];
                }

                mainBox.Select(currentSel, 0);
            }
        }
    }
}
