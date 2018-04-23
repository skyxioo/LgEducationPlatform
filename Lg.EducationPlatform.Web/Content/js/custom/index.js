jQuery(document).ready(function(){
								
	///// TRANSFORM CHECKBOX /////							
	//$('input:checkbox').uniform();
	
    ///// LOGIN FORM SUBMIT /////
    jQuery('#login').submit(function () {
        var username = jQuery('#UserName').val();
        var pwd = jQuery('#PassWord').val();
        if (username == '' || pwd == '')
            return false;

        jQuery.post("/Account/Login", jQuery(this).serialize(), function (data) {
            if (data.Status == "1") {
                var checked = jQuery("input[type='checkbox']").is(':checked');//获取“是否记住密码”复选框 
                if (checked) { //判断是否选中了“记住密码”复选框    
                    jQuery.cookie("username", username);  
                    jQuery.cookie("pwd", jQuery.base64.encode(pwd));//调用jquery.cookie.js中的方法设置cookie中的登陆密码，并使用base64（jquery.base64.js）进行加密    
                } else {
                    jQuery.cookie("username", null);
                    jQuery.cookie("pwd", null);
                } 
                window.location.href = data.CoreData;
            } else {
                layer.msg(data.Message, {
                    closeBtn: 1,
                    skin: 'layui-layer-molv',
                    shift: 4,
                    time: 2000
                }, function () {
                    jQuery('#UserName').val('');
                    jQuery('#PassWord').val('');
                    jQuery('#UserName').focus();
                });
            }
        });

        return false;
	});
	
	///// ADD PLACEHOLDER /////
    jQuery('#UserName').attr('placeholder','请输入用户名');
    jQuery('#PassWord').attr('placeholder', '请输入密码');

    var username = jQuery.cookie("username");     
    var pwd = jQuery.cookie("pwd");    
    if (pwd) {//密码存在的话把“记住用户名和密码”复选框勾选住    
        jQuery("input:checkbox").attr("checked", "true");
    }
    if (username) {   
        jQuery("#UserName").val(username);
    }
    if (pwd) {   
        jQuery("#PassWord").val(jQuery.base64.decode(pwd));
    } 
});
