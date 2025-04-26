using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid;
using JPlatform.Client.Controls6;

namespace CSI.MES.P
{
    public class GroupColumn
    {
        public string ColumnName;
        public DevExpress.Data.ColumnSortOrder ColumnSortOrder = ((DevExpress.Data.ColumnSortOrder)DevExpress.Data.ColumnSortOrder.Ascending);
    }

    public class GroupColumnCollection : ObservableCollection<GroupColumn>
    {
        public bool AutoExpandAllGroups = false;
        public bool ShowGroupPanel = false;
        public bool ShowGroups = false;
        public bool ShowGroupedColumns = false;


        public void SetGroupingColumn(GridViewEx gridView, GroupColumnCollection rGroupColumnCollection)
        {
            gridView.BeginUpdate();
            gridView.ClearGrouping();
            gridView.OptionsBehavior.AutoExpandAllGroups = rGroupColumnCollection.AutoExpandAllGroups;
            gridView.OptionsView.ShowGroupPanel = rGroupColumnCollection.ShowGroupPanel;
            gridView.OptionsView.ShowGroupedColumns = rGroupColumnCollection.ShowGroupedColumns;

            #region 예제
            //gridView.SortInfo.ClearAndAddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            //    new DevExpress.XtraGrid.Columns.GridColumnSortInfo(gridView.Columns["Country"], DevExpress.Data.ColumnSortOrder.Ascending), 
            //    new DevExpress.XtraGrid.Columns.GridColumnSortInfo(gridView.Columns["City"], DevExpress.Data.ColumnSortOrder.Descending),
            //    new DevExpress.XtraGrid.Columns.GridColumnSortInfo(gridView.Columns["CompanyName"], DevExpress.Data.ColumnSortOrder.Ascending)
            //}, 2);
            #endregion 예제

            foreach (GroupColumn column in rGroupColumnCollection)
            {
                gridView.SortInfo.Add(new DevExpress.XtraGrid.Columns.GridColumnSortInfo(gridView.Columns[column.ColumnName], column.ColumnSortOrder));

                #region OPTION 1 - GROUPING Show 설정

                // OPTION 1
                //  - 목록에 GROUP 설정
                if (rGroupColumnCollection.ShowGroups)
                    gridView.Columns[column.ColumnName].Group();
                else
                    gridView.Columns[column.ColumnName].UnGroup();

                #endregion OPTION 1 - GROUPING Show 설정
            }
            gridView.EndUpdate();
        }
    }

    public class SummaryColumnCollection : ObservableCollection<SummaryColumn>
    {
        public GroupFooterShowMode ShowGroupFooter = GroupFooterShowMode.Hidden;
        public bool ShowFooter = false;

        public void SetSummaryColumn(GridViewEx gridView, SummaryColumnCollection rSummaryColumnCollection)
        {
            DevExpress.XtraGrid.GridGroupSummaryItem item = null;
            gridView.GroupSummary.BeginUpdate();
            foreach (SummaryColumn column in rSummaryColumnCollection)
            {
                //item = new DevExpress.XtraGrid.GridSummaryItem();
                item = new DevExpress.XtraGrid.GridGroupSummaryItem();
                item.SummaryType = column.SummaryType;
                item.FieldName = column.FieldName;
                item.DisplayFormat = column.DisplayFormat;
                if (column.Tag != null)
                {
                    item.Tag = column.Tag;
                }

                item.ShowInGroupColumnFooter = gridView.Columns[column.FieldName];

                gridView.GroupSummary.Add(item);

                gridView.Columns[column.FieldName.Replace("col", "")].SummaryItem.SummaryType = column.SummaryType;
                gridView.Columns[column.FieldName.Replace("col", "")].SummaryItem.DisplayFormat = column.DisplayFormat;


                #region 예제
                //DevExpress.XtraGrid.GridGroupSummaryItem item = new DevExpress.XtraGrid.GridGroupSummaryItem();
                //item.FieldName = "DISP_VAL6";
                //item.SummaryType = DevExpress.Data.SummaryItemType.Count;
                //csbandedGridView1.GroupSummary.Add(item);

                //DevExpress.XtraGrid.GridGroupSummaryItem item1 = new DevExpress.XtraGrid.GridGroupSummaryItem();
                //item1.FieldName = "DISP_VAL7";
                //item1.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //item1.DisplayFormat = "Total {0:c2}";
                //item1.ShowInGroupColumnFooter = csbandedGridView1.Columns[7];
                //csbandedGridView1.GroupSummary.Add(item1);
                #endregion 예제

            }
            if (rSummaryColumnCollection.Count > 0)
            {
                // Custom Summary Event
                //gridView.CustomSummaryCalculate += new CustomSummaryEventHandler(gridView_CustomSummaryCalculate);

                #region OPTION 2 - Group Footer Display

                // OPTION 2
                //  - Group Footer Display
                gridView.OptionsView.GroupFooterShowMode = ShowGroupFooter;

                #endregion OPTION 2 - Group Footer Display

                #region OPTION 3 - Footer Display

                // OPTION 3
                //  - footer Display
                gridView.OptionsView.ShowFooter = ShowFooter;

                #endregion OPTION 3 - Footer Display

                //csbandedGridView1.OptionsView.GroupDrawMode = GroupDrawMode.Default;
                //csbandedGridView1.OptionsView.ShowChildrenInGroupPanel = true;
                //csbandedGridView1.OptionsView.ShowGroupExpandCollapseButtons = true;
                //csbandedGridView1.OptionsView.ShowGroupPanel = true;
                //gridView.UpdateGroupSummary();
            }
            gridView.GroupSummary.EndUpdate();
        }

        void gridView_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public class SummaryColumn
    {
        public string FieldName;
        public string DisplayFormat;
        public DevExpress.Data.SummaryItemType SummaryType;
        public object Tag;

        public SummaryColumn()
        {
            this.Tag = null;
        }

        public SummaryColumn(DevExpress.Data.SummaryItemType rSummaryItemType, string rColumnName, string rDisplayFormat)
        {
            this.Tag = null;
            this.SummaryType = rSummaryItemType;
            this.FieldName = rColumnName;
            this.DisplayFormat = rDisplayFormat;

        }
    }
}
