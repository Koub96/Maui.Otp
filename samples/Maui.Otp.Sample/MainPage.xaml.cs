namespace Maui.Otp.Sample
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            this.otp.OtpCompleted += (s, e) => 
            {
                this.otp.HasError = true;
            };

            this.otp.FillOtp("123456");
        }
    }
}
