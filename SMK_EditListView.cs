using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI {
    /// <summary>
    /// Summary description for SMK_EditListView.
    /// </summary>

    public delegate void ItemEditedEvent(object source, ItemEditArgs e);
    public class ItemEditArgs : EventArgs {
        private object EventInfo;
        public bool success = true;
        public ItemEditArgs(object item, ref bool success) {
            EventInfo = item;
            this.success = success;
        }
        public object GetInfo() {
            return EventInfo;
        }
    }

    public class SMK_EditListView : ListView {
        private ListViewItem li;
        private int X = 0;
        private int Y = 0;
        private string subItemText;
        private int subItemSelected = 0;
        private System.Windows.Forms.TextBox editBox = new System.Windows.Forms.TextBox();
        private System.Windows.Forms.ComboBox cmbBox = new System.Windows.Forms.ComboBox();
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        public event ItemEditedEvent OnItemEdited;

        public SMK_EditListView() {
            cmbBox.Size = new System.Drawing.Size(0, 0);
            cmbBox.Location = new System.Drawing.Point(0, 0);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.cmbBox });
            cmbBox.SelectedIndexChanged += new System.EventHandler(this.CmbSelected);
            cmbBox.LostFocus += new System.EventHandler(this.CmbFocusOver);
            cmbBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CmbKeyPress);
            cmbBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            cmbBox.BackColor = Color.SkyBlue;
            cmbBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBox.Hide();


            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();

            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                            this.columnHeader1,
                                                                            this.columnHeader2,
                                                                            this.columnHeader3,
                                                                            this.columnHeader4});
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.FullRowSelect = true;
            this.Name = "listView1";
            this.Size = new System.Drawing.Size(0, 0);
            this.TabIndex = 0;
            this.View = System.Windows.Forms.View.Details;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SMKMouseDown);
            this.DoubleClick += new System.EventHandler(this.SMKDoubleClick);
            this.GridLines = true;

            editBox.Size = new System.Drawing.Size(0, 0);
            editBox.Location = new System.Drawing.Point(0, 0);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.editBox });
            editBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
            editBox.LostFocus += new System.EventHandler(this.FocusOver);
            editBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            editBox.BackColor = Color.LightYellow;
            editBox.BorderStyle = BorderStyle.Fixed3D;
            editBox.Hide();
            editBox.Text = "";
        }

        private void CmbKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
            if (e.KeyChar == 13 || e.KeyChar == 27) {
                OnEditingCanceled();
            }
        }

        private void CmbSelected(object sender, System.EventArgs e) {
            int sel = cmbBox.SelectedIndex;
            if (sel >= 0) {
                string itemSel = cmbBox.Items[sel].ToString();
                li.SubItems[subItemSelected].Text = itemSel;
            }
        }

        private void CmbFocusOver(object sender, System.EventArgs e) {
            cmbBox.Hide();
        }

        private void EditOver(object sender, System.Windows.Forms.KeyPressEventArgs e) {
            if (e.KeyChar == 13) {
                OnEditingFinished();
            }

            if (e.KeyChar == 27) OnEditingCanceled();
        }

        private void FocusOver(object sender, System.EventArgs e) {
            OnEditingFinished();
        }
        private void OnEditingCanceled() {
            editBox.Hide();
        }
        private void OnEditingFinished() {
            var success = true;
            if (OnItemEdited != null) {
                OnItemEdited(this, new ItemEditArgs(li.SubItems[subItemSelected], ref success));
            }
            if (success) li.SubItems[subItemSelected].Text = editBox.Text;
            editBox.Hide();
        }

        public void SMKDoubleClick(object sender, System.EventArgs e) {
            int nStart = X;
            int spos = 0;
            int epos = this.Columns[0].Width;
            for (int i = 0; i < this.Columns.Count; i++) {
                if (nStart > spos && nStart < epos) {
                    subItemSelected = i;
                    break;
                }

                spos = epos;
                epos += this.Columns[i].Width;
            }

            subItemText = li.SubItems[subItemSelected].Text;
            Rectangle r = new Rectangle(spos, li.Bounds.Y, epos, li.Bounds.Bottom);
            editBox.Size = new System.Drawing.Size(epos - spos, li.Bounds.Bottom - li.Bounds.Top);
            editBox.Location = new System.Drawing.Point(spos, li.Bounds.Y);
            editBox.Show();
            editBox.Text = subItemText;
            editBox.SelectAll();
            editBox.Focus();
        }

        public void SMKMouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            li = this.GetItemAt(e.X, e.Y);
            X = e.X;
            Y = e.Y;
        }

    }
}
