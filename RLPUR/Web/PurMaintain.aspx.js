// 初始化

$(function () {

    // 表单验证
    $("#SaveButton").click(function () {
        if (iiWeb_ValidateRequiredField()) {
            return true;
        }
        else {
            iiWeb_ShowTip(this, iiWeb_RequiredFieldValidationErrorMessage);
            return false;
        }
    });

    // 提交

    $("#PostButton").click(function () {
        return confirm('确定要提交吗?');
    });

    //获取供应商名称
    $(".venno").blur(function () {
        var venNo = $(this).val();
        getVendor(venNo, this);
    });

    // 打印
    $("#PrintButton").click(function () {
        var prNo = $.trim($('#PRNo').val());
        if (prNo.length == 0) {
            iiWeb_ShowMessage('请输入请购单号');
            return false;
        }
        //        window.open(
        //            "PrintReport.aspx?ID={0}&Source=maintain".format(escape(prNo)),
        //            "_blank",
        //"height=600,width=750,location=no,toolbar=no,menubar=no,status=no,scrollbars=yes,resizable=yes"
        //        );
        //        return false;

        iiWeb_ShowDialog(
            "PrintReport.aspx?ID={0}&Source=maintain".format(escape(prNo)),
            750,
            600
            );
        return false;

    });

});

//获取供应商
function getVendor(venNo, obj) {
    $.ajax({
        url: 'GetVendor.ashx',
        type: 'POST',
        data: { 'VendorNo': venNo },
        dataType: 'json',
        //timeout: 50000,
        //contentType: 'application/json;charset=utf-8',
        success: function (response) {
            //alert(JSON.stringify(response));
            if (response.success) {
                $(obj).parent().parent().find('.venname').val(response.vendorName);
                $(obj).parent().parent().find('.vencurr').val(response.curr);
            } else {
                $(obj).val('');
                $(obj).parent().parent().find('.venname').val('');
                $(obj).parent().parent().find('.vencurr').val('');
                iiWeb_ShowMessage(response.message);
            }
        },
        error: function (err) {
            iiWeb_ShowMessage("执行失败");
        }

    });
}