﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using JetBrains.Annotations;
using MHR_Editor.Common;
using MHR_Editor.Common.Attributes;
using MHR_Editor.Common.Controls.Models;
using MHR_Editor.Common.Data;
using MHR_Editor.Common.Models;
using MHR_Editor.Windows;

namespace MHR_Editor.Controls;

public interface IAutoDataGrid {
    bool IsAddingAllowed { get; set; }
    void SetItems(object items, PropertyInfo sourceProperty = null);
    void Refresh();
}

public interface IAutoDataGrid<T> : IAutoDataGrid {
    ObservableCollection<T> Items { get; }

    void SetItems(ObservableCollection<T> items, PropertyInfo sourceProperty = null);
}

public abstract partial class AutoDataGrid : IAutoDataGrid {
    protected static readonly Brush BACKGROUND_BRUSH = (Brush) new BrushConverter().ConvertFrom("#c0e1fb");

    public abstract bool IsAddingAllowed { get; set; }

    protected AutoDataGrid() {
        InitializeComponent();
    }

    public abstract    void SetItems(object items, PropertyInfo sourceProperty = null);
    public abstract    void Refresh();
    public abstract    void RefreshHeaderText();
    protected abstract void On_AutoGeneratedColumns(object sender, EventArgs                             e);
    protected abstract void On_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e);
    protected abstract void On_CellEditEnding(object       sender, DataGridCellEditEndingEventArgs       e);
    protected abstract void On_GotFocus(object             sender, RoutedEventArgs                       e);
    protected abstract void On_Sorting(object              sender, DataGridSortingEventArgs              e);
    protected abstract void On_Cell_MouseClick(object      sender, MouseButtonEventArgs                  e);
}

public class AutoDataGridGeneric<T> : AutoDataGrid, IAutoDataGrid<T> {
    private             Dictionary<string, ColumnHolder> columnMap;
    private             GroupFilter                      groupFilter;
    [CanBeNull] private DataGridRow                      coloredRow;
    private             bool                             isManualEditCommit;
    private             bool                             shadeThisColumn;
    private             PropertyInfo                     sourceProperty;

    private ObservableCollection<T> items;
    public new ObservableCollection<T> Items {
        get => items;
        private set {
            columnMap  = new();
            coloredRow = null;
            items      = value;

            if (value == null) {
                ItemsSource = null;
                return;
            }

            groupFilter = new();
            ItemsSource = new ListCollectionView(items);

            // Auto-Generation will have setup a filter for each visible column.
            // This will set the merged check on all those filters so all we have to do layer is update the filter text, then refresh.
            ((ListCollectionView) ItemsSource).Filter = groupFilter.MergedFilters;
        }
    }

    public override bool IsAddingAllowed {
        get => CanUserAddRows || CanUserDeleteRows;
        set => CanUserAddRows = CanUserDeleteRows = value;
    }

    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public override void SetItems(object items, PropertyInfo sourceProperty = null) {
        SetItems((ObservableCollection<T>) items, sourceProperty);
    }

    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public void SetItems(ObservableCollection<T> items, PropertyInfo sourceProperty = null) {
        this.sourceProperty = sourceProperty;
        Items               = items;
    }

    public override void Refresh() {
        ((ListCollectionView) ItemsSource).Refresh();
    }

    public override void RefreshHeaderText() {
        foreach (var column in columnMap.Values) {
            column.headerInfo.OnPropertyChanged(nameof(HeaderInfo.OriginalText));
        }
    }

    protected override void On_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
        Type sourceClassType = ((dynamic) e.PropertyDescriptor).ComponentType;

        if (typeof(T).GetProperties().Any(prop => prop.Name == $"{e.PropertyName}_button")) {
            e.Cancel = true;
        }

        // TODO: Cancel for _button columns as we will use a text version with onClick opening a selector.
        if (e.Cancel) return;

        // Create 'X' button for delete column.
        if (e.PropertyName == "Delete") {
            var col  = new DataGridTemplateColumn();
            var btn1 = new FrameworkElementFactory(typeof(Button));

            btn1.SetValue(ContentControl.ContentProperty, "X");
            btn1.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(On_Cell_Delete_Click));

