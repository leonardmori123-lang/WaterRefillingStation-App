using System.Windows.Forms;

namespace WaterRefillingStation.Forms
{
    // Opens one screen at a time: hides the current form, shows the next one,
    // and brings the current form back when the next one closes.
    // Any screen that opens another full screen should call FormNavigator.Open(this, new OtherForm()).
    public static class FormNavigator
    {
        public static void Open(Form current, Form next)
        {
            next.StartPosition = FormStartPosition.CenterScreen;
            next.FormClosed += (s, e) =>
            {
                if (current.IsDisposed) return;
                current.Show();
                current.Activate();
            };

            current.Hide();
            next.Show();
        }
    }
}