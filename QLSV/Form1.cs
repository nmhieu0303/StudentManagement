using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class Form1 : Form
    {
        static string pathData = @"./../../data.txt";
        List<Student> students = new List<Student>();
        int rowIndex = -1;

        #region FUNCTION
        public void refeshForm()
        {
            txtId.Text = "";
            txtName.Text = "";
            chkMale.Checked = false;
            chkFemale.Checked = false;
            dtmBirth.Value = DateTime.Now;
            cmbCity.SelectedItem = "";
        }
        public void enableAll()
        {
            txtId.Enabled = true;
            txtName.Enabled = true;
            chkMale.Enabled = true;
            chkFemale.Enabled = true;
            dtmBirth.Enabled = true;
            cmbCity.Enabled = true;
        }

        public void unableAll()
        {
            txtId.Enabled = false;
            txtName.Enabled = false;
            chkMale.Enabled = false;
            chkFemale.Enabled = false;
            dtmBirth.Enabled = false;
            cmbCity.Enabled = false;
        }
        private void showInfo(Student student)
        {
            refeshForm();
            txtId.Text = student.id;
            txtName.Text = student.name;
            if (student.gender == "Male")
            {
                chkMale.Checked = true;
            }
            else chkFemale.Checked = true;
            dtmBirth.Value = student.birth;
            cmbCity.SelectedItem = student.born;
        }

        public Student getStudent()
        {
            Student student = new Student
            {
                id = txtId.Text,
                name = txtName.Text,
                birth = dtmBirth.Value,
                born = cmbCity.Text
            };
            if (chkMale.Checked) student.gender = "Male";
            else student.gender = "Female";
            return student;
        }
        public void render(List<Student> students)
        {
            dataGridViewStudents.DataSource = null;
            dataGridViewStudents.DataSource = students;
        }

        public List<Student> readFile(string pathFile, List<Student> students)
        {
            students.Clear();
            using (StreamReader sr = new StreamReader(pathData))
            {
                while (!sr.EndOfStream)
                {
                    var strLine = sr.ReadLine();
                    if (strLine.Length > 0)
                    {
                        var arrInfo = strLine.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        var sv = new Student
                        {
                            id = arrInfo[0],
                            name = arrInfo[1],
                            birth = DateTime.Parse(arrInfo[2]),
                            born = arrInfo[3],
                            gender = arrInfo[4]
                        };
                        students.Add(sv);
                    }
                }

            }
            return students;
        }

        public void writeFile(string filePath, Student student)
        {
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine(student.ToString());
            sw.Close();
        }
        public static void writeFile(string filePath, List<Student> listStudent)
        {
            File.WriteAllText(pathData, string.Empty);
            StreamWriter sw = new StreamWriter(filePath);
            for (int i = 0; i < listStudent.Count; i++)
            {
                sw.WriteLine(listStudent[i].ToString());
            }
            sw.Close();
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
            cmbField.SelectedIndex = 0;
            btnSave.Hide();
            btnAdd.Hide();
        }


      


        private void btnLoad_Click(object sender, EventArgs e)
        {
            readFile(pathData, students);
            render(students);
        }



        private void btnEdit_Click(object sender, EventArgs e)
        {
            pnlLB.Enabled = true;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (rowIndex != -1)
            {
                var dlr = MessageBox.Show("Remove Student", "Do you want remove student?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dlr == DialogResult.OK)
                { 
                    students.RemoveAt(rowIndex);
                    rowIndex = -1;                   
                    writeFile(pathData, students);
                    render(students);
                    refeshForm();
                }
            }
       
        }

 
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (rowIndex != -1)
            {
                Student student = getStudent();
                students[rowIndex] = student;
                writeFile(pathData, students);
                render(students);
                refeshForm();
                btnSave.Hide();
            }
           
           
        }

        private void dataGridViewStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }





        private void dataGridViewStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            unableAll();
            rowIndex = e.RowIndex;
            if (rowIndex!=-1) showInfo(students[rowIndex]);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            Student student = getStudent();
            refeshForm();
            students.Add(student);
            writeFile(pathData, student);
            render(students);
            btnAdd.Hide();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            enableAll();
            refeshForm();
            btnAdd.Show();
            btnSave.Hide();
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            btnAdd.Hide();
            btnSave.Show();
            enableAll();
        }


    }
}
