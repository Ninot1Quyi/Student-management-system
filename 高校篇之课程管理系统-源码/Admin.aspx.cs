using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class GridViewTemplate : ITemplate
{
    private DataControlRowType templateType;
    private string columnName;

    public GridViewTemplate(DataControlRowType type, string colName)
    {
        templateType = type;
        columnName = colName;
    }

    public void InstantiateIn(Control container)
    {
        switch (templateType)
        {
            case DataControlRowType.Header:
                CheckBox chkSelectAll = new CheckBox();
                chkSelectAll.ID = "chkSelectAll";
                chkSelectAll.AutoPostBack = true;
                chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
                container.Controls.Add(chkSelectAll);
                break;

            case DataControlRowType.DataRow:
                CheckBox chkSelect = new CheckBox();
                chkSelect.ID = "chkSelect";
                container.Controls.Add(chkSelect);
                break;

            default:
                break;
        }
    }

    private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelectAll = (CheckBox)sender;
        GridView gridView = (GridView)chkSelectAll.NamingContainer;
        foreach (GridViewRow row in gridView.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            chkSelect.Checked = chkSelectAll.Checked;
        }
    }

}

public partial class Admin : System.Web.UI.Page
{

    private string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        //处理session防止绕过登录
        if (Session["UserType"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            if (Session["UserType"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
            }
            btnUserName.Text = Session["UserName"].ToString();
            //ShowCoursesManagement();
        }

        if (!IsPostBack)
        {
            // 绑定教师下拉列表
            BindTeacherDropDownList();
            BindStudentsGridView();
            BindCoursesGridView();
            
            ShowCoursesManagement();
        }
        else//回发行为
        {
            
            string controlID = Request.Form["__EVENTTARGET"];

            // 判断触发 PostBack 的控件
            if (controlID == "ddlAddDepartment")
            {
               LoadMajorsByDepartment(ddlAddDepartment.SelectedValue);
            }else if (controlID == "btnTeacherSearch")
            {
                ShowTeachersManagement();
            }
            else if (controlID == "GridViewTeacher")
            {

                GridViewTeacher.PageIndexChanging += GridViewTeacher_PageIndexChanging;
                ShowTeachersManagement();
            }

        }


    }

    protected void lnkCoursesManagement_Click(object sender, EventArgs e)
    {
        // 处理课程管理链接的点击事件
        // 在此处添加你的代码

        ShowCoursesManagement();
    }
    protected void lnkStudentsManagement_Click(object sender, EventArgs e)
    {
        // 处理学生管理链接的点击事件
        // 在此处添加你的代码
        ShowStudentsManagement();
    }   
    
    protected void lnkTeachersManagement_Click(object sender, EventArgs e)
    {
        // 处理教师管理链接的点击事件
        // 在此处添加你的代码
        ShowTeachersManagement();   
    }
    


    protected void lnkResetTSPassword_Click(object sender, EventArgs e)
    {
        // 处理账号密码重置链接的点击事件
        // 在此处添加你的代码
        ShowResetTSPassword();  
    }

    protected void lnkResetSelfPassword_Click(object sender, EventArgs e)
    {
        // 处理修改密码链接的点击事件
        // 在此处添加你的代码
        ShowResetSelfPassword();//显示修改密码页面
    }

 
 

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        // 处理注销链接的点击事件
        // 在此处添加你的代码
        //回收session
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }

 

    protected void ShowCoursesManagement()
    {
    
        // 显示课程管理的内容，隐藏其他选项的内容
        pnlCoursesManagement.Visible = true;
        pnlStudentsManagement.Visible = false;
        pnlTeachersManagement.Visible = false;
        pnlResetTSPassword.Visible = false;
        pnlResetSelfPassword.Visible = false;
        PanelEditStudent.Visible = false;
        pnlEditCourse.Visible = false;
 
        pnlAddStudent.Visible = false;
    }

