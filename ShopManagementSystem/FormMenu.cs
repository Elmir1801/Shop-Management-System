using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ShopManagementSystem
{
    public partial class FormMenu : Form
    {
        private int childFormNumber = 0;

        private Panel panelCenterDesign;
        private Panel cardProducts;
        private Panel cardStocks;
        private PictureBox pictureBanner;
        private MdiClient mdiClientArea;

        public FormMenu()
        {
            InitializeComponent();
            this.Resize += FormMenu_Resize;
            this.Shown += FormMenu_Shown;
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void iNSERTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerInsert custInsert = new CustomerInsert();
            custInsert.MdiParent = this;
            custInsert.Show();
        }

        private void vIEWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerView custView = new CustomerView();
            custView.MdiParent = this;
            custView.Show();
        }

        private void uPDATEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerUpdate custUpdate = new CustomerUpdate();
            custUpdate.MdiParent = this;
            custUpdate.Show();
        }

        private void dELETEToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CustomerDelete custDelete = new CustomerDelete();
            custDelete.MdiParent = this;
            custDelete.Show();
        }

        private void FormMenu_MdiChildActivate(object sender, EventArgs e)
        {
            if (panelCenterDesign != null)
            {
                panelCenterDesign.Visible = this.ActiveMdiChild == null;
                panelCenterDesign.BringToFront();
            }

            if (toolStripStatusLabel != null)
            {
                if (this.ActiveMdiChild != null)
                    toolStripStatusLabel.Text = "Opened: " + this.ActiveMdiChild.Text;
                else
                    toolStripStatusLabel.Text = "Ready";
            }
        }

        private void iNSERTToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProductInsert prodInsert = new ProductInsert();
            prodInsert.MdiParent = this;
            prodInsert.Show();
        }

        private void vIEWToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProductView prodView = new ProductView();
            prodView.MdiParent = this;
            prodView.Show();
        }

        private void uPDATEToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProductUpdate prodUpdate = new ProductUpdate();
            prodUpdate.MdiParent = this;
            prodUpdate.Show();
        }

        private void dELETEToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProductDelete prodDelete = new ProductDelete();
            prodDelete.MdiParent = this;
            prodDelete.Show();
        }

        private void iNSERTToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            VendorInsert vendorInsert = new VendorInsert();
            vendorInsert.MdiParent = this;
            vendorInsert.Show();
        }

        private void vIEWToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            VendorView vendorView = new VendorView();
            vendorView.MdiParent = this;
            vendorView.Show();
        }

        private void uPDATEToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            VendorUpdate vendorUpdate = new VendorUpdate();
            vendorUpdate.MdiParent = this;
            vendorUpdate.Show();
        }

        private void dELETEToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            VendorDelete vendorDelete = new VendorDelete();
            vendorDelete.MdiParent = this;
            vendorDelete.Show();
        }

        private void vIEWToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            StockView stockView = new StockView();
            stockView.MdiParent = this;
            stockView.Show();
        }

        private void uPDATEToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            StockUpdate stockUpdate = new StockUpdate();
            stockUpdate.MdiParent = this;
            stockUpdate.Show();
        }

        private void iNSERTToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            StockInsert stockInsert = new StockInsert();
            stockInsert.MdiParent = this;
            stockInsert.Show();
        }

        private void dELETEToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            StockDelete stockDelete = new StockDelete();
            stockDelete.MdiParent = this;
            stockDelete.Show();
        }

        private void vIEWToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void cREATEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderInsert orderCreate = new OrderInsert();
            orderCreate.MdiParent = this;
            orderCreate.Show();
        }

        private void dELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderDelete orderDelete = new OrderDelete();
            orderDelete.MdiParent = this;
            orderDelete.Show();
        }

        private void pRODUCTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportProduct reportProduct = new ReportProduct();
            reportProduct.MdiParent = this;
            reportProduct.Show();
        }

        private void oRDERSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cUSTOMERSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReportCustomer reportCustomer = new ReportCustomer();
            reportCustomer.MdiParent = this;
            reportCustomer.Show();
        }

        private void vENDORSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReportVendors reportVendors = new ReportVendors();
            reportVendors.MdiParent = this;
            reportVendors.Show();
        }

        private void aVAILABLESTOCKSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportStocks reportStocks = new ReportStocks();
            reportStocks.MdiParent = this;
            reportStocks.Show();
        }

        private void FormMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void dEVELOPERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Developers dev = new Developers();
            dev.MdiParent = this;
            dev.Show();
        }

        private void aPPLICATIONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyApplications applications = new MyApplications();
            applications.MdiParent = this;
            applications.Show();
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            ApplyMenuTheme();

            if (toolStripStatusLabel != null)
                toolStripStatusLabel.Text = "Ready";
        }

        private void dETAILSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Details details = new Details();
            details.MdiParent = this;
            details.Show();
        }

        private void FormMenu_Shown(object sender, EventArgs e)
        {
            SetupMdiClient();
            CreateDashboardDesign();
            CenterDashboardDesign();
        }

        private void FormMenu_Resize(object sender, EventArgs e)
        {
            CenterDashboardDesign();
        }

        private void ApplyMenuTheme()
        {
            this.Text = "Shop Management System";
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            this.BackColor = Color.FromArgb(255, 153, 0); // orange

            if (menuStrip != null)
            {
                menuStrip.BackColor = Color.FromArgb(0, 102, 204); // blue
                menuStrip.ForeColor = Color.White;
                menuStrip.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }

            if (panel4 != null)
            {
                panel4.BackColor = Color.FromArgb(0, 102, 204); // blue footer/header strip
            }

            if (label10 != null)
            {
                label10.Text = "SHOP MANAGEMENT SYSTEM";
                label10.ForeColor = Color.White;
                label10.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            }

            if (statusStrip != null)
            {
                statusStrip.BackColor = Color.FromArgb(0, 102, 204);
                statusStrip.ForeColor = Color.White;
            }

            if (toolStripStatusLabel != null)
            {
                toolStripStatusLabel.ForeColor = Color.White;
            }
        }

        private void SetupMdiClient()
        {
            mdiClientArea = this.Controls.OfType<MdiClient>().FirstOrDefault();
            if (mdiClientArea != null)
            {
                mdiClientArea.BackColor = Color.FromArgb(255, 153, 0);
            }
        }

        private void CreateDashboardDesign()
        {
            if (panelCenterDesign != null)
                return;

            panelCenterDesign = new Panel();
            panelCenterDesign.Size = new Size(610, 220);
            panelCenterDesign.BackColor = Color.FromArgb(255, 153, 0); // same as form bg
            panelCenterDesign.BorderStyle = BorderStyle.None;

            Label lblTitle = new Label();
            lblTitle.Text = "SHOP MANAGEMENT SYSTEM";
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 102, 204);
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;

            Label lblSubTitle = new Label();
            lblSubTitle.Text = "Products and Stocks";
            lblSubTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblSubTitle.ForeColor = Color.FromArgb(255, 255, 255);
            lblSubTitle.AutoSize = true;
            lblSubTitle.BackColor = Color.Transparent;

            Panel borderPanel = new Panel();
            borderPanel.Size = new Size(610, 220);
            borderPanel.Location = new Point(0, 0);
            borderPanel.BackColor = Color.Transparent;
            borderPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(255, 230, 180), 2))
                {
                    e.Graphics.DrawRectangle(pen, 1, 1, borderPanel.Width - 3, borderPanel.Height - 3);
                }
            };

            panelCenterDesign.Controls.Add(lblTitle);
            panelCenterDesign.Controls.Add(lblSubTitle);
            panelCenterDesign.Controls.Add(borderPanel);

            this.Controls.Add(panelCenterDesign);
            panelCenterDesign.BringToFront();

            lblTitle.Left = (panelCenterDesign.Width - lblTitle.Width) / 2;
            lblTitle.Top = 60;

            lblSubTitle.Left = (panelCenterDesign.Width - lblSubTitle.Width) / 2;
            lblSubTitle.Top = 120;

            borderPanel.SendToBack();
        }
        private Panel CreateFeatureCard(string title, string description, Point location, Color backColor, Color titleColor)
        {
            Panel card = new Panel();
            card.Size = new Size(320, 150);
            card.Location = location;
            card.BackColor = backColor;
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lblIcon = new Label();
            lblIcon.Text = "■";
            lblIcon.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblIcon.ForeColor = titleColor;
            lblIcon.AutoSize = true;
            lblIcon.Location = new Point(20, 18);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = titleColor;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(50, 18);

            Label lblDescription = new Label();
            lblDescription.Text = description;
            lblDescription.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblDescription.ForeColor = Color.FromArgb(70, 70, 70);
            lblDescription.Size = new Size(270, 55);
            lblDescription.Location = new Point(25, 65);

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblDescription);

            return card;
        }

        private void CenterDashboardDesign()
        {
            if (panelCenterDesign == null)
                return;

            int topOffset = 0;
            int bottomOffset = 0;

            if (menuStrip != null)
                topOffset += menuStrip.Height;

            if (panel4 != null)
                bottomOffset += panel4.Height;

            if (statusStrip != null)
                bottomOffset += statusStrip.Height;

            int availableHeight = this.ClientSize.Height - topOffset - bottomOffset;

            panelCenterDesign.Left = (this.ClientSize.Width - panelCenterDesign.Width) / 2;
            panelCenterDesign.Top = topOffset + ((availableHeight - panelCenterDesign.Height) / 2) - 40;

            if (panelCenterDesign.Top < topOffset + 20)
                panelCenterDesign.Top = topOffset + 20;

            panelCenterDesign.BringToFront();
        }
    }
    }