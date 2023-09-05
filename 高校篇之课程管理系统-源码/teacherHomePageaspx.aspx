<%@ Page Language="C#" AutoEventWireup="true" CodeFile="teacherHomePageaspx.aspx.cs" Inherits="teacherHomePageaspx"  EnableEventValidation="true"%>
<pages enableEventValidation="true" />


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>课程信息显示</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <!-- 引入bootstrap 5 -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/css/bootstrap.min.css"/>
    <!-- 引入JQuery和bootstrap.js -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/js/bootstrap.bundle.min.js"></script>


    <script>
        function changePassword() {
            // 获取新密码和确认密码输入框的值
            var newPassword = document.getElementById("txtNewPassword").value;
            var confirmPassword = document.getElementById("txtConfirmPassword").value;

            // 检查密码和确认密码是否一致
            if (newPassword !== confirmPassword) {
                alert("密码和确认密码不一致");
                return;
            }

            // 发起 AJAX 请求执行密码更新操作
            var userName = '<%= Session["UserName"] %>';

            $.ajax({
                type: "POST",
                url: "teacherHomePageaspx.aspx/ChangePassword",
                data: JSON.stringify({ userName: userName, newPassword: newPassword }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // 更新成功后刷新页面或执行其他操作
                    alert("密码修改成功");
                    window.location.reload();
                },
                error: function (xhr, status, error) {
                    // 处理错误情况
                    alert("密码修改失败");
                }
            });
        }
    </script>
   <script>
       function updateGrade(studentID, courseID) {
           var newGrade = prompt("请输入新的成绩：");
           if (newGrade != null) {
               // 发起 AJAX 请求执行成绩更新操作
               var data = {
                   userName: studentID,
                   newPassword: newGrade,
                   courseID: courseID
               };

               $.ajax({
                   type: "POST",
                   url: "teacherHomePageaspx.aspx/ChangeGrade",
                   data: JSON.stringify(data),
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (response) {
                       // 成绩更新成功后刷新页面或执行其他操作
                       alert("成绩修改成功");
                       window.location.reload();
                   },
                   error: function (xhr, status, error) {
                       // 处理错误情况
                       alert("成绩修改失败");
                   }
               });
           }
       }
   </script>




</head>
<body>
    <!-- 顶栏 -->
    <form id="form1" runat="server">
        <div class="container" id="top">
            <div class="row">
                <div class="col-md-12">
                    <!-- 加入导航条标题 -->
                    <nav class="navbar navbar-expand-lg navbar-light bg-light">
                        <div class="container-fluid">
                            <a href="##" class="navbar-brand">学生管理系统(教师)</a>
                            <div class="dropdown">
                                <button class="btn btn-outline-primary dropdown-toggle" type="button" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                                    <%= Session["UserName"] %>
                                </button>
                                <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                    <li>
                                        <a class="dropdown-item" href="#">
                                            <span class="glyphicon glyphicon-cog me-2"></span>
                                            修改个人信息
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </nav>
                </div>
            </div>
        </div>

        <!-- 中间主体 -->
        <div class="container" id="content">
            <div class="row">
                <div class="col-md-2">
    <ul class="nav nav-pills flex-column" id="nav">
        <li class="nav-item">
            <asp:LinkButton runat="server" CssClass="nav-link" OnClick="myclassOnlick">
                <i class="fas fa-book"></i> 我的课程<span class="badge bg-secondary"></span>
            </asp:LinkButton>
        </li>
        <li class="nav-item">
            <a class="nav-link" href="#" data-bs-toggle="modal" data-bs-target="#changePasswordModal">
                <i class="fas fa-lock"></i> 修改密码<span class="glyphicon glyphicon-pencil"></span>
            </a>
        </li>
        <li class="nav-item">
            <a class="nav-link" href="Login.aspx">
                <i class="fas fa-sign-out-alt"></i> 退出系统<span class="glyphicon glyphicon-log-out"></span>
            </a>
        </li>
    </ul>
