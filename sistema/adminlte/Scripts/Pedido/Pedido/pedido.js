var Sistema = Sistema || {};

Sistema.Pedido = {
    Init: function () {
        var hoje = new Date();

        if (!$('#txtDataPedido').val())
            $('#txtDataPedido').val(hoje.toLocaleDateString('pt-BR'));
    },

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

    GridPedido: function (data) {
        $('#tblPedidos').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            sort: false,
            searching: false,
            data: data,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            columns: [
                {
                    sWidth: "5px",
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarPedido(" + full.Codigo + ")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código", sWidth: "10px" },
                { data: "DataEmissao", title: "Emissão", sWidth: "100px" },
                { data: "CodCliente", title: "Cód. Cliente", sWidth: "100px" },
                { data: "NomeCliente", title: "Cliente", sWidth: "100px" },
                { data: "DescSituacaoPedido", title: "Situação", sWidth: "70px" },
            ],
            columnDefs: [
                {
                    targets: 2, render: function (data) {
                        return moment(data).format('DD/MM/YYYYY');
                    }
                }
            ]

        });
    },

    GridCliente: function (data) {
        $('#tblClientes').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            sort: false,
            searching: false,
            data: data,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarCliente(" + full.Codigo + ")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Cód.", sWidth: "10px" },
                { data: "Nome", title: "Nome", sWidth: "300px" },
                { data: "Apelido", title: "Apelido", sWidth: "300px" },
                { data: "InscricaoEstadual", title: "Insc. Est.", sWidth: "70px" },
                { data: "CPF", title: "CPF", sWidth: "70px" },
                { data: "CodGrupo", title: "Grp. Empresa", sWidth: "70px" },
                { data: "Situacao", title: "Situação", sWidth: "70px" },
            ],

        });
    },

    GridModalAvalista: function () {
        $('#tblModalAvalistas').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            searching: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            ajax: {
                url: '/Pedido/ObterTodosClientes',
                type: 'GET',
                dataType: 'json',
                dataSrc: "Data"
            },
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            columns: [
                {
                    render: function (data, type, full, meta) {
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarAvalista(\"" + full.Codigo + "\")'><i class='fa fa-check'></i></button>";
                    },
                    sWidth: "5px"
                },
                { data: "Codigo", title: "Código", sWidth: "20px" },
                { data: "Nome", title: "Nome", sWidth: "100px" },
            ],

        });
    },

    GridGrupo: function () {
        $('#tblGrupos').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            ajax: {
                url: '/Pedido/ObterTodosGrupos',
                type: 'GET',
                dataType: 'json',
                dataSrc: "Data"
            },
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarGrupo(" + full.Codigo + ")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código" },
                { data: "Nome", title: "Nome" },
            ],

        });
    },

    GridRepresentantes: function () {
        $('#tblRepresentantes').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            ajax: {
                url: '/Pedido/ObterTodosRepresentantes',
                type: 'GET',
                dataType: 'json',
                dataSrc: "Data"
            },
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarRepresentante(" + full.Codigo + ")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código" },
                { data: "Nome", title: "Nome" },
            ],

        });
    },

    GridTransportadora: function () {
        $('#tblTransp').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            ajax: {
                url: '/Pedido/ObterTodosTransportadora',
                type: 'GET',
                dataType: 'json',
                dataSrc: "Data"
            },
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarTransportadora(" + full.Codigo + ")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código" },
                { data: "Nome", title: "Nome" },
            ],

        });
    },

    GridTransacao: function () {
        $('#tblTransacao').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            ajax: {
                url: '/Pedido/ObterTodasTransacoes',
                type: 'GET',
                dataType: 'json',
                dataSrc: "Data"
            },
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarTransacao(" + full.Codigo + ")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código" },
                { data: "Descricao", title: "Descrição" },
            ],

        });
    },

    GridFormaPgto: function () {
        $('#tblFormaPgto').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            ajax: {
                url: '/Pedido/ObterTodosFormaPgto',
                type: 'GET',
                dataType: 'json',
                dataSrc: "Data"
            },
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarFormaPgto(" + full.Codigo + ")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código" },
                { data: "Descricao", title: "Descrição" },
            ],

        });
    },

    GridCondPgto: function () {
        var codForma = $('#txtCodFormaPgto').val();

        $('#tblCondPgto').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            ajax: {
                url: '/Pedido/ObterTodosCondicaoPgto',
                type: 'GET',
                data: { formaPgto: codForma },
                dataType: 'json',
                dataSrc: "Data"
            },
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarCondPgto(\"" + full.Codigo + "\")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código", type: "string" },
                { data: "Descricao", title: "Descrição", type: "string" },
                { data: "TipoEspecial", title: "Tipo Especial", type: "string" },
            ],

        });
    },

    GridVeiculos: function (codTransportadora) {
        $('#tblVeiculos').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            ajax: {
                url: '/Pedido/ObterVeiculos',
                type: 'GET',
                data: { codTransportadora: codTransportadora },
                dataType: 'json',
                dataSrc: "Data"
            },
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarVeiculo(\"" + full.PlacaVeiculo + "\")'><i class='fa fa-check'></i></button>";
                    }
                },
                { data: "PlacaVeiculo", title: "Placa", type: "string" },
            ],

        });
    },

    GridItemPedido: function (data) {
        $('#tblItens').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            ordering: false,
            searching: false,
            lengthChange: false,
            paging: false,
            info: false,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            data: data,
            columns: [
                {
                    render: function (data, type, full, meta) {
                        if (full.Pedido == null || (full.Pedido.SituacaoPedido == 9 && full.Pedido.PodeAlterar == 'S')) {
                            return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.EditarProduto(\"" + full.CodigoProduto + "\", \"" + full.CodigoDerivacao + "\", \"" + full.DescricaoProduto + "\", \"" + full.Sequencia + "\", \"" + full.QuantidadePedido + "\", \"" + full.ValorAcrescimoUsuario + "\", \"" + full.PercentualAcrescimoUsuario + "\", \"" + full.ValorDescontoUsuario + "\", \"" + full.PercentualDescontoUsuario + "\")'><i class='fa fa-edit'></i></button><button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.DeleteItem(\"" + full.CodigoProduto + "\", \"" + full.ValorFrete + "\")'><i class='fa fa-window-close'></i></button>";
                        } else {
                            return "";
                        }
                        //else
                        //if (full.Pedido.SituacaoPedido == 4) {
                        //    return "<button type='button' disabled class='btn btn-default btn-xs' onclick='Sistema.Pedido.EditarProduto(\"" + full.CodigoProduto + "\", \"" + full.CodigoDerivacao + "\", \"" + full.DescricaoProduto + "\", \"" + full.Sequencia + "\")'><i class='fa fa-edit'></i></button><button type='button' disabled class='btn btn-default btn-xs' onclick='Sistema.Pedido.DeleteItem(\"" + full.CodigoProduto + "\", \"" + full.ValorFrete + "\")'><i class='fa fa-window-close'></i></button>";
                        //} else {
                        //    return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.EditarProduto(\"" + full.CodigoProduto + "\", \"" + full.CodigoDerivacao + "\", \"" + full.DescricaoProduto + "\", \"" + full.Sequencia + "\")'><i class='fa fa-edit'></i></button><button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.DeleteItem(\"" + full.CodigoProduto + "\", \"" + full.ValorFrete + "\")'><i class='fa fa-window-close'></i></button>";
                        //}
                    }
                },
                { data: "CodigoProduto", title: "Código" },
                { data: "DescricaoProduto", title: "Produto" },
                { data: "CodigoDerivacao", title: "Deriv." },
                { data: "CodigoDep", title: "Dep." },
                { data: "DataEntrega", title: "Entrega" },
                { data: "CodigoTRP", title: "Tab. Preço" },
                { data: "PrecoBase", title: "Preço Base" },
                { data: "PrecoUnitario", title: "Preço Venda" },
                { data: "QuantidadePedido", title: "Qtde." },
                { data: "ValorFrete", title: "Frete" },
                { data: "PercentualDescontoUsuario", title: "% Desc." },
                { data: "ValorDescontoUsuario", title: "Vlr. Desc." },
                { data: "PercentualAcrescimoUsuario", title: "% Acre" },
                { data: "ValorAcrescimoUsuario", title: "Vlr. Acre." },
                { data: "ValorLiquido", title: "Vlr. Liq." },
            ],
            columnDefs: [
                {
                    render: function (data, type, row) {
                        return data.toFixed(3).replace('.', ',');
                    },
                    targets: [7, 8]
                },
                {
                    render: function (data, type, row) {
                        return data.toFixed(2).replace('.', ',');
                    },
                    targets: [10, 11, 12, 13, 14, 15]
                },
                {
                    render: function (data, type, row) {
                        if (data.includes('-')) {
                            var nData = data.substring(10, 0).split('-');
                            return nData[2] + '/' + nData[1] + '/' + nData[0];
                        } else {
                            return data.substring(10, 0).split('-');
                        }
                    },
                    targets: [5]
                }
            ]
        });
    },

    GridAvalista: function (data) {
        $('#tblAvalistas').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            ordering: false,
            searching: false,
            lengthChange: false,
            paging: false,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            data: data,
            columns: [
                {
                    render: function (data, type, full, meta) {
                        //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                        return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.EditarAvalista(\"" + full.Codigo + "\", \"" + full.Nome + "\")'><i class='fa fa-edit'></i></button><button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.RemoverAvalista(\"" + full.Codigo + "\")'><i class='fa fa-window-close'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código" },
                { data: "Nome", title: "Nome" }
            ],
        });
    },

    GridParcelas: function (data) {
        $('#tblParcelas').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            ordering: false,
            searching: false,
            lengthChange: false,
            paging: false,
            info: false,
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
            data: data,
            columns: [
                {
                    render: function (data, type, full, meta) {
                        if (full.Pedido != null && full.Pedido.SituacaoPedido == 9)
                            return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.EditarParcela(\"" + full.Codigo + "\", \"" + full.SequenciaParcela + "\", \"" + full.DataVencimento + "\", \"" + full.Percentual.toFixed(2) + "\", \"" + full.ValorParcela.toFixed(2) + "\")'><i class='fa fa-edit'></i></button><button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.RemoverParcela(\"" + full.SequenciaParcela + "\")'><i class='fa fa-window-close'></i></button>";
                        else if ($('#condicao_pagamento_manutencao_parcela').val() == 'N')
                            return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.EditarParcela(\"" + full.Codigo + "\", \"" + full.SequenciaParcela + "\", \"" + full.DataVencimento + "\", \"" + full.Percentual.toFixed(2) + "\", \"" + full.ValorParcela.toFixed(2) + "\")' disabled><i class='fa fa-edit'></i></button><button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.RemoverParcela(\"" + full.SequenciaParcela + "\")' disabled><i class='fa fa-window-close'></i></button>";
                        else
                            return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.EditarParcela(\"" + full.Codigo + "\", \"" + full.SequenciaParcela + "\", \"" + full.DataVencimento + "\", \"" + full.Percentual.toFixed(2) + "\", \"" + full.ValorParcela.toFixed(2) + "\")'><i class='fa fa-edit'></i></button><button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.RemoverParcela(\"" + full.SequenciaParcela + "\")'><i class='fa fa-window-close'></i></button>";
                    }
                },
                { data: "Codigo", title: "Código", visible: false },
                { data: "SequenciaParcela", title: "Sequência" },
                { data: "DataVencimento", title: "Vencimento" },
                { data: "Percentual", title: "% Parcela" },
                { data: "ValorParcela", title: "Vlr. Parcelas" }
            ],
            columnDefs: [
                {
                    render: function (data, type, row) {
                        return SapiensJS.Util.formatReal(data.toFixed(2));
                    },
                    targets: [4, 5]
                },
                {
                    render: function (data, type, row) {
                        return data.substring(10, 0);
                    },
                    targets: [3]
                }
            ]
        });
    },

    GridBuscaProdutos: function () {
        $('#tblBuscaProdutos').dataTable({
            destroy: true,
            paging: true,
            processing: true,
            lengthMenu: [10, 25, 50],
            pageLength: 10,
            "dom": '<"top"if>rt<"bottom"lp><"clear">',
            language: {
                paginate: { "previous": "Anterior", "next": "Próximo" },
                search: "Pesquisar",
                info: "Exibindo página _PAGE_ de _PAGES_",
                infoEmpty: "",
                zeroRecords: "Nenhum registro",
                lengthMenu: "Exibir _MENU_ registros por página",
            },
        });
    },

    ChangeGrupo: function () {
        var codigo = $('#txtCodGrupo').val();
        Sistema.Pedido.SelecionarGrupo(codigo);
    },

    ChangeCliente: function () {
        var codigo = $('#txtCodCliente').val();
        Sistema.Pedido.SelecionarCliente(codigo);
    },

    ChangeRepresentante: function () {
        var codigo = $('#txtCodRepresentante').val();
        Sistema.Pedido.SelecionarRepresentante(codigo);
    },

    ChangeTransportadora: function () {
        var codigo = $('#txtCodTransportadora').val();
        Sistema.Pedido.SelecionarTransportadora(codigo);
    },

    ChangeTransacao: function () {
        var codigo = $('#txtCodTransacao').val();
        Sistema.Pedido.SelecionarTransacao(codigo);
    },

    ChangeFormaPgto: function () {
        var codigo = $('#txtCodFormaPgto').val();
        Sistema.Pedido.SelecionarFormaPgto(codigo);
    },

    ChangeCondPgto: function () {
        var codCond = $('#txtCodCondPgto').val();
        Sistema.Pedido.SelecionarCondPgto(codCond);
    },

    ChangeVeiculo: function () {
        var placa = $('#txtCodVeiculo').val();
        Sistema.Pedido.SelecionarVeiculo(placa);
    },

    SelecionarAvalista: function (codigo) {
        $('#modal-avalista').modal('hide');

        $.ajax({
            url: '/Pedido/ObterAvalista',
            type: 'GET',
            data: { Codigo: codigo },
            success: function (data) {
                Sistema.Pedido.GridAvalista(data.Data);
            }
        });
    },

    EditarAvalista: function (codigo, nome) { },

    SelecionarCliente: function (codigo) {
        $('#modal-cliente').modal('hide');

        $.ajax({
            url: '/Pedido/ObterCliente',
            type: 'GET',
            data: { Codigo: codigo },
            success: function (data) {
                $('#txtCodGrupo').val(data.Data.CodGrupo);
                $('#txtDescGrupo').val(data.Data.NomeGrupo);
                $('#txtCodCliente').val(data.Data.Codigo);
                $('#txtDescCliente').val(data.Data.Nome);
            }
        });
    },

    SelecionarGrupo: function (codigo) {
        $('#modal-grupo').modal('hide');

        $.ajax({
            url: '/Pedido/ObterGrupo',
            type: 'GET',
            data: { Codigo: codigo },
            success: function (data) {
                $('#txtCodGrupo').val(data.Data.CodGrupo);
                $('#txtDescGrupo').val(data.Data.NomeGrupo);
                $('#txtCodCliente').val(data.Data.Codigo);
                $('#txtDescCliente').val(data.Data.Nome);
            }
        });
    },

    SelecionarRepresentante: function (codigo) {
        $('#modal-representante').modal('hide');

        $.ajax({
            url: '/Pedido/ObterRepresentante',
            type: 'GET',
            data: { Codigo: codigo },
            success: function (data) {
                $('#txtCodRepresentante').val(data.Data.Codigo);
                $('#txtDescRepresentante').val(data.Data.Nome);
            }
        });
    },

    SelecionarTransportadora: function (codigo) {
        $('#modal-transportadora').modal('hide');

        $.ajax({
            url: '/Pedido/ObterTransportadora',
            type: 'GET',
            data: { Codigo: codigo },
            success: function (data) {
                $('#txtCodTransportadora').val(data.Data.Codigo);
                $('#txtDescTransportadora').val(data.Data.Nome);
            }
        });
    },

    SelecionarTransacao: function (codigo) {
        $('#modal-transacao').modal('hide');

        $.ajax({
            url: '/Pedido/ObterTransacao',
            type: 'GET',
            data: { Codigo: codigo },
            success: function (data) {
                $('#txtCodTransacao').val(data.Data.Codigo);
                $('#txtDescTransacao').val(data.Data.Descricao);
            }
        });
    },

    SelecionarFormaPgto: function (codigo) {
        $('#modal-formapgto').modal('hide');

        $.ajax({
            url: '/Pedido/ObterFormaPgto',
            type: 'GET',
            data: { Codigo: codigo },
            success: function (data) {
                $('#txtCodFormaPgto').val(data.Data.Codigo);
                $('#txtDescFormaPgto').val(data.Data.Descricao);
            }
        });
    },

    SelecionarCondPgto: function (codigo, carregandoPedido) {
        Sistema.Pedido.Load(true);

        $('#modal-condpgto').modal('hide');

        var codForma = $('#txtCodFormaPgto').val();

        if (codForma == null || codForma == "" || codForma == undefined) {
            Swal.fire('Atenção', 'Selecione uma Forma de Pagamento', 'warning');
            return;
        }

        $.ajax({
            url: '/Pedido/ObterCondicaoPgto',
            type: 'GET',
            data: { codFiltro: codigo, codFormaPagamento: codForma },
            success: function (data) {
                if (data != null && data != "") {
                    if (data.Data.retorno != null && data.Data.retorno != undefined) {
                        Swal.fire('Atenção', data.Data.retorno, 'error');
                        return;
                    }

                    var sumPercentual = 0.00;

                    $('#txtCodCondPgto').val(data.Data.Codigo);
                    $('#txtDescCondPgto').val(data.Data.Descricao);
                    $('#condicao_pagamento_aux').val(data.Data.Codigo);
                    $('#condicao_pagamento_Especial').val(data.Data.TipoEspecial);
                    $('#condicao_pagamento_total_parcelas').val(data.Data.QuantidadeParcelas);
                    $('#condicao_pagamento_manutencao_parcela').val(data.Data.ManutencaoParcela);
                    $('#condicao_pagamento_taxa_juros').val(data.Data.TaxaJuros);

                    if (carregandoPedido != true) {
                        Sistema.Pedido.CalcularParcela();
                        //RecalcularTodasParcelas();
                        Sistema.Pedido.RecalcularItensPedido(false);
                    } else {
                        $('#btnAddParcela').prop("disabled", false);

                        $('#tblParcelas > tbody > tr').each(function () {
                            sumPercentual += SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(3)').html());
                        });

                        //$("#containerParcela").html(response);

                        var percentual = sumPercentual != null ? sumPercentual.toFixed(2) : 0.00;
                        percentRest = 100 - percentual;

                        $('#txtPercentRestante').val(percentRest.toFixed(2));

                        if (percentual != 100)
                            $('#lblparcvalida').val('Não');
                        else
                            $('#lblparcvalida').val('Sim');
                    }

                    if (data.Data.ManutencaoParcela == "N") {
                        $('#btnAddParcela').prop("disabled", true);
                        Sistema.Pedido.RemoverParcelas();
                    } else {
                        $('#btnAddParcela').prop("disabled", false);
                    }

                    Sistema.Pedido.Load(false);
                } else {
                    $('#txtCodCondPgto').val(null);
                    $('#txtDescCondPgto').val(null);
                    $('#condicao_pagamento_aux').val(null);
                    $('#condicao_pagamento_Especial').val(null);
                    $('#condicao_pagamento_total_parcelas').val(null);
                    $('#condicao_pagamento_manutencao_parcela').val(null);
                    $('#condicao_pagamento_taxa_juros').val(null);

                    Sistema.Pedido.Load(false);
                }
            }
        });
    },

    SelecionarVeiculo: function (codigo) {
        $('#modal-veiculo').modal('hide');

        var codTransportadora = $('#txtCodTransportadora').val();

        $.ajax({
            url: '/Pedido/ObterVeiculo',
            type: 'GET',
            data: { codFiltro: codigo, codigoTransportadora: codTransportadora },
            success: function (data) {
				if(data.Success) {
					$('#txtCodVeiculo').val(data.Data.PlacaVeiculo);
					$('#txtDescVeiculo').val(data.Data.PlacaVeiculo);
				} else {
					$('#txtCodVeiculo').val(codigo);
					$('#txtDescVeiculo').val(codigo);
				}
            }
        });
    },

    EditarProduto: function (Codigo, codDeriv, descProd, sequencia, qtdeProduto, vlrAcre, percAcre, vlrDesc, percDesc) {
        var emissao = $('#txtDataPedido').val().split('-');

        var dataEmissao = emissao[2] + '/' + emissao[1] + '/' + emissao[0];
        var condPgto = $('#txtCodCondPgto').val();
        var codCli = $('#txtCodCliente').val();
        var transacao = $('#txtCodTransacao').val();
        $('#sequencia_item').val(sequencia);

        $.ajax({
            url: '/Pedido/ObterProduto',
            type: 'GET',
            data: {
                codProduto: Codigo,
                descProduto: descProd,
                dataEmissao: dataEmissao,
                condPgto: condPgto,
                codCli: codCli,
                transProd: transacao,
                codDer: codDeriv,
                qtdProduto: qtdeProduto,
                vlrAcre: vlrAcre,
                percAcre: percAcre,
                vlrDesc: vlrDesc,
                percDesc: percDesc
            },
            success: function (data) {
                Sistema.Pedido.AbrirModal('#modal-add-item');

                $('#divBtnAddItem').hide();
                $('#divBtnEditarItem').show();

                $.each(data.Data.ListaDerivacao, function (index, value) {
                    $('#cbDerivacao').append("<option value='" + value.Codigo + "'>" + value.Nome + "</option>");
                });

                $.each(data.Data.ListaDepositos, function (index, value) {
                    $('#cbDeposito').append("<option value='" + value.Deposito + "'>" + value.Deposito + "</option>");
                });

                $('#txtCodProduto').val(data.Data.CodigoProduto);
                $('#cbDerivacao').val(data.Data.CodigoDerivacao);
                $('#cbDeposito').val(data.Data.CodigoDep);
                $('#txtDescProd').val(data.Data.DescricaoProduto);
                $('#txtTabelaPreco').val(data.Data.CodigoTRP);
                $('#txtPrecoBase').val(data.Data.PrecoBase.toFixed(3));
                $('#txtPrecoUnit').val(data.Data.PrecoUnitario.toFixed(3));
                $('#txtPerAcre').val(data.Data.PercentualAcrescimoUsuario);
                $('#txtVlrAcre').val(data.Data.ValorAcrescimoUsuario);
                $('#txtPerDesc').val(data.Data.PercentualDescontoUsuario);
                $('#txtVlrDesc').val(data.Data.ValorDescontoUsuario);
                $('#txtVlrLiq').val(data.Data.ValorLiquido.toFixed(3));

                //var entrega = formataDataAmericana(data.Data.DataEntrega);

                //if (entrega != null)
                //    $('#txtEntrega').text(entrega);
                //else
                //    $('#txtEntrega').val(new Date());

                var entrega = data.Data.DataEntrega.split('/');

                var dataEntrega = entrega[2] + '-' + entrega[1] + '-' + entrega[0];

                $('#txtEntrega').val(dataEntrega);

                $('#txtQtdeProduto').val(data.Data.QuantidadePedido);
                $('#txtQtdeProduto').focus();
            }
        });
    },

    SelecionarProduto: function (Codigo, codDeriv, descProd) {
        if (Codigo == null || Codigo == '' || Codigo == undefined)
            return false;

        Sistema.Pedido.Load(true);

        var emissao = $('#txtDataPedido').val().split('-');

        var dataEmissao = emissao[2] + '/' + emissao[1] + '/' + emissao[0];

        var condPgto = $('#txtCodCondPgto').val();
        var codCli = $('#txtCodCliente').val();
        var transacao = $('#txtCodTransacao').val();

        $.ajax({
            url: '/Pedido/ObterProduto',
            type: 'GET',
            data: {
                codProduto: Codigo,
                descProduto: descProd,
                dataEmissao: dataEmissao,
                condPgto: condPgto,
                codCli: codCli,
                transProd: transacao,
                codDer: codDeriv
            },
            success: function (data) {
				if(data.Success == false) {
                    Swal.fire('Atenção', 'Produto não encontrado.', 'error');

                    setTimeout(function () {
                        $('#txtQtdeProduto').focus();
                    }, 500);

                    $('#txtCodProduto').val(null);

					return;
				} else
                if (data.Data == 0) {
                    Swal.fire('Atenção', 'Produto com Preço Base zerado.', 'error');
                    return;
                } else {
                    $('#modal-produto').modal('hide');

                    setTimeout(function () {
                        $('#txtQtdeProduto').focus();
                    }, 500);
                    
                    $.each(data.Data.ListaDerivacao, function (index, value) {
                        $('#cbDerivacao').append("<option value='" + value.Codigo + "'>" + value.Nome + "</option>");
                    });

                    $.each(data.Data.ListaDepositos, function (index, value) {
                        $('#cbDeposito').append("<option value='" + value.Deposito + "'>" + value.Deposito + "</option>");
                    });

                    $('#txtCodProduto').val(data.Data.CodigoProduto);
                    $('#cbDerivacao').val(data.Data.CodigoDerivacao);
                    $('#cbDeposito').val(data.Data.CodigoDep);

                    //$('#cbDeposito option').filter(function () {
                    //    return this.value == data.Data.CodigoDep;
                    //}).attr('selected', true);

                    $('#txtDescProd').val(data.Data.DescricaoProduto);
                    $('#txtTabelaPreco').val(data.Data.CodigoTRP);
                    $('#txtPrecoBase').val(SapiensJS.Util.formatReal(data.Data.PrecoBase.toFixed(3)));
                    $('#txtPrecoUnit').val(SapiensJS.Util.formatReal(data.Data.PrecoUnitario.toFixed(3)));
                    $('#txtPerAcre').val(data.Data.PercentualAcrescimoUsuario);
                    $('#txtVlrAcre').val(data.Data.ValorAcrescimoUsuario);
                    $('#txtPerDesc').val(data.Data.PercentualDescontoUsuario);
                    $('#txtVlrDesc').val(data.Data.ValorDescontoUsuario);
                    $('#txtVlrLiq').val(SapiensJS.Util.formatReal(data.Data.ValorLiquido.toFixed(3)));

                    var entrega = data.Data.DataEntrega.split('/');

                    var dataEntrega = entrega[2] + '-' + entrega[1] + '-' + entrega[0];

                    $('#txtEntrega').val(dataEntrega);

                    //var entrega = new Date(parseInt(.substr(6)));

                    //if (entrega != null)
                    //$('#txtEntrega').val(data.Data.DataEntrega);
                    //else
                    //    $('#txtDataEntrega').val(new Date());

                    $('#txtQtdeProduto').focus();

                    Sistema.Pedido.Load(false);
                }
            }
        });
    },

    ChangeQtdeProduto: function () {
        var qtde = parseInt($('#txtQtdeProduto').val());
        var preBas = parseFloat($('#txtPrecoBase').val());
        var total = 0.00;

        total = preBas * qtde;

        $('#txtVlrLiq').val(SapiensJS.Util.formatReal(total.toFixed(3)));
    },

    SelecionarPedido: function (codigo) {
        Sistema.Pedido.Load(true);

        $('#modal-pedido').modal('hide');

        var codPedido = codigo;
        //$("#divAcaoPedido").addClass("divDisabled");

        $.ajax({
            url: '/Pedido/ObterPedido/',
            data: { codigoPedido: codPedido },
            json: true,
            async: false,
            success: function (response) {
                var result = response.Data;

                if (result != "" && result != null) {
                    //popupPedido.Hide();
                    //btnImprimirPedido.SetVisible(false);

                    //carregandoPedido = true;
                    //carregarProcurador = true;
                    //carregarAvalista = true;
                    //carregarParcela = true;

                    //Limpando Tela de Pedido
                    //LimparDadosConvenio();
                    //SistemaWeb.ClientePedido.limparClientePedido();
                    //limparGrupoEmpresaPedido();
                    //LimparRepresentante();
                    //LimparTransportadora();
                    //LimparDadosVeiculo();
                    //LimparDadosTransacao();

                    //Limpando Cálculos
                    LimparTotais();
                    //lblQtdSacas.SetText(result.QtdSaca);
                    $('#txtObsPedido').val("");
                    $('#txtObsNf').val("");

                    $('#txtCodPedido').val(result.Codigo);

                    var emissao = result.DtEmissao.split('/');

                    var dataEmissao = emissao[2] + '-' + emissao[1] + '-' + emissao[0];

                    $('#txtDataPedido').val(dataEmissao);

                    //transacao_produto_codMdr.value = result.CodMdr;

                    $('#txtCodGrupo').val(result.GrupoEmpresaCodigo);
                    $('#txtDescGrupo').val(result.GrupoEmpresaDescricao);

                    $('#txtCodCliente').val(result.ClienteCodigo);
                    $('#txtDescCliente').val(result.ClienteNome);

                    $('#txtCodRepresentante').val(result.RepresentanteCodigo);
                    $('#txtDescRepresentante').val(result.RepresentanteNome);

                    $('#txtCodTransportadora').val(result.CodigoTransportadora);
                    $('#txtDescTransportadora').val(result.NomeTransportadora);

                    $('#txtCodVeiculo').val(result.NomePlaca);
                    $('#txtDescVeiculo').val(result.NomeTransportadora + ' - ' + result.NomePlaca);

                    $('#cbFrete').val(result.CIFFOB);
                    $('#vlrFrete').val(result.ValorFrete);
                    //valorFreteAux.value = result.ValorFrete;

                    $('#txtStatus').val(result.Situacao);

                    $('#txtCodTransacao').val(result.TransacaoCodigo);
                    //transacao_aux.value = result.TransacaoCodigo;
                    $('#txtDescTransacao').val(result.TransacaoDescricao);
                    //txtCPRTransProd.SetText(result.TransacaoCodigoTipoCPR);

                    $('#txtCodFormaPgto').val(result.FormaPagamentoCodigo);
                    $('#txtDescFormaPgto').val(result.FormaPagamentoDescricao);
                    //txtCPRFormPgto.SetText(result.FormaPagamentoTipoCPR);

                    $('#txtCodCondPgto').val(result.CondicaoPagamentoCodigo);
                    $('#txtDescCondPgto').val(result.CondicaoPagamentoDescricao);
                    $('#condicao_pagamento_aux').val(result.CondicaoPagamentoCodigo);

                    $('#condicao_pagamento_ini_jur').val(result.DataIniJur);
                    $('#condicao_pagamento_taxa_juros').val(result.TaxaJuros);
                    //produto_controlado.value = result.RepRec;
                    //cod_tecnico_responsavel.value = result.RepresentanteCodigo;

                    if (result.SituacaoCodigo == 9) {
                        $('#btnAddParcela').prop("disabled", false);
                        $('#btnAddAvalista').prop("disabled", false);

                        Sistema.Pedido.SelecionarCondPgto(result.CondicaoPagamentoCodigo, true);
                        Sistema.Pedido.ObterTodosParcelas(result.Codigo);

                        $('#condicao_pagamento_total_parcelas').val(result.QtdParcelas);
                        $('#condicao_pagamento_manutencao_parcela').val(result.ManutencaoParcelas);

                        //condicao_pagamento_CPR.value = result.CondicaoPagamentoTipoCPR;
                        $('#condicao_pagamento_Especial').val(result.CondicaoPagamentoEspecial);

                        //if (result.CondicaoPagamentoTipoCPR != "N") {
                        //    SistemaWeb.TabelaCPR.ObterTodosTabelaCPR(result.CondicaoPagamentoCodigo, result.TabelaCPR, result.CondicaoPagamentoTipoCPR);
                        //}
                        //else {
                        //    AtivarTabParcela(true);
                        //}

                        //if (result.CondicaoPagamentoTipoCPR == "I" || result.CondicaoPagamentoTipoCPR == "T") {
                        //    cbTabelaCpr.SetEnabled(true);
                        //    cbTabelaCpr.SetValue(result.TabelaCPR);
                        //}
                        //else
                        //    cbTabelaCpr.SetEnabled(false);

                        //if (result.CondicaoPagamentoEspecial == "S") {
                        //    AtivarTabParcela(true);
                        //    SelecionaTabelaCPRAtual(result.situacaoPedido == 9);
                        //} else {
                        //    AtivarTabParcela(false);
                        //}
                    }
                    else {
                        Sistema.Pedido.ObterTodosParcelas(result.Codigo);
                        $('#btnAddParcela').prop("disabled", true);
                        $('#btnAddAvalista').prop("disabled", true);
                    }
                    //else {
                    //    Sistema.Pedido.SelecionarCondPgto(result.CondicaoPagamentoCodigo);

                    //    if (result.CondicaoPagamentoTipoCPR != "N") {
                    //        SistemaWeb.TabelaCPR.ObterTodosTabelaCPR(result.CondicaoPagamentoCodigo, 0, result.CondicaoPagamentoTipoCPR);

                    //        if (result.CondicaoPagamentoTipoCPR == "I" || result.CondicaoPagamentoTipoCPR == "T") {
                    //            cbTabelaCpr.SetEnabled(true);
                    //            cbTabelaCpr.SetValue(result.CodigoTPR);
                    //        }
                    //        else
                    //            cbTabelaCpr.SetEnabled(false);
                    //    }
                    //    else {
                    //        AtivarTabParcela(true);

                    //        cbTabelaCpr.SetEnabled(true);
                    //        cbTabelaCpr.AddItem("Selecione Tabela CPR", "0");

                    //        //for (var i = 0; i < result.length; i++) {
                    //        var texto = result.NomeTPR != null ? result.NomeTPR : "";
                    //        cbTabelaCpr.AddItem(texto, result.CodigoTPR);
                    //        //}

                    //        cbTabelaCpr.SetValue(result.CodigoTPR);
                    //    }
                    //}

                    //condicao_pagamento_arras.value = result.Arras;

                    //if (result.CondicaoPagamentoTipoCPR != "N") {
                    //    SistemaWeb.TabelaCPR.ObterTodosTabelaCPR(result.CondicaoPagamentoCodigo, result.TabelaCPR);
                    //}
                    //else {
                    //    AtivarTabParcela(true);
                    //}

                    //if (result.CondicaoPagamentoEspecial == "S") {
                    //    AtivarTabParcela(true);
                    //} else {
                    //    AtivarTabParcela(false);
                    //}

                    //if (result.PedBlo == "S")
                    //    $("#txtObsAprovacao").addClass("txtBoxObservacao");
                    //else
                    //    $("#txtObsAprovacao").removeClass("txtBoxObservacao");

                    //txtObsAprovacao.SetText(result.ObsAprovacao);
                    //txtObsAprovacao.GetMainElement().title = result.ObsAprovacao;

                    $('#txtObsPedido').val(result.ObsPedido);
                    $('#txtObsNf').val(result.ObsNF);
                    Sistema.Pedido.ObterTodosItemPedido(result.Codigo);
                    //ObterProcuradores(false);

                    //ObterTodosAvalistas(result.Codigo);      --> PRECISA CRIAR AINDA!!!!!!! <--

                    //if (result.CondicaoPagamentoTipoCPR != "N") {
                    //    SelecionaTabelaCPRAtual(result.situacaoPedido == 9);

                    //    if (result.SituacaoCodigo == 9) {
                    //        SistemaWeb.Pedido.RecalcularItensPedido();
                    //    } else {
                    //        ObterTodasParcelas(result.Codigo);
                    //    }
                    //}
                    //else
                    //    ObterTodasParcelas(result.Codigo);

                    //gridViewParcelas.Refresh();
                    //gridViewProcuradores.Refresh();

                    //ValidarAba();

                    $('#lblVlrLiquido').val(SapiensJS.Util.formatReal(result.ValorLiquido));
                    //lblQtdSacas.SetText(result.QtdSaca);
                    $('#lblVlrFrete').val(result.ValorFrete);
                    //text_valor_total_cpr.value = result.QtdSaca;

                    //TRATANDO O BLOQUEIO DOS CAMPOS
                    //$('#div_imprimir').show();
                    //btnImprimir.SetEnabled(true);
                    //SistemaWeb.Relatorio.ObterTodos();

                    //recalcularItens = false;
                    if (result.SituacaoCodigo == 9) {
                        if (result.PodeAlterar == "S") {
                            $('#divFecharPedido').show();
                            //btnImprimirPedido.SetVisible(true);
                            Sistema.Pedido.HabilitarPedido();
                            recalcularItens = true;
                        }
                        $('#divCancelarPedido').hide();
                        $('#divEditarPedido').hide();

                        //Sistema.Pedido.RecalcularItensPedido(false);

                    } else if (result.SituacaoCodigo == 1 && result.PodeAlterar == "S") {
                        //Sistema.Pedido.SelecionarCondPgto(result.CondicaoPagamentoCodigo);
                        $('#divEditarPedido').show();
                        $('#divFecharPedido').hide();
                        $('#divCancelarPedido').show();
                        $('#divImprimirPedido').show();

                        Sistema.Pedido.DesabilitarPedido();

                    } else if ((result.SituacaoCodigo == 1 || result.SituacaoCodigo == 2) && result.PodeAlterar == "N") {
                        //var lbl = lblSt.GetText();
                        //lblSt.SetText(lbl + " / " + "NF Cobrança Gerada");
                        //lblSt.SetText(lbl);

                        $('#divCancelarPedido').hide();
                        $('#divEditarPedido').hide();
                        $('#divImprimirPedido').show();

                        Sistema.Pedido.DesabilitarPedido();
                    } else {
                        $('#divCancelarPedido').hide();
                        $('#divEditarPedido').hide();
                        $('#divFecharPedido').hide();
                        $('#divImprimirPedido').hide();

                        Sistema.Pedido.DesabilitarPedido();
                    }

                    //Refresh nas grids
                    //gvEditing.Refresh();
                    //if (result.RefreshParcelas == "S") {
                    //    if (gridViewParcelas != null)
                    //        gridViewParcelas.Refresh();
                    //} else {
                    //    if (gridViewParcelas != null)
                    //        gridViewParcelas.Refresh();
                    //}

                    if (result.CIFFOB == "X" || !result.CIFFOB) {
                        $('#lblVlrFrete').val('0,00');
                    }

                    //codPedidoAux.value = result.Codigo;

                    //gvEditing.cptotalLiquido = result.SomatorioTotalLiq;

                } else {

                    Swal.fire("Atenção", "Pedido não localizado.", "error");
                    //txtNumPedido.SetText(codPedidoAux.value);

                    //carregandoPedido = false;
                    //codPedidoAux.value = "";

                }

                //if (print) {
                //    if (result.Codigo > 0 && result.CodMdr != "" && result.CodMdr != null)
                //        geraRelatorio(result.Codigo, result.CodMdr, 3000);
                //}

                Sistema.Pedido.Load(false);
            },
            error: function (result) {
                Swal.fire('Erro', result, 'error');
                return false;
            }

        });

        //$("#divAcaoPedido").removeClass("divDisabled");
        //pcTabs.SetActiveTabIndex(0);
    },

    DesabilitarPedido: function () {
        $("#txtCodGrupo").prop("disabled", true);
        $("#txtCodCliente").prop("disabled", true);
        $("#txtCodRepresentante").prop("disabled", true);
        $("#txtCodTransportadora").prop("disabled", true);
        $("#txtCodVeiculo").prop("disabled", true);
        $("#cbFrete").prop("disabled", true);
        $('#vlrFrete').prop("disabled", true);
        $("#txtDataEntrega").prop("disabled", true);
        $("#txtCodTransacao").prop("disabled", true);
        $("#txtCodFormaPgto").prop("disabled", true);
        $("#txtCodCondPgto").prop("disabled", true);
        $("#txtObsPedido").prop("disabled", true);
        $("#txtObsNf").prop("disabled", true);

        $('#btnCodGrupo').prop("disabled", true);
        $('#btnCodCliente').prop("disabled", true);
        $('#btnCodRep').prop("disabled", true);
        $('#btnCodTransp').prop("disabled", true);
        $('#btnCodTransacao').prop("disabled", true);
        $('#btnCodFormaPgto').prop("disabled", true);
        $('#btnCodCondPgto').prop("disabled", true);
        $('#btnCodVeiculo').prop("disabled", true);
        $('#btnAddItem').prop("disabled", true);

        $('#txtVlrArredondamento').prop("disabled", true);
        $('#txtVlrDesconto').prop("disabled", true);
    },

    HabilitarPedido: function () {

        //btnExcluirParc.SetEnabled(true);

        $("#txtCodGrupo").prop("disabled", false);
        $("#txtCodCliente").prop("disabled", false);
        $("#txtCodRepresentante").prop("disabled", false);
        $("#txtCodTransportadora").prop("disabled", false);
        $("#txtCodVeiculo").prop("disabled", false);
        $("#cbFrete").prop("disabled", false);
        $('#vlrFrete').prop("disabled", false);
        $("#txtDataEntrega").prop("disabled", false);
        $("#txtCodTransacao").prop("disabled", false);
        $("#txtCodFormaPgto").prop("disabled", false);
        $("#txtCodCondPgto").prop("disabled", false);
        $("#txtObsPedido").prop("disabled", false);
        $("#txtObsNf").prop("disabled", false);

        if ($('#vlrFrete').val() != null && $('#vlrFrete').val() != 0)
            $('#vlrFrete').prop("disabled", false);
        else
            $('#vlrFrete').prop("disabled", true);

        $('#btnCodGrupo').prop("disabled", false);
        $('#btnCodCliente').prop("disabled", false);
        $('#btnCodRep').prop("disabled", false);
        $('#btnCodTransp').prop("disabled", false);
        $('#btnCodTransacao').prop("disabled", false);
        $('#btnCodFormaPgto').prop("disabled", false);
        $('#btnCodCondPgto').prop("disabled", false);
        $('#btnCodVeiculo').prop("disabled", false);
        $('#btnAddItem').prop("disabled", false);

        $('#txtVlrArredondamento').prop("disabled", false);
        $('#txtVlrDesconto').prop("disabled", false);
    },

    ObterTodosAvalista: function () {
        var codigo = $('#txtCodAvalista').val();
        var nome = $('#txtNomeAvalista').val();
    },

    ObterTodosPedidos: function () {
        Sistema.Pedido.Load(true);

        var pedido = $('#txtPesquisaPedido').val();
        var cliente = $('#txtPesquisaPedidoCliente').val();

        if ((pedido != null && pedido != "") || (cliente != null && cliente != "")) {
            $.ajax({
                url: '/Pedido/ObterTodosPedidos/',
                data: {
                    'pesquisar_pedido_codigo': pedido,
                    'pesquisar_pedido': cliente,
                },
                type: "GET",
                success: function (response) {
                    if (response.Success == false) {
                        Sistema.Pedido.Load(false);
                        Swal.fire("Erro", response.Message, "error");
                    }
                    else {
                        Sistema.Pedido.Load(false);
                        Sistema.Pedido.GridPedido(response.Data);
                    }
                }
            });
        }
    },

    ObterTodosClientes: function () {
        Sistema.Pedido.Load(true);

        var pesquisa = $('#txtPesquisaCliente').val();

        $.ajax({
            url: '/Pedido/ObterTodosClientes',
            type: 'GET',
            dataType: 'json',
            data: {
                pesquisa: pesquisa
            },
            success: function (result) {
                if (result.success != undefined && result.success == false) {
                    Swal.fire('Atenção', result.message, 'error');
                    return;
                }

                Sistema.Pedido.GridCliente(result.Data);

                Sistema.Pedido.Load(false);
            }
        })
    },

    ObterTodosItemPedido: function (codSelecionado) {
        $.ajax({
            type: "GET",
            url: "/Pedido/ObterItensPedido",
            data: { codPedido: codSelecionado },
            async: false,
            success: function (response) {
                Sistema.Pedido.GridItemPedido(response.Data);
            }
        });
    },

    IncluirItem: function (validacao) {
        Sistema.Pedido.Load(true);

        var codPedido = $('#txtCodPedido').val();
        var updatePedido = false;

        if (codPedido != null || codPedido != "" || codPedido != undefined)
            updatePedido = true;

        var item = {};

        var codProduto = $('#txtCodProduto').val();
        var descProduto = $('#txtDescProd').val();
        var qtdPedido = $('#txtQtdeProduto').val();
        var precoBase = $('#txtPrecoBase').val();
        var codDer = $('#cbDerivacao').val();
        var codDep = $('#cbDeposito').val();
        var perDes = $('#txtPerDesc').val();
        var vlrDes = $('#txtVlrDesc').val();
        var perAcr = $('#txtPerAcre').val();
        var vlrAcr = $('#txtVlrAcre').val();

        var emissao = $('#txtDataPedido').val().split('-');
        var dataEntrega = $('#txtEntrega')[0].value.split('-');

        var dataEmissao = emissao[2] + '/' + emissao[1] + '/' + emissao[0];
        var entrega = dataEntrega[2] + '/' + dataEntrega[1] + '/' + dataEntrega[0];

        var vlrLiq = $('#txtVlrLiq').val();
        
        var codTpr = $('#txtTabelaPreco').val();
        var vlrUnit = $('#txtPrecoUnit').val();        

        if (codProduto == null || codProduto == "") {
            Swal.fire('Atenção', 'Favor informar um produto.', 'error');
            return false;
        }

        if (descProduto == null || descProduto == "") {
            Swal.fire('Atenção', 'Favor informar um produto.', 'error');
            return false;
        }

        if (qtdPedido <= 0) {
            Swal.fire('Atenção', 'Favor informar a quantidade para este item.', 'error');
            $('#txtQtdeProduto').focus();
            return false;
        }

        if (precoBase == null || precoBase == 0) {
            Swal.fire('Atenção', 'Favor verificar o produto, pois não identificamos o valor do mesmo.', 'error');
            return false;
        }

        //Validando permissão de desconto
        var autorizaDescontoUsuario = $('#autorizaDesconto').val();
        var perctDesc = SapiensJS.Util.converteMoedaFloat($('#txtPerDesc').val());
        var valorDesc = SapiensJS.Util.converteMoedaFloat($('#txtVlrDesc').val());
        var perctAcres = SapiensJS.Util.converteMoedaFloat($('#txtPerAcre').val());
        var valorAcres = SapiensJS.Util.converteMoedaFloat($('#txtVlrAcre').val());
        var totalLiq = 0;

        if (validacao == false && (autorizaDescontoUsuario == "" || autorizaDescontoUsuario == null || autorizaDescontoUsuario == "N") && (perctDesc > 0 || valorDesc > 0)) {
            Swal.close();
            $('#modal-aprovacao').modal('show');
            return false;
        } else {
            if (perctDesc > 0 || valorDesc > 0) {
                var data = new Date();
                var hora = "";
                var min = "";
                var seg = "";

                item.DataDesconto = entrega;

                if (data.getHours() < 10) {
                    hora = "0" + data.getHours().toLocaleString();
                } else {
                    hora = data.getHours().toLocaleString();
                }

                if (data.getMinutes() < 10) {
                    min = "0" + data.getMinutes().toLocaleString();
                } else {
                    min = data.getMinutes().toLocaleString();
                }

                if (data.getSeconds() < 10) {
                    seg = "0" + data.getSeconds().toLocaleString();
                } else {
                    seg = data.getSeconds().toLocaleString();
                }

                item.HoraDesconto = hora + ":" + min + ":" + seg;

                if (autorizaDescontoUsuario == 'N') {
                    item.UsuarioDesconto = $('#usuarioDesconto').val();
                    $('#usuarioDesconto').val(null);
                } else {
                    item.UsuarioDesconto = $('#sessao_usuario_logado').val();
                }
            }
        }

        item.CodigoProduto = codProduto;
        item.DescricaoProduto = descProduto;
        item.CodigoDerivacao = codDer;
        item.CodigoDep = codDep;
        item.PrecoBase = precoBase.replace(',', '.');
        item.QuantidadePedido = qtdPedido;
        item.ValorDescontoUsuario = vlrDes.replace('.', '').replace(',', '.');
        item.PercentualDescontoUsuario = perDes.replace(',', '.');
        item.PercentualAcrescimoUsuario = perAcr.replace(',', '.');
        item.ValorAcrescimoUsuario = vlrAcr.replace('.', '').replace(',', '.');
        item.ValorLiquido = vlrLiq.replace('.', '').replace(',', '.');
        item.DataEntrega = entrega;
        item.CodigoTRP = codTpr;
        item.PrecoUnitario = vlrUnit.replace(',', '.');

        $.ajax({
            url: '/Pedido/IncluirItem',
            data: {
                produto: JSON.stringify(item),
                dataEmissao: dataEmissao,
                dataIniJur: dataEmissao
            },
            type: 'GET',
            success: function (result) {
                if (result.Data != undefined && result.Data.Error != undefined && result.Data.Error == true) {
                    Swal.fire('Atenção', result.Data.Message, 'error')
                } else {
                    $('#modal-add-item').modal('hide');
                    Sistema.Pedido.GridItemPedido(result.Data);
                    //CalcularTotais();
                    Sistema.Pedido.RecalcularItensPedido(false, updatePedido);
                }

                Sistema.Pedido.Load(false);
            },
            error: function (err) {
                $('#modal-add-item').modal('hide');
                Sistema.Pedido.GridItemPedido(result.Data);
                //CalcularTotais();
                Sistema.Pedido.RecalcularItensPedido(false, updatePedido);
                Sistema.Pedido.Load(false);
            }
        });
    },

    EditarItem: function (validacao) {
        Sistema.Pedido.Load(true);

        var item = {};

        var codProduto = $('#txtCodProduto').val();
        var descProduto = $('#txtDescProd').val();
        var qtdPedido = $('#txtQtdeProduto').val();
        var precoBase = $('#txtPrecoBase').val();
        var codDer = $('#cbDerivacao').val();
        var codDep = $('#cbDeposito').val();
        var perDes = $('#txtPerDesc').val();
        var vlrDes = $('#txtVlrDesc').val();
        var perAcr = $('#txtPerAcre').val();
        var vlrAcr = $('#txtVlrAcre').val();

        var emissao = $('#txtDataPedido').val().split('-');

        var dataEmissao = emissao[2] + '/' + emissao[1] + '/' + emissao[0];

        var vlrLiq = $('#txtVlrLiq').val();
        var dataEntrega = $('#txtEntrega').val();
        var codTpr = $('#txtTabelaPreco').val();
        var vlrUnit = $('#txtPrecoUnit').val();
        var sequencia = $('#sequencia_item').val();

        var data = new Date(dataEntrega);
        var entrega = FormatarData(data);

        if (codProduto == null || codProduto == "") {
            Swal.fire('Atenção', 'Favor informar um produto.', 'error');
            return false;
        }

        if (descProduto == null || descProduto == "") {
            Swal.fire('Atenção', 'Favor informar um produto.', 'error');
            return false;
        }

        if (qtdPedido <= 0) {
            Swal.fire('Atenção', 'Favor informar a quantidade para este item.', 'error');
            $('#txtQtdeProduto').focus();
            return false;
        }

        if (precoBase == null || precoBase == 0) {
            Swal.fire('Atenção', 'Favor verificar o produto, pois não identificamos o valor do mesmo.', 'error');
            return false;
        }

        //Validando permissão de desconto
        var autorizaDescontoUsuario = $('#autorizaDesconto').val();
        var perctDesc = SapiensJS.Util.converteMoedaFloat($('#txtPerDesc').val());
        var valorDesc = SapiensJS.Util.converteMoedaFloat($('#txtVlrDesc').val());
        var perctAcres = SapiensJS.Util.converteMoedaFloat($('#txtPerAcre').val());
        var valorAcres = SapiensJS.Util.converteMoedaFloat($('#txtVlrAcre').val());
        var totalLiq = 0;

        if (validacao == false && (autorizaDescontoUsuario == "" || autorizaDescontoUsuario == null || autorizaDescontoUsuario == "N") && (perctDesc > 0 || valorDesc > 0)) {
            Swal.close();
            $('#modal-aprovacao-editar').modal('show');
            return false;
        } else {
            if (perctDesc > 0 || valorDesc > 0) {
                var data = new Date();
                var hora = "";
                var min = "";
                var seg = "";

                item.DataDesconto = entrega;

                if (data.getHours() < 10) {
                    hora = "0" + data.getHours().toLocaleString();
                } else {
                    hora = data.getHours().toLocaleString();
                }

                if (data.getMinutes() < 10) {
                    min = "0" + data.getMinutes().toLocaleString();
                } else {
                    min = data.getMinutes().toLocaleString();
                }

                if (data.getSeconds() < 10) {
                    seg = "0" + data.getSeconds().toLocaleString();
                } else {
                    seg = data.getSeconds().toLocaleString();
                }

                item.HoraDesconto = hora + ":" + min + ":" + seg;

                if (autorizaDescontoUsuario == 'N') {
                    item.UsuarioDesconto = $('#usuarioDesconto').val();
                    $('#usuarioDesconto').val(null);
                } else {
                    item.UsuarioDesconto = $('#sessao_usuario_logado').val();
                }
            }
        }

        item.CodigoProduto = codProduto;
        item.DescricaoProduto = descProduto;
        item.CodigoDerivacao = codDer;
        item.CodigoDep = codDep;
        item.PrecoBase = precoBase;
        item.QuantidadePedido = qtdPedido;
        item.ValorDescontoUsuario = vlrDes.replace('.', '').replace(',', '.');
        item.PercentualDescontoUsuario = perDes.replace(',', '.');
        item.PercentualAcrescimoUsuario = perAcr.replace(',', '.');
        item.ValorAcrescimoUsuario = vlrAcr.replace('.', '').replace(',', '.');
        item.ValorLiquido = vlrLiq.replace('.', '').replace(',', '.');
        item.DataEntrega = entrega;
        item.CodigoTRP = codTpr;
        item.PrecoUnitario = vlrUnit.replace(',', '.');
        item.Sequencia = sequencia;

        $.ajax({
            url: '/Pedido/EditarItem',
            data: {
                produto: JSON.stringify(item),
                dataEmissao: dataEmissao,
                dataIniJur: dataEmissao
            },
            type: 'GET',
            success: function (result) {
                if (result.Data != undefined && result.Data.Error != undefined && result.Data.Error == true) {
                    Swal.fire('Atenção', result.Data.Message, 'error')
                } else {
                    $('#modal-add-item').modal('hide');
                    Sistema.Pedido.GridItemPedido(result.Data);
                    Sistema.Pedido.RecalcularItensPedido();
                }

                Sistema.Pedido.Load(false);
            }
        });
    },

    DeleteItem: function (codigoProduto, valorFrete) {
        $.ajax({
            url: '/Pedido/RemoverItem',
            type: 'GET',
            data: { CodigoProduto: codigoProduto, vlFrete: valorFrete },
            success: function (result) {
                if (result.Data != undefined && result.Data.Error != undefined && result.Data.Error == true) {
                    Swal.fire('Atenção', result.Data.Message, 'error')
                } else {
                    Sistema.Pedido.GridItemPedido(result.Data);
                    Sistema.Pedido.RecalcularItensPedido(false);
                }
            }
        })
    },

    FecharPedido: function () {
        Sistema.Pedido.Load(true);

        var situacao = $('#txtStatus').val() != null ? $('#txtStatus').val() : "";
        var codCliente = $('#txtCodCliente').val() != null ? $('#txtCodCliente').val() : 0;
        var nomeCliente = $('#txtDescCliente').val() != null ? $('#txtDescCliente').val() : "";
        var codPedido = $('#txtCodPedido').val() != null ? $('#txtCodPedido').val() : 0;
        var grupoEmpresa = $('#txtCodGrupo').val() != null ? $('#txtCodGrupo').val() : 0;
        var codTransacao = $('#txtCodTransacao').val() != null ? $('#txtCodTransacao').val() : "";
        var formaPgto = $('#txtCodFormaPgto').val() != null ? $('#txtCodFormaPgto').val() : 0;
        var dscFrmaPgto = $('#txtDescFormaPgto').val() != null ? $('#txtDescFormaPgto').val() : "";
        var condicaoPgto = $('#txtCodCondPgto').val() != null ? $('#txtCodCondPgto').val() : "";
        //var codMdr = transacao_produto_codMdr.value;
        var tipoCondicaoPgto = $('#condicao_pagamento_Especial').val();
        var codRepresentante = $('#txtCodRepresentante').val() != null ? $('#txtCodRepresentante').val() : 0;
        var nomeRepresentante = $('#txtDescRepresentante').val() != null ? $('#txtDescRepresentante').val() : "";
        var transportadora = $('#txtCodTransportadora').val() != null ? $('#txtCodTransportadora').val() : 0;
        var placaVeiculo = $('#txtCodVeiculo').val() != null ? $('#txtCodVeiculo').val() : "";
        var cifFob = $('#cbFrete').val() != null ? $('#cbFrete').val() : "";
        var valorFrete = $('#vlrFrete').val() != null ? SapiensJS.Util.converteMoedaFloat($('#vlrFrete').val()) : 0;
        var obsPedido = $('#txtObsPedido').val() != null ? $('#txtObsPedido').val() : "";
        var obsNF = $('#txtObsNf').val() != null ? $('#txtObsNf').val() : "";
        var dataEmissao = $('#txtDataPedido').text() != null ? $('#txtDataPedido').text() : "";
        var valorLiquido = $('#lblVlrLiquido').val() != null ? $('#lblVlrLiquido').val() : 0;
        var dataIniJur = dataEmissao;//$('#condicao_pagamento_ini_jur').val();
        var taxaJuros = $('#condicao_pagamento_taxa_juros').val().replace(',', '.');
        var rowCountItens = $('#tblItens tr').length - 1;

        if (rowCountItens <= 0) {
            Swal.fire("Validação", "Favor informar pelo menos um item no pedido.", "error");
            $('#btnFecharPedido').prop("disabled", false);
            return false;
        }

        if (codCliente == "" || codCliente == null) {
            Swal.fire("Validação", "Código do cliente é de preenchimento obrigatório.", "error");
            $('#btnFecharPedido').prop("disabled", false);
            return false;
        }

        if (codTransacao == "" || codTransacao == null) {
            Swal.fire("Validação", "Transação é de preenchimento obrigatório.", "error");
            $('#btnFecharPedido').prop("disabled", false);
            return false;
        }

        if (formaPgto == "" || formaPgto == null) {
            Swal.fire("Validação", "Forma de pagamento é de preenchimento obrigatório.", "error");
            $('#btnFecharPedido').prop("disabled", false);
            return false;
        }

        if (condicaoPgto == "" || condicaoPgto == null) {
            Swal.fire("Validação", "Condição de Pagamento é de preenchimento obrigatório.", "error");
            $('#btnFecharPedido').prop("disabled", false);
            return false;
        }

        if (tipoCondicaoPgto == "S" && $('#lblparcvalida').val() != "Sim") {
            Swal.fire("Validação", "Parcelas não válidas, favor verificar!", "error");
            $('#btnFecharPedido').prop("disabled", false);

            //var tab = pcTabs.GetTabByName('Parcelas');

            //if (tab != null) {
            //    tab.SetVisible(true);;
            //    pcTabs.SetActiveTab(tab);
            //}

            return false;
        }

        if (codRepresentante == "" || codRepresentante == null) {
            Swal.fire("Validação", "Representante é de preenchimento obrigatório.", "error");
            $('#btnFecharPedido').prop("disabled", false);
            return false;
        }

        if (cifFob == "" || cifFob == null) {
            Swal.fire("Validação", "Tipo de frete é de preenchimento obrigatório.", "error");
            $('#btnFecharPedido').prop("disabled", false);
            return false;
        }

        var pedido = {};
        pedido.Codigo = codPedido != null ? codPedido : 0;
        pedido.CodGrupoEmpresa = grupoEmpresa != null ? grupoEmpresa : 0;
        pedido.CodCliente = codCliente;
        pedido.NomeCliente = nomeCliente;
        pedido.CodTransacao = codTransacao;
        pedido.CodFormaPagamento = formaPgto;
        pedido.DescFormaPagamento = dscFrmaPgto;
        pedido.CodCondicaoPagamento = condicaoPgto;
        pedido.CodRepresentante = codRepresentante;
        pedido.NomeRepresentante = nomeRepresentante;
        pedido.CodigoTransportadora = transportadora != null ? transportadora : 0;
        pedido.NomePlaca = placaVeiculo != null ? placaVeiculo : 0;
        pedido.CIFFOB = cifFob;
        pedido.ValorFrete = valorFrete;
        pedido.ObsPedido = obsPedido;
        pedido.ObsEntrega = obsNF;

        var emissao = $('#txtDataPedido').val().split('-');

        var dataEmissao = emissao[0] + '/' + emissao[1] + '/' + emissao[2];
        pedido.DataEmissao = dataEmissao;

        pedido.ValorLiquido = valorLiquido != null ? SapiensJS.Util.converteMoedaFloat(valorLiquido) : 0;
        pedido.DataIniJuros = dataEmissao;
        pedido.TaxaJuros = taxaJuros;

        if (codPedido == 0) {
            $.ajax({
                url: '/Pedido/IncluirPedido',
                json: true,
                contentType: 'application/json',
                dataType: 'json',
                data: { pedidoPost: JSON.stringify(pedido), isJson: true, parcelasValidas: $('#lblparcvalida').val() },
                success: function (result) {
                    if (result.Message != '') {
                        Swal.fire("Alerta", result.Message, "error");
                        return false;
                    }

                    if (result.Data[0].retorno == "OK") {
                        Sistema.Pedido.SelecionarPedido(result.Data[0].numPed);
                        //SistemaWeb.Pedido.SelecionarPedido(result[0].numPed, "focus", true);

                        if (result.Data[0].numPed > 0)
                            geraRelatorio(result.Data[0].numPed, "RVPE102.GER", 3000);

                        var msg = "Número do Pedido Gerado: " + result.Data[0].numPed + "<br />Situação do Pedido: " + result.Data[0].situacaoPedido + "<br/>";

                        if (result.Data[0].pedBlo == "S")
                            msg += "<b><font color='red'>" + result.Data[0].msgRetorno + "</font><b/>";

                        Swal.fire("Pedido Fechado", msg, "success");
                    } else {
                        if (result.Data[0].numPed != null && result.Data[0].numPed != "" && result.Data[0].numPed != "0" && result.Data[0].numPed != " ")
                            console.log('Selecionar Pedido');
                        //SistemaWeb.Pedido.SelecionarPedido(result[0].numPed, "focus", false);

                        var mensagem = result.Data[0].msgRetorno;

                        if (result.Data[0].retornoParcela != "") {
                            mensagem = mensagem + "<br />Retorno Parcela(s): " + result.Data[0].retornoParcela;
                            $('#btnFecharPedido').prop("disabled", false);
                        }

                        if (result.Data[0].retornoProduto != "") {
                            mensagem = mensagem + "<br />Retorno Itens do Pedido: " + result.Data[0].retornoProduto;
                            $('#btnFecharPedido').prop("disabled", false);
                        }

                        if (result.Data[0].retornoParcela == "" && result.Data[0].retornoProduto == "") {
                            if (result.Data[0].numPed != null && result.Data[0].numPed != "" && result.Data[0].numPed != "0" && result.Data[0].numPed != " ")
                                console.log('Selecionar Pedido');
                            //SistemaWeb.Pedido.SelecionarPedido(result[0].numPed, "focus", false);
                        }

                        Swal.fire("Alerta", mensagem, "error");

                        $('#btnFecharPedido').prop("disabled", false);
                        $('#btnFecharPedido').prop("visible", true);
                        return false;
                    }

                    //Sistema.Pedido.Load(false);
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire("Alerta", error, "error");
                }
            });
        } else {
            $.ajax({
                url: '/Pedido/EditarPedido',
                data: {
                    pedidoPost: JSON.stringify(pedido)
                },
                json: true,
                modalID: 'lpPedido',
                assync: true,
                success: function (result) {
                    //if (result.Data[0].mensagemRetorno != null && result.Data[0].mensagemRetorno != "") {
                    //    Swal.fire("Atenção!", result.Data[0].mensagemRetorno, "error");
                    //    return false;
                    //}

                    if (!result.Success) {
                        Swal.fire("Alerta", result.Message, "error");
                    }

                    if (result.Data[0].retorno == "OK") {

                        Sistema.Pedido.SelecionarPedido(result.Data[0].numPed);

                        var msg = "  " + result.Data[0].retorno + "<br />Número do pedido: " + result.Data[0].numPed + "<br />";
                        if (result.Data[0].pedBlo == "S")
                            msg += "<b><font color='red'>" + result.Data[0].msgRetorno + "</font><b/>";

                        Swal.fire("Sucesso", msg, "success");

                        return false;
                    } else {

                        var mensagem = result.Data[0].msgRetorno;

                        if (result.Data[0].retornoParcela != "") {
                            mensagem = mensagem + "<br />Retorno Parcela(s): " + result.Data[0].retornoParcela;
                        }

                        if (result.Data[0].retornoProduto != "") {
                            mensagem = mensagem + "<br />Retorno Itens do Pedido: " + result.Data[0].retornoProduto;
                        }

                        Swal.fire("Alerta", mensagem, "error");
                        $('#btnFecharPedido').prop("disabled", false);

                        return false;
                    }
                },
                error: function (error) {
                    console.log(error);
                    Swal.fire("Alerta", error, "error");
                }
            });
        }
    },

    EditarPedido: function () {
        Sistema.Pedido.Load(true);

        var codPedido = $('#txtCodPedido').val();
        //var pedido_filial = 3;
        //var pedido_empresa = 1;
        var codCondPgto = $('#txtCodCondPgto').val();
        var codFormaPgto = $('#txtCodFormaPgto').val();

        $.ajax({
            url: '/Pedido/ObterCondicaoPagamento',
            data: { codFiltro: codCondPgto, codFormaPagamento: codFormaPgto, verificaPodSel: false },
            json: true,
            success: function (result) {
                if (result.retornoCondPgto != "" && result.retornoCondPgto != undefined) {
                    Swal.fire({
                        icon: "question",
                        title: "Atenção!",
                        text: "No momento a condição de pagamento " + codCondPgto + " não está disponível. Deseja habilitar o pedido mesmo assim?",
                        allowOutsideClick: false,
                        showCancelButton: true,
                        cancelButtonText: 'Não',
                        confirmButtonText: 'Sim'
                    }).then((isConfirm) => {
                        if (isConfirm.value) {
                            $.ajax({
                                url: '/Pedido/HabilitarPedido',
                                data: { codPedido: codPedido },
                                json: true,
                                modalID: 'lpPedido',
                                success: function (result) {
                                    if (result[0].retorno == "OK") {
                                        //var mensagem = result[0].mensagemRetorno;
                                        //SistemaWeb.Pedido.SelecionarPedido(codPedido, "focus");
                                        $('#txtStatus').val(result.Data[0].situacaoPedido);
                                        Sistema.Pedido.HabilitarPedido();

                                        $('#divFecharPedido').show();
                                        $('#divImprimirPedido').hide();
                                        $('#divCancelarPedido').hide();
                                        $('#divEditarPedido').hide();

                                        //condicao_pagamento_CPR.value = result[0].CondicaoPagamentoTipoCPR;
                                        $('#condicao_pagamento_Especial').val(result.Data[0].CondicaoPagamentoEspecial);
                                        $('#condicao_pagamento_total_parcelas').val(result.Data[0].QtdParcelas);
                                        $('#condicao_pagamento_manutencao_parcela').val(result.Data[0].ManutencaoParcelas);

                                        //btnCancelarPedido.SetVisible(false);
                                        //btnEditarPedido.SetVisible(false);
                                        //btnFecharPedido.SetVisible(true);
                                        Sistema.Pedido.SelecionarCondPgto(result.Data[0].CodCondicaoPagamento, true);
                                        Sistema.Pedido.RecalcularItensPedido(false);
                                        //gvEditing.Refresh();

                                        //if (result[0].CondicaoPagamentoTipoCPR != "N") {
                                        //    SistemaWeb.TabelaCPR.ObterTodosTabelaCPR(result[0].CodCondicaoPagamento, result[0].TabelaCPR, result[0].CondicaoPagamentoTipoCPR);
                                        //}
                                        //else {
                                        //    AtivarTabParcela(true);
                                        //}

                                        //if (result[0].TabelaCPR != "" && result[0].TabelaCPR != " " && result[0].TabelaCPR != null && (result[0].CondicaoPagamentoTipoCPR == "I" || result[0].CondicaoPagamentoTipoCPR == "T"))
                                        //    cbTabelaCpr.SetValue(result[0].TabelaCPR);
                                        //else
                                        //    cbTabelaCpr.SetText("Selecione Tabela CPR");

                                        //if (result[0].CondicaoPagamentoEspecial == "S") {
                                        //    AtivarTabParcela(true);
                                        //} else {
                                        //    AtivarTabParcela(false);
                                        //}

                                        //gridViewAvalistas.Refresh();
                                        //if (gridViewParcelas != null)
                                        //    gridViewParcelas.Refresh();
                                        //gridViewProcuradores.Refresh();

                                        //return false;

                                    } else {
                                        var mensagem = (result.Data[0].retorno != null && result.Data[0].retorno != "") ? result.Data[0].retorno : result.Data[0].msgRetorno;
                                        Swal.fire("Alerta", mensagem, "error");
                                        $('#divFecharPedido').show();
                                        //return false;
                                    }

                                    Sistema.Pedido.Load(false);
                                }
                            });
                        }
                    });
                } else {
                    Swal.fire({
                        icon: "question",
                        title: "Atenção!",
                        text: "Deseja realmente habilitar este pedido para sua edição?",
                        allowOutsideClick: false,
                        showCancelButton: true,
                        cancelButtonText: 'Não',
                        confirmButtonText: 'Sim'
                    }).then((isConfirm) => {
                        if (isConfirm.value) {
                            Sistema.Pedido.Load(true);

                            $.ajax({
                                url: '/Pedido/HabilitarPedido',
                                data: { codPedido: codPedido },
                                json: true,
                                modalID: 'lpPedido',
                                success: function (result) {
                                    if (!result.Success) {
                                        Swal.fire('Atenção', result.Message, 'error');
                                        return false;
                                    }

                                    if (result.Data[0].retorno == "OK") {
                                        //var mensagem = result[0].mensagemRetorno;
                                        //SistemaWeb.Pedido.SelecionarPedido(codPedido, "focus");
                                        $('#txtStatus').val(result.Data[0].situacaoPedido);
                                        Sistema.Pedido.HabilitarPedido();

                                        $('#divFecharPedido').show();
                                        $('#divImprimirPedido').hide();
                                        $('#divCancelarPedido').hide();
                                        $('#divEditarPedido').hide();

                                        //condicao_pagamento_CPR.value = result[0].CondicaoPagamentoTipoCPR;
                                        $('#condicao_pagamento_Especial').val(result.Data[0].CondicaoPagamentoEspecial);
                                        $('#condicao_pagamento_total_parcelas').val(result.Data[0].QtdParcelas);
                                        $('#condicao_pagamento_manutencao_parcela').val(result.Data[0].ManutencaoParcelas);
                                        $('#condicao_pagamento_taxa_juros').val(result.Data[0].TaxaJuros);

                                        //btnCancelarPedido.SetVisible(false);
                                        //btnEditarPedido.SetVisible(false);
                                        //btnFecharPedido.SetVisible(true);
                                        Sistema.Pedido.SelecionarCondPgto(result.Data[0].CodCondicaoPagamento, true);
                                        Sistema.Pedido.ObterTodosParcelas(codPedido);
                                        Sistema.Pedido.ObterTodosItemPedido(codPedido);
                                        //Sistema.Pedido.RecalcularItensPedido(false);
                                        //gvEditing.Refresh();

                                        //if (result[0].CondicaoPagamentoTipoCPR != "N") {
                                        //    SistemaWeb.TabelaCPR.ObterTodosTabelaCPR(result[0].CodCondicaoPagamento, result[0].TabelaCPR, result[0].CondicaoPagamentoTipoCPR);
                                        //}
                                        //else {
                                        //    AtivarTabParcela(true);
                                        //}

                                        //if (result[0].TabelaCPR != "" && result[0].TabelaCPR != " " && result[0].TabelaCPR != null && (result[0].CondicaoPagamentoTipoCPR == "I" || result[0].CondicaoPagamentoTipoCPR == "T")) {
                                        //    cbTabelaCpr.SetValue(result[0].TabelaCPR);
                                        //    SelecionaTabelaCPRAtual();
                                        //}
                                        //else
                                        //    cbTabelaCpr.SetText("Selecione Tabela CPR");

                                        //if (result[0].CondicaoPagamentoEspecial == "S") {
                                        //    AtivarTabParcela(true);
                                        //} else {
                                        //    AtivarTabParcela(false);
                                        //}

                                        //gridViewAvalistas.Refresh();
                                        //if (gridViewParcelas != null)
                                        //    gridViewParcelas.Refresh();
                                        //gridViewProcuradores.Refresh();

                                        //return false;
                                        Sistema.Pedido.Load(false);

                                    } else {
                                        var mensagem = (result.Data[0].retorno != null && result.Data[0].retorno != "") ? result.Data[0].retorno : result.Data[0].msgRetorno;
                                        Swal.fire("Alerta", mensagem, "error");
                                        $('#divFecharPedido').show();
                                        //return false;
                                    }
                                }
                            });
                        }
                    });
                }
            }
        });
    },

    CancelarPedido: function () {
        var codPedido = $('#txtCodPedido').val();

        Swal.fire({
            icon: "question",
            title: "Atenção!",
            text: 'Deseja realmente cancelar este pedido?',
            allowOutsideClick: false,
            showCancelButton: true,
            cancelButtonText: 'Não',
            confirmButtonText: 'Sim'
        }).then((result) => {
            if (result.value) {
                Sistema.Pedido.Load(true);

                $.ajax({
                    url: '/Pedido/CancelarPedido',
                    data: { codPedido: codPedido },
                    success: function (result) {
                        if (result.Data[0].retorno == 'OK') {
                            Sistema.Pedido.Load(false);

                            var mensagem = result.Data[0].mensagemRetorno;
                            Swal.fire('Sucesso', mensagem, 'success');

                            Sistema.Pedido.SelecionarPedido(codPedido);

                            $('#divFecharPedido').hide();
                            $('#divImprimirPedido').hide();
                            $('#divCancelarPedido').hide();
                            $('#divEditarPedido').hide();

                            return false;
                        } else {
                            Sistema.Pedido.Load(false);

                            var mensagem = result.Data[0].retorno;
                            Swal.fire("Alerta", mensagem, "error");

                            return false;
                        }
                    }
                });
            }
        });
    },

    ImprimirPedido: function () {
        var codPedido = $('#txtCodPedido').val();

        Sistema.Pedido.Load(true);

        geraRelatorio(codPedido, "RVPE102.GER", 3000);

        Sistema.Pedido.Load(false);
    },

    RemoverDesconto: function () {
        $('#txtVlrDesconto').val("0,00");
        Sistema.Pedido.RecalcularItensPedido(true, true);
    },

    RemoverAcrescimo: function () {
        $('#txtVlrArredondamento').val("0,00");
        Sistema.Pedido.RecalcularItensPedido(true, true);
    },

    RecalcularItensPedido: function (validaDesconto, updatePedido) {
        var condicaoEspecial = $('#condicao_pagamento_Especial').val();
        var condicaoPgtoTipoCalculo = $('#condicao_pagamento_tipo_calculo').val();
        var taxaJuros = $('#condicao_pagamento_taxa_juros').val();
        var totalParcelas = $('#condicao_pagamento_total_parcelas').val();
        var arredondamento = SapiensJS.Util.converteMoedaFloat($('#txtVlrArredondamento').val());
        var desconto = SapiensJS.Util.converteMoedaFloat($('#txtVlrDesconto').val());

        if (validaDesconto && desconto != null && desconto != "") {
            //Validando permissão de desconto
            var autorizaDescontoUsuario = $('#autorizaDesconto').val();
            var perctDesc = SapiensJS.Util.converteMoedaFloat($('#txtPerDesc').val());
            var valorDesc = SapiensJS.Util.converteMoedaFloat($('#txtVlrDesc').val());
            var totalLiq = 0;

            if ((autorizaDescontoUsuario == "" || autorizaDescontoUsuario == null || autorizaDescontoUsuario == "N") && (perctDesc > 0 || valorDesc > 0 || desconto > 0)) {
                Swal.close();
                $('#modal-aprovacao').modal('show');
                return false;
            }
        } else {
            $('#modal-aprovacao-geral').modal('hide');
            $('#modal-aprovacao').modal('hide');
        }

        var tipoCalculo = "";

        if (condicaoEspecial == "S" || condicaoEspecial == "Sim")
            tipoCalculo = "Juros";
        else
            tipoCalculo = "Base";

        var emissao = $('#txtDataPedido').val().split('-');

        var dataEmissao = emissao[0] + '/' + emissao[1] + '/' + emissao[2];

        if (condicaoPgtoTipoCalculo == "A")
            dataEmissao = $('#grupo01_dt_hoje').val();

        var agComercial = "";//txtAgComercial.GetValue();
        var agEstoque = "";//txtAgEstoque.GetValue();
        var condPgto = $('#txtCodCondPgto').val();
        var codCli = $('#txtCodCliente').val();
        var transProd = $('#txtCodTransacao').val();
        var dataIniJur = $('#condicao_pagamento_ini_jur').val() != "" ? $('#condicao_pagamento_ini_jur').val() : dataEmissao;
        var codDerivacao = null;
        var codRep = $('#txtCodRepresentante').val();
        var vlrLiquido = $('#text_valor_total_liquido').val().replace(',', '.');

        $.ajax({
            url: '/Pedido/RecalculaItens',
            type: "GET",
            async: false,
            data: {
                'tipoCalculo': tipoCalculo,
                'tipo_especial': condicaoEspecial,
                'taxa_juros': taxaJuros,
                'total_parcelas': totalParcelas,
                'agComercial': agComercial,
                'agEstoque': agEstoque,
                'dataEmissao': dataEmissao,
                'condPgto': condPgto,
                'codCli': codCli,
                'transProd': transProd,
                'codDerivacao': codDerivacao,
                'dataIniJur': dataIniJur,
                'codRepresentante': codRep,
                'vlrDescontoTotal': desconto,
                'vlrArredondamento': arredondamento,
                'vlrLiquido': vlrLiquido
            },
            success: function (result) {
                if (result.Data != undefined && result.Data.Error != undefined && result.Data.Error == true) {
                    Swal.fire('Atenção', result.Data.Message, 'error');
                    return;
                } else {
                    Sistema.Pedido.GridItemPedido(result.Data);
                    CalcularTotais(updatePedido);
                    Sistema.Pedido.Load(false);
                }
            }
        });
    },

    ObterProdutos: function () {
        var codProduto = "";
        var descProduto = $('#txtBuscaProduto').val();

        var emissao = $('#txtDataPedido').val().split('-');

        var dataEmissao = emissao[2] + '/' + emissao[1] + '/' + emissao[0];

        var condPgto = $('#txtCodCondPgto').val();
        var codCli = $('#txtCodCliente').val();
        var transacao = $('#txtCodTransacao').val();

        if (codCli == null || codCli == "") {
            Swal.fire('Atenção', 'Selecione um cliente', 'warning');
            return false;
        }

        $.ajax({
            url: "/Pedido/ObterProdutoPesquisar",
            type: "GET",
            data: {
                codProduto: codProduto,
                descProduto: descProduto,
                dataEmissao: dataEmissao,
                codCondPgto: condPgto,
                codCli: codCli,
                transProd: transacao
            },
            success: function (data) {
                $('#tblBuscaProdutos').dataTable({
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
                        {
                            render: function (data, type, full, meta) {
                                //return "<a class='btn btn-info' href='" + Sistema.Pedido.SelecionarCliente(full.Codigo) + "'>Editar</a>";
                                return "<button type='button' class='btn btn-default btn-xs' onclick='Sistema.Pedido.SelecionarProduto(\"" + full.Codpro + "\", \"" + full.Codder + "\", \"" + full.Despro + "\")'><i class='fa fa-check'></i></button>";
                            }
                        },
                        { data: "Codpro", title: "Cód. Produto" },
                        { data: "Codder", title: "Deriv." },
                        { data: "Despro", title: "Descrição" },
                        { data: "Unimed", title: "UN" },
                        { data: "Salest", title: "Sal. Est." },
                        { data: "Codtpr", title: "Tab. Preço" },
                        { data: "Prebas", title: "Preço Base" },
                    ],
                    columnDefs: [{
                        targets: [5, 7],
                        render: function (data, type, full) {
                            var formmatedvalue = data.toFixed(2)
                            return formmatedvalue;
                        }
                    }],
                    data: data.Data
                });
            }
        });
    },

    CalcularDescontosQuantidade: function () {
        var precoBase = $('#txtPrecoUnit').val();
        var item_produto_codigo = $('#txtCodProduto').val();
        var item_produto_label = $('#txtDescProd').val();

        if (item_produto_codigo != "" && item_produto_label != "") {
            if (precoBase == "") {
                Swal.fire('Informação', 'Preço base do produto não localizado.', 'error');
                $("#item_produto_valorLiquido").val('');
                return false;
            }

            if ($('#txtQtdeProduto').val() != null && $('#txtQtdeProduto').val() != "") {

                if ($('#txtVlrDesc').val() != 0)
                    calcularDesconto('valor', $('#txtVlrDesc').val());

                if ($('#txtVlrAcre').val() != 0)
                    calcularAcrescimo('valor', $('#txtVlrAcre').val());

            }
        }

        calcularValorLiquido();
    },

    ObterTodosParcelas: function (codPedido) {
        $.ajax({
            type: "GET",
            url: "/Pedido/ObterTodasParcelas/",
            data: { codPedido: codPedido },
            success: function (response) {
                if (response.Success == false)
                    Swal.fire("Erro", response.Message, "error");
                else {
                    Sistema.Pedido.GridParcelas(response.Data);

                    var sumPercentual = 0.00;

                    $('#tblParcelas > tbody > tr').each(function () {
                        sumPercentual += SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(3)').html());
                    });

                    //$("#containerParcela").html(response);

                    var percentual = sumPercentual != null ? sumPercentual.toFixed(2) : 0.00;
                    percentRest = 100 - percentual;

                    $('#txtPercentRestante').val(percentRest.toFixed(2));
                    if (percentual != 100)
                        $('#lblparcvalida').val('Não');
                    else
                        $('#lblparcvalida').val('Sim');

                    //$("#containerParcela").html(response);
                    //gridViewParcelas.Refresh();
                }
            }
        });
    },

    CalcularParcela: function () {
        if ($('#txtCodCondPgto').val() != "" && $('#txtCodCondPgto').val() != null) {
            var tipoCondicaoPagamento = $('#condicao_pagamento_Especial').val();
            var emissao = $('#txtDataPedido').val().split('-');

            var dataVencimento = emissao[2] + '/' + emissao[1] + '/' + emissao[0];

            var valorLiquido = $('#lblVlrLiquido').val();
            var percentRest = 0.00;
            var sumPercentual = 0.00;

            if (tipoCondicaoPagamento == 'S') {
                $.ajax({
                    url: "/Pedido/CalcularParcela/",
                    data: { valorLiquido: valorLiquido, dataVencimento: dataVencimento },
                    success: function (response) {
                        if (response.Success != false) {
                            Sistema.Pedido.GridParcelas(response.Data);

                            $('#btnAddParcela').prop("disabled", false);

                            $('#tblParcelas > tbody > tr').each(function () {
                                sumPercentual += SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(3)').html());
                            });

                            //$("#containerParcela").html(response);

                            var percentual = sumPercentual != null ? sumPercentual.toFixed(2) : 0.00;
                            percentRest = 100 - percentual;

                            $('#txtPercentRestante').val(percentRest.toFixed(2));

                            if (percentual != 100)
                                $('#lblparcvalida').val('Não');
                            else {
                                if (dataVencimento == response.Data[0].DataVencimento) {
                                    $('#lblparcvalida').val('Não');
                                } else {
                                    $('#lblparcvalida').val('Sim');
                                }
                                //$('#lblparcvalida').val('Sim');
                                //$('#condicao_pagamento_total_parcelas').val(gridViewParcelas.cpRowCount);     TIRAR COMENTÁRIO QUANDO ESTIVER PRONTO
                                Sistema.Pedido.RecalcularItensPedido(false);
                            }

                            Sistema.Pedido.RecalcularItensPedido(false);
                        }

                    }
                });
            } else {
                $('#btnAddParcela').prop("disabled", true);
                $('#lblparcvalida').val('Sim');
                $('#txtPercentRestante').val(100.00);
            }
        }
        else {
            Swal.fire("Mensagem do Sistema", "Não foi possível calcular a parcela. <br>Selecione uma condição de pagamento.", "error");
        }
    },

    InsereParcela: function () {
        //lpPedido.Show();
        //addParcela, updateParcela, editParcela = true;
        var sumPercentual = 0.00;
        var rowCount = $('#tblParcelas tr').length - 1;

        if (rowCount > 0) {
            $('#tblParcelas > tbody > tr').each(function () {
                //console.log($(this).find('td:eq(1)').html()); //sequencia
                //console.log($(this).find('td:eq(2)').html()); //vencimento
                //console.log($(this).find('td:eq(3)').html()); //percentual
                //console.log($(this).find('td:eq(4)').html()); //valor

                sumPercentual += SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(3)').html());
            });
        }

        //if (rowCount <= 0) {
        //    Swal.fire('Erro', 'Para realizar o cálculo das parcelas é necessário informar primeiro os itens do pedido.', 'error');
        //    //lpPedido.Hide();
        //    return false;
        //}

        ////if (addParcela) {
        if (sumPercentual == 100) {
            Swal.fire('Erro', 'Limite de 100% já foi atingido.', 'error');
            //lpPedido.Hide();
            return false;
        }
        ////}

        ////if (editParcela || addParcela || updateParcela) {
        //var totalPercentual = sumPercentual.toFixed(2);
        //var Percentual = $('#txtPercentual').val();
        //var somaPercentual = (totalPercentual * 1) + (Percentual * 1);

        //if (somaPercentual > 100) {
        //    Swal.fire('Erro', 'Não é permitido inserir uma parcela maior que o valor total do pedido.', 'error');
        //    lpPedido.Hide();
        //    return false;
        //}
        //}

        //SequenciaParcela.SetText("1");

        //gridViewParcelas.UpdateEdit();
        Sistema.Pedido.Load(true);

        var permiteManutencao = $('#condicao_pagamento_manutencao_parcela').val();

        var parcela = {};
        parcela.Percentual = parseFloat($('#txtPercentual').val().replace(',', '.')).toFixed(3);
        parcela.ValorParcela = parseFloat($('#txtValorParcela').val().replace(',', '.')).toFixed(3);

        var dataVcto = $('#DataVencimento').val().split('-');

        parcela.DataVencimento = dataVcto[2] + '/' + dataVcto[1] + '/' + dataVcto[0];

        var emissao = $('#txtDataPedido').val().split('-');

        var dataEmissao = emissao[2] + '/' + emissao[1] + '/' + emissao[0];

        if (parcela.Percentual > 100) {
            Swal.fire('Erro', 'Não é permitido inserir uma parcela maior que o valor total do pedido.', 'error');
            lpPedido.Hide();
            return false;
        }

        $.ajax({
            url: '/Pedido/IncluirParcela',
            data: {
                parcela: JSON.stringify(parcela),
                EmissaoPedido: dataEmissao,
                PermiteManutencao: permiteManutencao
            },
            type: 'GET',
            async: false,
            success: function (result) {
                if (result.Data.Success != undefined && result.Data.Success == false) {
                    Swal.fire('Atenção', result.Data.Message, 'error');
                    Sistema.Pedido.Load(false);
                    return;
                } else {
                    $('#modal-add-parcela').modal('hide');
                    Sistema.Pedido.GridParcelas(result.Data);

                    var sumPercentual = 0.00;
                    var percentRest = 0.00;
                    var rowCount = $('#tblParcelas tr').length - 1;

                    if (rowCount > 0) {
                        $('#tblParcelas > tbody > tr').each(function () {
                            //console.log($(this).find('td:eq(1)').html()); //sequencia
                            //console.log($(this).find('td:eq(2)').html()); //vencimento
                            //console.log($(this).find('td:eq(3)').html()); //percentual
                            //console.log($(this).find('td:eq(4)').html()); //valor

                            sumPercentual += SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(3)').html());
                        });
                    }

                    percentRest = 100 - sumPercentual;
                    $('#txtPercentRestante').val(percentRest.toFixed(2));

                    if (sumPercentual == 100) {
                        $('#lblparcvalida').val('Sim');
                        Sistema.Pedido.RecalcularItensPedido(false);
                        AjustarParcela();
                    } else {
                        $('#lblparcvalida').val('Não');
                    }

                    Sistema.Pedido.Load(false);
                }
            }
        });
    },

    EditarParcela: function (codigo, sequencia, vencimento, percent, valor) {
        $('#modal-add-parcela').modal('show');

        var vcto = vencimento.split("/");

        var today = vcto[2] + "-" + vcto[1] + "-" + vcto[0];

        $('#codigo_parcela').val(codigo);
        $('#sequencia_parcela').val(sequencia);
        $('#DataVencimento').val(today);
        $('#txtPercentual').val(percent);
        $('#txtValorParcela').val(valor);
        $('#divBtnAddParcela').hide();
        $('#divBtnCancelarParcela').show();
        $('#divBtnEditarParcela').show();
    },

    AlterarParcela: function () {
        Sistema.Pedido.Load(true);

        var emissao = $('#txtDataPedido').val().split('-');

        var dataEmissao = emissao[2] + '/' + emissao[1] + '/' + emissao[0];
        var permiteManutencao = $('#condicao_pagamento_manutencao_parcela').val();

        var parcela = {};
        parcela.Codigo = $('#codigo_parcela').val();
        parcela.SequenciaParcela = $('#sequencia_parcela').val();
        parcela.Percentual = parseFloat($('#txtPercentual').val().replace(',', '.')).toFixed(3);
        parcela.ValorParcela = parseFloat($('#txtValorParcela').val().replace(',', '.')).toFixed(3);

        var dataVcto = $('#DataVencimento').val().split('-');

        parcela.DataVencimento = dataVcto[2] + '/' + dataVcto[1] + '/' + dataVcto[0];

        if (parcela.Percentual > 100) {
            Swal.fire('Erro', 'Não é permitido inserir uma parcela maior que o valor total do pedido.', 'error');
            lpPedido.Hide();
            return false;
        }

        $.ajax({
            url: '/Pedido/EditarParcela',
            type: 'GET',
            data: {
                parcela: JSON.stringify(parcela),
                PermiteManutencao: permiteManutencao,
                EmissaoPedido: dataEmissao
            },
            async: false,
            success: function (result) {
                if (result.Data.success != undefined && result.Data.success == false) {
                    Sistema.Pedido.Load(false);
                    Swal.fire('Atenção', result.Data.message, 'error');
                    return;
                } else {
                    $('#modal-add-parcela').modal('hide');
                    Sistema.Pedido.GridParcelas(result.Data);

                    var sumPercentual = 0.00;
                    var percentRest = 0.00;
                    var rowCount = $('#tblParcelas tr').length - 1;

                    if (rowCount > 0) {
                        $('#tblParcelas > tbody > tr').each(function () {
                            //console.log($(this).find('td:eq(1)').html()); //sequencia
                            //console.log($(this).find('td:eq(2)').html()); //vencimento
                            //console.log($(this).find('td:eq(3)').html()); //percentual
                            //console.log($(this).find('td:eq(4)').html()); //valor

                            sumPercentual += SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(3)').html());
                        });
                    }

                    percentRest = 100 - sumPercentual;
                    $('#txtPercentRestante').val(percentRest.toFixed(2));

                    if (sumPercentual == 100) {
                        $('#lblparcvalida').val('Sim');
                        Sistema.Pedido.RecalcularItensPedido(false);
                        AjustarParcela();
                    } else {
                        $('#lblparcvalida').val('Não');
                    }

                    Sistema.Pedido.Load(false);
                }
            }
        });
    },

    RemoverParcela: function (sequencia) {
        Sistema.Pedido.Load(true);

        Swal.fire({
            icon: "question",
            title: "Atenção!",
            text: 'Deseja realmente remover esta parcela?',
            allowOutsideClick: false,
            showCancelButton: true,
            cancelButtonText: 'Não',
            confirmButtonText: 'Sim'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: '/Pedido/RemoveParcela',
                    data: { SequenciaParcela: sequencia },
                    success: function (result) {
                        Sistema.Pedido.GridParcelas(result.Data);
                        RecalcularTodasParcelas();
                        Sistema.Pedido.Load(false);
                    }
                });
            }
        });
    },

    RemoverParcelas: function () {
        Sistema.Pedido.Load(true);

        $.ajax({
            url: '/Pedido/RemoverTodasParcelas',
            success: function (result) {
                Sistema.Pedido.GridParcelas(result.Data);
                Sistema.Pedido.Load(false);
            }
        });
    },

    ValidaDescontoGeral: function () {
        Sistema.Pedido.Load(true);

        //Validando permissão de desconto
        var autorizaDescontoUsuario = $('#autorizaDesconto').val();
        var desconto = SapiensJS.Util.converteMoedaFloat($('#txtVlrDesconto').val());

        if ((autorizaDescontoUsuario == "" || autorizaDescontoUsuario == null || autorizaDescontoUsuario == "N") && desconto > 0) {
            Swal.close();
            $('#modal-aprovacao-geral').modal('show');
            return;
        } else if(desconto > 0) {
            Sistema.Pedido.RecalcularItensPedido(false);
        }
		
		Sistema.Pedido.Load(false);

        //var login = $('#txtLoginAprovacao').val();
        //var senha = $('#txtSenhaAprovacao').val();

        //if (login == null || login == "") {
        //    Swal.fire('Atenção', 'Informe o login.', 'warning');
        //    return;
        //}

        //if (senha == null || senha == "") {
        //    Swal.fire('Atenção', 'Informe a senha.', 'warning');
        //    return;
        //}

        //$.ajax({
        //    url: '/Pedido/ValidaDesconto/',
        //    data: { login: login, senha: senha },
        //    json: true,
        //    type: "GET",
        //    success: function (result) {
        //        if (result != "") {
        //            if (result.Data.venPds == "S") {
        //                $('#txtLoginAprovacao').val(null);
        //                $('#txtSenhaAprovacao').val(null);
        //                $('#modal-aprovacao').modal('hide');
        //                //usuarioDesc = result.Data.usuario;
        //                Sistema.Pedido.Load(false);
        //                Sistema.Pedido.RecalcularItensPedido();

        //                CalcularTotais();
        //                RecalcularTodasParcelas();
        //            } else {
        //                Swal.fire('Informação', 'Desconto não validado!', 'error');
        //                $('#txtLoginAprovacao').val(null);
        //                $('#txtSenhaAprovacao').val(null);
        //                //usuarioDesc = null;
        //                //popupValidarUsuario.Hide();
        //            }
        //        }
        //    },
        //    error: function (result) {
        //        var errors = result;
        //        Swal.fire('Validação', result, 'error');
        //    }
        //});
    },

    ValidarDesconto: function () {
        Sistema.Pedido.Load(true);

        var login = $('#txtLoginAprovacao').val();
        var senha = $('#txtSenhaAprovacao').val();

        if (login == null || login == "") {
            Swal.fire('Atenção', 'Informe o login.', 'warning');
            return;
        }

        if (senha == null || senha == "") {
            Swal.fire('Atenção', 'Informe a senha.', 'warning');
            return;
        }

        $.ajax({
            url: '/Pedido/ValidaDesconto/',
            data: { login: login, senha: senha },
            json: true,
            type: "GET",
            success: function (result) {
                if (result != "") {
                    if (result.Data.venPds == "S") {
                        $('#txtLoginAprovacao').val(null);
                        $('#txtSenhaAprovacao').val(null);
                        $('#modal-aprovacao').modal('hide');
                        //usuarioDesc = result.Data.usuario;
                        $('#usuarioDesconto').val(result.Data.usuario);
                        Sistema.Pedido.Load(false);
                        Sistema.Pedido.IncluirItem(true);

                        //CalcularTotais();
                        //RecalcularTodasParcelas();
                    } else {
                        Swal.fire('Informação', 'Desconto não validado!', 'error');
                        $('#txtLoginAprovacao').val(null);
                        $('#txtSenhaAprovacao').val(null);
                        //usuarioDesc = null;
                        //popupValidarUsuario.Hide();
                    }
                }
            },
            error: function (result) {
                var errors = result;
                Swal.fire('Validação', result, 'error');
            }
        });
    },

    ValidarDescontoEditar: function () {
        Sistema.Pedido.Load(true);

        var login = $('#txtLoginAprovacaoEditar').val();
        var senha = $('#txtSenhaAprovacaoEditar').val();

        if (login == null || login == "") {
            Swal.fire('Atenção', 'Informe o login.', 'warning');
            return;
        }

        if (senha == null || senha == "") {
            Swal.fire('Atenção', 'Informe a senha.', 'warning');
            return;
        }

        $.ajax({
            url: '/Pedido/ValidaDesconto/',
            data: { login: login, senha: senha },
            json: true,
            type: "GET",
            success: function (result) {
                if (result != "") {
                    if (result.Data.venPds == "S") {
                        $('#txtLoginAprovacaoEditar').val(null);
                        $('#txtSenhaAprovacaoEditar').val(null);
                        $('#modal-aprovacao-editar').modal('hide');
                        //usuarioDesc = result.Data.usuario;
                        $('#usuarioDesconto').val(result.Data.usuario);
                        Sistema.Pedido.Load(false);
                        Sistema.Pedido.EditarItem(true);

                        //CalcularTotais();
                        //RecalcularTodasParcelas();
                    } else {
                        Swal.fire('Informação', 'Desconto não validado!', 'error');
                        $('#txtLoginAprovacaoEditar').val(null);
                        $('#txtSenhaAprovacaoEditar').val(null);
                        //usuarioDesc = null;
                        //popupValidarUsuario.Hide();
                    }
                }
            },
            error: function (result) {
                var errors = result;
                Swal.fire('Validação', result, 'error');
            }
        });
    },

    Recalcular: function () {
        Sistema.Pedido.Load(true);

        var arredondamento = SapiensJS.Util.converteMoedaFloat($('#txtVlrArredondamento').val());
        var desconto = SapiensJS.Util.converteMoedaFloat($('#txtVlrDesconto').val());

        $.ajax({
            url: '/Pedido/Recalcular',
            data: {
                vlrArredondamento: arredondamento,
                vlrDesconto: desconto
            },
            type: 'GET',
            json: true,
            success: function (result) {
                if (result.Data.error == undefined || result.Data.error == false) {
                    //Sistema.Pedido.GridItemPedido(result.Data);
                    Sistema.Pedido.RecalcularItensPedido(false);
                    Sistema.Pedido.Load(false);
                } else {
                    Swal.fire('Atenção', result.Data.message, 'error');
                    Sistema.Pedido.Load(false);
                }
            },
            error: function (result) {
                Swal.fire('Atenção', 'Erro ao recalcular os itens.');
                Sistema.Pedido.Load(false);
            }
        });
    },

    SelecionarDataEntrega: function () {
        var dataEntrega = $('#txtDataEntrega').val();
        var rowCountItens = $('#tblItens tr').length - 1;

        if (dataEntrega != "" && rowCountItens > 0) {
            Sistema.Pedido.Load(true);

            $.ajax({
                type: "GET",
                url: "/Pedido/AlterarDataEntrega/",
                data: { 'dataSelecionada': dataEntrega },
                success: function (response) {
                    Sistema.Pedido.GridItemPedido(response.Data);
                    Sistema.Pedido.Load(false);
                }
            });
        }
    },

    TrocarFilial: function () {
        Sistema.Pedido.Load(true);

        var codFilial = $('#ddlTrocaFilial').val();

        $.ajax({
            url: '/Pedido/TrocaFilial',
            data: {
                codFilial: codFilial
            },
            type: 'GET',
            json: true,
            success: function (result) {
                if (result.success == false) {
                    Swal.fire('Atenção', result.erro, 'error');
                    Sistema.Pedido.Load(false);
                } else {
                    console.log(result);
                    location.href = "/Dashboard/Dashboardv1";
                    Sistema.Pedido.Load(false);
                }
            }
        })
    },

    AbrirModal: function (modal) {
        switch (modal) {
            case '#modal-cliente': Sistema.Pedido.GridCliente();
                break;
            case '#modal-grupo': Sistema.Pedido.GridGrupo();
                break;
            case '#modal-representante': Sistema.Pedido.GridRepresentantes();
                break;
            case '#modal-transportadora': Sistema.Pedido.GridTransportadora();
                break;
            case '#modal-transacao': Sistema.Pedido.GridTransacao();
                break;
            case '#modal-formapgto': Sistema.Pedido.GridFormaPgto();
                break;
            case '#modal-avalista': Sistema.Pedido.GridModalAvalista();
                break;
            case '#modal-condpgto':
                var codForma = $('#txtCodFormaPgto').val();

                if (codForma == null || codForma == "" || codForma == undefined) {
                    alert('Selecione uma Forma de Pagamento');
                    return;
                } else {
                    Sistema.Pedido.GridCondPgto();
                }

                break;
            case '#modal-veiculo':
                var codTransportadora = $('#txtCodTransportadora').val();

                if (codTransportadora == null || codTransportadora == "" || codTransportadora == undefined) {
                    alert('Selecione uma Transportadora');
                    return;
                } else {
                    Sistema.Pedido.GridVeiculos(codTransportadora);
                }

                break;
            case '#modal-add-item':
                $('#divBtnAddItem').show();
                $('#divBtnEditarItem').hide();

                setTimeout(function () {
                    $('input[name="txtCodProduto"]').focus();
                }, 500);

                $('#txtCodProduto').val(null);
                $('#cbDerivacao').val(null);
                $('#cbDeposito').val(null);
                $('#txtDescProd').val(null);
                $('#txtQtdeProduto').val('');
                $('#txtTabelaPreco').val(null);
                $('#txtPrecoBase').val(null);
                $('#txtPrecoUnit').val(null);
                $('#txtPerAcre').val(null);
                $('#txtVlrAcre').val(null);
                $('#txtPerDesc').val(null);
                $('#txtVlrDesc').val(null);
                $('#txtVlrLiq').val(null);

                var entrega = formataDataAmericana(DataAtual());

                $('#txtEntrega').text(entrega);

                $('#cbDerivacao').empty().append('<option value="">Selecione</option>');
                $('#cbDeposito').empty().append('<option value="">Selecione</option>');
                break;
            case '#modal-add-parcela':
                $('#divBtnAddParcela').show();
                $('#divBtnCancelarParcela').show();
                $('#divBtnEditarParcela').hide();
            default:
        }

        $(modal).modal('show');
    },

    Ping: function () {
        $.ajax({
            url: '/Pedido/Ping',
            async: false
        })
    }
}

