// 初始化

$(function () {

    // 打印
    $("#OKButton").click(function () {
        var prNoFrom = $.trim($('#PRNoFrom').val());
        var prNoTo = $.trim($('#PRNoTo').val());
        var venNo = $.trim($('#VenNo').val());
        var prType = $.trim($("input[name='PrType']:checked").val()); //$("input[name='PrType']:checked").val()
        //if (prNo.length == 0) {
        //    iiWeb_ShowMessage('请输入请购单号');
        //    return false;
        //}

        //        window.open(
        //            "PrintReport.aspx?ID={0}&Source=maintain".format(escape(prNo)),
        //            "_blank",
        //"height=600,width=800,location=no,toolbar=no,menubar=no,status=no,scrollbars=yes,resizable=yes"
        //        );
        //        return false;

        iiWeb_ShowDialog(
            "PrintReport.aspx?Source=print&prType={0}&venNo={1}&prNoFrom={2}&prNoTo={3}".format(escape(prType), escape(venNo), escape(prNoFrom), escape(prNoTo)),
            800,
            600
            );
        return false;

    });

});