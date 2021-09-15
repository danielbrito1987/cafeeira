var Sistema = Sistema || {};

Sistema.ConsultaProduto = {
    Load: function (visible) {
        if (visible) {
            Swal.fire({
                title: 'Carregando...',
                imageUrl: '../Content/adminlte/img/load.gif',
                imageWidth: 70,
                showConfirmButton: false,
                allowOutsideClick: false
            })
        } else {
            Swal.close();
        }
    },

    Init: function () {
        setTimeout(function () {
            $('#txtCodProduto').focus();
        }, 500);
    },

    GridTabelaPrecos: function () {
        $('#tblTabelaPreco').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            filter: false,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
                decimal: ",",
                thousands: "."
            },
            //"dom": '<"top"if>rt<"bottom"lp><"clear">',
            dom: 'Bfrtip',
            ajax: {
                url: '/ConsultaProduto/ObterTodosTabelaPreco',
                type: 'GET',
                dataType: 'json',
                dataSrc: "Data"
            },
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.ConsultaProduto.SelecionarTabelaPreco(\"" + full.Codigo + "\")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código" },
                { data: "Descricao", title: "Descrição" }
            ]
        });
    },

    GridBuscaProdutos: function (data) {
        $('#tblConsultaProdutos').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            filter: false,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
                decimal: ",",
                thousands: "."
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            columns: [
                { data: "CodPro", title: "Cód. Produto" },
                { data: "CodDer", title: "Deriv." },
                { data: "DesPro", title: "Descrição" },
                { data: "UniMed", title: "UN" },
                { data: "SalEst", title: "Sal. Est." },
                { data: "CodTpr", title: "Tab. Preço" },
                { data: "PreBas", title: "Preço Base" },
            ],
            columnDefs: [{
                targets: [4, 6],
                render: function (data, type, full) {
                    var formmatedvalue = data.toFixed(2)
                    return formmatedvalue;
                }
            }],
            data: data
        });
    },

    SelecionarTabelaPreco: function (codTpr) {
        $('#modal-tabelapreco').modal('hide');

        $('#txtTabelaPreco').val(codTpr);
    },

    ObterProduto: function () {
        Sistema.ConsultaProduto.Load(true);

        var codProduto = $('#txtCodProduto').val();
        var descProduto = $('#txtDescProduto').val();
        var codTpr = $('#txtTabelaPreco').val();

        $.ajax({
            url: "/ConsultaProduto/ObterProduto",
            type: "GET",
            data: {
                codProduto: codProduto,
                descProduto: descProduto,
                codTpr: codTpr
            },
            success: function (data) {
                Sistema.ConsultaProduto.Load(false);
                Sistema.ConsultaProduto.GridBuscaProdutos(data.Data);
            }
        });
    },

    AbrirModal: function (modal) {
        switch (modal) {
            case '#modal-tabelapreco': Sistema.ConsultaProduto.GridTabelaPrecos();
                break;
            default:
        }

        $(modal).modal('show');
    }
}

function Enter(e) {
    if (e.keyCode == 13) {
        switch (document.activeElement.name) {
            case 'txtCodProduto':
            case 'txtDescProduto':
            case 'txtTabelaPreco':
                Sistema.ConsultaProduto.ObterProduto();
                break;
            default:
        }
    }
}