function Enter(e) {
    if (e.keyCode == 13) {
        switch (document.activeElement.name) {
            case 'txtPesquisaPedido':
            case 'txtPesquisaPedidoCliente':
                Sistema.Pedido.ObterTodosPedidos();
                break;
            case 'txtPesquisaCliente':
                Sistema.Pedido.ObterTodosClientes();
                break;
            case 'txtBuscaProduto':
                Sistema.Pedido.ObterProdutos();
                break;
            default:
        }
    }
}

function CalcularTotais(updatePedido) {
    LimparTotais();

    var vl_frete = SapiensJS.Util.converteMoedaFloat($('#lblVlrFrete').val());
    var total_liquido = 0.00;
    var preco_bruto = 0.00
    var liquido_produtos = 0.00//gvEditing.cptotalLiquido != null ? gvEditing.cptotalLiquido : 0.00;
    var total_desconto = 0.00//gvEditing.cptotalDesconto != null ? gvEditing.cptotalDesconto.toFixed(2) : 0.00;
    var total_acrescimo = 0.00//gvEditing.cptotalAcrescimo != null ? gvEditing.cptotalAcrescimo.toFixed(2) : 0.00;
    var total_base = 0.00//gvEditing.cpPrecoBase != null ? gvEditing.cpPrecoBase.toFixed(2) : 0.00;

    $('#tblItens > tbody > tr').each(function () {
        var vlrLiq = SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(15)').html());
        var vlrDesc = SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(12)').html());
        var vlrAcr = SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(14)').html());
        var vlrBase = SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(7)').html());

        total_liquido += vlrLiq;
        total_desconto += vlrDesc;
        total_acrescimo += vlrAcr;
        total_base += vlrBase;
    });

    if (vl_frete != null && vl_frete != "") {
        $('#lblVlrFrete').val(SapiensJS.Util.formatReal(vl_frete));

        if ($('#cbFrete').val() == "C")
            total_liquido = (total_liquido * 1) + vl_frete;

    } else {
        $('#lblVlrFrete').val("0,00");
    }

    if (liquido_produtos > 0)
        total_liquido = (total_liquido * 1) + (liquido_produtos * 1);

    var preco_bruto = (liquido_produtos * 1) - (total_acrescimo * 1) + (total_desconto * 1);

    $('#lblVlrLiquido').val(SapiensJS.Util.formatReal(total_liquido.toFixed(2)));
    $('#text_valor_total_liquido').val(total_liquido.toFixed(2));

    //AjustarParcela();    
    RecalcularTodasParcelas(updatePedido);
}

