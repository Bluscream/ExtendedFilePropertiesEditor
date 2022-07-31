using LogicNP.EZShellExtensions;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EFPE {
    [Guid("6B970FF5-435F-4c75-B8BF-3EBB7A7BB0B2"), ComVisible(true)]
    public class EFPE : PropertySheetExtension {
        private System.Windows.Forms.ColumnHeader propColumn;
        private System.Windows.Forms.ListView lvProp;
        private System.Windows.Forms.ColumnHeader ValueColumn;

        public EFPE() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EFPE));
            this.lvProp = new System.Windows.Forms.ListView();
            this.propColumn = new System.Windows.Forms.ColumnHeader();
            this.ValueColumn = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lvProp
            // 
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
            this.Name = "Properties";
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.XYZPropertySheetExtension_Load);
            this.ResumeLayout(false);

        }

        private void XYZPropertySheetExtension_Load(object sender, System.EventArgs e) {
            try {
                Assembly a = Assembly.LoadFrom(TargetFiles[0]);
                AddProperty("Location", a.Location);
                AddProperty("CodeBase", a.CodeBase);
                AddProperty("FullName", a.FullName);
                AddProperty("Num Resources", a.GetManifestResourceNames().Length);
                AddProperty("Num Defined Types", a.GetTypes().Length);
                AddProperty("Num Exported Types", a.GetExportedTypes().Length);
                AddProperty("Num Referenced Assemblies", a.GetReferencedAssemblies().Length);

            } catch (BadImageFormatException) {
                AddProperty("ERROR : Not a .Net Assembly", string.Empty);
            } catch (Exception) {
            }

            foreach (ColumnHeader c in lvProp.Columns) {
                c.Width = -2;
            }

        }

        void AddProperty(string propName, object propValue) {
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


    }
}