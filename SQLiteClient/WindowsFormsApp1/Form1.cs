using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private const string DB_NAME = "members.db";
        private List<Member> members;

        public Form1()
        {
            InitializeComponent();

            members = new List<Member>();
            bsMembers.DataSource = members;
            dgvMembers.DataSource = bsMembers;

            InitDB();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.Name = txtName.Text;
            member.Age = int.Parse(txtAge.Text);

            bsMembers.Add(member);

            //データベースに保存する
            AddToDB(member);
        }

        //更新内容のDataGridViewへの保存
        private void AddToDB(Member member)
        {
            var sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = DB_NAME };

            using (var cn = new SQLiteConnection(sqlConnectionSb.ToString()))
            {
                //データベースファイルを開く（usingを使用しているので、自動で閉じます）
                cn.Open();

                //テーブル作成
                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = "INSERT into members(name, age) values(@name,@age)";
                    cmd.Parameters.Add(new SQLiteParameter("name", member.Name));
                    cmd.Parameters.Add(new SQLiteParameter("age", member.Age));

                    cmd.ExecuteNonQuery();                                                     //CrawlerData.dbの中にInfoTableができる
                }
            }
        }
        private void InitDB()
        {
            var sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = DB_NAME };

            using (var cn = new SQLiteConnection(sqlConnectionSb.ToString()))
            {
                //データベースファイルを開く（usingを使用しているので、自動で閉じます）
                cn.Open();

                //テーブル作成
                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS members(" +
                        "no INTEGER NOT NULL PRIMARY KEY," +
                        "name TEXT NOT NULL," +
                        "age INTEGER NOT NULL)";
                    cmd.ExecuteNonQuery();                                                     //CrawlerData.dbの中にInfoTableができる
                }

                //リストを空にする
                members.Clear();

                //データベースから取得した値を、InfoPattern1にします
                using (var cmd = new SQLiteCommand(cn))
                {
                    cmd.CommandText = "Select * from members";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            Member member = new Member();
                            member.Name = (string)reader["name"];
                            // SQLiteのINTEGERはlongなので、int 型への変換が必要
                            member.Age = (int)(long)reader["age"];
                            members.Add(member);
                        }
                        bsMembers.ResetBindings(false);
                    }
                }
            }
        }
    }
}