function RecalcularTodasParcelas(updatePedido) {
    var item = $('#cbFrete').val();
    var tipoCondicaoPagamento = $('#condicao_pagamento_Especial').val();
    var parcelasValidas = $('#lblparcvalida').val();

    var emissao = $('#txtDataPedido').val().split('-');

    var dataVencimento = emissao[2] + '/' + emissao[1] + '/' + emissao[0];

    var valorLiquido = SapiensJS.Util.converteMoedaFloat($('#lblVlrLiquido').val());
    var valorLiquidoSemJuros = SapiensJS.Util.converteMoedaFloat($('#text_valor_total_liquido_sem_juros').val());
    var valorLiquidoComJuros = SapiensJS.Util.converteMoedaFloat($('#text_valor_total_liquido').val());
    var tipoCalculo = $('#condicao_pagamento_tipo_calculo').val();
    var totalPercentual = 0.00;
    var frete = 0;
    var rowCountParcelas = $('#tblParcelas tr').length - 1;
    var rowCountItens = $('#tblItens tr').length - 1;

    $('#tblParcelas > tbody > tr').each(function () {
        var percent = SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(3)').html());

        totalPercentual += percent;
    });

    if (item.value == 'C')
        frete = SapiensJS.Util.converteMoedaFloat($('#vlrFrete').val());

    if (tipoCondicaoPagamento == "S" || (tipoCondicaoPagamento == "N" && $('#txtVlrDesconto').val() != null)) {
        if (rowCountItens > 0 && rowCountParcelas > 0) {

            //lpPedido.Show();
            $.ajax({
                url: '/Pedido/RecalcularParcelas/',
                data: { frete: frete, TipoCondicaoPagamento: tipoCondicaoPagamento, ParcelasValidas: parcelasValidas, DataVencimento: dataVencimento, ValorLiquido: valorLiquido, ValorLiquidoSemJuros: valorLiquidoSemJuros, ValorLiquidoComJuros: valorLiquidoComJuros, TipoCalculoCondPgto: tipoCalculo, ValorTotalPercentual: totalPercentual },
                async: false,
                success: function (response) {
                    if (response.Success == false)
                        alerta("Erro", response.Message, "error");
                    else {
                        //$("#containerParcela").html(response);
                        Sistema.Pedido.GridParcelas(response.Data);

                        if (totalPercentual <= 100) {
                            percentRest = 100 - totalPercentual;

                            $('#txtPercentRestante').val(percentRest.toFixed(2));

                            if (totalPercentual == 100) {
                                $('#lblparcvalida').val('Sim');
                                ValidarDataParcela();
                            }
                            else
                                $('#lblparcvalida').val('Não');
                        }

                        if (updatePedido)
                            AjustarParcela();
                    }

                    //if (DataVencimento.IsVisible()) {
                    //    DataVencimento.SetText(DataAtual());
                    //    DataVencimento.SetFocus();
                    //}
                }
            });
        }
    }
}

