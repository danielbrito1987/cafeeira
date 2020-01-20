/// <reference path="jquery-1.11.3.min.js" />
/// <reference path="Script.js" />
/// <reference path="sweetalert.min.js" />

senha = "";

function mascaraSenha(s, e) {
    var theEvent = e.htmlEvent || window.event;
    var tecla = theEvent.key;

    if (theEvent.keyCode != 9 && theEvent.keyCode != 16 && theEvent.keyCode != 13)
        senha += tecla;
    else if (theEvent.keyCode == 13)
        Login();
    else
        Password.SetFocus();

    var str = Password.GetText();
    //var psw = Password.GetText();
    //senha += psw;
    str = str.replace(/[a-zA-Z0-9@!]/, '*');
    Password.SetText(str);
}

function Login() {
    var usr = Username.GetText();
    var pass = Password.GetText();
    senha = "";
    var login = {};

    login.UserName = usr;
    login.Password = pass;

    if (login != null) {
        executeRequest({
            url: '/Account/Logon',
            data: { loginPost: JSON.stringify(login) },
            success: function (result) {
                Password.SetText('');
                window.location.href = '/Home/Index';
            },
            error: function (result) {

            }
        })
    }
}


var SapiensJS = SapiensJS || {};

SapiensJS.Util = {

    PopularCombo: function (codigoValue, objetoCombo, url) {
        $('.' + objetoCombo).empty();

        executeRequestSemModal({
            url: url,
            success: function (result) {

                for (var i = 0; i < result.length; i++) {
                    var texto = result[i].Nome != null ? result[i].Nome : result[i].Descricao;
                    $('.' + objetoCombo).append('<option value="' + result[i].Codigo + '">' + texto + '</option>');
                }

                if (codigoValue != "")
                    SelecionarCombo('.' + objetoCombo, codigoValue);
                $('.' + objetoCombo).val(codigoValue);


            }
        });
    },

    SomenteNumero: function (e) {
        var theEvent = e.htmlEvent || window.event;
        var tecla = theEvent.keyCode || theEvent.which;

        if (tecla == 44 && (theEvent.key.indexOf(',') > 0 || theEvent.key.length == 0)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault)
                theEvent.preventDefault();
        };

        if (tecla > 47 && tecla < 58) {
            return true;
        }
        else if (tecla == 8 || tecla == 0 || tecla == 44 || tecla == 9) {
            return true;
        } else {
            theEvent.returnValue = false;
            if (theEvent.preventDefault)
                theEvent.preventDefault();
        }
    },

    SomenteNumeroEUmaVirgula: function (s, e, numerosAntesDaVirgula, numerosDepoisDaVirgula) {
        var theEvent = e.htmlEvent || window.event;
        var tecla = theEvent.keyCode || theEvent.which;

        if (tecla == 44 && (theEvent.key.indexOf(',') > 0 || theEvent.key.length == 0)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault)
                theEvent.preventDefault();
        };

        numerosAntesDaVirgula = numerosAntesDaVirgula ? numerosAntesDaVirgula : 0;
        numerosDepoisDaVirgula = numerosDepoisDaVirgula ? numerosDepoisDaVirgula : 0;

        var qtdNumerosAntesVirgula = s.GetText().split(',')[0].length;
        var qtdNumerosDepoisVirgula = s.GetText().split(',').length > 1 ? s.GetText().split(',')[1].length : 0;

        if ((tecla > 47 && tecla < 58) && qtdNumerosAntesVirgula < numerosAntesDaVirgula && qtdNumerosDepoisVirgula < numerosDepoisDaVirgula) {
            return true;
        }
        else {
            if (tecla == 8 || tecla == 0 || tecla == 44 || tecla == 9) {
                return true;
            }
            else {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }
    },

    SomenteTAB: function (s, e) {
        var theEvent = e.htmlEvent || window.event;
        var tecla = theEvent.keyCode || theEvent.which;

        if (tecla == 9)
            return true;

        theEvent.returnValue = false;
        theEvent.preventDefault();
    },

    money: function () {
        var el = this
        , exec = function (v) {
            v = v.replace(/\D/g, "");
            v = new String(Number(v));
            var len = v.length;
            if (1 == len)
                v = v.replace(/(\d)/, "0.0$1");
            else if (2 == len)
                v = v.replace(/(\d)/, "0.$1");
            else if (len > 2) {
                v = v.replace(/(\d{2})$/, '.$1');
            }
            return v;
        };

        setTimeout(function () {
            el.value = exec(el.value);
        }, 1);
    },



    PopularComboMulti: function (codigoValue, objetoCombo, url, customCombo, noneSelected, semModal) {
        $('.' + objetoCombo).empty();

        if (semModal) {

            executeRequestSemModal({
                url: url,
                success: function (result) {

                    for (var i = 0; i < result.length; i++) {
                        var texto = result[i].Nome != null ? result[i].Nome : result[i].Descricao;
                        $('.' + objetoCombo).append('<option value="' + result[i].Codigo + '">&nbsp&nbsp' + texto + '</option>');
                    }

                    if (codigoValue != "")
                        SelecionarCombo('.' + objetoCombo, codigoValue);
                    $('.' + objetoCombo).val(codigoValue);


                    if (customCombo == "Multi") {

                        $('.' + objetoCombo).multiselect({
                            header: true,
                            height: 175,
                            minWidth: 250,
                            classes: '',
                            checkAllText: 'Marcar todos',
                            uncheckAllText: 'Desmarcar todos',
                            noneSelectedText: noneSelected != null ? noneSelected : 'Selecione itens',
                            selectedText: '# selecionado(s)',
                            selectedList: 0,
                            show: null,
                            hide: null,
                            autoOpen: false,
                            multiple: true,
                        }).multiselectfilter();

                    } else if (customCombo == "Filter") {

                        $(".filterselect").multiselect({
                            multiple: false,
                            header: false,
                            header: "Selecione valor(es)",
                            noneSelectedText: "Selecione valor(es)",
                            selectedList: 0
                        });

                    }

                }
            });
        }
        else {
            executeRequest({
                url: url,
                success: function (result) {

                    for (var i = 0; i < result.length; i++) {
                        var texto = result[i].Nome != null ? result[i].Nome : result[i].Descricao;
                        $('.' + objetoCombo).append('<option value="' + result[i].Codigo + '">&nbsp&nbsp' + texto + '</option>');
                    }

                    if (codigoValue != "")
                        SelecionarCombo('.' + objetoCombo, codigoValue);
                    $('.' + objetoCombo).val(codigoValue);


                    if (customCombo == "Multi") {

                        $('.' + objetoCombo).multiselect({
                            header: true,
                            height: 175,
                            minWidth: 250,
                            classes: '',
                            checkAllText: 'Marcar todos',
                            uncheckAllText: 'Desmarcar todos',
                            noneSelectedText: noneSelected != null ? noneSelected : 'Selecione itens',
                            selectedText: '# selecionado(s)',
                            selectedList: 0,
                            show: null,
                            hide: null,
                            autoOpen: false,
                            multiple: true,
                        }).multiselectfilter();

                    } else if (customCombo == "Filter") {

                        $(".filterselect").multiselect({
                            multiple: false,
                            header: false,
                            header: "Selecione valor(es)",
                            noneSelectedText: "Selecione valor(es)",
                            selectedList: 0
                        });

                    }

                }
            });
        };
    },

    PopularComboMultiParametro: function (parameters, objetoCombo, url, customCombo, noneSelected) {
        $('.' + objetoCombo).empty();

        executeRequest({
            url: url,
            data: { codigo: parameters },
            success: function (result) {

                for (var i = 0; i < result.length; i++) {
                    var texto = result[i].Nome != null ? result[i].Nome : result[i].Descricao;
                    $('.' + objetoCombo).append('<option value="' + result[i].Codigo + '">' + texto + '</option>');
                }



                if (customCombo == "Multi") {

                    $('.' + objetoCombo).multiselect({
                        header: true,
                        height: 175,
                        minWidth: 250,
                        classes: '',
                        checkAllText: 'Marcar todos',
                        uncheckAllText: 'Desmarcar todos',
                        noneSelectedText: noneSelected != null ? noneSelected : 'Selecione itens',
                        selectedText: '# selecionado(s)',
                        selectedList: 0,
                        show: null,
                        hide: null,
                        autoOpen: false,
                        multiple: true,
                    }).multiselectfilter();

                } else if (customCombo == "Filter") {

                    $(".filterselect").multiselect({
                        multiple: false,
                        header: false,
                        header: "Selecione valor(es)",
                        noneSelectedText: "Selecione valor(es)",
                        selectedList: 0
                    });

                }

            }
        });
    },


    PopularComboFiltro: function (codigoValue, objetoCombo, url, nomePrimeiraLinha) {
        $('.' + objetoCombo).empty();

        executeRequestSemModal({
            url: url,
            success: function (result) {

                $('.' + objetoCombo).append('<option value="">' + nomePrimeiraLinha + '</option>');

                for (var i = 0; i < result.length; i++) {
                    var texto = result[i].Nome != null ? result[i].Nome : result[i].Descricao;
                    $('.' + objetoCombo).append('<option value="' + result[i].Codigo + '">' + texto + '</option>');
                }

                if (codigoValue != "")
                    SelecionarCombo('.' + objetoCombo, codigoValue);
                $('.' + objetoCombo).val(codigoValue);
            }
        });
    },

    PopularComboFiltroParam: function (codigoValue, objetoCombo, url, nomePrimeiraLinha, codigo) {
        $('.' + objetoCombo).empty();

        executeRequestSemModal({
            url: url,
            data: { codigo: codigo },
            success: function (result) {

                $('.' + objetoCombo).append('<option value="">' + nomePrimeiraLinha + '</option>');

                for (var i = 0; i < result.length; i++) {
                    var texto = result[i].Nome != null ? result[i].Nome : result[i].Descricao;
                    $('.' + objetoCombo).append('<option value="' + result[i].Codigo + '">' + texto + '</option>');
                }

                if (codigoValue != "")
                    SelecionarCombo('.' + objetoCombo, codigoValue);
                $('.' + objetoCombo).val(codigoValue);
            }
        });
    },

    getNextTabindex: function (currentTabIndex, add) {

        add = 1;

        return $("[tabindex]").index($("[tabindex=" + currentTabIndex + "]")) + add;

    },

    getNextTabindex: function (elementId) {

        //É preciso adicionar o sufixo _I ao final do ID por causa do DevExpress
        var currentTabIndex = document.getElementById(elementId + "_I").tabIndex;

        $('[tabIndex=' + (+currentTabIndex + 1) + ']')[0].focus();
        //return $("[tabindex]").index($("[tabindex=" + currentTabIndex + 1 + "]"));

    },

    getTabindex: function (elementId) {

        //É preciso adicionar o sufixo _I ao final do ID por causa do DevExpress
        var currentTabIndex = document.getElementById(elementId + "_I").tabIndex;

        $('[tabIndex=' + (+currentTabIndex) + ']')[0].focus();
        //return $("[tabindex]").index($("[tabindex=" + currentTabIndex + 1 + "]"));

    },

    ArredondarCasaDecimalFloat: function (valor, numCasas) {

        return parseFloat(valor.toFixed(numCasas));

    },

    formatarCampo: function (src, event, mask) {
        var i = src.value.length;
        var saida = mask.substring(i, i + 1);
        var ascii = event.keyCode;
        if (saida == "A") {
            if ((ascii >= 97) && (ascii <= 122)) { event.keyCode -= 32; }
            else { event.keyCode = 0; }
        } else if (saida == "0") {
            if ((ascii >= 48) && (ascii <= 57)) { return }
            else { event.keyCode = 0 }
        } else if (saida == "#") {
            return;
        } else {
            src.value += saida;


            if (saida == "A") {
                if ((ascii >= 97) && (ascii <= 122)) { event.keyCode -= 32; }
            } else { return; }
        }
    },

    // CRIAÇÃO DE FUNÇÕES
    RetornarStatusPadrao: function (status) {

        var strHTMLStatus = "";


        if (status == "true" || status == "1") {
            strHTMLStatus = "<span class='label label-success'><i class='fa fa-check'></i> Sim</span>";
        } else {
            strHTMLStatus = "<span class='label label-danger'><i class='fa fa-times'></i>  Não</span>";
        }

        return strHTMLStatus;

    },
    //Limpando todos os itens do form

    clearForm: function (formName) {

        $('#' + formName).each(function () {
            this.reset();
        });

    },

    //ajustaColuna: function(tableref){

    //    $('#' + tableref).colResizable({
    //        liveDrag: true,
    //        fixed: false
    //    });
    //},

    PreparaCodigoParametro: function (strCodigo) {

        var str = new String(strCodigo);
        return str.substring(3, (str.length));

    },
    //Função comum para recuperar querystrings
    getQuerystring: function (key, default_) {
        if (default_ == null) default_ = "";
        key = key.replace(/[[]/, "[").replace(/[]]/, "]");
        var regex = new RegExp("[?&]" + key + "=([^&#]*)");
        var qs = regex.exec(window.location.href);
        if (qs == null)
            return default_;
        else
            return qs[1];
    },

    bytesToSize: function (bytes) {
        var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        if (bytes == 0) return '0 Byte';
        var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
        return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
    },

    removerAcentos: function (newStringComAcento) {

        var string = newStringComAcento;
        var mapaAcentosHex = {
            a: /[\xE0-\xE6]/g,
            e: /[\xE8-\xEB]/g,
            i: /[\xEC-\xEF]/g,
            o: /[\xF2-\xF6]/g,
            u: /[\xF9-\xFC]/g,
            c: /\xE7/g,
            n: /\xF1/g
            //,
            //'-': /\s/g
        };

        for (var letra in mapaAcentosHex) {
            var expressaoRegular = mapaAcentosHex[letra];
            string = string.replace(expressaoRegular, letra);
        }

        return string;
    },

    formatReal: function (numero) {

        //alert(SapiensJS.Util.formatReal(numero));

        return SapiensJS.Util.colocaMascaraBrasil(numero);
        //return SapiensJS.Util.converterFloatReal(numero);
        //return SapiensJS.Util.moeda(numero, 2, ',', '.');

    },

    converteMoedaFloat: function (valor) {

        if (valor === "" || valor === 0 || valor == undefined) {
            valor = 0;
        } else {
            valor = valor.replace(/\./g, "");
            valor = valor.replace(",", ".");
            valor = valor * 1;
        }
        return valor;

    },

    converteMoedaFloatDevExpress: function (valor) {

        if (valor === "" || valor === 0) {
            valor = 0;
        } else {
            valor = valor * 1;
        }
        return valor;

    },



    moeda: function (valor, casas, separdor_decimal, separador_milhar) {

        var valor_total = parseInt(valor * (Math.pow(10, casas)));
        var inteiros = parseInt(parseInt(valor * (Math.pow(10, casas))) / parseFloat(Math.pow(10, casas)));
        var centavos = parseInt(parseInt(valor * (Math.pow(10, casas))) % parseFloat(Math.pow(10, casas)));


        if (centavos % 10 == 0 && centavos + "".length < 2) {
            centavos = centavos + "0";
        } else if (centavos == 0) {
            var inteiros = inteiros % 1000;
            if (inteiros == 0) {
                inteiros = "000";
            } else if (inteiros < 10) {
                inteiros = "00" + inteiros;
            } else if (inteiros < 100) {
                inteiros = "0" + inteiros;
            }
            var retorno = milhar(milhares, separador_milhar) + "" + separador_milhar + "" + inteiros;
            return retorno;
        }
        else {
            return inteiros;
        }

    },


    colocaMascaraBrasil: function (numero) {
        var numeroStr = '';
        numeroStr = numero.toString();
        if (numero - (Math.round(numero)) == 0) {
            if (numeroStr.indexOf('.')) {
                numeroStr = Math.floor(numero).toString();
            }
            numeroStr = numeroStr + ',00';
            return numeroStr;
        }
        var parteDecial = numeroStr.slice(numeroStr.indexOf('.'), numeroStr.length);
        if (parteDecial.length == 2) {
            parteDecial = parteDecial + '0';
        }
        parteDecial = parteDecial.replace('.', ',');
        var parteInteira = numeroStr.slice(0, numeroStr.indexOf('.'));
        var vetorParteInteira = [];
        for (var i = 0; i < parteInteira.length; i++) {
            vetorParteInteira.push(parteInteira.slice(i, i + 1));
        }

        var parteInteiraFinal = '';
        var comprimento = vetorParteInteira.length - 1;
        for (var i = 0; i < vetorParteInteira.length; i++) {
            if (((((comprimento - i) + 1) / 3) - (Math.floor((((comprimento - i) + 1) / 3)))) == 0 && (((comprimento - i) + 1) != vetorParteInteira.length)) {
                parteInteiraFinal = parteInteiraFinal + '.' + vetorParteInteira[i];
            } else {
                parteInteiraFinal = parteInteiraFinal + vetorParteInteira[i];
            }
        }

        var valorFinalCorrigido = 0.00

        //alert(parteDecial);
        if (parteDecial.length > 3) {

            parteDecial = parteDecial.substring(0, 3);
            //alert(parteDecial);

        }

        valorFinalCorrigido = parteInteiraFinal + parteDecial;



        //valorFinalCorrigidoArredondado = parseFloat(valorFinalCorrigido).toFixed(2);
        //alert('valor com erro: ' + valorFinalCorrigido);
        return valorFinalCorrigido;
    },


    formatReal_old: function (int) {
        var int = parseInt(mixed.toFixed(2).toString().replace(/[^\d]+/g, ''));
        var tmp = int + '';
        tmp = tmp.replace(/([0-9]{2})$/g, ",$1");
        if (tmp.length > 6)
            tmp = tmp.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");

        return tmp;
    },

    converterFloatReal: function (valor) {
        var inteiro = null, decimal = null, c = null, j = null;
        var aux = new Array();
        alert(valor);

        valor = valor.toFixed(2);
        valor = "" + valor;
        c = valor.indexOf(".", 0);
        //encontrou o ponto na string
        if (c > 0) {
            //separa as partes em inteiro e decimal
            inteiro = valor.substring(0, c);
            decimal = valor.substring(c + 1, valor.length);
            if (decimal.length === 1) {
                decimal += "0";
            }

        } else {
            inteiro = valor;
        }

        //pega a parte inteiro de 3 em 3 partes
        for (j = inteiro.length, c = 0; j > 0; j -= 3, c++) {
            aux[c] = inteiro.substring(j - 3, j);
        }

        //percorre a string acrescentando os pontos
        inteiro = "";
        for (c = aux.length - 1; c >= 0; c--) {
            inteiro += aux[c] + '.';
        }
        //retirando o ultimo ponto e finalizando a parte inteiro

        inteiro = inteiro.substring(0, inteiro.length - 1);

        decimal = parseInt(decimal);
        if (isNaN(decimal)) {
            decimal = "00";
        } else {
            decimal = "" + decimal;
            if (decimal.length === 1) {
                decimal = "0" + decimal;
            }
        }

        valor = inteiro + "," + decimal;

        return valor;
    },

    PreparaAnexos: function (evt, divPost, divView) {
        var files = evt.target.files; // FileList object
        $('#' + divPost).html('');
        $('#' + divView).html('');


        // Loop through the FileList and render image files as thumbnails.
        for (var i = 0, f; f = files[i]; i++) {


            var reader = new FileReader();

            // Closure to capture the file information.
            reader.onload = (function (theFile) {

                var desc = SapiensJS.Util.removerAcentos(theFile.name);

                return function (e) {
                    // Render thumbnail.

                    var validatedFile = false;
                    var strvalidatedFile = "<span style='color: red'>O Limite  para upload é de " + SapiensJS.Util.bytesToSize(limitUpload) + ", este arquivo não será enviado! </span>"
                    if (escape(theFile.size) <= limitUpload) {
                        validatedFile = true;
                        strvalidatedFile = "<span style='color: green'>Tamanho do arquivo validado!</span>"
                    }


                    if (validatedFile) {
                        $('<div>', {
                            id: 'itemAnexo',
                            html: '{"Descricao":"' + desc + '", "Tipo":"' + escape(theFile.type) + '", "Size":' + escape(theFile.size) + ', "Validated":' + validatedFile + ', "Base64":"' + e.target.result + '"}'
                        }).appendTo('#' + divPost);

                    }

                    $('<div>', {
                        id: 'itemAnexo',
                        css: {
                            width: '60%',
                            height: 'auto',
                            background: '#eee',
                            fontFamily: 'Tahoma',
                            fontSize: '0.8em',
                            margin: '10px',
                            padding: '10px'
                        },
                        html: '<span><b>Nome: </b>' + desc + '<br /> <b>Tipo do arquivo: </b>' + escape(theFile.type) + '<br /><b>Tamanho: </b>' + SapiensJS.Util.bytesToSize(escape(theFile.size)) + '<br />' + strvalidatedFile + '</span>'
                    }).appendTo('#' + divView);

                };
            })(f);


            // Read in the image file as a data URL.
            reader.readAsDataURL(f);
        }
    },
    //deve ser chamado evento key press
    ClicarBotaoAoPressionarEnter: function (idBotao) {
        //verifica se tecla pressionada foi enter
        var tecla = (window.event) ? event.keyCode : e.which;
        if (tecla == 13)
            $("#" + idBotao).click();
    }
}

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}

