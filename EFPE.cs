using LogicNP.EZShellExtensions;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Bluscream;
using UI;
namespace EFPE {

    [TargetExtension(SpecialProgIDTargets.AllFiles, true)]
    [TargetExtension(".*", true)]
    [TargetExtension(".dll", true)]
    [TargetExtension(".exe", true)]
    [TargetExtension(".lnk", true)]
    public class EFPE : PropertySheetExtension {
        private System.Windows.Forms.ColumnHeader propColumn;
        private System.Windows.Forms.ListView lvProp;
        private System.Windows.Forms.ColumnHeader ValueColumn;

        public EFPE() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EFPE));
            this.lvProp = new SMK_EditListView() as SMK_EditListView; // new ListView();
            // this.lvProp.OnItemEdited += OnItemEdited;
            this.propColumn = new System.Windows.Forms.ColumnHeader();
            this.ValueColumn = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lvProp
            // 
            this.lvProp.Columns.Clear();
            this.lvProp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                     this.propColumn,
                                                                                     this.ValueColumn});
            this.lvProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvProp.Name = "lvProp";
            this.lvProp.Size = new System.Drawing.Size(350, 450);
            this.lvProp.TabIndex = 0;
            this.lvProp.View = System.Windows.Forms.View.Details;
            this.lvProp.SelectedIndexChanged += new System.EventHandler(this.lvProp_SelectedIndexChanged);
            // 
            // propColumn
            // 
            this.propColumn.Text = "Property";
            this.propColumn.Width = 145;
            // 
            // ValueColumn
            // 
            this.ValueColumn.Text = "Value";
            this.ValueColumn.Width = 185;
            // 
            // AssemblyInfo
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.lvProp});
            this.Icon = ((System.Drawing.Bitmap)(resources.GetObject("$this.Icon")));
            this.Name = "EFPE";
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.XYZPropertySheetExtension_Load);
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EFPE_KeyEvent);
            //this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EFPE_KeyPress);
            //this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EFPE_KeyEvent);
            this.ResumeLayout(false);

        }

        private void XYZPropertySheetExtension_Load(object sender, System.EventArgs e) {
            try {
                var file = ShellFile.FromFilePath(TargetFiles[0]);
                foreach (var prop in file.Properties.DefaultPropertyCollection) {
                    if (prop.Description?.DisplayName is null) continue;
                    if (prop.ValueAsObject is null) continue;
                    try {
                        //if (prop.ValueType == typeof(System.String[])) {
                        //    AddProperty(prop.Description.DisplayName, string.Join(", ", prop.ValueAsObject));
                        //} else {
                            AddProperty(prop);
                        //}
                    } catch (Exception ex) {
                        AddProperty(prop.Description.DisplayName, $"[E] {ex.Message}");
                    }
                }
            } catch (Exception ex) {
                AddProperty("ERROR : ", ex.Message);
            }
            foreach (ColumnHeader c in lvProp.Columns) {
                c.Width = -2;
            }
        }

        void AddProperty(string propName, string propValue, object tag = null) {
            if (string.IsNullOrWhiteSpace(propName) || propValue is null) return;
            ListViewItem item = lvProp.Items.Add(propName);
            item.Tag = tag;
            item.SubItems.Add(propValue);
        }

        void AddProperty(IShellProperty prop) {
            if (prop.Description is null || prop.Description.DisplayName is null) return;
            if (prop.ValueAsObject is null) return;
            AddProperty(prop.Description.DisplayName, prop.ValueAsObject.ToJSON(), prop);
        }

        public void OnItemEdited(object item, bool success) {

        }

        private void lvProp_SelectedIndexChanged(object sender, System.EventArgs e) {

        }

        protected override NotifyResult OnApply() {
            var result = NotifyResult.NoError;
            foreach (var _file in TargetFiles) {
                var file = ShellFile.FromFilePath(_file);
                //file.Properties.System.Author.Value = new string[] { "Author #1", "Author #2" };
                //file.Properties.System.Title.Value = "Example Title";
                ShellPropertyWriter propertyWriter = file.Properties.GetPropertyWriter();
                foreach (ListViewItem item in lvProp.Items) {
                    try {
                        if (item.Tag is null) continue;
                        var tag = item.Tag as IShellProperty;
                        if (tag.ValueAsObject.ToJSON() != item.SubItems[0].Text) {
                            //propertyWriter.WriteProperty(tag.CanonicalName, item.SubItems[0].Text.FromJSON());
                        }
                    } catch (Exception ex) {
                        AddProperty("ERROR", ex.Message);
                        result = NotifyResult.Invalid;
                    }
                }
                propertyWriter.Close();
            }
            return result;
        }

        [ComRegisterFunction]
        public static void Register(System.Type t) {
            PropertySheetExtension.RegisterExtension(typeof(EFPE));
        }

        [ComUnregisterFunction]
        public static void UnRegister(System.Type t) {
            PropertySheetExtension.UnRegisterExtension(typeof(EFPE));
        }

        private void EFPE_KeyPress(object sender, KeyPressEventArgs e) {
            if (string.IsNullOrWhiteSpace(e.KeyChar.ToString())) {
                e.Handled = true;
            }
        }

        private void EFPE_KeyEvent(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
    }
}