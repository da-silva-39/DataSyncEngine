using Views.Components;

namespace Views.Forms;

public partial class MainView : Form
{
    public MainView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
    }
}
