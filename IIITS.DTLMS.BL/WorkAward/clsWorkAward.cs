using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsWorkAward
    {
        public string strFormCode = "clsWorkAward";
        public String sWoId { get; set; }
        public String sWoNo { get; set; }
        public String sEstNo { get; set; }
        public string sWoDate { get; set; }
        public string sWoAmount { get; set; }
        public string sWOAId { get; set; }
        public string sWOANo { get; set; }
        public string sWOADate { get; set; }
        public double WoaAmount { get; set; }
        public string sRepairer { get; set; }

        public string sOfficeCode { get; set; }
        public string sClientIp { get; set; }
        public string sActionType { get; set; }
        public string sWFDataId { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFO_id { get; set; }
        public string sUserId { get; set; }



        public string sWoSlno { get; set; }
        public string sSubDivision { get; set; }
        public string sSection { get; set; }
        public string sMake { get; set; }
        public string sCapacity { get; set; }
        public string sSlno { get; set; }
        public string sTCode { get; set; }
        public string sAmount { get; set; }

        public DataTable dtEstDetails { get; set; }
        public DataTable dtWODetails { get; set; }

        PGSqlConnection objcon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand NpgsqlCommand;
        public void GetWoDetails(clsWorkAward obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("WoId",Convert.ToInt32(obj.sWoId));
                sQry = "SELECT \"WO_SLNO\",\"WO_NO\", TO_CHAR(\"WO_DATE\",'DD/MM/YYYY') \"WO_DATE\", \"WO_AMT\" FROM \"TBLWORKORDER\" WHERE \"WO_SLNO\" = :WoId";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    obj.sWoId = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                    obj.sWoNo = Convert.ToString(dt.Rows[0]["WO_NO"]);
                    obj.sWoDate = Convert.ToString(dt.Rows[0]["WO_DATE"]);
                    obj.sWoAmount = Convert.ToString(dt.Rows[0]["WO_AMT"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetEstimationDetails(clsWorkAward obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                //sQry = "SELECT \"WO_SLNO\",\"WO_NO\", \"TC_CODE\", \"TC_CAPACITY\", \"TM_NAME\", SUM(\"TAXABLE(12%)\") \"AMOUNT_12\", SUM(\"TAXABLE(18%)\") \"AMOUNT_18\", ";
                //sQry += " SUM(\"TAXABLE(28%)\") \"AMOUNT_28\", SUM(\"GST(12%)\") \"GST_12\", SUM(\"GST(18%)\") \"GST_18\", SUM(\"GST(28%)\") \"GST_28\", ";
                //sQry += " SUM(\"TOTAL\") \"TOTAL\" FROM (SELECT \"WO_SLNO\",\"WO_NO\", \"TC_CODE\", \"TC_CAPACITY\", \"TM_NAME\", (CASE WHEN \"MRI_TAX\"= 12 THEN SUM(\"ESTM_ITEM_RATE\") ";
                //sQry += " ELSE 0 END) \"TAXABLE(12%)\", (CASE WHEN \"MRI_TAX\"= 18 THEN SUM(\"ESTM_ITEM_RATE\") ELSE 0 END) \"TAXABLE(18%)\", ";
                //sQry += " (CASE WHEN \"MRI_TAX\"= 28 THEN SUM(\"ESTM_ITEM_RATE\") ELSE 0 END) \"TAXABLE(28%)\", (CASE WHEN \"MRI_TAX\"= 12 THEN SUM(\"ESTM_ITEM_TAX\") ";
                //sQry += " ELSE 0 END) \"GST(12%)\",(CASE WHEN \"MRI_TAX\"= 18 THEN SUM(\"ESTM_ITEM_TAX\") ELSE 0 END) \"GST(18%)\", ";
                //sQry += " (CASE WHEN \"MRI_TAX\"= 28 THEN SUM(\"ESTM_ITEM_TAX\") ELSE 0 END) \"GST(28%)\",SUM(\"ESTM_ITEM_TOTAL\")  \"TOTAL\" ";
                //sQry += " FROM \"TBLESTIMATIONDETAILS\", \"TBLESTIMATIONMATERIAL\", \"TBLMINORREPAIRITEMRATEMASTER\", \"TBLDTCFAILURE\", \"TBLTCMASTER\", ";
                //sQry += " \"TBLWORKORDER\", \"TBLTRANSMAKES\" WHERE \"WO_DF_ID\" = \"DF_ID\" AND \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND  \"DF_ID\" = \"EST_FAILUREID\" AND ";
                //sQry += " \"TC_MAKE_ID\" = \"TM_ID\" AND \"EST_ID\" = \"ESTM_EST_ID\" AND \"ESTM_ITEM_ID\" = \"MRI_MRIM_ID\" AND \"EST_CRON\" BETWEEN \"MRI_EFFECTIVE_FROM\" AND ";
                //sQry += " \"MRI_EFFECTIVE_TO\" AND CAST(\"EST_CAPACITY\" AS TEXT) = (SELECT \"MD_NAME\" FROM \"TBLMASTERDATA\" WHERE \"MD_ID\" = \"MRI_CAPACITY\" ";
                //sQry += " AND \"MD_TYPE\" = 'C') AND \"WO_SLNO\" = " + obj.sWoId + " GROUP BY \"WO_SLNO\",\"WO_NO\", \"TC_CODE\", \"TC_CAPACITY\", \"MRI_TAX\", \"TM_NAME\")A GROUP BY \"WO_NO\", ";
                //sQry += " \"TC_CODE\", \"TC_CAPACITY\", \"WO_SLNO\",\"TM_NAME\"";

                NpgsqlCommand.Parameters.AddWithValue("WoId",Convert.ToInt32(obj.sWoId));
                sQry = "SELECT \"WO_SLNO\", \"SD_SUBDIV_NAME\",\"OM_NAME\", \"TM_NAME\", \"TC_CAPACITY\", \"TC_SLNO\", \"TC_CODE\",\"WO_NO\",TO_CHAR(\"WO_DATE\",'DD/MM/YYYY') \"WO_DATE\", ";
                sQry += " \"WO_AMT\" FROM \"TBLWORKORDER\", \"TBLDTCFAILURE\", \"TBLOMSECMAST\", \"TBLSUBDIVMAST\", \"TBLTRANSMAKES\", \"TBLTCMASTER\" ";
                sQry += " WHERE \"WO_DF_ID\" = \"DF_ID\" AND \"DF_LOC_CODE\" = \"OM_CODE\"  AND \"OM_SUBDIV_CODE\" = \"SD_SUBDIV_CODE\" AND ";
                sQry += " \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"TC_MAKE_ID\" = \"TM_ID\" AND \"WO_SLNO\" =:WoId  ORDER BY \"WO_SLNO\"";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                obj.dtEstDetails = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public string[] SaveWorkAwardDetails(clsWorkAward obj)
        {
           
            string[] Arr = new string[3];
            try
            {
                string sQry = string.Empty;
                


                StringBuilder sbQuery = new StringBuilder();
                String sQry1 = string.Empty;

                for (int i = 0; i < obj.dtWODetails.Rows.Count; i++)
                {
                    sQry1 = "INSERT INTO \"TBLWORKAWARDDETAILS\"(\"WAD_WA_ID\", \"WAD_WO_SLNO\") VALUES ('{0}','" + Convert.ToString(obj.dtWODetails.Rows[i]["WO_SLNO"]) + "')";
                    sbQuery.Append(sQry1);
                    sbQuery.Append(";");

                    obj.sWoSlno += Convert.ToString(obj.dtWODetails.Rows[i]["WO_SLNO"]) + "`";
                    obj.sSubDivision += Convert.ToString(obj.dtWODetails.Rows[i]["SD_SUBDIV_NAME"]) + "`";
                    obj.sSection += Convert.ToString(obj.dtWODetails.Rows[i]["OM_NAME"]) + "`";
                    obj.sMake += Convert.ToString(obj.dtWODetails.Rows[i]["TM_NAME"]) + "`";
                    obj.sCapacity += Convert.ToString(obj.dtWODetails.Rows[i]["TC_CAPACITY"]) + "`";
                    obj.sSlno += Convert.ToString(obj.dtWODetails.Rows[i]["TC_SLNO"]) + "`";
                    obj.sTCode += Convert.ToString(obj.dtWODetails.Rows[i]["TC_CODE"]) + "`";
                    obj.sWoNo += Convert.ToString(obj.dtWODetails.Rows[i]["WO_NO"]) + "`";
                    obj.sWoDate += Convert.ToString(obj.dtWODetails.Rows[i]["WO_DATE"]) + "`";
                    obj.sAmount += Convert.ToString(obj.dtWODetails.Rows[i]["WO_AMT"]) + "`";

                    obj.WoaAmount = obj.WoaAmount + Convert.ToDouble(obj.dtWODetails.Rows[i]["WO_AMT"]);
                }

                sQry = "INSERT INTO \"TBLWORKAWARD\"(\"WAO_ID\", \"WOA_NO\", \"WOA_DATE\", \"WOA_AMOUNT\", \"WOA_TR_ID\",  \"WOA_CRBY\", \"WOA_CRON\")";
                sQry += " VALUES('{0}','" + obj.sWOANo + "',TO_DATE('" + obj.sWOADate + "','DD/MM/YYYY'),'" + obj.WoaAmount + "','" + obj.sRepairer + "','" + obj.sUserId + "',NOW() )";
                sQry = sQry.Replace("'", "''");





                sbQuery = sbQuery.Replace("'", "''");
                string sParam = "SELECT COALESCE(MAX(\"WAO_ID\"),0)+1 FROM \"TBLWORKAWARD\"";

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "WorkAward";
                objApproval.sOfficeCode = obj.sOfficeCode;
                objApproval.sClientIp = obj.sClientIp;
                objApproval.sCrby = obj.sUserId;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLWORKAWARD";
                objApproval.sDataReferenceId = obj.sWoNo;
                objApproval.sDescription = "Work Award for Minor Repair & Reconditioning ";
                objApproval.sRefOfficeCode = obj.sOfficeCode;
                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "WAO_ID,WOA_NO,WOA_DATE,WOA_AMOUNT,WOA_CRBY,WOA_TR_ID";
                objApproval.sColumnNames += ";WO_SLNO,SD_SUBDIV_NAME,OM_NAME,TM_NAME,TC_CAPACITY,TC_SLNO,TC_CODE,WO_NO,WO_DATE,WO_AMT";
                objApproval.sColumnValues = "" + sPrimaryKey + "," + obj.sWOANo + "," + obj.sWOADate + "," + obj.WoaAmount + "," + obj.sUserId + "," + obj.sRepairer + "";
                objApproval.sColumnValues += "; " + obj.sWoSlno + "," + obj.sSubDivision + "," + obj.sSection + "," + obj.sMake + "," + obj.sCapacity + "," + obj.sSlno + "," + obj.sTCode + "," + obj.sWoNo + "," + obj.sWoDate + "," + obj.sAmount + "";
                objApproval.sTableNames = "TBLWORKAWARD;TBLWORKAWARDDETAILS";

                //Check for Duplicate Approval
                bool bApproveResult = objApproval.CheckDuplicateApprove(objApproval);
                if (bApproveResult == false)
                {
                    Arr[0] = "Selected Record Already Approved";
                    Arr[1] = "2";
                    return Arr;
                }

                if (obj.sActionType == "M")
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    obj.sWFDataId = objApproval.sWFDataId;
                }
                else
                {
                    objApproval.SaveWorkFlowData(objApproval);
                    objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                    objApproval.SaveWorkflowObjects(objApproval);
                }


                Arr[0] = "Saved Successfully";
                Arr[1] = "0";
                Arr[2] = objApproval.sNewRecordId;
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Arr;
            }
        }
        
        public void GetEstimateDetailsFromXML(clsWorkAward obj)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dt = new DataTable();

                DataSet ds = new DataSet();

                ds = objApproval.GetDatatableFromMultipleXML(obj.sWFDataId);

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        dt = ds.Tables[i];
                        if (i == 0)
                        {
                            obj.sWOANo = Convert.ToString(dt.Rows[0]["WOA_NO"]);
                            obj.sWOADate = Convert.ToString(dt.Rows[0]["WOA_DATE"]);
                            obj.WoaAmount = Convert.ToDouble(dt.Rows[0]["WOA_AMOUNT"]);
                            obj.sRepairer = Convert.ToString(dt.Rows[0]["WOA_TR_ID"]);
                        }
                        else if (i == 1)
                        {
                            obj.sWoSlno = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                            obj.sSubDivision = Convert.ToString(dt.Rows[0]["SD_SUBDIV_NAME"]);
                            obj.sSection = Convert.ToString(dt.Rows[0]["OM_NAME"]);
                            obj.sMake = Convert.ToString(dt.Rows[0]["TM_NAME"]);
                            obj.sCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                            obj.sSlno = Convert.ToString(dt.Rows[0]["TC_SLNO"]);
                            obj.sTCode = Convert.ToString(dt.Rows[0]["TC_CODE"]);
                            obj.sWoNo = Convert.ToString(dt.Rows[0]["WO_NO"]);
                            obj.sWoDate = Convert.ToString(dt.Rows[0]["WO_DATE"]);
                            obj.sAmount = Convert.ToString(dt.Rows[0]["WO_AMT"]);
                            obj.dtWODetails = CreateDatatableFromString(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public DataTable CreateDatatableFromString(clsWorkAward objWo)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("WO_SLNO");
            dt.Columns.Add("SD_SUBDIV_NAME");
            dt.Columns.Add("OM_NAME");
            dt.Columns.Add("TM_NAME");
            dt.Columns.Add("TC_CAPACITY");
            dt.Columns.Add("TC_SLNO");
            dt.Columns.Add("TC_CODE");
            dt.Columns.Add("WO_NO");
            dt.Columns.Add("WO_DATE");
            dt.Columns.Add("WO_AMT");


            string[] WoSlno = objWo.sWoSlno.Split('`');
            string[] SubDivision = objWo.sSubDivision.Split('`');
            string[] Section = objWo.sSection.Split('`');
            string[] Make = objWo.sMake.Split('`');
            string[] Capacity = objWo.sCapacity.Split('`');
            string[] Slno = objWo.sSlno.Split('`');
            string[] TCode = objWo.sTCode.Split('`');
            string[] WoNo = objWo.sWoNo.Split('`');
            string[] WoDate = objWo.sWoDate.Split('`');
            string[] Amount = objWo.sAmount.Split('`');

            for (int i = 0; i < WoSlno.Length; i++)
            {
                for (int j = 0; j < SubDivision.Length; j++)
                {
                    for (int k = 0; k < Section.Length; k++)
                    {
                        for (int l = 0; l < Make.Length; l++)
                        {
                            for (int m = 0; m < Capacity.Length; m++)
                            {
                                for (int n = 0; n < Slno.Length; n++)
                                {
                                    for (int o = 0; o < TCode.Length; o++)
                                    {
                                        for (int p = 0; p < WoNo.Length; p++)
                                        {
                                            for (int q = 0; q < WoDate.Length; q++)
                                            {
                                                for (int r = 0; r < Amount.Length; r++)
                                                {
                                                    if (Amount[r] != "" && Amount[r] != " ")
                                                    {
                                                        DataRow dRow = dt.NewRow();
                                                        dRow["WO_SLNO"] = WoSlno[i];
                                                        dRow["SD_SUBDIV_NAME"] = SubDivision[j];
                                                        dRow["OM_NAME"] = Section[k];
                                                        dRow["TM_NAME"] = Make[l];
                                                        dRow["TC_CAPACITY"] = Capacity[m];
                                                        dRow["TC_SLNO"] = Slno[n];
                                                        dRow["TC_CODE"] = TCode[o];
                                                        dRow["WO_NO"] = WoNo[p];
                                                        dRow["WO_DATE"] = WoDate[q];
                                                        dRow["WO_AMT"] = Amount[r];
                                                        dt.Rows.Add(dRow);
                                                        dt.AcceptChanges();
                                                    }
                                                    i++;
                                                    j++;
                                                    k++;
                                                    l++;
                                                    m++;
                                                    n++;
                                                    o++;
                                                    p++;
                                                    q++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }

        public void GetWorkAwardDetails(clsWorkAward objWo)
        {            
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT \"WAO_ID\", \"WOA_NO\", \"WOA_AMOUNT\", \"TR_NAME\" \"WOA_TR_ID\", TO_CHAR(\"WOA_DATE\",'DD-MON-YYYY') \"WOA_DATE\" FROM \"TBLWORKAWARD\", \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\" WHERE CAST(\"WOA_TR_ID\" AS TEXT) = CAST(\"TR_ID\" AS TEXT) and \"TR_ID\"=\"TRO_TR_ID\" and cast(\"TRO_OFF_CODE\" as text) like '" + objWo.sOfficeCode + "%'";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry);
                objWo.dtWODetails = objcon.FetchDataTable(sQry);
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetWorkAwardobjects(clsWorkAward objWo)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("WOAId", objWo.sWOAId);
                sQry = "SELECT \"WAO_ID\", \"WOA_NO\", \"WOA_AMOUNT\",  \"WOA_TR_ID\", TO_CHAR(\"WOA_DATE\",'DD/MM/YYYY') \"WOA_DATE\" FROM \"TBLWORKAWARD\" ";
                sQry += " WHERE CAST(\"WAO_ID\" AS TEXT) =:WOAId";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                if(dt.Rows.Count>0)
                {
                    objWo.sWOANo = Convert.ToString(dt.Rows[0]["WOA_NO"]);
                    objWo.sWoAmount = Convert.ToString(dt.Rows[0]["WOA_AMOUNT"]);
                    objWo.sRepairer = Convert.ToString(dt.Rows[0]["WOA_TR_ID"]);
                    objWo.sWOADate = Convert.ToString(dt.Rows[0]["WOA_DATE"]);
                }
                NpgsqlCommand.Parameters.AddWithValue("WOAId1", Convert.ToInt32(objWo.sWOAId));
                sQry = "SELECT \"WO_SLNO\", \"SD_SUBDIV_NAME\",\"OM_NAME\", \"TM_NAME\", \"TC_CAPACITY\", \"TC_SLNO\", \"TC_CODE\",\"WO_NO\",TO_CHAR(\"WO_DATE\",'DD/MM/YYYY') \"WO_DATE\", ";
                sQry += " \"WO_AMT\" FROM \"TBLWORKORDER\", \"TBLDTCFAILURE\", \"TBLOMSECMAST\", \"TBLSUBDIVMAST\", \"TBLTRANSMAKES\", \"TBLTCMASTER\", \"TBLWORKAWARDDETAILS\" ";
                sQry += " WHERE \"WO_DF_ID\" = \"DF_ID\" AND \"DF_LOC_CODE\" = \"OM_CODE\"  AND \"OM_SUBDIV_CODE\" = \"SD_SUBDIV_CODE\" AND ";
                sQry += " \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"TC_MAKE_ID\" = \"TM_ID\" AND \"WO_SLNO\" = \"WAD_WO_SLNO\" AND \"WAD_WA_ID\" = :WOAId1 ORDER BY \"WO_SLNO\"";

                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                objWo.dtEstDetails = dt;

            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetWorkAwardReport(clsWorkAward objWo)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("WOAId1", Convert.ToInt32(objWo.sWOAId));
                sQry = "SELECT \"WAO_ID\", \"DIV_NAME\", (SELECT string_agg(\"SD_SUBDIV_NAME\",',') \"SUBDIVISIONS\" FROM \"TBLSUBDIVMAST\" A WHERE A.\"SD_DIV_CODE\" = \"DIV_CODE\"), ";
                sQry += " \"WOA_NO\", \"WOA_AMOUNT\", TO_CHAR(\"WOA_DATE\",'DD-MON-YYYY') \"WOA_DATE\", \"TR_NAME\", \"TR_ADDRESS\", \"WO_SLNO\", \"SD_SUBDIV_NAME\",\"OM_NAME\", \"TM_NAME\", ";
                sQry += " \"TC_CAPACITY\", \"TC_SLNO\", \"TC_CODE\",\"WO_NO\", TO_CHAR(\"WO_DATE\",'DD-MON-YYYY') \"WO_DATE\", \"WO_AMT\" FROM \"TBLWORKORDER\", \"TBLDTCFAILURE\", \"TBLOMSECMAST\", \"TBLSUBDIVMAST\", ";
                sQry += " \"TBLTRANSMAKES\", \"TBLTCMASTER\", \"TBLWORKAWARD\", \"TBLWORKAWARDDETAILS\", \"TBLTRANSREPAIRER\", \"TBLDIVISION\" WHERE \"WO_DF_ID\" = \"DF_ID\" AND ";
                sQry += " \"DF_LOC_CODE\" = \"OM_CODE\"  AND \"OM_SUBDIV_CODE\" = \"SD_SUBDIV_CODE\" AND \"DF_EQUIPMENT_ID\" = \"TC_CODE\" AND \"TC_MAKE_ID\" = \"TM_ID\" ";
                sQry += " AND \"WAO_ID\" = \"WAD_WA_ID\" AND \"WAD_WO_SLNO\" = \"WO_SLNO\" AND \"TR_ID\" = \"WOA_TR_ID\" AND \"SD_DIV_CODE\" = \"DIV_CODE\" AND \"WAO_ID\" = :WOAId1  ORDER BY \"WAO_ID\"";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                objWo.dtWODetails = dt;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}
        
