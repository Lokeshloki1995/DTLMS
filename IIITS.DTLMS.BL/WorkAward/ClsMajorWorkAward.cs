using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIITS.PGSQL.DAL;
using System.Data;
using Npgsql;
using System.Collections;

namespace IIITS.DTLMS.BL.WorkAward
{
    public  class ClsMajorWorkAward
    {
        public string strFormCode = "ClsMajorWorkAward";

        public string division { get; set; }
        public string soil { get; set; }

        public string sbarrel { get; set; }

        public string actualoil { get; set; }

        public string oiltype { get; set; }

        public string issuedate { get; set; }
        public String sWoId { get; set; }
        public String sWoNo { get; set; }
        public String sEstNo { get; set; }
        public string sWoDate { get; set; }
        public string sWoAmount { get; set; }
        public string sInnCost { get; set; }

        public string sWOAId { get; set; }
        public string sWOANo { get; set; }
        public string sWOADate { get; set; }
        public double WoaAmount { get; set; }

        public double WoaInAmount { get; set; }
        public string sRepairer { get; set; }

        public string sOfficeCode { get; set; }
        public string sClientIp { get; set; }
        public string sActionType { get; set; }
        public string sWFDataId { get; set; }
        public string sWFAutoId { get; set; }
        public string sWFO_id { get; set; }
        public string sUserId { get; set; }
        public string matrialfolio { get; set; }


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