function alerta(titulo, mensagem, type, callback) {
    callback = callback == null ? function () { } : callback;

    swal({
        title: titulo,
        text: mensagem,
        icon: type,
        animation: "false",
        closeOnClickOutside: false,
        button: {
            text: "FECHAR",
            value: true,
            visible: true,
            closeModal: true,
        },
        content: "html"
    }).then((retorno) => {
        callback();
    });
    $(".swal-text").html(mensagem);
    $(".swal-button").focus();

}


function confirmacao(titulo, mensagem, funcCancel, funcConfirm) {
    var alerta = swal({
        title: titulo,
        text: mensagem,
        icon: "warning",
        buttons: {
            cancel: {
                text: "CANCELAR",
                value: false,
                visible: true,
                className: "",
                closeModal: true,
            },
            confirm: {
                text: "CONFIRMAR",
                value: true,
                visible: true,
                className: "",
                closeModal: true
            }
        },
        dangerMode: true,
    })
        .then((retorno) => {
            if (retorno)
                funcConfirm.call();
            else
                funcCancel.call();
        });
    $(".swal-button").focus();
}

function confirmacaoV2(titulo, mensagem, callback, txtBtnCancel, txtBtnConfirmar) {
    txtBtnCancel = txtBtnCancel != null ? txtBtnCancel : "CANCELAR";
    txtBtnConfirmar = txtBtnConfirmar != null ? txtBtnConfirmar : "CONFIRMAR";

    var alerta = swal({
        title: titulo,
        text: mensagem,
        icon: "warning",
        buttons: {
            cancel: {
                text: txtBtnCancel,
                value: false,
                visible: true,
                className: "",
                closeModal: true,
            },
            confirm: {
                text: txtBtnConfirmar,
                value: true,
                visible: true,
                className: "",
                closeModal: true
            }
        },
        dangerMode: true,
    })
        .then((retorno) => {
            callback(retorno);
        });
    $(".swal-button").focus();
}

