using UI_V2.WebView;

namespace UI_V2
{
    public partial class AppWindow : Form
    {
        private readonly Window _window;
        private readonly TopBar _topBar;
        private readonly Main _main;

        public AppWindow()
        {
            _window = new(this, OnFocus, OnBlur, OnMaximize, OnRestore);

            InitializeComponent();

            _topBar = new(_window, WebView_TopBar);
            _main = new(_window, WebView_Main);
            
            _window.FixStyle();
            _window.SetIcon("icon.ico");
        }

        #region Public Methods



        #endregion

        #region Private Methods

        private void OnFocus(object sender, EventArgs e) => _topBar.SetFocus();

        private void OnBlur(object sender, EventArgs e) => _topBar.SetBlur();

        private void OnMaximize(object sender, EventArgs e) => _topBar.SetMaximized();

        private void OnRestore(object sender, EventArgs e) => _topBar.SetRestored();

        protected override void WndProc(ref Message message)
        {
            bool handled = _window.WndProc(ref message);

            // If it wasn't handled by the custom logic pass it on
            if (!handled)
                base.WndProc(ref message);
        }

        #endregion
    }
}
