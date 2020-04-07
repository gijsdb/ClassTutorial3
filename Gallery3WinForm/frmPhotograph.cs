namespace Gallery3WinForm
{
    public sealed partial class frmPhotograph : Gallery3WinForm.frmWork
    {   //Singleton
        public static readonly frmPhotograph Instance = new frmPhotograph();

        private frmPhotograph()
        {
            InitializeComponent();
        }

        public static void Run(clsAllWork prPhotograph)
        {
            Instance.SetDetails(prPhotograph);
        }

        protected override void updateForm()
        {
            base.updateForm();
            //clsAllWork lcWork = (clsAllWork)this._Work;
            txtWidth.Text = _Work.Width.ToString();
            txtHeight.Text = _Work.Height.ToString();
            txtType.Text = _Work.Type;
        }

        protected override void pushData()
        {
            base.pushData();
            //clsAllWork lcWork = (clsAllWork)_Work;
            _Work.Width = float.Parse(txtWidth.Text);
            _Work.Height = float.Parse(txtHeight.Text);    
            _Work.Type = txtType.Text;
        }
    }
}