//converte campos do formulário para Json
function formToJson(idForm) {
    var o = {};

    var a = $(idForm).serializeArray();

    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });

    return o;
}

function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//auxilia nos métodos de população de selectbox
function addOption(selectbox, text, value) {
    var optn = document.createElement("OPTION");
    optn.text = text;
    optn.value = value;
    document.getElementById(selectbox).options.add(optn);
}

function executeRequest(config) {
    var modal;
    if (config.json) {
        config.contentType = "application/json; charset=utf-8";
        config.dataType = "json";

        //config.data = JSON.stringify(config.data);
    }

    if (!config.dontShowPanel) {
        modal = ASPxClientControl.GetControlCollection().GetByName(config.modalID);
        if (modal != null) modal.Show();
    }

    return $.ajax({
        url: config.url,
        data: config.data,
        cache: false,
        async: (config.async === undefined || config.async == true) ? true : false,
        contentType: config.contentType,
        dataType: config.dataType,
        crossDomain: true,
        method: config.type,
        beforeSend: SistemaWeb.All.Load("show"),
        complete: SistemaWeb.All.Load("none"),
        success: function (result) {
            if (config.json) {
                if (result.Success)
                    config.success.call(this, result.Data);
                else {
                    alerta("Erro", result.Message, 'error');
                    btnFecharPedido.SetEnabled(true);                    
                }
            } else
                config.success.call(this, result);
            if (!config.dontShowPanel) {
                if (modal != null) modal.Hide();
            }

        },
        error: function (jqXHR, textStatus, errorThrown, result) {
            if (!config.dontShowPanel) {
                if (modal != null) modal.Hide();
            } else {
                config.error.call(this, errorThrown);
            }

            alerta("Erro", errorThrown, 'error');
            btnFecharPedido.SetEnabled(true);
        }
    });

}



