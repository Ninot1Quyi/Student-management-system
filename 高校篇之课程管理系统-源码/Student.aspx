<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Student.aspx.cs" Inherits="Student" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>学生管理系统</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .nav-link:hover {
            background-color: #f8f9fa;
        }

        .nav-link.active {
            background-color: #e9ecef;
        }
        .search-container {
        display: flex;
        align-items: center;
        }
    
        .search-container label {
            margin-right: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div class="container" id="top">
            <div class="row">
                <div class="col-md-12">
                    <!-- 加入导航条标题 -->
                    <asp:Panel runat="server" CssClass="navbar navbar-expand-lg navbar-light bg-light">
                        <div class="container-fluid">
                            <a href="##" class="navbar-brand">学生管理系统(学生)</a>
                            <div class="dropdown">
                                <asp:LinkButton runat="server" ID="btnUserName" CssClass="btn btn-outline-primary dropdown-toggle"   />
                                 
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>


        <div class="container">
            <div class="row">
                <div class="col-md-3">
                <ul class="nav flex-column">
                    <li class="nav-item">
                        <asp:LinkButton runat="server" ID="lnkAllCourses" CssClass="nav-link" OnClick="lnkAllCourses_Click">
                            <i class="fas fa-book"></i> 所有课程
                        </asp:LinkButton>
                    </li>
                    <li class="nav-item">
                        <asp:LinkButton runat="server" ID="lnkEnrolledCourses" CssClass="nav-link" OnClick="lnkEnrolledCourses_Click">
                            <i class="fas fa-graduation-cap"></i> 已选课程
                        </asp:LinkButton>
                    </li>
 
                    <li class="nav-item">
                        <asp:LinkButton runat="server" ID="lnkChangePassword" CssClass="nav-link" OnClick="lnkChangePassword_Click">
                            <i class="fas fa-lock"></i> 修改密码
                        </asp:LinkButton>
                    </li>
                    <li class="nav-item">
                        <asp:LinkButton runat="server" ID="lnkLoginOut" CssClass="nav-link" OnClick="lnkLoginOut_Click">
                            <i class="fas fa-sign-out-alt"></i> 退出登录
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
            <script src="https://kit.fontawesome.com/2f45d073c9.js" crossorigin="anonymous"></script>
                <div class="col-md-9">
                <asp:Panel runat="server" ID="pnlAllCourses" Visible="false">
                    <h2>所有课程</h2>
                    <div class="form-group">
                        <div class="search-container">
                             
                            <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control"></asp:TextBox>
                            <asp:Button runat="server" ID="btnSearch" OnClick="btnAllCoursesSearch_Click" Text="搜索" CssClass="btn btn-primary" />
                        </div>
                    </div>
                    <asp:GridView runat="server" ID="gridAllCourses" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" OnRowCommand="gridAllCourses_RowCommand" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:BoundField DataField="CourseID" HeaderText="课程ID" />
                            <asp:BoundField DataField="CourseDescription" HeaderText="课程描述" />
                            <asp:BoundField DataField="CourseCredits" HeaderText="学分" />
                            <asp:BoundField DataField="TeacherName" HeaderText="任课老师" />
                            <asp:TemplateField HeaderText="选课">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnSelectCourse" Text="选课" CommandName="SelectCourse" CommandArgument='<%# Eval("CourseID") %>' CssClass="btn btn-primary btn-sm " ></asp:LinkButton>
                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlEnrolledCourses" Visible="false">
                    <h2>已选课程</h2>
                    <div class="form-group">
                        <div class="search-container">
                            
                            <asp:TextBox runat="server" ID="txtEnrolledSearch" CssClass="form-control"></asp:TextBox>
                            <asp:Button runat="server" ID="btnEnrolledSearch" OnClick="btnEnrolledCoursesSearch_Click" Text="搜索" CssClass="btn btn-primary" />
                        </div>
                    </div>
                    <asp:GridView runat="server" ID="gridEnrolledCourses" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" OnRowCommand="gridEnrolledCourses_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CourseID" HeaderText="课程ID" />
                            <asp:BoundField DataField="CourseDescription" HeaderText="课程描述" />
                            <asp:BoundField DataField="CourseCredits" HeaderText="学分" />
                            <asp:BoundField DataField="Grade" HeaderText="成绩" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnAssignments" Text="作业" CommandName="ViewAssignments" CommandArgument='<%# Eval("CourseID") %>' CssClass="btn btn-primary btn-sm" />
                                    <asp:LinkButton runat="server" ID="btnExperiments" Text="实验" CommandName="ViewExperiments" CommandArgument='<%# Eval("CourseID") %>' CssClass="btn btn-primary btn-sm" />
                                    <asp:LinkButton runat="server" ID="LinkButton1" Text="退课" CommandName="DropCourse" CommandArgument='<%# Eval("CourseID") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirmFirstConfirmation();" />
                                    <script>
                                        function confirmFirstConfirmation() {
                                            var confirmation = confirm('确认要退课吗？');
                                            if (confirmation) {
                                                var secondConfirmation = confirm('请再次确认是否要退课？');
                                                return secondConfirmation;
                                            }
                                            return false;
                                        }
                                    </script>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                <asp:Panel runat="server" ID="pnlAssignments" Visible="false">
                    <h2>作业</h2>
                    <asp:Label ID="lbHomeWorkFromName" runat="server" Text="" Font-Size="Medium"></asp:Label>
                    <asp:GridView runat="server" ID="gridAssignments" CssClass="table table-bordered table-striped" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="AssignmentID" HeaderText="作业ID" />
                            <asp:BoundField DataField="AssignmentName" HeaderText="作业名称" />
                            <asp:BoundField DataField="Deadline" HeaderText="截止日期" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Description" HeaderText="描述" />
                        </Columns>
                    </asp:GridView>

                </asp:Panel>

                <asp:Panel runat="server" ID="pnlExperiments" Visible="false">
                    <h2>实验</h2
                    <asp:Label ID="lbExFromName" runat="server" Text="" Font-Size="Medium"></asp:Label>
                    <asp:GridView runat="server" ID="gridExperiments" CssClass="table table-bordered table-striped" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="ExperimentID" HeaderText="实验ID" />
                            <asp:BoundField DataField="ExperimentName" HeaderText="实验名称" />
                            <asp:BoundField DataField="ExperimentDate" HeaderText="实验日期" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Description" HeaderText="描述" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                </asp:Panel>





                    <asp:Panel runat="server" ID="pnlChangePassword" Visible="false">
                        <h2>修改密码</h2>
                        <div class="form-group">
                            <label for="txtCurrentPassword">当前密码：</label>
                            <input type="password" id="txtCurrentPassword" runat="server" class="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="txtNewPassword">新密码：</label>
                            <input type="password" id="txtNewPassword" runat="server" class="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="txtConfirmPassword">确认密码：</label>
                            <input type="password" id="txtConfirmPassword" runat="server" class="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Button runat="server" ID="btnChangePassword" OnClick="btnChangePassword_Click" Text="修改密码" CssClass="btn btn-primary" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
