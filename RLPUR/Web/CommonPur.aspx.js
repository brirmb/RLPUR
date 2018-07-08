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

    // 删除

    $("#DeleteButton").click(function () {
        return confirm(iiWeb_ConfirmMessage);
    });
});