function AjustarParcela() {
    var valorLiquidoPedido = $('#lblVlrLiquido').val().replace(',', '.');
    var valorTotalParcelas = 0.00;
    var percentualTotalParcelas = 0.00;

    $('#tblParcelas > tbody > tr').each(function () {
        valorTotalParcelas += SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(4)').html());
        percentualTotalParcelas += SapiensJS.Util.converteMoedaFloat($(this).find('td:eq(3)').html());
    });

    $.ajax({
        url: '/Pedido/AjustaParcelas/',
        data: { valorLiquidoPedido: valorLiquidoPedido, valorTotalParcelas: valorTotalParcelas, percentualTotalParcelas: percentualTotalParcelas },
        success: function (response) {
            if (response.Success == false)
                Swal.fire("Erro", response.Message, "error");
            else
                Sistema.Pedido.GridParcelas(response.Data);
        }
    });
}

function ValidarDataParcela() {
    $.ajax({
        url: '/Pedido/ObterDataVencimento',
        json: true,
        success: function (result) {
            if (DataAtual() == result.Data)
                $('#lblparcvalida').val('Não');
        }
    });
}

function CalcularValorParcela() {
    if ($('#txtPercentual').val() != null && $('#txtPercentual').val() != "") {
        //var valorLiquido = SapiensJS.Util.converteMoedaFloat($('#lblVlrLiquido').val());
        //var percentual = SapiensJS.Util.converteMoedaFloat($('#txtPercentual').val());
        var valorLiquido = $('#lblVlrLiquido').val().replace(',', '.');
        var percentual = $('#txtPercentual').val().replace(',', '.');

        var valorParcela = (valorLiquido * percentual) / 100;

        $('#txtValorParcela').val(SapiensJS.Util.formatReal(valorParcela.toFixed(2)));
    }

}

