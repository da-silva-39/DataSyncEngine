using Views.Components;
using Views.ViewModels;

namespace Views.Forms.SubViews;

public partial class MasterKeyPopUpView : MaterialSkin.Controls.MaterialForm
{
    private readonly MasterKeyViewModel _viewModel = new();

    public bool Authorized => _viewModel.IsAuthorized;

    public MasterKeyPopUpView()
    {
        InitializeComponent();
        MaterialThemeModule.Apply(this);
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        _viewModel.EnteredKey = txtMasterKey.Text;
        if (_viewModel.Validate())
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            lblError.Text = _viewModel.ErrorMessage;
        }
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
