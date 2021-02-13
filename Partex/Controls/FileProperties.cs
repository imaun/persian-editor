using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Farcin.Editor.Core.Models;
using Farcin.Editor.Core.Date;
using Farcin.Editor.Core.Helpers;
using Farcin.Editor.Core.Models.Types;

namespace Farcin.Editor.Controls
{
    public partial class FileProperties : UserControl {

        public FileProperties(TxtEditor editor) {
            InitializeComponent();
            initListView();
            LoadFileInfo(editor);
        }

        private void initListView() {
            ColumnHeader header1, header2;
            header1 = new ColumnHeader {
                Text = "مشخصه",
                TextAlign = HorizontalAlignment.Left,
                Width = -1
            };
            header2 = new ColumnHeader {
                Text = "مقدار",
                TextAlign = HorizontalAlignment.Right,
                Width = -1
            };
            listView.Columns.Add(header1);
            listView.Columns.Add(header2);
            listView.View = View.Details;
        }

        public EventHandler OnWindowClose { get; set; }

        public IList<FileProperty> DataSource { get; set; } 

        public void LoadFileInfo(TxtEditor editor) {
            var info = editor.File.Info;

            if(info == null) {
                DataSource = new List<FileProperty> {
                    new FileProperty {
                        Title = "تعداد کاراکترها",
                        Value = editor.TextLength.ToString("N0")
                    },
                    new FileProperty {
                        Title = "تعداد خطوط",
                        Value = editor.LinesCount.ToString("N0")
                    },
                    new FileProperty {
                        Title = "تعداد کلمات",
                        Value = editor.WordsCount.ToString("N0")
                    }
                };
            }
            else {
                DataSource = new List<FileProperty> {
                    new FileProperty {
                        Title = "نام فایل",
                        Value = editor.File.Name
                    },
                    new FileProperty {
                        Title = "مسیر",
                        Value = editor.File.Info.DirectoryName
                    },
                    new FileProperty {
                        Title = "سایز فایل",
                        Value = editor.File.FileSizeDisplay
                    },
                    new FileProperty {
                        Title = "تاریخ ایجاد",
                        Value = DateTimeHelper.GetPersianDateStr(
                                    editor.File.Info.CreationTimeUtc,
                                    InsertDateFormat.DayMonthNameYear,
                                    true)
                    },
                    new FileProperty {
                        Title = "آخرین تغییر",
                        Value = DateTimeHelper.GetPersianDateStr(
                                    editor.File.Info.LastWriteTimeUtc,
                                    InsertDateFormat.DayMonthNameYear,
                                    true)
                    },
                    new FileProperty {
                        Title = "آخرین دسترسی",
                        Value = DateTimeHelper.GetPersianDateStr(
                                    editor.File.Info.LastAccessTimeUtc,
                                    InsertDateFormat.DayMonthNameYear,
                                    true)
                    },
                    new FileProperty {
                        Title = "تعداد کاراکترها",
                        Value = editor.TextLength.ToString("N0")
                    },
                    new FileProperty {
                        Title = "تعداد خطوط",
                        Value = editor.LinesCount.ToString("N0")
                    },
                    new FileProperty {
                        Title = "تعداد کلمات",
                        Value = editor.WordsCount.ToString("N0")
                    }
                };
            }

            bindDataSource();
        }

        private void bindDataSource() {
            if (DataSource.Count == 0)
                return;

            listView.Items.Clear();
            foreach(var data in DataSource) {
                var listItem = new ListViewItem {
                    Text = data.Title
                };
                listItem.SubItems.Add(data.Value);
                listView.Items.Add(listItem);
            }
        }

        private void btnClose_Click(object sender, EventArgs e) {
            OnWindowClose?.Invoke(sender, e);
        }
    }
}
