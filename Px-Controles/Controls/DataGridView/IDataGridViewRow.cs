﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Px_Controles.Controls.DataGridView
{
    /// <summary>
    /// Interface IDataGridViewRow
    /// </summary>
    public interface IDataGridViewRow
    {
        /// <summary>
        /// Occurs when [row custom event].
        /// </summary>
        event DataGridViewRowCustomEventHandler RowCustomEvent;
        /// <summary>
        /// CheckBox选中事件
        /// </summary>
        event DataGridViewEventHandler CheckBoxChangeEvent;
        /// <summary>
        /// 点击单元格事件
        /// </summary>
        event DataGridViewEventHandler CellClick;
        /// <summary>
        /// 数据源改变事件
        /// </summary>
        event DataGridViewEventHandler SourceChanged;
        /// <summary>
        /// 列参数，用于创建列数和宽度
        /// </summary>
        /// <value>The columns.</value>
        List<DataGridViewColumnEntity> Columns { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is show CheckBox.
        /// </summary>
        /// <value><c>true</c> if this instance is show CheckBox; otherwise, <c>false</c>.</value>
        bool IsShowCheckBox { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        bool IsChecked { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        /// <value>The data source.</value>
        object DataSource { get; set; }
        /// <summary>
        /// 添加单元格元素，仅做添加控件操作，不做数据绑定，数据绑定使用BindingCells
        /// </summary>
        void ReloadCells();
        /// <summary>
        /// 绑定数据到Cell
        /// </summary>
        /// <returns>返回true则表示已处理过，否则将进行默认绑定（通常只针对有Text值的控件）</returns>
        void BindingCellData();
        /// <summary>
        /// 设置选中状态，通常为设置颜色即可
        /// </summary>
        /// <param name="blnSelected">是否选中</param>
        void SetSelect(bool blnSelected);
        /// <summary>
        /// 行高
        /// </summary>
        /// <value>The height of the row.</value>
        int RowHeight { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        /// <value>The Index of the row.</value>
        int RowIndex { get; set; }
    }
}
