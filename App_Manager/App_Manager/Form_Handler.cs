using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Manager
{
    class Form_Handler
    {
        InputForm IF;
        public Form_Handler(InputForm inf)
        {
            IF = inf;
        }

        public void WriteToCSVFile(String filename)
        {
            FileStream writeTo = File.Open(filename + ".csv", FileMode.Append);
            StreamWriter formToSave = new StreamWriter(writeTo);
                formToSave.Write(String.Format("Company: {0}", IF.Company_Name) + "," );
                formToSave.Write(String.Format("Position: {0}", IF.Position_Name) + ",");
                formToSave.Write(String.Format("Date: {0}", IF.Date_Text) + ",");
                formToSave.Write(String.Format("ReqID: {0}", IF.Req_ID_Text) + ",");
                formToSave.WriteLine(String.Format("Additional Info: {0}", IF.Additional_Text));
            formToSave.Close();
        }

        public void saveForm()
        {
            SaveFileDialog saveFormD = new SaveFileDialog();
            saveFormD.Title = "Save File";
            saveFormD.ShowDialog();
            if (saveFormD.FileName != "")
            {
                WriteToCSVFile(saveFormD.FileName);
            }
        }
    }
}

