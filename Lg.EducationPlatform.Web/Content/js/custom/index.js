jQuery(document).ready(function(){
								
	///// TRANSFORM CHECKBOX /////							
	//$('input:checkbox').uniform();
	
	///// LOGIN FORM SUBMIT /////
    $('#login').submit(function () {
        var username = $('#UserName').val();
        var pwd = $('#PassWord').val();
        if (username == '' || pwd == '')
            return false;

        $.post("/Account/Login", $(this).serialize(), function (data) {
            if (data.Status == "1") {
                var checked = $("input[type='checkbox']").is(':checked');//获取“是否记住密码”复选框 
                if (checked) { //判断是否选中了“记住密码”复选框    
                    $.cookie("username", username);  
                    $.cookie("pwd", $.base64.encode(pwd));//调用jquery.cookie.js中的方法设置cookie中的登陆密码，并使用base64（jquery.base64.js）进行加密    
                } else {
                    $.cookie("username", null);
                    $.cookie("pwd", null);
                } 
                window.location.href = data.CoreData;
            } else {
                layer.msg(data.Message, {
                    closeBtn: 1,
                    skin: 'layui-layer-molv',
                    shift: 4,
                    time: 2000
                }, function () {
                    $('#UserName').val('');
                    $('#PassWord').val('');
                    $('#UserName').focus();
                });
            }
        });

        return false;
	});
	
	///// ADD PLACEHOLDER /////
	$('#UserName').attr('placeholder','请输入用户名');
    $('#PassWord').attr('placeholder', '请输入密码');

    var username = $.cookie("username");     
    var pwd = $.cookie("pwd");    
    if (pwd) {//密码存在的话把“记住用户名和密码”复选框勾选住    
        $("input:checkbox").attr("checked", "true");
    }
    if (username) {   
        $("#UserName").val(username);
    }
    if (pwd) {   
        $("#PassWord").val($.base64.decode(pwd));
    } 
});
