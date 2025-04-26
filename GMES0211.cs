using System.Data;
using System.Drawing;
using JPlatform.Client.CSIGMESBaseform6;
using System.Runtime.InteropServices;
using System.Windows.Forms;
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using JPlatform.Client.JERPBaseForm6;
using CSI.MES.P.DAO;
using JPlatform.Client.Library6.interFace;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Diagnostics;
using JPlatform.Client.Controls6;
using DevExpress.XtraGrid;
//using JPlatform.Client.CSIGMESBaseform6;
using System.Net;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
//using System.Runtime.InteropServices;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Media;
using System.IO;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Threading;
using DevExpress.XtraGrid.Views.Base;
using System.Text.RegularExpressions;
using System.Net.Sockets;

namespace CSI.MES.P
{
    public partial class GMES0211 : CSIGMESBaseform6
    {
        //2025.01.08
        //Update Plant G

        public GMES0211()
        {
            InitializeComponent();
        }

        #region [VARIABLE]
        DataTable dtPlant = new DataTable();
        DataTable dtLocation = new DataTable();
        DataTable dtArea = new DataTable();
        DataTable dtItemClass_type = new DataTable();
        DataTable dtLabour = new DataTable();
        DataTable dtGet = new DataTable();
        DataTable dtDefect = new DataTable();
        DataTable dtSolution = new DataTable();
        DataTable dtMachine = new DataTable();
        DataTable dtWorkshop = null;
        DataTable dtMEP = null;
        DataTable dtMC_Loc = null;
        DataTable dtCIS = null;
        DataTable dtOnOff = null;
        DataTable dtAllLine = null;
        DataTable dtAssyLine = null;
        DataTable dtMatPart = new DataTable();
        DataTable dtMEP_Repair = new DataTable();
        DataTable dtAndonCurr = new DataTable();
        DataTable dtPlay = new DataTable();
        DataTable dtSpart = new DataTable();
        DataTable dtReqSpart = new DataTable();
        DataTable dtWoType = new DataTable();
        SP_GMES0211 cProc = new SP_GMES0211();
        //SP_GMES0211_NEWTEST cProc = new SP_GMES0211_NEWTEST();
        DataTable dtData = null;
        ResultSet rs = null;
        //DataTable dtSummary = new DataTable();
        string V_P_ACTION = string.Empty, V_P_DATE = string.Empty, V_P_SCAN_TYPE = string.Empty,
                V_P_LOCATION = string.Empty, V_P_PROCESS = string.Empty, V_P_ITPO = string.Empty,
                V_P_LINE = string.Empty, V_P_USER = string.Empty;
        bool _bFormLoaded = false;
        string sound_yn = "N";
        int times = 600;
        int repeat = 1;
        int lineFrom = 1;
        int lineTo = 1;
        int mcFrom = 1;
        int mcTo = 1;
        string andonLine = "0";
        string andonMC = "0";
        string andonTitle = "";
        int result = -1;
        int error = -1;
        bool forceStop = false;
        bool installFont = false;
        //bool chkFirstSearch = false;
        //bool chkManualClick = false;
        string selectedOP_CD = "";
        string selectedDivision = "";
        string mc_line = "";
        string mc_no = "";
        [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)]
                                         string lpFileName);

        //string[] plantC = new string[4] { "21","22","23","24" };

        private Label customToolTip;
        #endregion

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            pictureBox1.Image = Properties.Resources.load;

            int newWidth = 500;
            int newHeight = 500;
            pictureBox1.Size = new Size(newWidth, newHeight);

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox1.Dock = DockStyle.Fill;
            grdCtrl.Controls.Add(pictureBox1);

            int x = (this.ClientSize.Width - pictureBox1.Width) / 2;
            int y = (this.ClientSize.Height - pictureBox1.Height) / 2;

            #region [COMMENT]
            //int x = (grdCtrl.Width) / 2;
            //int y = (grdCtrl.Height) / 2;
            #endregion

            pictureBox1.BackColor = Color.Transparent;
            
            pictureBox1.Visible = false;
            pictureBox1.BringToFront();
            pictureBox1.Location = new Point(x, y);

            #region [COMMENT]
            //allControls.ForEach(
            //    k => 
            //        k.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold)
                    
            //);

            //List<Control> allControls = GetAllControls(this);
            //allControls.ForEach(k => k.Font = new System.Drawing.Font("Verdana", 12));
            
            
            //CopyDependency();
            //readXML();
            //if (!installFont)
            //{
            //    result = AddFontResource(Application.StartupPath + "\\digital-7.ttf");
            //    //AddFontResource(Application.StartupPath + "\\digital-7.ttf");
            //    error = Marshal.GetLastWin32Error();
            //}
            #endregion

            timerCheckAndon.Interval = 1000 * times;
            NewButton = false;
            AddButton = false;
            DeleteButton = false;
            PreviewButton = false;
            PrintButton = false;
            DeleteRowButton = false;
            SaveButton = false;

            #region COMMENT
            //lblPlant.Font = new Font("Tahoma", 15, FontStyle.Bold);
            //lblItemClass.Font = lblPlant.Font;
            ////lblMachineLine.Font = lblPlant.Font;
            ////lblMachineNo.Font = lblPlant.Font;
            //cboPlant.Font = lblPlant.Font;
            //cbItemClassType.Font = lblPlant.Font;
            ////lblCurrMCLine.Font = lblPlant.Font;
            ////lblCurMCNo.Font = lblPlant.Font;
            //lblTitleTotal.Font = lblPlant.Font;
            //lblTitleB.Font = lblPlant.Font;
            //lblTitleBreakdown.Font = lblPlant.Font;
            //lblTitleRepair.Font = lblPlant.Font;
            //lblTitleDone.Font = lblPlant.Font;
            //lblB.Font = lblPlant.Font;
            //lblBreakdown.Font = lblPlant.Font;
            //lblRepair.Font = lblPlant.Font;
            //lblDone.Font = lblPlant.Font;
            //lblValTotal.Font = lblPlant.Font;
            //cboPlant.Properties.AppearanceDropDownHeader.Font = lblPlant.Font;
            //cboPlant.Properties.AppearanceDropDown.Font = lblPlant.Font;
            //cbItemClassType.Properties.AppearanceDropDownHeader.Font = lblPlant.Font;
            //cbItemClassType.Properties.AppearanceDropDown.Font = lblPlant.Font;
            


            //lblAndonLine.Font = new Font("Tahoma", 50, FontStyle.Bold);
            //lblAndonMC.Font = lblAndonLine.Font;
            //lblAndonTitle.Font = lblAndonLine.Font;

            //lblAndonLineVal.Font = new Font("Digital-7", 250, FontStyle.Bold);
            //lblAndonMCVal.Font = lblAndonLineVal.Font;

            //pnlAndon.Location = new Point(
            //                                (this.ClientSize.Width / 2 - pnlPopUp.Size.Width / 2) - 100,
            //                                (this.ClientSize.Height / 2 - pnlPopUp.Size.Height / 2) + 100);
            //pnlAndon.Anchor = AnchorStyles.None;
            #endregion

            lblGroupWS.Font = new Font("Tahoma", 8, FontStyle.Bold);
            lblGroupMEP.Font = new Font("Tahoma", 8, FontStyle.Bold);
            lblRequest.Font = new Font("Tahoma", 8, FontStyle.Bold);
            lblChk.Font = new Font("Tahoma", 8, FontStyle.Bold);
            lblFix.Font = new Font("Tahoma", 8, FontStyle.Bold);
            lblGroupRepair.Font = new Font("Tahoma", 8, FontStyle.Bold);
            lblTitleDone.Visible = false;
            lblDone.Visible = false;

            #region [COMMENT]
            //cboPlant.Location = new Point(195, 14);
            //cbItemClassType.Location = new Point(195, 65);
            ////lblMachineLine.Location = new Point(319, 17);
            ////lblMachineNo.Location = new Point(338, 67);
            ////lblCurrMCLine.Location = new Point(440, 17);
            ////lblCurMCNo.Location = new Point(440, 67);
            #endregion

            dtMatPart.Columns.Add("PART_NAME", typeof(string));
            dtMatPart.Columns.Add("PART_QTY", typeof(Int32));
            dtMatPart.Columns.Add("PART_UNIT", typeof(string));
            SetData(grdSparePart, dtMatPart);

            dtMEP_Repair.Columns.Add("CODE", typeof(string));
            dtMEP_Repair.Columns.Add("DESCRIPTION", typeof(string));
            SetData(grdMEP, dtMEP_Repair);

            riSpart.ValueMember = "PART_NAME";
            riSpart.DisplayMember = "PART_NAME";

            lookEMC_ID.Properties.ValueMember = "Barcode";
            lookEMC_ID.Properties.DisplayMember = "Machine_ID";

            dtReqSpart.Columns.Add("REQ_YMD", typeof(string));
            dtReqSpart.Columns.Add("PART_CD", typeof(string));
            dtReqSpart.Columns.Add("PART_NAME", typeof(string));
            dtReqSpart.Columns.Add("REQ_QTY", typeof(decimal));
            dtReqSpart.Columns.Add("UNIT", typeof(string));
            dtReqSpart.Columns.Add("SPEC", typeof(string));
            dtReqSpart.Columns.Add("RO_DATE", typeof(DateTime));
            dtReqSpart.Columns.Add("PO_DATE", typeof(DateTime));
            dtReqSpart.Columns.Add("ETD", typeof(DateTime));
            dtReqSpart.Columns.Add("IN_DATE", typeof(DateTime));
            dtReqSpart.Columns.Add("IN_QTY", typeof(decimal));
            SetData(grdReqSpart, dtReqSpart);

            fn_GetBase();
            
            pnlPopUp.Location = new Point(
                                            this.ClientSize.Width / 2 - pnlPopUp.Size.Width / 2,
                                            this.ClientSize.Height / 2 - pnlPopUp.Size.Height / 2);

            pnlPopUp.Anchor = AnchorStyles.None;
            pnlPopUp.Visible = false;

            spinEdit1_EditValueChanged(this, null);

            #region [COMMENT]

            //timerOnLoadClick.Enabled = true;
            //timerOnLoadClick.Start();

            //chkAndon.Checked = (sound_yn.ToUpper() == "Y" ? true : false);
            //timerCheckAndon.Enabled = true;
            //timerCheckAndon.Stop();
            #endregion

            setPnlCallTLQC();
            timerRefresh.Enabled = true;
            timerRefresh.Start();
            _bFormLoaded = true;
            cboArea_EditValueChanged(null, null);

            //2024.12.31
            lblWoNoOld.Visible = false;
            lblWoNo.ReadOnly = true;

        }

        public void setPnlCallTLQC(string param_stat = "N")
        {
            if (param_stat == "N")
            {
                btnReqSpart.Visible = false;
                btnReqSpart.Enabled = false;
                pnlCallTLQC.Enabled = false;
                pnlCallTLQC.Visible = false;
                pnlCallTLQC.SendToBack();
            }
            else
                if (param_stat == "Y")
                {
                    btnReqSpart.Visible = true;
                    btnReqSpart.Enabled = true;
                    pnlCallTLQC.Enabled = true;
                    pnlCallTLQC.Visible = true;
                    pnlCallTLQC.BringToFront();
                    pnlCallTLQC.Location = new Point(
                                        this.ClientSize.Width / 2 - pnlCallTLQC.Size.Width / 2,
                                        this.ClientSize.Height / 2 - pnlCallTLQC.Size.Height / 2);
                }

            rbDivision.SelectedIndex = 0;
            rbDivision.Properties.Columns = 3;
            rgQuality.SelectedIndex = 0;
            rgQuality.Properties.Columns = 3;
        }

        public override void QueryClick()
        {
            forceStop = bwAndon.IsBusy;

            if (bwAndon.IsBusy) return;

            base.QueryClick();

            ////if (_bFormLoaded)
            ////{
            //pbProgressShow();
            ////}

            int x = (this.ClientSize.Width - pictureBox1.Width) / 2;
            int y = (this.ClientSize.Height - pictureBox1.Height) / 2;

            pictureBox1.Location = new Point(x, y);

            pictureBox1.Visible = true;

            try
            {
                lblDone.Text = "0";
                lblRepair.Text = "0";
                lblBreakdown.Text = "0";
                lblB.Text = "0";
                lblNot_Confirm.Text = "0";
                lblValTotal.Text = "0";

                if (cboArea.EditValue.ToString() == "OSP" && cboLocation.EditValue.ToString() == "51BT")
                    fn_Search("GET_LAYOUT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString());
                else
                {
                    switch (cboArea.EditValue.ToString())
                    {
                        case "FSS,FGA":
                            if (cboLocation.EditValue.ToString() == "51B1" || cboLocation.EditValue.ToString() == "51C1"
                                || cboLocation.EditValue.ToString() == "51E1" //|| cboLocation.EditValue.ToString() == "51F1"
                                || cboLocation.EditValue.ToString() == "51A1" || cboLocation.EditValue.ToString() == "51A3"
                                //|| cboLocation.EditValue.ToString() == "51G1"
                               )
                                setContentLayout(this.layoutPlantC_FSS_FGA, null);
                            else
                                setContentLayout(this.layoutD_FSS_FGA, null);
                            break;

                        case "UPS":
                            if (cboLocation.EditValue.ToString() != "51D1")
                                setContentLayout(this.layoutPlantC_UPS, null);
                            else
                                setContentLayout(this.layoutPlantD_UPS, null);
                            break;

                        case "UPC,UPN":
                            if (cboLocation.EditValue.ToString() != "51D1")
                                setContentLayout(this.layoutPlantC_UPC_UPN, null);
                            else
                                setContentLayout(this.layoutPlantD_UPC_UPN, null);
                            break;

                        case "PHM,BUF,PHH,PHU":
                            setContentLayout(this.layoutCKP_PH, null);
                            break;

                        case "IPI,IPU":
                            setContentLayout(this.layoutCKP_IP, null);
                            break;

                        case "OSR":
                            setContentLayout(this.layoutOSR, null);
                            break;

                        #region [OLD]
                        /*case "CIN,SKI,INC":
                            setContentLayout(this.layoutAcc_CinSkiInc, null);
                            break;

                        case "UPE,UPF":
                            setContentLayout(this.layoutAcc_UPE_UPF, null);
                            break;

                        case "UPH":
                            setContentLayout(this.layoutAcc_UPH, null);
                            break;*/
                        #endregion

                        case "PUR,BEA,BEM": //2024.10.04 
                            setContentLayout(this.layoutPUR, null);
                            break;

                        case "FGA,FSS,UPC,UPN,UPS": //2024.10.25 
                            if (cboLocation.EditValue.ToString() == "51G1")
                            {
                                setContentLayout(this.layoutPlantG, null); //2025.01.08
                            }
                            else
                            {
                                setContentLayout(this.layoutPlantH, null);
                            }
                            break;

                        case "CIN,SKI,INC,UPE,UPF,UPH": //2024.11.08 
                            setContentLayout(this.layoutAcc, null);
                            break;
                    }

                    fn_Search("GET_LAYOUT_PLANT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("QueryClick() " + ex.ToString());
            }
            finally {
                //pbSetProgressHide();
                if (cboArea.EditValue.ToString() == "OSP") //2024.10.10
                {
                    ShowToolTipAtCell(13, 50);
                }
                pictureBox1.Visible = false;
            }

            dtAndonCurr.Rows.Clear();
        }

        private void fn_GetBase()
        {
            dtData = null;

            dtData = cProc.SetParamData(dtData, "GET_BASE");

            rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

            if (rs != null && rs.ResultDataSet.Tables.Count > 0)
            {
                dtData = rs.ResultDataSet.Tables[0];
                dtData.AcceptChanges();

                if (dtData.Rows.Count > 0)
                {
                    var plant = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "PLANT CD").Select(row => new { CODE = row["CODE"], DESCRIPTION = row["CODE_NAME"] });
                    //var itemClassType = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "ITEM CLASS TYPE").Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"], PLANT_CD = row["PLANT_CD"] });
                    var defective = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "DEFECTIVE").Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"] });
                    var solution = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "SOLUTION").Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"] });
                    //var machine = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "MACHINE").Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"], REMARKS = row["REMARKS"], PLANT_CD = row["PLANT_CD"] });
                    //before var machine = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "MACHINE").Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"], REMARKS = row["REMARKS"], PLANT_CD = row["PLANT_CD"] });
                    var machine = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "MACHINE").Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"], REMARKS = row["REMARKS"], PLANT_CD = row["PLANT_CD"], REMARKS2 = row["REMARKS2"] });
                    var location = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "LOCATION").Select(row => new { CODE = row["CODE"], PLANT_CD = row["PLANT_CD"] });
                    var area = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "AREA").Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"], REMARKS = row["REMARKS"], PLANT_CD = row["PLANT_CD"] });
                    var line = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "LINE").Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"], REMARKS = row["REMARKS"], PLANT_CD = row["PLANT_CD"] });
                    var spart = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS2") == "SPART").Select(row => new { PART_CODE = row["CODE"], PART_NAME = row["CODE_NAME"], SPEC = row["REMARKS"], UNIT = row["PLANT_CD"], GROUP_CD = row["REMARKS1"] });
                    //WO_TYPE
                    var woType = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "WO_TYPE").Select(row => new { CODE = row["CODE"], DESCRIPTION = row["CODE_NAME"] });

                    if (plant.Any())
                    {
                        dtPlant = ConvertToDataTable(plant);

                        if (dtPlant != null && dtPlant.Rows.Count > 0)
                        {
                            cboPlant.Properties.DisplayMember = "DESCRIPTION";
                            cboPlant.Properties.ValueMember = "CODE";
                            cboPlant.Properties.DataSource = dtPlant;
                            cboPlant.SelectedIndex = 0;
                        }
                    }

                    if (line.Any())
                    {
                        dtAllLine = ConvertToDataTable(line);
                    }

                    if (location.Any())
                    {
                        dtLocation = ConvertToDataTable(location);
                    }

                    if (area.Any())
                    {
                        dtArea = ConvertToDataTable(area);
                    }


                    //if (itemClassType.Any())
                    //{
                    //    dtItemClass_type = ConvertToDataTable(itemClassType);
                    cboPlant_EditValueChanged(this, null);
                    //}

                    var labour = dtData.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "MEP" || row.Field<string>("REMARKS1") == "WORKSHOP" || row.Field<string>("REMARKS1") == "QC").Select(row => new { CODE = row["CODE"], DESCRIPTION = row["CODE_NAME"], REMARKS1 = row["REMARKS1"], PLANT_CD = row["PLANT_CD"] });
                    if (labour.Any())
                    {
                        dtLabour = ConvertToDataTable(labour);
                    }

                    if (defective.Any())
                    {
                        dtDefect = ConvertToDataTable(defective);
                    }

                    if (solution.Any())
                    {
                        dtSolution = ConvertToDataTable(solution);
                    }

                    if (machine.Any())
                    {
                        dtMachine = ConvertToDataTable(machine);
                    }

                    if (spart.Any())
                    {
                        dtSpart = ConvertToDataTable(spart);
                    }

                    if (woType.Any())
                    {
                        dtWoType = ConvertToDataTable(woType);
                    }
                }
            }
        }

        private void fn_Search(
                string param_type, string param_plant = "", string param_location = "", string param_area = "", string param_item_class_type = ""
                , string param_division = "", string param_mc_line = "", string param_mc_no = "", string param_date = ""
        )
        {
            try
            {
                //if (_bFormLoaded)
                //{
                //    pbProgressShow();
                //}

                lblValTotal.Text = (Convert.ToInt32(lblBreakdown.Text) + Convert.ToInt32(lblRepair.Text) + Convert.ToInt32(lblB.Text) + Convert.ToInt32(lblNot_Confirm.Text)).ToString();

                dtData = null;

                dtData = cProc.SetParamData(dtData,
                                                param_type,
                                                param_plant,
                                                param_location,
                                                param_area,
                                                param_item_class_type,
                                                param_division,
                                                param_mc_line,
                                                param_mc_no,
                                                param_date
                                               );

                rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];

                    dtData.AcceptChanges();

                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        if (param_type != "SUMMARY")
                        {
                            if (param_type != "GET_DATA")
                            {
                                if (param_type == "GET_SPART")
                                {
                                    SetData(grdReqSpart, dtData);

                                    fn_FormatGrid("SPART");
                                }
                                else
                                //if (dtData != null && dtData.Rows.Count > 0)
                                //{
                                if (cboArea.EditValue.ToString() == "OSP")
                                {
                                  #region OSP
                                        var dtGrid = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "DATA");
                                        //var dtLabel = dtData.AsEnumerable().Where(row => row.Field<decimal>("MACHINE_SEQ") == "SUMMARY");
                                        var cis = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SET_CIS");
                                        var onoff = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SET_ON_OFF");

                                        if (!dtGrid.Any())
                                            return;

                                        if (!onoff.Any())
                                            return;
                                        else
                                            dtOnOff = onoff.CopyToDataTable();

                                        gvwData.BeginUpdate();
                                        gvwData.Columns.Clear();
                                        
                                        DataTable dtNew = dtGrid.CopyToDataTable();
                                        for (int col = 0; col < dtNew.Columns.Count; col++)
                                        {
                                            for (int row = 0; row < dtNew.Rows.Count; row++)
                                            {
                                                string val = dtNew.Rows[row][col].ToString();
                                                val = (val.Length > 1 ? val.Substring(val.Length - 1, 1) : val);

                                                if (val == "F")
                                                {
                                                    dtNew.Rows[row][col] = "";
                                                    dtNew.AcceptChanges();
                                                }
                                            }
                                        }

                                        grdCtrl.DataSource = dtNew; //dtGrid.CopyToDataTable();
                                        gvwData.PopulateColumns(grdCtrl.DataSource);
                                        gvwData.EndUpdate();

                                        if (cis.Any())
                                        {
                                            dtCIS.Clear();
                                            dtCIS.Dispose();
                                            dtCIS = cis.CopyToDataTable();

                                            if (dtCIS == null)
                                                return;
                                            else
                                                fn_setHeaderColor();
                                        }

                                        fn_FormatGrid("WOF");


                                        var done = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("1") == "F");
                                        var repair = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("1") == "C");
                                        var breakdown = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("1") == "R");
                                        var black = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("1").Contains("B"));
                                        var notConfirm = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("1").Contains("D"));

                                        lblDone.Text = (done.Any() ? done.CopyToDataTable().Rows[0]["2"].ToString() : "0");
                                        lblRepair.Text = (repair.Any() ? repair.CopyToDataTable().Rows[0]["2"].ToString() : "0");
                                        lblBreakdown.Text = (breakdown.Any() ? breakdown.CopyToDataTable().Rows[0]["2"].ToString() : "0");
                                        lblB.Text = (black.Any() ? black.CopyToDataTable().Rows[0]["2"].ToString() : "0");
                                        lblNot_Confirm.Text = (notConfirm.Any() ? notConfirm.CopyToDataTable().Rows[0]["2"].ToString() : "0");
                                        //lblValTotal.Text = (Convert.ToInt32(lblBreakdown.Text) + Convert.ToInt32(lblRepair.Text) + Convert.ToInt32(lblB.Text) + Convert.ToInt32(lblNot_Confirm.Text)).ToString();
                                        lblValTotal.Text = (Convert.ToInt32(lblBreakdown.Text) + Convert.ToInt32(lblRepair.Text) + Convert.ToInt32(lblB.Text)).ToString(); //2023-05-15
                                        #endregion
                                }
                                else
                                {
                                    #region NOT OSP
                                    //if (dtData != null && dtData.Rows.Count > 0)
                                    //{
                                    var data = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "DATA");

                                    if (data.Any())
                                    {
                                        switch (cboArea.EditValue.ToString())
                                        {
                                            case "FSS,FGA":
                                                if (
                                                    cboLocation.EditValue.ToString() == "51B1" || cboLocation.EditValue.ToString() == "51C1"
                                                    || cboLocation.EditValue.ToString() == "51E1" //|| cboLocation.EditValue.ToString() == "51F1"
                                                    || cboLocation.EditValue.ToString() == "51A1" || cboLocation.EditValue.ToString() == "51A3"
                                                    //|| cboLocation.EditValue.ToString() == "51G1"
                                                    )
                                                    setContentLayout(this.layoutPlantC_FSS_FGA, data.CopyToDataTable());
                                                else
                                                    //if (cboLocation.EditValue.ToString() == "51D1")
                                                    setContentLayout(this.layoutD_FSS_FGA, data.CopyToDataTable());
                                                break;

                                            case "UPS":
                                                if (cboLocation.EditValue.ToString() != "51D1")
                                                    setContentLayout(this.layoutPlantC_UPS, data.CopyToDataTable());
                                                else
                                                    setContentLayout(this.layoutPlantD_UPS, data.CopyToDataTable());
                                                break;
                                            case "UPC,UPN":
                                                if (cboLocation.EditValue.ToString() != "51D1")
                                                    setContentLayout(this.layoutPlantC_UPC_UPN, data.CopyToDataTable());
                                                else
                                                    setContentLayout(this.layoutPlantD_UPC_UPN, data.CopyToDataTable());
                                                break;
                                            case "PHM,BUF,PHH,PHU":
                                                setContentLayout(this.layoutCKP_PH, data.CopyToDataTable());
                                                break;
                                            case "UPC,UPN,UPS,FSS,FGA":
                                                setContentLayout(this.layoutPlantMMTL, data.CopyToDataTable());
                                                break;
                                            case "IPI,IPU":
                                                setContentLayout(this.layoutCKP_IP, data.CopyToDataTable());
                                                break;
                                            case "OSR":
                                                setContentLayout(this.layoutOSR, data.CopyToDataTable());
                                                break;
                                            /*case "CIN,SKI,INC":
                                                setContentLayout(this.layoutAcc_CinSkiInc, data.CopyToDataTable());
                                                break;
                                            case "UPE,UPF":
                                                setContentLayout(this.layoutAcc_UPE_UPF, data.CopyToDataTable());
                                                break;
                                            case "UPH":
                                                setContentLayout(this.layoutAcc_UPH, data.CopyToDataTable());
                                                break;*/
                                            case "PUR,BEA,BEM":
                                                setContentLayout(this.layoutPUR, data.CopyToDataTable()); //2024.10.12
                                                break;
                                            case "FGA,FSS,UPC,UPN,UPS":
                                                if (cboLocation.EditValue.ToString() == "51G1")
                                                {
                                                    setContentLayout(this.layoutPlantG, data.CopyToDataTable()); //2025.01.08
                                                }
                                                else
                                                {
                                                    setContentLayout(this.layoutPlantH, data.CopyToDataTable()); //2024.10.25
                                                }
                                                break;
                                            case "CIN,SKI,INC,UPE,UPF,UPH":
                                                setContentLayout(this.layoutAcc, data.CopyToDataTable()); //2024.11.08
                                                break;
                                        }
                                    }

                                    var done = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("NEW_STATUS") == "F");
                                    var repair = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("NEW_STATUS") == "C");
                                    var breakdown = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("NEW_STATUS") == "R");
                                    var black = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("NEW_STATUS").Contains("B"));
                                    var notConfirm = dtData.AsEnumerable().Where(row => row.Field<string>("DIV") == "SUMMARY" && row.Field<string>("NEW_STATUS").Contains("D"));

                                    lblDone.Text = (done.Any() ? done.CopyToDataTable().Rows[0]["CONT"].ToString() : "0");
                                    lblRepair.Text = (repair.Any() ? repair.CopyToDataTable().Rows[0]["CONT"].ToString() : "0");
                                    lblBreakdown.Text = (breakdown.Any() ? breakdown.CopyToDataTable().Rows[0]["CONT"].ToString() : "0");
                                    lblB.Text = (black.Any() ? black.CopyToDataTable().Rows[0]["CONT"].ToString() : "0");
                                    lblNot_Confirm.Text = (notConfirm.Any() ? notConfirm.CopyToDataTable().Rows[0]["CONT"].ToString() : "0");
                                    lblValTotal.Text = (Convert.ToInt32(lblBreakdown.Text) + Convert.ToInt32(lblRepair.Text) + Convert.ToInt32(lblB.Text) + Convert.ToInt32(lblNot_Confirm.Text)).ToString();
                                    //}
                                    #endregion
                                }
                                //}
                            }
                            else
                            {
                                //if (dtData != null && dtData.Rows.Count > 0)
                                //{
                                dtGet = dtData;
                                //}
                            }
                        }
                        else
                            if (param_type == "SUMMARY")
                            {
                                gvwSummary.BeginUpdate();
                                gvwSummary.Columns.Clear();
                                grdSummary.DataSource = dtData;
                                gvwSummary.PopulateColumns(grdSummary.DataSource);
                                gvwSummary.EndUpdate();

                                fn_FormatGrid("SUMMARY");
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                //SetErrorMessage(ex);
                //throw;
                //MessageBoxW("fn_Search " + ex.ToString());
            }
            finally
            {
                if (_bFormLoaded)
                {
                    lblLastUpdate.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");

                    //pbSetProgressHide();

                    if (!pnlPopUp.Visible)
                    {
                        timerRefresh.Stop();
                        timerRefresh.Start();
                    }

                    //if (chkAndon.Checked && !pnlPopUp.Visible)
                    //{
                    //    timerCheckAndon.Stop();
                    //    timerCheckAndon.Start();
                    //}
                }
            }
        }

        private void fn_FormatGrid(string param)
        {
            try
            {
                if (param == "WOF")
                {
                    int width = 0;
                    width = (grdCtrl.Width - gvwData.IndicatorWidth) / (gvwData.Columns.Count);

                    if (chkFit.Checked)
                        width += 25;


                    for (int i = 0; i < gvwData.Columns.Count; i++)
                    {
                        gvwData.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvwData.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        gvwData.Columns[i].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        gvwData.Columns[i].OptionsColumn.AllowEdit = false;
                        gvwData.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        gvwData.Columns[i].Width = width;
                        gvwData.Columns[i].AppearanceHeader.Font = new Font(gvwData.Columns[i].AppearanceHeader.Font.FontFamily, 8f);
                        gvwData.Columns[i].AppearanceHeader.Font = new Font(gvwData.Columns[i].AppearanceHeader.Font, FontStyle.Bold);

                        if (i <= 1)
                            gvwData.Columns[i].Visible = false;

                        gvwData.Columns[i].ColumnEdit = repoMemoEdit;


                    }

                    gvwData.RowHeight = ((grdCtrl.Height - gvwData.ColumnPanelRowHeight - 15) / gvwData.RowCount) - (chkFit.Checked ? 1 : 0);

                    //gvwData.OptionsView.ColumnAutoWidth = true;


                }
                else
                    if (param == "SPART")
                    {
                        for (int i = 0; i < gvwReqSpart.Columns.Count; i++)
                        {
                            gvwReqSpart.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            gvwReqSpart.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                            if (gvwReqSpart.Columns[i].FieldName == "PART_CD" || gvwReqSpart.Columns[i].FieldName == "UNIT" || gvwReqSpart.Columns[i].FieldName == "SPEC")
                                gvwReqSpart.Columns[i].OptionsColumn.AllowEdit = false;

                            if (gvwReqSpart.Columns[i].FieldName == "PART_CD")
                            {
                                gvwReqSpart.Columns[i].Width = 95;
                                gvwReqSpart.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            }
                            else
                                if (gvwReqSpart.Columns[i].FieldName == "PART_NAME" || gvwReqSpart.Columns[i].FieldName == "SPEC")
                                    gvwReqSpart.Columns[i].Width = 180;
                                else
                                    if (gvwReqSpart.Columns[i].FieldName == "REQ_QTY" || gvwReqSpart.Columns[i].FieldName == "IN_QTY")
                                        gvwReqSpart.Columns[i].Width = 60;
                                    else
                                        if (gvwReqSpart.Columns[i].FieldName == "UNIT")
                                        {
                                            gvwReqSpart.Columns[i].Width = 50;
                                            gvwReqSpart.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        }
                                        else
                                            if (gvwReqSpart.Columns[i].FieldName == "RO_DATE" || gvwReqSpart.Columns[i].FieldName == "PO_DATE"
                                                || gvwReqSpart.Columns[i].FieldName == "ETD" || gvwReqSpart.Columns[i].FieldName == "IN_DATE")
                                            {
                                                gvwReqSpart.Columns[i].Width = 80;
                                                gvwReqSpart.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                            }
                        }
                    }
                    else
                    if (param == "SUMMARY")
                    {
                        for (int i = 0; i < gvwSummary.Columns.Count; i++)
                        {
                            gvwSummary.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            gvwSummary.Columns[i].OptionsColumn.AllowEdit = false;
                            gvwSummary.Columns[i].OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

                            if (i > 0)
                                gvwSummary.Columns[i].Width = 130;

                            if (i == 0)
                            {
                                gvwSummary.Columns[i].Width = 100;
                                gvwSummary.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            }

                        }

                        gvwSummary.Columns["LINE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                        gvwSummary.Columns["LINE"].SummaryItem.DisplayFormat = "TOTAL";
                        gvwSummary.Columns["TOTAL"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gvwSummary.Columns["TOTAL"].SummaryItem.FieldName = "TOTAL";
                        gvwSummary.Columns["TOTAL"].SummaryItem.DisplayFormat = "{0:N0}";
                        gvwSummary.Columns["OFF"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gvwSummary.Columns["OFF"].SummaryItem.FieldName = "OFF";
                        gvwSummary.Columns["OFF"].SummaryItem.DisplayFormat = "{0:N0}";
                        gvwSummary.Columns["BREAKDOWN"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gvwSummary.Columns["BREAKDOWN"].SummaryItem.FieldName = "BREAKDOWN";
                        gvwSummary.Columns["BREAKDOWN"].SummaryItem.DisplayFormat = "{0:N0}";
                        gvwSummary.Columns["UNDER REPAIR"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gvwSummary.Columns["UNDER REPAIR"].SummaryItem.FieldName = "UNDER REPAIR";
                        gvwSummary.Columns["UNDER REPAIR"].SummaryItem.DisplayFormat = "{0:N0}";
                        gvwSummary.Columns["DONE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gvwSummary.Columns["DONE"].SummaryItem.FieldName = "DONE";
                        gvwSummary.Columns["DONE"].SummaryItem.DisplayFormat = "{0:N0}";
                    }


                //for (int col = 0; col < gvwData.Columns.Count; col++)
                //{
                //    for (int row = 0; row < gvwData.RowCount; row++)
                //    {
                //        string fieldName = gvwData.Columns[col].ToString();
                //        string val = gvwData.GetRowCellValue(row, fieldName).ToString();
                //        val = (val.Length > 1 ? val.Substring(val.Length - 1, 1) : val);

                //        //string val = gvwData.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString();
                //        //val = (val.Length > 1 ? val.Substring(val.Length - 1, 1) : val);
                //        if (val == "F")
                //        {
                //            gvwData.SetRowCellValue(row, fieldName, "");
                //            gvwData.UpdateCurrentRow();
                //        }
                //    }
                //}
            }
            catch (Exception)
            {

            }
        }

        private bool fn_ChkPass(string p_user_id, string p_current_pass)
        {
            try
            {
                SP_GMES0211_1 cProc = new SP_GMES0211_1("Q");

                dtData = null;

                dtData = cProc.SetParamData(dtData, p_user_id, p_current_pass);

                rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    dtData.AcceptChanges();
                    if (Int16.Parse(dtData.Rows[0]["RESULT"].ToString()) > 0)
                        return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBoxW(ex.ToString());
            }

            return false;
        }

        public void fn_Save(
            string p_action, string p_plant_cd, string p_location, string p_op_cd = "", string p_item_class = "", string p_division = "", string p_req_ymd = "", string p_req_hms = "",
            string p_req_emp_id = "", string p_req_emp_nm = "", string p_req_msg = "", string p_wo_ymd = "", string p_wo_seq = "", string p_wo_no = "", string p_mc_line = "", 
            string p_mc_no = "", string p_status = "", string p_division_type = "", string p_chk_ymd = "", string p_chk_hms = "", string p_chk_emp_id = "", string p_chk_emp_nm = "",
            string p_chk_msg = "", string p_mc_id = "", string p_mc_type = "", string p_defec_cd = "", string p_lost_time_yn = "", string p_param1 = "", string p_param2 = "",
            string p_param3 = "", string p_param4 = "", string p_param5 = "", string p_repair_ymd = "", string p_repair_hms = "", string p_repair_emp_id = "", string p_repair_emp_nm = "", 
            string p_solu_cd = "", string p_mat_part = "", string p_cfm_empid = "", string p_cfm_emp_nm = ""
        )
        {
            SP_GMES0211 cProc = new SP_GMES0211("S");
            //SP_GMES0211_NEWTEST cProc = new SP_GMES0211_NEWTEST("S");
            dtData = null;

            #region [COMMENT]
            //string wh_cd = "";
            //if (p_item_class == "OS" || p_item_class == "PU")
            //    wh_cd = "51BT";
            //else
            //    if (p_item_class == "II" || p_item_class == "PP")
            //        wh_cd = "51IP";
            #endregion

            dtData = cProc.SetParamDataSave(dtData
                , p_action, p_plant_cd, p_location, p_op_cd, p_item_class, p_division, p_req_ymd, p_req_hms, p_req_emp_id, p_req_emp_nm, p_req_msg
                , p_wo_ymd, p_wo_seq, p_wo_no, p_mc_line, p_mc_no, p_chk_ymd, p_chk_hms, p_chk_emp_id, p_chk_emp_nm, p_chk_msg, p_mc_id
                , p_mc_type, p_defec_cd, p_repair_ymd, p_repair_hms, p_repair_emp_id, p_repair_emp_nm, p_solu_cd, p_mat_part
                , p_status, SessionInfo.UserID, p_cfm_empid, p_cfm_emp_nm, p_division_type, p_lost_time_yn, p_param1, p_param2,p_param3, p_param4, p_param5
            );

            if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
            {
                //MessageBoxW("Success Saved");
            }
            else
            {
                //MessageBoxW("Failed Saved");
            }
        }

        private void fn_SavePass(string p_user_id, string p_current_pass, string p_new_pass, string p_updater, string p_update_pc)
        {
            try
            {
                SP_GMES0211_1 cProc = new SP_GMES0211_1("S");

                dtData = null;

                dtData = cProc.SetParamDataSave(dtData, p_user_id, p_current_pass, p_new_pass, p_updater, p_update_pc);

                if (CommonProcessSave(dtData, cProc.ProcName, cProc.GetParamInfo(), null))
                {
                    MessageBoxW("Success Saved");
                }
                else
                {
                    MessageBoxW("Failed Saved");
                }
            }
            catch (Exception ex)
            {
                MessageBoxW(ex.ToString());
            }
        }

        private void fn_GetDataAndon()
        {
            try
            {
                dtData = null;

                dtData = cProc.SetParamData(dtData, "GET_ANDON", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString());

                rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo(), false, 90000, "", true);

                if (rs != null && rs.ResultDataSet.Tables.Count > 0)
                {
                    dtData = rs.ResultDataSet.Tables[0];
                    dtData.AcceptChanges();

                    if (dtData.Rows.Count > 0)
                    {
                        //if (dtAndonCurr.Rows.Count == 0)
                        //{
                        //callSound(dtAndonCurr);
                        if (!bwAndon.IsBusy)
                        {
                            var dt = dtData.AsEnumerable().Where(row => Convert.ToInt32(row.Field<string>("MACHINE_LINE")) >= lineFrom && Convert.ToInt32(row.Field<string>("MACHINE_LINE")) <= lineTo);

                            if (!dt.Any())
                                return;

                            dtAndonCurr = dtData;

                            timerBlink.Start();
                            bwAndon.RunWorkerAsync();
                        }
                        //}
                        //else
                        //{
                        //    var differences = dtData.AsEnumerable().Except(dtAndonCurr.AsEnumerable(), DataRowComparer.Default);
                        //    DataTable tmp = null;

                        //    if (differences.Any())
                        //    {
                        //        tmp = differences.CopyToDataTable();

                        //        foreach (DataRow row in tmp.Rows)
                        //        {
                        //            dtAndonCurr.Rows.Add(row.ItemArray);
                        //        }

                        //        dtAndonCurr.AcceptChanges();
                        //        dtPlay = tmp;
                        //        pnlAndon.Visible = true;
                        //        if (!bwAndon.IsBusy)
                        //            bwAndon.RunWorkerAsync();
                        //        //callSound(tmp);

                        //        timerBlink.Start();
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxW("fn_GetDataAndon " + ex.ToString());
            }
        }

        private void callSound(DataTable param_src)
        {
            foreach (DataRow row in param_src.Rows)
            {
                if (
                    (
                        Convert.ToInt16(row["MACHINE_LINE"]) >= lineFrom && Convert.ToInt16(row["MACHINE_LINE"]) <= lineTo
                        && Convert.ToInt16(row["MACHINE_NO"]) >= mcFrom && Convert.ToInt16(row["MACHINE_NO"]) <= mcTo
                    )
                    || (
                            //Convert.ToInt16(row["MACHINE_NO"]) == 98 || Convert.ToInt16(row["MACHINE_NO"]) == 99 || Convert.ToInt16(row["MACHINE_NO"]) == 100
                            Convert.ToInt16(row["MACHINE_NO"]) == 98 || Convert.ToInt16(row["MACHINE_NO"]) == 99 || Convert.ToInt16(row["MACHINE_NO"]) == 100 || Convert.ToInt16(row["MACHINE_NO"]) == 101 || Convert.ToInt16(row["MACHINE_NO"]) == 102 || Convert.ToInt16(row["MACHINE_NO"]) == 103 //2023-05-11
                        )
                   )
                {
                    andonLine = row["MACHINE_LINE"].ToString().PadLeft(2, '0');
                    andonMC = row["MACHINE_NO"].ToString().PadLeft(2, '0');
                    andonTitle = row["DIVISION_NAME"].ToString();

                    for (int i = 0; i < repeat; i++)
                    {
                        if (forceStop) break;
                        playSound(Convert.ToInt16(row["MACHINE_LINE"]), Convert.ToInt16(row["MACHINE_NO"]));
                    }
                }
            }

            //UPDATE SOUND YN
            string wo_no = "";
            foreach (DataRow row in param_src.Rows)
            {
                if (
                    (Convert.ToInt16(row["MACHINE_LINE"]) >= lineFrom && Convert.ToInt16(row["MACHINE_LINE"]) <= lineTo
                    && Convert.ToInt16(row["MACHINE_NO"]) >= mcFrom && Convert.ToInt16(row["MACHINE_NO"]) <= mcTo
                    )
                    || (
                        //Convert.ToInt16(row["MACHINE_NO"]) == 98 || Convert.ToInt16(row["MACHINE_NO"]) == 99 || Convert.ToInt16(row["MACHINE_NO"]) == 100
                        Convert.ToInt16(row["MACHINE_NO"]) == 98 || Convert.ToInt16(row["MACHINE_NO"]) == 99 || Convert.ToInt16(row["MACHINE_NO"]) == 100 || Convert.ToInt16(row["MACHINE_NO"]) == 101 || Convert.ToInt16(row["MACHINE_NO"]) == 102 || Convert.ToInt16(row["MACHINE_NO"]) == 103 //2023-05-11
                        )
                   )
                    
                {
                    wo_no += row["WO_NO"].ToString() + "|";
                }
            }

            if (param_src.Rows.Count > 0 && wo_no.Length > 0)
            {
                wo_no = wo_no.Remove(wo_no.Length - 1, 1);
                fn_Save("UPDATE_SOUND", cboPlant.EditValue.ToString(), cbItemClassType.EditValue.ToString(), "", "", "", "", "", "", "", "", "", wo_no, "", "", "Y");
            }

        }

        public static DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        private void gvwResult_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            #region [COMMENT]
            //GridView view = sender as GridView;
            //string val = view.GetRowCellDisplayText(e.RowHandle, e.Column);

            //if (e.RowHandle == 0)
            //{
            //    e.Appearance.Font = new Font("Tahoma", 8, FontStyle.Bold);
            //}

            ////int fs = Convert.ToInt32(val);

            //if (e.RowHandle == 3 && e.Column.AbsoluteIndex > 0 && Convert.ToInt32(val.Replace(",", "")) < 0)
            //{
            //    e.Appearance.ForeColor = Color.Red;
            //}
            #endregion
        }

        private void cboPlant_EditValueChanged(object sender, EventArgs e)
        {
            #region
            //var itemClsType = dtItemClass_type.AsEnumerable().Where(row => row.Field<string>("PLANT_CD") == cboPlant.EditValue.ToString()).Select(row => new { CODE = row["CODE"], DESCRIPTION = row["CODE_NAME"] });
            //if (itemClsType.Any())
            //{
            //    if (dtItemClass_type != null && dtItemClass_type.Rows.Count > 0)
            //    {
            //        cbItemClassType.Properties.DisplayMember = "CODE";
            //        cbItemClassType.Properties.ValueMember = "CODE";
            //        cbItemClassType.Properties.DataSource = ConvertToDataTable(itemClsType);
            //        cbItemClassType.SelectedIndex = 0;
            //    }
            //}
            #endregion

            var location = dtLocation.AsEnumerable().Where(row => row.Field<string>("PLANT_CD") == cboPlant.EditValue.ToString()).Select(row => new { CODE = row["CODE"] });

            cboLocation.Properties.DataSource = null;

            if (location.Any())
            {
                if (dtLocation != null && dtLocation.Rows.Count > 0)
                {
                    cboLocation.Properties.DisplayMember = "CODE";
                    cboLocation.Properties.ValueMember = "CODE";
                    cboLocation.Properties.DataSource = ConvertToDataTable(location);
                    cboLocation.SelectedIndex = 0;
                }
            }


        }

        private void gvwData_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
            {
                e.Info.DisplayText = "M/C  ~  LINE";
            }

            if (e.Info.Kind != DevExpress.Utils.Drawing.IndicatorKind.Header)
            {
                e.Info.DisplayText = (grdCtrl.DataSource as DataTable).Rows[e.RowHandle]["MACHINE_SEQ"].ToString();
            }

            //if (e.Info.DisplayText == "CUTTING" || e.Info.DisplayText == "TRIMMING" || e.Info.DisplayText == "HEATER"
            //    || e.Info.DisplayText == "CLEANING" || e.Info.DisplayText == "THOMSON"
            //   )
            if (Regex.IsMatch(e.Info.DisplayText.ToString(), "[a-z]", RegexOptions.IgnoreCase) && !e.Info.DisplayText.Contains("M/C"))
            {
                e.Appearance.BackColor = Color.LemonChiffon;
            }

            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        private void gvwData_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Value != null && e.Value.ToString() != " " && e.Value.ToString() != "")
            {
                string val = e.Value.ToString();
                int idx = val.IndexOf("~");
                e.DisplayText = val.Substring(0, (val.Length - 1) - (val.Length - 1 - idx));
            }
        }

        private void gvwData_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            string val = view.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString();
            val = (val.Length > 1 ? val.Substring(val.Length - 1, 1) : val); 


            if (val != "" && val != " ")
            {
                if (val == "R")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                }
                else if (val == "C")
                {
                    e.Appearance.BackColor = Color.Yellow;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (val == "F")
                {
                    //gvwData.SetRowCellValue(e.RowHandle, e.Column.FieldName, "");
                    //gvwData.UpdateCurrentRow();
                    //gvwData.RowUpdated
                    //e.Appearance.BackColor = Color.LightGreen;
                    //e.Appearance.ForeColor = Color.Black;
                }
                else if (val == "B")
                {
                    e.Appearance.BackColor = Color.Black;
                    e.Appearance.ForeColor = Color.White;
                }
                else if (val == "D")
                {
                    e.Appearance.BackColor = Color.Gold;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Column.ColumnHandle > 1)
            {
                for (int j = 2; j < dtOnOff.Columns.Count; j++)
                {
                    string chk = dtOnOff.Rows[e.RowHandle][j].ToString();

                    if (e.Column.ColumnHandle == j && (chk == "O" || chk == "")) //&& view.GetRowCellValue(e.RowHandle, view.Columns[j].FieldName).ToString() == "")
                        e.Appearance.BackColor = Color.Gray;
                }
            }
        }

        private void gvwData_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            #region [COMMENT]
            //GridView view = sender as GridView;
            //string val = view.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString();
            //val = (val.Length > 1 ? val.Substring(val.Length - 1, 1) : val);
            ////if (val != "" && val != " ")
            ////{
            //    Pen brush = new Pen(Color.FromArgb(255, 195, 215, 245));
            //    Rectangle r = e.Bounds;

            //    switch (val)
            //    {
            //        case "R" :
            //            e.Cache.FillRectangle(Color.Red, e.Bounds);
            //            break;
            //        case "C" :
            //            e.Cache.FillRectangle(Color.Yellow, e.Bounds);
            //            break;
            //        case "F":
            //            e.Cache.FillRectangle(Color.LightGreen, e.Bounds);
            //            break;
            //        case "B":
            //            e.Cache.FillRectangle(Color.Black, e.Bounds);
            //            break;
            //    }

            //    e.Graphics.DrawRectangle(brush, r.X, r.Y, r.Width, r.Height);
            //    e.Appearance.DrawString(e.Cache, e.DisplayText, r);
            //    e.Handled = true;
            ////} 
            #endregion
        }

        private void gvwData_MouseDown(object sender, MouseEventArgs e)
        {
            GridViewEx gvw = (GridViewEx)sender;
            GridHitInfo hi = gvw.CalcHitInfo(e.Location);
            GridViewInfo gvwInfo = gvw.GetViewInfo() as GridViewInfo;
            GridCellInfo cellInfo = gvwInfo.GetGridCellInfo(hi);

            if (hi.HitTest == GridHitTest.EmptyRow) return;

            // Call TL / QC
            if (e.Button == System.Windows.Forms.MouseButtons.Right && hi.HitTest != GridHitTest.EmptyRow && hi.HitTest != GridHitTest.Column && hi.HitTest != GridHitTest.RowIndicator)
            {
                if (cellInfo.Appearance.BackColor.Name == "Gray")
                    return;

                if (pnlPopUp.Visible)
                    return;

                string mcLine = "";
                string mcNo = "";

                switch(gvw.GetRowCellValue(hi.RowHandle, "MACHINE_SEQ").ToString())
                {
                    case "CUTTING":
                        mcNo = "98";
                        break;
                    case "TRIMMING MC": //ANTISIPASI ERROR
                        mcNo = "99";
                        break;
                    case "TRIMMING": //ANTISIPASI ERROR
                        mcNo = "99";
                        break;
                    case "TRIM.MACHINE": //ANTISIPASI ERROR
                        mcNo = "99";
                        break;
                    case "HEATER":
                        mcNo = "100";
                        break;
                    case "CLEANING":
                        mcNo = "101";
                        break;
                    //case "THOMSON":
                    case "MTL.DETECTOR":
                        mcNo = "102";
                        break;
                    case "TRIMMING KNF": //ANTISIPASI ERROR
                        mcNo = "103";
                        break;
                    case "TRIM.KNIFE": //ANTISIPASI ERROR
                        mcNo = "103";
                        break;
                    case "TRIMMING KNIFE": //ANTISIPASI ERROR
                        mcNo = "103";
                        break;
                    default:
                        mcNo = gvw.GetRowCellValue(hi.RowHandle, "MACHINE_SEQ").ToString();
                        break;
                }

                mcLine = gvw.Columns[hi.Column.AbsoluteIndex].FieldName;

                showPopUp_CallTLQC("OSP", mcLine, mcNo.ToString(), null);

                #region [COMMENT]
                //if (MessageBox.Show("Do you want to Call " + " TL " + " for " + "OSP" + " M /C Line : " + mcLine + "  M/C No : " + gvw.GetRowCellValue(hi.RowHandle, "MACHINE_SEQ").ToString() + " ? ", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                //    fn_Save("CALL_TL_QC", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString(), "", "TL", "", "", "", "", "", "", "", "", mcLine, mcNo);
                //else
                //    return;
                #endregion
            }
            else
            if (e.Clicks == 2 && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (pnlCallTLQC.Visible)
                    return;

                #region DOUBLE CLICK LEFT MOUSE ON GRID
                if (cellInfo.Appearance.BackColor.Name == "Gray")
                    return;

                //timerCheckAndon.Stop();
                string clrStatus = cellInfo.CellValue.ToString();
                int idx = clrStatus.IndexOf("~");
                clrStatus = clrStatus.Substring(idx + 1, clrStatus.Length - 1 - idx);
                clrStatus = (clrStatus.Length > 1 ? clrStatus.Substring(0, 1) : clrStatus);
                //string op_cd = "";
                string mc_no = "";
                switch (gvw.GetRowCellValue(hi.RowHandle, "MACHINE_SEQ").ToString())
                {
                    case "CUTTING":
                        //op_cd = "OSC";
                        //selectedOP_CD = "OSC";
                        mc_no = "98";
                        break;
                    case "TRIMMING MC":
                        mc_no = "99";
                        break;
                    case "TRIMMING":
                        //op_cd = "OST";
                        //selectedOP_CD = "OST";
                        mc_no = "99";
                        break;
                    case "TRIM.MACHINE":
                        mc_no = "99";
                        break;
                    case "HEATER":
                        //op_cd = "OSH";
                        //selectedOP_CD = "OSH";
                        mc_no = "100";
                        break;
                    case "CLEANING":
                        //op_cd = "OSH";
                        //selectedOP_CD = "OSH";
                        mc_no = "101";
                        break;
                    //case "THOMSON":
                    case "MTL.DETECTOR":
                        //op_cd = "OSH";
                        //selectedOP_CD = "OSH";
                        mc_no = "102";
                        break;
                    case "TRIMMING KNF": //ADD SEQUENCE
                        mc_no = "103";
                        break;
                    case "TRIM.KNIFE":
                        mc_no = "103";
                        break;
                    case "TRIMMING KNIFE":
                        mc_no = "103";
                        break;
                    default:
                        mc_no = gvw.GetRowCellValue(hi.RowHandle, "MACHINE_SEQ").ToString();
                        break;
                }
                selectedOP_CD = "OSP";
                selectedDivision = "MEP";

                if (clrStatus == "F")
                {
                    if (MessageBox.Show("Do you want to Register ? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        clrStatus = "";
                }
                pnlPopUp.Visible = true;

                //if (cboArea.EditValue.ToString() == "OSP")
                //{
                    btnReqSpart.Visible = true;
                    btnReqSpart.Enabled = true;
                //}
                //else
                //{
                //    btnReqSpart.Visible = false;
                //    btnReqSpart.Enabled = false;
                //}

                pnlPopUp.BringToFront();
                timerRefresh.Stop();
                dtGet = null;

                //if (_bFormLoaded)
                //{
                pbProgressShow();
                //}

                try
                {
                    fn_Search("GET_DATA", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), selectedOP_CD, "", selectedDivision, gvw.Columns[hi.Column.AbsoluteIndex].FieldName, mc_no, DateTime.Now.ToString("yyyyMMdd"));
                    //GMES0211_INPUT frmInput = new GMES0211_INPUT(cboPlant.EditValue.ToString(), cbItemClassType.EditValue.ToString(), cellInfo.CellValue.ToString(), gvw.Columns[hi.Column.AbsoluteIndex].FieldName, gvw.GetRowCellValue(hi.RowHandle, "MACHINE_SEQ").ToString(), dtLabour, dtGet, dtDefect, dtSolution, dtMachine);
                    //frmInput.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBoxW("gvwData_MouseDown() " + ex.ToString());
                }
                finally
                {
                    pbSetProgressHide();
                }

                pnlRequest.Enabled = false;
                pnlCheck.Enabled = false;
                pnlRepair.Enabled = false;
                pnlDone.Enabled = false;

                fn_clearAll();
                lblMCLine.Text = gvw.Columns[hi.Column.AbsoluteIndex].FieldName;
                lblMCNo.Text = mc_no;//gvw.GetRowCellValue(hi.RowHandle, "MACHINE_SEQ").ToString();
                lblOPCD.Text = selectedOP_CD;
                fn_setCombo(cboLocation.EditValue.ToString());

                // jika new data
                if (clrStatus != "R" && clrStatus != "C" && clrStatus != "F")
                {
                    //lblWoNo.Text = (wo.Any() ? wo.CopyToDataTable().Rows[0]["WO_NO"].ToString() : "-");
                    //lblWoSeq.Text = (wo.Any() ? wo.CopyToDataTable().Rows[0]["WO_SEQ"].ToString() : "-");
                    //lblWoNoOld.Text = "";
                    lblWoNo.Text = "";
                    lblWoSeq.Text = "";
                    dateRequest.Text = "";//DateTime.Now.ToString("yyyy-MM-dd HH : mm");
                    lblReqYMD.Text = "";//DateTime.Now.ToString("yyyyMMdd");
                    lblReqHMS.Text = "";//DateTime.Now.ToString("HHmmss");
                    btnCall.Enabled = false;
                }

                if (dtGet != null && dtGet.Rows.Count > 0)
                {
                    DataTable dtSet = null;
                    var data = dtGet.AsEnumerable().Where(row => row.Field<string>("DIV") == "DATA");
                    //var wo = dtGet.AsEnumerable().Where(row => row.Field<string>("DIV") == "WO");

                    if (data.Any())
                        dtSet = data.CopyToDataTable();


                    //clrStatus = (clrStatus.Length == 1 ? clrStatus : clrStatus.Substring(clrStatus.Length - 1, 1));

                    //jika data ada
                    if (clrStatus == "R" || clrStatus == "C" || clrStatus == "F")
                    {
                        btnCall.Enabled = true;
                        //jika R / C / F
                        //lblWoNoOld.Text = dtSet.Rows[0]["WO_NO"].ToString();
                        lblWoNo.Text = dtSet.Rows[0]["WO_NO"].ToString();
                        dateRequest.Text = dtSet.Rows[0]["REQ_DT"].ToString();
                        //cbNikRequest.Text = dtSet.Rows[0]["REQ_EMP_ID"].ToString();
                        txtNikRequest.Text = dtSet.Rows[0]["REQ_EMP_ID"].ToString();
                        txtCondition.Text = dtSet.Rows[0]["REQ_MSG"].ToString();
                        lblDownTimeRequest.Text = dtSet.Rows[0]["DOWNTIME_REQ"].ToString();
                        lblReqYMD.Text = dtSet.Rows[0]["REQ_YMD"].ToString();
                        lblReqHMS.Text = dtSet.Rows[0]["REQ_HMS"].ToString();
                        lblWoSeq.Text = dtSet.Rows[0]["WO_SEQ"].ToString();
                        txtChkMsg.Text = dtSet.Rows[0]["CHK_MSG"].ToString();

                        for (int i = 0; i < rgMechanic.Properties.Items.Count; i++)
                        {
                            if (rgMechanic.Properties.Items[i].Value.ToString() == dtSet.Rows[0]["DIVISION_TYPE"].ToString())
                            {
                                rgMechanic.SelectedIndex = i;
                            }
                        }

                        cboWoType.EditValue = dtSet.Rows[0]["WO_TYPE"].ToString();

                        //jika R setting C
                        if (clrStatus == "R")
                        {
                            dateCheck.Text = "";//DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            lblChkYMD.Text = "";//DateTime.Now.ToString("yyyyMMdd");
                            lblChkHMS.Text = "";//DateTime.Now.ToString("HHmmss");
                            lookEMC_ID.Text = dtSet.Rows[0]["MACHINE_NAME"].ToString(); //move dari check ke request
                        }

                        //jika C
                        if (clrStatus == "C" || clrStatus == "F")
                        {
                            //setting C
                            cbNikCheck.Text = dtSet.Rows[0]["CHK_EMP_ID"].ToString();
                            dateCheck.Text = dtSet.Rows[0]["CHK_DT"].ToString();
                            cbDefective.Text = dtSet.Rows[0]["DEFEC_NM"].ToString();
                            //cbMCID.Text = dtSet.Rows[0]["MACHINE_ID"].ToString();
                            lookEMC_ID.Text = dtSet.Rows[0]["MACHINE_NAME"].ToString();
                            lblDownTimeCheck.Text = dtSet.Rows[0]["DOWNTIME_CHK"].ToString();
                            lblChkYMD.Text = dtSet.Rows[0]["CHK_YMD"].ToString();
                            lblChkHMS.Text = dtSet.Rows[0]["CHK_HMS"].ToString();

                            //if (clrStatus == "C")
                            //{
                            //setting F
                            dateRepair.Text = "";//DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            lblRepairYMD.Text = "";//DateTime.Now.ToString("yyyyMMdd");
                            lblRepairHMS.Text = "";//DateTime.Now.ToString("HHmmss");
                            //}
                            //else
                            //{
                            if (dtSet.Rows[0]["REPAIR_YMD"].ToString() != "")
                            {
                                //cbNikRepair.Text = dtSet.Rows[0]["REPAIR_EMP_ID"].ToString();
                                dateRepair.Text = dtSet.Rows[0]["REPAIR_DT"].ToString();
                                cbSolution.Text = dtSet.Rows[0]["SOLU_NM"].ToString();
                                lblDownTimeRepair.Text = dtSet.Rows[0]["DOWNTIME_REPAIR"].ToString();
                                lblRepairYMD.Text = dtSet.Rows[0]["REPAIR_YMD"].ToString();
                                lblRepairHMS.Text = dtSet.Rows[0]["REPAIR_HMS"].ToString();

                                txtSoluMsg.Text = dtSet.Rows[0]["SOLU_MSG"].ToString(); //2024.12.31

                                string[] rowMat = dtSet.Rows[0]["MAT_PART"].ToString().Split(new Char[] { ';' });
                                string[] rowMEP_NIK = dtSet.Rows[0]["REPAIR_EMP_ID"].ToString().Split(new Char[] { ';' });
                                string[] rowMEP_Name = dtSet.Rows[0]["REPAIR_EMP_NM"].ToString().Split(new Char[] { ';' });

                                dtMatPart = grdSparePart.DataSource as DataTable;

                                if (rowMat[0] != "")
                                {
                                    foreach (string row in rowMat)
                                    {
                                        string[] colDt = row.Split(new Char[] { '~' });
                                        object[] obj = new object[3];
                                        int i = 0;

                                        foreach (string dt in colDt)
                                        {
                                            obj[i] = dt;
                                            i++;
                                        }

                                        dtMatPart.Rows.Add(obj);
                                    }

                                    SetData(grdSparePart, dtMatPart);
                                }

                                dtMEP_Repair = grdMEP.DataSource as DataTable;

                                if (rowMEP_NIK[0] != "" && rowMEP_Name[0] != "")
                                {
                                    //foreach (string nik in rowMEP_NIK)
                                    //{
                                    //    obj
                                    //}
                                    for (int i = 0; i < rowMEP_NIK.Length; i++)
                                    {
                                        dtMEP_Repair.Rows.Add(new object[] { rowMEP_NIK[i], rowMEP_Name[i] });
                                    }


                                    SetData(grdMEP, dtMEP_Repair);
                                }
                            }

                            if (clrStatus == "F")
                            {
                                //cbNikConfirm.Text = dtSet.Rows[0]["CFM_EMPID"].ToString();
                                txtNikConfirm.Text = dtSet.Rows[0]["CFM_EMPID"].ToString();
                                lblEmpNameConfirm.Text = dtSet.Rows[0]["CFM_EMP_NM"].ToString();
                                lblCfmDate.Text = dtSet.Rows[0]["CFM_DT"].ToString();
                            }
                            //}
                        }
                    }
                }

                switch (clrStatus)
                {
                    case "R":
                        pnlRequest.Enabled = true;
                        pnlCheck.Enabled = true;
                        if (lblDownTimeRequest.Text.ToString() != "")
                            btnCall.Enabled = true;

                        break;

                    case "C":
                        pnlCheck.Enabled = true;
                        pnlRepair.Enabled = true;
                        if (lblDownTimeRepair.Text.ToString() != "-")
                        {
                            pnlCheck.Enabled = false;
                            pnlDone.Enabled = true;
                            btnCall_TL.Enabled = true;
                        }
                        break;

                    case "":
                        pnlRequest.Enabled = true;
                        break;

                    //case "F":
                    //    pnlRepair.Enabled = true;
                    //    break;
                }
                //timerRefresh.Start();
                #endregion
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlPopUp.Visible = false;

            #region [COMMENT]
            //if (chkAndon.Checked)
            //    timerCheckAndon.Start();
            #endregion
            timerRefresh.Start();
        }

        private void fn_clearAll(string p_condition = "ALL")
        {
            if (p_condition == "ALL")
            {
                #region Request
                //lblWoNoOld.Text = "";
                lblWoNo.Text = "";
                dateRequest.Text = "";
                txtNikRequest.Text = "";
                //cbNikRequest.Text = "";
                //cbNikRequest.Properties.Items.Clear();
                //cbNikCheck.Text = "";
                //cbNikCheck.Properties.Items.Clear();
                //cbNikRepair.Text = "";
                //cbNikRepair.Properties.Items.Clear();
                lblEmpNameRequest.Text = "";
                lblEmpNameCheck.Text = "";
                lblEmpNameRepair.Text = "";
                //lblDefectiveCD.Text = "";
                lblMCDesc.Text = "";
                lblSolutionCD.Text = "";
                lblMCLine.Text = "";
                lblMCNo.Text = "";
                lblOPCD.Text = "";
                txtCondition.Clear();
                lblDefectiveCD.Text = "";
                lblSolutionCD.Text = "";
                lblDownTimeRequest.Text = "-";
                lblReqYMD.Text = "";
                lblReqHMS.Text = "";
                btnCall.Enabled = false;
                rgMechanic.SelectedIndex = 0;
                #endregion

                #region Check
                dateCheck.Text = "";
                cbNikCheck.Text = "";
                cbNikCheck.Properties.Items.Clear();
                lblEmpNameCheck.Text = "";
                //cbMCID.Text = "";
                lookEMC_ID.EditValue = "";
                lookEMC_ID.Properties.DataSource = null;
                //cbMCID.Properties.Items.Clear();
                cbDefective.Text = "";
                cbDefective.Properties.Items.Clear();
                lblDownTimeCheck.Text = "-";
                lblChkYMD.Text = "";
                lblChkHMS.Text = "";
                txtChkMsg.Text = "";
                #endregion

                #region Repair
                dateRepair.Text = "";
                //cbNikRepair.Text = "";
                //cbNikRepair.Properties.Items.Clear();
                lblEmpNameRepair.Text = "";
                cbSolution.Text = "";
                cbSolution.Properties.Items.Clear();
                dtMatPart.Rows.Clear();
                dtMEP_Repair.Rows.Clear();
                riComboBox.Items.Clear();
                lblDownTimeRepair.Text = "-";
                lblRepairYMD.Text = "";
                lblRepairHMS.Text = "";
                btnCall_TL.Enabled = false;

                txtSoluMsg.Text = "";
                #endregion

                #region Confirm
                lblEmpNameConfirm.Text = "";
                //cbNikConfirm.Text = "";
                //cbNikConfirm.Properties.Items.Clear();
                teCurrentPass.EditValue = null;
                teNewPass.Visible = false;
                teNewPass.EditValue = null;
                teCfmNewPass.Visible = false;
                teCfmNewPass.EditValue = null;
                BtnReset.Text = "Change Password";
                txtNikConfirm.Text = "";
                lblCfmDate.Text = "";
                #endregion
            }
            else
                if (p_condition == "SPART")
                {
                    dtReqSpart.Rows.Clear();
                    SetData(grdReqSpart, dtReqSpart);
                    pnlReqSpart.Enabled = false;
                    pnlReqSpart.Visible = false;
                    button1.Enabled = true;
                }
        }

        private void fn_setCombo(string param_location)
        {
            var workshop = dtLabour.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "WORKSHOP" && (row.Field<string>("PLANT_CD") == param_location || row.Field<string>("PLANT_CD") == "EXP")).Select(row => new { CODE = row["CODE"], DESCRIPTION = row["DESCRIPTION"] });
            var mep = dtLabour.AsEnumerable().Where(row => row.Field<string>("REMARKS1") == "MEP" && (param_location == "51IP" ? row.Field<string>("PLANT_CD") == param_location : row.Field<string>("PLANT_CD") == row.Field<string>("PLANT_CD"))).Select(row => new { CODE = row["CODE"], DESCRIPTION = row["DESCRIPTION"] });
            //BEFORE var mc = dtMachine.AsEnumerable().Where(row => row.Field<string>("PLANT_CD") == cboPlant.EditValue.ToString()).Select(row => new { Barcode = row["CODE"], Machine_ID = row["CODE_NAME"], Description = row["REMARKS"] });

            if (workshop.Any())
                dtWorkshop = ConvertToDataTable(workshop);

            #region [COMMENT]
            //foreach (DataRow row in dtWorkshop.Rows)
            //{
            //    cbNikRequest.Properties.Items.Add(row["CODE"]);
            //}
            #endregion

            if (mep.Any())
            {
                dtMEP = ConvertToDataTable(mep);

                foreach (DataRow row in dtMEP.Rows)
                {
                    cbNikCheck.Properties.Items.Add(row["CODE"]);
                    //cbNikRepair.Properties.Items.Add(row["CODE"]);
                    riComboBox.Items.Add(row["CODE"]);
                }
            }

            //BEFORE
            //if (mc.Any())
            //    dtMC_Loc = ConvertToDataTable(mc);

            // MODIFY
            if (param_location == "51BT" && cboArea.EditValue.ToString() == "OSP")
            {
                var mc = dtMachine.AsEnumerable().Where(row => row.Field<string>("PLANT_CD") == cboPlant.EditValue.ToString() && row.Field<string>("REMARKS2") == "OSP" + lblMCLine.Text.PadLeft(2, '0')).Select(row => new { Barcode = row["CODE"], Machine_ID = row["CODE_NAME"], Description = row["REMARKS"] });
                if (mc.Any())
                    dtMC_Loc = ConvertToDataTable(mc);
            }
            else if (param_location == "51BT" && cboArea.EditValue.ToString() == "OSR")
            {
                var mc = dtMachine.AsEnumerable().Where(row => row.Field<string>("PLANT_CD") == cboPlant.EditValue.ToString() && row.Field<string>("REMARKS2") == "OSRBR").Select(row => new { Barcode = row["CODE"], Machine_ID = row["CODE_NAME"], Description = row["REMARKS"] });
                if (mc.Any())
                    dtMC_Loc = ConvertToDataTable(mc);
            }
            else if (param_location == "51BT" && cboArea.EditValue.ToString() == "PUR,BEA,BEM") //2024.10.11
            {
                var mc = dtMachine.AsEnumerable().Where(row => row.Field<string>("PLANT_CD") == cboPlant.EditValue.ToString() && row.Field<string>("REMARKS2") == "PUR").Select(row => new { Barcode = row["CODE"], Machine_ID = row["CODE_NAME"], Description = row["REMARKS"] });
                if (mc.Any())
                    dtMC_Loc = ConvertToDataTable(mc);
            }
            else
            {
                var mc = dtMachine.AsEnumerable().Where(row => row.Field<string>("PLANT_CD") == cboPlant.EditValue.ToString()).Select(row => new { Barcode = row["CODE"], Machine_ID = row["CODE_NAME"], Description = row["REMARKS"] });
                if (mc.Any())
                    dtMC_Loc = ConvertToDataTable(mc);

            }


            foreach (DataRow row in dtDefect.Rows)
            {
                cbDefective.Properties.Items.Add(row["CODE_NAME"]);
            }

            foreach (DataRow row in dtSolution.Rows)
            {
                cbSolution.Properties.Items.Add(row["CODE_NAME"]);
            }

            #region [COMMENT]
            //foreach (DataRow row in dtMC_Loc.Rows)
            //{
            //    cbMCID.Properties.Items.Add(row["CODE"]);
            //}
            #endregion

            lookEMC_ID.Properties.DataSource = dtMC_Loc;
            riSpart.DataSource = dtSpart;

            cboWoType.Properties.DataSource = dtWoType;
            cboWoType.Properties.ValueMember = "CODE";
            cboWoType.Properties.DisplayMember = "DESCRIPTION";
            if (dtWoType.Rows.Count > 0)
            {
                cboWoType.EditValue = dtWoType.Rows[0]["DESCRIPTION"];
            }

            #region [COMMENT]
            //foreach (DataRow row in dtWorkshop.Rows)
            //{
            //    cbNikConfirm.Properties.Items.Add(row["CODE"]);
            //}
            #endregion
        }

        private void cbNikRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region [COMMENT]
            //string nik = cbNikRequest.SelectedItem.ToString();
            //var name = dtWorkshop.AsEnumerable().Where(row => row.Field<string>("CODE") == nik).Select(row => new { DESCRIPTION = row["DESCRIPTION"] });

            //if (name.Any())
            //    lblEmpNameRequest.Text = ConvertToDataTable(name).Rows[0][0].ToString();
            //else
            //    lblEmpNameRequest.Text = "Not Found";
            #endregion
        }

        private void cbNikCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nik = cbNikCheck.SelectedItem.ToString();
            var name = dtMEP.AsEnumerable().Where(row => row.Field<string>("CODE") == nik).Select(row => new { DESCRIPTION = row["DESCRIPTION"] });

            if (name.Any())
                lblEmpNameCheck.Text = ConvertToDataTable(name).Rows[0][0].ToString();
            else
                lblEmpNameCheck.Text = "Not Found";
        }

        private void cbNikRepair_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region [COMMENT]
            //string nik = cbNikRepair.SelectedItem.ToString();
            //var name = dtMEP.AsEnumerable().Where(row => row.Field<string>("CODE") == nik).Select(row => new { DESCRIPTION = row["DESCRIPTION"] });

            //if (name.Any())
            //    lblEmpNameRepair.Text = ConvertToDataTable(name).Rows[0][0].ToString();
            //else
            //    lblEmpNameRepair.Text = "Not Found";
            #endregion
        }

        private void cbSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            string solution = cbSolution.SelectedItem.ToString();
            var name = dtSolution.AsEnumerable().Where(row => row.Field<string>("CODE_NAME") == solution).Select(row => new { CODE = row["CODE"] });

            if (name.Any())
                lblSolutionCD.Text = ConvertToDataTable(name).Rows[0][0].ToString();
            else
                lblSolutionCD.Text = "Not Found";
        }

        private void cbDefective_SelectedIndexChanged(object sender, EventArgs e)
        {
            string request = cbDefective.SelectedItem.ToString();
            var name = dtDefect.AsEnumerable().Where(row => row.Field<string>("CODE_NAME") == request).Select(row => new { CODE = row["CODE"] });

            if (name.Any())
                lblDefectiveCD.Text = ConvertToDataTable(name).Rows[0][0].ToString();
            else
                lblDefectiveCD.Text = "Not Found";
        }

        private void cbMCID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbNikConfirm_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region [COMMENT]
            //string nik = cbNikConfirm.SelectedItem.ToString();
            //var name = dtWorkshop.AsEnumerable().Where(row => row.Field<string>("CODE") == nik).Select(row => new { DESCRIPTION = row["DESCRIPTION"] });

            //if (name.Any())
            //    lblEmpNameConfirm.Text = ConvertToDataTable(name).Rows[0][0].ToString();
            //else
            //    lblEmpNameConfirm.Text = "Not Found";
            #endregion
        }

        private void btnRequestSave_Click(object sender, EventArgs e)
        {
            //if (cbNikRequest.SelectedIndex == -1 || txtCondition.Text == "" || txtCondition.Text == " ")
            if (lblEmpNameRequest.Text == "Not Found" || lblEmpNameRequest.Text == "" || lblEmpNameRequest.Text == " " || txtCondition.Text == "" || txtCondition.Text == " " || lookEMC_ID.EditValue.ToString() == "")
            {
                MessageBoxW("Please fill out NIK, Condition field !");
                return;
            }

            #region [COMMENT]
            //switch (lblMCNo.Text.ToString())
            //{
            //    case "98":
            //        selectedOP_CD = "OSC";
            //        break;
            //    case "99":
            //        selectedOP_CD = "OST";
            //        break;
            //    case "100":
            //        selectedOP_CD = "OSH";
            //        break;
            //    default:
            //        selectedOP_CD = "OSP";
            //        break;
            //}
            #endregion

            pbProgressShow();

            try
            {
                fn_Save("SAVE", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), lblOPCD.Text, "", selectedDivision, lblReqYMD.Text, lblReqHMS.Text.ToString(), txtNikRequest.Text, lblEmpNameRequest.Text, txtCondition.Text, lblReqYMD.Text, lblWoSeq.Text, /*lblWoNoOld.Text*/ lblWoNo.Text, lblMCLine.Text, lblMCNo.Text, "R", rgMechanic.Properties.Items[rgMechanic.SelectedIndex].Value.ToString(), "", "", "", "", "", lookEMC_ID.EditValue.ToString(), "", "", "", "", cboWoType.EditValue.ToString()); 
                pnlPopUp.Visible = false;
                #region [COMMENT]
                //    if (cboArea.EditValue.ToString() == "OSP" && cboLocation.EditValue.ToString() == "51BT")
            //        fn_Search("GET_LAYOUT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), "OSP");
            //    else
            //        fn_Search("GET_LAYOUT_PLANT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString());
            ////if (chkAndon.Checked)
                ////    playSound(lblMCLine.Text, lblMCNo.Text);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBoxW("btnRequestSave_Click() " + ex.ToString());
            }
            finally
            {
                pbSetProgressHide();
                QueryClick();
            }
        }

        private void btnCheckSave_Click(object sender, EventArgs e)
        {
            if (cbNikCheck.SelectedIndex == -1 || cbDefective.SelectedIndex == -1 /*|| cbMCID.SelectedIndex == -1*/ /*|| lookEMC_ID.EditValue.ToString() == ""*/)
            {
                MessageBoxW("Please fill out NIK, Defective, Machine ID field !");
                return;
            }

            #region [COMMENT]
            //selectedOP_CD = "";
            //switch (lblMCNo.Text.ToString())
            //{
            //    case "98":
            //        selectedOP_CD = "OSC";
            //        break;
            //    case "99":
            //        selectedOP_CD = "OST";
            //        break;
            //    case "100":
            //        selectedOP_CD = "OSH";
            //        break;
            //    default:
            //        selectedOP_CD = "OSP";
            //        break;
            //}
            #endregion

            pbProgressShow();
            try
            {
                string p_lost_time = chkLostTime.CheckState == CheckState.Checked ? "Y" : "N";
                //fn_Save("SAVE", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), lblOPCD.Text, cbItemClassType.EditValue.ToString(), selectedDivision, lblReqYMD.Text, lblReqHMS.Text, txtNikRequest.Text, lblEmpNameRequest.Text, txtCondition.Text, lblReqYMD.Text, lblWoSeq.Text, lblWoNo.Text, lblMCLine.Text, lblMCNo.Text, "C", lblChkYMD.Text, lblChkHMS.Text, cbNikCheck.Text, lblEmpNameCheck.Text, txtChkMsg.Text, cbMCID.Text, "", lblDefectiveCD.Text);
                fn_Save("SAVE", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), lblOPCD.Text, "", selectedDivision, lblReqYMD.Text, lblReqHMS.Text, txtNikRequest.Text, lblEmpNameRequest.Text, txtCondition.Text, lblReqYMD.Text, lblWoSeq.Text, /*lblWoNoOld.Text*/ lblWoNo.Text, lblMCLine.Text, lblMCNo.Text, "C", rgMechanic.Properties.Items[rgMechanic.SelectedIndex].Value.ToString(), lblChkYMD.Text, lblChkHMS.Text, cbNikCheck.Text, lblEmpNameCheck.Text, txtChkMsg.Text, /*cbMCID.Text*/ lookEMC_ID.EditValue.ToString(), "", lblDefectiveCD.Text, p_lost_time);
                pnlPopUp.Visible = false;

                #region [COMMENT]
                //if (cboArea.EditValue.ToString() == "OSP" && cboLocation.EditValue.ToString() == "51BT")
                //    fn_Search("GET_LAYOUT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), "OSP");
                //else
                //    fn_Search("GET_LAYOUT_PLANT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString());
                #endregion 
            }
            catch (Exception ex)
            {
                MessageBoxW("btnCheckSave_Click() " + ex.ToString());
            }
            finally {
                pbSetProgressHide();
                QueryClick();
            }
        }

        private void btnRepairSave_Click(object sender, EventArgs e)
        {
            if (gvwMechanic.RowCount < 1 || cbSolution.SelectedIndex == -1)
            {
                MessageBoxW("Please fill out NIK, Solution field !");
                return;
            }

            string part = "";

            foreach (DataRow row in dtMatPart.Rows)
            {
                if (row["PART_NAME"].ToString() == "" || row["PART_QTY"].ToString() == "" || row["PART_UNIT"].ToString() == "")
                {
                    MessageBoxW("Please fill out Replacement Material Data !");
                    return;
                }

                part += row["PART_NAME"].ToString() + "/" + row["PART_QTY"].ToString() + "/" + row["PART_UNIT"].ToString() + ";";
            }

            if (part.Length > 0)
                part = part.Remove(part.Length - 1, 1);


            string emp_id = "";
            string emp_name = "";
            foreach (DataRow row in dtMEP_Repair.Rows)
            {
                if (row["CODE"].ToString() == "" || row["DESCRIPTION"].ToString() == "Not Found")
                {
                    MessageBoxW("Please fill out NIK field !");
                    return;
                }

                emp_id += row["CODE"].ToString() + ";";
                emp_name += row["DESCRIPTION"].ToString() + ";";
            }

            if (emp_id.Length > 0)
                emp_id = emp_id.Remove(emp_id.Length - 1, 1);

            if (emp_name.Length > 0)
                emp_name = emp_name.Remove(emp_name.Length - 1, 1);

            #region [COMMENT]
            //selectedOP_CD = "";
            //switch (lblMCNo.Text.ToString())
            //{
            //    case "98":
            //        selectedOP_CD = "OSC";
            //        break;
            //    case "99":
            //        selectedOP_CD = "OST";
            //        break;
            //    case "100":
            //        selectedOP_CD = "OSH";
            //        break;
            //    default:
            //        selectedOP_CD = "OSP";
            //        break;
            //}
            #endregion 

            pbProgressShow();

            try
            {
                string p_lost_time = chkLostTime.CheckState == CheckState.Checked ? "Y" : "N";

                fn_Save("SAVE", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), lblOPCD.Text, "", selectedDivision, lblReqYMD.Text, lblReqHMS.Text, txtNikRequest.Text, lblEmpNameRequest.Text, txtCondition.Text, lblReqYMD.Text, lblWoSeq.Text, /*lblWoNoOld.Text*/ lblWoNo.Text, lblMCLine.Text, lblMCNo.Text, "C", rgMechanic.Properties.Items[rgMechanic.SelectedIndex].Value.ToString(), lblChkYMD.Text, lblChkHMS.Text, cbNikCheck.Text, lblEmpNameCheck.Text, txtChkMsg.Text, /*cbMCID.Text*/ lookEMC_ID.EditValue.ToString(), "", lblDefectiveCD.Text, p_lost_time, txtSoluMsg.Text, "", "", "", "", lblRepairYMD.Text, lblRepairHMS.Text, emp_id, emp_name, lblSolutionCD.Text, part);
                pnlPopUp.Visible = false;

                #region [COMMENT]
                //if (cboArea.EditValue.ToString() == "OSP" && cboLocation.EditValue.ToString() == "51BT")
                //    fn_Search("GET_LAYOUT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), "OSP");
                //else
                //    fn_Search("GET_LAYOUT_PLANT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString());
                #endregion
            }
            catch (Exception ex)
            {
                MessageBoxW("btnRepairSave_Click() " + ex.ToString());
            }
            finally
            {
                pbSetProgressHide();
                QueryClick();
            }
        }

        private void btnRepairAdd_Click(object sender, EventArgs e)
        {
            #region [COMMENT]
            //dtMatPart = grdSparePart.DataSource as DataTable;
            //dtMatPart.Rows.Add(new object[] { "", 1, "EA" });
            //SetData(grdSparePart, dtMatPart);
            #endregion

            bool chk = false;
            if (gvwSparePart.RowCount >= 1)
            {
                for (int i = 0; i < gvwSparePart.RowCount; i++)
                {
                    if (
                        gvwSparePart.GetRowCellValue(i, "PART_NAME") != null && gvwSparePart.GetRowCellValue(i, "PART_NAME").ToString() != ""
                        && gvwSparePart.GetRowCellValue(i, "PART_QTY") != null && gvwSparePart.GetRowCellValue(i, "PART_QTY").ToString() != ""
                        && gvwSparePart.GetRowCellValue(i, "PART_UNIT") != null && gvwSparePart.GetRowCellValue(i, "PART_UNIT").ToString() != ""
                        )
                        chk = true;
                    else
                        chk = false;
                }

                if (chk)
                {
                    gvwSparePart.AddNewRow();
                    gvwSparePart.SetRowCellValue(gvwSparePart.RowCount - 1, gvwSparePart.Columns["PART_NAME"], "");
                    gvwSparePart.SetRowCellValue(gvwSparePart.RowCount - 1, gvwSparePart.Columns["PART_QTY"], 1);
                    gvwSparePart.SetRowCellValue(gvwSparePart.RowCount - 1, gvwSparePart.Columns["PART_UNIT"], "EA");
                }
            }
            else
            {
                gvwSparePart.AddNewRow();
                gvwSparePart.SetRowCellValue(gvwSparePart.RowCount - 1, gvwSparePart.Columns["PART_NAME"], "");
                gvwSparePart.SetRowCellValue(gvwSparePart.RowCount - 1, gvwSparePart.Columns["PART_QTY"], 1);
                gvwSparePart.SetRowCellValue(gvwSparePart.RowCount - 1, gvwSparePart.Columns["PART_UNIT"], "EA");
            }
        }

        private void btnRepairDelete_Click(object sender, EventArgs e)
        {
            if (dtMatPart.Rows.Count > 0)
            {
                dtMatPart.Rows.RemoveAt(gvwSparePart.FocusedRowHandle);
                SetData(grdSparePart, dtMatPart);
            }
        }

        private void timerOnLoadClick_Tick(object sender, EventArgs e)
        {
            #region [COMMENT]
            //pbProgressShow();

            //try
            //{
            //    if (cboArea.EditValue.ToString() == "OSP" && cboLocation.EditValue.ToString() == "51BT")
            //        fn_Search("GET_LAYOUT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), "OSP");

            //    timerOnLoadClick.Stop();
            
            //}
            //catch (Exception ex)
            //{
            //    MessageBoxW("QueryClick() " + ex.ToString());
            //}
            //finally
            //{
            //    pbSetProgressHide();
            //}
            #endregion
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            #region COMMENT
            //pbProgressShow();

            //try
            //{
            //    if (cboArea.EditValue.ToString() == "OSP" && cboLocation.EditValue.ToString() == "51BT")
            //        fn_Search("GET_LAYOUT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), "OSP");
            //    else
            //    {
            //        switch (cboArea.EditValue.ToString())
            //        {
            //            case "FSS & FGA":
            //                setContentLayout(this.layoutPlantC_FSS_FGA, null);
            //                break;
            //            case "UPS":
            //                setContentLayout(this.layoutPlantC_UPS, null);
            //                break;
            //        }

            //        fn_Search("GET_LAYOUT_PLANT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBoxW("timerRefresh_Tick() " + ex.ToString());
            //}
            //finally
            //{
            //    pbSetProgressHide();
            //}
            #endregion

            QueryClick();
        }

        private void spinEdit1_EditValueChanged(object sender, EventArgs e)
        {
            timerRefresh.Stop();
            timerRefresh.Interval = 1000 * 60 * Convert.ToInt32(spinRefresh.EditValue.ToString());
            timerRefresh.Start();
        }

        private void chkFit_CheckedChanged(object sender, EventArgs e)
        {
            fn_FormatGrid("WOF");
            if (chkFit.Checked)
            {
                customToolTip.Visible = false;
            }
            else
            {
                customToolTip.Visible = true;
            }
        }

        private void fn_setHeaderColor()
        {
            for (int i = 0; i < gvwData.Columns.Count; i++)
            {
                gvwData.Columns[i].AppearanceHeader.BackColor = gvwData.Columns[0].AppearanceHeader.BackColor;
                gvwData.Columns[i].AppearanceHeader.ForeColor = gvwData.Columns[0].AppearanceHeader.ForeColor;
            }

            int idx = 0;
            foreach (DataRow row in dtCIS.Rows)
            {
                for (int i = 0; i < gvwData.Columns.Count; i++)
                {
                    if (row["2"].ToString() == gvwData.Columns[i].FieldName)
                    {
                        int[] objBackColor = new int[3];
                        int[] objForeColor = new int[3];
                        string[] strBackColor = dtCIS.Rows[idx]["3"].ToString().Split(new Char[] { ',' });
                        string[] strForeColor = dtCIS.Rows[idx]["4"].ToString().Split(new Char[] { ',' });

                        int z = 0;
                        foreach (string dt in strBackColor)
                        {
                            objBackColor[z] = Convert.ToInt32(dt);
                            z++;
                        }
                        gvwData.Columns[i].AppearanceHeader.BackColor = Color.FromArgb(objBackColor[0], objBackColor[1], objBackColor[2]);

                        z = 0;
                        foreach (string dt in strForeColor)
                        {
                            objForeColor[z] = Convert.ToInt32(dt);
                            z++;
                        }
                        gvwData.Columns[i].AppearanceHeader.ForeColor = Color.FromArgb(objForeColor[0], objForeColor[1], objForeColor[2]);
                    }
                }
                idx++;
            }
        }

        private void gvwSummary_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView gvw = sender as GridView;
            string val = gvw.GetRowCellValue(e.RowHandle, e.Column.FieldName).ToString();

            if (e.Column.ColumnHandle > 1)
            {
                if (e.Column.FieldName == "OFF" && val != "0")
                {
                    e.Appearance.BackColor = Color.Gray;
                    e.Appearance.ForeColor = Color.White;
                }

                if (e.Column.FieldName == "BREAKDOWN" && val != "0")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                }

                if (e.Column.FieldName == "UNDER REPAIR" && val != "0")
                {
                    e.Appearance.BackColor = Color.Yellow;
                }

                if (e.Column.FieldName == "DONE" && val != "0")
                {
                    e.Appearance.BackColor = Color.LightGreen;
                }

            }

        }

        private void gvwSummary_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Value.ToString() == "0")
                e.DisplayText = "";
        }

        private void chkAndon_CheckedChanged(object sender, EventArgs e)
        {
            //timerCheckAndon.Enabled = chkAndon.Checked;
        }

        private void playSound(int param_line, int param_mc_no)
        {
            SoundPlayer player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("open"));
            player.PlaySync();

            player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("panggilan"));
            player.PlaySync();

            player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("maintenance"));
            player.PlaySync();

            if (param_mc_no != 100)
            {
                player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("line"));
                player.PlaySync();

                player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("_" + param_line));
                player.PlaySync();
            }

            player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("mesin"));
            player.PlaySync();

            //if (param_mc_no != 98 && param_mc_no != 99 && param_mc_no != 100)
            if (param_mc_no != 98 && param_mc_no != 99 && param_mc_no != 100 && param_mc_no != 101 && param_mc_no != 102 && param_mc_no != 103) //2023-05-11
            {
                player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("_" + param_mc_no));
                player.PlaySync();
            }

            //if (param_mc_no == 98 || param_mc_no == 99 || param_mc_no == 100)
            if (param_mc_no == 98 || param_mc_no == 99 || param_mc_no == 100 || param_mc_no == 101 || param_mc_no == 102 || param_mc_no == 103) //2023-05-11
            {
                if (param_mc_no == 98)
                {
                    player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("cutting"));
                    player.PlaySync();
                }
                else if (param_mc_no == 99)
                {
                    player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("trimming"));
                    player.PlaySync();
                }
                else if (param_mc_no == 100) //2023-05-11
                {
                    player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("trimming"));
                    player.PlaySync();
                }
                else if (param_mc_no == 101)
                {
                    player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("hii_ter"));
                    player.PlaySync();

                    player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("_" + param_line));
                    player.PlaySync();
                }
                else if (param_mc_no == 102) //2023-05-11
                {
                    player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("cleaning"));
                    player.PlaySync();
                }
                else if (param_mc_no == 103) //2023-05-11
                {
                    player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("thomson"));
                    player.PlaySync();
                }
            }

            player = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject("close"));
            player.PlaySync();
        }

        void WriteResourceToFile(string resourceName, string fileName)
        {
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        private void CopyDependency()
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\WOF_Andon.xml") == false)
            {
                #region [COMMENT]
                //installFont = false;
                //try
                //{
                //    var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().Single(str => str.EndsWith("WOF_Andon.xml"));
                //    WriteResourceToFile(resourceName, Application.StartupPath + "\\WOF_Andon.xml");
                //}
                //catch
                //{
                //    MessageBox.Show("Error accessing resources!");
                //}
                #endregion
            }
            else
                installFont = true;

            if (System.IO.File.Exists(Application.StartupPath + "\\digital-7.ttf") == false)
            {
                try
                {
                    var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().Single(str => str.EndsWith("digital-7.ttf"));
                    WriteResourceToFile(resourceName, Application.StartupPath + "\\digital-7.ttf");
                }
                catch
                {
                    MessageBox.Show("Error accessing resources!");
                }
            }
        }

        private void readXML()
        {
            using (FileStream fs = new FileStream("WOF_Andon.xml", FileMode.Open, FileAccess.Read))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fs);

                foreach (XmlNode xmlnode in xmldoc.GetElementsByTagName("sound_yn"))
                {
                    sound_yn = xmlnode.InnerText.ToString();
                }

                foreach (XmlNode xmlnode in xmldoc.GetElementsByTagName("times"))
                {
                    int.TryParse(xmlnode.InnerText, out times);
                }

                foreach (XmlNode xmlnode in xmldoc.GetElementsByTagName("repeat"))
                {

                    int.TryParse(xmlnode.InnerText, out repeat);
                }

                #region [COMMENT]
                //foreach (XmlNode xmlnode in xmldoc.GetElementsByTagName("delay"))
                //{
                //    int.TryParse(xmlnode.InnerText, out delay);
                //}
                #endregion

                foreach (XmlNode xmlnode in xmldoc.GetElementsByTagName("line"))
                {
                    foreach (XmlElement xmllist in xmlnode.ChildNodes)
                    {
                        if (xmllist.Name == "from")
                            lineFrom = Convert.ToInt16(xmllist.InnerText);

                        if (xmllist.Name == "to")
                            lineTo = Convert.ToInt16(xmllist.InnerText);
                    }
                }

                foreach (XmlNode xmlnode in xmldoc.GetElementsByTagName("machine"))
                {
                    foreach (XmlElement xmllist in xmlnode.ChildNodes)
                    {
                        if (xmllist.Name == "from")
                            mcFrom = Convert.ToInt16(xmllist.InnerText);

                        if (xmllist.Name == "to")
                            mcTo = Convert.ToInt16(xmllist.InnerText);
                    }
                }
            }
        }

        private void timerCheckAndon_Tick(object sender, EventArgs e)
        {
            #region [COMMENT]
            //if (chkAndon.Checked)
            //{
            //    fn_GetDataAndon();
            //}
            #endregion
        }

        private void bwAndon_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                callSound(dtAndonCurr);
            }
            catch (Exception ex)
            {
                MessageBoxW("Showing Andon Error : " + ex.ToString());
            }
        }

        #region [COMMENT]
        //private void bwAndon_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    try
        //    {
        //        pnlAndon.Visible = false;
        //        timerBlink.Stop();
        //        zz = 0;
        //        lblAndonLineVal.Text = "";
        //        lblAndonMCVal.Text = "";
        //        //fn_Save("UPDATE_SOUND", cboPlant.EditValue.ToString(), cbItemClassType.EditValue.ToString(), lblReqYMD.Text, lblReqHMS.Text.ToString(), cbNikRequest.Text, lblEmpNameRequest.Text, txtCondition.Text, lblReqYMD.Text, lblWoSeq.Text, lblWoNo.Text, lblMCLine.Text, lblMCNo.Text, "R");
        //        if (!chkManualClick)
        //        {
        //            pbProgressShow();
        //            try
        //            {
        //                if (cboArea.EditValue.ToString() == "OSP" && cboLocation.EditValue.ToString() == "51BT")
        //                    fn_Search("GET_LAYOUT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), "OSP");
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBoxW("bwAndon_RunWorkerCompleted() " + ex.ToString());
        //            }
        //            finally
        //            {
        //                pbSetProgressHide();
        //            }
                    
        //        }
        //        chkManualClick = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Worker Completed", ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        timerType.Stop();
        //        dtData.Clear();
        //        dtData.Dispose();
        //        bwAndon.Dispose();
        //    }
        //}

        //int zz = 0;
        //private void timerBlink_Tick(object sender, EventArgs e)
        //{
        //    lblAndonLineVal.Text = andonLine;
        //    lblAndonMCVal.Text = (andonMC == "98" ? "CUT" : (andonMC == "99" ? "TRIM" : (andonMC == "100" ? "HEAT" : andonMC)));

        //    lblAndonLine.Text = (andonMC == "100" ? "NO" : "LINE");

        //    if (zz == 0)
        //        timerType.Start();

        //    if (zz % 2 == 1)
        //    {
        //        lblAndonLineVal.BackColor = Color.Red;
        //        lblAndonLineVal.ForeColor = Color.White;
        //        lblAndonMCVal.BackColor = Color.Yellow;
        //        lblAndonMCVal.ForeColor = Color.Black;
        //        //lblAndonTitle.BackColor = Color.Gold;
        //    }
        //    else
        //    {
        //        lblAndonLineVal.BackColor = Color.Yellow;
        //        lblAndonLineVal.ForeColor = Color.Black;
        //        lblAndonMCVal.BackColor = Color.Red;
        //        lblAndonMCVal.ForeColor = Color.White;
        //        //lblAndonTitle.BackColor = Color.FromArgb(195, 215, 245);
        //    }
        //    zz++;
        //}
        #endregion

        private void GMES0211_FormClosed(object sender, FormClosedEventArgs e)
        {
            forceStop = true;
        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            pnlPopUp.Visible = false;
            //chkManualClick = true;

            if (!bwCall.IsBusy)
            {
                //timerCheckAndon.Stop();
                andonLine = lblMCLine.Text.ToString().PadLeft(2, '0');
                andonMC = lblMCNo.Text.ToString().PadLeft(2, '0');

                //pnlAndon.Visible = !pnlPopUp.Visible;
                //timerBlink.Start();
                bwCall.RunWorkerAsync("MEP");
            }
        }

        private void btnCall_TL_Click(object sender, EventArgs e)
        {
            pnlPopUp.Visible = false;
            //chkManualClick = true;

            if (!bwCall.IsBusy)
            {
                //timerCheckAndon.Stop();
                andonLine = lblMCLine.Text.ToString().PadLeft(2, '0');
                andonMC = lblMCNo.Text.ToString().PadLeft(2, '0');

                //pnlAndon.Visible = !pnlPopUp.Visible;
                //timerBlink.Start();
                bwCall.RunWorkerAsync("TL");
            }
        }

        private void bwCall_DoWork(object sender, DoWorkEventArgs e)
        {
            fn_Save("UPDATE_SOUND", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), selectedOP_CD, "", e.Argument.ToString(), "", "", "", "", "", "", "", /*lblWoNoOld.Text.ToString()*/ lblWoNo.Text.ToString(), "", "", "N");
            //playSound(Convert.ToInt16(lblMCLine.Text), Convert.ToInt16(lblMCNo.Text));
        }

        private void bwCall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            #region [COMMENT]
            //timerBlink.Stop();
            //zz = 0;
            //pnlAndon.Visible = false;
            //timerCheckAndon.Start();
            #endregion
        }

        #region [COMMENT]
        //private void timerType_Tick(object sender, EventArgs e)
        //{
        //    if (lblAndonTitle.Text.Length != andonTitle.Length)
        //    {
        //        for (int x = 0; x < andonTitle.Length; x++)
        //        {
        //            lblAndonTitle.Text = andonTitle.Substring(0, x);
        //        }
        //    }

        //}
        #endregion

        private void tblLayoutPlantC_Paint(object sender, PaintEventArgs e)
        {

        }

        private List<Control> GetAllControls(Control container, List<Control> list)
        {
            foreach (Control c in container.Controls)
            {
                if (c.Controls.Count > 0)
                    list = GetAllControls(c, list);
                else
                    list.Add(c);
            }

            return list;
        }

        private List<Control> GetAllControls(Control container)
        {
            return GetAllControls(container, new List<Control>());
        }

        private void cboLocation_EditValueChanged(object sender, EventArgs e)
        {
            var area = dtArea.AsEnumerable().Where(row => row.Field<string>("REMARKS") == cboLocation.EditValue.ToString()).Select(row => new { CODE = row["CODE"], CODE_NAME = row["CODE_NAME"] });

            cboArea.Properties.DataSource = null;

            if (area.Any())
            {
                if (dtArea != null && dtArea.Rows.Count > 0)
                {
                    cboArea.Properties.DisplayMember = "CODE";
                    cboArea.Properties.ValueMember = "CODE";
                    cboArea.Properties.DataSource = ConvertToDataTable(area);
                    cboArea.SelectedIndex = 0;
                }
            }

            cboArea_EditValueChanged(null, null);
            #region [COMMENT]
            //setShowLayout();

            //switch (cboArea.EditValue.ToString())
            //{
            //    case "FSS & FGA":
            //        setContentLayout(this.layoutPlantC_FSS_FGA, null);
            //        break;
            //    case "UPS":
            //        setContentLayout(this.layoutPlantC_UPS, null);
            //        break;
            //    case "UPC & UPN":
            //        setContentLayout(this.layoutPlantC_UPC_UPN, null);
            //        break;
            //}
            #endregion
        }

        private void MEPClick(object sender)
        {
            if (pnlCallTLQC.Visible || pnlPopUp.Visible)
                return;

            Label lbl = (Label)sender;
            string clrStatus = "";
            int chk = 0;

            clrStatus = lbl.Parent.Text;
            chk = lbl.Name.IndexOf("_") + 1;

            if (dtAssyLine != null && dtAssyLine.Rows.Count > 0 && (int.Parse(lbl.Name.Substring(chk, 1)) - 1) >= dtAssyLine.Rows.Count && !dtAssyLine.Rows[0][2].ToString().Contains("51H1") && !dtAssyLine.Rows[0][2].ToString().Contains("51G1"))
                return;

            selectedOP_CD = lbl.Name.Substring(chk - 4, 3).ToUpper();
            selectedDivision = lbl.Name.Substring(lbl.Name.IndexOf("_", chk) + 1, 3);

            //mc_line = plantC[int.Parse(lbl.Name.Substring(chk, 1)) - 1];
            int cek = (int.Parse(lbl.Name.Substring(chk, 1)) - 1);
            if (cboPlant.EditValue.ToString() == "3110" && (cboArea.EditValue.ToString() != "OSR" && cboArea.EditValue.ToString() != "PUR,BEA,BEM" && cboLocation.EditValue.ToString() != "51SL" && cboLocation.EditValue.ToString() != "51H1" && cboLocation.EditValue.ToString() != "51G1") && (int.Parse(lbl.Name.Substring(chk, 1)) - 1) < dtAssyLine.Rows.Count) //2024.10.07
                mc_line = int.Parse(dtAssyLine.Rows[int.Parse(lbl.Name.Substring(chk, 1)) - 1]["CODE"].ToString()).ToString();
            else
            {
                if (lbl.Name.Substring(chk, 2).ToString().Contains('_'))
                    mc_line = lbl.Name.Substring(chk, 1);
                else
                    mc_line = lbl.Name.Substring(chk, 2);
            }

            //mc_no = lbl.Name.Substring(lbl.Name.Length - 1, 1);
            chk = lbl.Name.IndexOf("zone") + 4;
            mc_no = lbl.Name.Substring(chk, lbl.Name.Length-chk);

            if (clrStatus == "F")
            {
                if (MessageBox.Show("Do you want to Register ? ", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    clrStatus = "";
            }

            pnlPopUp.Visible = true;

            //if (cboArea.EditValue.ToString() == "OSP")
            //{
                btnReqSpart.Visible = true;
                btnReqSpart.Enabled = true;
            //}
            //else
            //{
            //    btnReqSpart.Visible = false;
            //    btnReqSpart.Enabled = false;
            //}

            pnlPopUp.BringToFront();
            timerRefresh.Stop();
            dtGet = null;

            pbProgressShow();

            try
            {
                fn_Search("GET_DATA", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), selectedOP_CD, "", selectedDivision, mc_line, mc_no, DateTime.Now.ToString("yyyyMMdd"));
            }
            catch (Exception ex)
            {
                MessageBoxW("gvwData_MouseDown() " + ex.ToString());
            }
            finally
            {
                pbSetProgressHide();
            }

            pnlRequest.Enabled = false;
            pnlCheck.Enabled = false;
            pnlRepair.Enabled = false;
            pnlDone.Enabled = false;

            fn_clearAll();
            lblMCLine.Text = mc_line;
            lblMCNo.Text = mc_no;//gvw.GetRowCellValue(hi.RowHandle, "MACHINE_SEQ").ToString();
            lblOPCD.Text = selectedOP_CD;
            fn_setCombo(cboLocation.EditValue.ToString());

            // jika new data
            if (clrStatus != "R" && clrStatus != "C" && clrStatus != "F")
            {
                //lblWoNoOld.Text = "";
                lblWoNo.Text = "";
                lblWoSeq.Text = "";
                dateRequest.Text = "";//DateTime.Now.ToString("yyyy-MM-dd HH : mm");
                lblReqYMD.Text = "";//DateTime.Now.ToString("yyyyMMdd");
                lblReqHMS.Text = "";//DateTime.Now.ToString("HHmmss");
                btnCall.Enabled = false;
            }

            if (dtGet != null && dtGet.Rows.Count > 0)
            {
                DataTable dtSet = null;
                var data = dtGet.AsEnumerable().Where(row => row.Field<string>("DIV") == "DATA");

                if (data.Any())
                    dtSet = data.CopyToDataTable();

                //jika data ada
                if (clrStatus == "R" || clrStatus == "C" || clrStatus == "F")
                {
                    btnCall.Enabled = true;
                    //jika R / C / F
                    //lblWoNoOld.Text = dtSet.Rows[0]["WO_NO"].ToString();
                    lblWoNo.Text = dtSet.Rows[0]["WO_NO"].ToString();
                    dateRequest.Text = dtSet.Rows[0]["REQ_DT"].ToString();
                    //cbNikRequest.Text = dtSet.Rows[0]["REQ_EMP_ID"].ToString();
                    txtNikRequest.Text = dtSet.Rows[0]["REQ_EMP_ID"].ToString();
                    txtCondition.Text = dtSet.Rows[0]["REQ_MSG"].ToString();
                    lblDownTimeRequest.Text = dtSet.Rows[0]["DOWNTIME_REQ"].ToString();
                    lblReqYMD.Text = dtSet.Rows[0]["REQ_YMD"].ToString();
                    lblReqHMS.Text = dtSet.Rows[0]["REQ_HMS"].ToString();
                    lblWoSeq.Text = dtSet.Rows[0]["WO_SEQ"].ToString();
                    txtChkMsg.Text = dtSet.Rows[0]["CHK_MSG"].ToString();

                    for (int i = 0; i < rgMechanic.Properties.Items.Count; i++)
                    {
                        if (rgMechanic.Properties.Items[i].Value.ToString() == dtSet.Rows[0]["DIVISION_TYPE"].ToString())
                        {
                            rgMechanic.SelectedIndex = i;
                        }
                    }

                    chkLostTime.CheckState = dtSet.Rows[0]["LOST_TIME_YN"].ToString().ToUpper().Contains("Y") ? CheckState.Checked : CheckState.Unchecked;
                    cboWoType.EditValue = dtSet.Rows[0]["WO_TYPE"].ToString();


                    //jika R setting C
                    if (clrStatus == "R")
                    {
                        dateCheck.Text = "";//DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        lblChkYMD.Text = "";//DateTime.Now.ToString("yyyyMMdd");
                        lblChkHMS.Text = "";//DateTime.Now.ToString("HHmmss");
                        lookEMC_ID.Text = dtSet.Rows[0]["MACHINE_NAME"].ToString(); //move dari check ke request
                    }

                    //jika C
                    if (clrStatus == "C" || clrStatus == "F")
                    {
                        //setting C
                        cbNikCheck.Text = dtSet.Rows[0]["CHK_EMP_ID"].ToString();
                        dateCheck.Text = dtSet.Rows[0]["CHK_DT"].ToString();
                        cbDefective.Text = dtSet.Rows[0]["DEFEC_NM"].ToString();
                        //cbMCID.Text = dtSet.Rows[0]["MACHINE_ID"].ToString();
                        lookEMC_ID.Text = dtSet.Rows[0]["MACHINE_NAME"].ToString();
                        lblDownTimeCheck.Text = dtSet.Rows[0]["DOWNTIME_CHK"].ToString();
                        lblChkYMD.Text = dtSet.Rows[0]["CHK_YMD"].ToString();
                        lblChkHMS.Text = dtSet.Rows[0]["CHK_HMS"].ToString();

                        txtCmmsNo.Text = dtSet.Rows[0]["CMMS_WO_NO"].ToString(); //2024.10.26

                        //if (clrStatus == "C")
                        //{
                        //setting F
                        dateRepair.Text = "";//DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        lblRepairYMD.Text = "";//DateTime.Now.ToString("yyyyMMdd");
                        lblRepairHMS.Text = "";//DateTime.Now.ToString("HHmmss");
                        //}
                        //else
                        //{

                        if (dtSet.Rows[0]["REPAIR_YMD"].ToString() != "")
					    {
                            //cbNikRepair.Text = dtSet.Rows[0]["REPAIR_EMP_ID"].ToString()

                            dateRepair.Text = dtSet.Rows[0]["REPAIR_DT"].ToString();
                            cbSolution.Text = dtSet.Rows[0]["SOLU_NM"].ToString();
                            lblDownTimeRepair.Text = dtSet.Rows[0]["DOWNTIME_REPAIR"].ToString();
                            lblRepairYMD.Text = dtSet.Rows[0]["REPAIR_YMD"].ToString();
                            lblRepairHMS.Text = dtSet.Rows[0]["REPAIR_HMS"].ToString();

                            txtSoluMsg.Text = dtSet.Rows[0]["SOLU_MSG"].ToString();

                            string[] rowMat = dtSet.Rows[0]["MAT_PART"].ToString().Split(new Char[] { ';' });
                            string[] rowMEP_NIK = dtSet.Rows[0]["REPAIR_EMP_ID"].ToString().Split(new Char[] { ';' });
                            string[] rowMEP_Name = dtSet.Rows[0]["REPAIR_EMP_NM"].ToString().Split(new Char[] { ';' });

                            dtMatPart = grdSparePart.DataSource as DataTable;

                            if (rowMat[0] != "")
                            {
                                foreach (string row in rowMat)
                                {
                                    string[] colDt = row.Split(new Char[] { '~' });
                                    object[] obj = new object[3];
                                    int i = 0;

                                    foreach (string dt in colDt)
                                    {
                                        obj[i] = dt;
                                        i++;
                                    }

                                    dtMatPart.Rows.Add(obj);
                                }

                                SetData(grdSparePart, dtMatPart);
                            }

                            dtMEP_Repair = grdMEP.DataSource as DataTable;

                            if (rowMEP_NIK[0] != "" && rowMEP_Name[0] != "")
                            {
                                //foreach (string nik in rowMEP_NIK)
                                //{
                                //    obj
                                //}
                                for (int i = 0; i < rowMEP_NIK.Length; i++)
                                {
                                    dtMEP_Repair.Rows.Add(new object[] { rowMEP_NIK[i], rowMEP_Name[i] });   
                                }

                                
                                SetData(grdMEP, dtMEP_Repair);
                            }
                        }

                        if (clrStatus == "F")
                        {
                            //cbNikConfirm.Text = dtSet.Rows[0]["CFM_EMPID"].ToString();
                            txtNikConfirm.Text = dtSet.Rows[0]["CFM_EMPID"].ToString();
                            lblEmpNameConfirm.Text = dtSet.Rows[0]["CFM_EMP_NM"].ToString();
                            lblCfmDate.Text = dtSet.Rows[0]["CFM_DT"].ToString();
                        }
                    }
                }
            }

            switch (clrStatus)
            {
                case "R":
                    pnlRequest.Enabled = true;
                    pnlCheck.Enabled = true;

                    if (lblDownTimeRequest.Text.ToString() != "")
                        btnCall.Enabled = true;
                    
                    break;

                case "C":
                    pnlCheck.Enabled = true;
			        pnlRepair.Enabled = true;
			        if (lblDownTimeRepair.Text.ToString() != "-")
			        {
				        pnlCheck.Enabled = false;
				        pnlDone.Enabled = true;
				        btnCall_TL.Enabled = true;
			        }

                    //2024.12.31 enabled only wo no
                    pnlRequest.Enabled = true;
                    txtNikRequest.Enabled = false;
                    lookEMC_ID.Enabled = true;
                    txtCondition.Enabled = false;
                    rgMechanic.Enabled = false;
                    btnCall.Enabled = false;
                    btnRequestSave.Enabled = false;
                    lblWoNo.Enabled = true;
                    lblWoNo.ReadOnly = true;
                    cboWoType.ReadOnly = true;
                    break;

                case "":
                    pnlRequest.Enabled = true;
                    cboWoType.ReadOnly = false;
                    txtNikRequest.Enabled = true;
                    rgMechanic.Enabled = true;
                    txtCondition.Enabled = true;
                    btnRequestSave.Enabled = true;
                    break;
            }
        }

        private void TL_QC_Click(object sender)
        {
            Label lbl = (Label)sender;
            int chk = 0;

            chk = lbl.Name.IndexOf("_") + 1;

            selectedOP_CD = lbl.Name.Substring(chk - 4, 3).ToUpper();
            selectedDivision = lbl.Name.Substring(lbl.Name.IndexOf("_", chk) + 1, 2);

            if (cboPlant.EditValue.ToString() == "3110" && cboArea.EditValue.ToString() != "OSR" && cboArea.EditValue.ToString() != "PUR,BEA,BEM") //2024.10.07
                mc_line = int.Parse(dtAssyLine.Rows[int.Parse(lbl.Name.Substring(chk, 1)) - 1]["CODE"].ToString()).ToString();
            else
                mc_line = lbl.Name.Substring(chk, 1);

            chk = lbl.Name.IndexOf("zone") + 4;
            mc_no = lbl.Name.Substring(chk, lbl.Name.Length - chk);

            if (selectedDivision == "QC")
            {
                pnlCallTLQC.Enabled = true;
                pnlCallTLQC.Visible = true;
                txtQOP_CD.Text = selectedOP_CD;
                txtQMc_Line.Text = mc_line;
                txtQMc_No.Text = mc_no;
                pnlCallTLQC.BringToFront();
                pnlCallTLQC.Location = new Point(
                                            this.ClientSize.Width / 2 - pnlCallTLQC.Size.Width / 2,
                                            this.ClientSize.Height / 2 - pnlCallTLQC.Size.Height / 2);
            }
            else
            {
                string tmp = "";

                if (lbl.Name.ToUpper().IndexOf("PHM") >= 0)
                {
                    tmp = "ST";
                }
                else
                {
                    tmp = "M/C";
                }

                if (lbl.Name.ToUpper().IndexOf("PHM") >= 0 || lbl.Name.ToUpper().IndexOf("BUF") >= 0 ||
                    lbl.Name.ToUpper().IndexOf("PHH") >= 0 || lbl.Name.ToUpper().IndexOf("PHU") >= 0 ||
                    lbl.Name.ToUpper().IndexOf("OSR") >= 0 || lbl.Name.ToUpper().IndexOf("PUR,BEA,BEM") >= 0 //2024.10.07
                   )
                {
                    selectedDivision = "TL";
                }


                if (MessageBox.Show("Do you want to Call " + selectedDivision + " for " + selectedOP_CD + " M/C Line : " + mc_line + "  " + tmp + " No : " + mc_no + " ? ", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    fn_Save("CALL_TL_QC", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), selectedOP_CD, "", selectedDivision, "", "", "", "", "", "", "", "", mc_line, mc_no);
            }
        }

        private void showPopUp_CallTLQC(string param_OP_CD, string param_mc_line, string param_mc_no, object sender)
        {
            if (pnlCallTLQC.Visible || pnlPopUp.Visible)
                return;

            if (pnlCallTLQC.Visible || pnlPopUp.Visible)
                return;

            if (sender != null)
            {
                Label lbl = (Label)sender;
                int chk = 0;

                chk = lbl.Name.IndexOf("_") + 1;

                selectedOP_CD = lbl.Name.Substring(chk - 4, 3).ToUpper();
                selectedDivision = lbl.Name.Substring(lbl.Name.IndexOf("_", chk) + 1, 2);

                if (dtAssyLine != null && dtAssyLine.Rows.Count > 0 && (int.Parse(lbl.Name.Substring(chk, 1)) - 1) >= dtAssyLine.Rows.Count)
                    return;

                if (cboPlant.EditValue.ToString() == "3110" && cboArea.EditValue.ToString() != "OSR" && cboArea.EditValue.ToString() != "PUR,BEA,BEM" && cboLocation.EditValue.ToString() != "51SL")
                    mc_line = int.Parse(dtAssyLine.Rows[int.Parse(lbl.Name.Substring(chk, 1)) - 1]["CODE"].ToString()).ToString();
                else
                {
                    if (lbl.Name.Substring(chk, 2).ToString().Contains('_'))
                        mc_line = lbl.Name.Substring(chk, 1);
                    else
                        mc_line = lbl.Name.Substring(chk, 2);
                }

                chk = lbl.Name.IndexOf("zone") + 4;
                mc_no = lbl.Name.Substring(chk, lbl.Name.Length - chk);
                
                txtQOP_CD.Text = selectedOP_CD;
                txtQMc_Line.Text = mc_line;
                txtQMc_No.Text = mc_no;
            }
            else
            {
                mc_line = param_mc_line;
                mc_no = param_mc_no;
                selectedOP_CD = cboArea.EditValue.ToString();
                txtQOP_CD.Text = param_OP_CD;
                txtQMc_Line.Text = mc_line;
                txtQMc_No.Text = mc_no;
            }

            setPnlCallTLQC("Y");
        }

        #region PLANT C
        //FSS 1 MEP
        private void lblFss_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFss_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //FSS TL
        private void lblFss_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //FSS QC
        private void lblFss_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFss_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //FGA MEP
        private void lblFga_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblFga_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //FGA TL
        private void lblFga_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //FGA QC
        private void lblFga_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblFga_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        #endregion

        #region PLANT D
        //FSS MEP
        private void lblDFss_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_5_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_5_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_5_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFss_5_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //FSS TL
        private void lblDFss_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_5_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_5_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_5_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_5_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //FSS QC
        private void lblDFss_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_5_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_5_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_5_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFss_5_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        // FGA MEP
        private void lblDFga_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_5_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_5_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_5_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDFga_5_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //FGA TL
        private void lblDFga_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_5_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_5_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_5_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_5_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //FGA QC
        private void lblDFga_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_5_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_5_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_5_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDFga_5_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        #endregion  

        private void cboArea_EditValueChanged(object sender, EventArgs e)
        {
            setShowLayout();

            switch (cboArea.EditValue.ToString())
            {
                case "OSP":
                    //QueryClick();
                    break;

                case "FSS,FGA":
                    if (
                        cboLocation.EditValue.ToString() == "51B1" || cboLocation.EditValue.ToString() == "51C1"
                        || cboLocation.EditValue.ToString() == "51E1" //|| cboLocation.EditValue.ToString() == "51F1"
                        || cboLocation.EditValue.ToString() == "51A1" || cboLocation.EditValue.ToString() == "51A3"
                        || cboLocation.EditValue.ToString() == "51G1"
                       )
                        setContentLayout(this.layoutPlantC_FSS_FGA, null);
                    else
                        //if (cboLocation.EditValue.ToString() == "51D1")
                            setContentLayout(this.layoutD_FSS_FGA, null);
                    //else
                    //    if (cboLocation.EditValue.ToString() == "51W1")
                    //        setContentLayout(this.layoutPlantMMTL, null);
                    //QueryClick();
                    break;
                case "UPS":
                    if (cboLocation.EditValue.ToString() != "51D1")
                        setContentLayout(this.layoutPlantC_UPS, null);
                    else
                        setContentLayout(this.layoutPlantD_UPS, null);

                    //QueryClick();
                    break;
                case "UPC,UPN":
                    if (cboLocation.EditValue.ToString() != "51D1")
                        setContentLayout(this.layoutPlantC_UPC_UPN, null);
                    else
                        setContentLayout(this.layoutPlantD_UPC_UPN, null);
                    //QueryClick();
                    break;
                case "PHM,BUF,PHH,PHU":
                    setContentLayout(this.layoutCKP_PH, null);
                    //QueryClick();
                    break;
                case "UPC,UPN,UPS,FSS,FGA":
                    setContentLayout(this.layoutPlantMMTL, null);
                    break;
                case "IPI,IPU":
                    setContentLayout(this.layoutCKP_IP, null);
                    break;
                case "OSR":
                    setContentLayout(this.layoutOSR, null);
                    break;
                #region [COMMENT]
                /*case "CIN,SKI,INC":
                    setContentLayout(this.layoutAcc_CinSkiInc, null);
                    break;
                case "UPE,UPF":
                    setContentLayout(this.layoutAcc_UPE_UPF, null);
                    break;
                case "UPH":
                    setContentLayout(this.layoutAcc_UPH, null);
                    break;*/
                #endregion
                case "PUR,BEA,BEM":
                    setContentLayout(this.layoutPUR, null); //2024.10.04
                    break;
                case "CIN,SKI,INC,UPE,UPF,UPH":
                    setContentLayout(this.layoutAcc, null); //2024.11.08
                    break;
            }
        }

        private void setShowLayout()
        {
            grdCtrl.Visible = false;
            grdCtrl.Dock = DockStyle.None;
            
            
            layoutPlantC_FSS_FGA.Visible = false;
            layoutPlantC_FSS_FGA.Dock = DockStyle.None;
            
            layoutPlantC_UPS.Visible = false;
            layoutPlantC_UPS.Dock = DockStyle.None;

            
            layoutPlantC_UPC_UPN.Visible = false;
            layoutPlantC_UPC_UPN.Dock = DockStyle.None;
            layoutCKP_PH.Visible = false;
            layoutCKP_PH.Dock = DockStyle.None;
            layoutD_FSS_FGA.Visible = false;
            layoutD_FSS_FGA.Dock = DockStyle.None;           
            layoutPlantD_UPS.Visible = false;
            layoutPlantD_UPS.Dock = DockStyle.None;
            layoutPlantD_UPC_UPN.Visible = false;
            layoutPlantD_UPC_UPN.Dock = DockStyle.None;
            layoutPlantMMTL.Visible = false;
            layoutPlantMMTL.Dock = DockStyle.None;
            layoutCKP_IP.Visible = false;
            layoutCKP_IP.Dock = DockStyle.None;
            layoutOSR.Visible = false;
            layoutOSR.Dock = DockStyle.None;
            #region [COMMENT]
            //layoutAcc_CinSkiInc.Visible = false;
            //layoutAcc_CinSkiInc.Dock = DockStyle.None;
            //layoutAcc_UPE_UPF.Visible = false;
            //layoutAcc_UPE_UPF.Dock = DockStyle.None;
            //layoutAcc_UPH.Visible = false;
            //layoutAcc_UPH.Dock = DockStyle.None;
            #endregion
            layoutPUR.Visible = false; //2024.10.04
            layoutPUR.Dock = DockStyle.None;
            layoutPlantH.Visible = false;
            layoutPlantH.Dock = DockStyle.None;
            layoutAcc.Visible = false; //2024.11.08
            layoutAcc.Dock = DockStyle.None;
            layoutPlantG.Visible = false;
            layoutPlantG.Dock = DockStyle.None;

            if (!_bFormLoaded)
                return;

            if (cboArea.EditValue.ToString() == "OSP")
            {
                grdCtrl.Dock = DockStyle.Fill;
                grdCtrl.Visible = true;
                grdCtrl.BringToFront();
                //layoutPlantC_FSS_FGA.Dock = DockStyle.None;
                //layoutPlantC_FSS_FGA.Visible = false;
            }
            else if (cboArea.EditValue.ToString() == "OSR")
            {
                layoutOSR.Dock = DockStyle.Fill;
                layoutOSR.Visible = true;
                layoutOSR.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "PUR,BEA,BEM") //2024.10.04
            {
                layoutPUR.Dock = DockStyle.Fill;
                layoutPUR.Visible = true;
                layoutPUR.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "FSS,FGA" && (cboLocation.EditValue.ToString() == "51B1" || cboLocation.EditValue.ToString() == "51C1"
                                                                   || cboLocation.EditValue.ToString() == "51E1" //|| cboLocation.EditValue.ToString() == "51F1"
                                                                   || cboLocation.EditValue.ToString() == "51A1" || cboLocation.EditValue.ToString() == "51A3"
                                                                   /*|| cboLocation.EditValue.ToString() == "51G1"*/))
            {
                //grdCtrl.Dock = DockStyle.None;
                //grdCtrl.Visible = false;
                layoutPlantC_FSS_FGA.Dock = DockStyle.Fill;
                layoutPlantC_FSS_FGA.Visible = true;
                layoutPlantC_FSS_FGA.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "FSS,FGA" && (cboLocation.EditValue.ToString() == "51D1"
                                                                    || cboLocation.EditValue.ToString() == "51A2"
                                                                    || cboLocation.EditValue.ToString() == "51A4"
                                                                    || cboLocation.EditValue.ToString() == "51F1"))
            {
                layoutD_FSS_FGA.Dock = DockStyle.Fill;
                layoutD_FSS_FGA.Visible = true;
                layoutD_FSS_FGA.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "FGA,FSS,UPC,UPN,UPS" && (cboLocation.EditValue.ToString() == "51H1"))
            {
                layoutPlantH.Dock = DockStyle.Fill;
                layoutPlantH.Visible = true;
                layoutPlantH.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "UPS" && (cboLocation.EditValue.ToString() == "51B1" || cboLocation.EditValue.ToString() == "51C1" || cboLocation.EditValue.ToString() == "51G1"))
            {
                layoutPlantC_UPS.Dock = DockStyle.Fill;
                layoutPlantC_UPS.Visible = true;
                layoutPlantC_UPS.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "UPS" && cboLocation.EditValue.ToString() == "51D1")
            {
                layoutPlantD_UPS.Dock = DockStyle.Fill;
                layoutPlantD_UPS.Visible = true;
                layoutPlantD_UPS.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "UPC,UPN" && (cboLocation.EditValue.ToString() == "51B1" || cboLocation.EditValue.ToString() == "51C1" || cboLocation.EditValue.ToString() == "51G1"))
            {
                layoutPlantC_UPC_UPN.Dock = DockStyle.Fill;
                layoutPlantC_UPC_UPN.Visible = true;
                layoutPlantC_UPC_UPN.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "UPC,UPN" && cboLocation.EditValue.ToString() == "51D1")
            {
                layoutPlantD_UPC_UPN.Dock = DockStyle.Fill;
                layoutPlantD_UPC_UPN.Visible = true;
                layoutPlantD_UPC_UPN.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "PHM,BUF,PHH,PHU")
            {
                layoutCKP_PH.Dock = DockStyle.Fill;
                layoutCKP_PH.Visible = true;
                layoutCKP_PH.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "UPC,UPN,UPS,FSS,FGA" && cboLocation.EditValue.ToString() == "51W1")
            {
                layoutPlantMMTL.Dock = DockStyle.Fill;
                layoutPlantMMTL.Visible = true;
                layoutPlantMMTL.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "IPI,IPU" && cboLocation.EditValue.ToString() == "51IP")
            {
                layoutCKP_IP.Dock = DockStyle.Fill;
                layoutCKP_IP.Visible = true;
                layoutCKP_IP.BringToFront();
            }

            #region [COMMENT]
            /*else if (cboLocation.EditValue.ToString() == "51SL" && cboArea.EditValue.ToString() == "CIN,SKI,INC")
            {
                layoutAcc_CinSkiInc.Dock = DockStyle.Fill;
                layoutAcc_CinSkiInc.Visible = true;
                layoutAcc_CinSkiInc.BringToFront();
            }
            else if (cboLocation.EditValue.ToString() == "51SL" && cboArea.EditValue.ToString() == "UPE,UPF")
            {
                layoutAcc_UPE_UPF.Dock = DockStyle.Fill;
                layoutAcc_UPE_UPF.Visible = true;
                layoutAcc_UPE_UPF.BringToFront();
            }
            else if (cboLocation.EditValue.ToString() == "51SL" && cboArea.EditValue.ToString() == "UPH")
            {
                layoutAcc_UPH.Dock = DockStyle.Fill;
                layoutAcc_UPH.Visible = true;
                layoutAcc_UPH.BringToFront();
            }*/
            #endregion

            else if (cboLocation.EditValue.ToString() == "51SL" && cboArea.EditValue.ToString() == "CIN,SKI,INC,UPE,UPF,UPH") //2024.11.08
            {
                layoutAcc.Dock = DockStyle.Fill;
                layoutAcc.Visible = true;
                layoutAcc.BringToFront();
            }
            else if (cboArea.EditValue.ToString() == "FGA,FSS,UPC,UPN,UPS" && (cboLocation.EditValue.ToString() == "51G1"))
            {
                layoutPlantG.Dock = DockStyle.Fill;
                layoutPlantG.Visible = true;
                layoutPlantG.BringToFront();
            }
            else
                return;
        }

        private void setContentLayout(Control ctrl, DataTable dtTbl)
        {
            int chk = 0;
            int chk1 = 0;
            int chk2 = 0;
            int dtIdx = 0;
            string val = "";
            string str = "";
            string seq = "";
            string div = "";
            dtAssyLine = null;
            var dt1 = dtAllLine.AsEnumerable().Where(row => row.Field<string>("PLANT_CD") == cboPlant.EditValue.ToString() && row.Field<string>("REMARKS") == cboLocation.EditValue.ToString());

            if (dt1.Any())
            //{
                dtAssyLine = dt1.CopyToDataTable();
            //}
            //else
            //    return;

            List<Control> allControls = GetAllControls(ctrl);

            #region [COMMENT]
            //DataTable dtCek = new DataTable();
            //dtCek.Columns.Add("Name", typeof(string));
            //foreach (var cek in allControls)
            //{
            //    DataRow row = dtCek.NewRow();
            //    row["Name"] = cek.Name;
            //    dtCek.Rows.Add(row);
            //}
            #endregion

            //NGAKALIN YANG ANEH DI CONTROL 2023.04.05
            foreach (Control x in allControls)
            {
                if (x.Name == "lblIpi_1_MEP_zone200" || x.Name == "lblIpi_1_TL_zone200" || x.Name == "lblIpi_1_MEP_zone201" || x.Name == "lblIpi_1_TL_zone201" ||
                    x.Name == "lblIpi_1_MEP_zone202" || x.Name == "lblIpi_1_TL_zone202" || x.Name == "lblIpi_1_MEP_zone203" || x.Name == "lblIpi_1_TL_zone203" ||
                    x.Name == "lblIpi_1_MEP_zone204" || x.Name == "lblIpi_1_TL_zone204" || x.Name == "lblIpi_1_MEP_zone205" || x.Name == "lblIpi_1_TL_zone205" ||
                    x.Name == "lblIpi_1_MEP_zone206" || x.Name == "lblIpi_1_TL_zone206" || x.Name == "lblIpi_1_MEP_zone207" || x.Name == "lblIpi_1_TL_zone207" ||
                    x.Name == "lblIpi_1_MEP_zone208" || x.Name == "lblIpi_1_TL_zone208" || x.Name == "lblIpi_1_MEP_zone209" || x.Name == "lblIpi_1_TL_zone209" ||
                    x.Name == "lblIpi_1_MEP_zone210" || x.Name == "lblIpi_1_TL_zone210" || x.Name == "lblIpi_1_MEP_zone211" || x.Name == "lblIpi_1_TL_zone211" ||
                    x.Name == "lblIpi_1_MEP_zone212" || x.Name == "lblIpi_1_TL_zone212" || x.Name == "lblIpi_1_MEP_zone213" || x.Name == "lblIpi_1_TL_zone213" ||
                    x.Name == "lblIpi_1_MEP_zone214" || x.Name == "lblIpi_1_TL_zone214" || x.Name == "lblIpi_1_MEP_zone215" || x.Name == "lblIpi_1_TL_zone215" ||
                    x.Name == "lblIpi_1_MEP_zone216" || x.Name == "lblIpi_1_TL_zone216" || x.Name == "lblIpi_1_MEP_zone217" || x.Name == "lblIpi_1_TL_zone217" ||
                    x.Name == "lblIpi_1_MEP_zone218" || x.Name == "lblIpi_1_TL_zone218" || x.Name == "lblIpi_1_MEP_zone219" || x.Name == "lblIpi_1_TL_zone219" ||
                    x.Name == "lblIpi_1_MEP_zone220" || x.Name == "lblIpi_1_TL_zone220" || x.Name == "lblIpi_1_MEP_zone221" || x.Name == "lblIpi_1_TL_zone221" 
                    )
                {
                    x.Font = new Font("Tahoma", 8, FontStyle.Bold);

                    if (
                        x.Name.ToUpper().Contains("FSS") || x.Name.ToUpper().Contains("FGA") || x.Name.ToUpper().Contains("UPS_") || x.Name.ToUpper().Contains("UPS_")
                        || x.Name.ToUpper().Contains("UPC_") || x.Name.ToUpper().Contains("UPN_") || x.Name.ToUpper().Contains("PHM_") || x.Name.ToUpper().Contains("BUF_")
                        || x.Name.ToUpper().Contains("PHH_") || x.Name.ToUpper().Contains("PHU_") || x.Name.ToUpper().Contains("IPI_") || x.Name.ToUpper().Contains("IPU_")
                        || x.Name.ToUpper().Contains("OSR_") || x.Name.ToUpper().Contains("CIN") || x.Name.ToUpper().Contains("SKI") || x.Name.ToUpper().Contains("INC")
                        || x.Name.ToUpper().Contains("UPE") || x.Name.ToUpper().Contains("UPF") || x.Name.ToUpper().Contains("UPH") || x.Name.ToUpper().Contains("PUR")
                        || x.Name.ToUpper().Contains("BEA") || x.Name.ToUpper().Contains("BEM") //2024.10.04
                        )
                    {
                        x.Text = "";
                        x.Parent.Text = "";
                        x.BackColor = Color.White;
                    }

                    if (x.Name.ToUpper().Contains("TITLE") || x.Name.ToUpper().Contains("MLINE"))
                    {
                        str = x.Name.Substring(x.Name.Length - 1, 1);
                        //c.Text = plantC[int.Parse(str)-1];
                        if (dtAssyLine == null || (int.Parse(str) - 1) >= dtAssyLine.Rows.Count || dtAssyLine.Rows.Count < 1)
                            x.Text = "0";
                        else
                            x.Text = dtAssyLine.Rows[int.Parse(str) - 1]["CODE"].ToString();

                        continue;
                    }

                    if (dtTbl != null && dtTbl.Rows.Count > 0)
                    {
                        if (x.Name == "lblIpi_1_MEP_zone208" || x.Name == "lblIpi_1_MEP_zone206" || x.Name == "lblIpi_1_MEP_zone201")
                        {
                        }

                        chk = x.Name.IndexOf("_") + 1;
                        chk1 = x.Name.IndexOf("_", chk) + 1;

                        if (chk > 1 || (chk > 1 && dtAssyLine != null && dtAssyLine.Rows.Count > 0 && (int.Parse(x.Name.Substring(chk, 1)) - 1) < dtAssyLine.Rows.Count))
                        {
                            str = x.Name.ToUpper().Substring(chk - 4, 3);
                        }
                        else
                            continue;

                        chk2 = x.Name.IndexOf("zone") + 4;
                        seq = x.Name.Substring(chk2, x.Name.Length - chk2);
                        div = x.Name.Substring(chk1, 3);
                        dtIdx = int.Parse(x.Name.Substring(chk, 1)) - 1;

                        var dt = dtTbl.AsEnumerable().Where(row => row.Field<string>("OP_CD") == str
                            //&& row.Field<string>("MACHINE_LINE") == dtAssyLine.Rows[int.Parse(c.Name.Substring(chk, 1)) - 1]["CODE"].ToString() //plantC[int.Parse(c.Name.Substring(chk, 1)) - 1]
                                        && row.Field<string>("MACHINE_LINE") == ((cboPlant.EditValue.ToString() == "3110" && dtAssyLine != null && dtIdx < dtAssyLine.Rows.Count && cboArea.EditValue.ToString() != "OSR" && cboLocation.EditValue.ToString() != "51SL") ? int.Parse(dtAssyLine.Rows[dtIdx]["CODE"].ToString()).ToString() : x.Name.Substring(chk, 1)) //dtAssyLine.Rows[int.Parse(c.Name.Substring(chk, 1)) - 1]["CODE"].ToString()
                                        && row.Field<string>("MACHINE_SEQ") == seq
                                        && row.Field<string>("DIVISION") == div
                                    );

                        //string cekkk = ((cboPlant.EditValue.ToString() == "3110" && dtAssyLine != null && dtIdx < dtAssyLine.Rows.Count && cboArea.EditValue.ToString() != "OSR" && cboLocation.EditValue.ToString() != "51SL") ? int.Parse(dtAssyLine.Rows[dtIdx]["CODE"].ToString()).ToString() : c.Name.Substring(chk, 1)).ToString();

                        if (dt.Any())
                        {
                            x.Text = dt.CopyToDataTable().Rows[0]["CONT"].ToString();
                            val = dt.CopyToDataTable().Rows[0]["NEW_STATUS"].ToString();
                            x.Parent.Text = val.Substring(0, 1);

                            switch (val.Substring(val.Length - 1, 1))
                            {
                                case "R":
                                    x.BackColor = Color.Red;
                                    x.ForeColor = Color.White;
                                    break;
                                case "C":
                                    x.BackColor = Color.Yellow;
                                    x.ForeColor = Color.Black;
                                    break;
                                //case "F":
                                //    x.BackColor = Color.LightGreen;
                                //    x.ForeColor = Color.Black;
                                //    break;
                                case "B":
                                    x.BackColor = Color.Black;
                                    x.ForeColor = Color.White;
                                    break;
                                case "D":
                                    x.BackColor = Color.Gold;
                                    x.ForeColor = Color.Black;
                                    break;
                            }
                        }
                    }
                }
            }

            #region [ORIGINAL]
            foreach (Control c in allControls)
            {
                //try
                //{
                c.Font = new Font("Tahoma", 8, FontStyle.Bold);

                if (
                    c.Name.ToUpper().Contains("FSS") || c.Name.ToUpper().Contains("FGA") || c.Name.ToUpper().Contains("UPS_") || c.Name.ToUpper().Contains("UPS_")
                    || c.Name.ToUpper().Contains("UPC_") || c.Name.ToUpper().Contains("UPN_") || c.Name.ToUpper().Contains("PHM_") || c.Name.ToUpper().Contains("BUF_")
                    || c.Name.ToUpper().Contains("PHH_") || c.Name.ToUpper().Contains("PHU_") || c.Name.ToUpper().Contains("IPI_") || c.Name.ToUpper().Contains("IPU_")
                    || c.Name.ToUpper().Contains("OSR_") || c.Name.ToUpper().Contains("CIN") || c.Name.ToUpper().Contains("SKI") || c.Name.ToUpper().Contains("INC")
                    || c.Name.ToUpper().Contains("UPE") || c.Name.ToUpper().Contains("UPF") || c.Name.ToUpper().Contains("UPH") || c.Name.ToUpper().Contains("PUR")
                    || c.Name.ToUpper().Contains("BEA") || c.Name.ToUpper().Contains("BEM")
                    )
                {

                    c.Text = "";
                    c.Parent.Text = "";
                    c.BackColor = Color.White;
                }

                //if (c.Name.ToUpper().Contains("TITLE_FS") || c.Name.ToUpper().Contains("TITLE_FG") || c.Name.ToUpper().Contains("TITLE_UPS") || c.Name.ToUpper().Contains("MLINE"))
                if (c.Name.ToUpper().Contains("TITLE") || c.Name.ToUpper().Contains("MLINE"))
                {
                    str = c.Name.Substring(c.Name.Length - 1, 1);
                    //c.Text = plantC[int.Parse(str)-1];
                    if (dtAssyLine == null || (int.Parse(str) - 1) >= dtAssyLine.Rows.Count || dtAssyLine.Rows.Count < 1)
                        c.Text = "0";
                    else
                        c.Text = dtAssyLine.Rows[int.Parse(str) - 1]["CODE"].ToString();

                    continue;
                }

                if (dtTbl != null && dtTbl.Rows.Count > 0)
                {
                    chk = c.Name.IndexOf("_") + 1;
                    chk1 = c.Name.IndexOf("_", chk) + 1;

                    if (chk > 1 || (chk > 1 && dtAssyLine != null && dtAssyLine.Rows.Count > 0 && (int.Parse(c.Name.Substring(chk, 1)) - 1) < dtAssyLine.Rows.Count))
                    {
                        str = c.Name.ToUpper().Substring(chk - 4, 3);
                    }
                    else
                        continue;

                    chk2 = c.Name.IndexOf("zone") + 4;
                    seq = c.Name.Substring(chk2, c.Name.Length - chk2);
                    div = c.Name.Substring(chk1, 3);
                    dtIdx = int.Parse(c.Name.Substring(chk, 1)) - 1;

                    string mc_line = ""; //2024.10.18

                    if (c.Name.Substring(chk, 2).ToString().Contains('_'))
                        mc_line = c.Name.Substring(chk, 1);
                    else
                        mc_line = c.Name.Substring(chk, 2);


                    var dt = dtTbl.AsEnumerable().Where(row => row.Field<string>("OP_CD") == str
                        //&& row.Field<string>("MACHINE_LINE") == dtAssyLine.Rows[int.Parse(c.Name.Substring(chk, 1)) - 1]["CODE"].ToString() //plantC[int.Parse(c.Name.Substring(chk, 1)) - 1]
                                    //&& row.Field<string>("MACHINE_LINE") == ((cboPlant.EditValue.ToString() == "3110" && dtAssyLine != null && dtIdx < dtAssyLine.Rows.Count && cboArea.EditValue.ToString() != "OSR" && cboArea.EditValue.ToString() != "PUR,BEA,BEM" && cboLocation.EditValue.ToString() != "51SL") ? int.Parse(dtAssyLine.Rows[dtIdx]["CODE"].ToString()).ToString() : c.Name.Substring(chk, 1)) //dtAssyLine.Rows[int.Parse(c.Name.Substring(chk, 1)) - 1]["CODE"].ToString()
                                    && row.Field<string>("MACHINE_LINE") == ((cboPlant.EditValue.ToString() == "3110" && dtAssyLine != null && dtIdx < dtAssyLine.Rows.Count && cboArea.EditValue.ToString() != "OSR" && cboArea.EditValue.ToString() != "PUR,BEA,BEM" && cboLocation.EditValue.ToString() != "51SL") ? int.Parse(dtAssyLine.Rows[dtIdx]["CODE"].ToString()).ToString() : mc_line)
                                    && row.Field<string>("MACHINE_SEQ") == seq
                                    && row.Field<string>("DIVISION") == div
                                );

                    //string cekkk = ((cboPlant.EditValue.ToString() == "3110" && dtAssyLine != null && dtIdx < dtAssyLine.Rows.Count && cboArea.EditValue.ToString() != "OSR" && cboLocation.EditValue.ToString() != "51SL") ? int.Parse(dtAssyLine.Rows[dtIdx]["CODE"].ToString()).ToString() : c.Name.Substring(chk, 1)).ToString();
                    if (dt.Any())
                    {
                        //DataTable xxx = dt.CopyToDataTable();
                        val = dt.CopyToDataTable().Rows[0]["NEW_STATUS"].ToString();
                        if (val == "F")
                        {
                            c.Text = null;
                        }
                        else
                        {
                            c.Text = dt.CopyToDataTable().Rows[0]["CONT"].ToString();
                            c.Parent.Text = val.Substring(0, 1);
                        }


                        switch (val.Substring(val.Length - 1, 1))
                        {
                            case "R":
                                c.BackColor = Color.Red;
                                c.ForeColor = Color.White;
                                break;
                            case "C":
                                c.BackColor = Color.Yellow;
                                c.ForeColor = Color.Black;
                                break;
                            //case "F":
                            //    c.BackColor = Color.LightGreen;
                            //    c.ForeColor = Color.Black;
                            //    break;
                            case "B":
                                c.BackColor = Color.Black;
                                c.ForeColor = Color.White;
                                break;
                            case "D":
                                c.BackColor = Color.Gold;
                                c.ForeColor = Color.Black;
                                break;
                        }
                    }
                }
                //}
                //catch(Exception ex)
                //{

                //}
            }
            #endregion [ORIGINAL]
        }

        #region UPS PLANT B, C
        // UPS MEP
        private void lblUps_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_1_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_2_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        
        private void lblUps_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_3_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUps_4_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        // UPS TL
        private void lblUps_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPS QC
        private void lblUps_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_1_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_2_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_3_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUps_4_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        #endregion

        #region UPS PLANT D
        //UPS MEP
        private void lblDUps_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_1_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUps_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_2_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUps_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_3_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUps_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_4_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUps_5_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblDUps_5_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //UPS TL
        private void lblDUps_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUps_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUps_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUps_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUps_5_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPS QC
        private void lblDUps_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_1_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUps_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_2_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUps_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_3_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUps_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_4_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUps_5_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblDUps_5_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        #endregion

        #region UPN
        //UPN MEP
        private void lblUpn_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_2_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_3_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpn_4_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //UPN TL
        private void lblUpn_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPN QC
        private void lblUpn_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_2_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_3_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpn_4_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //Plant D
        //UPN MEP
        private void lblDUpn_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_2_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_3_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_4_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpn_5_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //Plant D
        //UPN TL
        private void lblDUpn_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //Plant D
        //UPN QC
        private void lblDUpn_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_2_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_3_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_4_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpn_5_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        #endregion

        #region UPC
        //UPC MEP
        private void lblUpc_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_2_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_3_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblUpc_4_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //UPC TL
        private void lblUpc_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPC QC
        private void lblUpc_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_2_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_3_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblUpc_4_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //PLANT D
        //UPC MEP
        private void lblDUpc_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_2_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_3_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_4_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblDUpc_5_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //UPC TL
        private void lblDUpc_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPC QC
        private void lblDUpc_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_2_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_3_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_4_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblDUpc_5_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        #endregion

        #region PH CKP
        // PHM
        private void lblPhm_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_2_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_2_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_3_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_3_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_3_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_3_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_4_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_4_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_4_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhm_4_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        // BUFFING
        private void lblBuf_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone10_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone11_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone12_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone13_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone14_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone15_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone16_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblBuf_1_MEP_zone17_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblBuf_1_MEP_zone18_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblBuf_1_MEP_zone19_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblBuf_1_MEP_zone20_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        // PHH
        private void lblPhh_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_2_MEP_zone9_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_2_MEP_zone10_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_2_MEP_zone11_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_2_MEP_zone12_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_2_MEP_zone13_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone14_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone15_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone16_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone17_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone18_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone19_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone20_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone21_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhh_3_MEP_zone22_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPhh_3_MEP_zone23_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPhh_3_MEP_zone24_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPhh_4_MEP_zone25_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPhh_4_MEP_zone26_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPhh_4_MEP_zone27_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        // PHU
        private void lblPhu_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhu_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblPhu_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        //2024.12.12
        private void lblPhm_1_MEP_zone102_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPhm_1_MEP_zone227_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }
        #endregion

        #region PLANT MMTL
        //FSS MEP
        private void lblEcoFss_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoFss_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoFss_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoFss_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //FSS TL
        private void lblEcoFss_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFss_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender); 
        }

        private void lblEcoFss_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFss_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //FSS QC
        private void lblEcoFss_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFss_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFss_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFss_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //FGA MEP
        private void lblEcoFga_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoFga_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoFga_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoFga_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //FGA TL
        private void lblEcoFga_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFga_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFga_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFga_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //FGA QC
        private void lblEcoFga_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFga_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFga_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoFga_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPS MEP
        private void lblEcoUps_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUps_1_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //UPS TL
        private void lblEcoUps_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPS QC
        private void lblEcoUps_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUps_1_QC_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPN MEP
        private void lblEcoUpn_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpn_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpn_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpn_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpn_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpn_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpn_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpn_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpn_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //UPN TL
        private void lblEcoUpn_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPN QC
        private void lblEcoUpn_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpn_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPC MEP
        private void lblEcoUpc_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpc_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpc_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpc_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpc_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpc_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpc_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpc_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblEcoUpc_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //UPC TL
        private void lblEcoUpc_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //UPC QC
        private void lblEcoUpc_1_QC_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_QC_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_QC_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_QC_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_QC_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_QC_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_QC_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_QC_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        private void lblEcoUpc_1_QC_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        #endregion

        #region IP CKP
        //ZONE 1
        private void lblIpi_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //ZONE 2
        private void lblIpi_2_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_2_MEP_zone8_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_2_MEP_zone9_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_2_MEP_zone10_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_2_MEP_zone11_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_2_MEP_zone12_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //ZONE 3
        private void lblIpi_3_MEP_zone13_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_3_MEP_zone14_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_3_MEP_zone15_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_3_MEP_zone16_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_3_MEP_zone17_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_3_MEP_zone18_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //ZONE 4
        private void lblIpi_4_MEP_zone19_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_4_MEP_zone20_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_4_MEP_zone21_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_4_MEP_zone22_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_4_MEP_zone23_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //ZONE 5
        private void lblIpi_5_MEP_zone24_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_5_MEP_zone25_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_5_MEP_zone26_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_5_MEP_zone27_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_5_MEP_zone28_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_5_MEP_zone29_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //ZONE 6
        private void lblIpi_6_MEP_zone30_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_6_MEP_zone31_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_6_MEP_zone32_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_6_MEP_zone33_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_6_MEP_zone34_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_6_MEP_zone35_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //ZONE 7
        private void lblIpi_7_MEP_zone36_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_7_MEP_zone37_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_7_MEP_zone38_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_7_MEP_zone39_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_7_MEP_zone40_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpi_7_MEP_zone41_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        //IPU 
        private void lblIpu_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpu_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpu_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpu_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpu_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpu_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblIpu_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }


        private void lblIpi_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //ZONE 2
        private void lblIpi_2_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_2_TL_zone8_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_2_TL_zone9_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_2_TL_zone10_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_2_TL_zone11_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_2_TL_zone12_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //ZONE 3
        private void lblIpi_3_TL_zone13_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_3_TL_zone14_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_3_TL_zone15_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_3_TL_zone16_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_3_TL_zone17_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_3_TL_zone18_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //ZONE 4
        private void lblIpi_4_TL_zone19_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_4_TL_zone20_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_4_TL_zone21_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_4_TL_zone22_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_4_TL_zone23_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //ZONE 5
        private void lblIpi_5_TL_zone24_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_5_TL_zone25_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_5_TL_zone26_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_5_TL_zone27_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_5_TL_zone28_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_5_TL_zone29_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //ZONE 6
        private void lblIpi_6_TL_zone30_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_6_TL_zone31_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_6_TL_zone32_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_6_TL_zone33_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_6_TL_zone34_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_6_TL_zone35_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //ZONE 7
        private void lblIpi_7_TL_zone36_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_7_TL_zone37_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_7_TL_zone38_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_7_TL_zone39_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpi_7_TL_zone40_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //IPU 
        private void lblIpu_1_TL_zone1_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpu_1_TL_zone2_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpu_1_TL_zone3_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpu_1_TL_zone4_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpu_1_TL_zone5_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpu_1_TL_zone6_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }
        private void lblIpu_1_TL_zone7_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("","","",sender);
        }

        //2023-03-31 New Layout
        private void lblPolymer_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblPolymer_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblPigment_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblPigment_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblCompound_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblCompound_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblMix1_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblMix1_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblMix2_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblMix2_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblMix3_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblMix3_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblMix4_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblMix4_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblKneader_A_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblKneader_A_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblKneader_B_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblKneader_B_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblKneader_C_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblKneader_C_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblKneader_D_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblKneader_D_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblRoll_A_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblRoll_A_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblRoll_B_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblRoll_B_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblRoll_C_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblRoll_C_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblRoll_D_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblRoll_D_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblEx_A_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblEx_A_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblEx_B_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblEx_B_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblEx_C_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblEx_C_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblEx_D_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblEx_D_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblPallet_A_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblPallet_A_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblPallet_B_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblPallet_B_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblPallet_C_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblPallet_C_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblPallet_D_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblPallet_D_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblSpray_1_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblSpray_1_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblSpray_2_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblSpray_2_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblSpray_3_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblSpray_3_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }
        private void lblPadPrinting_1_MEP_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }
        private void lblPadPrinting_1_TL_Click(object sender, EventArgs e)
        {
            showPopUp_CallTLQC("", "", "", sender);
        }

        //2024.12.10
        private void lblIpi_1_MEP_zone244_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone227_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone228_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone229_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone230_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone231_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone232_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone233_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone234_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone235_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone236_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone237_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone238_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone239_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone240_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone241_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone242_Click(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        private void lblIpi_1_MEP_zone242_Click_1(object sender, EventArgs e)
        {
            MEPClick(sender);
        }

        #endregion

        #region OSR
        //LINE 1
        private void lblOsr_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        //LINE 2
        private void lblOsr_2_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_2_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_2_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_2_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_2_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        //LINE 3
        private void lblOsr_3_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_3_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        //LINE 4
        private void lblOsr_4_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_4_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_4_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_4_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_4_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        //LINE 5
        private void lblOsr_5_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_5_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        //LINE 6
        private void lblOsr_6_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        private void lblOsr_6_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("","","",sender);
        }

        //New Layout 2023-04-15
        private void lblOsr_1_MEP_zone100_Click(object sender, EventArgs e) //POLYMER
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblOsr_1_MEP_zone101_Click(object sender, EventArgs e) //PIGMEN
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblOsr_1_MEP_zone102_Click(object sender, EventArgs e) //AUTO WEIGHING
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblOsr_1_MEP_zone103_Click(object sender, EventArgs e) //EVA AUTO WEIGHING
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        #endregion

        #region [PUR]

        private void lblPUR_1_zone100_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_4_zone104_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_1_zone107_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        //Midsole Spray

        private void lblPUR_3_MEP_zone111_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_6_MEP_zone114_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_9_MEP_zone117_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_12_MEP_zone120_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        //Rotari

        private void lblPUR_2_MEP_zone122_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_6_MEP_zone126_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);

        }

        private void lblPUR_5_MEP_zone125_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_9_MEP_zone129_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_8_MEP_zone128_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_3_MEP_zone123_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_1_zone130_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_2_zone131_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_3_zone132_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_4_zone133_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_5_zone134_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_6_zone135_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_7_zone136_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_8_zone137_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_1_zone138_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_2_zone139_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_3_zone140_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_1_zone141_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_1_zone142_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_2_zone143_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_1_MEP_zone144_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_2_MEP_zone145_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblPUR_3_MEP_zone146_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        #endregion [PUR]

        #region CUP INSOLE, SKIVING, INSOLE CUT [not in use]
        private void lblCin_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone10_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone11_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone12_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblSki_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblSki_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblSki_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }
        #endregion

        #region EMBRIODERY, H/F WELDING
        private void lblUpe_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone2_Click(object sender, EventArgs e)
        {

        }

        private void lblUpe_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone10_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone11_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone12_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone13_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone14_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone15_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone16_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone17_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone18_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone19_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone20_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone21_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone9_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone10_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone11_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone12_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        #endregion

        #region MOLDING
        private void lblUph_1_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUph_1_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUph_1_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUph_1_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUph_1_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUph_1_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }
        #endregion

        #region [PLANT H]

        private void lblFGA_47_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_47_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_47_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_47_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_47_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_47_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_47_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFSS_47_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFSS_47_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_47_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_47_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_47_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_47_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_47_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_47_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_47_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_47_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_47_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_47_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_47_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_47_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone9_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone10_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_47_MEP_zone11_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_48_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_48_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_48_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_48_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_48_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_48_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_48_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFSS_48_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFSS_48_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_48_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_48_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_48_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_48_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_48_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_48_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_48_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_48_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_48_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_48_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_48_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_48_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone8_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone9_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone10_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_48_MEP_zone11_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }


        #endregion [PLANT H]

        #region [ACCESSORIES]

        private void lblCin_1_MEP_zone1_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone2_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone3_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone4_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone5_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblCin_1_MEP_zone6_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone1_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone2_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone3_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone4_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblInc_1_MEP_zone5_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblSki_1_MEP_zone1_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblSki_1_MEP_zone2_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblSki_1_MEP_zone100_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone1_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone2_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone3_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone4_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpe_1_MEP_zone5_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone1_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone2_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone3_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone4_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone5_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone6_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone101_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUpf_1_MEP_zone102_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone1_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone2_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone3_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone4_Click_1(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone103_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone105_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone106_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone107_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone108_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone109_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone110_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone111_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPH_1_MEP_zone112_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        #endregion [ACCESSORIES]

        #region [PLANT G]

        private void lblFGA_44_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_44_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_44_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_44_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_44_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_44_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_44_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_45_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_45_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_45_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_45_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_45_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_45_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFGA_45_MEP_zone7_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFSS_44_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFSS_44_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFSS_45_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblFSS_45_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_44_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_44_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_44_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_44_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_45_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_45_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_45_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPS_45_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_44_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_44_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_44_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_44_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_45_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_45_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_45_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPC_45_MEP_zone4_Click(object sender, EventArgs e)
        {

        }

        private void lblUPN_44_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_44_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_44_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_44_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_44_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_44_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_45_MEP_zone1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_45_MEP_zone2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_45_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_45_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_45_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblUPN_45_MEP_zone6_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        #endregion

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (lblEmpNameConfirm.Text == "Not Found" || lblEmpNameConfirm.Text == "" || lblEmpNameConfirm.Text == " ")
            {
                //MessageBoxW("Please fill out NIK field !");
                MessageBoxW("Silahkan isi kolom NIK");
                return;
            }

            if (teCurrentPass.EditValue == null || teCurrentPass.EditValue.ToString() == "" || teCurrentPass.EditValue.ToString() == " ")
            {
                //MessageBoxW("Please fill out Current Password");
                MessageBoxW("Silahkan isi password saat ini");
                return;
            }

            if (!fn_ChkPass(txtNikConfirm.Text.ToString(), teCurrentPass.EditValue.ToString()))
            {
                //MessageBoxW("Not correct User ID and Password");
                MessageBoxW("User ID dan Password Salah");
                return;
            }
            else
            {
                if (teCurrentPass.EditValue.ToString() == "1234")
                {
                    //MessageBoxW("Your current password is default. Please change the password");
                    MessageBoxW("Password Anda saat ini adalah default. Silahkan ganti Password dahulu");
                    teNewPass.Visible = true;
                    teNewPass.EditValue = null;
                    teCfmNewPass.Visible = true;
                    teCfmNewPass.EditValue = null;
                    BtnReset.Text = "Save";
                    this.ActiveControl = teNewPass;
                    return;
                }
            }

            pbProgressShow();

            try
            {
                string p_lost_time = chkLostTime.CheckState == CheckState.Checked ? "Y" : "N";
                //string p_param1, p_param2, p_param3, p_param4, p_param5;
                fn_Save("CONFIRM", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), lblOPCD.Text, txtCmmsNo.Text, selectedDivision, lblReqYMD.Text, lblReqHMS.Text, txtNikRequest.Text, lblEmpNameRequest.Text, txtCondition.Text, lblReqYMD.Text, lblWoSeq.Text, /*lblWoNoOld.Text*/ lblWoNo.Text, lblMCLine.Text, lblMCNo.Text, "F", "", lblChkYMD.Text, lblChkHMS.Text, cbNikCheck.Text, lblEmpNameCheck.Text, txtChkMsg.Text, /*cbMCID.Text*/ lookEMC_ID.EditValue.ToString(), "", lblDefectiveCD.Text, p_lost_time, "", "", "", "", "", lblRepairYMD.Text, lblRepairHMS.Text, "", "", lblSolutionCD.Text, "", txtNikConfirm.Text, lblEmpNameConfirm.Text);
                pnlPopUp.Visible = false;
                #region [COMMENT]
                //if (cboArea.EditValue.ToString() == "OSP" && cboLocation.EditValue.ToString() == "51BT")
                //    fn_Search("GET_LAYOUT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), "OSP");
                //else
                //    fn_Search("GET_LAYOUT_PLANT", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), cboArea.EditValue.ToString());
                #endregion
            }
            catch(Exception ex)
            {
                MessageBoxW("btnConfirm_Click() " + ex.ToString());
            }
            finally
            {
                pbSetProgressHide();
                QueryClick();
            }
        }

        private void txtNikRequest_TextChanged(object sender, EventArgs e)
        {
            string nik = txtNikRequest.Text; //cbNikRequest.SelectedItem.ToString();
            var name = dtWorkshop.AsEnumerable().Where(row => row.Field<string>("CODE") == nik).Select(row => new { DESCRIPTION = row["DESCRIPTION"] });

            if (name.Any())
                lblEmpNameRequest.Text = ConvertToDataTable(name).Rows[0][0].ToString();
            else
                lblEmpNameRequest.Text = "Not Found";
        }

        private void txtNikConfirm_TextChanged(object sender, EventArgs e)
        {
            string nik = txtNikConfirm.Text;
            var name = dtWorkshop.AsEnumerable().Where(row => row.Field<string>("CODE") == nik).Select(row => new { DESCRIPTION = row["DESCRIPTION"] });

            if (name.Any())
                lblEmpNameConfirm.Text = ConvertToDataTable(name).Rows[0][0].ToString();
            else
                lblEmpNameConfirm.Text = "Not Found";
        }

        private void btnMEPAdd_Click(object sender, EventArgs e)
        {
            #region [COMMENT]
            //dtMEP_Repair = grdMEP.DataSource as DataTable;
            //SetData(grdMEP, dtMEP_Repair);

            //gvwMechanic.AddNewRow();
            //gvwMechanic.SetRowCellValue(gvwMechanic.FocusedRowHandle, "CODE", "10090052");
            //dtMEP_Repair = grdMEP.DataSource as DataTable;
            #endregion
            bool chk = false;
            if (gvwMechanic.RowCount >= 1)
            {
                //dtMEP_Repair.Rows.Add(new object[] { null, "Not Found" });
                //SetData(grdMEP, dtMEP_Repair);
                for (int i = 0; i < gvwMechanic.RowCount; i++)
                {
                    if (gvwMechanic.GetRowCellValue(i, "CODE") != null && gvwMechanic.GetRowCellValue(i, "CODE").ToString() != "")
                        chk = true;
                    else
                        chk = false;
                }

                if (chk)
                {
                    gvwMechanic.AddNewRow();
                    gvwMechanic.SetRowCellValue(gvwMechanic.RowCount-1, gvwMechanic.Columns["CODE"], riComboBox.Items[0]);
                }

                #region [COMMENT]
                //int i = 0;
                //foreach (DataRow row in dtMEP_Repair.Rows)
                //{
                //    if (row["CODE"].ToString() == "")
                //    {
                //        return;
                //    }
                //    else
                //        gvwMechanic.AddNewRow();

                //    i++;
                //}
                #endregion
            }
            else
            {
                gvwMechanic.AddNewRow();
                gvwMechanic.SetRowCellValue(gvwMechanic.RowCount - 1, gvwMechanic.Columns["CODE"], riComboBox.Items[0]);
                //dtMEP_Repair = grdMEP.DataSource as DataTable;
                //SetData(grdMEP, dtMEP_Repair);
            }
        }

        private void btnCallYes_Click(object sender, EventArgs e)
        {
            switch (rbDivision.SelectedIndex)
            {
                case 0:
                    selectedDivision = rbDivision.EditValue.ToString();
                    break;
                case 1:
                    selectedDivision = rgQuality.EditValue.ToString();
                    break;
            }

            fn_Save("CALL_TL_QC", cboPlant.EditValue.ToString(), cboLocation.EditValue.ToString(), selectedOP_CD, "", selectedDivision, "", "", "", "", "", "", "", "", mc_line, mc_no);
            pnlCallTLQC.Enabled = false;
            pnlCallTLQC.Visible = false;
            rgQuality.SelectedIndex = 0;
            pnlCallTLQC.SendToBack();
        }

        private void btnCallNo_Click(object sender, EventArgs e)
        {
            pnlCallTLQC.Enabled = false;
            pnlCallTLQC.Visible = false;
            rgQuality.SelectedIndex = 0;
            pnlCallTLQC.SendToBack();
        }

        private void tableLayoutPanel56_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMEPDelete_Click(object sender, EventArgs e)
        {
            if (dtMEP_Repair.Rows.Count > 0)
            {
                dtMEP_Repair.Rows.RemoveAt(gvwMechanic.FocusedRowHandle);
                SetData(grdMEP, dtMEP_Repair);
            }
        }

        private void riComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit cbe = (ComboBoxEdit)sender;
            string nik = cbe.SelectedItem.ToString();
            var name = dtMEP.AsEnumerable().Where(row => row.Field<string>("CODE") == nik).Select(row => new { DESCRIPTION = row["DESCRIPTION"] });

            if (name.Any())
                gvwMechanic.SetRowCellValue(gvwMechanic.FocusedRowHandle, "DESCRIPTION", ConvertToDataTable(name).Rows[0][0].ToString());
            else
                gvwMechanic.SetRowCellValue(gvwMechanic.FocusedRowHandle, "DESCRIPTION", "Not Found");
        }

        private void riComboBox_Validating(object sender, CancelEventArgs e)
        {
            ComboBoxEdit editor = sender as ComboBoxEdit;
            if (editor.EditValue.ToString() == "" || !Regex.IsMatch(editor.EditValue.ToString(), @"^\d+$"))
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void gvwSparePart_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            #region [COMMENT]
            //ColumnView view = sender as ColumnView;
            //if (e.Value.ToString() == "")
            //    e.Valid = false;
            //else
            //    e.Valid = true;
            ////GridColumn column = (e as EditFormValidateEditorEventArgs)?.Column ?? view.FocusedColumn;
            ////if (column.Name != "colBudget") return;
            ////if ((Convert.ToInt32(e.Value) < 0) || (Convert.ToInt32(e.Value) > 1000000))
            ////    e.Valid = false;
            #endregion
        }

        private void gvwMechanic_LostFocus(object sender, EventArgs e)
        {
            #region [COMMENT]
            //if (gvwMechanic.RowCount > 0)
            //{
            //    int i = 0;
            //    foreach (DataRow row in dtMEP_Repair.Rows)
            //    {
            //        if (row["CODE"].ToString() == "")
            //        {
            //            gvwMechanic.DeleteRow(i);
            //        }

            //        i++;
            //    }
            //}
            #endregion
        }

        private void gvwMechanic_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            #region [COMMENT]
            //GridView view = sender as GridView;

            //if (view.Columns["CODE"].ToString() == "") e.Valid = false;
            #endregion
        }

        private void riComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            ComboBoxEdit cbe = (ComboBoxEdit)sender;
            string nik = cbe.Text.ToString();
            var name = dtMEP.AsEnumerable().Where(row => row.Field<string>("CODE") == nik).Select(row => new { DESCRIPTION = row["DESCRIPTION"] });

            if (name.Any())
                gvwMechanic.SetRowCellValue(gvwMechanic.FocusedRowHandle, "DESCRIPTION", ConvertToDataTable(name).Rows[0][0].ToString());
            else
                gvwMechanic.SetRowCellValue(gvwMechanic.FocusedRowHandle, "DESCRIPTION", "Not Found");
        }

        private void btnReqSpart_Click(object sender, EventArgs e)
        {
            pbProgressShow();

            pnlReqSpart.Location = new Point(
                                            this.ClientSize.Width / 2 - pnlReqSpart.Size.Width / 2,
                                            this.ClientSize.Height / 2 - pnlReqSpart.Size.Height / 2);

            button1.Enabled = false;
            pnlReqSpart.Enabled = true;
            pnlReqSpart.Visible = true;
            pnlReqSpart.BringToFront();

            try
            {
                fn_Search("GET_SPART", cboPlant.EditValue.ToString(), /*lblWoNoOld.Text.ToString()*/ lblWoNo.Text.ToString());
            }
            catch (Exception ex)
            {
                MessageBoxW("btnReqSpart_Click() " + ex.ToString());
            }
            finally
            {
                pbSetProgressHide();
            }

        }

        private void btnCloseReqSpart_Click(object sender, EventArgs e)
        {
            fn_clearAll("SPART");
        }

        private void btnAddReqSpart_Click(object sender, EventArgs e)
        {
            #region [COMMENT]
            ////bool chk = false;
            ////if (gvwReqSpart.RowCount >= 1)
            ////{

            ////}
            ////else
            ////{
            //    gvwReqSpart.AddNewRow();
            //    //gvwReqSpart.SetRowCellValue(gvwReqSpart.RowCount - 1, gvwReqSpart.Columns["CODE"], riComboBox.Items[0]);
            //////}
            #endregion
        }

        private void btnDeleteReqSpart_Click(object sender, EventArgs e)
        {
            #region [COMMENT]
            //DataTable dtSrc = grdReqSpart.DataSource as DataTable;

            //if (dtSrc.Rows.Count > 0)
            //{
            //    dtSrc.Rows.RemoveAt(gvwReqSpart.FocusedRowHandle);
            //    SetData(grdReqSpart, dtSrc);
            //}
            #endregion
        }

        private void riSpart_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit lookup = sender as LookUpEdit;
            DataRowView dataRow = lookup.GetSelectedDataRow() as DataRowView;
            if (dataRow != null)
            {
                gvwReqSpart.SetRowCellValue(gvwReqSpart.FocusedRowHandle, gvwReqSpart.Columns["PART_CD"], dataRow["PART_CODE"].ToString());
                gvwReqSpart.SetRowCellValue(gvwReqSpart.FocusedRowHandle, gvwReqSpart.Columns["PART_NAME"], dataRow["PART_NAME"].ToString());
                gvwReqSpart.SetRowCellValue(gvwReqSpart.FocusedRowHandle, gvwReqSpart.Columns["UNIT"], dataRow["UNIT"].ToString());
                gvwReqSpart.SetRowCellValue(gvwReqSpart.FocusedRowHandle, gvwReqSpart.Columns["SPEC"], dataRow["SPEC"].ToString());
            }
        }

        private void gvwReqSpart_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["REQ_QTY"], 1);
        }

        private void btnSaveReqSpart_Click(object sender, EventArgs e)
        {
            pbProgressShow();

            DataTable dtSrc = grdReqSpart.DataSource as DataTable;

            string req_date = "";
            string part_cd = "";
            string req_qty = "";
            string ro_date = "";
            string po_date = "";
            string etd_date = "";
            string in_date = "";
            string in_qty = "";

            foreach (DataRow row in dtSrc.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["PART_CD"].ToString() != "")
                {
                    req_date += row["REQ_YMD"] + "|";
                    part_cd += row["PART_CD"] + "|";
                    req_qty += row["REQ_QTY"] + "|";
                    ro_date += (row["RO_DATE"].ToString() != "" ? ((DateTime)row["RO_DATE"]).ToString("yyyyMMdd") : "") + "|";
                    po_date += (row["PO_DATE"].ToString() != "" ? ((DateTime)row["PO_DATE"]).ToString("yyyyMMdd") : "") + "|";
                    etd_date += (row["ETD"].ToString() != "" ? ((DateTime)row["ETD"]).ToString("yyyyMMdd") : "") + "|";
                    in_date += (row["IN_DATE"].ToString() != "" ? ((DateTime)row["IN_DATE"]).ToString("yyyyMMdd") : "") + "|";
                    in_qty += row["IN_QTY"] + "|";
                }
            }

            if (req_date.Length > 0)
                req_date = req_date.Remove(req_date.Length - 1, 1);

            if (part_cd.Length > 0)
                part_cd = part_cd.Remove(part_cd.Length - 1, 1);

            if (req_qty.Length > 0)
                req_qty = req_qty.Remove(req_qty.Length - 1, 1);

            if (ro_date.Length > 0)
                ro_date = ro_date.Remove(ro_date.Length - 1, 1);

            if (po_date.Length > 0)
                po_date = po_date.Remove(po_date.Length - 1, 1);

            if (etd_date.Length > 0)
                etd_date = etd_date.Remove(etd_date.Length - 1, 1);

            if (in_date.Length > 0)
                in_date = in_date.Remove(in_date.Length - 1, 1);

            if (in_qty.Length > 0)
                in_qty = in_qty.Remove(in_qty.Length - 1, 1);

            try
            {
                fn_Save("SAVE_REQ_SPART", cboPlant.EditValue.ToString(), "", "", "", "", req_date, "", "", "", "", ro_date, "", /*lblWoNoOld.Text.ToString()*/ lblWoNo.Text.ToString(), req_qty, in_qty, "", "", po_date, "", "", "", "", "", "", "", etd_date, in_date, "", "", "", part_cd);
                MessageBoxW("Success Saved !");
            }
            catch(Exception ex)
            {
                MessageBoxW("Error Saved !" + ex.ToString());
            }
            finally
            {
                fn_clearAll("SPART");
                pbSetProgressHide();
            }

        }

        private void rbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rbDivision.SelectedIndex)
            {
                case 0:
                    grpQC.Enabled = false;
                    grpQC.Visible = false;
                    //selectedDivision = rbDivision.EditValue.ToString();
                    break;

                case 1:
                    rgQuality.SelectedIndex = 0;
                    grpQC.Enabled = true;
                    grpQC.Visible = true;
                    //selectedDivision = rgQuality.EditValue.ToString();
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            #region [COMMENT]
            //using (GMES0211_1 bd = new GMES0211_1())
            //{
            //    var res = bd.ShowDialog();
            //    if (res == DialogResult.OK)
            //    {
                    
            //    }
            //}
            #endregion
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (BtnReset.Text.ToUpper() == "CHANGE PASSWORD")
            {
                BtnReset.Text = "Save";
                teNewPass.Visible = true;
                teNewPass.EditValue = null;
                teCfmNewPass.Visible = true;
                teCfmNewPass.EditValue = null;
            }
            else
                if (BtnReset.Text.ToUpper() == "SAVE")
                {

                    #region USER CONTROL VALIDATION
                    if (txtNikConfirm.Text == "" || txtNikConfirm.Text == " ")
                    {
                        //MessageBoxW("Please check Employee ID");
                        MessageBoxW("Silahkan periksa kolom NIK");
                        this.ActiveControl = txtNikConfirm;
                        return;
                    }

                    if (teCurrentPass.EditValue == null || teCurrentPass.EditValue.ToString() == "" || teCurrentPass.EditValue.ToString() == " ")
                    {
                        //MessageBoxW("Please check Current Password");
                        MessageBoxW("Silahkan periksa Password saat ini");
                        this.ActiveControl = teCurrentPass;
                        return;
                    }

                    if (teNewPass.EditValue == null || teNewPass.EditValue.ToString() == "" || teNewPass.EditValue.ToString() == " ")
                    {
                        //MessageBoxW("Please check New Password");
                        MessageBoxW("Silahkan periksa Password Baru");
                        this.ActiveControl = teNewPass;
                        return;
                    }

                    if (teCfmNewPass.EditValue == null || teCfmNewPass.EditValue.ToString() == "" || teCfmNewPass.EditValue.ToString() == " ")
                    {
                        //MessageBoxW("Please chek Confirm New Password");
                        MessageBoxW("SIlahkan periksa Password Konfirmasi");
                        this.ActiveControl = teCfmNewPass;
                        return;
                    }

                    if (teNewPass.EditValue.ToString() != "" && teNewPass.EditValue.ToString() != " "
                        && teCfmNewPass.EditValue.ToString() != "" && teCfmNewPass.EditValue.ToString() != " "
                        && teNewPass.EditValue.ToString() != teCfmNewPass.EditValue.ToString()
                       )
                    {
                        //MessageBoxW("New Password & Confirm New Password are not same");
                        MessageBoxW("Password Baru dan Password Konfirmasi tidak sama");
                        this.ActiveControl = teCfmNewPass;
                        return;
                    }
                    #endregion

                    pbProgressShow();

                    if (!fn_ChkPass(txtNikConfirm.Text.ToString(), teCurrentPass.EditValue.ToString()))
                    {
                        //MessageBoxW("Not correct User ID and Password");
                        MessageBoxW("User ID dan Password Salah");
                        pbSetProgressHide();
                        return;
                    }

                    //pbProgressShow();

                    fn_SavePass(txtNikConfirm.Text, teCurrentPass.EditValue.ToString(), teNewPass.EditValue.ToString(), SessionInfo.UserID, GetLocalIPAddress());

                    teNewPass.Visible = false;
                    teNewPass.EditValue = null;
                    teCfmNewPass.Visible = false;
                    teCfmNewPass.EditValue = null;
                    BtnReset.Text = "Change Password";

                    pbSetProgressHide();
                }
        }

        private void lookEMC_ID_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit lookup = sender as LookUpEdit;
            DataRowView dataRow = lookup.GetSelectedDataRow() as DataRowView;
            if (dataRow != null)
                lblMCDesc.Text = dataRow["Description"].ToString();
            else
                lblMCDesc.Text = "Not Found";
        }

        private void labelControl23_Click(object sender, EventArgs e)
        {
        }

        private void ShowToolTipAtCell(int rowIndex, int colIndex)
        {
            try
            {
                if (rowIndex >= 0 && rowIndex < gvwData.RowCount && colIndex >= 0 && colIndex < gvwData.Columns.Count)
                {
                    if (customToolTip != null)
                    {
                        this.Controls.Remove(customToolTip);
                        customToolTip.Dispose();
                    }

                    customToolTip = new Label();
                    customToolTip.Text = "M/C Izmi =>";
                    customToolTip.Font = new Font("Calibri", 11, FontStyle.Bold);
                    customToolTip.BackColor = Color.LightYellow;
                    customToolTip.BorderStyle = BorderStyle.FixedSingle;
                    customToolTip.AutoSize = true;
                    customToolTip.Padding = new Padding(5);

                    #region [OLD]
                    ////int sW = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 550;
                    ////int sH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 300;
                    ////customToolTip.Location = new Point(sW, sH);

                    //int gW = grdCtrl.Width;
                    //int gH = grdCtrl.Height;

                    //double cX = gW / 68;
                    //double cY = gH / 18;

                    //int rcX = (int)Math.Ceiling(cX);
                    //int rcY = (int)Math.Ceiling(cY);

                    //if (gW < 1900)
                    //{
                    //    customToolTip.Location = new Point(rcX * 51, rcY * 17);
                    //}
                    //else
                    //{
                    //    customToolTip.Location = new Point(rcX * 51, rcY * 16);
                    //}
                    #endregion [OLD]

                    int left = 0;
                    int top = 0;
                    int x = 0;
                    int y = 0;

                    #region [OLD V1]
                    //int grdSH = 0;
                    //for (int i = 0; i < gvwData.RowCount; i++)
                    //{
                    //    grdSH += gvwData.RowHeight;
                    //}

                    //int x = 0;
                    //int y = 0;

                    //if (grdSH < 612)
                    //{
                    //    x = 52;
                    //    y = 18;
                    //}
                    //else
                    //{
                    //    x = 52;
                    //    y = 17;
                    //}

                    //for (int i = 0; i < x; i++)
                    //{
                    //    cellW += gvwData.Columns[i].Width;
                    //}

                    //for (int i = 0; i < y; i++)
                    //{
                    //    cellH += gvwData.RowHeight;
                    //}
                    #endregion [OLD V1]

                    int scrnHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

                    if (scrnHeight <= 768)
                    {
                        x = 50;
                        y = 16;
                    }
                    else
                    {
                        x = 50;
                        y = 15;
                    }

                    for (int i = 0; i < x; i++)
                    {
                        left = left + gvwData.Columns[i].Width;
                    }

                    for (int i = 0; i < y; i++)
                    {
                        top = top + gvwData.RowHeight;
                    }


                    customToolTip.Location = new Point(left, top + panelEx1.Height -20);

                    this.Controls.Add(customToolTip);
                    customToolTip.BringToFront();

                }
            }
            catch (Exception ex)
            {
                MessageBoxW("Error: ShowToolTipAtCell " + ex.Message);
            }
        }

        private void lblOsr_3_MEP_zone3_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblOsr_3_MEP_zone4_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void lblOsr_3_MEP_zone5_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
                MEPClick(sender);
            else
                if (me.Button == MouseButtons.Right)
                    showPopUp_CallTLQC("", "", "", sender);
        }

        private void chkLostTime_Click(object sender, EventArgs e)
        {
            #region [COMMENT]
            //if (chkLostTime.CheckState == CheckState.Checked)
            //{

            //}
            #endregion
        }

        

    }
}