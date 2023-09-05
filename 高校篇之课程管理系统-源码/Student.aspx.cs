using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student : System.Web.UI.Page
{
    private string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
    private string studentID;//学生ID
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            if (Session["UserName"] == null)
            {
                // 用户未登录，重定向到登录页面或执行其他操作
                Response.Redirect("Login.aspx");
                return;
            }

            // 用户已登录
            //Response.Write(Session["UserName"]);
            btnUserName.Text = Session["UserName"].ToString();
            studentID = Session["UserName"].ToString();
            
            // 默认显示所有课程
            ShowAllCourses();
        }
        else
        {
            // 重新绑定数据后，需要重新设置已选课程行的按钮状态
            List<string> enrolledCourses = GetEnrolledCourses();
            SetEnrolledCoursesButtonState(enrolledCourses);
        }
        // 注册分页事件处理程序
        gridAllCourses.PageIndexChanging += gridAllCourses_PageIndexChanging;
    }

    private List<string> GetEnrolledCourses()
    {
        List<string> enrolledCourses = new List<string>();

        string enrolledCoursesQuery = "SELECT CourseID FROM CourseEnrollment WHERE StudentID = @StudentID";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand enrolledCoursesCommand = new SqlCommand(enrolledCoursesQuery, connection);
            enrolledCoursesCommand.Parameters.AddWithValue("@StudentID", GetStudentID());

            connection.Open();
            SqlDataReader reader = enrolledCoursesCommand.ExecuteReader();

            while (reader.Read())
            {
                string courseID = reader["CourseID"].ToString();
                enrolledCourses.Add(courseID);
            }
        }

        return enrolledCourses;
    }

    private void SetEnrolledCoursesButtonState(List<string> enrolledCourses)
    {
        foreach (GridViewRow row in gridAllCourses.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                string courseID = row.Cells[0].Text; // 获取第0列的值，即CourseID

                if (enrolledCourses.Contains(courseID))
                {
                    LinkButton btnSelectCourse = (LinkButton)row.FindControl("btnSelectCourse");
                    btnSelectCourse.Enabled = false;
                    btnSelectCourse.Text = "已选";
                    btnSelectCourse.CssClass = "btn btn-secondary disabled btn-sm";
                }
            }
        }
    }


    protected void ShowAllCourses()
    {
        // 查询并绑定全部课程
        string query = "SELECT CourseID, CourseDescription, CourseCredits, TeacherName FROM Courses AS c, Teachers AS t WHERE c.TeacherID = t.TeacherID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);

            gridAllCourses.DataSource = dataTable;
            gridAllCourses.DataBind();
        }

        // 获取学生已选课程的CourseID列表
        List<string> enrolledCourses = GetEnrolledCourses();
        // 设置已选课程行的按钮为灰色且不可点击
        SetEnrolledCoursesButtonState(enrolledCourses);

        pnlAllCourses.Visible = true;
        pnlEnrolledCourses.Visible = false;
        pnlChangePassword.Visible = false;
    }




    protected void ShowEnrolledCourses()
    {
        string studentID = GetStudentID();
        string query = "SELECT C.CourseID, C.CourseDescription, C.CourseCredits, E.Grade FROM Courses C INNER JOIN CourseEnrollment E ON C.CourseID = E.CourseID WHERE E.StudentID = @StudentID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentID", studentID);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);

            gridEnrolledCourses.DataSource = dataTable;
            gridEnrolledCourses.DataBind();
        }

        pnlAllCourses.Visible = false;
        pnlEnrolledCourses.Visible = true;
        pnlChangePassword.Visible = false;
        pnlAssignments.Visible = false;
        pnlExperiments.Visible = false;
    }

    protected void ChangePassword()
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

        string studentID = GetStudentID();
        string query = "SELECT StudentPassword FROM Students WHERE StudentID = @StudentID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentID", studentID);

            connection.Open();
            string storedPassword = command.ExecuteScalar()?.ToString();

            if (currentPassword != storedPassword)
            {
                // 当前密码不匹配
                // 显示错误消息，不执行密码修改操作
                return;
            }
        }

        string updateQuery = "UPDATE Students SET StudentPassword = @NewPassword WHERE StudentID = @StudentID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@NewPassword", newPassword);
            command.Parameters.AddWithValue("@StudentID", studentID);

            connection.Open();
            command.ExecuteNonQuery();
        }

        txtCurrentPassword.Value = "";
        txtNewPassword.Value = "";
        txtConfirmPassword.Value = "";

        // 可以添加其他逻辑，例如显示密码修改成功的消息
    }

    protected string GetStudentID()
    {
        // 这里根据实际情况获取学生ID，例如从登录信息或会话中获取
        // 可以自行修改为你的实际逻辑
        return Session["UserName"].ToString();
    }

    protected void lnkAllCourses_Click(object sender, EventArgs e)
    {
        ShowAllCourses();
    }

    protected void lnkEnrolledCourses_Click(object sender, EventArgs e)
    {
        ShowEnrolledCourses();
    }

    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
        ShowChangePassword();
    }
    protected void lnkLoginOut_Click(object sender, EventArgs e)
    {
        // 销毁当前会话
        Session.Abandon();

        // 执行其他操作，例如重定向到登录页面或显示退出成功消息等
        Response.Redirect("Login.aspx");
    }

    protected void ShowChangePassword()
    {
        // 显示修改密码的内容，隐藏其他选项的内容
        pnlAllCourses.Visible = false;
        pnlEnrolledCourses.Visible = false;
        pnlChangePassword.Visible = true;
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        ChangePassword();
    }
    // 检查学生是否已经选过该门课程
    protected void gridAllCourses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SelectCourse")
        {
            string courseID = e.CommandArgument.ToString();
            string studentID = GetStudentID();

            // 检查学生是否已经选过该门课程
            if (IsCourseEnrolled(courseID, studentID))
            {
                // 学生已经选过该门课程，显示错误消息或执行其他逻辑
                // ...
                return;
            }

            // 添加选课信息到选课表
            if (EnrollCourse(courseID, studentID))
            {
                // 选课成功，刷新已选课程列表
                ShowEnrolledCourses();
            }
            else
            {
                // 选课失败，显示错误消息或执行其他逻辑
                // ...
            }
        }
    }

    protected void gridEnrolledCourses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
         
        if (e.CommandName == "DropCourse")
        {
            string courseID = e.CommandArgument.ToString();
            string studentID = GetStudentID();
            // 检查学生是否已经选过该门课程
            if (IsCourseEnrolled(courseID, studentID))
            {
                // 学生已经选过该门课程，显示错误消息或执行其他逻辑
                // ...
                //学过这么课之后将其删除
                
                if (DropCourse(courseID,studentID))
                {
                     
                    //删除成功
                    ShowEnrolledCourses();
                }
                else
                {
                    Response.Write("删除失败");
                    // 删除失败，显示错误消息或执行其他逻辑
                    // ...
                }
            }


        }
        else if (e.CommandName == "ViewAssignments")
        {
            string courseID = e.CommandArgument.ToString();


            // 根据课程ID执行显示作业的操作
            // 查询对应课程的作业信息
      

            // 根据课程ID查询课程名称
            string courseName = string.Empty;
            string queryCourse = "SELECT CourseDescription FROM Courses WHERE CourseID = @CourseID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryCourse, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        courseName = reader["CourseDescription"].ToString();
                    }
                    reader.Close();
                }
            }

            lbHomeWorkFromName.Text = "当前课程:  "+courseName;

            // 根据课程ID执行显示作业的操作
            // 查询对应课程的作业信息
            string queryAssignments = "SELECT AssignmentID, AssignmentName, Deadline, Description FROM Assignments WHERE CourseID = @CourseID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryAssignments, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseID);
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gridAssignments.DataSource = dt;
                    gridAssignments.DataBind();
                    // 显示作业Panel，隐藏其他Panel
                    pnlAssignments.Visible = true;
                    pnlExperiments.Visible = false;
                }
            }
        }
        else if (e.CommandName == "ViewExperiments")
        {
            string courseID = e.CommandArgument.ToString();
           


            // 根据课程ID执行显示作业的操作
            // 查询对应课程的作业信息


            // 根据课程ID查询课程名称
            string courseName = string.Empty;
            string queryCourse = "SELECT CourseDescription FROM Courses WHERE CourseID = @CourseID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryCourse, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        courseName = reader["CourseDescription"].ToString();
                    }
                    reader.Close();
                }
            }

            lbExFromName.Text = "当前课程:  " + courseName;
            // 根据课程ID执行显示实验的操作
            // 查询对应课程的实验信息
            string query = "SELECT ExperimentID, ExperimentName, ExperimentDate, Description FROM Experiments WHERE CourseID = @CourseID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        // 将实验数据绑定到GridView
                        gridExperiments.DataSource = reader;
                        gridExperiments.DataBind();
                        // 显示实验Panel，隐藏其他Panel
                        pnlExperiments.Visible = true;
                        pnlAssignments.Visible = false;
                    }
                    else
                    {
                        // 如果没有实验信息，您可以显示相应的提示或执行其他操作
                    }
                    reader.Close();
                }
            }
        }

    }

        // 检查学生是否已经选过该门课程
    private bool IsCourseEnrolled(string courseID, string studentID)
    {
        bool isEnrolled = false;
        string query = "SELECT COUNT(*) FROM CourseEnrollment WHERE CourseID = @CourseID AND StudentID = @StudentID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CourseID", courseID);
            command.Parameters.AddWithValue("@StudentID", studentID);

            connection.Open();
            int count = (int)command.ExecuteScalar();

            if (count > 0)
            {
                isEnrolled = true;
            }
        }

        return isEnrolled;
    }

    // 将选课信息添加到选课表
    private bool EnrollCourse(string courseID, string studentID)
    {
        bool isEnrolled = false;
        string query = "INSERT INTO CourseEnrollment (CourseID, StudentID) VALUES (@CourseID, @StudentID)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CourseID", courseID);
            command.Parameters.AddWithValue("@StudentID", studentID);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                isEnrolled = true;
            }
        }

        return isEnrolled;
    }

    // 把选课信息从选课表中删除
    private bool DropCourse(string courseID, string studentID) {

        bool isDroped = false;
        string query = "DELETE  FROM CourseEnrollment WHERE StudentID= @StudentID AND CourseID= @CourseID";
         
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
             
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CourseID", courseID);
            command.Parameters.AddWithValue("@StudentID", studentID);
            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                isDroped = true;
            }
            
        }
        return isDroped;
    }

    protected void btnAllCoursesSearch_Click(object sender, EventArgs e)
    {
        Response.Write("点击查询");
        string searchKeyword = txtSearch.Text.Trim();
        Response.Write("\n查询内容：" + searchKeyword + "\n");
        // 根据搜索关键字修改查询语句，例如使用 WHERE 子句过滤结果
        string query = "SELECT CourseID, CourseDescription, CourseCredits, TeacherName FROM Courses as c, Teachers as t WHERE c.TeacherID = t.TeacherID AND CourseDescription LIKE '%' + @SearchKeyword + '%'";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SearchKeyword", searchKeyword);
            Response.Write("指令");
           
            //打印带参数的command sql语句

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);
            // 添加查询结果数据
            gridAllCourses.DataSource = dataTable;
            gridAllCourses.DataBind();
        }

        pnlAllCourses.Visible = true;
        pnlEnrolledCourses.Visible = false;
        pnlChangePassword.Visible = false;
    }
    private string GetParameterizedQuery(SqlCommand command)
{
    string query = command.CommandText;
    foreach (SqlParameter parameter in command.Parameters)
    {
        query = query.Replace(parameter.ParameterName, "'" + parameter.Value.ToString() + "'");
    }
    return query;
}
 


    protected void gridAllCourses_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridAllCourses.PageIndex = e.NewPageIndex;
        // 重新绑定 GridView
        ShowAllCourses();
    }

    protected void btnEnrolledCoursesSearch_Click(object sender, EventArgs e)
    {
        string searchKeyword = txtEnrolledSearch.Text.Trim();
        string studentID = GetStudentID();
        string query = "SELECT C.CourseID, C.CourseDescription, C.CourseCredits,E.Grade FROM Courses C INNER JOIN CourseEnrollment E ON C.CourseID = E.CourseID WHERE E.StudentID = @StudentID AND C.CourseDescription LIKE '%' + @searchKeyword + '%'";
        //Response.Write(studentID);
        //Response.Write(searchKeyword);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StudentID", studentID);
            command.Parameters.AddWithValue("@searchKeyword", searchKeyword);
           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);

            gridEnrolledCourses.DataSource = dataTable;
            gridEnrolledCourses.DataBind();
        }
    }



}
