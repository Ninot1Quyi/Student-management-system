<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 
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
       <asp:ScriptManager runat="server"></asp:ScriptManager>
       <div class="container" id="top">
            <div class="row">
                <div class="col-md-12">
                    <!-- 加入导航条标题 -->
                    <asp:Panel runat="server" CssClass="navbar navbar-expand-lg navbar-light bg-light">
                        <div class="container-fluid">
                            <a href="##" class="navbar-brand">学生管理系统(管理员)</a>
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
                    <asp:LinkButton runat="server" ID="lnkCoursesManagement" CssClass="nav-link" OnClick="lnkCoursesManagement_Click">
                        <i class="fas fa-book-open"></i> 课程管理
                    </asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton runat="server" ID="lnkStudentsManagement" CssClass="nav-link" OnClick="lnkStudentsManagement_Click">
                        <i class="fas fa-user-graduate"></i> 学生管理
                    </asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton runat="server" ID="lnkTeachersManagement" CssClass="nav-link" OnClick="lnkTeachersManagement_Click">
                        <i class="fas fa-chalkboard-teacher"></i> 教师管理
                    </asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton runat="server" ID="lnkResetTSPassword" CssClass="nav-link" OnClick="lnkResetTSPassword_Click">
                        <i class="fas fa-key"></i> 账号密码重置
                    </asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton runat="server" ID="lnkResetSelfPassword" CssClass="nav-link" OnClick="lnkResetSelfPassword_Click">
                        <i class="fas fa-lock"></i> 修改密码
                    </asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton runat="server" ID="lnkLoginOut" CssClass="nav-link" OnClick="lnkLogout_Click">
                        <i class="fas fa-sign-out-alt"></i> 退出登录
                    </asp:LinkButton>
                </li>
            </ul>
        </div>
        <script src="https://kit.fontawesome.com/2f45d073c9.js" crossorigin="anonymous"></script>
                <div class="col-md-9">
                <asp:Panel runat="server" ID="pnlCoursesManagement" Visible="false">
                    <h2>课程管理</h2>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <asp:TextBox ID="txtCourseSearch" runat="server" CssClass="form-control" placeholder="请输入搜索关键词"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnCourseSearch" runat="server" Text="搜索" CssClass="btn btn-primary" OnClick="btnCourseSearch_Click" />
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="text-align: right;">
                        <asp:Button runat="server" ID="Button1" Text="添加课程" OnClick="btnAddCourse_Click" CssClass="btn btn-primary btn-sm"/>
                    </div>
                    <asp:GridView ID="gvCourses" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" OnRowCommand="gvCourses_RowCommand" AllowPaging="True"  OnPageIndexChanging="GridViewCourses_PageIndexChanging" PageSize="5">
                        <Columns>
                            <asp:BoundField DataField="CourseID" HeaderText="课程号" />
                            <asp:BoundField DataField="CourseDescription" HeaderText="课程名称" />
                            <asp:BoundField DataField="TeacherID" HeaderText="授课老师编号" />
                            <asp:BoundField DataField="TeacherName" HeaderText="授课老师" />
                            <asp:BoundField DataField="Address" HeaderText="上课地点" />
                            <asp:BoundField DataField="CourseCredits" HeaderText="学分" />
                            <asp:BoundField DataField="CourseHours" HeaderText="学时" />
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:Button runat="server" Text="修改" CommandName="EditCourse" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary btn-sm" />
                                    <asp:Button runat="server" Text="删除" CommandName="DeleteCourse" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-danger btn-sm"/>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView> 
 <asp:Panel runat="server" ID="pnlAddCourse" Visible="false">
   <table class="table table-bordered table-striped">
    <tr>
        <th width="62px">课程号</th>
        <th width="150px">课程名称</th>
        <th width="200px">授课老师</th>
        <th width="80px">上课地点</th>
        <th width="50px">学分</th>
        <th width="50px">学时</th>
        <th width="110px">操作</th>
    </tr>
    <tr>
        <td width="62px"><asp:TextBox runat="server" ID="txtAddCourseID" CssClass="form-control"></asp:TextBox></td>
        <td width="150px"><asp:TextBox runat="server" ID="txtAddCourseDescription" CssClass="form-control"></asp:TextBox></td>
        <td width="200px">
            <asp:DropDownList runat="server" ID="ddlAddTeacherID" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
        </td>
        <td width="80px"><asp:TextBox runat="server" ID="txtAddAddress" CssClass="form-control"></asp:TextBox></td>
        <td width="50px"><asp:TextBox runat="server" ID="txtCourseCredits" CssClass="form-control"></asp:TextBox></td>
        <td width="50px"><asp:TextBox runat="server" ID="txtCourseHours" CssClass="form-control"></asp:TextBox></td>
        <td width="110px">
            <span>
                <asp:Button runat="server" Text="添加" CommandName="ConfirmAddCourse" CssClass="btn btn-success btn-sm inline-button" OnClick="btnConfirmAdd_Click" />
                <asp:Button runat="server" Text="取消" CommandName="CancelAddCourse" CssClass="btn btn-danger btn-sm inline-button" OnClick="btnCancelAddCourse_Click" />
            </span>
        </td>
    </tr>
