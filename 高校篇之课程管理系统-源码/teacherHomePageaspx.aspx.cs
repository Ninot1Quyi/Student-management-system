using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
 

public partial class teacherHomePageaspx : System.Web.UI.Page
{
    private string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (!IsPostBack)
        {
            showGrid1();
            Teacher.SelectParameters["UserName"].DefaultValue = Session["UserName"].ToString();
            GridView1.Visible = true;
            Teacher_select.Visible = true;
            pnlAssignments.Visible = false;
            pnlAssignAssignment.Visible = false;
            pnlExperiments.Visible = false;
            pnlAssignExperiment.Visible = false;
        }

    }

    protected void refreshAssignments(object sender, GridViewCommandEventArgs e)
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

        lbHomeWorkFromName.Text = "当前课程:  " + courseName;

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
                Teacher_select.Visible = false;
                pnlAssignments.Visible = true;
                pnlExperiments.Visible = false;
                pnlAssignAssignment.Visible = false;
                pnlAssignExperiment.Visible = false;
              
            }
        }
    }

    protected void refreshExperiments(object sender, GridViewCommandEventArgs e)
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
                    Teacher_select.Visible = false;
                    pnlExperiments.Visible = true;
                    pnlAssignments.Visible = false;
                    pnlAssignAssignment.Visible = false;
                    pnlAssignExperiment.Visible = false;
                }
                else
                {
                    // 如果没有实验信息，您可以显示相应的提示或执行其他操作
                }
                reader.Close();
            }
        }
    }


    protected void gridEnrolledCourses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DropAssignment")
        {
           
            string assignmentID = e.CommandArgument.ToString();
            string queryCourse = " DELETE FROM Assignments WHERE AssignmentID = @AssignmentID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryCourse, connection))
                {
                    command.Parameters.AddWithValue("@AssignmentID", assignmentID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            refreshAssignments(sender, e);
            pnlAssignments.Visible=false;
        }
        else if (e.CommandName == "DropExperiment")
        {

            string assignmentID = e.CommandArgument.ToString();
            string queryCourse = " DELETE FROM Experiments WHERE ExperimentID = @ExperimentID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryCourse, connection))
                {
                    command.Parameters.AddWithValue("@ExperimentID", assignmentID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            refreshExperiments(sender, e);
            pnlExperiments.Visible=false;
        }
        else if (e.CommandName == "ViewAssignments")
        {
            refreshAssignments(sender, e);
        }
        else if (e.CommandName == "ViewExperiments")
        {
            refreshExperiments(sender, e);  
        }

    }


    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }

    
    [WebMethod]
    public static void ChangePassword(string userName, string newPassword)
    {
        Console.WriteLine("ChangePassword方法被调用");

        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string updateQuery = "UPDATE Teachers SET TeacherPassword = @Password WHERE TeacherID = @UserName";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@Password", newPassword);
                command.Parameters.AddWithValue("@UserName", userName);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("密码修改成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("密码修改失败: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }







    protected void ViewDetails_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string courseID = btn.CommandArgument.ToString();

        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT DISTINCT s.StudentID 学号, s.StudentName 姓名, sc.Grade 课程成绩,  sc.CourseID 课程编号 FROM CourseEnrollment sc, Students s, Teachers t WHERE sc.StudentID = s.StudentID AND sc.CourseID = @CourseID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CourseID", courseID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                Teacher_select.DataSource = dt;
                Teacher_select.DataBind();

                GridView1.Visible = false;

            }
        }
    }

    protected void myclassOnlick(object sender, EventArgs e)
    {
        showGrid1();
    }

    private void showGrid1()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        string query = @"SELECT Courses.CourseID AS 课程编号, Courses.CourseDescription AS 课程名, Courses.CourseHours AS 课时, Courses.CourseCredits AS 学分
                     FROM Teachers, Courses
                     WHERE Courses.TeacherID = Teachers.TeacherID
                     AND Teachers.TeacherID = @UserName";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", Session["UserName"].ToString()); 
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                GridView1.DataSource = reader;
                GridView1.DataBind();
            }
        }
        GridView1.Visible = true;
        Teacher_select.Visible = false;
    }


    [WebMethod]
    public static void ChangeGrade(string userName, string newPassword, string courseID)
    {
        Console.WriteLine("ChangeGrade方法被调用");

        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string updateQuery = "UPDATE CourseEnrollment SET Grade = @Grade WHERE StudentID = @StudentID AND CourseID = @CourseID";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@Grade", newPassword);
                command.Parameters.AddWithValue("@StudentID", userName);
                command.Parameters.AddWithValue("@CourseID", courseID);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("成绩修改成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("成绩修改失败: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
    protected void AssignAssignment_Click(object sender, EventArgs e)
    {
        // 获取作业相关的课程编号
        Button btnAssign = (Button)sender;
        GridViewRow row = (GridViewRow)btnAssign.NamingContainer;
        string courseID = GridView1.DataKeys[row.RowIndex].Value.ToString();

        // 将课程编号存储在 Session 中，以便在 btnAssign_Click 方法中使用
        Session["SelectedCourseID"] = courseID;

        // 显示作业布置面板
        Teacher_select.Visible = false;
        pnlAssignments.Visible = true;
        pnlAssignAssignment.Visible = true;
        pnlExperiments.Visible = false;
        pnlAssignExperiment.Visible = false;
    }


    protected void btnAssign_Click(object sender, EventArgs e)
    {
        // 获取作业信息
        string assignmentID = txtAssignmentID.Text;
        string assignmentName = txtAssignmentName.Text;
        string deadline = txtDeadline.Text;
        string description = txtDescription.Text;

        // 获取存储的课程编号
        string courseID = Session["SelectedCourseID"].ToString();

        // 执行插入数据库的操作，将作业信息插入 Assignments 表中，包括课程编号
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Assignments (AssignmentID, AssignmentName, Deadline, Description, CourseID) VALUES (@AssignmentID, @AssignmentName, @Deadline, @Description, @CourseID)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AssignmentID", assignmentID);
                command.Parameters.AddWithValue("@AssignmentName", assignmentName);
                command.Parameters.AddWithValue("@Deadline", deadline);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@CourseID", courseID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // 隐藏作业布置面板
        pnlAssignAssignment.Visible = false;

        // 清空输入框
        txtAssignmentID.Text = string.Empty;
        txtAssignmentName.Text = string.Empty;
        txtDeadline.Text = string.Empty;
        txtDescription.Text = string.Empty;

        // 刷新 GridView 或执行其他操作
        showGrid1();
        pnlAssignments.Visible = false;
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // 隐藏作业布置面板
        pnlAssignAssignment.Visible = false;

        // 清空输入框
        txtAssignmentID.Text = string.Empty;
        txtAssignmentName.Text = string.Empty;
        txtDeadline.Text = string.Empty;
        txtDescription.Text = string.Empty;

        // 刷新 GridView 或执行其他操作
        showGrid1();
    }

    protected void AssignExperiment_Click(object sender, EventArgs e)
    {
        // 获取课程编号和显示实验布置面板
        Button btnAssignExperiment = (Button)sender;
        GridViewRow row = (GridViewRow)btnAssignExperiment.NamingContainer;
        string courseID = GridView1.DataKeys[row.RowIndex].Value.ToString();
        Session["SelectedCourseID"] = courseID;
        Teacher_select.Visible = false;
        pnlAssignments.Visible = false;
        pnlAssignAssignment.Visible = false;
        pnlExperiments.Visible = true;
        pnlAssignExperiment.Visible = true;
        
    }

    protected void btnAssignExperiment_Click(object sender, EventArgs e)
    {
        // 获取实验信息
        string experimentID = txtExperimentID.Text;
        string experimentName = txtExperimentName.Text;
        string ExperimentDate = txtExperimentDeadline.Text;
        string description = txtExperimentDescription.Text;
        string courseID = Session["SelectedCourseID"].ToString();
        // 执行插入数据库的操作，将实验信息插入Experiments表中
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Experiments (ExperimentID, ExperimentName, ExperimentDate, Description, CourseID) VALUES (@ExperimentID, @ExperimentName, @ExperimentDate, @Description, @CourseID)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ExperimentID", experimentID);
                command.Parameters.AddWithValue("@ExperimentName", experimentName);
                command.Parameters.AddWithValue("@ExperimentDate", ExperimentDate);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@CourseID", courseID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // 隐藏实验布置面板
        pnlAssignExperiment.Visible = false;

        // 清空输入框
        txtExperimentID.Text = string.Empty;
        txtExperimentName.Text = string.Empty;
        txtExperimentDeadline.Text = string.Empty;
        txtExperimentDescription.Text = string.Empty;

        // 刷新 GridView 或执行其他操作
        showGrid1();
        pnlExperiments.Visible = false;
    }

    protected void btnCancelExperiment_Click(object sender, EventArgs e)
    {
        // 隐藏实验布置面板
        pnlAssignExperiment.Visible = false;

        // 清空输入框
        txtExperimentID.Text = string.Empty;
        txtExperimentName.Text = string.Empty;
        txtExperimentDeadline.Text = string.Empty;
        txtExperimentDescription.Text = string.Empty;

        // 刷新 GridView 或执行其他操作
        showGrid1();
    }



}