function executeRequestImpressaoVisitante(config) {

    $.ajax({
        url: config.url,
        type: 'POST',
        crossDomain: true,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ base64StringPDF: config.data }),

        success: function (result) {
            if (result && result == "Sucesso") {
                //alerta('Alerta', 'Visitante cadastrado com Sucesso!', 'sucesso.png');
                $("#success-alert").alert();
                $("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
                    $("#success-alert").slideUp(500);
                });
            }
            else {
                alerta("Erro", "Erro na Impressão: " + result, 'error');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

            var mensagemErro;

            if (jqXHR.status === 0) {
                mensagemErro = 'Serviço local de impressão não encontrado.';
            } else if (jqXHR.status == 404) {
                mensagemErro = "Página não encontrada.";
            } else {
                mensagemErro = "Erro indeterminado. Contate o administrador do sistema.";
            }

            alerta("Erro", "Erro na Impressão: " + mensagemErro, 'error');
        }
    });
}

function executeRequestReImpressaoVisitante(config) {

    $.ajax({
        url: config.url,
        type: 'POST',
        crossDomain: true,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ base64StringPDF: config.data }),

        success: function (result) {
            console.log(result);
        },
        error: function (jqXHR, textStatus, errorThrown) {

            var mensagemErro;

            if (jqXHR.status === 0) {
                mensagemErro = 'Serviço local de impressão não encontrado.';
            } else if (jqXHR.status == 404) {
                mensagemErro = "Página não encontrada.";
            } else {
                mensagemErro = "Erro indeterminado. Contate o administrador do sistema.";
            }

            alerta("Erro", "Erro na Impressão: " + mensagemErro, 'erro.png');
        }
    });
}