</table>

</asp:Panel>

                </asp:Panel>
<asp:Panel runat="server" ID="pnlEditCourse" Visible="false" CssClass="panel panel-default">
    <div class="panel-heading">
        <h2>修改课程信息</h2>
    </div>
    <div class="panel-body">
        <div class="form-horizontal">
            <div class="form-group">
                <label for="txtCourseID" class="col-sm-3 control-label">课程号:</label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtCourseID" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="txtCourseName" class="col-sm-3 control-label">课程名称:</label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtCourseName" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="txtTeacherID" class="col-sm-3 control-label">授课老师编号:</label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtTeacherID" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="txtTeacherName" class="col-sm-3 control-label">授课老师:</label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtTeacherName" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="txtAddress" class="col-sm-3 control-label">上课地点:</label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtAddress" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="txtCredits" class="col-sm-3 control-label">学分:</label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtCredits" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="txtHours" class="col-sm-3 control-label">学时:</label>
                <div class="col-sm-9">
                    <asp:TextBox runat="server" ID="txtHours" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="form-group">
                <div class="col-sm-offset-3 col-sm-9">
                    <asp:Button ID="btnUpdate" runat="server" Text="更新" OnClick="btnUpdateCourse_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="Button2" runat="server" Text="取消" OnClick="btnCancel_Click" CssClass="btn btn-secondary" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
                    <asp:Panel runat="server" ID="pnlStudentsManagement" Visible="false" CssClass="panel panel-default">
                    <div class="panel-heading">
                        <h2>学生管理</h2>
                    </div>
                    
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="请输入搜索关键词"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:Button ID="btnSearchStudent" runat="server" Text="搜索" CssClass="btn btn-primary" OnClick="btnSearchStudent_Click" />
                                    </span>
                                </div>
                            </div>
                            <div style="text-align: right;">
                                <asp:Button runat="server" ID="Button3" Text="添加学生" OnClick="btnAddStudent_Click" CssClass="btn btn-primary btn-sm" />
                            </div>
                        </div>
                        
                     
                        <br />
                        <asp:GridView ID="GridViewStudents" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False"  OnRowCommand="GridViewStudents_RowCommand"  AllowPaging="True"  OnPageIndexChanging="GridViewStudents_PageIndexChanging" PageSize="5">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" />
                            <Columns>
                                <asp:BoundField DataField="StudentID" HeaderText="学号" />
                                <asp:BoundField DataField="StudentName" HeaderText="姓名" />
                                <asp:BoundField DataField="StudentSex" HeaderText="性别" />
                                <asp:BoundField DataField="StudentBirthday" HeaderText="出生年份" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="AdmissionDate" HeaderText="入学时间" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="DepartmentName" HeaderText="学院" />
                                <asp:BoundField DataField="MajorName" HeaderText="专业" />
                                <asp:TemplateField HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EditStudent" CommandArgument='<%# Eval("StudentID") %>' Text="修改" CssClass="btn btn-primary btn-sm" />
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DeleteStudent" CommandArgument='<%# Eval("StudentID") %>' Text="删除" CssClass="btn btn-danger btn-sm"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                        </asp:GridView>
                    </div>
                </asp:Panel>
<asp:Panel runat="server" ID="pnlAddStudent" Visible="false">
    <table class="table table-bordered table-striped">
        <tr>
            <th width="50px">编号</th>
            <th width="50px">姓名</th>
            <th width="30px">性别</th>
            <th width="80px">出生日期</th>
            <th width="80px">入学日期</th>
            <th width="100px">所属院系</th>
            <th width="100px">所属专业</th>
            <th width="100px">操作</th>
        </tr>
        <tr>
            <td width="50px"><asp:TextBox runat="server" ID="txtAddStudentID" Font-Size ="Smaller" CssClass="form-control"></asp:TextBox></td>
            <td width="50px"><asp:TextBox runat="server" ID="txtAddStudentName" Font-Size ="Smaller" CssClass="form-control"></asp:TextBox></td>
            <td width="30px">
                <asp:DropDownList runat="server" ID="ddlAddStudentGender" Font-Size ="Smaller" CssClass="form-control">
                    <asp:ListItem Text="男" Value="男"></asp:ListItem>
                    <asp:ListItem Text="女" Value="女"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td width="80px"><asp:TextBox runat="server" ID="txtAddStudentBirthday" CssClass="form-control" Font-Size ="Smaller"></asp:TextBox></td>
            <td width="80px"><asp:TextBox runat="server" ID="txtAddAdmissionDate" CssClass="form-control" Font-Size ="Smaller"></asp:TextBox></td>
            <td width="100px">
                <asp:DropDownList runat="server" ID="ddlAddDepartment" Font-Size ="Smaller" CssClass="form-control" AutoPostBack="true"  ></asp:DropDownList>
            </td>
            <td width="100px"
                <asp:DropDownList runat="server" ID="ddlAddMajor" Font-Size ="Smaller" CssClass="form-control"></asp:DropDownList>
            </td>
            <td width="100px" style="text-align: center;">
                <asp:Button runat="server" Text="添加" CommandName="ConfirmAddStudent" CssClass="btn btn-success btn-sm"  onClick="btnConfirmAddStudent_Click"/>
                <asp:Button ID="btnCancalAddStudent" runat="server" Text="取消" OnClick="btnCancel_Click" CssClass="btn btn-danger btn-sm" />
            </td>
        </tr>
    </table>