</div>
<script src="https://kit.fontawesome.com/2f45d073c9.js" crossorigin="anonymous"></script>
                <div class="col-md-10">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" DataKeyNames="课程编号" OnRowCommand="gridEnrolledCourses_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="课程编号" HeaderText="课程编号" ReadOnly="True" SortExpression="课程编号" />
                        <asp:BoundField DataField="课程名" HeaderText="课程名" SortExpression="课程名" />
                        <asp:BoundField DataField="课时" HeaderText="课时" SortExpression="课时" />
                        <asp:BoundField DataField="学分" HeaderText="学分" SortExpression="学分" />
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                <asp:Button runat="server" Text="查看" CommandName="ViewDetails" CommandArgument='<%# Eval("课程编号") %>' CssClass="btn btn-primary" OnClick="ViewDetails_Click" />
                                <asp:LinkButton runat="server" ID="btnAssignments" Text="作业" CommandName="ViewAssignments" CommandArgument='<%# Eval("课程编号") %>' CssClass="btn btn-primary " />
                                <asp:Button runat="server" Text="作业布置" CommandName="AssignAssignment" CommandArgument='<%# Eval("课程编号") %>' CssClass="btn btn-success" OnClick="AssignAssignment_Click" />
                                <asp:LinkButton runat="server" ID="btnExperiments" Text="实验" CommandName="ViewExperiments" CommandArgument='<%# Eval("课程编号") %>' CssClass="btn btn-primary " />
                                <asp:Button runat="server" Text="实验布置" CommandName="AssignExperiment" CommandArgument='<%# Eval("课程编号") %>' CssClass="btn btn-success" OnClick="AssignExperiment_Click" />
                     
                                
                                
                             </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Panel runat="server" ID="pnlAssignments" Visible="false">
                    <h2>作业</h2>
                    <asp:Label ID="lbHomeWorkFromName" runat="server" Text="" Font-Size="Medium"></asp:Label>
                    <asp:GridView runat="server" ID="gridAssignments" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" OnRowCommand="gridEnrolledCourses_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="AssignmentID" HeaderText="作业ID" />
                            <asp:BoundField DataField="AssignmentName" HeaderText="作业名称" />
                            <asp:BoundField DataField="Deadline" HeaderText="截止日期" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Description" HeaderText="描述" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnDropAssignment" Text="删除" CommandName="DropAssignment" CommandArgument='<%# Eval("AssignmentID") %>' CssClass="btn btn-danger btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </asp:Panel>

                <asp:Panel runat="server" ID="pnlExperiments" Visible="false">
                    <h2>实验</h2
                    <asp:Label ID="lbExFromName" runat="server" Text="" Font-Size="Medium"></asp:Label>
                    <asp:GridView runat="server" ID="gridExperiments" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" OnRowCommand="gridEnrolledCourses_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="ExperimentID" HeaderText="实验ID" />
                            <asp:BoundField DataField="ExperimentName" HeaderText="实验名称" />
                            <asp:BoundField DataField="ExperimentDate" HeaderText="实验日期" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Description" HeaderText="描述" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnDropExperiment" Text="删除" CommandName="DropExperiment" CommandArgument='<%# Eval("ExperimentID") %>' CssClass="btn btn-danger btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                

                    <asp:SqlDataSource ID="Teacher" runat="server" ConnectionString="<%$ ConnectionStrings:SchoolDBConnectionString %>"
                        SelectCommand="SELECT Courses.CourseID 课程编号, Courses.CourseDescription 课程名, Courses.CourseHours 课时, Courses.CourseCredits 学分
                                        FROM Teachers, Courses
                                        WHERE Courses.TeacherID = Teachers.TeacherID
                                        AND Teachers.TeacherID = @UserName">
                        <SelectParameters>
                            <asp:Parameter Name="UserName" Type="String" DefaultValue="T001" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:GridView ID="Teacher_select" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="学号" HeaderText="学号" SortExpression="学号" />
                            <asp:BoundField DataField="姓名" HeaderText="姓名" SortExpression="姓名" />
                            <asp:BoundField DataField="课程成绩" HeaderText="课程成绩" SortExpression="课程成绩" />
                             <asp:BoundField DataField="课程编号" HeaderText="课程编号" SortExpression="课程编号" />
                            <asp:TemplateField HeaderText="成绩修改">
                                <ItemTemplate>
                                    <button class="btn btn-primary" onclick="updateGrade('<%# Eval("学号") %>','<%# Eval("课程编号") %>')">成绩修改</button>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                    <!--布置作业的界面-->
                    <asp:Panel runat="server" ID="pnlAssignAssignment" CssClass="assignment-panel" Visible="False">
                        <h3>作业布置</h3>
                        <div>
                            <label for="txtAssignmentID">作业ID:</label>
                            <asp:TextBox runat="server" ID="txtAssignmentID" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div>
                            <label for="txtAssignmentName">作业名:</label>
                            <asp:TextBox runat="server" ID="txtAssignmentName" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div>
                            <label for="txtDeadline">截至日期:</label>
                            <asp:TextBox runat="server" ID="txtDeadline" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div>
                            <label for="txtDescription">作业描述:</label>
                            <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Button runat="server" ID="btnAssign" Text="提交" CssClass="btn btn-primary" OnClick="btnAssign_Click" />
                            <asp:Button runat="server" ID="btnCancel" Text="取消" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                        </div>
                    </asp:Panel>
                    <!--布置实验的界面-->
                    <asp:Panel runat="server" ID="pnlAssignExperiment" CssClass="assignment-panel" Visible="false">
                        <h3>实验布置</h3>
                        <div>
                            <label>实验ID:</label>
                            <asp:TextBox runat="server" ID="txtExperimentID" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div>
                            <label>实验名:</label>
                            <asp:TextBox runat="server" ID="txtExperimentName" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div>
                            <label>截至日期:</label>
                            <asp:TextBox runat="server" ID="txtExperimentDeadline" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div>
                            <label>实验描述:</label>
                            <asp:TextBox runat="server" ID="txtExperimentDescription" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Button runat="server" ID="btnAssignExperiment" Text="确认" OnClick="btnAssignExperiment_Click" CssClass="btn btn-primary" />
                            <asp:Button runat="server" ID="btnCancelExperiment" Text="取消" OnClick="btnCancelExperiment_Click" CssClass="btn btn-secondary" />
                        </div>
                    </asp:Panel>


                </div>

                <!--修改密码的部分-->
                <!-- 模态框 -->
                <div class="modal fade" id="changePasswordModal" tabindex="-1" aria-labelledby="changePasswordModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="changePasswordModalLabel">修改密码</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <label for="txtNewPassword">新密码：</label>
                                    <input type="password" id="txtNewPassword" class="form-control" />
                                </div>
                                <div class="form-group">
                                    <label for="txtConfirmPassword">确认密码：</label>
                                    <input type="password" id="txtConfirmPassword" class="form-control" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" onclick="changePassword()">确认修改</button>
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">关闭</button>
                            </div>
                        </div>
                    </div>
                </div>





            </div>
        </div>
    </form>
</body>
</html>