function executeRequestSemModal(config) {

    if (config.json) {
        config.contentType = "application/json; charset=utf-8";
        config.dataType = "json";

        config.data = JSON.stringify(config.data);
    };



    $.ajax({
        url: config.url,
        data: config.data,
        async: false,
        cache: false,
        contentType: config.contentType,
        dataType: config.dataType,
        method: 'POST',
        success: function (result) {
            if (result.Success) {

                config.success.call(this, result.Data);
            }
            else {

                alerta("Erro", result.Message, 'erro.png');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            dialog.modal('hide')
            alerta("Erro", "Houve algum erro no sistema. Contate o administrador.", 'erro.png');
        }
    });
}
//adiciona a linha em uma table
function addLinhaTable(idTable, obj, redraw, buttons) {

    var nomeTabela = idTable;

    if ($.type(idTable) === 'string') {
        idTable = $(idTable).dataTable();
    }

    if (redraw)
        idTable.fnClearTable();

    if (!jQuery.isArray(obj))
        obj = [obj];

    if (obj.length > 0) {

        var rows = [];

        for (var i = 0; i < obj.length; i++) {

            var row = [];

            if (buttons) {

                var drpDown = '<div class="dropdown">  <button class="btn btn-primary btn-xs" data-toggle="dropdown">Opções <span class="caret"></span> </button>';

                drpDown += '<ul class="dropdown-menu list-inline" style="min-width: 200px; padding:5px 0" aria-labelledby="dLabel">'

                drpDown += "<li><button class='btn btn-primary btn-xs' data-toggle='tooltip' data-placement='top' title='Alterar' onclick='detalhar(" + obj[i].Codigo + ")'><i class='icon-edit'></i> Editar</button></li>";

                if (!jQuery.isArray(buttons)) {
                    buttons = [buttons]
                }

                for (var j = 0; j < buttons.length; j++) {
                    var button = buttons[j],
                        btClass = button.btClass || "btn btn-primary",
                        fn = button.fn,
                        tooltip = button.tooltip;

                    if (tooltip)
                        tooltip = String.format("data-toggle='tooltip' data-placement='top' title='{0}'", tooltip);

                    fn = String.format("{0}({1})", fn, obj[i][button.paramProperty]);

                    drpDown += String.format("<li> <button class='{0} btn-xs' {1} onclick={2}> {3} </button></li>", btClass, tooltip, fn, button.text);
                }

                drpDown += '</ul></div>'

                row.push(drpDown);

            } else


                if (buttons != null && nomeTabela != "#tblClasseLinha") {
                    row.push("<button class='btn btn-primary btn-xs' data-toggle='tooltip' data-placement='top' title='Alterar' onclick='detalhar(" + obj[i].Codigo + ")'><i class='icon-edit'></i> Editar</button>");
                }


            if (nomeTabela == "#tblSegUsuario") {
                if (obj[i].Status == true) {

                    row.push("<a href='javascript: void(0);'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.SegUsuario.ObterDados(" + obj[i].Codigo + ")' ><i class='fa fa-pencil'></i> Alterar</a>  <a href='javascript: void(0);'  class='btn btn-danger  btn-xs' onclick=SapiensJS.SegUsuario.Inativar('" + obj[i].Login + "'); ><i class='fa fa-times'></i> Inativar</a>");

                } else {

                    row.push("<a href='javascript: void(0);'  class='btn btn-success  btn-xs' onclick=SapiensJS.SegUsuario.Ativar('" + obj[i].Login + "'); ><i class='fa fa-times'></i> Ativar</a>");

                }

            }

            if (nomeTabela == "#tblSegArea") {
                if (obj[i].Status == true) {

                    row.push("<a href='/SegArea/SegAreaPermissoesIndex?id=" + obj[i].Codigo + "'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.SegArea.ObterPermissoes(" + obj[i].Codigo + ")' ><i class='fa fa-lock'></i> Permissões</a>   <a href='/SegArea/SegAreaUsuariosIndex?id=" + obj[i].Codigo + "'  class='btn btn-secondary btn-xs'  ><i class='fa fa-group'></i> Usuários</a>   <a href='javascript: void(0);'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.SegArea.ObterDados(" + obj[i].Codigo + ")' ><i class='fa fa-pencil'></i> Alterar</a>  <a href='javascript: void(0);'  class='btn btn-danger  btn-xs' onclick=SapiensJS.SegArea.Inativar(" + obj[i].Codigo + "); ><i class='fa fa-times'></i> Inativar</a>");

                } else {

                    row.push("<a href='javascript: void(0);'  class='btn btn-success  btn-xs' onclick=SapiensJS.SegArea.Ativar(" + obj[i].Codigo + "); ><i class='fa fa-times'></i> Ativar</a>");

                }

            }



            if (nomeTabela == "#tblSegAreaPermissao") {
                if (obj[i].Status == true) {

                    row.push(" <a href='javascript: void(0);'  class='btn btn-danger  btn-xs' onclick=SapiensJS.SegAreaPermissao.InativarPermissao(" + obj[i].Codigo + "); ><i class='fa fa-times'></i> Inativar</a>");

                } else {

                    row.push(" <a href='javascript: void(0);'  class='btn btn-success  btn-xs' onclick=SapiensJS.SegAreaPermissao.AtivarPermissao(" + obj[i].Codigo + "); ><i class='fa fa-times'></i> Ativar</a>");

                }

            }


            if (nomeTabela == "#tblSegAreaUsuarios") {
                if (obj[i].Status == true) {

                    row.push("<a href='javascript: void(0);'  class='btn btn-danger  btn-xs' onclick=SapiensJS.SegAreaUsuarios.InativarUsuario(" + obj[i].Codigo + "); ><i class='fa fa-times'></i> Inativar</a>");

                } else {

                    row.push(" <a href='javascript: void(0);'  class='btn btn-success  btn-xs' onclick=SapiensJS.SegAreaUsuarios.AtivarUsuario(" + obj[i].Codigo + "); ><i class='fa fa-times'></i> Ativar</a>");

                }

            }

            if (nomeTabela == "#tblSGPProjeto") {

                row.push("<a href='javascript: void(0);'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.SGPProjeto.GerenciarProjeto(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].Codigo) + ")' ><i class='fa fa-pencil'></i> Gerenciar</a> ");

            }

            if (nomeTabela == "#tblProjetosDashboardGerenciados") {

                row.push("<a href='javascript: void(0);'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.SGPDashboard.GerenciarProjeto(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].Codigo) + ")' ><i class='fa fa-pencil'></i> Gerenciar</a> ");

            }

            if (nomeTabela == "#tblSGPProjetoPresidente") {


                strBotaoTarefa = " <div class='btn-group'> ";
                strBotaoTarefa = strBotaoTarefa + " <button type='button' class='btn   btn-xs  btn-secondary dropdown-toggle' data-toggle='dropdown'>";
                strBotaoTarefa = strBotaoTarefa + "       Opções ... <span class='caret'></span>";
                strBotaoTarefa = strBotaoTarefa + "     </button>";
                strBotaoTarefa = strBotaoTarefa + "     <ul class='dropdown-menu' role='menu'>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPPresidente.ObterQuestionamentosProjeto(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].Codigo) + ")' >Questionamentos do Projeto</a></li>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPPresidente.ObterTarefas(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].Codigo) + ")' >Tarefas</a></li>";
                strBotaoTarefa = strBotaoTarefa + "     </ul>";
                strBotaoTarefa = strBotaoTarefa + "   </div>";
                row.push(strBotaoTarefa);

            }


            if (nomeTabela == "#tblSGPTarefasPendentesAprovacao") {

                row.push("<a href='javascript: void(0);'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.SGPAprovacoes.DetalhesSolicitacaoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' ><i class='fa fa-ellipsis-h'></i> Visualizar</a> ");

                // row.push("<a href='javascript: void(0);'  class='btn btn-success btn-xs'  onclick='SapiensJS.SGPAprovacoes.ConcluirTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ", 1)'  ><i class='fa fa-check'></i> </a>  <a href='javascript: void(0);'  class='btn btn-danger  btn-xs' onclick='SapiensJS.SGPAprovacoes.ConcluirTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ", 2);' ><i class='fa fa-times'></i> </a>");
            }


            if (nomeTabela == "#tblSGPProjetoTarefas") {

                strBotaoTarefa = "<div class='btn-group'> ";
                strBotaoTarefa = strBotaoTarefa + " <button type='button' class='btn  btn-sm  btn-secondary dropdown-toggle' data-toggle='dropdown'>";
                strBotaoTarefa = strBotaoTarefa + "       Ações <span class='caret'></span>";
                strBotaoTarefa = strBotaoTarefa + "     </button>";
                strBotaoTarefa = strBotaoTarefa + "     <ul class='dropdown-menu' role='menu'>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='../SGPSubTarefas?id=" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + "&idProjeto=" + $("#codigo").val() + "'>Subtarefas</a></li>";

                if (obj[i].Status != "Concluído" && obj[i].Status != "Concluído - Aguardando Aprovação") {

                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPProjeto.IncluirEvolucao(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Registrar evolução</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPProjeto.NovoPrazoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Solicitar alteração de prazo</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li class='divider'></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPProjeto.QuestionamentoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Registrar Questionamento</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPProjeto.ConcluirTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Concluir tarefa?</a></li>";
                }

                strBotaoTarefa = strBotaoTarefa + "     </ul>";
                strBotaoTarefa = strBotaoTarefa + "   </div>";

                strBotaoTarefa = strBotaoTarefa + " <div class='btn-group'> ";
                strBotaoTarefa = strBotaoTarefa + " <button type='button' class='btn  btn-sm  btn-warning dropdown-toggle' data-toggle='dropdown'>";
                strBotaoTarefa = strBotaoTarefa + "       ... <span class='caret'></span>";
                strBotaoTarefa = strBotaoTarefa + "     </button>";
                strBotaoTarefa = strBotaoTarefa + "     <ul class='dropdown-menu' role='menu'>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPProjeto.ObterAnexosTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Lista de Anexos</a></li>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPProjeto.ObterQuestionamentosTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Questionamentos</a></li>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPProjeto.ObterEvolucaoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Evolução</a></li>";
                strBotaoTarefa = strBotaoTarefa + "     </ul>";
                strBotaoTarefa = strBotaoTarefa + "   </div>";

                row.push(strBotaoTarefa);

            }


            if (nomeTabela == "#tblSGPProjetoTarefasGerenciadas") {

                strBotaoTarefa = "<div class='btn-group'> ";
                strBotaoTarefa = strBotaoTarefa + " <button type='button' class='btn  btn-sm  btn-secondary dropdown-toggle' data-toggle='dropdown'>";
                strBotaoTarefa = strBotaoTarefa + "       Ações <span class='caret'></span>";
                strBotaoTarefa = strBotaoTarefa + "     </button>";
                strBotaoTarefa = strBotaoTarefa + "     <ul class='dropdown-menu' role='menu'>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='../SGPSubTarefas?id=" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + "&idProjeto=" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codProjeto + "&origem=tg") + "'>Subtarefas</a></li>";

                if (obj[i].Status != "Concluído" && obj[i].Status != "Concluído - Aguardando Aprovação") {

                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPTarefa.IncluirEvolucao(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Registrar evolução</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPTarefa.NovoPrazoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Solicitar alteração de prazo</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li class='divider'></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPTarefa.QuestionamentoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Registrar Questionamento</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPTarefa.ConcluirTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Concluir tarefa?</a></li>";
                }

                strBotaoTarefa = strBotaoTarefa + "     </ul>";
                strBotaoTarefa = strBotaoTarefa + "   </div>";

                strBotaoTarefa = strBotaoTarefa + " <div class='btn-group'> ";
                strBotaoTarefa = strBotaoTarefa + " <button type='button' class='btn  btn-sm  btn-warning dropdown-toggle' data-toggle='dropdown'>";
                strBotaoTarefa = strBotaoTarefa + "       ... <span class='caret'></span>";
                strBotaoTarefa = strBotaoTarefa + "     </button>";
                strBotaoTarefa = strBotaoTarefa + "     <ul class='dropdown-menu' role='menu'>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPTarefa.ObterAnexosTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Lista de Anexos</a></li>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPTarefa.ObterQuestionamentosTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Questionamentos</a></li>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPTarefa.ObterEvolucaoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Evolução</a></li>";
                strBotaoTarefa = strBotaoTarefa + "     </ul>";
                strBotaoTarefa = strBotaoTarefa + "   </div>";

                row.push(strBotaoTarefa);

            }


            if (nomeTabela == "#tblSGPProjetoSubTarefas") {

                if (obj[i].Status != "Concluído") {
                    strBotaoTarefa = "<div class='btn-group'> ";
                    strBotaoTarefa = strBotaoTarefa + " <button type='button' class='btn  btn-sm  btn-secondary dropdown-toggle' data-toggle='dropdown'>";
                    strBotaoTarefa = strBotaoTarefa + "       Ações <span class='caret'></span>";
                    strBotaoTarefa = strBotaoTarefa + "     </button>";
                    strBotaoTarefa = strBotaoTarefa + "     <ul class='dropdown-menu' role='menu'>";

                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPSubTarefa.ObterAnexosTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Lista de Anexos</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPSubTarefa.ObterEvolucaoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Evolução</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li class='divider'></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPSubTarefa.IncluirEvolucao(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Registrar evolução</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPSubTarefa.NovoPrazoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Solicitar alteração de prazo</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li class='divider'></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPSubTarefa.ConcluirTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Concluir subtarefa?</a></li>";



                    strBotaoTarefa = strBotaoTarefa + "     </ul>";
                    strBotaoTarefa = strBotaoTarefa + "   </div>";
                } else {
                    strBotaoTarefa = "<div class='btn-group'> ";
                    strBotaoTarefa = strBotaoTarefa + " <button type='button' class='btn  btn-sm  btn-secondary dropdown-toggle' data-toggle='dropdown'>";
                    strBotaoTarefa = strBotaoTarefa + "       Detalhes <span class='caret'></span>";
                    strBotaoTarefa = strBotaoTarefa + "     </button>";
                    strBotaoTarefa = strBotaoTarefa + "     <ul class='dropdown-menu' role='menu'>";

                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPSubTarefa.ObterAnexosTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Lista de Anexos</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPSubTarefa.ObterEvolucaoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")' >Evolução</a></li>";

                    strBotaoTarefa = strBotaoTarefa + "     </ul>";
                    strBotaoTarefa = strBotaoTarefa + "   </div>";
                }

                row.push(strBotaoTarefa);

            }

            if (nomeTabela == "#tblSGPProjetoSubTarefasAtribuidas") {

                strBotaoTarefa = "<div class='btn-group'> ";
                strBotaoTarefa = strBotaoTarefa + " <button type='button' class='btn  btn-sm  btn-secondary dropdown-toggle' data-toggle='dropdown'>";
                strBotaoTarefa = strBotaoTarefa + "       Ações <span class='caret'></span>";
                strBotaoTarefa = strBotaoTarefa + "     </button>";
                strBotaoTarefa = strBotaoTarefa + "     <ul class='dropdown-menu' role='menu'>";

                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPSubTarefaAtribuidas.ObterAnexosTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codSubTarefa) + ")' >Lista de Anexos</a></li>";
                strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript: void(0);' onclick='SapiensJS.SGPSubTarefaAtribuidas.ObterEvolucaoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codSubTarefa) + ")' >Consultar Evolução</a></li>";

                if (obj[i].Status != "Concluído") {
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPSubTarefaAtribuidas.IncluirEvolucao(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codSubTarefa) + ", " + SapiensJS.Util.PreparaCodigoParametro(obj[i].codTarefa) + ")'  >Registrar evolução</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPSubTarefaAtribuidas.NovoPrazoTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codSubTarefa) + ")'  >Solicitar alteração de prazo</a></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li class='divider'></li>";
                    strBotaoTarefa = strBotaoTarefa + "       <li><a href='javascript:;' onclick='SapiensJS.SGPSubTarefaAtribuidas.ConcluirTarefa(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].codSubTarefa) + ")'  >Concluir subtarefa?</a></li>";
                }

                strBotaoTarefa = strBotaoTarefa + "     </ul>";
                strBotaoTarefa = strBotaoTarefa + "   </div>";

                row.push(strBotaoTarefa);

            }

            if (nomeTabela == "#tblSGPAProvacoesPendentes" || nomeTabela == "#tblSGPAProvacoesPendentesTarefas" || nomeTabela == "#tblSGPAProvacoesPendentesSubTarefas") {
                row.push("<a href='javascript: void(0);'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.SGPAprovacoes.DetalhesSolicitacao(" + obj[i].CodSolicitacao + ")' ><i class='fa fa-ellipsis-h'></i> Visualizar</a> ");
            }

            if (nomeTabela == "#tblSGPProjetoQuestionamentos" || nomeTabela == "#tblSGPProjetoTarefasQuestionamentos") {
                row.push("<a href='javascript: void(0);'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.SGPProjeto.VisualizarResposta(" + SapiensJS.Util.PreparaCodigoParametro(obj[i].CodQuestionamento) + ")' ><i class='fa fa-ellipsis-h'></i> Visualizar</a> ");
            }

            if (nomeTabela == "#tblModeloDocumento") {
                if (obj[i].Status == "Ativo") {
                    row.push("<a href='javascript: void(0);'  class='btn btn-info btn-xs'   onclick='SapiensJS.ModeloDocumento.VisualizarDocumento(" + obj[i].Codigo + ")' ><i class='fa fa-search'></i> Visualizar Documento</a> <a href='javascript: void(0);'  class='btn btn-secondary btn-xs'   onclick='SapiensJS.ModeloDocumento.ObterDados(" + obj[i].Codigo + ")' ><i class='fa fa-pencil'></i> Alterar</a>  <a href='javascript: void(0);'  class='btn btn-danger  btn-xs' onclick=SapiensJS.ModeloDocumento.Inativar('" + obj[i].Codigo + "'); ><i class='fa fa-times'></i> Inativar</a>");
                } else {
                    row.push("<a href='javascript: void(0);'  class='btn btn-info btn-xs'   onclick='SapiensJS.ModeloDocumento.VisualizarDocumento(" + obj[i].Codigo + ")' ><i class='fa fa-search'></i> Visualizar Documento</a> <a href='javascript: void(0);'  class='btn btn-success  btn-xs' onclick=SapiensJS.ModeloDocumento.Ativar('" + obj[i].Codigo + "'); ><i class='fa fa-times'></i> Ativar</a>");
                }
            }

            if (nomeTabela == "#tblCamposModelo") {
                row.push("<a href='javascript: void(0);'  class='btn btn-info btn-sm'   onclick='SapiensJS.ModeloDocumento.AlterarCampos(" + obj[i].Codigo + ")' >Alterar Campo</a>");
            }

            //************* FIM QSMS ****************/

            for (var k in obj[i]) {
                var o = obj[i];

                if ((o[k] == true || o[k] == false) && (nomeTabela != "#tblSGPProjetoQuestionamentos")) {

                    o[k] = SapiensJS.Util.RetornarStatusPadrao(o[k]);

                }

                row.push(o[k]);
            }

            rows.push(row);
        }
    }

    idTable.fnAddData(rows);
}

