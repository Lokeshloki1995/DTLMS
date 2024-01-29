using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Data.OleDb;

namespace IIITS.DTLMS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdUpload_click(object sender, EventArgs e)
        {
            try
            {


                if (fupUpload.PostedFile.ContentLength == 0)
                {                    
                    fupUpload.Focus();
                    return;
                }

                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["UploadFormat"]);
                string sUploadFileExt = System.IO.Path.GetExtension(fupUpload.FileName).ToString().ToLower();
                sUploadFileExt = ";" + sUploadFileExt.Remove(0, 1) + ";";

                //if (!sFileExt.Contains(sUploadFileExt))
                //{                   
                //    return;
                //}
                string excelPath = Server.MapPath("~/DTLMSDocs/") + Path.GetFileName(fupUpload.PostedFile.FileName);
                fupUpload.SaveAs(excelPath);
                string conString = string.Empty;
                string extension = Path.GetExtension(fupUpload.PostedFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;
                }
                conString = string.Format(conString, excelPath);
                DataTable dtExcelData = new DataTable();

                DataTable dtmaterial = new DataTable();
                DataTable dtlabour = new DataTable();
                DataTable dtsalvage = new DataTable();



                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();


                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);

                    }
                    if (dtExcelData.Columns.Contains("Quantity"))
                    {
                        string result = string.Empty;
                        // obj.ExecuteUploadExecl(dtExcelData);
                        
                        List<DataTable> result1 = dtExcelData.AsEnumerable()
                        .GroupBy(x => x.Field<Double>("MRIM_ITEM_TYPE"))
                        .Select(g => g.CopyToDataTable())
                        .ToList();

                        foreach (DataTable dt in result1)
                        {
                            string itemtype = Convert.ToString(dt.Rows[0]["MRIM_ITEM_TYPE"]);
                            if (itemtype == "1")
                            {
                                dtmaterial = dt;
                            }
                            else if(itemtype == "2")
                            {
                                dtlabour = dt;
                            }
                            else if(itemtype == "3")
                            {
                                dtsalvage = dt;
                            }
                        }

                        for (int i = 0; i < dtExcelData.Rows.Count; i++)
                        {                            

                            string itemtype = Convert.ToString(dtExcelData.Rows[i]["MRIM_ITEM_TYPE"]);
                            if (itemtype == "1")
                            {
                                // dtmaterial.Rows.Add(dr.ItemArray);
                                dtmaterial.ImportRow(dtExcelData.Rows[i]);

                            }

                            if (itemtype == "2")
                            {
                                //dtlabour.Rows.Add(dr.ItemArray);

                                dtlabour.ImportRow(dtExcelData.Rows[i]);
                                //dtlabour = dtExcelData.Rows[i].Table;
                            }

                            if (itemtype == "3")
                            {
                                dtsalvage.ImportRow(dtExcelData.Rows[i]);
                                // dtsalvage.Rows.Add(dr.ItemArray);
                                //dtsalvage = dtExcelData.Rows[i].Table;
                            }


                        }


                        //if (result == "1")
                        //{
                        //    ShowMsgBox("Uploaded Successfully");
                        //}
                        //else
                        //{
                        //    ShowMsgBox("Something went Wrong Please Try again....!!!");
                        //}
                        excel_con.Close();
                        System.IO.File.Delete(excelPath);
                        //excelPath

                    }
                    else
                    {

                        excel_con.Close();
                        System.IO.File.Delete(excelPath);
                    }

                }




            }
            catch (Exception ex)
            {

            }
        }
    }
}