</asp:Panel>


                    <asp:Panel runat="server" ID="PanelEditStudent" Visible="false" CssClass="panel panel-default">
                        <div class="panel-heading">
                            <h2>修改学生信息</h2>
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="txtStudentID" class="col-sm-3 control-label">学号:</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox runat="server" ID="txtStudentID" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="txtStudentName" class="col-sm-3 control-label">姓名:</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox runat="server" ID="txtStudentName" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="rblStudentSex" class="col-sm-3 control-label">性别:</label>
                                    <div class="col-sm-9">
                                        <asp:RadioButtonList runat="server" ID="rblStudentSex" CssClass="form-control" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="男" Value="男"></asp:ListItem>
                                            <asp:ListItem Text="女" Value="女"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txtStudentBirthday" class="col-sm-3 control-label">出生年份:</label>
                                    <div class="col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="txtStudentBirthday" CssClass="form-control"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="ceStudentBirthday" runat="server" TargetControlID="txtStudentBirthday" CssClass="form-control" PopupPosition="Right"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="txtAdmissionDate" class="col-sm-3 control-label">入学时间:</label>
                                    <div class="col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="txtAdmissionDate" CssClass="form-control"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="ceAdmissionDate" runat="server" TargetControlID="txtAdmissionDate" CssClass="form-control" PopupPosition="Right"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="ddlDepartment" class="col-sm-3 control-label">院系:</label>
                                    <div class="col-sm-9">
                                        <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="ddlMajor" class="col-sm-3 control-label">专业:</label>
                                    <div class="col-sm-9">
                                        <asp:DropDownList runat="server" ID="ddlMajor" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                   
                                <div class="form-group">
                                    <div class="col-sm-offset-3 col-sm-9">
                                        <asp:Button ID="btnSave" runat="server" Text="更新" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                                        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" CssClass="btn btn-secondary" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>





                    <asp:Panel runat="server" ID="pnlTeachersManagement" Visible="false">
                        <h2>教师管理</h2>
                    
                        <div id ="search_Teacher" runat="server" Visible="false">
                            <div>
                                <label for="txtSearchName">姓名:</label>
                            <input type="text" id="txtSearchName" runat="server" />
                            <label for="ddlSearchGender">性别:</label>
                            <asp:DropDownList runat="server" ID="ddlSearchGender">
                                <asp:ListItem Text="全部" Value=""></asp:ListItem>
                                <asp:ListItem Text="男" Value="男"></asp:ListItem>
                                <asp:ListItem Text="女" Value="女"></asp:ListItem>
                            </asp:DropDownList>
                            <label for="txtSearchTitle">职称:</label>
                            <input type="text" id="txtSearchTitle" runat="server" />
                            <asp:Button runat="server" ID="btnTeacherSearch" Text="搜索" CssClass="btn btn-primary" OnClick="btnSearchTeacher_Click"  />
                            </div>
                        </div>
                         <asp:GridView runat="server" ID="GridViewTeacher" CssClass="table table-bordered table-striped"
                             AutoGenerateColumns="false" OnRowCommand="GridViewTeacher_RowCommand" DataKeyNames="教师号"
                             AllowPaging="true" PageSize="5">
                           <PagerSettings Position="Bottom" />
                            <Columns >
                                <asp:BoundField DataField="教师号" HeaderText="教师号"  ReadOnly ="true"/>
                                <asp:BoundField DataField="姓名" HeaderText="姓名"  ReadOnly ="true"/>
                                <asp:BoundField DataField="年龄" HeaderText="年龄"  ReadOnly ="true"/>
                                <asp:BoundField DataField="出生日期" HeaderText="出生日期" ReadOnly ="true" DataFormatString="{0:yyyy-MM-dd}"/>
                                <asp:BoundField DataField="性别" HeaderText="性别"  ReadOnly ="true"/>
                                <asp:BoundField DataField="职称" HeaderText="职称"  ReadOnly ="true"/>
                                <asp:TemplateField HeaderText="操作">
                                    <ItemTemplate>
                                       <asp:Button runat="server" ID="btnEdit" Text="修改" CssClass="btn btn-primary" CommandName="Change" CommandArgument='<%# Container.DataItemIndex %>' OnCommand="btnTEdit_Command" />
                                       <asp:Button runat="server" ID="btnDelete" Text="删除" CssClass="btn btn-danger" CommandName="DeleteTeacher" CommandArgument='<%# Container.DataItemIndex %>'   />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>


                        <div id="editTeacherDiv" runat="server" Visible="false">
                        <div style="display: flex; flex-direction: column;">
                            <label for="txtEditName">姓名:</label>
                            <input type="text" id="txtEditName" runat="server" />
                            <br />
                            <label for="txtEditSex">性别:</label>
                            <input type="text" id="txtEditSex" runat="server" />
                            <br />
                            <label for="txtEditBirthday">出生日期:</label>
                            <input type="text" id="txtEditBirthday" runat="server" />
                            <br />
                            <label for="txtEditTitle">职称:</label>
                            <input type="text" id="txtEditTitle" runat="server" />

                            <br />
                            <label for="ddlEditDepartment">系号:</label>
                            <asp:DropDownList runat="server" ID="ddlEditDepartment"></asp:DropDownList>

                            <br />
                            <asp:Button runat="server" ID="Button5" Text="更新" CssClass="btn btn-primary" OnClick="btnTeacherUpdate_Click" />
                        </div>
                    </div>

                    
                <asp:Button runat="server" ID="btnAddTeacher" Text="添加教师" CssClass="btn btn-primary" OnClick="btnAddTeacher_Click" Visible="false" />
                <div id="divAddTeacher" runat="server" Visible="false">
                    <div style="display: flex; flex-direction: column;">
                        <label for="txtTeacherID">教师号:</label>
                        <input type="text" id="txtAddTeacherID" runat="server" />

                        <label for="txtTeacherName">姓名:</label>
                        <input type="text" id="txtAddTeacherName" runat="server" />

                        <label for="ddlTeacherSex">性别:</label>
                        <asp:DropDownList runat="server" ID="ddlTeacherSex">
                            <asp:ListItem Text="男" Value="男"></asp:ListItem>
                            <asp:ListItem Text="女" Value="女"></asp:ListItem>
                        </asp:DropDownList>

                        <label for="txtTeacherPassword">密码:</label>
                        <input type="password" id="txtTeacherPassword" runat="server" />

                        <label for="ddlDepartment">系号:</label>
                        <asp:DropDownList runat="server" ID="dwlAddTDepartment"></asp:DropDownList>

                        <label for="txtTitle">职称:</label>
                        <input type="text" id="txtTitle" runat="server" />

                        <label for="txtTeacherBirthday">出生日期:</label>
                        <asp:TextBox runat="server" ID="txtTeacherBirthday" CssClass="datepicker"></asp:TextBox>

                        <asp:Button runat="server" ID="btnSaveTeacher" Text="保存" CssClass="btn btn-primary" OnClick="btnSaveTeacher_Click" />
                    </div>
                </div>
                    </asp:Panel>





                    <asp:Panel runat="server" ID="pnlResetTSPassword" Visible="false" >
                        <h2 class="h4">账号密码重置</h2>
                        <asp:DropDownList runat="server" ID="ddlUserType">
                            <asp:ListItem Text="学生" Value="student"></asp:ListItem>
                            <asp:ListItem Text="教师" Value="teacher"></asp:ListItem>
                            <asp:ListItem Text="管理员" Value="admin"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button runat="server" ID="btnShowTable" Text="确定" OnClick="btnShowResetUserTable_Click" CssClass="btn btn-primary mt-3" />
                        <asp:GridView runat="server" ID="gvUserInfo" AutoGenerateColumns="false" CssClass="table table-striped">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true"  />

                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="Name" HeaderText="名字"  />
                                <%-- Add other columns from your database table here --%>
                            </Columns>
                        </asp:GridView>


                        <asp:Button runat="server" ID="btnResetPassword" Text="重置密码" OnClick="btnResetPassword_Click" CssClass="btn btn-primary mt-3" Visible="false"/>
                    </asp:Panel>






                



                    <asp:Panel runat="server" ID="pnlResetSelfPassword" Visible="false">
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
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
<script>
    function showEditModal() {
        $('#courseModal').modal('show');
    }
</script>
</body>
</html>


