using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.PGSQL.DAL;
using System.Data;
using System.Data.OleDb;
using Npgsql;

namespace IIITS.DTLMS.BL
{
    public class clsContactUs
    {
        string strFormCode = "clsContactUs";
        PGSqlConnection ObjCon = new PGSqlConnection(Constants.Password);
        NpgsqlCommand cmd = new NpgsqlCommand();

        public DataSet LoadDetails()
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataSet Dset = new DataSet();
            string[] strQry = new string[4];
            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    strQry[i] = "Select \"CC_NAME\",\"CC_MOBILENO\",\"CC_EMAIL\",(select \"DIV_NAME\" from \"TBLDIVISION\" where \"DIV_CODE\"=\"CC_OFFCODE\")DIV,(select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" where \"CM_CIRCLE_CODE\"=substr(\"CC_OFFCODE\",0,1)) \"CIRCLE\" from \"TBLCONTACTCONFIG\" where \"CC_LEVEL\" = '" + i + "'";
                    if (i == 1)
                    {
                        dt1 = ObjCon.FetchDataTable(strQry[i]);
                        Dset.Tables.Add(dt1);
                    }
                    else if (i == 2)
                    {
                        dt2 = ObjCon.FetchDataTable(strQry[i]);
                        Dset.Tables.Add(dt2);
                    }
                    else if(i==3)
                    {
                         dt3 = ObjCon.FetchDataTable(strQry[i]);
                         Dset.Tables.Add(dt3);
                    }
                }
                return Dset;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return Dset;
            }

        }


        public DataTable LoadDetailsForFirstGrid()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "Select \"CC_NAME\",\"CC_MOBILENO\",\"CC_EMAIL\",(select \"DIV_NAME\" from \"TBLDIVISION\" where \"DIV_CODE\"=\"CC_OFFCODE\")DIV,";
                //strQry += " (select \"CM_CIRCLE_NAME\" from \"TBLCIRCLE\" where cast(\"CM_CIRCLE_CODE\" as text) =substr(cast(\"CC_OFFCODE\" as text),0,1)) CIRCLE";
                //strQry += " from \"TBLCONTACTCONFIG\"  where \"CC_LEVEL\" = '1'";
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loaddetailsforfirstgrid");
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }

        public DataTable LoadDetailsForSecondGrid()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "Select \"CC_NAME\",\"CC_MOBILENO\",\"CC_EMAIL\" ";
                //strQry += "   from \"TBLCONTACTCONFIG\"  where \"CC_LEVEL\" = '2'";
                //dt = ObjCon.FetchDataTable(strQry);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loaddetailsforsecondgrid");
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }


        public DataTable LoadDetailsForThirdGrid()
        {
            DataTable dt = new DataTable();
            string strQry = string.Empty;
            try
            {
                //strQry = "SELECT \"CC_NAME\",\"CC_MOBILENO\",\"CC_EMAIL\" ";
                //strQry += " FROM \"TBLCONTACTCONFIG\"  WHERE \"CC_LEVEL\" = '3'";
                // dt = ObjCon.FetchDataTable(strQry);
                NpgsqlCommand cmd = new NpgsqlCommand("sp_loaddetailsforthirdgrid");
                dt = ObjCon.FetchDataTable(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                return dt;
            }

        }

    }
}

