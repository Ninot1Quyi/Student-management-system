using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            string id = Request.Form["txtID"];
            string password = Request.Form["txtPassword"];
            string userType = null;
            // 使用数据库进行身份验证
            if ((userType=AuthenticateUser(id, password) )!= null)
            {
               

                // 首先在管理员表中查找
                 
                if (userType == "Admin")
                {
                    // 管理员登录，设置会话变量
                    Session["UserType"] = userType;
                    Session["UserName"] = id;
                    Response.Redirect("Admin.aspx");
                    
                }

                // 其次在教师表中查找
                
                if (userType == "Teacher")
                {
                    // 老师登录，设置会话变量
                    Session["UserType"] = userType;
                    Session["UserName"] = id;
                    Response.Redirect("teacherHomePageaspx.aspx");
                    
                }

                // 最后在学生表中查找
                
                if (userType == "Student")
                {
                    // 学生登录，设置会话变量
                    Session["UserType"] = userType;
                    Session["UserName"] = id;
                    Response.Redirect("Student.aspx");
                   
                }

                
            }
            else
            {
                // 身份验证失败，显示错误信息
                string errorMessage = "账号或密码错误，请重新输入";

                lbshowmsg.Text = errorMessage;

                //ShowErrorMessage(errorMessage);
            }
        }
    }

    private string AuthenticateUser(string id, string password)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // 查询学生表
            string studentQuery = "SELECT COUNT(*) FROM Students WHERE StudentID = @ID AND StudentPassword = @Password";
            SqlCommand studentCommand = new SqlCommand(studentQuery, connection);
            studentCommand.Parameters.AddWithValue("@ID", id);
            studentCommand.Parameters.AddWithValue("@Password", password);

            // 查询教师表
            string teacherQuery = "SELECT COUNT(*) FROM Teachers WHERE TeacherID = @ID AND TeacherPassword = @Password";
            SqlCommand teacherCommand = new SqlCommand(teacherQuery, connection);
            teacherCommand.Parameters.AddWithValue("@ID", id);
            teacherCommand.Parameters.AddWithValue("@Password", password);

            // 查询管理员表
            string adminQuery = "SELECT COUNT(*) FROM Administrators WHERE AdminID = @ID AND AdminPassword = @Password";
            SqlCommand adminCommand = new SqlCommand(adminQuery, connection);
            adminCommand.Parameters.AddWithValue("@ID", id);
            adminCommand.Parameters.AddWithValue("@Password", password);

            connection.Open();

            // 首先检查学生表
            int studentCount = (int)studentCommand.ExecuteScalar();
            if (studentCount > 0)
            {
                connection.Close();
                return "Student";
            }

            // 然后检查教师表
            int teacherCount = (int)teacherCommand.ExecuteScalar();
            if (teacherCount > 0)
            {
                connection.Close();
                return "Teacher";
            }

            // 最后检查管理员表
            int adminCount = (int)adminCommand.ExecuteScalar();
            if (adminCount > 0)
            {
                connection.Close();
                return "Admin";
            }

            connection.Close();
            return null;
        }
    }

    private string GetUserTypeFromTable(string tableName, string id, string password)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string columNamePrefix = null;
            if (tableName == "Administrators")
            {
                columNamePrefix = "Admin";
            }
            else if (tableName == "Teachers")
            {
                columNamePrefix = "Teacher";
            }
            else if (tableName == "Students")
            {
                columNamePrefix = "Student";
            }

            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {columNamePrefix}ID = @ID AND {columNamePrefix}Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@Password", password);

            connection.Open();
            int count = (int)command.ExecuteScalar();
            connection.Close();

            if (count > 0)
            {
                return columNamePrefix;
            }

            return string.Empty;
        }
    }

    private void ShowErrorMessage(string message)
    {
        string script = $"<script>alert('{message}');</script>";
        ClientScript.RegisterStartupScript(GetType(), "ShowErrorMessage", script);
    }
}
