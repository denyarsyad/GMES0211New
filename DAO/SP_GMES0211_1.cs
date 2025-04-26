using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JPlatform.Client.JBaseForm6;
using System.Data;
using System.Data.SqlClient;

namespace CSI.MES.P.DAO
{
    public class SP_GMES0211_1 : BaseProcClass
    {
        public SP_GMES0211_1(string type = "Q")
        {
            // Modify Code : Procedure Name
            if (type == "Q")
            {
                _ProcName = "SP_GMES0211_1_Q_JJ";
                ParamAdd();
            }
            else
                if (type == "S")
                {
                    _ProcName = "SP_GMES0211_1_S_JJ";
                    ParamSave();
                }
        }

        private void ParamAdd()
        {
            _ParamInfo.Add(new ParamInfo("@V_P_USER_ID", "Varchar", 100, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CURRENT_PASS", "Varchar", 100, "Input", typeof(System.String)));
        }

        private void ParamSave()
        {
            _ParamInfo.Add(new ParamInfo("@V_P_USER_ID", "Varchar", 100, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_CURRENT_PASS", "Varchar", 100, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_NEW_PASS", "Varchar", 100, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_UPDATER", "Varchar", 100, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("@V_P_UPDATE_PC", "Varchar", 100, "Input", typeof(System.String)));
        }

        public DataTable SetParamData(DataTable dataTable,
                                    System.String V_P_USER_ID,
                                    System.String V_P_CURRENT_PASS
        )
        {
            if (dataTable == null)
            {
                dataTable = new DataTable(_ProcName);
                foreach (ParamInfo pi in _ParamInfo)
                {
                    dataTable.Columns.Add(pi.ParamName, pi.TypeClass);
                }
            }
            // Modify Code : Procedure Parameter
            object[] objData = new object[] {
		                            V_P_USER_ID,
                                    V_P_CURRENT_PASS
            };
            dataTable.Rows.Add(objData);
            return dataTable;
        }

        public DataTable SetParamDataSave(
                                    DataTable dataTable,
                                    System.String V_P_USER_ID,
                                    System.String V_P_CURRENT_PASS,
                                    System.String V_P_NEW_PASS,
                                    System.String V_P_UPDATER,
                                    System.String V_P_UPDATE_PC
        )
        {
            if (dataTable == null)
            {
                dataTable = new DataTable(_ProcName);
                foreach (ParamInfo pi in _ParamInfo)
                {
                    dataTable.Columns.Add(pi.ParamName, pi.TypeClass);
                }
            }
            // Modify Code : Procedure Parameter
            object[] objData = new object[] {
                                    V_P_USER_ID,
                                    V_P_CURRENT_PASS,
                                    V_P_NEW_PASS,
                                    V_P_UPDATER,
                                    V_P_UPDATE_PC
            };
            dataTable.Rows.Add(objData);
            return dataTable;
        }
    }
}