            col.CellTemplate = new() {VisualTree = btn1};
            e.Column         = col;
        }

        if (e.PropertyType == typeof(DateTime)) {
            e.Column = new DataGridTextColumn {
                Header = e.Column.Header,
                Binding = new Binding(e.PropertyName) {
                    StringFormat = "{0:yyyy-MM-dd}" // Can't be negative, but needed to hide all 0 cases.
                },
                CanUserSort = true,
                IsReadOnly  = true
            };
        }

        if (e.PropertyName == "Index") {
            e.Column.IsReadOnly = true; // Do before normal readOnly checks.
        }

        e.Column.CanUserSort = true;

        // Use [DisplayName] attribute for the column header text.
        // Use [SortOrder] attribute to control the position. Generated fields have spacing so it's easy to say 'generated_field_sortOrder + 1'.
        // Use [CustomSorter] to define an IComparer class to control sorting.
        var           propertyInfo     = sourceClassType.GetProperties().FirstOrDefault(info => info.Name == e.PropertyName);
        var           displayName      = ((DisplayNameAttribute) propertyInfo?.GetCustomAttribute(typeof(DisplayNameAttribute), true))?.DisplayName;
        var           sortOrder        = ((SortOrderAttribute) propertyInfo?.GetCustomAttribute(typeof(SortOrderAttribute), true))?.sortOrder;
        var           customSorterType = ((CustomSorterAttribute) propertyInfo?.GetCustomAttribute(typeof(CustomSorterAttribute), true))?.customSorterType;
        var           isList           = (IsListAttribute) propertyInfo?.GetCustomAttribute(typeof(IsListAttribute), true) != null;
        var           showAsHex        = (ShowAsHexAttribute) propertyInfo?.GetCustomAttribute(typeof(ShowAsHexAttribute), true) != null;
        ICustomSorter customSorter     = null;

        if (displayName != null) {
            if (displayName == "") { // Use empty DisplayName as a way to hide columns.
                e.Cancel = true;
                return;
            }

            if (displayName.StartsWith("-") && displayName.Length > 1) displayName = displayName[1..];

            e.Column.Header = displayName;
        }

        if (isList || e.PropertyType.IsGenericType && e.PropertyType.GetGenericTypeDefinition().Is(typeof(ObservableCollection<>))) {
            var col  = new DataGridTemplateColumn();
            var btn1 = new FrameworkElementFactory(typeof(Button));

            btn1.SetValue(ContentControl.ContentProperty, "Open");
            btn1.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler((o, args) => On_Cell_Open_Click(o, args, propertyInfo, displayName ?? propertyInfo?.Name)));

            col.CellTemplate = new() {VisualTree = btn1};
            e.Column         = col;
            e.Column.Header  = displayName ?? propertyInfo?.Name;
        }

        if (customSorterType != null) {
            customSorter = (ICustomSorter) Activator.CreateInstance(customSorterType);
            if (customSorter is ICustomSorterWithPropertyName csWithName) {
                csWithName.PropertyName = e.PropertyName;
            }
        }

        if (showAsHex) {
            ((DataGridTextColumn) e.Column).Binding.StringFormat = "0x{0:X}";
        }

        var headerInfo = new HeaderInfo((string) e.Column.Header!, e.PropertyName);
        e.Column.HeaderTemplate = CreateHeader(headerInfo);

        columnMap[e.PropertyName] = new(e.Column, sortOrder ?? -1, headerInfo, customSorter);

        // TODO: Fix enum value display at some point.
    }

    private static DataTemplate CreateHeader(HeaderInfo headerInfo) {
        var headerFilter = new FrameworkElementFactory(typeof(HeaderFilter));

        headerFilter.SetValue(HeaderFilter.HeaderInfoProperty, headerInfo);
        headerFilter.AddHandler(HeaderFilter.OnFilterChangedEvent, new RoutedEventHandler(On_FilterChanged));

        return (new() {VisualTree = headerFilter});
    }

    protected override void On_AutoGeneratedColumns(object sender, EventArgs e) {
        try {
            var columns = columnMap.Values.ToList();
            columns.Sort((c1, c2) => c1.sortOrder.CompareTo(c2.sortOrder));
            for (var i = 0; i < columns.Count; i++) {
                var column = columns[i].column;
                column.DisplayIndex = i;

                if (shadeThisColumn) {
                    column.CellStyle = new(typeof(DataGridCell));
                    column.CellStyle.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrush(Color.FromRgb(230, 230, 230))));
                    shadeThisColumn = !shadeThisColumn;
                } else {
                    shadeThisColumn = !shadeThisColumn;
                }
            }

            // Since T may be dynamic/object. Also need to account for custom views.
            var outType = typeof(T);

            if (outType == typeof(object)) {
                outType = ((dynamic) items)[0].GetType();
            }

            foreach (var propertyName in columnMap.Keys) {
                groupFilter.AddFilter(outType.GetProperty(propertyName)!);
            }
        } catch (Exception err) when (!Debugger.IsAttached) {
            MainWindow.ShowError(err, "Error Occurred");
        }
    }

    private static void On_FilterChanged(object sender, RoutedEventArgs e) {
        try {
            var headerFilter = (HeaderFilter) e.OriginalSource;
            var headerInfo   = headerFilter.HeaderInfo;
            var grid         = ((DependencyObject) e.OriginalSource).GetParent<AutoDataGridGeneric<T>>();
            var listColView  = (ListCollectionView) grid.ItemsSource;

            grid.groupFilter.SetFilterValue(headerInfo.PropertyName, headerFilter.FilterText);

            // If we're editing, Refresh() throws InvalidOperationException. Try to commit the edit.
            grid.CommitEdit(DataGridEditingUnit.Row, true);

            try {
                listColView.Refresh();
            } catch (InvalidOperationException) {
            }
        } catch (Exception err) when (!Debugger.IsAttached) {
            MainWindow.ShowError(err, "Error Occurred");
        }
    }

    protected override void On_GotFocus(object sender, RoutedEventArgs e) {
        try {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource is DataGridCell cell) {
                ColorCell(cell);

                if (MainWindow.SingleClickToEditMode) {
                    // Needs to only happen when it's a button. If not, we stop regular fields from working.
                    if (CheckCellForButtonTypeAndHandleClick(cell)) return;

                    // We're past the _button check, now we just want to avoid a normal drop-down set to read only.
                    if (cell.IsReadOnly) return;

                    // Starts the Edit on the row;
                    BeginEdit(e);

                    if (cell.Content is ComboBox cbx) {
                        cbx.IsDropDownOpen = true;
                    }
                }
            }
        } catch (Exception err) when (!Debugger.IsAttached) {
            MainWindow.ShowError(err, "Error Occurred");
        }
    }

    private void ColorCell(DependencyObject cell) {
        if (coloredRow != null) coloredRow.Background = Brushes.White;
        coloredRow = cell.GetParent<DataGridRow>();
        // ReSharper disable once PossibleNullReferenceException
        coloredRow.Background = BACKGROUND_BRUSH;
    }

    protected override void On_Cell_MouseClick(object sender, MouseButtonEventArgs e) {
        try {
            if (sender is DataGridCell cell) {
                // We come here on both single & double click. If we don't check for focus, this hijacks the click and prevents focusing.
                if (e?.ClickCount == 1 && !cell.IsFocused) return;

                CheckCellForButtonTypeAndHandleClick(cell);
            }
        } catch (Exception err) when (!Debugger.IsAttached) {
            MainWindow.ShowError(err, "Error Occurred");
        }
    }

    private void On_Cell_Delete_Click(object sender, RoutedEventArgs e) {
        try {
            var obj = ((FrameworkElement) sender).DataContext;
            if (obj.ToString() == "{DataGrid.NewItemPlaceholder}") return;
            items.Remove((T) obj);
        } catch (Exception err) when (!Debugger.IsAttached) {
            MainWindow.ShowError(err, "Error Occurred");
        }
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private void On_Cell_Open_Click(object sender, RoutedEventArgs _, PropertyInfo propertyInfo, string displayName) {
        try {
            var frameworkElement = (FrameworkElement) sender;
            var obj              = frameworkElement.DataContext;
            var list             = propertyInfo.GetGetMethod()?.Invoke(obj, null);
            var listType         = list?.GetType().GenericTypeArguments[0];
            var viewType         = typeof(SubStructViewDynamic<>).MakeGenericType(listType ?? throw new InvalidOperationException());
            var subStructView    = (SubStructView) Activator.CreateInstance(viewType, Window.GetWindow(this), displayName, list, propertyInfo);

            ColorCell(frameworkElement);
            subStructView?.ShowDialog();
        } catch (Exception err) when (!Debugger.IsAttached) {
            MainWindow.ShowError(err, "Error Occurred");
        }
    }

    private bool CheckCellForButtonTypeAndHandleClick(DataGridCell cell) {
        if (cell.Content is not TextBlock) return false;

        // Have to loop though our column list to file the original property name.
        return (from propertyName in columnMap.Keys.Where(key => key.Contains("_button"))
                where cell.Column == columnMap[propertyName].column
                select EditSelectedItemId(cell, propertyName)).FirstOrDefault();
    }

    private bool EditSelectedItemId(FrameworkElement cell, string propertyName) {
        if (cell.DataContext is not IOnPropertyChanged obj) return false;

        // `sourceProperty` will only have a value if we come from a button.

        var property       = obj.GetType().GetProperty(propertyName.Replace("_button", ""), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!;
        var propertyType   = property.PropertyType;
        var value          = property.GetValue(obj);
        var dataSourceType = ((DataSourceAttribute) (sourceProperty ?? property).GetCustomAttribute(typeof(DataSourceAttribute), true))?.dataType;
        var showAsHex      = (ButtonIdAsHexAttribute) (sourceProperty ?? property).GetCustomAttribute(typeof(ButtonIdAsHexAttribute), true) != null;

        dynamic dataSource = dataSourceType switch {
            DataSourceType.DANGO_SKILLS => DataHelper.DANGO_SKILL_NAME_LOOKUP[Global.locale],
            DataSourceType.ITEMS => DataHelper.ITEM_NAME_LOOKUP[Global.locale],
            DataSourceType.RAMPAGE_SKILLS => DataHelper.RAMPAGE_SKILL_NAME_LOOKUP[Global.locale],
            DataSourceType.SKILLS => DataHelper.SKILL_NAME_LOOKUP[Global.locale],
            DataSourceType.SWITCH_SKILLS => DataHelper.SWITCH_SKILL_NAME_LOOKUP[Global.locale],
            _ => throw new ArgumentOutOfRangeException(dataSourceType.ToString())
        };

        var getNewItemId = new GetNewItemId(value, dataSource, showAsHex);
        getNewItemId.ShowDialog();

        if (!getNewItemId.Cancelled) {
            property.SetValue(obj, Convert.ChangeType(getNewItemId.CurrentItem, propertyType));
            obj.OnPropertyChanged(propertyName);
        }

        return true;
    }

    protected override void On_Sorting(object sender, DataGridSortingEventArgs e) {
        try {
            // Does the column we're sorting define a custom sorter?
            var matches = columnMap.Where(pair => pair.Value.column == e.Column && pair.Value.customSorter != null).ToList();
            if (!matches.Any()) return;
            var customSorter = matches.First().Value.customSorter;

            e.Column.SortDirection = customSorter!.SortDirection = e.Column.SortDirection != ListSortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            var listColView = (ListCollectionView) ItemsSource;
            listColView.CustomSort = customSorter;

            e.Handled = true;
        } catch (Exception err) when (!Debugger.IsAttached) {
            MainWindow.ShowError(err, "Error Occurred");
        }
    }

    protected override void On_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
        try {
            // Commit as cell edit ends instead of DG waiting till we leave the row.
            if (!isManualEditCommit) {
                isManualEditCommit = true;
                CommitEdit(DataGridEditingUnit.Row, true);
                isManualEditCommit = false;
            }
        } catch (Exception err) when (!Debugger.IsAttached) {
            MainWindow.ShowError(err, "Error Occurred");
        }
    }
}