function CalcularPercentualParcela() {
    if ($('#txtValorParcela').val() != null && $('#txtValorParcela').val() != "") {
        //var valorLiquido = SapiensJS.Util.converteMoedaFloat($('#lblVlrLiquido').val());
        //var valorParcela = SapiensJS.Util.converteMoedaFloat($('#txtValorParcela').val());
        var valorLiquido = $('#lblVlrLiquido').val().replace(',', '.');
        var valorParcela = $('#txtValorParcela').val().replace(',', '.');

        var percentual = (valorParcela / valorLiquido) * 100;

        $('#txtPercentual').val(SapiensJS.Util.formatReal(percentual.toFixed(2)));
    }

}

function LimparTotais() {
    $('#lblVlrLiquido').val(0);
    $('#txtValorFreteTotal').val(0);
}

function calcularDesconto(origem, valor, recalculaAc) {
    var quantidadePedido = $('#txtQtdeProduto').val().replace(",", ".");

    if (origem != "" && quantidadePedido != "" && quantidadePedido != null) {

        if (valor == "" || valor == null)
            valor = "0";

        //if (recalculaAc) {
        //    calcularAcrescimo('percentual', "0,00", false);
        //    calcularAcrescimo('valor', "0,00", false);
        //}

        var precoUnit = SapiensJS.Util.converteMoedaFloat($('#txtPrecoUnit').val().replace(".", ","));
        var valorPar = SapiensJS.Util.converteMoedaFloat(valor.replace(".", ","));
        var totalBrutoCalculado = (precoUnit * quantidadePedido);
        totalBrutoCalculado = totalBrutoCalculado.toFixed(5);

        if (origem == "percentual") {

            if (valorPar > 100)
                valorPar = 100;

            var valorLiquido = (totalBrutoCalculado * (100.00 - valorPar) / 100).toFixed(5);
            var valorDesconto = (((totalBrutoCalculado * 100) - (valorLiquido * 100)) / 100).toFixed(2);

            $('#txtPerDesc').val(SapiensJS.Util.formatReal(valorPar));
            $('#txtVlrDesc').val(SapiensJS.Util.formatReal(valorDesconto));

        } else if (origem == "valor") {

            if (valorPar > parseFloat(totalBrutoCalculado)) {
                valorPar = 0;

            }

            var percentualCalculado = parseFloat(valorPar / totalBrutoCalculado);
            percentualCalculado = percentualCalculado * 100;
            percentualCalculado = (Math.floor(percentualCalculado * 100)) / 100;

            $('#txtVlrDesc').val(SapiensJS.Util.formatReal(valorPar));
            $('#txtPerDesc').val(SapiensJS.Util.formatReal(percentualCalculado));

        }
    }

    calcularValorLiquido();
}