function addLinhaTableNew(idTable, obj, redraw, buttons, boleanos) {

    var nomeTabela = idTable;

    if ($.type(idTable) === 'string') {
        idTable = $(idTable).dataTable();
    }

    if (redraw)
        idTable.fnClearTable();

    if (!jQuery.isArray(obj))
        obj = [obj];

    var rows = [];

    for (var i = 0; i < obj.length; i++) {

        var row = [];
        var buttonsConfirmados = [];
        if (buttons) {
            for (var x = 0; x < buttons.length; x++) {
                if (obj[i].Ativo == 1 && (buttons[x][0] == "Alterar")) {
                    buttonsConfirmados.push(buttons[x][1].replace("CODIGOPARAM", obj[i].Codigo));
                }
                else if (obj[i].Ativo == 1 && (buttons[x][0] == "Inativar")) {
                    buttonsConfirmados.push(buttons[x][1].replace("CODIGOPARAM", obj[i].Codigo));
                }
                else if (obj[i].Ativo == 0 && (buttons[x][0] == "Ativar")) {
                    buttonsConfirmados.push(buttons[x][1].replace("CODIGOPARAM", obj[i].Codigo));
                }
                else if ((buttons[x][0] != "Alterar") && (buttons[x][0] != "Inativar") && (buttons[x][0] != "Ativar")) {
                    buttonsConfirmados.push(buttons[x][1].replace("CODIGOPARAM", obj[i].Codigo));
                }
            }

            var stringButton = "";
            for (var sb = 0; sb < buttonsConfirmados.length; sb++) {
                stringButton = stringButton + "&nbsp;" + buttonsConfirmados[sb];
            }

            row.push(stringButton);
        }

        for (var k in obj[i]) {
            var o = obj[i];

            for (var b = 0; b < boleanos.length; b++) {
                if (k == boleanos[b]) {
                    o[k] = SapiensJS.Util.RetornarStatusPadrao(o[k]);
                }
            }

            row.push(o[k]);
        }

        rows.push(row);
    }

    idTable.fnAddData(rows);
}

