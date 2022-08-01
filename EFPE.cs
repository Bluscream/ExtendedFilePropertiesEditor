using LogicNP.EZShellExtensions;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EFPE));
            this.lvProp = new WindowsApplication1.SMK_EditListView();
            this.propColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
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
            // EFPE
            // 
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

                // Read and Write:
                foreach (var prop in file.Properties.DefaultPropertyCollection) {
                    if (prop.Description is null || prop.Description.DisplayName is null) continue;
                    if (prop.ValueAsObject is null) continue;
                    try {
                        AddProperty(prop.Description.DisplayName, prop.ValueAsObject.ToString());
                    } catch (Exception ex) {
                        AddProperty(prop.Description.DisplayName, $"[E] {ex.Message}");
                    }
                }

                //file.Properties.System.Author.Value = new string[] { "Author #1", "Author #2" };
                //file.Properties.System.Title.Value = "Example Title";

                //// Alternate way to Write:

                //ShellPropertyWriter propertyWriter = file.Properties.GetPropertyWriter();
                //propertyWriter.WriteProperty(SystemProperties.System.Author, new string[] { "Author" });
                //propertyWriter.Close();

            } catch (Exception ex) {
                AddProperty("ERROR : ", ex.Message);
            }

            foreach (ColumnHeader c in lvProp.Columns) {
                c.Width = -2;
            }

        }

        void AddProperty(string propName, object propValue) {
            if (string.IsNullOrWhiteSpace(propName) || propValue is null) return;
            ListViewItem item = lvProp.Items.Add(propName);
            item.SubItems.Add(propValue.ToString());
        }

        private void lvProp_SelectedIndexChanged(object sender, System.EventArgs e) {

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