function calcularAcrescimo(origem, valor, recalculaDc) {
    var quantidadePedido = parseFloat($('#txtQtdeProduto').val().replace(",", "."));

    if (origem != "" && quantidadePedido != "" && quantidadePedido != null) {

        if (valor == "" || valor == null)
            valor = "0";

        //if (recalculaDc) {
        //    calcularDesconto('percentual', "0,00", false);
        //    calcularDesconto('valor', "0,00", false);
        //}

        var precoUnit = SapiensJS.Util.converteMoedaFloat($('#txtPrecoUnit').val().replace(".", ","));
        var valorPar = SapiensJS.Util.converteMoedaFloat(valor.replace(".", ","));
        var totalBrutoCalculado = (precoUnit * quantidadePedido).toFixed(5);
        if (origem == "percentual") {
            if (valorPar > 100)
                valorPar = 100;
            var valorLiquido = (totalBrutoCalculado * (100.00 - valorPar) / 100).toFixed(5);
            var valorAcrescimo = (((totalBrutoCalculado * 100) - (valorLiquido * 100)) / 100);

            $('#txtPerAcre').val(SapiensJS.Util.formatReal(valorPar));
            $('#txtVlrAcre').val(SapiensJS.Util.formatReal(valorAcrescimo.toFixed(2)));

        } else if (origem == "valor") {
            if (valorPar > parseFloat(totalBrutoCalculado)) {
                valorPar = 0;
            }
            var percentualCalculado = parseFloat(valorPar / totalBrutoCalculado);
            percentualCalculado = percentualCalculado * 100;
            percentualCalculado = (Math.floor(percentualCalculado * 100)) / 100;

            $('#txtVlrAcre').val(SapiensJS.Util.formatReal(valorPar.toFixed(2)));
            $('#txtPerAcre').val(SapiensJS.Util.formatReal(percentualCalculado));
        }
    }

    calcularValorLiquido();
}

