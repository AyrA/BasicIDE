using System.Collections.Generic;
using System.Windows.Forms;

namespace BasicIDE
{
    /// <summary>
    /// Make Message box usage more comfortable.
    /// </summary>
    /// <remarks>
    /// The first letter of a method is the box type, the other letters the buttons.
    /// EARI: Error box with [Abort] + [Retry] + [Ignore]
    /// </remarks>
    public static class MBox
    {
        private static IEnumerable<Form> Forms()
        {
            foreach (var F in Application.OpenForms)
            {
                yield return (Form)F;
            }
        }

        public static DialogResult Box(string Message, string Title = null, MessageBoxButtons Buttons = MessageBoxButtons.OK, MessageBoxIcon Icon = MessageBoxIcon.Information)
        {
            //If title is not set, try to obtain from active form
            if (Title == null)
            {
                foreach (var F in Forms())
                {
                    if (F.Focused)
                    {
                        Title = F.Text;
                        break;
                    }
                }
                //Default to name of icon if no form was active
                if (Title == null)
                {
                    Title = Icon.ToString();
                }
            }
            return MessageBox.Show(Message, Title, Buttons, Icon);
        }

        #region Error

        public static DialogResult E(string Msg, string Title = null)
        {
            return Box(Msg, Title, Icon: MessageBoxIcon.Error);
        }

        public static DialogResult EYN(string Msg, string Title = null)
        {
            return Box(Msg, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        }

        public static DialogResult ERC(string Msg, string Title = null)
        {
            return Box(Msg, Title, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
        }

        public static DialogResult EARI(string Msg, string Title = null)
        {
            return Box(Msg, Title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
        }

        #endregion

        #region Warning

        public static DialogResult W(string Msg, string Title = null)
        {
            return Box(Msg, Title, Icon: MessageBoxIcon.Warning);
        }

        public static DialogResult WYN(string Msg, string Title = null)
        {
            return Box(Msg, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        #endregion

        #region Info

        public static DialogResult I(string Msg, string Title = null)
        {
            return Box(Msg, Title, Icon: MessageBoxIcon.Information);
        }

        public static DialogResult IOC(string Msg, string Title = null)
        {
            return Box(Msg, Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        #endregion
    }
}