//adiciona a linha em uma table
function addLinha(idTable, obj, redraw, buttons) {

    if (idTable instanceof String)
        idTable = $(idTable).dataTable();

    if (redraw)
        //        $(idTable + " tbody tr").remove();
        idTable.fnClearTable();

    if (!jQuery.isArray(obj))
        obj = [obj];

    var rows = [];

    for (var i = 0; i < obj.length; i++) {
        var row = [];

        if (buttons) {

            var drpDown = '<div class="dropdown">  <button class="btn btn-primary btn-xs" data-toggle="dropdown">Opções <span class="caret"></span> </button>';

            drpDown += '<ul class="dropdown-menu list-inline" style="min-width: 200px; padding:5px 0" aria-labelledby="dLabel">';

            if (!jQuery.isArray(buttons)) {
                buttons = [buttons]
            }

            for (var j = 0; j < buttons.length; j++) {
                var button = buttons[j],
                    btClass = button.btClass || "btn btn-primary",
                    fn = button.fn,
                    tooltip = button.tooltip;

                if (tooltip)
                    tooltip = String.format("data-toggle='tooltip' data-placement='top' title='{0}'", tooltip);

                fn = String.format("{0}({1})", fn, obj[i][button.paramProperty]);

                drpDown += String.format("<li> <button class='{0} btn-xs' {1} onclick={2}> {3} </button></li>", btClass, tooltip, fn, button.text);
            }

            drpDown += '</ul></div>'

            row.push(drpDown);

        }

        for (var k in obj[i]) {
            var o = obj[i];


            row.push(o[k]);
        }

        rows.push(row);
    }

    idTable.fnAddData(rows);


}

function addLinhaTableWithoutButtons(idTable, obj, redraw) {

    if ($.type(idTable) === 'string') {
        idTable = $(idTable).dataTable();
    }

    if (redraw)
        idTable.fnClearTable();

    if (!jQuery.isArray(obj))
        obj = [obj];

    var rows = [];

    for (var i = 0; i < obj.length; i++) {

        var row = [];

        for (var k in obj[i]) {
            var o = obj[i];
            row.push(o[k]);
        }

        rows.push(row);
    }

    idTable.fnAddData(rows);
}

function addLinhaTableBotaoFinal(idTable, obj, redraw, buttons) {

    var nomeTabela = idTable;

    if ($.type(idTable) === 'string') {
        idTable = $(idTable).dataTable();

    }

    if (redraw)
        idTable.fnClearTable();

    if (!jQuery.isArray(obj))
        obj = [obj];

    var rows = [];

    for (var i = 0; i < obj.length; i++) {

        var row = [];

        for (var k in obj[i]) {
            var o = obj[i];
            row.push(o[k]);
        }

        if (nomeTabela == "#tblCategoria") {

            row.push(" <a href='/Categoria/DadosIndex?codigo=" + obj[i].Codigo + "' class='btn btn-success btn-xs'><i class='fa fa-plus'></i>Criar Subcategoria</a> <button type='button' class='btn btn-info btn-xs' onclick='SapiensJS.Categoria.AbrirModalAlterar();'>Alterar</button> <button type='button' class='btn btn-primary btn-xs' onclick='SapiensJS.Categoria.AtivarInativar();'>Ativar/Inativar</button>");


        }
        rows.push(row);
    }

    idTable.fnAddData(rows);
}

function calculaDias(date1, date2) {
    //formato do brasil 'pt-br'
    moment.locale('pt-br');
    //setando data1
    var data1 = moment(date1, 'DD/MM/YYYY');
    //setando data2
    var data2 = moment(date2, 'DD/MM/YYYY');
    //tirando a diferenca da data2 - data1 em dias
    var diff = data2.diff(data1, 'days');

    return diff;
}

function addLinhaTableNew(idTable, obj, redraw, buttons) {

    var nomeTabela = idTable;

    if ($.type(idTable) === 'string') {
        idTable = $(idTable).dataTable();
    }

    if (redraw)
        idTable.fnClearTable();

    if (!jQuery.isArray(obj))
        obj = [obj];

    var rows = [];

    for (var i = 0; i < obj.length; i++) {

        var row = [];
        var buttonsConfirmados = [];
        if (buttons) {
            for (var x = 0; x < buttons.length; x++) {
                if (obj[i].Ativo == 1 && (buttons[x][0] == "Alterar")) {
                    buttonsConfirmados.push(buttons[x][1].replace("CODIGOPARAM", obj[i].Codigo));
                }
                else if (obj[i].Ativo == 1 && (buttons[x][0] == "Inativar")) {
                    buttonsConfirmados.push(buttons[x][1].replace("CODIGOPARAM", obj[i].Codigo));
                }
                else if (obj[i].Ativo == 0 && (buttons[x][0] == "Ativar")) {
                    buttonsConfirmados.push(buttons[x][1].replace("CODIGOPARAM", obj[i].Codigo));
                }
                else if ((buttons[x][0] != "Alterar") && (buttons[x][0] != "Inativar") && (buttons[x][0] != "Ativar")) {
                    buttonsConfirmados.push(buttons[x][1].replace("CODIGOPARAM", obj[i].Codigo));
                }
            }

            var stringButton = "";
            for (var sb = 0; sb < buttonsConfirmados.length; sb++) {
                stringButton = stringButton + "&nbsp;" + buttonsConfirmados[sb];
            }

            row.push(stringButton);
        }

        for (var k in obj[i]) {
            var o = obj[i];

            if (k == 'Ativo') {
                o[k] = SapiensJS.Util.RetornarStatusPadrao(o[k]);
            }

            row.push(o[k]);
        }

        rows.push(row);
    }

    idTable.fnAddData(rows);
}

function validaDat(campo, valor) {
    var date = valor;
    var ardt = new Array;
    retorno = "V";
    var ExpReg = new RegExp("(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}");
    ardt = date.split("/");
    erro = false;
    if (date.search(ExpReg) == -1) {
        erro = true;
    }
    else if (((ardt[1] == 4) || (ardt[1] == 6) || (ardt[1] == 9) || (ardt[1] == 11)) && (ardt[0] > 30))
        erro = true;
    else if (ardt[1] == 2) {
        if ((ardt[0] > 28) && ((ardt[2] % 4) != 0))
            erro = true;
        if ((ardt[0] > 29) && ((ardt[2] % 4) == 0))
            erro = true;
    }
    if (erro) {
        retorno = "F";
    }
    return retorno;
}

