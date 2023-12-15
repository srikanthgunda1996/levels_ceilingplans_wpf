using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using micro = Microsoft.Win32;
using System;
using Autodesk.Revit.DB.Structure;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.IO;
using frms = System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace levels_ceilingplans_wpf
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MyForm : Window
    {
        public MyForm()
        {
            InitializeComponent();
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select csv File";
            dialog.Filter = "csv files | *.csv";
            dialog.Multiselect = false;

            if(dialog.ShowDialog() == frms.DialogResult.OK)
            {
                fileloc.Text = dialog.FileName;
            }
        }


        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
          this.DialogResult = true; this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false; this.Close();
        }



        internal string Getfloorplan()
        {
            if (floorPlans.IsChecked == true) { return "floor"; }
            else return null;
        }

        internal string Getceilingplan()
        {
           if(ceilingPlans.IsChecked == true) { return "ceilings"; }
           else { return null; }
        }


             public string Getunits()
            {
                if (imperial.IsChecked == true) { return "imperial"; }
                else { return "metric"; }
            }
    }
}
