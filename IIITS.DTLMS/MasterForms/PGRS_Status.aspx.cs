using IIITS.DTLMS.BL;
using IIITS.PGSQL.DAL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.MasterForms
{
    public partial class PGRS_Status : System.Web.UI.Page
    {
        static string strFormCode = "PGRSStatus";
        clsSession objSession;

        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        string StrQuery = string.Empty;
        string sURL = string.Empty;
        string sURLParam = string.Empty;
        string StrQry = string.Empty;
        string Description = string.Empty;
        string UpdateDate = string.Empty;
        string DocketNumber = string.Empty;
        string status = string.Empty;
        string status2desc = string.Empty;
        string time = string.Empty;
        string st = string.Empty;
        string stdes = string.Empty;
        string sStrQry = string.Empty;
        string sDFId = string.Empty;
        int j = 0;
        NpgsqlCommand NpgsqlCommand = new NpgsqlCommand();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    objSession = (clsSession)Session["clsSession"];
                    string stroffCode = string.Empty;
                   // Genaral.Load_Combo("SELECT \"ZO_ID\",\"ZO_NAME\" FROM \"TBLZONE\" ORDER BY \"ZO_ID\"", "--Select--", cmbZone);
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtPgrsNo.Text = string.Empty;
            }
            catch (Exception ex)
            {
               // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void GridView()
        {
            DataTable dtDTCDetails = new DataTable();
            try
            {
             StrQuery = "  SELECT COALESCE(CAST(\"DF_PGRS_UPDATE_TIME\" AS TEXT), 'NA') \"DF_PGRS_UPDATE_TIME\",\"DF_PGRS_DESCRIPTION\" ,\"DF_PGRS_STATUS\" ,\"DF_DTC_CODE\", ";
                StrQuery += " \"DF_EQUIPMENT_ID\",  \"DF_DATE\",\"TC_SLNO\",\"DF_PGRS_DOCKET\", \"DF_PGRS_DOCKET_DATE\",\"DT_TIMS_CODE\" FROM (SELECT CASE WHEN \"UPDATE\" IS NOT NULL THEN \"UPDATE\" ELSE NULL ";
                StrQuery += " END  \"DF_PGRS_UPDATE_TIME\",* FROM (SELECT  CASE WHEN \"DESCRIPTION\" IS NOT NULL THEN \"DESCRIPTION\" ELSE 'NA' END \"DF_PGRS_DESCRIPTION\",* FROM(SELECT CASE \"STATUS\" ";
                StrQuery += " WHEN '0' THEN 'NO UPDATE'  WHEN '1' THEN 'PENDING' WHEN '2' THEN 'RESOLVED' ELSE 'NA' END \"DF_PGRS_STATUS\" ,* FROM ( SELECT \"DF_PGRS_DESCRIPTION\" AS \"DESCRIPTION\", ";
                StrQuery += " \"DF_PGRS_UPDATE_TIME\" AS \"UPDATE\",\"DF_PGRS_STATUS\" AS \"STATUS\",  \"DF_DTC_CODE\",\"DF_EQUIPMENT_ID\",TO_CHAR(\"DF_DATE\",'YYYY/MM/DD')\"DF_DATE\",\"TC_SLNO\",  \"DF_PGRS_DOCKET\", ";
                StrQuery += " TO_CHAR(\"DF_PGRS_DOCKET_DATE\",'YYYY/MM/DD') \"DF_PGRS_DOCKET_DATE\",\"DT_TIMS_CODE\" FROM \"TBLDTCFAILURE\" INNER JOIN \"TBLDTCMAST\" ON \"DT_CODE\"=\"DF_DTC_CODE\" INNER JOIN \"TBLTCMASTER\" ";
                StrQuery += " ON \"DF_EQUIPMENT_ID\"=\"TC_CODE\" WHERE  \"DF_ID\" =" + sDFId + "  )A)B )C)E";

                dtDTCDetails = ObjCon.FetchDataTable(StrQuery);
                grdPGRSDetails.DataSource = dtDTCDetails;
                grdPGRSDetails.DataBind();

            }
            catch (Exception ex)
            {
             
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string spgrsNo = txtPgrsNo.Text;

                if (ValidateForm() == true)
                {


                    //   StrQry = " SELECT \"DF_ID\",\"DF_PGRS_DOCKET\" FROM(select * from \"TBLDTCFAILURE\" where \"DF_PGRS_DOCKET\" is not null)A where \"DF_PGRS_STATUS\" is NULL or CAST(\"DF_PGRS_STATUS\" AS INT)= 1 ";
                    StrQry = " select \"DF_ID\" from \"TBLDTCFAILURE\" where \"DF_PGRS_DOCKET\" = '" + spgrsNo + "'";
                    sDFId = ObjCon.get_value(StrQry, NpgsqlCommand);
                    if (sDFId.Length > 0)
                    {


                        sURL = "https://bescompgrs.com/";
                        sURLParam = "exicutive/apis/newmithra2019/docketupdate/updatedocket?action=pgrsdocketstatus_tomithra&docket_number=" + spgrsNo;

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("https://bescompgrs.com/");
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var postTask = client.GetAsync(sURLParam).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            var responseBody = postTask.Content.ReadAsStringAsync().Result;

                            string temp = responseBody.Split(':').GetValue(1).ToString().Trim('\"');
                            temp = temp.Split(' ').GetValue(0).ToString().Trim();
                            if (temp != "No")
                            {


                                if (responseBody.Contains("update_description"))
                                {
                                    Description = Convert.ToString(responseBody.Trim());
                                    //string statuss = responseBody.Split(':').GetValue(1).ToString().Trim('\"');
                                    string status1 = Description.Split(':').GetValue(4).ToString().Trim('\"');
                                    status2desc = status1.Split(',').GetValue(0).ToString().Trim('\"');
                                    status1 = status1.Split(' ').GetValue(1).ToString().Trim();
                                    if (status1 == "resolved")
                                    {
                                        j = 2;
                                    }

                                    else if (status1 == "Pending")
                                    {
                                        j = 1;
                                    }
                                    else
                                    {
                                        j = 1;
                                    }

                                }
                                else
                                {
                                    Description = null;
                                }
                                if (responseBody.Contains("update_dateime"))
                                {
                                    UpdateDate = Convert.ToString(responseBody.Trim());
                                    time = Description.Split('"').GetValue(3).ToString().Trim('\"');
                                    //  des = des.Split(' ').GetValue(0).ToString().Trim();

                                }
                                else
                                {
                                    UpdateDate = null;
                                }
                                if (responseBody.Contains("docketnumber"))
                                {
                                    DocketNumber = Convert.ToString(responseBody.Trim());
                                    string dockno = DocketNumber.Split('"').GetValue(11).ToString().Trim('\"');
                                }
                                else
                                {
                                    DocketNumber = null;
                                }
                            }
                            else
                            {
                                if (responseBody.Contains("status"))
                                {
                                    status = Convert.ToString(responseBody.Trim());
                                    stdes = responseBody.Split(':').GetValue(1).ToString().Trim('\"');
                                    st = stdes.Split(' ').GetValue(0).ToString().Trim();
                                    //  st = Convert.ToString(st.Replace('"', ' ').Split(',').GetValue(0)).Trim();

                                    if (st == "No")
                                    {
                                        j = 0;
                                    }
                                }
                            }
                            // sStrQry = "UPDATE \"TBLDTCFAILURE\" SET \"DF_PGRS_STATUS\"= '" +j+ "' ,\"DF_PGRS_DESCRIPTION\"= '" +Description+ "' ,\"DF_PGRS_UPDATE_TIME\"= '" +UpdateDate+ "'  WHERE \"DF_ID\"= " + dt.Rows[i]["DF_ID"] + " ";
                            sStrQry = "UPDATE \"TBLDTCFAILURE\" SET ";

                            if (j != 0)
                            {


                                if (status2desc != "" && status2desc != null)
                                {
                                    sStrQry += " \"DF_PGRS_DESCRIPTION\"= '" + status2desc + "', ";
                                }
                                if (j != null)
                                {
                                    sStrQry += " \"DF_PGRS_STATUS\"= '" + j + "',  ";
                                }
                                if (time != "" && time != null)
                                {
                                    sStrQry += " \"DF_PGRS_UPDATE_TIME\"= '" + time + "' ";
                                }
                            }
                            else
                            {
                                //if (stdes != "" && stdes != null)
                                //{
                                //    sStrQry += " \"DF_PGRS_DESCRIPTION\"= '" + stdes + "', ";
                                //}
                                if (j != null)
                                {
                                    sStrQry += " \"DF_PGRS_STATUS\"= '" + j + "'  ";
                                }
                            }

                            sStrQry += " WHERE \"DF_ID\"= " + sDFId + " ";

                            ObjCon.ExecuteQry(sStrQry);
                            //  MessageBox.Show("Updated succecfully", "Title", MessageBoxIcon.Information);

                        }
                        GridView();
                    }
                    else
                    {
                        ShowMsgBox("Please Enter Valid PGRS Number");
                    }
                    txtPgrsNo.Text = string.Empty;
                }
                //GridView();
            }
            catch (Exception ex)
            {
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
               // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            string sResultPO = string.Empty;
            try
            {               
                if (txtPgrsNo.Text.Trim() == "")
                {
                 //   txtFailedDate.Focus();
                    ShowMsgBox("Please Enter the PGRS Number.");

                   // cmdSave.Enabled = true;
                    return bValidate;
                }
                bValidate = true;
                return bValidate;

            }
            catch (Exception ex)
            {
              //  lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return bValidate;
            }
        }

    }
}