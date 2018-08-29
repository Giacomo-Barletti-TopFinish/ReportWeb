var URL_CAMBIO_SEQUENZA_SPELEMENTI;
var URL_CANCELLA_SPELEMENTI;
var URL_SALVAVALORE_SPELEMENTI;
var URL_INFOAGGIUNTIVE_SPELEMENTI;
var URL_VERIFICAFORMACALZOLERIA;
var URL_ALTRIDATI_CALZOLERIA;
var URL_SECTION_MENU;
var URL_SECTION_MENU_LEVEL2;
var URL_CONFIRM_POPUP;
var URL_CARICA_EVENTI;
var URL_SALVA_PROFILO_UTENTE;

var URL_NUOVA_RICERCA;
var URL_NUOVO_ARTICOLO;
var URL_GO_HOME;
var URL_AGGIORNAMENTI_MASSIVI;
var URL_REFRESH_PAGE;
var URL_LOGOUT;
var URL_ASYNC_OPERATION;
var URL_GEN_REPORT;
var URL_FillANAGTAGLIEByID_ANTAGCOD;
var URL_COPIA_SCHEDE_ARTICOLO;
var URL_CONFIGURAZIONE_STAMPE;

var MESSAGGIO_VALORIZZARE_UN_CAMPO = '· Almeno un campo di ricerca deve essere valorizzato';
var MESSAGGIO_DEVE_ESSERE_VALORIZZATO = ' deve essere valorizzato/a';
var MESSAGGIO_PUNTINO = '·';
var DECIMAL_SEPARETOR_SYMBOL;

