using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicIDE
{
    public partial class FrmInput : Form
    {
        public const string DefaultInvalidFormatMessage = "The supplied value is in an invalid format";
        public const string DefaultEmptyValueMessage = "The supplied value cannot be left empty";

        private readonly bool allowEmpty;
        private readonly Regex expression;
        
        /// <summary>
        /// Gets the text entered by the user
        /// </summary>
        /// <remarks>This is null if the dialog is cancelled</remarks>
        public string Response { get; private set; }

        /// <summary>
        /// Get or set the custom message for when the expression fails to match.
        /// Has no effect if the expression is not supplied in the constructor.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="DefaultInvalidFormatMessage"/>
        /// </remarks>
        public string InvalidFormatMessage { get; set; } = DefaultInvalidFormatMessage;

        /// <summary>
        /// Gets or sets the custom message for when the user tries to commit an empty answer.
        /// Has no effect if empty answers are allowed in the constructor.
        /// </summary
        /// <remarks>
        /// Defaults to <see cref="DefaultEmptyValueMessage"/>
        /// </remarks>
        public string EmptyValueMessage { get; set; } = DefaultEmptyValueMessage;

        /// <summary>
        /// Creates a new text prompt dialog
        /// </summary>
        /// <param name="Prompt">Label text (required)</param>
        /// <param name="Title">Dialog title (required)</param>
        /// <param name="Prefill">Prefilled string</param>
        /// <param name="AllowEmpty">Allow submitting of empty strings</param>
        /// <param name="Expression">Regular expression to match the response against</param>
        /// <param name="Options">Options for <paramref name="Expression"/></param>
        public FrmInput(string Prompt, string Title, string Prefill = null, bool AllowEmpty = false, string Expression = null, RegexOptions Options = RegexOptions.IgnoreCase)
        {
            if (string.IsNullOrEmpty(Title))
            {
                throw new ArgumentNullException(nameof(Title));
            }

            if (string.IsNullOrEmpty(Prompt))
            {
                throw new ArgumentNullException(nameof(Prompt));
            }

            InitializeComponent();
            Text = Title;
            LblPrompt.Text = Prompt;
            TbInput.Text = Prefill;
            allowEmpty = AllowEmpty;
            if (expression != null)
            {
                expression =new Regex(Expression, Options);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Response = null;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if(!allowEmpty && string.IsNullOrEmpty(TbInput.Text))
            {
                MessageBox.Show(EmptyValueMessage, "Invalid response", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(expression!=null && !expression.IsMatch(TbInput.Text))
            {
                MessageBox.Show(InvalidFormatMessage, "Invalid format", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Response = TbInput.Text;
            DialogResult = DialogResult.OK;
        }

        private void FrmInput_Load(object sender, EventArgs e)
        {
            TbInput.SelectAll();
        }
    }
}
