
var MESSAGGIO_VALORIZZARE_UN_CAMPO = '· Almeno un campo di ricerca deve essere valorizzato';
var MESSAGGIO_DEVE_ESSERE_VALORIZZATO = ' deve essere valorizzato/a';
var MESSAGGIO_PUNTINO = '·';
var DECIMAL_SEPARETOR_SYMBOL;



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


function InfoPopupWithRedirect(messaggio,newUrl) {
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
        open: function () {
            var data = '<table><tr><td><h5><i class="fa fa-info-circle fa-2x pull-left" style="color:#3c8dbc"></i></h5></td><td><h5><label style="font-weight: normal; padding-top: 7px; padding-left: 7px;">' + messaggio + '</label></h5></td></tr></table>';
            $(this).html(data).show();
        },
        buttons:
            [{
                text: 'OK',
                click: function () {
                    $(this).html('').show();
                    $(this).dialog("close");
                    window.location.href = newUrl;
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


function HTMLDecode(testo)
{
    return $('<textarea />').html(testo).text();
}

function ValidateEmail(email)
{
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test(email);
}