function mvalor(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito  
    v = v.replace(/(\d)(\d{8})$/, "$1.$2"); //coloca o ponto dos milhões  
    v = v.replace(/(\d)(\d{5})$/, "$1.$2"); //coloca o ponto dos milhares  

    v = v.replace(/(\d)(\d{2})$/, "$1,$2"); //coloca a virgula antes dos 2 últimos dígitos  
    return v;
}

function Mascara(o, f) {
    v_obj = o
    v_fun = f
    setTimeout("execmascara()", 1)
}

/*Função que Executa os objetos*/
function execmascara() {
    v_obj.value = v_fun(v_obj.value)
}

function somenteNumeroDecimal(objTextBox, e) {
    var sep = 0;
    var key = '';
    var i = j = 0;
    var len = len2 = 0;
    var strCheck = '0123456789';
    var aux = aux2 = '';
    var whichCode = null;

    if (e.which) {
        whichCode = e.which;
    } else {
        whichCode = e.keyCode;
    }
    if ((whichCode == 13) || (whichCode == 0) || (whichCode == 8)) return true;
    key = String.fromCharCode(whichCode); // Valor para o código da Chave
    if (strCheck.indexOf(key) == -1) return false; // Chave inválida
    len = objTextBox.value.length;
    for (i = 0; i < len; i++)
        if ((objTextBox.value.charAt(i) != '0') && (objTextBox.value.charAt(i) != ",")) break;
    aux = '';
    for (; i < len; i++)
        if (strCheck.indexOf(objTextBox.value.charAt(i)) != -1) aux += objTextBox.value.charAt(i);
    aux += key;
    len = aux.length;
    if (len == 0) objTextBox.value = '';
    if (len == 1) objTextBox.value = '0' + "," + '0' + aux;
    if (len == 2) objTextBox.value = '0' + "," + aux;
    if (len > 2 && len < 13) {
        aux2 = '';
        for (j = 0, i = len - 3; i >= 0; i--) {
            if (j == 3) {
                aux2 += ".";
                j = 0;
            }
            aux2 += aux.charAt(i);
            j++;
        }
        objTextBox.value = '';
        len2 = aux2.length;
        for (i = len2 - 1; i >= 0; i--)
            objTextBox.value += aux2.charAt(i);
        objTextBox.value += "," + aux.substr(len - 2, len);
    }
    return false;
}

function upperText() {
    // Para tratar o colar
    $(".maiusculo").bind('paste', function (e) {
        var el = $(this);
        setTimeout(function () {
            var text = $(el).val();
            el.val(text.toUpperCase());
        }, 100);
    });

    // Para tratar quando é digitado
    $(".maiusculo").keypress(function () {
        var el = $(this);
        setTimeout(function () {
            var text = $(el).val();
            el.val(text.toUpperCase());
        }, 100);
    });
}

function SelecionarRadio(seletor, valorParaSelecao) {
    var jqObjeto = null;
    $(seletor).each(function () {

        jqObjeto = $(this);

        jqObjeto.removeAttr("checked");
        if (jqObjeto.val() == valorParaSelecao) {
            jqObjeto.attr("checked", "checked");
        }

    });
}

function decimalAdjust(type, value, exp) {
    // If the exp is undefined or zero...
    if (typeof exp === 'undefined' || +exp === 0) {
        return Math[type](value);
    }
    value = +value;
    exp = +exp;
    // If the value is not a number or the exp is not an integer...
    if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
        return NaN;
    }
    // Shift
    value = value.toString().split('e');
    value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
    // Shift back
    value = value.toString().split('e');
    return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
}



function SelecionarCombo(seletor, valorParaSelecao) {
    $(seletor).val(valorParaSelecao);
}

function getParamUrl(name, url) {
    if (!url) url = location.href
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(url);
    return results == null ? null : results[1];
}

function RecalculateCharsRemaining(editor) {
    var maxLength = parseInt(editor.maxLength ? editor.maxLength : editor.GetInputElement().maxLength);
    var editValue = editor.GetValue();
    var valueLength = editValue != null ? editValue.toString().length : 0;
    var charsRemaining = maxLength - valueLength;
    SetCharsRemainingValue(editor, charsRemaining >= 0 ? charsRemaining : 0);
}
function SetCharsRemainingValue(textEditor, charsRemaining) {
    var associatedLabel = ASPxClientControl.GetControlCollection().Get(textEditor.name + "_cr");
    var color = GetLabelColor(charsRemaining).toString();
    associatedLabel.SetText("<span style='color: " + color + ";'>" + charsRemaining.toString() + "</span>");
}
function GetLabelColor(charsRemaining) {
    if (charsRemaining < 10) return "red";
    if (charsRemaining < 100) return "#F3A250";
    return "green";
}

// ASPxMemo - MaxLength emulation
function InitMemoMaxLength(memo, maxLength) {
    memo.maxLength = maxLength;
}
function EnableMaxLengthMemoTimer(memo) {
    memo.maxLengthTimerID = window.setInterval(function () {
        var text = memo.GetText();
        if (text.length > memo.maxLength) {
            memo.SetText(text.substr(0, memo.maxLength));
            RecalculateCharsRemaining(memo);
        }
    }, 50);
}
function DisableMaxLengthMemoTimer(memo) {
    if (memo.maxLengthTimerID) {
        window.clearInterval(memo.maxLengthTimerID);
        delete memo.maxLengthTimerID;
    }
}

function addTabMenu(s, e) {
    if (e.item.name != "")
        addTabControl(e.item.name);
}

function buttonAddTabMenu(s, e) {
    addTabControl(s.name);
}

function addTabControl(tabName) {
    LeftPane.Collapse(true);
    var tab = pageControl.GetTabByName(tabName);
    if (tab != null) {
        tab.SetVisible(true);
        window["SistemaWeb"][tabName]["LimparTela"](null); // succeeds
        pageControl.SetActiveTab(tab);
    }
}

function hideTabMenu(tabName) {
    pageControl.GetTabByName(tabName).SetVisible(false);
    window["SistemaWeb"][tabName]["LimparTela"](null); // succeeds
}

var pass = "";
function mascaraSenha(s, e) {
    //Eventos
    //Enter 113
    //TAB 9
    var theEvent = e.htmlEvent || window.event;
    var tecla = theEvent.keyCode || theEvent.which;
    var char = tecla.key;

}

function moeda2(a, e, r, t) {
    let n = ""
        , h = j = 0
        , u = tamanho2 = 0
        , l = ajd2 = ""
        , o = window.Event ? t.which : t.keyCode;
    if (13 == o || 8 == o)
        return !0;
    if (n = String.fromCharCode(o),
        -1 == "0123456789".indexOf(n))
        return !1;
    for (u = a.value.length,
        h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++)
        ;
    for (l = ""; h < u; h++)
        -1 != "0123456789".indexOf(a.value.charAt(h)) && (l += a.value.charAt(h));
    if (l += n,
        0 == (u = l.length) && (a.value = ""),
        1 == u && (a.value = "0" + r + "0" + l),
        2 == u && (a.value = "0" + r + l),
        u > 2) {
        for (ajd2 = "",
            j = 0,
            h = u - 3; h >= 0; h--)
            3 == j && (ajd2 += e,
                j = 0),
                ajd2 += l.charAt(h),
                j++;
        for (a.value = "",
            tamanho2 = ajd2.length,
            h = tamanho2 - 1; h >= 0; h--)
            a.value += ajd2.charAt(h);
        a.value += r + l.substr(u - 2, u)
    }
    return !1
}