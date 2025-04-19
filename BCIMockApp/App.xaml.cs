namespace BCIMockApp
{
    public partial class App : Application
    {
        public static bool IsLightTheme { get; set; } = true;

        public App()
        {
            InitializeComponent();

            // Set the initial theme based on system preference
            var appTheme = Current.RequestedTheme;
            IsLightTheme = appTheme == AppTheme.Light;
            
            // Apply the theme
            ApplyTheme();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        public static void ApplyTheme()
        {
            Current.UserAppTheme = IsLightTheme ? AppTheme.Light : AppTheme.Dark;
        }

        public static void SwitchTheme()
        {
            IsLightTheme = !IsLightTheme;
            ApplyTheme();
        }
    }
}