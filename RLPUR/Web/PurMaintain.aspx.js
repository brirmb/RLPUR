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
});