    protected void ShowStudentsManagement()
    {
        // 显示学生管理的内容，隐藏其他选项的内容
        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = true;
        pnlTeachersManagement.Visible = false;
        pnlResetTSPassword.Visible = false;
        pnlResetSelfPassword.Visible = false;
        PanelEditStudent.Visible = false;
        pnlEditCourse.Visible = false;
        pnlAddCourse.Visible = false;   
    }

   
    protected void ShowTeachersManagement()
    {

        // 构建 SQL 查询语句
        string sqlQuery = "SELECT t.TeacherID AS '教师号', t.TeacherName AS '姓名', t.TeacherAge AS '年龄', t.TeacherBirthday AS '出生日期', t.TeacherSex AS '性别', t.Title AS '职称' FROM Teachers t";

        // 执行 SQL 查询并获取结果
        DataTable dtTeachers = new DataTable();
        // 假设你已经有一个名为 "connectionString" 的有效数据库连接字符串
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dtTeachers);
        }

        // 将结果绑定到 GridView 控件
        GridViewTeacher.DataSource = dtTeachers;
        GridViewTeacher.DataBind();


        // 显示教师管理的内容，隐藏其他选项的内容
        GridViewTeacher.Visible = true;
        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = false;
        pnlTeachersManagement.Visible = true;
        pnlResetTSPassword.Visible = false;
        pnlResetSelfPassword.Visible = false;
        PanelEditStudent.Visible = false;
        pnlEditCourse.Visible = false;
        pnlAddCourse.Visible = false;
        pnlAddStudent.Visible = false;

        editTeacherDiv.Visible = false;
        gvUserInfo.Visible = false;
        search_Teacher.Visible = true;
        btnResetPassword.Visible = false;

        btnAddTeacher.Visible = true;
    }
    protected void ShowResetTSPassword()
    {
        // 显示账号密码重置的内容，隐藏其他选项的内容
        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = false;
        pnlTeachersManagement.Visible = false;
        pnlResetTSPassword.Visible = true;
        pnlResetSelfPassword.Visible = false;
        PanelEditStudent.Visible = false;
        pnlEditCourse.Visible = false;
        pnlAddCourse.Visible = false;
        pnlAddStudent.Visible=false;
    }

    protected void ShowResetSelfPassword()
    {
        // 显示修改密码的内容，隐藏其他选项的内容
        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = false;
        pnlTeachersManagement.Visible = false;
        pnlResetTSPassword.Visible = false;
        pnlResetSelfPassword.Visible = true;
        PanelEditStudent.Visible = false;
        pnlEditCourse.Visible = false;
        pnlAddCourse.Visible = false;
        pnlAddStudent.Visible = false;
    }

    protected void ShowPanelEditStudent()
    {
        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = false;
        pnlTeachersManagement.Visible = false;
        pnlResetTSPassword.Visible = false;
        pnlResetSelfPassword.Visible = false;
        PanelEditStudent.Visible = true;
        pnlEditCourse.Visible = false;
        pnlAddCourse.Visible = false;
        pnlAddStudent.Visible = false;
    }
    
    protected void ShowPanelEditCourse()
    {
        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = false;
        pnlTeachersManagement.Visible = false;
        pnlResetTSPassword.Visible = false;
        pnlResetSelfPassword.Visible = false;
        PanelEditStudent.Visible = false;
        pnlEditCourse.Visible = true;
        pnlAddCourse.Visible = false;
        pnlAddStudent.Visible = false;
    }

    

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        
        string currentPassword = txtCurrentPassword.Value.Trim();
        string newPassword = txtNewPassword.Value.Trim();
        string confirmPassword = txtConfirmPassword.Value.Trim();

        if (newPassword != confirmPassword)
        {
            // 新密码和确认密码不一致
            // 显示错误消息，不执行密码修改操作
            return;
        }
        
        string adminID = Session["UserName"].ToString();
        string query = "SELECT AdminPassword FROM Administrators WHERE AdminID = @AdminID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AdminID", adminID);

            connection.Open();
            string storedPassword = command.ExecuteScalar()?.ToString();

            if (currentPassword != storedPassword)
            {
                // 当前密码不匹配
                // 显示错误消息，不执行密码修改操作
                return;
            }
        }

        string updateQuery = "UPDATE Administrators SET AdminPassword = @NewPassword WHERE AdminID = @AdminID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@NewPassword", newPassword);
            command.Parameters.AddWithValue("@AdminID", adminID);

            connection.Open();
            command.ExecuteNonQuery();
        }

        txtCurrentPassword.Value = "";
        txtNewPassword.Value = "";
        txtConfirmPassword.Value = "";

        // 可以添加其他逻辑，例如显示密码修改成功的消息
    }
    //------------------------修改课程信息----------------------------
    protected void btnCourseSearch_Click(object sender, EventArgs e)
    {
        string keyword = txtCourseSearch.Text.Trim();

        // 执行课程搜索并重新绑定GridView数据源
        DataTable searchResults = SearchCourses(keyword);
        BindCoursesGridView(searchResults);
        ShowCoursesManagement(); // 根据你的实现来显示课程管理面板
    }

    private DataTable SearchCourses(string keyword)
    {
        DataTable searchResults = new DataTable();

        // 使用ADO.NET连接数据库并执行查询语句
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // 构建查询语句
            string query = @"SELECT C.CourseID, C.CourseDescription, C.CourseHours, C.CourseCredits, C.TeacherID, T.TeacherName, C.Address
                         FROM Courses AS C
                         JOIN Teachers AS T ON C.TeacherID = T.TeacherID
                         WHERE C.CourseID LIKE '%' + @Keyword + '%'
                         OR C.CourseDescription LIKE '%' + @Keyword + '%'
                         OR C.TeacherID LIKE '%' + @Keyword + '%'
                         OR T.TeacherName LIKE '%' + @Keyword + '%'
                         OR C.Address LIKE '%' + @Keyword + '%' ";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 添加查询参数
                command.Parameters.AddWithValue("@Keyword", keyword);

                // 打开数据库连接
                connection.Open();

                // 执行查询并填充结果到DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(searchResults);
            }
        }

        return searchResults;
    }

    protected void GridViewCourses_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        // 设置GridView的新页索引并重新绑定数据
        gvCourses.PageIndex = e.NewPageIndex;
        
        string keyword = txtCourseSearch.Text.Trim();
        DataTable searchResults = SearchCourses(keyword);
        BindCoursesGridView(searchResults);
    }
 
    protected void BindCoursesGridView(DataTable dt)
    {
        // 连接数据库，查询课程信息
 

        gvCourses.DataSource = dt;
        gvCourses.DataBind();
       
    }

    protected void BindCoursesGridView()
    {
        // 连接数据库，查询课程信息
    
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT c.CourseID, c.CourseDescription,t.TeacherID, t.TeacherName, c.Address, c.CourseCredits,c.CourseHours FROM Courses as c, Teachers as t WHERE c.TeacherID = t.TeacherID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }
    }

    protected void gvCourses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditCourse")
        {
            // 获取要编辑的课程ID
            
            // 从GridView中获取选定行的索引
            // 获取所点击行的索引
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            rowIndex %= gvCourses.PageSize;
            // 获取所选课程的数据
            GridViewRow row = gvCourses.Rows[rowIndex];
            string courseID = row.Cells[0].Text;
            string courseName = row.Cells[1].Text;
            string teacherID = row.Cells[2].Text;
            string teacherName = row.Cells[3].Text;
            string address = row.Cells[4].Text;
            string credits = row.Cells[5].Text;
            string hours = row.Cells[6].Text;
            // 将数据填充到面板中的控件
            txtCourseID.Text =  courseID;
            txtCourseName.Text =  courseName;
            txtTeacherID.Text = teacherID;
            txtTeacherName.Text = teacherName;
            txtAddress.Text = address;
            txtCredits.Text = credits;
            txtHours.Text = hours;

            ShowPanelEditCourse();


        }
        else if (e.CommandName == "DeleteCourse")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            rowIndex %= gvCourses.PageSize;
            // 获取所选课程的数据
            GridViewRow row = gvCourses.Rows[rowIndex];
            string courseID = row.Cells[0].Text;

            // 执行删除操作
            string deleteQuery = "DELETE FROM Courses WHERE CourseID = @CourseID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // 删除成功后，隐藏修改课程信息面板并刷新数据
            // 重新绑定GridView
            BindCoursesGridView();
            ShowCoursesManagement();
           

           
        }
    }
 

    protected void btnUpdateCourse_Click(object sender, EventArgs e)
    {
        string courseID = txtCourseID.Text;
        string teacherID = txtTeacherID.Text;
        string address = txtAddress.Text;
        int credits = Convert.ToInt32(txtCredits.Text); 
        int hours = Convert.ToInt32(txtHours.Text);
        // 执行更新操作
        string updateQuery = "UPDATE Courses SET TeacherID = @TeacherID, Address = @Address, CourseCredits = @CourseCredits,CourseHours = @CourseHours  WHERE CourseID = @CourseID";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@TeacherID", teacherID);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@CourseCredits", credits);
                command.Parameters.AddWithValue("@CourseID", courseID);
                command.Parameters.AddWithValue("@CourseHours", hours);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // 更新成功后，隐藏修改课程信息面板并刷新数据
        ShowCoursesManagement();
        BindCoursesGridView();
    }

    protected void btnAddCourse_Click(object sender, EventArgs e)
    {
        // 重定向到添加课程页面
        // 显示添加课程面板
        pnlAddCourse.Visible = true;
        pnlAddStudent.Visible = false;
    }
    private string GetTeacherNameByID(string teacherID)
    {
        string teacherName = string.Empty;

 

        // 构建 SQL 查询语句
        string query = "SELECT TeacherName FROM Teachers WHERE TeacherID = @TeacherID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 设置查询参数
                command.Parameters.AddWithValue("@TeacherID", teacherID);

                connection.Open();

                // 执行查询并获取结果
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    teacherName = result.ToString();
                }
            }
        }

        return teacherName;
    }
    private void BindTeacherDropDownList()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT TeacherID, TeacherName FROM Teachers";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // 绑定数据到下拉列表
                while (reader.Read())
                {
                    string teacherID = reader["TeacherID"].ToString();
                    string teacherName = reader["TeacherName"].ToString();
                    string listItemText = teacherID + " - " + teacherName;

                    ddlAddTeacherID.Items.Add(new ListItem(listItemText, teacherID));
                }

                // 添加一个空选项，以便用户不选择任何教师
                ddlAddTeacherID.Items.Insert(0, new ListItem("", ""));
            }
        }
    }

 

    protected void btnConfirmAdd_Click(object sender, EventArgs e)
    {
        // 获取输入的课程信息
        string courseID = txtAddCourseID.Text;
        string courseDescription = txtAddCourseDescription.Text;
        string teacherID = ddlAddTeacherID.SelectedValue;  // 使用下拉列表选择的教师ID
        string teacherName = GetTeacherNameByID(teacherID);  // 获取对应的教师姓名
        string address = txtAddAddress.Text;
        int courseCredits = Convert.ToInt32(txtCourseCredits.Text);
        int courseHours = Convert.ToInt32(txtCourseHours.Text);

        // 将课程信息添加到数据库中
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Courses (CourseID, CourseDescription, TeacherID,  Address, CourseCredits,CourseHours) VALUES (@CourseID, @CourseDescription, @TeacherID, @Address, @CourseCredits,@CourseHours)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CourseID", courseID);
                command.Parameters.AddWithValue("@CourseDescription", courseDescription);
                command.Parameters.AddWithValue("@TeacherID", teacherID);
                command.Parameters.AddWithValue("@TeacherName", teacherName);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@CourseCredits", courseCredits);
                command.Parameters.AddWithValue("@CourseHours", courseHours);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // 隐藏添加课程面板
        pnlAddCourse.Visible = false;

        // 刷新 GridView
        // 重新绑定数据源
        BindCoursesGridView(); // 根据你的实现来刷新 GridView 数据源
    }
    protected void btnCancelAddCourse_Click(object sender, EventArgs e)
    {
        pnlAddCourse.Visible = false;
    }

    //------------------------修改学生信息相关代码----------------------

    protected void btnSearchStudent_Click(object sender, EventArgs e)
    {
        string keyword = txtSearch.Text.Trim();

        // 执行模糊查询并重新绑定GridView数据源
        DataTable searchResults = PerformSearch(keyword);
        BindGridView(searchResults);
        ShowStudentsManagement();
    }

    private DataTable PerformSearch(string keyword)
    {
        DataTable searchResults = new DataTable();

        // 使用ADO.NET连接数据库并执行查询语句
         
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // 构建查询语句
            string query = @"SELECT S.StudentID, S.StudentName, S.StudentSex, S.StudentBirthday, S.AdmissionDate, D.DepartmentName,M.MajorName 
                FROM Students AS S
                JOIN Majors AS M ON S.MajorID = M.MajorID
                JOIN Departments AS D ON M.DepartmentID = D.DepartmentID
                WHERE S.StudentName LIKE '%' + @Keyword + '%'
                OR S.StudentID LIKE '%' + @Keyword + '%'
                OR S.StudentSex LIKE '%' + @Keyword + '%'
                OR M.MajorName LIKE '%' + @Keyword + '%'
                OR M.CounselorName LIKE '%' + @Keyword + '%'
                OR D.DepartmentName LIKE '%' + @Keyword + '%';";


            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 添加查询参数
                command.Parameters.AddWithValue("@Keyword", keyword);

                // 打开数据库连接
                connection.Open();

                // 执行查询并填充结果到DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(searchResults);
            }
        }

        return searchResults;
    }

    protected void GridViewStudents_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        // 设置GridView的新页索引并重新绑定数据
        GridViewStudents.PageIndex = e.NewPageIndex;
        string keyword = txtSearch.Text.Trim();
        DataTable searchResults = PerformSearch(keyword);
        BindGridView(searchResults);
    }
 

   
    private void BindGridView(DataTable data)
    {
        GridViewStudents.DataSource = data;
        GridViewStudents.DataBind();
    }

    protected void GridViewStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditStudent")
        {
            string studentID = e.CommandArgument.ToString();
            EditStudentPanel(studentID);
        }
        else if (e.CommandName == "DeleteStudent")
        {
            string studentID = e.CommandArgument.ToString();
            DeleteStudent(studentID);
            BindStudentsGridView();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string studentID = txtStudentID.Text;
        string studentName = txtStudentName.Text;
        string studentSex = rblStudentSex.SelectedValue;
        DateTime studentBirthday = Convert.ToDateTime(txtStudentBirthday.Text);
        DateTime admissionDate = Convert.ToDateTime(txtAdmissionDate.Text);
        string majorID = ddlMajor.SelectedValue;

        // 更新学生信息到数据库
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE Students SET  StudentName = @StudentName, StudentSex = @StudentSex, StudentBirthday = @StudentBirthday, AdmissionDate = @AdmissionDate, MajorID = @MajorID WHERE StudentID = @StudentID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentID", studentID);
                command.Parameters.AddWithValue("@StudentName", studentName);
                command.Parameters.AddWithValue("@StudentSex", studentSex);
                command.Parameters.AddWithValue("@StudentBirthday", studentBirthday);
                command.Parameters.AddWithValue("@AdmissionDate", admissionDate);
                command.Parameters.AddWithValue("@MajorID", majorID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // 隐藏修改面板并重新绑定数据
        PanelEditStudent.Visible = false;
        BindStudentsGridView();
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        PanelEditStudent.Visible = false;
        pnlEditCourse.Visible = false;
    }
  
    private void BindStudentsGridView()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT Students.StudentID, Students.StudentName, Students.StudentSex, Students.StudentBirthday, Students.AdmissionDate, Departments.DepartmentName,Majors.MajorName " +
                           "FROM Students " +
                           "INNER JOIN Majors ON Students.MajorID = Majors.MajorID " +
                           "INNER JOIN Departments ON Majors.DepartmentID = Departments.DepartmentID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtStudents = new DataTable();
                adapter.Fill(dtStudents);

                GridViewStudents.DataSource = dtStudents;
                GridViewStudents.DataBind();
            }
        }
    }

    //点击添加学生显示添加panel
    protected void btnAddStudent_Click(object sender, EventArgs e)
    {
        ShowStudentsManagement();
        InitializeAddStudentPage();
        pnlAddCourse.Visible = false;  
        pnlAddStudent.Visible = true;
       
    }
    protected void ddlAddDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 根据选择的院系加载对应的专业列表
        string selectedDepartmentID = ddlAddDepartment.SelectedValue;
       
        LoadMajorsByDepartment(selectedDepartmentID);
    }

    protected void btnConfirmAddStudent_Click(object sender, EventArgs e)
    {
        // 获取输入的学生信息
        string studentID = txtAddStudentID.Text.Trim();
        string studentName = txtAddStudentName.Text.Trim();
        string gender = ddlAddStudentGender.SelectedValue;
        string birthdayString = txtAddStudentBirthday.Text.Trim();
        string admissionDateString = txtAddAdmissionDate.Text.Trim();
        string departmentID = ddlAddDepartment.SelectedValue;
        string majorID = ddlAddMajor.SelectedValue;

        // 检查输入数据的合法性
        if (string.IsNullOrEmpty(studentID))
        {
            ShowErrorMessage("请输入学生ID");
            return;
        }

        if (!studentID.StartsWith("S"))
        {
            ShowErrorMessage("学生ID必须以字母'S'开头");
            return;
        }

        if (string.IsNullOrEmpty(studentName))
        {
            ShowErrorMessage("请输入学生姓名");
            return;
        }

        if (string.IsNullOrEmpty(birthdayString))
        {
            ShowErrorMessage("请输入出生日期");
            return;
        }

        if (string.IsNullOrEmpty(admissionDateString))
        {
            ShowErrorMessage("请输入入学日期");
            return;
        }

        DateTime birthday;
        if (!DateTime.TryParseExact(birthdayString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthday))
        {
            ShowErrorMessage("出生日期格式必须为yyyy-MM-dd");
            return;
        }

        DateTime admissionDate;
        if (!DateTime.TryParseExact(admissionDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out admissionDate))
        {
            ShowErrorMessage("入学日期格式必须为yyyy-MM-dd");
            return;
        }

        // 执行添加学生的操作
        AddStudent(studentID, studentName, gender, birthday, admissionDate, departmentID, majorID);

        // 刷新学生管理界面
        BindStudentsGridView();
    }

    private void ShowErrorMessage(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", $"alert('{message}');", true);
    }

    private void AddStudent(string studentID, string studentName, string gender, DateTime birthday, DateTime admissionDate, string departmentID, string majorID)
    {
        // 创建数据库连接
      
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // 构建 SQL 插入语句
            string sql = @"INSERT INTO Students (StudentID, StudentName, StudentSex, StudentBirthday, AdmissionDate, MajorID, StudentPassword)
                       VALUES (@StudentID, @StudentName, @StudentSex, @StudentBirthday, @AdmissionDate, @MajorID, @StudentPassword)";

            // 创建 SQL 命令对象
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                // 添加参数并设置值
                command.Parameters.AddWithValue("@StudentID", studentID);
                command.Parameters.AddWithValue("@StudentName", studentName);
                command.Parameters.AddWithValue("@StudentSex", gender);
                command.Parameters.AddWithValue("@StudentBirthday", birthday);
                command.Parameters.AddWithValue("@AdmissionDate", admissionDate);
                command.Parameters.AddWithValue("@MajorID", majorID);
                command.Parameters.AddWithValue("@StudentPassword", birthday.ToString("yyyyMMdd")); // 使用出生日期作为初始密码

                try
                {
                    // 打开数据库连接
                    connection.Open();

                    // 执行 SQL 命令
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // 处理异常
                    // 可以根据实际情况进行日志记录或其他处理
                    throw ex;
                }
            }
        }
    }

    
  
    private void InitializeAddStudentPage()
    {
        // 初始化院系列表
        LoadDepartments();

        // 初始化专业列表
        string initialDepartmentID = ddlAddDepartment.SelectedValue;
        LoadMajorsByDepartment(initialDepartmentID);
    }

    private void LoadDepartments()
    {
        // 从数据库加载院系列表并绑定到下拉列表框
        DataTable departments = GetDepartments(); // 从数据库中获取院系列表的方法，请根据实际情况实现
        ddlAddDepartment.DataSource = departments;
        ddlAddDepartment.DataTextField = "DepartmentName";
        ddlAddDepartment.DataValueField = "DepartmentID";
        ddlAddDepartment.DataBind();
    }

    private void LoadMajorsByDepartment(string departmentID)
    {
        // 根据选定的院系加载对应的专业列表
        DataTable majors = GetMajorsByDepartment(departmentID); // 从数据库中根据院系ID获取专业列表的方法，请根据实际情况实现
        ddlAddMajor.DataSource = majors;
        ddlAddMajor.DataTextField = "MajorName";
        ddlAddMajor.DataValueField = "MajorID";
        ddlAddMajor.DataBind();
    }
    // 获取院系列表
    private DataTable GetDepartments()
    {
        DataTable departments = new DataTable();
        // 连接数据库，执行查询操作获取院系列表
      
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT DepartmentID, DepartmentName FROM Departments";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(departments);
        }
        return departments;
    }

    // 根据院系ID获取专业列表
    private DataTable GetMajorsByDepartment(string departmentID)
    {
        DataTable majors = new DataTable();
        // 连接数据库，执行查询操作获取指定院系的专业列表
       
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT MajorID, MajorName FROM Majors WHERE DepartmentID = @DepartmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentID", departmentID);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(majors);
        }
        return majors;
    }

    private void EditStudentPanel(string studentID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT Students.StudentID, Students.StudentName, Students.StudentSex, Students.StudentBirthday, Students.AdmissionDate, Majors.DepartmentID, Majors.MajorID " +
                           "FROM Students " +
                           "INNER JOIN Majors ON Students.MajorID = Majors.MajorID " +
                           "WHERE Students.StudentID = @StudentID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentID", studentID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    txtStudentID.Text = reader["StudentID"].ToString();
                    txtStudentName.Text = reader["StudentName"].ToString();
                    rblStudentSex.SelectedValue = reader["StudentSex"].ToString();
                    txtStudentBirthday.Text = Convert.ToDateTime(reader["StudentBirthday"]).ToString("yyyy-MM-dd");
                    txtAdmissionDate.Text = Convert.ToDateTime(reader["AdmissionDate"]).ToString("yyyy-MM-dd");
                    LoadDepartmentsDropdownList(reader["DepartmentID"].ToString());
                    LoadMajorsDropdownList(reader["DepartmentID"].ToString(), reader["MajorID"].ToString());
                }

                reader.Close();
            }
        }

        ShowPanelEditStudent();
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        string departmentID = ddlDepartment.SelectedValue;
        LoadMajorsDropdownList(departmentID);
    }
    private void LoadMajorsDropdownList(string departmentID, string selectedMajorID = "")
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT MajorID, MajorName FROM Majors WHERE DepartmentID = @DepartmentID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DepartmentID", departmentID);

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtMajors = new DataTable();
                adapter.Fill(dtMajors);

                ddlMajor.DataSource = dtMajors;
                ddlMajor.DataTextField = "MajorName";
                ddlMajor.DataValueField = "MajorID";
                ddlMajor.DataBind();

                // 设置选中的项
                if (!string.IsNullOrEmpty(selectedMajorID))
                {
                    ddlMajor.SelectedValue = selectedMajorID;
                }
            }
        }
    }


    private void LoadDepartmentsDropdownList(string selectedDepartmentID = "")
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT DepartmentID, DepartmentName FROM Departments";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtDepartments = new DataTable();
                adapter.Fill(dtDepartments);

                ddlDepartment.DataSource = dtDepartments;
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();

                // 设置选中的项
                if (!string.IsNullOrEmpty(selectedDepartmentID))
                {
                    ddlDepartment.SelectedValue = selectedDepartmentID;
                }
            }
        }
    }

    private void DeleteStudent(string studentID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Students WHERE StudentID = @StudentID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentID", studentID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    //================================================================
    //---------------------------教师管理+账号密码重置------------------

    protected void btnTEdit_Command(object sender, CommandEventArgs e)
    {

    }

    /*protected void btnTDeleteTeacher_Command(object sender, CommandEventArgs e)
    {
        // 获取行索引
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        string studentID = e.CommandArgument.ToString();

        // 根据行索引获取对应的教师编号
        string teacherID = GridViewTeacher.DataKeys[rowIndex]["教师号"].ToString();

        // 执行 SQL DELETE 语句
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "DELETE FROM Teachers WHERE TeacherID = @TeacherID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@TeacherID", teacherID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // 执行完删除操作后，重新绑定 GridView 数据
        ShowTeachersManagement();s
    }*/
    private void DeleteTeacher(int row)
    {
        row = row % GridViewTeacher.PageSize;
        string teacherID = GridViewTeacher.DataKeys[row]["教师号"].ToString();

        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "DELETE FROM Teachers WHERE TeacherID = @TeacherID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@TeacherID", teacherID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    protected void BindTecherGridTView()
    {
        string sqlQuery = "SELECT t.TeacherID AS '教师号', t.TeacherName AS '姓名', t.TeacherAge AS '年龄', t.TeacherBirthday AS '出生日期', t.TeacherSex AS '性别', t.Title AS '职称' FROM Teachers t";
        DataTable dtTeachers = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dtTeachers);
        }
        GridViewTeacher.DataSource = dtTeachers;
        GridViewTeacher.DataBind();
        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = false;

        pnlResetTSPassword.Visible = false;
        pnlResetSelfPassword.Visible = false;
        editTeacherDiv.Visible = false;
        GridViewTeacher.Visible = true;
    }

    protected void GridViewTeacher_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Change")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewTeacher.EditIndex = rowIndex;
            pnlCoursesManagement.Visible = false;
            pnlStudentsManagement.Visible = false;
            pnlTeachersManagement.Visible = true;
            pnlResetTSPassword.Visible = false;
            pnlResetSelfPassword.Visible = false;
            editTeacherDiv.Visible = true;
            GridViewTeacher.Visible = false;

            // 获取对应行的数据
            string teacherID = GridViewTeacher.DataKeys[rowIndex]["教师号"].ToString();
            string teacherName = GridViewTeacher.Rows[rowIndex].Cells[1].Text;
            string teacherAge = GridViewTeacher.Rows[rowIndex].Cells[2].Text;
            string teacherBirthday = GridViewTeacher.Rows[rowIndex].Cells[3].Text;
            string teacherSex = GridViewTeacher.Rows[rowIndex].Cells[4].Text;
            string title = GridViewTeacher.Rows[rowIndex].Cells[5].Text;

            // 在文本框中显示对应的值
            txtEditName.Value = teacherName;
            txtEditSex.Value = teacherSex;
            txtEditBirthday.Value = teacherBirthday;
            txtEditTitle.Value = title;

            // 填充ddlEditDepartment下拉列表
            FillEditDepartmentDropdown();
        }
        else if(e.CommandName == "DeleteTeacher")
        {
            int row = int.Parse(e.CommandArgument.ToString());
            Response.Write(row);
            DeleteTeacher(row);
            ShowTeachersManagement();
        }
    }

    protected void btnTeacherUpdate_Click(object sender, EventArgs e)
    {
        int rowIndex = GridViewTeacher.EditIndex;
        if (rowIndex >= 0)
        {
            string teacherID = GridViewTeacher.DataKeys[rowIndex]["教师号"].ToString();
            string teacherName = txtEditName.Value;
            string teacherSex = txtEditSex.Value;
            string teacherBirthday = txtEditBirthday.Value;
            string title = txtEditTitle.Value;
            string department = ddlEditDepartment.SelectedValue; // 获取所选的系号

            // 执行 SQL UPDATE 语句
            string sql = "UPDATE Teachers SET TeacherName = @TeacherName, TeacherSex = @TeacherSex, TeacherBirthday = @TeacherBirthday, Title = @Title, DepartmentID = @Department WHERE TeacherID = @TeacherID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@TeacherName", teacherName);
                command.Parameters.AddWithValue("@TeacherSex", teacherSex);
                command.Parameters.AddWithValue("@TeacherBirthday", teacherBirthday);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Department", department);
                command.Parameters.AddWithValue("@TeacherID", teacherID);

                connection.Open();
                command.ExecuteNonQuery();
            }

            GridViewTeacher.EditIndex = -1;
            GridViewTeacher.EditIndex = rowIndex;
            pnlCoursesManagement.Visible = false;
            pnlStudentsManagement.Visible = false;
            pnlTeachersManagement.Visible = false;
            pnlResetTSPassword.Visible = false;
            pnlResetSelfPassword.Visible = false;
            editTeacherDiv.Visible = false;
            GridViewTeacher.Visible = true;

            ShowTeachersManagement();
        }
    }
    protected void GridViewTeacher_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 设置 GridView 的新页索引
        GridViewTeacher.PageIndex = e.NewPageIndex;

        // 重新绑定 GridView
        BindTecherGridTView();
    }
    List<int> selectedRows = new List<int>();
    protected void btnShowResetUserTable_Click(object sender, EventArgs e)
    {
        string userType = ddlUserType.SelectedValue;
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        string query = "";
   

        switch (userType)
        {
            case "student":
                query = "SELECT StudentID as ID,StudentName as  Name FROM Students";
               
                gvUserInfo.DataKeyNames = new string[] { "ID" };
                break;
            case "teacher":
                query = "SELECT TeacherID as ID,TeacherName as  Name  FROM Teachers";

                gvUserInfo.DataKeyNames = new string[] { "ID" };
                break;
            case "admin":
                query = "SELECT AdminID as ID,  AdminName as Name FROM Administrators";

                gvUserInfo.DataKeyNames = new string[] { "ID" };
                break;
        }

        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
        }
        // 添加 "chkSelectAll" 列（CheckBox 全选列）
        
        gvUserInfo.DataSource = dt;
        gvUserInfo.DataBind();
        // 恢复已选中行的状态
        /* foreach (GridViewRow row in gvUserInfo.Rows)
          {
              CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
              int rowIndex = row.RowIndex;
              // 根据已保存的选中行列表，重新设置复选框状态
              if (selectedRows.Contains(rowIndex))
              {
                  chkSelect.Checked = true;
              }
              else
              {
                  chkSelect.Checked = false;
              }
          }*/

        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = false;
        pnlTeachersManagement.Visible = false;
        pnlResetTSPassword.Visible = true;
        pnlResetSelfPassword.Visible = false;
        editTeacherDiv.Visible = false;
        GridViewTeacher.Visible = false;
        gvUserInfo.Visible = true;
        btnResetPassword.Visible = true;
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelectAll = (CheckBox)sender;

        selectedRows.Clear(); // 清空选中行列表

        foreach (GridViewRow row in gvUserInfo.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            chkSelect.Checked = chkSelectAll.Checked;

            if (chkSelectAll.Checked)
            {
                // 如果全选复选框被选中，则将当前行的索引保存到选中行列表中
                selectedRows.Add(row.RowIndex);
            }
        }
        pnlCoursesManagement.Visible = false;
        pnlStudentsManagement.Visible = false;
        pnlTeachersManagement.Visible = false;
        pnlResetTSPassword.Visible = true;
        pnlResetSelfPassword.Visible = false;
        editTeacherDiv.Visible = false;
        GridViewTeacher.Visible = false;
        gvUserInfo.Visible = true;
    }

    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        string userType = ddlUserType.SelectedValue;
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        string updateQuery = "";

        switch (userType)
        {
            case "student":
                updateQuery = "UPDATE Students SET StudentPassword = '123' WHERE StudentID = @StudentID";
                break;
            case "teacher":
                updateQuery = "UPDATE Teachers SET TeacherPassword = '123' WHERE TeacherID = @TeacherID";
                break;
            case "admin":
                updateQuery = "UPDATE Administrators SET AdminPassword = '123' WHERE AdminID = @AdminID";
                break;
        }

        foreach (GridViewRow row in gvUserInfo.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            if (chkSelect != null && chkSelect.Checked)
            {
                string userID = gvUserInfo.DataKeys[row.RowIndex].Value.ToString();
               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    if (userType == "student")
                        command.Parameters.AddWithValue("@StudentID", userID);
                    else if (userType == "teacher")
                        command.Parameters.AddWithValue("@TeacherID", userID);
                    else if (userType == "admin")
                        command.Parameters.AddWithValue("@AdminID", userID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        // Refresh the grid view
        btnShowResetUserTable_Click(sender, e);
    }
    protected void btnSearchTeacher_Click(object sender, EventArgs e)
    {
        string name = txtSearchName.Value.Trim();
        string gender = ddlSearchGender.SelectedValue;
        string title = txtSearchTitle.Value.Trim();

        // 构建查询语句
        string query = "SELECT  t.TeacherID AS '教师号', t.TeacherName AS '姓名', t.TeacherAge AS '年龄', t.TeacherBirthday AS '出生日期', t.TeacherSex AS '性别', t.Title AS '职称' FROM Teachers t WHERE 1=1"; // 1=1是为了方便后续拼接查询条件
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        if (!string.IsNullOrEmpty(name))
        {
            query += " AND TeacherName LIKE '%" + name + "%'";
        }

        if (!string.IsNullOrEmpty(gender))
        {
            query += " AND TeacherSex = '" + gender + "'";
        }

        if (!string.IsNullOrEmpty(title))
        {
            query += " AND Title LIKE '%" + title + "%'";
        }

        // 执行查询并重新绑定GridView
        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
        }

        GridViewTeacher.DataSource = dt;
        GridViewTeacher.DataBind();
        GridViewTeacher.Visible = true;
    }

    protected void btnAddTeacher_Click(object sender, EventArgs e)
    {
        divAddTeacher.Visible = true;
        FillDepartmentDropdown();
    }

    protected void btnSaveTeacher_Click(object sender, EventArgs e)
    {
        string teacherID = txtAddTeacherID.Value.Trim();
        string teacherName = txtAddTeacherName.Value.Trim();
        string teacherSex = ddlTeacherSex.SelectedValue;
        string teacherPassword = txtTeacherPassword.Value.Trim();
        string title = txtTitle.Value.Trim();
        string departmentID = ddlDepartment.SelectedValue;
        string teacherBirthdayString = txtTeacherBirthday.Text.Trim();

        // 检查输入数据的合法性
        if (string.IsNullOrEmpty(teacherID))
        {
            ShowErrorMessage("请输入教师ID");
            return;
        }

        if (!teacherID.StartsWith("T"))
        {
            ShowErrorMessage("教师ID必须以字母'T'开头");
            return;
        }

        if (string.IsNullOrEmpty(teacherName))
        {
            ShowErrorMessage("请输入教师姓名");
            return;
        }

        if (string.IsNullOrEmpty(teacherPassword))
        {
            ShowErrorMessage("请输入教师密码");
            return;
        }

        if (string.IsNullOrEmpty(title))
        {
            ShowErrorMessage("请输入教师职称");
            return;
        }

        if (string.IsNullOrEmpty(teacherBirthdayString))
        {
            ShowErrorMessage("请输入教师生日");
            return;
        }

        DateTime teacherBirthday;
        if (!DateTime.TryParseExact(teacherBirthdayString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out teacherBirthday))
        {
            ShowErrorMessage("教师生日格式必须为yyyy-MM-dd");
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Teachers (TeacherID, DepartmentID, TeacherName, TeacherSex, TeacherPassword, Title, TeacherBirthday) " +
                "VALUES (@TeacherID, @DepartmentID, @TeacherName, @TeacherSex, @TeacherPassword, @Title, @TeacherBirthday)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TeacherID", teacherID);
            command.Parameters.AddWithValue("@DepartmentID", departmentID);
            command.Parameters.AddWithValue("@TeacherName", teacherName);
            command.Parameters.AddWithValue("@TeacherSex", teacherSex);
            command.Parameters.AddWithValue("@TeacherPassword", teacherPassword);
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@TeacherBirthday", teacherBirthday);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // 保存完成后，隐藏添加教师的div
        divAddTeacher.Visible = false;
        ShowTeachersManagement();
    }

    private void FillDepartmentDropdown()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        string query = "SELECT DepartmentID FROM Departments";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            // 清空下拉列表
            dwlAddTDepartment.Items.Clear();

            // 添加选项
            while (reader.Read())
            {
                string departmentID = reader["DepartmentID"].ToString();
                dwlAddTDepartment.Items.Add(new ListItem(departmentID, departmentID));
            }

            // 关闭连接
            reader.Close();
        }
    }
    private void FillEditDepartmentDropdown()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        string query = "SELECT DepartmentID FROM Departments";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            // 清空下拉列表
            ddlEditDepartment.Items.Clear();
            // 添加选项
            while (reader.Read())
            {
                string departmentID = reader["DepartmentID"].ToString();
                ddlEditDepartment.Items.Add(new ListItem(departmentID, departmentID));
            }

            // 关闭连接
            reader.Close();
        }
    }

    //================================================================
}