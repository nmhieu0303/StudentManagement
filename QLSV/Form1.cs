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

        public int getAge(DateTime birthDate)
        {
            return DateTime.Now.Year - birthDate.Year;
        }
        public bool isValidStudentID(string id, List<Student> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (id == list[i].id)
                {
                    return false;
                }
            }
            return true;
        }
        public bool isValidBirthDate(DateTime birthDate)
        {
            return getAge(birthDate) >= 17;
        }
        public bool isEmpty(Control.ControlCollection listControl)
        {
            int countChecked = 0;
            foreach (var ctl in listControl)
            {
                if (ctl is TextBox)
                {
                    TextBox txt = (TextBox)ctl;
                    if (string.IsNullOrEmpty(txt.Text))
                    {
                        return true;
                    }
                }
                if (ctl is ComboBox)
                {
                    ComboBox cbo = (ComboBox)ctl;
                    if (cbo.SelectedItem == null)
                    {
                        return true;
                    }
                } 
            }
            //Form have 2 checkboxs
            return countChecked == 0;
        }
        public bool isValidInfo(List<Student> listStudent)
        {
            if (!isValidStudentID(txtId.Text, listStudent) || String.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Your student ID isn't valid. It already exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Your student name is empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (!isValidBirthDate(dtmBirth.Value))
            {
                MessageBox.Show("Your birth date isn't valid. Please check again your information!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (cmbCity.SelectedIndex == 0)
            {
                MessageBox.Show("Your city isn't valid. Please check again your information!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (String.IsNullOrEmpty(txtScore.Text) || float.Parse(txtScore.Text) > 10.0f || float.Parse(txtScore.Text)<0)
            {
                MessageBox.Show("Your score isn't valid. Please check again your information!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        public bool isValidInfoDontCheckID(List<Student> listStudent)
        {
            if (String.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Your student ID isn't valid. It already exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Your student name is empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (!isValidBirthDate(dtmBirth.Value))
            {
                MessageBox.Show("Your birth date isn't valid. Please check again your information!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (cmbCity.SelectedIndex == 0)
            {
                MessageBox.Show("Your city isn't valid. Please check again your information!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (String.IsNullOrEmpty(txtScore.Text) || float.Parse(txtScore.Text) > 10.0f || float.Parse(txtScore.Text) < 0)
            {
                MessageBox.Show("Your score isn't valid. Please check again your information!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        public void refeshForm()
        {
            txtId.Text = "";
            txtName.Text = "";
            dtmBirth.Value = DateTime.Now;
            cmbCity.SelectedIndex = 0;
            txtScore.Text = "";
        }
        public void enableAll()
        {
            txtId.Enabled = true;
            txtName.Enabled = true;
            dtmBirth.Enabled = true;
            cmbCity.Enabled = true;
            txtScore.Enabled = true;
        }
        public void unableAll()
        {
            txtId.Enabled = false;
            txtName.Enabled = false;
            dtmBirth.Enabled = false;
            cmbCity.Enabled = false;
            txtScore.Enabled =  false;
        }
        private void showInfo(Student student)
        {
            refeshForm();
            txtId.Text = student.id;
            txtName.Text = student.name;
            dtmBirth.Value = student.birth;
            cmbCity.SelectedItem = student.born;
            txtScore.Text = student.score.ToString();
        }
        public Student getStudent()
        {
            if (isEmpty(pnlControl.Controls) && isValidInfo(students))
            {
                Student student = new Student
                {
                    id = txtId.Text,
                    name = txtName.Text,
                    birth = dtmBirth.Value,
                    born = cmbCity.Text,
                    score = float.Parse(txtScore.Text)
                };
                return student;
            }
            return null;
        }
        public Student getStudentDontCheckID()
        {
            if ( isValidInfoDontCheckID(students))
            {
                Student student = new Student
                {
                    id = txtId.Text,
                    name = txtName.Text,
                    birth = dtmBirth.Value,
                    born = cmbCity.Text,
                    score = float.Parse(txtScore.Text)
                };
                return student;
            }
            return null;
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
                            score = float.Parse(arrInfo[4])
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

        public void fillData(List<Student> listStudent, int option)
        {
            switch (option)
            {
                case 1:
                    dataGridViewStudents.DataSource = findStudentsByName(students, txtFill.Text);
                    break;
                case 2:
                    dataGridViewStudents.DataSource = findStudentsByAge(students, int.Parse(txtFill.Text));
                    break;
                case 3:
                    dataGridViewStudents.DataSource = findStudentsByCity(students, txtFill.Text);
                    break;
                case 4:
                    float score;
                    if (float.TryParse(txtFill.Text, out score))
                    {
                        dataGridViewStudents.DataSource = findStudentsByScore(students, score);
                    }
                    else dataGridViewStudents.DataSource = null;
                    break;
            }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
            cmbField.SelectedIndex = 0;
            cmbCity.SelectedIndex = 0;
            readFile(pathData, students);
            render(students);
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
            lblFunction.Text = "Info";
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
            if (rowIndex != -1 )
            {
                Student student = getStudentDontCheckID();
                if (student != null)
                {
                    students[rowIndex] = student;
                    writeFile(pathData, students);
                    render(students);
                    refeshForm();
                    btnSave.Hide();
                }
              
            }
           
           
        }

        private void dataGridViewStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblFunction.Text = "Info";
            unableAll();
            rowIndex = e.RowIndex;
            if (rowIndex!=-1) showInfo(students[rowIndex]);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            Student student = getStudent();
            if(student!=null)
            {
                refeshForm();
                students.Add(student);
                writeFile(pathData, student);
                render(students);
                btnAdd.Hide();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

            lblFunction.Text = "Insert";
            enableAll();
            refeshForm();
            btnAdd.Show();
            btnSave.Hide();
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            lblFunction.Text = "Edit";
            txtId.Enabled = false;
            btnAdd.Hide();
            btnSave.Show();
            enableAll();
        }

        private void cmbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillData(students, cmbField.SelectedIndex);
        }
        public  List<Student> findStudentsByCity(List<Student> listStudent, string city)
        {
            List<Student> listResult = new List<Student>();
            for (int i = 0; i < listStudent.Count; i++)
            {
                if ((listStudent[i].born).ToUpper().Contains(city.ToUpper()))
                {
                    listResult.Add(listStudent[i]);
                }
            }
            return listResult;
        }
        public List<Student> findStudentsByAge(List<Student> listStudent, int age)
        {
            List<Student> listResult = new List<Student>();
            for (int i = 0; i < listStudent.Count; i++)
            {
                if (getAge(listStudent[i].birth) == age)
                {
                    listResult.Add(listStudent[i]);
                }
            }
            return listResult;
        }
        public static List<Student> findStudentsByScore(List<Student> listStudent, float score)
        {
            List<Student> listResult = new List<Student>();
            for (int i = 0; i < listStudent.Count; i++)
            {
                if (listStudent[i].score == score)
                {
                    listResult.Add(listStudent[i]);
                }
            }
            return listResult;
        }
        public static List<Student> findStudentsByName(List<Student> listStudent, string name)
        {
            List<Student> listResult = new List<Student>();
            for (int i = 0; i < listStudent.Count; i++)
            {
                if ((listStudent[i].name).ToUpper().Contains(name.ToUpper()))
                {
                    listResult.Add(listStudent[i]);
                }
            }
            return listResult;
        }



        private void txtFill_KeyUp(object sender, KeyEventArgs e)
        {
            fillData(students, cmbField.SelectedIndex);
        }

        private void txtFill_TextChanged(object sender, EventArgs e)
        {
            lblFunction.Text = "Info";
        }
    }

}
