using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Gallery3WinForm
{
    public partial class frmWork : Form
    {
        protected clsAllWork _Work;

        public frmWork()
        {
            InitializeComponent();
        }

        public void SetDetails(clsAllWork prWork)
        {
            _Work = prWork;
            updateForm();
            ShowDialog();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                pushData();
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected virtual bool isValid()
        {
            return true;
        }

        protected virtual void updateForm()
        {
            txtName.Text = _Work.Name;
            txtCreation.Text = _Work.Date.ToShortDateString();
            txtValue.Text = _Work.Value.ToString();
        }

        protected virtual void pushData()
        {
            _Work.Name = txtName.Text;
            _Work.Date = DateTime.Parse(txtCreation.Text);
            _Work.Value = decimal.Parse(txtValue.Text);
        }

        public delegate void LoadWorkFormDelegate(clsAllWork prWork);
        public static Dictionary<char, Delegate> _WorksForm = new Dictionary<char, Delegate>
        {
            { 'P', new LoadWorkFormDelegate(frmPainting.Run) },
            { 'H', null },
            { 'S', null }
        };

        public static void DispatchWorkForm(clsAllWork prWork) {
             char lcType = char.Parse(prWork.Type);
            _WorksForm[lcType].DynamicInvoke(prWork);
        }

    }
}