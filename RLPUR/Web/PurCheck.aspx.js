// 初始化

$(function () {

    // 核准

    $("#ApproveButton").click(function () {
        return confirm("确认核准吗!?");
    });

    // 否决

    $("#RejectButton").click(function () {
        return confirm("确认否决吗!?");
    });

});