function calcularValorLiquido() {

    var pcUnitario = $('#txtPrecoUnit').val() != null ? SapiensJS.Util.converteMoedaFloat($('#txtPrecoUnit').val().replace(".", ",")) : 0;
    var pcBase = $('#txtPrecoBase').val() != null ? SapiensJS.Util.converteMoedaFloat($('#txtPrecoBase').val().replace(".", ",")) : 0;
    var qtde = $('#txtQtdeProduto').val() != null ? SapiensJS.Util.converteMoedaFloat($('#txtQtdeProduto').val().replace(".", ",")) : 0;
    var perctDesc = $('#txtPerDesc').val() != null ? SapiensJS.Util.converteMoedaFloat($('#txtPerDesc').val().replace(".", ",")) : 0;
    var vlrDesc = 0.00;
    var vlrAcre = 0.00;

    if ($('#txtCodProduto').val() != "" && $('#txtCodProduto').val() != null && $('#txtDescProd').val() != "" && $('#txtDescProd').val() != null && $('#txtQtdeProduto').val() != "" && $('#txtQtdeProduto').val() != null) {

        if (pcBase == 0) {
            Swal.fire('Informação', 'Preço base do produto não localizado.', 'error');
            $('#txtVlrLiq').val(null);
            return false;
        }

        if ($('#txtVlrDesc').val() != "" && $('#txtVlrDesc').val() != null)
            vlrDesc = $('#txtVlrDesc').val().replace(",", ".");
        else
            vlrDesc = 0.00;

        var perctAcre = SapiensJS.Util.converteMoedaFloat($('#txtPerAcre').val());

        if ($('#txtVlrAcre').val() != "" && $('#txtVlrAcre').val() != null)
            vlrAcre = $('#txtVlrAcre').val().replace(",", ".");
        else
            vlrAcre = 0.00;

        vlrDesc = (vlrDesc * 100) / 100;
        vlrAcre = (vlrAcre * 100) / 100;

        //var totalLiq = parseFloat($('#lblVlrLiquido').val().replace(",", "."));

        var total = 0;
        var totalBruto = 0;
        total = (pcUnitario * qtde);
        totalBruto = (pcUnitario * qtde);

        total = parseFloat(total);

        var totalBase = 0;
        totalBase = (pcBase * qtde);

        if (perctDesc > 0) {
            if (perctDesc > 100) {
                $('#txtPerDesc').val("0,00");
                return false;
            }
        }

        if (vlrDesc > 0) {
            if (vlrDesc > total) {
                $('#txtVlrDesc').val("0,00");
            }

            total = parseFloat(total) - parseFloat(vlrDesc);
        }

        if (perctAcre > 0) {
            if (perctAcre > 100) {
                $('#txtPerAcre').val("0,00");
                return false;
            }
        }

        if (vlrAcre > 0) {
            if (vlrAcre > total) {
                $('#txtVlrAcre').val("0,00");
                return false;
            }

            total = (parseFloat(total) + parseFloat(vlrAcre)).toFixed(5);
        }

        //totalLiq = parseFloat(totalLiq) + parseFloat(total);

        $('#txtVlrLiq').val(SapiensJS.Util.formatReal(parseFloat(total).toFixed(3)));
        //$('#txtPrecoBase').val(totalBase);
        //ValorBruto.SetValue(totalBruto);
    }
}

function FormatarData(date) {
    var data = new Date(date);
    var dd = data.getDate() + 1;
    var mm = data.getMonth() + 1;
    var yyyy = data.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    data = dd + '/' + mm + '/' + yyyy;

    return data;
}

function formataDataAmericana(data) {
    if (data != null) {
        var splt = data.split('/');
        var novaData = splt[1] + "/" + splt[2] + "/" + splt[0];
        var dataAmericana = new Date(novaData);

        return dataAmericana;
    }
}

function formataDataAmericanaParcela(data) {
    //dd/mm/yyyy
    if (data != null) {
        var splt = data.split('/');
        var novaData = splt[1] + "/" + splt[0] + "/" + splt[2];
        var dataAmericana = new Date(novaData);

        return dataAmericana;
    }
}

function DataAtual() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = dd + '/' + mm + '/' + yyyy;

    return today;
}