#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Reflection;

#endregion

namespace levels_ceilingplans_wpf
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // put any code needed for the form here

            // open form
            MyForm currentForm = new MyForm()
            {
                Width = 1000,
                Height = 550,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm.ShowDialog();

            // get form data and do something

            if(currentForm.DialogResult == false ) { return Result.Failed; }


            string[] csvData = System.IO.File.ReadAllLines(currentForm.fileloc.Text);
            List<string[]> fromcsv = getfromcsv(csvData);
            int count = 0;

            Transaction t = new Transaction(doc);
            t.Start("Creating Levels");
            {
                foreach (string[] fromcsvData in fromcsv )
                { 
                    if(count == 0) { count++; continue; }
                    string levelName = fromcsvData[0];
                    double levelele = 0;

                    if(currentForm.Getunits() == "imperial")
                        {
                            levelele = Double.Parse(fromcsvData[1]);
                        }
                    else
                        {
                            levelele = Double.Parse(fromcsvData[2]) * 3.28;
                        }

                        Level newLevel = Level.Create(doc, levelele);
                        newLevel.Name = levelName;
                         ViewFamilyType vft = getViewFamilyType(doc, "Floor Plan", ViewFamily.FloorPlan);
                         ViewFamilyType ceilingvft = getViewFamilyType(doc, "Ceiling Plan", ViewFamily.CeilingPlan);
                    if (currentForm.Getfloorplan() == "floor")
                         {
                            ViewPlan newfloorplan = ViewPlan.Create(doc, vft.Id, newLevel.Id);
                         }
                     if(currentForm.Getceilingplan() == "ceilings")
                         {
                            ViewPlan newfloorplan = ViewPlan.Create(doc, ceilingvft.Id, newLevel.Id);
                         }
            
                    count++;
                }

            }

            t.Commit();
            t.Dispose();
            return Result.Succeeded;
        }

        private ViewFamilyType getViewFamilyType(Document doc, string v, ViewFamily curPlan)
        {
            FilteredElementCollector familytype = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType));
            foreach(ViewFamilyType curvft in familytype)
            {
                if (curvft.Name == v && curvft.ViewFamily == curPlan)
                {
                    return curvft;
                }
                
            }
            return null;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }


            public List<string[]> getfromcsv(string[] data)
            {
                List<string[]> result = new List<string[]>();
                foreach (string s in data)
                {
                    string[] strings = s.Split(',');
                    result.Add(strings);
                }
                return result;
            }
        
    }
}
