using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<Member> members;

        public Form1()
        {
            InitializeComponent();

            members = new List<Member>();
            bsMembers.DataSource = members;
            dgvMembers.DataSource = bsMembers;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.Name = txtName.Text;
            member.Age = int.Parse(txtAge.Text);

            bsMembers.Add(member);
        }
    }
}
