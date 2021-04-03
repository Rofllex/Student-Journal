using Journal.Common.Entities;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace Journal.WindowsForms.Forms
{
    public partial class SelectRoleForm : Form
    {
        public static UserRole? SelectRoleDialog(UserRole[] rolesToSelect)
        {
            SelectRoleForm form = new SelectRoleForm(rolesToSelect);
            form.ShowDialog();
            return form.SelectedRole;
        }


        public SelectRoleForm(IEnumerable<UserRole> rolesToSelect)
        {

            if (rolesToSelect == null)
                throw new ArgumentNullException(nameof(rolesToSelect));
            InitializeComponent();
            DialogResult = DialogResult.Abort;
            _buttonWidth = this.ClientSize.Width - _buttonsLocationOffset.X - 12;

            using (IEnumerator<UserRole> enumerator = rolesToSelect.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    int roleIndex = -1;
                    do
                    {
                        roleIndex++;
                        UserRole currentRole = enumerator.Current;
                        Button btn = _CreateButton(currentRole.ToString());
                        btn.Location = new Point(_buttonsLocationOffset.X + _buttonRowOffset.X * roleIndex
                                                , _buttonsLocationOffset.Y + _buttonRowOffset.Y * roleIndex);
                        btn.Tag = currentRole;
                        btn.Click += SelectRoleButton_Click;
                        Controls.Add(btn);
                    } while (enumerator.MoveNext());
                }
                else
                    throw new ArgumentException("Перечисление выбора ролей не может быть пустым", nameof(rolesToSelect));

            }
        }

        public UserRole? SelectedRole { get; private set; } = null;

        
        private Point _buttonRowOffset = new Point(0, 32);
        private Point _buttonsLocationOffset = new Point(15, 15);
        private int _buttonWidth;
        private int _buttonHeight = 30;
        private Font _buttonFont = new Font("DejaVu Sans Mono", 11.25f);

        private Button _CreateButton(string text)
        {
            return new Button()
            {
                Width = _buttonWidth,
                Height = _buttonHeight,
                Text = text,
                Font = _buttonFont
            };
        }

        private void SelectRoleButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            SelectedRole = (UserRole)btn.Tag;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