        // Coded by Rudra //on 05-03-2020 // for Binding Repairer Name in WorkAward for based on work order Number
        public void GetWoDetails(ClsMajorWorkAward obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                //NpgsqlCommand.Parameters.AddWithValue("WoId", Convert.ToInt32(obj.sWoId));
                sQry = "SELECT \"RWO_SLNO\",\"RWO_NO\", TO_CHAR(\"RWO_DATE\",'DD/MM/YYYY') \"RWO_DATE\", \"RWO_AMT\",\"RWO_INNC_COST\",\"RESTD_REPAIRER\",\"RESTD_TC_CODE\" FROM \"TBLREPAIRERWORKORDER\",\"TBLREPAIRERESTIMATIONDETAILS\" WHERE \"RWO_EST_ID\"=\"RESTD_ID\" and \"RWO_SLNO\" = '" + obj.sWoId + "'";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry);
                if (dt.Rows.Count > 0)
                {
                    obj.sWoId = Convert.ToString(dt.Rows[0]["RWO_SLNO"]);
                    obj.sWoNo = Convert.ToString(dt.Rows[0]["RWO_NO"]);
                    obj.sWoDate = Convert.ToString(dt.Rows[0]["RWO_DATE"]);
                    obj.sWoAmount = Convert.ToString(dt.Rows[0]["RWO_AMT"]);
                    obj.sInnCost = Convert.ToString(dt.Rows[0]["RWO_INNC_COST"]);
                    obj.sTCode = Convert.ToString(dt.Rows[0]["RESTD_TC_CODE"]);
                    if (obj.sInnCost=="" || obj.sInnCost== null )
                    {
                        obj.sInnCost = "0";
                    }
                    obj.sRepairer = Convert.ToString(dt.Rows[0]["RESTD_REPAIRER"]);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        // Coded by Rudra on 05-03-2020 //  for Adding Inccured Cost into the grid view with the Work order amount 
        public void GetEstimationDetails(ClsMajorWorkAward obj)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                
              //  NpgsqlCommand.Parameters.AddWithValue("WoId", Convert.ToInt32(obj.sWoId));
                sQry = "SELECT \"RWO_SLNO\", \"SD_SUBDIV_NAME\",\"OM_NAME\", \"TM_NAME\", \"TC_CAPACITY\", \"TC_SLNO\", \"TC_CODE\",\"RWO_NO\",TO_CHAR(\"RWO_DATE\",'DD/MM/YYYY') \"RWO_DATE\", ";
                sQry += " \"RWO_AMT\",\"RWO_INNC_COST\" FROM \"TBLREPAIRERWORKORDER\", \"TBLREPAIRERESTIMATIONDETAILS\",\"TBLOMSECMAST\", \"TBLSUBDIVMAST\", \"TBLTRANSMAKES\", \"TBLTCMASTER\" ";
                sQry += " WHERE \"RWO_EST_ID\" = \"RESTD_ID\" AND \"RESTD_OFF_CODE\" = cast(\"OM_CODE\" as text)  AND \"OM_SUBDIV_CODE\" = \"SD_SUBDIV_CODE\" AND ";
                sQry += " \"RESTD_TC_CODE\" = \"TC_CODE\" AND \"TC_MAKE_ID\" = \"TM_ID\" AND \"RWO_SLNO\" ='"+ obj.sWoId + "'  ORDER BY \"RWO_SLNO\"";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry);
                obj.dtEstDetails = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        // changed BY Rudra on 05-03-2020 for adding Inncurred field amount in WorkAward 
        public string[] SaveWorkAwardDetails(ClsMajorWorkAward obj)
        {

            string[] Arr = new string[3];
            try
            {
                string sQry = string.Empty;



                StringBuilder sbQuery = new StringBuilder();
                String sQry1 = string.Empty;

                for (int i = 0; i < obj.dtWODetails.Rows.Count; i++)
                {
                    sQry1 = "INSERT INTO \"TBLREPAIRWORKAWARDDETAILS\"(\"RWAD_WA_ID\", \"RWAD_WO_SLNO\") VALUES ('{0}','" + Convert.ToString(obj.dtWODetails.Rows[i]["RWO_SLNO"]) + "')";
                    sbQuery.Append(sQry1);
                    sbQuery.Append(";");

                    obj.sWoSlno += Convert.ToString(obj.dtWODetails.Rows[i]["RWO_SLNO"]) + "`";
                    obj.sSubDivision += Convert.ToString(obj.dtWODetails.Rows[i]["SD_SUBDIV_NAME"]) + "`";
                    obj.sSection += Convert.ToString(obj.dtWODetails.Rows[i]["OM_NAME"]) + "`";
                    obj.sMake += Convert.ToString(obj.dtWODetails.Rows[i]["TM_NAME"]) + "`";
                    obj.sCapacity += Convert.ToString(obj.dtWODetails.Rows[i]["TC_CAPACITY"]) + "`";
                    obj.sSlno += Convert.ToString(obj.dtWODetails.Rows[i]["TC_SLNO"]) + "`";
                    obj.sTCode += Convert.ToString(obj.dtWODetails.Rows[i]["TC_CODE"]) + "`";
                    obj.sWoNo += Convert.ToString(obj.dtWODetails.Rows[i]["RWO_NO"]) + "`";
                    obj.sWoDate += Convert.ToString(obj.dtWODetails.Rows[i]["RWO_DATE"]) + "`";
                    obj.sAmount += Convert.ToString(obj.dtWODetails.Rows[i]["RWO_AMT"]) + "`";
                    obj.sInnCost += Convert.ToString(obj.dtWODetails.Rows[i]["RWO_INNC_COST"]) + "`";

                    obj.WoaAmount = obj.WoaAmount + Convert.ToDouble(obj.dtWODetails.Rows[i]["RWO_AMT"]);
                    obj.WoaInAmount = obj.WoaInAmount + Convert.ToDouble(obj.dtWODetails.Rows[i]["RWO_INNC_COST"]);
                }

                sQry = "INSERT INTO \"TBLREPAIRERWORKAWARD\"(\"RWAO_ID\", \"RWOA_NO\", \"RWOA_DATE\", \"RWOA_AMOUNT\",\"RWOA_INNC_COST\", \"RWOA_TR_ID\",  \"RWOA_CRBY\", \"RWOA_CRON\")";
                sQry += " VALUES('{0}','" + obj.sWOANo + "',TO_DATE('" + obj.sWOADate + "','DD/MM/YYYY'),'" + obj.WoaAmount + "','" + obj.WoaInAmount + "','" + obj.sRepairer + "','" + obj.sUserId + "',NOW() )";
                sQry = sQry.Replace("'", "''");





                sbQuery = sbQuery.Replace("'", "''");
                string sParam = "SELECT COALESCE(MAX(\"RWAO_ID\"),0)+1 FROM \"TBLREPAIRERWORKAWARD\"";

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "MajorWorkAward";
                objApproval.sOfficeCode = obj.sOfficeCode;
                objApproval.sClientIp = obj.sClientIp;
                objApproval.sCrby = obj.sUserId;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLREPAIRERWORKAWARD";
                objApproval.sDataReferenceId = obj.sTCode;
                objApproval.sDescription = "Work Award for Major Repair & Reconditioning ";
                objApproval.sRefOfficeCode = obj.sOfficeCode;
                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "RWAO_ID,RWOA_NO,RWOA_DATE,RWOA_AMOUNT,RWOA_INNC_COST,RWOA_CRBY,RWOA_TR_ID";
                objApproval.sColumnNames += ";RWO_SLNO,SD_SUBDIV_NAME,OM_NAME,TM_NAME,TC_CAPACITY,TC_SLNO,TC_CODE,RWO_NO,RWO_DATE,RWO_AMT,RWO_INNC_COST,RDIV_CODE";
                objApproval.sColumnValues = "" + sPrimaryKey + "," + obj.sWOANo + "," + obj.sWOADate + "," + obj.WoaAmount + "," + obj.WoaInAmount + "," + obj.sUserId + "," + obj.sRepairer + "";
                objApproval.sColumnValues += "; " + obj.sWoSlno + "," + obj.sSubDivision + "," + obj.sSection + "," + obj.sMake + "," + obj.sCapacity + "," + obj.sSlno + "," + obj.sTCode + "," + obj.sWoNo + "," + obj.sWoDate + "," + obj.sAmount + "," + obj.sInnCost + "," + obj.division+"";
                objApproval.sTableNames = "TBLREPAIRERWORKAWARD;TBLREPAIRWORKAWARDDETAILS";

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
        public ArrayList getCreatedByUserName(string sOffCode)
        {
            ArrayList strQrylist = new ArrayList();
            string sWoid = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (sOffCode.Length > 2)
                {
                    sOffCode = sOffCode.Substring(0, 2);
                }

                NpgsqlCommand cmd = new NpgsqlCommand("sp_getworkawardusername");
                cmd.Parameters.AddWithValue("sofficecode", sOffCode);
                dt = objcon.FetchDataTable(cmd);

                for (int i = 0; i < 4; i++)
                {
                    if (dt.Rows.Count > i)
                    {
                        if (dt.Rows[i][0].ToString() != "" || dt.Rows[i][0].ToString() != null)
                            strQrylist.Add(dt.Rows[i][0].ToString());
                    }
                    else
                        strQrylist.Add("");

                }
                return strQrylist;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return strQrylist;
            }


        }
        public bool CheckAlreadyCreated( string sTcCode)
        {

            string[] Arr = new string[3];
            try
            {
                string sQry = string.Empty;
                sQry = " SELECT \"WO_DATA_ID\" from  \"TBLWORKFLOWOBJECTS\" ,\"TBLWFODATA\"  where \"WO_BO_ID\" = 75 and \"WO_RECORD_ID\" < 0  and \"WO_WFO_ID\" = cast(\"WFO_ID\" as text)";
                sQry += "  AND \"WO_DATA_ID\" like '%"+ sTcCode +"`%' ";
               string  result=objcon.get_value(sQry);
                if(result!="" && result!=null )
                {
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public string[] SaveWorkAwardoilDetails(ClsMajorWorkAward obj)
        {
            string[] Arr = new string[3];
            try
            {
                string sQry = string.Empty;

                StringBuilder sbQuery = new StringBuilder();
                String sQry1 = string.Empty;
              
                sQry = "INSERT INTO \"TBLTRANSILOIL\"(\"TO_ID\",\"TO_WOA_ID\", \"TO_TOTAL_BARREL\", \"TO_ACT_OIL\", \"TO_SEND_OIL\",  \"TO_OIL_TYPE\", \"TO_DATE\" ,\"TO_MATERIAL_FOLIO\")";
                sQry += " VALUES('{0}','" + obj.sWOAId + "','" + obj.sbarrel + "','" + obj.actualoil + "','" + obj.soil + "','" + obj.oiltype + "',TO_DATE('" + obj.issuedate + "','DD/MM/YYYY') ,'" + obj.matrialfolio + "' )";
                sQry = sQry.Replace("'", "''");

                sbQuery = sbQuery.Replace("'", "''");
                string sParam = "SELECT COALESCE(MAX(\"TO_ID\"),0)+1 FROM \"TBLTRANSILOIL\"";

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Transiloil";
                objApproval.sOfficeCode = obj.sOfficeCode;
                objApproval.sClientIp = obj.sClientIp;
                objApproval.sCrby = obj.sUserId;
                objApproval.sQryValues = sQry + ";" + sbQuery;
                objApproval.sParameterValues = sParam;
                objApproval.sMainTable = "TBLTRANSILOIL";
                objApproval.sDataReferenceId = obj.sWOAId;
                objApproval.sDescription = "Transil oil for rapairer ";
                objApproval.sRefOfficeCode = obj.sOfficeCode;
                objApproval.sWFObjectId = obj.sWFO_id;

                objApproval.sMaterialfoloi = obj.matrialfolio;
                string sPrimaryKey = "{0}";

                objApproval.sColumnNames = "TO_ID,TO_WOA_ID,TO_TOTAL_BARREL,TO_ACT_OIL,TO_SEND_OIL";
                objApproval.sColumnNames += ",TO_OIL_TYPE,TO_DATE,TO_MATERIAL_FOLIO";
                objApproval.sColumnValues = "" + sPrimaryKey + "," + obj.sWOAId + "," + obj.sbarrel + "," + obj.actualoil + "," + obj.soil + "";
                objApproval.sColumnValues += ", " + obj.oiltype + "," + obj.issuedate + "," + obj.matrialfolio + " ";
                objApproval.sTableNames = "TBLTRANSILOIL";

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

        public void GetEstimateDetailsFromXML(ClsMajorWorkAward obj)
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
                            obj.sWOANo = Convert.ToString(dt.Rows[0]["RWOA_NO"]);
                            obj.sWOADate = Convert.ToString(dt.Rows[0]["RWOA_DATE"]);
                            obj.WoaAmount = Convert.ToDouble(dt.Rows[0]["RWOA_AMOUNT"]);

                            if(dt.Columns.Contains("RWOA_INNC_COST"))
                            {
                                obj.WoaInAmount = Convert.ToDouble(dt.Rows[0]["RWOA_INNC_COST"]);
                            }
                            else
                            {
                                dt.Columns.Add("RWOA_INNC_COST", typeof(string));
                                dt.Rows[0]["RWOA_INNC_COST"] = "0";
                                obj.WoaInAmount = Convert.ToDouble(dt.Rows[0]["RWOA_INNC_COST"]);
                            }
                            //obj.WoaInAmount = Convert.ToDouble(dt.Rows[0]["RWOA_INNC_COST"]);
                            obj.sRepairer = Convert.ToString(dt.Rows[0]["RWOA_TR_ID"]);
                           
                        }
                        else if (i == 1)
                        {
                            obj.sWoSlno = Convert.ToString(dt.Rows[0]["RWO_SLNO"]);
                            obj.sSubDivision = Convert.ToString(dt.Rows[0]["SD_SUBDIV_NAME"]);
                            obj.sSection = Convert.ToString(dt.Rows[0]["OM_NAME"]);
                            obj.sMake = Convert.ToString(dt.Rows[0]["TM_NAME"]);
                            obj.sCapacity = Convert.ToString(dt.Rows[0]["TC_CAPACITY"]);
                            obj.sSlno = Convert.ToString(dt.Rows[0]["TC_SLNO"]);
                            obj.sTCode = Convert.ToString(dt.Rows[0]["TC_CODE"]);
                            obj.sWoNo = Convert.ToString(dt.Rows[0]["RWO_NO"]);
                            obj.sWoDate = Convert.ToString(dt.Rows[0]["RWO_DATE"]);
                            obj.sAmount = Convert.ToString(dt.Rows[0]["RWO_AMT"]);
                            if(dt.Columns.Contains("RWO_INNC_COST"))
                            {
                                obj.sInnCost = Convert.ToString(dt.Rows[0]["RWO_INNC_COST"]);
                            }
                            else
                            {
                                dt.Columns.Add("RWO_INNC_COST", typeof(string));

                                if (obj.sInnCost == null)
                                {
                                    string[] WoSlno = obj.sWoSlno.Split('`');
                                    for (int a = 0; a < WoSlno.Length - 1; a++)
                                    {
                                        obj.sInnCost += "0`";
                                    }
                                }
                                // dt.Rows[0]["RWO_INNC_COST"] = "0";
                                //  obj.sInnCost = Convert.ToString(dt.Rows[0]["RWO_INNC_COST"]);
                            }
                            //obj.sInnCost = Convert.ToString(dt.Rows[0]["RWO_INNC_COST"]);                
                            obj.division = Convert.ToString(dt.Rows[0]["RDIV_CODE"]);
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


        public void GetoilDetailsFromXML(ClsMajorWorkAward obj)
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
                            obj.sWOAId = Convert.ToString(dt.Rows[0]["TO_WOA_ID"]);
                            obj.issuedate = Convert.ToString(dt.Rows[0]["TO_DATE"]);
                            obj.oiltype = Convert.ToString(dt.Rows[0]["TO_OIL_TYPE"]);
                            obj.soil = Convert.ToString(dt.Rows[0]["TO_SEND_OIL"]);
                            obj.sbarrel = Convert.ToString(dt.Rows[0]["TO_TOTAL_BARREL"]);
                            obj.matrialfolio = Convert.ToString(dt.Rows[0]["TO_MATERIAL_FOLIO"]);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        // coded by Rudra for Inncurred cost on 06-03-2020
        public DataTable CreateDatatableFromString(ClsMajorWorkAward objWo)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("RWO_SLNO");
            dt.Columns.Add("SD_SUBDIV_NAME");
            dt.Columns.Add("OM_NAME");
            dt.Columns.Add("TM_NAME");
            dt.Columns.Add("TC_CAPACITY");
            dt.Columns.Add("TC_SLNO");
            dt.Columns.Add("TC_CODE");
            dt.Columns.Add("RWO_NO");
            dt.Columns.Add("RWO_DATE");
            dt.Columns.Add("RWO_AMT");
            dt.Columns.Add("RWO_INNC_COST");

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
            string[] InnCost = objWo.sInnCost.Split('`');

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
                                                        for (int s = 0; s < InnCost.Length; s++)
                                                        {
                                                            if (InnCost[s] != "" && InnCost[s] != " ")
                                                            {
                                                                DataRow dRow = dt.NewRow();
                                                                dRow["RWO_SLNO"] = WoSlno[i];
                                                                dRow["SD_SUBDIV_NAME"] = SubDivision[j];
                                                                dRow["OM_NAME"] = Section[k];
                                                                dRow["TM_NAME"] = Make[l];
                                                                dRow["TC_CAPACITY"] = Capacity[m];
                                                                dRow["TC_SLNO"] = Slno[n];
                                                                dRow["TC_CODE"] = TCode[o];
                                                                dRow["RWO_NO"] = WoNo[p];
                                                                dRow["RWO_DATE"] = WoDate[q];
                                                                dRow["RWO_AMT"] = Amount[r];
                                                                dRow["RWO_INNC_COST"] = InnCost[s];
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
                                                            r++;
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
                }
            }
            return dt;
        }
        public string getofficeName(string offcode)
        {
            try
            {
                string sQry = string.Empty;
                sQry = "SELECT \"DIV_NAME\" FROM \"TBLDIVISION\" WHERE CAST(\"DIV_CODE\" AS TEXT)=SUBSTR(CAST('" + offcode + "' AS TEXT),1,'" + Constants.Division + "')";
                //            sQry = "SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" WHERE CAST(\"CM_CIRCLE_CODE\" AS TEXT)=SUBSTR(CAST('" + offcode + "' AS TEXT),1,'" + Constants.Circle + "')";
                return objcon.get_value(sQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return "";
            }
        }
        public void GetWorkAwardDetails(ClsMajorWorkAward objWo)
        {
            try
            {
                string sQry = string.Empty;
               // objWo.sOfficeCode = objWo.sOfficeCode.Substring(0,3);
                sQry = "SELECT DISTINCT \"RWAO_ID\", \"RWOA_NO\", \"RWOA_AMOUNT\", \"TR_NAME\" \"RWOA_TR_ID\", TO_CHAR(\"RWOA_DATE\",'DD-MON-YYYY') \"RWOA_DATE\" FROM \"TBLREPAIRERWORKAWARD\", \"TBLTRANSREPAIRER\",\"TBLTRANSREPAIREROFFCODE\",\"TBLREPAIRWORKAWARDDETAILS\",\"TBLREPAIRERWORKORDER\" WHERE \"RWAD_WA_ID\"=\"RWAO_ID\" and  \"RWAD_WO_SLNO\"= \"RWO_SLNO\" and  CAST(\"RWOA_TR_ID\" AS TEXT) = CAST(\"TR_ID\" AS TEXT) and \"TR_ID\"=\"TRO_TR_ID\" and cast(\"RWO_OFF_CODE\" as text) like '" + objWo.sOfficeCode + "%'";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry);
                objWo.dtWODetails = objcon.FetchDataTable(sQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void GetWorkAwardobjects(ClsMajorWorkAward objWo)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("WOAId", objWo.sWOAId);
                sQry = "SELECT \"RWAO_ID\", \"RWOA_NO\", \"RWOA_AMOUNT\", \"RWOA_INNC_COST\", \"RWOA_TR_ID\", TO_CHAR(\"RWOA_DATE\",'DD/MM/YYYY') \"RWOA_DATE\" FROM \"TBLREPAIRERWORKAWARD\" ";
                sQry += " WHERE CAST(\"RWAO_ID\" AS TEXT) =:WOAId";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                if (dt.Rows.Count > 0)
                {
                    objWo.sWOANo = Convert.ToString(dt.Rows[0]["RWOA_NO"]);
                    objWo.sWoAmount = Convert.ToString(dt.Rows[0]["RWOA_AMOUNT"]);
                    objWo.sInnCost = Convert.ToString(dt.Rows[0]["RWOA_INNC_COST"]);
                    objWo.sRepairer = Convert.ToString(dt.Rows[0]["RWOA_TR_ID"]);
                    objWo.sWOADate = Convert.ToString(dt.Rows[0]["RWOA_DATE"]);
                }
                NpgsqlCommand.Parameters.AddWithValue("WOAId1", Convert.ToInt32(objWo.sWOAId));
                sQry = "SELECT \"RWO_SLNO\", \"SD_SUBDIV_NAME\",\"OM_NAME\", \"TM_NAME\", \"TC_CAPACITY\", \"TC_SLNO\", \"TC_CODE\",\"RWO_NO\",TO_CHAR(\"RWO_DATE\",'DD/MM/YYYY') \"RWO_DATE\", ";
                sQry += " \"RWO_AMT\",\"RWO_INNC_COST\",\"RWO_OFF_CODE\" FROM \"TBLREPAIRERWORKORDER\",\"TBLREPAIRERESTIMATIONDETAILS\", \"TBLOMSECMAST\", \"TBLSUBDIVMAST\", \"TBLTRANSMAKES\", \"TBLTCMASTER\", \"TBLREPAIRWORKAWARDDETAILS\" ";
                sQry += " WHERE \"RWO_EST_ID\" = \"RESTD_ID\" AND \"RESTD_OFF_CODE\" = cast(\"OM_CODE\" as text)  AND \"OM_SUBDIV_CODE\" = \"SD_SUBDIV_CODE\" AND ";
                sQry += " \"RESTD_TC_CODE\" = \"TC_CODE\" AND \"TC_MAKE_ID\" = \"TM_ID\" AND \"RWO_SLNO\" = \"RWAD_WO_SLNO\" AND \"RWAD_WA_ID\" = :WOAId1 ORDER BY \"RWO_SLNO\"";

                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                objWo.dtEstDetails = dt;
                objWo.division = Convert.ToString(dt.Rows[0]["RWO_OFF_CODE"]);
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        // coded by Rudra for Inncurred cost on 06-03-2020 
        public void GetWorkAwardReport(ClsMajorWorkAward objWo)
        {
            NpgsqlCommand = new NpgsqlCommand();
            try
            {
                string sQry = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("WOAId1", Convert.ToInt32(objWo.sWOAId));
                sQry = "SELECT \"RWAO_ID\", \"DIV_NAME\", (SELECT \"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\"  WHERE  \"CM_CIRCLE_CODE\" =\"DIV_CICLE_CODE\")  \"CIRCLE_NAME\",(SELECT string_agg(\"SD_SUBDIV_NAME\",',') \"SUBDIVISIONS\" FROM \"TBLSUBDIVMAST\" A WHERE A.\"SD_DIV_CODE\" = \"DIV_CODE\"), ";
                sQry += " \"RWOA_NO\", \"RWOA_AMOUNT\", TO_CHAR(\"RWOA_DATE\",'DD-MON-YYYY') \"RWOA_DATE\", \"TR_NAME\", \"TR_ADDRESS\", \"TR_ENQUIRY_NO\",\"TR_CONTRACT_NO\" ,\"TR_RENEWAL\" , \"TR_ACCOUNT_HEAD\" ,\"TR_DTR_VIDE_NO\",\"TR_GST\",\"RWO_SLNO\", \"SD_SUBDIV_NAME\",\"OM_NAME\", \"TM_NAME\", ";
                sQry += " \"TC_CAPACITY\", \"TC_SLNO\", \"TC_CODE\",\"RWO_NO\", TO_CHAR(\"RWO_DATE\",'DD-MON-YYYY') \"RWO_DATE\",\"RWO_INNC_COST\", \"RWO_AMT\" FROM \"TBLREPAIRERWORKORDER\", \"TBLREPAIRERESTIMATIONDETAILS\", \"TBLOMSECMAST\", \"TBLSUBDIVMAST\", ";
                sQry += " \"TBLTRANSMAKES\", \"TBLTCMASTER\", \"TBLREPAIRERWORKAWARD\", \"TBLREPAIRWORKAWARDDETAILS\", \"TBLTRANSREPAIRER\", \"TBLDIVISION\" WHERE \"RWO_EST_ID\" = \"RESTD_ID\" AND ";
                sQry += " \"RESTD_OFF_CODE\" =cast(\"OM_CODE\" as text)  AND \"OM_SUBDIV_CODE\" = \"SD_SUBDIV_CODE\" AND \"RESTD_TC_CODE\" = \"TC_CODE\" AND \"TC_MAKE_ID\" = \"TM_ID\" ";
                sQry += " AND \"RWAO_ID\" = \"RWAD_WA_ID\" AND \"RWAD_WO_SLNO\" = \"RWO_SLNO\" AND \"TR_ID\" = \"RWOA_TR_ID\" AND \"SD_DIV_CODE\" = \"DIV_CODE\" AND \"RWAO_ID\" = :WOAId1  ORDER BY \"RWAO_ID\"";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);
                objWo.dtWODetails = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public object GetawrdDetails(ClsMajorWorkAward objDetails)
        {
            DataTable dtDetails = new DataTable();
           
            try
            {
                NpgsqlCommand = new NpgsqlCommand();
                string sQry = string.Empty;
                string sQry1 = string.Empty;
                NpgsqlCommand.Parameters.AddWithValue("WOAId1", Convert.ToInt32(objDetails.sWoSlno));
                sQry= " SELECT \"RWOA_NO\",to_char(\"RWOA_DATE\",'dd-mm-yyyy') as \"RWOA_DATE\",\"RWAO_ID\" from \"TBLREPAIRERWORKAWARD\" WHERE \"RWAO_ID\" =:WOAId1";
                DataTable dt = new DataTable();
                dt = objcon.FetchDataTable(sQry, NpgsqlCommand);

                NpgsqlCommand = new NpgsqlCommand();
                NpgsqlCommand.Parameters.AddWithValue("WOAId", Convert.ToInt32(dt.Rows[0]["RWAO_ID"].ToString()));
                sQry1 = "SELECT \"RWAD_WO_SLNO\" from \"TBLREPAIRWORKAWARDDETAILS\" WHERE \"RWAD_WA_ID\"=:WOAId";
                DataTable dtslno = new DataTable();
                dtslno = objcon.FetchDataTable(sQry1, NpgsqlCommand);

                if(dt.Rows.Count>0)
                { 
                    objDetails.sWOANo = dt.Rows[0]["RWOA_NO"].ToString();
                    objDetails.sWOADate = dt.Rows[0]["RWOA_DATE"].ToString();
                }
                double total=0;
                if(dtslno.Rows.Count>0)
                {
                    for (int i = 0; i < dtslno.Rows.Count; i++)
                    {
                        NpgsqlCommand = new NpgsqlCommand();
                        NpgsqlCommand.Parameters.AddWithValue("woslno", Convert.ToInt32(dtslno.Rows[i]["RWAD_WO_SLNO"].ToString()));
                        string qrycap =objcon.get_value(" SELECT \"RESTD_TC_OILCAP\" from \"TBLREPAIRERESTIMATIONDETAILS\" inner join \"TBLREPAIRERWORKORDER\" on \"RESTD_ID\" = \"RWO_EST_ID\"  WHERE  \"RWO_SLNO\" =:woslno", NpgsqlCommand);

                        total += Convert.ToDouble(qrycap);


                    }
                    objDetails.actualoil =Convert.ToString(total);
                    //objDetails.actualoil = dtDetails.Rows[0]["ACTUAL_OIL"].ToString();
                }

                return objDetails;
            }

            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return objDetails;
            }
        }
        
            
    }
}