function GetEvents(seasonsId, destDll, selectedValue, addAllEvents)
{
    var sessionId = $("#hfSessionID").val();
    var destDllElement = '#' + destDll;
    var procemessage = "<option value='0'> Attendere...</option>";
    $(destDllElement).html(procemessage).show();
    $.ajax({
        url: URL_CARICA_EVENTI,
        data: { SessionId: sessionId, ID_TDSTAG: seasonsId, addAllEvents: addAllEvents },
        cache: false,
        type: "POST",
        success: function (data)
        {
            var markup = '';
            for (var x = 0; x < data.length; x++)
            {
                markup += '<option value="' + data[x].Value + '">' + data[x].Text + '</option>';
            }
            $(destDllElement).html(markup).show();
            $(destDllElement).val(selectedValue);
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

function GetTaglie(tagcode, url, destDll, withDefault)
{
    var sessionId = $("#hfSessionID").val();
    var destDllElement = '#' + destDll;
    var procemessage = "<option value='0'> Attendere...</option>";
    $(destDllElement).html(procemessage).show();
    $.ajax({
        url: url,
        data: { ID_ANTAGCOD: tagcode, SessionId: sessionId },
        cache: false,
        type: "POST",
        success: function (data)
        {
            var defaultValue = data.DefaultValue;
            var markup = '';
            for (var x = 0; x < data.Items.length; x++)
            {
                markup += '<option value="' + data.Items[x].Value + '">' + data.Items[x].Text + '</option>';
            }
            $(destDllElement).html(markup).show();
            if (withDefault)
                $(destDllElement).val(defaultValue);
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

function GetStagioni(url, destDll)
{
    var destDllElement = '#' + destDll;
    var procemessage = "<option value='0'> Attendere...</option>";
    $(destDllElement).html(procemessage).show();
    $.ajax({
        url: url,
        data: {},
        cache: false,
        type: "POST",
        success: function (data)
        {
            var markup = '';
            for (var x = 0; x < data.length; x++)
            {
                markup += '<option value="' + data[x].Value + '">' + data[x].Text + '</option>';
            }
            $(destDllElement).html(markup).show();
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });

}

function ShowWaiting()
{
    $('body').css('cursor', 'wait');
    $.LoadingOverlay('show');
}

function HideWaiting()
{
    $('body').css('cursor', 'auto');
    $.LoadingOverlay("hide");
}

function NascondiPusanteMenu()
{
    $("#menuWrench").hide();
}

function EnableWaitCursorOnAjaxCall()
{
    $(document).ajaxStart(function ()
    {
        
        EnableWaitCursorOnAjaxCallCounter++;
        ShowWaiting();

    }).ajaxStop(function ()
    {
        EnableWaitCursorOnAjaxCallCounter--;
        if (EnableWaitCursorOnAjaxCallCounter == 0)
        {
            //    $(document.body).css({ 'cursor': 'default' });
            HideWaiting();
            InitScrollbar();
        }
    });
}

function FilterInput(event, t)
{
    var theEvent = event || window.event;
    var key = theEvent.key;

    var string = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890_-+* ";
    if (string.includes(key))
        return true;
    return false;
}

function FilterAlphaNumericInput(event, t)
{
    var theEvent = event || window.event;
    var key = theEvent.key;

    var string = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
    if (string.includes(key))
        return true;
    return false;
}


function FilterInteger(event, t, maxLength)
{
    var theEvent = event || window.event;
    var key = theEvent.key;

    var string = "0123456789";
    if (!string.includes(key))
        return false;

    var content = $(t).val();
    if (content.length >= maxLength)
        return false;
    return true;
}

function FilterDecimal(event, t)
{
    var theEvent = event || window.event;
    var key = theEvent.key;
    var string = "0123456789.";
    if (DECIMAL_SEPARETOR_SYMBOL == ',')
        string = "0123456789,";
    if (!string.includes(key))
        return false;

    var content = $(t).val();
    if (key == DECIMAL_SEPARETOR_SYMBOL)
    {
        if (content.includes(DECIMAL_SEPARETOR_SYMBOL))
            return false;
    }

    t.setAttribute('data-oldvaluedec', content);
    return true;
}


function onDecimalInput(sender, parteIntera, parteDecimale)
{
    var content = $(sender).val();
    var oldValue = sender.getAttribute('data-oldvaluedec');
    if (oldValue == '') return;
    if (content.includes(DECIMAL_SEPARETOR_SYMBOL))
    {
        var decimal = content.split(DECIMAL_SEPARETOR_SYMBOL)[1]
        var integer = content.split(DECIMAL_SEPARETOR_SYMBOL)[0]
        if (decimal.length > parteDecimale)
        {
            decimal = oldValue.split(DECIMAL_SEPARETOR_SYMBOL)[1];
        }

        if (integer.length > parteIntera)
        {
            integer = oldValue.split(DECIMAL_SEPARETOR_SYMBOL)[0];
        }
        var value = integer + DECIMAL_SEPARETOR_SYMBOL + decimal;
        $(sender).val(value)
    }
    else
    {
        if (content.length > parteIntera)
            $(sender).val(oldValue)
    }
    sender.setAttribute('data-oldvaluedec', $(sender).val());
}

function isNumberKey(evt, t)
{
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    {
        theEvent.returnValue = false;
        return false;
    }
    return true;
}

function FilterInputToUpper(event, t)
{

    var theEvent = event || window.event;
    var key = theEvent.key;

    var string = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890_-+* ";
    if (string.includes(key))
    {
        var charValue = (document.all) ? event.keyCode : event.which;
        if (charValue != "8" && charValue != "0" && charValue != "27")
        {
            t.value += String.fromCharCode(charValue).toUpperCase();
        }

        return false;
    }
    return false;
}

function ToUpper(sender)
{
    var content = $(sender).val().toUpperCase();
    $(sender).val(content);
}

function ResetScrollbar()
{
    var sessionId = $('#hfSessionID').val();
    var cookie = 'top' + sessionId;
    Cookies.remove(cookie);
}

function spcCollapseBox(idPulsanteCollapseBox, chiudi)
{
    var pulsante = $(idPulsanteCollapseBox);
    var box = pulsante.parents(".box").first();
    //Find the body and the footer
    var bf = box.find(".box-body, .box-footer");
    if (chiudi)
    {
        box.addClass("collapsed-box");
        $(pulsante).children(".fa-chevron-up").removeClass("fa-chevron-up").addClass("fa-chevron-down");
        bf.slideUp();
    } else
    {
        box.removeClass("collapsed-box");
        $(pulsante).children(".fa-chevron-down").removeClass("fa-chevron-down").addClass("fa-chevron-up");
        bf.slideDown();
    }
}

function ErrorPopup(messaggio)
{
    var titolo = "Errore";

    $("#divInfoAggiuntive").dialog({
        autoOpen: true,
        position: { my: "center", at: "center", of: window },
        width: 500,
        resizable: false,
        title: titolo,
        modal: true,
        closeText: '',
        open: function ()
        {
            var data = '<table><tr><td><h5><i class="fa fa-times-circle fa-2x pull-left" style="color:#3c8dbc"></i></h5></td><td><h5><label style="font-weight: normal; padding-top: 7px; padding-left: 7px;">' + messaggio + '</label></h5></td></tr></table>';
            $(this).html(data).show();
        },
        buttons:
         [{
             text: 'OK',
             click: function ()
             {
                 $(this).html('').show();
                 $(this).dialog("close");
             }
         }
         ]
    });
}

function InfoPopup(messaggio)
{
    var titolo = "Informazione";

    messaggio = messaggio.replace("\\", "\\\\")
        .replace("\r\n", "\n")
        .replace("\n", "\\n")
        .replace("\t", "\\t")
        .replace("\"", "\\\"");

    $("#divInfoAggiuntive").dialog({
        autoOpen: true,
        position: { my: "center", at: "center", of: window },
        width: 500,
        resizable: false,
        title: titolo,
        modal: true,
        closeText: '',
        open: function ()
        {
            var data = '<table><tr><td><h5><i class="fa fa-info-circle fa-2x pull-left" style="color:#3c8dbc"></i></h5></td><td><h5><label style="font-weight: normal; padding-top: 7px; padding-left: 7px;">' + messaggio + '</label></h5></td></tr></table>';
            $(this).html(data).show();
        },
        buttons:
         [{
             text: 'OK',
             click: function ()
             {
                 $(this).html('').show();
                 $(this).dialog("close");
             }
         }
         ]
    });
}

function AlertPopup(messaggio)
{
    var titolo = "Avviso";

    $("#divInfoAggiuntive").dialog({
        autoOpen: true,
        position: { my: "center", at: "center", of: window },
        width: 500,
        resizable: false,
        title: titolo,
        modal: true,
        closeText: '',
        open: function ()
        {
            var data = '<table><tr><td><h5><i class="fa fa-exclamation-circle fa-2x pull-left" style="color:#3c8dbc"></i></h5></td><td><h5><label style="font-weight: normal; padding-top: 7px; padding-left: 7px;">' + messaggio + '</label></h5></td></tr></table>';
            $(this).html(data).show();
        },
        buttons:
         [{
             text: 'OK',
             click: function ()
             {
                 $(this).html('').show();
                 $(this).dialog("close");
             }
         }
         ]
    });
}

function ConfirmPopup(messaggio, javaConfirmFunction)
{
    var titolo = "Conferma";

    $("#divInfoAggiuntive").dialog({
        autoOpen: true,
        position: { my: "center", at: "center", of: window },
        width: 500,
        resizable: false,
        title: titolo,
        modal: true,
        closeText: '',
        open: function ()
        {
            var data = '<table><tr><td><h5><i class="fa fa-question-circle fa-2x pull-left" style="color:#3c8dbc"></i></h5></td><td><h5><label style="font-weight: normal; padding-top: 7px; padding-left: 7px;">' + messaggio + '</label></h5></td></tr></table>';
            $(this).html(data).show();
        },
        buttons:
         [{
             text: 'OK',
             click: function ()
             {
                 $(this).html('').show();
                 $(this).dialog("close");
                 javaConfirmFunction();
             }
         },
        {
            text: 'Annulla',
            click: function ()
            {
                $(this).html('').show();
                $(this).dialog("close");
            }
        }
         ]
    });
}

function OptionConfirmPopup(titolo, messaggio, optionMatrix, defaultOption)
{
    // optionMatrix = string[][] option - function to be run if option is selected
    if (titolo == 'undefined') titolo = '';

    var numberOfRows = optionMatrix.length;
    if (numberOfRows <= 0) return;

    $("#divInfoAggiuntive").dialog({
        autoOpen: true,
        position: { my: "center", at: "center", of: window },
        width: 500,
        resizable: false,
        title: titolo,
        modal: true,
        closeText: '',
        open: function ()
        {
            var data = '<table><tr><td style="width: 40px"><h5><i class="fa fa-question-circle fa-2x pull-left" style="color:#3c8dbc"></i></h5></td><td><h5><label style="font-weight: normal; padding-top: 7px;">' + messaggio + '</label></h5></td></tr></table>';
            data += "<table>";
            for (i = 0; i < numberOfRows; i++)
            {
                var checkedString = "";
                if (defaultOption == i)
                    checkedString = "checked='checked'";
                var option = optionMatrix[i];
                data += "<tr>";
                data += "<td style='width: 40px'></td>";
                data += "<td>";
                data += "<h5 style='margin: 0px'><input class='spcOption' " + checkedString + " type=radio name='printType' data-func=" + option[1] + "><label style='font-weight: normal; padding-left: 10px'>" + option[0] + "</label></input></h5>";
                data += "</td>";
                data += "</tr>";
                data += "<tr style='height:5px'></tr>";
            }
            data += "</table>";

            $(this).html(data).show();
        },
        buttons:
         [{
             text: 'OK',
             click: function ()
             {
                 var options = $('.spcOption');
                 var eseguita = false;
                
                 for (i = 0; i < options.length; i++)
                 {
                     var opzione = options[i];
                     var selezione = $(opzione).prop('checked');
                     var funzione = $(opzione).attr('data-func');

                     if(selezione)
                     {
                         eseguita = true;
                         if (typeof funzione !== 'undefined' )
                         {
                             eval(funzione);
                         }
                         break;
                     }
                 }

                 if(eseguita)
                 {
                     $(this).html('').show();
                     $(this).dialog("close");
                 }

             }
         },
        {
            text: 'Annulla',
            click: function ()
            {
                $(this).html('').show();
                $(this).dialog("close");
            }
        }
         ]
    });
}


function DatiNonSalvatiSiNoCancelConfirmationPopup(OperazioneSalvataggio, AltraOperazione)
{
    if (paginaModificata)
    {
        YesNoConfirmPopup('Dati modificati, vuoi salvare?', OperazioneSalvataggio, AltraOperazione);
    }
    else
    {
        if (AltraOperazione != '')
            AltraOperazione();
    }

}

function DatiNonSalvatiPopup(Operazione)
{

    if (paginaModificata)
    {
        ConfirmPopup('Dati modificati, continuare senza salvare?', Operazione);
    }
    else
    {
        if (Operazione != '')
            Operazione();
    }
}

function DatiNonSalvatiAlert(Operazione)
{
    if (paginaModificata)
    {
        AlertPopup('Dati modificati, non è possibile continuare senza salvare.');
    }
    else
    {
        if (Operazione != '')
            Operazione();
    }
}

function EseguiNuovaRicerca()
{
    window.location.href = URL_NUOVA_RICERCA;
}

function EseguiNuovoArticolo()
{
    window.location.href = URL_NUOVO_ARTICOLO;
}

function EseguiVaiHomePage()
{
    window.location.href = URL_GO_HOME;
}

function EseguiVaiAggiornamentiMassivi()
{
    window.location.href = URL_AGGIORNAMENTI_MASSIVI;

}

function EseguiVaiCopiaSuSchedeArticolo() {
    window.location.href = URL_COPIA_SCHEDE_ARTICOLO;

}

function EseguiVaiConfigurazioneStampe() {
    
    window.location.href = URL_CONFIGURAZIONE_STAMPE;
}


function AskLogout()
{
    ConfirmPopup('Uscire da Scheda Prodotto Calzature?', EseguiLogout);
}

function EseguiLogout()
{
    window.location.href = URL_LOGOUT;
}

function RetrievePageInformation()
{
    var url = URL_REFRESH_PAGE;
    var sessionId = $("#hfSessionID").val();
    $.ajax({
        url: url,
        data: { SessionId: sessionId },
        cache: false,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        type: "GET",
        success: function (data)
        {
            if (data == '') return;

            var nPrint = data.numOfPrint;
            if (nPrint == '0')
                nPrint = '';
            var nPicture = data.numOfPicture;
            var nCopies = data.numOfCopies;
            var boolReport = data.thereAreReportToRead;
            var boolMessage = data.thereAreMessageToRead;
            if (nCopies == '') nCopies = 0;
            if (nPicture == '') nPicture = 0;

            var totAsyncOpe = parseInt(nPicture) + parseInt(nCopies);
            if (totAsyncOpe == '0')
            {
                $("#asyncOperationIndicator").removeClass('fa-spin');
                totAsyncOpe = '';
            }
            else
            {
                $("#asyncOperationIndicator").addClass('fa-spin');
            }

            $("#asyncOperationInProcess").text(totAsyncOpe);
            $("#printInProcess").text(nPrint);
           
            if(boolMessage)
                $("#asyncOperationIndicator").css({'color':'orange'})
            else
                $("#asyncOperationIndicator").css({ 'color': 'white' })

        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

function RetrieveAsyncOperations()
{
    $("#divOpAsync").html('');
    var url = URL_ASYNC_OPERATION;
    var sessionId = $("#hfSessionID").val();
    $("#divOpAsync").dialog({
        autoOpen: true,
        position: { my: "center top+50", at: "center top", of: window },
        width: 800,
        resizable: false,
        title: 'Lavori in corso',
        modal: true,
        closeText: '',
        open: function ()
        {
            $(".ui-dialog-titlebar-close").hide();
            $(this).css('maxHeight', $(window).height() - 200);
            $.ajax({
                url: url,
                data: { SessionId: sessionId },
                cache: false,
                type: "POST",
                success: function (data)
                {
                    $("#divOpAsync").html(data).show();
                },
                error: function (response)
                {
                    document.open();
                    document.write(response.responseText);
                    document.close();
                }
            });
        },
        buttons:
        [{
            text: 'Chiudi',
            click: function () {
                $(this).dialog("close");
                $(this).dialog("destroy");
                $(this).html('');
                RetrievePageInformation();
            }
        }]
    });
}

function RetrieveReportsGenerations()
{
    $("#divOpReports").html('');
    var url = URL_GEN_REPORT;
    $("#divOpReports").dialog({
        autoOpen: true,
        position: { my: "center top+50", at: "center top", of: window },
        width: 800,
        resizable: false,
        title: 'Generazione stampe',
        modal: true,
        closeText: '',
        open: function ()
        {
            $(".ui-dialog-titlebar-close").hide();
            $(this).css('maxHeight', $(window).height() - 200);
            $.ajax({
                url: url,
                data: {},
                cache: false,
                type: "POST",
                success: function (data) {
                    $("#divOpReports").html(data).show();
                },
                error: function (response) {
                    document.open();
                    document.write(response.responseText);
                    document.close();
                }
            });
        },
        buttons:
        [{
            text: 'Chiudi',
            click: function ()
            {
                $(this).dialog("close");
                $(this).dialog("destroy");
                $(this).html('');
            }
        }]
    });
}

function YesNoConfirmPopup(messaggio, YesJavaConfirmFunction, NoJavaConfirmFunction)
{
    var titolo = "Conferma";

    $("#divInfoAggiuntive").dialog({
        autoOpen: true,
        position: { my: "center", at: "center", of: window },
        width: 500,
        resizable: false,
        title: titolo,
        modal: true,
        closeText: '',
        open: function ()
        {
            var data = '<table><tr><td><h5><i class="fa fa-question-circle fa-2x pull-left" style="color:#3c8dbc"></i></h5></td><td><h5><label style="font-weight: normal; padding-top: 7px; padding-left: 7px;">' + messaggio + '</label></h5></td></tr></table>';
            $(this).html(data).show();
        },
        buttons:
         [{
             text: 'Si',
             click: function ()
             {
                 $(this).html('').show();
                 $(this).dialog("close");
                 YesJavaConfirmFunction();
             }
         },
         {
             text: 'No',
             click: function ()
             {
                 $(this).html('').show();
                 $(this).dialog("close");
                 NoJavaConfirmFunction();
             }
         },
        {
            text: 'Annulla',
            click: function ()
            {
                $(this).html('').show();
                $(this).dialog("close");
            }
        }
         ]
    });
}


function SalvaProfiloUtente(flag, valoreNumerico,valoreTesto, refreshFunction)
{
    
    var sessionId = $("#hfSessionID").val();
    var url = URL_SALVA_PROFILO_UTENTE;
    $.ajax({
        url: url,
        data: { flag: flag, valoreNumerico: valoreNumerico, valoreTesto: valoreTesto, SessionId: sessionId },
        cache: false,
        type: "POST",
        success: function (data)
        {
            if (refreshFunction != '')
                refreshFunction();
        },
        error: function (response)
        {
        }
    });
}

function HTMLDecode(testo)
{
    return $('<textarea />').html(testo).text();
}

function ImpostaTooltip()
{
    $('select').each(
           function () {
               var tooltip = $(this).find('option:selected').text()
               $(this).prop('title', tooltip);
           }
           );

    $('select').change(
        function () {
            var tooltip = $(this).find('option:selected').text()
            $(this).prop('title', tooltip);
        }
        );
}