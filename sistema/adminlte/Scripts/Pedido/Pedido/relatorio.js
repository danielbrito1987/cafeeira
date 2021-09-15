var SistemaWeb = SistemaWeb || {};

SistemaWeb.Relatorio = {

    ObterTodos: function () {
        executeRequest({
            url: '/Relatorio/ObterTodos/',
            //data: { codCondPagamento: codCondPagamento, dataEmissao: dataEmissao },
            async: false,
            type: 'GET',
            json: true,
            dontShowPanel: true,
            success: function (result) {

                cbImprimir.ClearItems();
                for (var i = 0; i < result.length; i++) {
                    cbImprimir.AddItem(result[i].DescricaoRelatorio, result[i].Codigo);
                }

                cbImprimir.SetSelectedIndex(0);

            }
        });
    }

}

function geraRelatorio(codPedido, codRelatorio, delay) {
    setTimeout(function () {
        var newwindow = window.open("/Relatorio/GeraRelatorio/?codPedido=" + codPedido + "&relatorio=" + codRelatorio, 'Relatório' + codRelatorio);

        if (newwindow != null) {
            if (window.focus) {
                newwindow.focus()
            }
            setTimeout(function () {
                printWindow.print();
            }, 500);
        }
    });
    return false;
}

function imprimeRelatorio() {
    var codPedido = $('#txtCodPedido').val();

    if (codPedido == "") {
        Swal.fire("Validação", "Favor informar pesquisar o número do pedido para imprimir o relatório.", "error");
        return false;
    }

    var relatorio = cbImprimir.GetValue();
    if (relatorio == "0") {
        Swal.fire("Validação", "Favor informar o relatório.", "error");
        return false;
    }

    geraRelatorio(codPedido, relatorio, 0);

}