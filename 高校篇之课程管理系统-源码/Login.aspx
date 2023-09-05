<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Login</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="robots" content="all,follow" />
    <link rel="stylesheet" href="https://www.jq22.com/jquery/bootstrap-4.2.1.css" />
    <link rel="stylesheet" href="/css/style.default.css" class="theme-stylesheet">
</head>
<body>
    <div class="page login-page">
        <div class="container d-flex align-items-center">
            <div class="form-holder has-shadow">
                <div class="row">
                    <!-- Logo & Information Panel -->
                    <div class="col-lg-6">
                        <div class="info d-flex align-items-center" style="background-image: url('img/bkground.jpg'); background-size: cover;">
                            <div class="content">
                                <div class="logo">
                                    <h1>欢迎登录</h1>
                                </div>
                                <p>合肥工业大学-学生管理系统</p>
                            </div>
                        </div>
                    </div>


                    <!-- Form Panel -->
                    <div class="col-lg-6 bg-white">
                        <div class="form d-flex align-items-center">
                            <div class="content">
                                <form runat="server" method="post" class="form-validate" id="loginForm">
                                    <div class="form-group">
                                        <label for="txtID" class="input-material">用户名:</label>
                                        <input type="text" class="form-control input-material" id="txtID" name="txtID" required data-msg="请输入用户名" />
                                    </div>
                                    <div class="form-group">
                                        <label for="txtPassword" class="input-material">密码:</label>
                                        <input type="password" class="form-control input-material" id="txtPassword" name="txtPassword" required data-msg="请输入密码" />
                                    </div>
                                    <button type="submit" class="btn btn-primary">登录</button>
                                    <asp:Label ID="lbshowmsg" runat="server" Text=""></asp:Label>
                                    
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- JavaScript files -->
    <script src="https://www.jq22.com/jquery/jquery-1.10.2.js"></script>
    <script src="https://www.jq22.com/jquery/bootstrap-3.3.4.js"></script>
    <script src="vendor/jquery-validation/jquery.validate.min.js"></script><!--表单验证-->
    <script src="js/front.js"></script>
    <script>
        $(function () {
            $("#loginForm").submit(function () {
                var userName = $("#txtID").val();
                var passWord = $("#txtPassword").val();
                // 执行登录操作或其他处理逻辑
                return false; // 阻止表单提交
            });
        });
    </script>
</body>
</html>
