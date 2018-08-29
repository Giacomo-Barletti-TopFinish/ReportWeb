
function ManageCheckAll()
{
    var checkAll = true;
    if ($('#chkDatiGenerali').prop("checked")
        && $('#chkTaglio').prop("checked")
        && $('#chkAccessori').prop("checked")
        && $('#chkPSpeciali').prop("checked")
        && $('#chkGiunteria').prop("checked")
        && $('#chkCalzoleria').prop("checked")
        && $('#chkConfezionamento').prop("checked")
        && $('#chkMarcature').prop("checked")
        && $('#chkFinissaggi').prop("checked")
        && $('#chkGT').prop("checked"))
    {
        checkAll = true;
    }
    else
    {
        checkAll = false;
    }

    $('#chkSelTutti').prop('checked', checkAll);
}


function verifyTomaiaExist()
{
    var taglioTable = $('#tblTaglio');
    if (taglioTable == null) return false;

    var tr1 = $('#tblTaglio').find('tr')[1]
    if (tr1 == null) return false;
    var tipoElemento = $($(tr1).children()[0]).find('label').text();
    if (tipoElemento == 'Tomaia') return true;

    tipoElemento = $($(tr1).children()[0]).find('a').text();
    if (tipoElemento == 'Tomaia') return true;

    return false;
}

function salvaScheda()
{
    ShowWaiting();
    executeSubmit();
}

function SubmitForm(sender)
{
    $(sender).prop('disabled', true);
    salvaScheda()
}

function executeSubmit()
{
    var validStagEvens = getStagEvenValues();
    $('#hfStagEvenJSON').val(JSON.stringify(validStagEvens));
    var tipperTippersecTipscas = getTipperTippersecTipscaValues();
    $('#hfTipperTippersecTipscaJSON').val(JSON.stringify(tipperTippersecTipscas));
    var calzFase = getSpCalzValues();
    $('#hfPaginaModifica').val(paginaModificata);
    $('#hfSelectedSPCALZsJSON').val(JSON.stringify(calzFase));
    $.LoadingOverlay("hide");
    $("#ArticleForm").submit();
}

function GetStagioneEventoRiferimento(soloValidita)
{
    if (!soloValidita)
    {
        var valoreProfilo = $("#ddlRightSE").val();
        if (valoreProfilo != '-1#-1#-1')
        {
            var arrayOfStrings = valoreProfilo.split('#');
            return arrayOfStrings[1] + '#' + arrayOfStrings[2];
        }
    }

    var validStagEvens = getStagEvenValues();
    if (validStagEvens.length == 0)
    {
        var stagEven = $('#hfStagEvenJSON').val();
        if (stagEven == '')
        {
            var ideven = $("#hd_ID_TDEVENCREATE").val();
            var idstag = $("#hd_ID_TDSTAGCREATE").val();
            return idstag + '#' + ideven;
        }

        validStagEvens = JSON.parse($('#hfStagEvenJSON').val());
        if (validStagEvens.length == 0)
        {
            var ideven = $("#hd_ID_TDEVENCREATE").val();
            var idstag = $("#hd_ID_TDSTAGCREATE").val();
            return idstag + '#' + ideven;
        }

    }
    var selectedStag = 0;
    var selectedEven = 0;
    for (i = 0; i < validStagEvens.length; i++)
    {
        if (selectedStag < validStagEvens[i].Stag)
        {
            selectedStag = validStagEvens[i].Stag
            selectedEven = validStagEvens[i].Even
        }
    }

    return selectedStag + '#' + selectedEven;
}

function ExternalTDMatArt(lblMATERIALECOD, lblMATERIALEDESC, lblCOLORECOD, lblCOLOREDESC, idMatartXColart, idmatart, idcolart)
{
    paginaModificata = true;
    matColModificato = 1;
    $("#hfID_TDMATART_X_TDCOLART").val(idMatartXColart);
    $("#hfID_TDMATART").val(idmatart);
    $("#hfID_TDCOLART").val(idcolart);
}

function ExternalOnSelectSchCmpForma(ID_SCFORM, descrizione)
{
    paginaModificata = true;
    $("#hd_ID_SCFORM").val(ID_SCFORM);
}

function getStagEvenValues()
{
    var data = [];
    var trs = $('#validStagEvenTable >tbody >tr');
    for (i = 0; i < trs.length; i++)
    {
        var tr = trs[i];
        var stag = $(".hdValidStag", tr).val();
        var even = $(".hdValidEven", tr).val();

        var tdStag = $(tr).find('td')[0];
        var stagDesc = $(tdStag).text().trim();

        var tdEven = $(tr).find('td')[1];
        var evenDesc = $(tdEven).text().trim();

        var prot = $(tr).find('td')[2];
        var protChecked = $($(prot).children()[0]).prop('checked');

        var camp = $(tr).find('td')[3];
        var campChecked = $($(camp).children()[0]).prop('checked');

        var prod = $(tr).find('td')[4];
        var prodChecked = $($(prod).children()[0]).prop('checked');

        var stagEven = { 'Stag': stag, 'Even': even, 'StagDesc': stagDesc, 'EvenDesc': evenDesc, 'Prot': protChecked, 'Camp': campChecked, 'Prod': prodChecked };
        data.push(stagEven);
    }
    return data;
}

function setStagEvenValues(dataString)
{
    var data = [];
    data = JSON.parse(dataString);
    if (data.length == 0) return;

    var fontbold = 'style="font-weight:bold";';
    for (i = 0; i < data.length; i++)
    {
        var selectedEventText = data[i].EvenDesc;
        var selectedEventVal = data[i].Even;
        var selectedStagText = data[i].StagDesc;
        var selectedStagVal = data[i].Stag;
        var prot = data[i].Prot;
        var camp = data[i].Camp;
        var prod = data[i].Prod;

        if (i > 0)
            fontbold = '';

        var tr = '<tr ' + fontbold + '>';
        tr += '<input type="hidden" value = ' + selectedStagVal + ' class="hdValidStag" />';
        tr += '<input type="hidden" value = ' + selectedEventVal + ' class="hdValidEven" />';
        tr += '<td width="20%">' + selectedStagText + '</td>';
        tr += '<td width="20%">' + selectedEventText + '</td>';
        tr += prot ? '<td width="16%" align="center"><input type="checkbox" name="prot" checked onclick="paginaModificata=true;" /></td>' : '<td width="16%" align="center"><input type="checkbox" name="prot" onclick="paginaModificata=true;" /></td>';
        tr += camp ? '<td width="16%" align="center"><input type="checkbox" name="camp" checked onclick="paginaModificata=true;"/></td>' : '<td width="16%" align="center"><input type="checkbox" name="camp" onclick="paginaModificata=true;" /></td>';
        tr += prod ? '<td width="16%" align="center"><input type="checkbox" name="prod" checked onclick="paginaModificata=true;"/></td>' : '<td width="16%" align="center"><input type="checkbox" name="prod" onclick="paginaModificata=true;" /></td>';
        tr += '<td width="11%"><a class="disabilitaVersione" title="Cancella"><i class="fa fa-fw fa-remove"></i></a></td>';
        tr += '<\tr>';

        $('#validStagEvenTable').last().append(tr);

    }
}

function AggiungiStagEvenVali()
{
    paginaModificata = true;
    var selectedEventText = $("#ddlValidEvent").find("option:selected").text();
    var selectedEventVal = $("#ddlValidEvent").val();
    var selectedStagText = $("#ddlValidStag").find("option:selected").text();
    var selectedStagVal = $("#ddlValidStag").val();

    if (selectedEventVal == "") selectedEventVal = "-1";
    if (selectedEventVal == null) selectedEventVal = "-1";
    if (selectedEventVal == "") selectedEventVal = "-1";
    if (selectedStagVal == null) selectedStagVal = "-1";

    if (selectedEventVal == "-1" || selectedStagVal == "-1")
        return;

    if (!ExistsStagEven(selectedStagVal, selectedEventVal))
    {
        var tr = '<tr>';
        tr += '<input type="hidden" value = ' + selectedStagVal + ' class="hdValidStag" />';
        tr += '<input type="hidden" value = ' + selectedEventVal + ' class="hdValidEven" />';
        tr += '<td width="20%">' + selectedStagText + '</td>';
        tr += '<td width="20%">' + selectedEventText + '</td>';
        tr += '<td width="16%" align="center"><input type="checkbox" name="prot" /></td>';
        tr += '<td width="16%" align="center"><input type="checkbox" name="camp" /></td>';
        tr += '<td width="16%" align="center"><input type="checkbox" name="prod" /></td>';
        tr += '<td width="11%"><a class="disabilitaVersione" title="Cancella"><i class="fa fa-fw fa-remove"></i></a></td>';
        tr += '<\tr>';
        $('#validStagEvenTable').prepend(tr);
        //$('#validStagEvenTable').first().append(tr);
        $("#divValidStEv").scrollTop($("#divValidStEv")[0].scrollHeight);
        var elementi = JSON.stringify(getStagEvenValues());
        $('#validStagEvenTable tbody tr').remove();
        setStagEvenValues(elementi);
        LoadCalzaturifici();

        var stagioneCreazione = $('#ddlTDSTAGCREATE').val();
        var eventoCreazione = $('#ddlTDEVENCREATE').val();
        if (stagioneCreazione == -1 && eventoCreazione == -1)
        {
            $('#ddlTDSTAGCREATE').val(selectedStagVal);
            GetEvents(selectedStagVal, "ddlTDEVENCREATE", selectedEventVal, 0);
        }
    }
}

function ExistsStagEven(selectedStagVal, selectedEventVal)
{
    var data = [];
    data = getStagEvenValues();
    for (i = 0; i < data.length; i++)
    {
        var stagCode = data[i].Stag;
        var evenCode = data[i].Even;

        if (stagCode == selectedStagVal && evenCode == selectedEventVal)
            return true;
    }
    return false;
}

function setTipperTippersecTipscaValues(dataString)
{
    var data = [];
    data = JSON.parse(dataString);
    if (data.length == 0) return;

    var fontbold = 'style="font-weight:bold";';
    for (i = 0; i < data.length; i++)
    {
        var tipperCode = data[i].tipper;
        var tippersecCode = data[i].tippersec;
        var tipscaCode = data[i].tipsca;

        $("#ddlTipPer").val(tipperCode);
        var selectedTipperText = $("#ddlTipPer").find("option:selected").text();

        var selectedTipperSecText = '';
        $("#ddlTipSca").val(tipscaCode);
        var selectedTipScaText = $("#ddlTipSca").find("option:selected").text();


        if (i > 0)
            fontbold = '';

        $('#tipperTable').last().append('<tr ' + fontbold + '><input type="hidden" value = ' + tipperCode + ' class="hdTipper" /><input type="hidden" value = ' + tippersecCode + ' class="hdTipperSec" /><input type="hidden" value = ' + tipscaCode + ' class="hdTipSca" /><td width="45%">' + selectedTipperText + '</td><td width="45%">' + selectedTipScaText + '</td><td width="10%"><a class="disabilitaVersione" title="Cancella"><i class="fa fa-fw fa-remove"></i></a></td></tr>');

    }
    $("#ddlTipPer").val(-1);
    $("#ddlTipSca").val(-1);
}

function getTipperTippersecTipscaValues()
{
    var data = [];
    var trs = $('#tipperTable >tbody >tr');
    for (i = 0; i < trs.length; i++)
    {
        var tr = trs[i];
        var tipper = $(".hdTipper", tr).val();
        var tippersec = $(".hdTipperSec", tr).val();
        tippersec = -1;
        var tipsca = $(".hdTipSca", tr).val();
        var selectedTipper = { 'tipper': tipper, 'tippersec': tippersec, 'tipsca': tipsca };
        data.push(selectedTipper);
    }
    return data;
}

function AggiungiTipper()
{
    
    paginaModificata = true;
    var selectedTipperText = $("#ddlTipPer").find("option:selected").text();
    var selectedTipperVal = $("#ddlTipPer").val();
    var selectedTipperSecText = $("#ddlTipperSec").find("option:selected").text();
    var selectedTipperSecVal = $("#ddlTipperSec").val();
    var selectedTipScaText = $("#ddlTipSca").find("option:selected").text();
    var selectedTipScaVal = $("#ddlTipSca").val();

    if (selectedTipperVal == null) selectedTipperVal = -1;
    if (selectedTipperVal == "") selectedTipperVal = -1;
    if (selectedTipScaVal == null) selectedTipScaVal = -1;
    if (selectedTipScaVal == "") selectedTipScaVal = -1;

    if (selectedTipperSecVal == null) selectedTipperSecVal = -1;
    if (selectedTipperSecVal == "") selectedTipperSecVal = -1;

    if (selectedTipperVal == "-1" || selectedTipScaVal == "-1")
        return;

    if (!ExistsTipperTippersecTipsca(selectedTipperVal, selectedTipperSecVal, selectedTipScaVal))
    {
        var codiceAnagTagCod = '21';
        var taglia = '37';
        if (selectedTipperVal == 'D' && selectedTipScaVal == 'E')
        {
            codiceAnagTagCod = '21';
            taglia = '37';
        }
        if (selectedTipperVal == 'D' && selectedTipScaVal == 'S')
        {
            codiceAnagTagCod = '28';
            taglia = '37';
        }
        if (selectedTipperVal == 'U' && selectedTipScaVal == 'E')
        {
            codiceAnagTagCod = '29';
            taglia = '9';
        }
        if (selectedTipperVal == 'U' && selectedTipScaVal == 'S')
        {
            codiceAnagTagCod = '29';
            taglia = '9';
        }

        var idAnagCodTag = $('#ddlTagCod').val();

        var trs = $('#tipperTable >tbody >tr');
        if (trs.length == 0 && idAnagCodTag == '-1')
        {
            var options = $('#ddlTagCod').find('option');
            for (i = 0; i < options.length; i++)
            {
                if (options[i].text.length > 2)
                {
                    var codice = options[i].text.substring(0, 2);
                    if(codice==codiceAnagTagCod)
                    {
                        var idCodTag = options[i].value;
                        $('#ddlTagCod').val(idCodTag);
                        var url = URL_FillANAGTAGLIEByID_ANTAGCOD;
                        GetTaglie(idCodTag, url, "ddlTaglie", true);
                    }
                }
            }
        }

        $('#tipperTable').last().append('<tr><input type="hidden" value = ' + selectedTipperVal + ' class="hdTipper" /><input type="hidden" value = ' + selectedTipperSecVal + ' class="hdTipperSec" /><input type="hidden" value = ' + selectedTipScaVal + ' class="hdTipSca" /><td width="45%">' + selectedTipperText + '</td><td width="45%">' + selectedTipScaText + '</td><td width="10%"><a class="disabilitaVersione" title="Cancella"><i class="fa fa-fw fa-remove"></i></a></td></tr>');
        LoadCalzaturifici();
    }
}

function ExistsTipperTippersecTipsca(selectedTipperVal, selectedTipperSecVal, selectedTipScaVal)
{
    var data = [];
    data = getTipperTippersecTipscaValues();
    for (i = 0; i < data.length; i++)
    {
        var tipperCode = data[i].tipper;
        var tippersecCode = data[i].tippersec;
        var tipscaCode = data[i].tipsca;

        if (tipperCode == selectedTipperVal && tippersecCode == selectedTipperSecVal && tipscaCode == selectedTipScaVal)
            return true;
    }
    return false;
}

function CheckAllRightMenu(isChecked, sender)
{
    var parent = $(sender).closest(".tab-pane");
    $(parent).find(".pull-right:checkbox").each(function ()
    {
        $(this).prop("checked", isChecked);
        $(this).change();
    });
}

function CancellaGT(sender, hiddenfieldName)
{
    paginaModificata = true;
    hiddenfieldName = '#' + hiddenfieldName;
    var hiddenField = $(hiddenfieldName).val('-1');
    var parent = $(sender).closest('tr');
    var tds = parent.find('td');
    tds[1].innerText = '';
    tds[2].innerText = '';
    tds[3].innerText = '';
    CreaEtichettaGT();
}

function CreaEtichettaGT()
{
    var etichetta = "GT (";
    var trs = $('#tblGT >tbody >tr');
    var index = trs.length;
    if (index > 12) index = 12;
    for (i = 0; i < index; i++)
    {
        var tr = trs[i];
        var tds = $(tr).find('td');
        var codice = tds[1].innerText;
        if (codice == '')
            etichetta = etichetta + '-';
        else
            etichetta = etichetta + codice;
    }
    etichetta = etichetta + ")";
    $('#lblEtichettaGT').text(etichetta);
}

function AggiungiSpCalz()
{
    paginaModificata = true;
    var selectedCalzText = $("#ddlCalz").find("option:selected").text();
    var selectedCalzVal = $("#ddlCalz").val();
    var ispettore = $("#ddlCalz").find("option:selected").attr('ispettore');
    var industrializzatore = $("#ddlCalz").find("option:selected").attr('industrializzatore');

    var selectedFaseText = $("#ddlFase").find("option:selected").text();
    var selectedFaseVal = $("#ddlFase").val();

    if (selectedCalzVal == "" || selectedCalzVal == null) selectedCalzVal = "-1";
    if (selectedFaseVal == "" || selectedFaseVal == null) selectedFaseVal = "-1";

    if (selectedCalzVal == "-1" || selectedFaseVal == "-1")
        return;

    if (!ExistsSpCalz(selectedCalzVal, selectedFaseVal, selectedFaseText))
    {
        $('#validSpCalzTable').last().append('<tr><input type="hidden" value = ' + selectedCalzVal + ' class="hdValidCalz" /> <input type="hidden" value = ' + selectedFaseVal + ' class="hdValidFase" /> <td width="23%">' + selectedCalzText + '</td><td width="20%">' + selectedFaseText + '</td><td width="23%">' + industrializzatore + '</td><td width="23%">' + ispettore + '</td><td width="11%"><a class="disabilitaVersione" title="Cancella"><i class="fa fa-fw fa-remove"></i></a></td></tr>');
        $("#divValidSpCalz").scrollTop($("#divValidSpCalz")[0].scrollHeight);
    }

}
function ExistsSpCalz(selectedCalzVal, selectedFaseVal, selectedFaseText)
{
    var data = [];
    data = getSpCalzValues();
    for (i = 0; i < data.length; i++)
    {
        var calz = data[i].Calz;
        var fase = data[i].Fase;

        if (calz == selectedCalzVal && fase == selectedFaseVal)
            return true;
        //if (fase == selectedFaseVal && selectedFaseText != "Produzione")
        //    return true;

    }
    return false;
}

function getSpCalzValues()
{
    var data = [];
    var trs = $('#validSpCalzTable >tbody >tr');
    for (i = 0; i < trs.length; i++)
    {
        var tr = trs[i];
        var calz = $(".hdValidCalz", tr).val();
        var fase = $(".hdValidFase", tr).val();

        var tdCalz = $(tr).find('td')[0];
        var calzDesc = $(tdCalz).text().trim();

        var tdFase = $(tr).find('td')[1];
        var faseDesc = $(tdFase).text().trim();

        var spCalz = { 'Calz': calz, 'Fase': fase, 'CalzDesc': calzDesc, 'FaseDesc': faseDesc, 'Industrializzatore': '', 'Ispettore': '' };
        data.push(spCalz);
    }
    return data;
}

function setSpCalzValues(dataString)
{
    var data = [];
    data = JSON.parse(dataString);
    if (data.length == 0) return;
    for (i = 0; i < data.length; i++)
    {
        var selectedCalzText = data[i].CalzDesc;
        var selectedCalzVal = data[i].Calz;
        var selectedFaseText = data[i].FaseDesc;
        var selectedFaseVal = data[i].Fase;
        var industrializzatore = data[i].Industrializzatore;
        var Ispettore = data[i].Ispettore;

        $('#validSpCalzTable').last().append('<tr><input type="hidden" value = ' + selectedCalzVal + ' class="hdValidCalz" /> <input type="hidden" value = ' + selectedFaseVal + ' class="hdValidFase" /> <td width="23%">' + selectedCalzText + '</td><td width="20%">' + selectedFaseText + '</td><td width="23%">' + industrializzatore + '</td><td width="23%">' + Ispettore + '</td><td width="11%"><a class="disabilitaVersione" title="Cancella"><i class="fa fa-fw fa-remove"></i></a></td></tr>');
    }
}

function VerificaFormaCalzoleria(ID_ANTIPOELEMSP, ID_ELEMENTO)
{
    var idsp = $("#hfIdSp").val();
    if (idsp == '') return;
    var sessionId = $("#hfSessionID").val();

    if (ID_ELEMENTO > 0)
    {
        var url = URL_VERIFICAFORMACALZOLERIA;
        $.ajax({
            url: url,
            data: { id_sp: idsp, ID_ANTIPOELEMSP: ID_ANTIPOELEMSP, ID_ELEMENTO: ID_ELEMENTO, SessionId: sessionId },
            cache: false,
            type: "POST",
            success: function (data)
            {
                var divElement = $('#htSPElemId').parent().parent().parent().parent().parent();

                if (data)
                {
                    divElement.addClass('has-warning');
                } else
                {
                    divElement.removeClass('has-warning');
                }
                return data
            },
            error: function (response)
            {
                document.open();
                document.write(response.responseText);
                document.close();
            }
        });
    }
}

function GetAltriDati(ANTIPOELEMSPCODE)
{
    var sessionId = $("#hfSessionID").val();
    var idsp = $("#hfIdSp").val();
    var idSpelem = $("#htSPElemId").val();
    var idElemento = $("#hfCalzElemId").val();

    var urlVarable = URL_ALTRIDATI_CALZOLERIA

    $.ajax({
        url: urlVarable,
        data: {
            idSpElem: idSpelem,
            idElemento: idElemento,
            id_sp: idsp,
            SessionId: sessionId,
            ANTIPOELEMSPCODE: ANTIPOELEMSPCODE
        },
        cache: false,
        type: "POST",
        success: function (data)
        {
            $("#pnlAltriDati").html(data).show();
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
    return true;
}

function CancellaSPElemento(idElemento, refreshFunction)
{
    var sessionId = $("#hfSessionID").val();
    var idsp = $("#hfIdSp").val();

    var url = URL_CANCELLA_SPELEMENTI;
    $.ajax({
        url: url,
        data: { id_sp: idsp, idSPElemento: idElemento, sessionId: sessionId },
        cache: false,
        type: "POST",
        success: function (data)
        {
            paginaModificata = true;
            if (refreshFunction != null)
            {
                var externalFuction = refreshFunction;
                externalFuction();
            }
            //$("#divTaglioContainer").html(data).show();
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

function CambioSequenzaElemento(direction, idElementoTaglio, refreshFunction, IDPADRE)
{
    var sessionId = $("#hfSessionID").val();
    var idsp = $("#hfIdSp").val();

    var url = URL_CAMBIO_SEQUENZA_SPELEMENTI;
    $.ajax({
        url: url,
        data: { id_sp: idsp, id_elemento: idElementoTaglio, SessionId: sessionId, direction: direction, idPadre: IDPADRE },
        cache: false,
        type: "POST",
        success: function (data)
        {
            paginaModificata = true;
            var externalFuction = refreshFunction;
            externalFuction();
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

function CambioSequenzaElementoLivello2(direction, idElementoTaglio, refreshFunction)
{
    var sessionId = $("#hfSessionID").val();
    var idsp = $("#hfIdSp").val();

    var url = URL_CAMBIO_SEQUENZA_SPELEMENTI;
    $.ajax({
        url: url,
        data: { id_sp: idsp, id_elementoTaglio: idElementoTaglio, sessionId: sessionId, direction: direction },
        cache: false,
        type: "POST",
        success: function (data)
        {
            paginaModificata = true;
            var externalFuction = refreshFunction;
            externalFuction();
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

function SendValueToServer(sender, idSpElem, colonna)
{
    var oldValue = sender.getAttribute('data-oldvalue');
    var actualValue = $(sender).val();
    if (oldValue == actualValue) return;
    var url = URL_SALVAVALORE_SPELEMENTI;
    var sessionId = $("#hfSessionID").val();
    var idsp = $("#hfIdSp").val();
    $.ajax({
        url: url,
        data: { id_sp: idsp, SessionId: sessionId, idSpElem: idSpElem, valore: actualValue, colonna: colonna },
        cache: false,
        type: "POST",
        success: function (data)
        {
            paginaModificata = true;
            if (!data)
                AlertPopup('Impossibile aggiornare il valore modificato');
            else
                sender.setAttribute('data-oldvalue', actualValue);
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

var TRSelezionata;
function evidenziaTR(sender)
{
    var tr = $(sender).closest('tr');
    TRSelezionata = $(tr).attr('id');
    $(tr).addClass('evidenzia');
}

function disevidenziaTR()
{
    var tr = '#' + TRSelezionata;
    $(tr).removeClass('evidenzia');
}

function disabilitaSeVersione()
{
    if ($('#hfTipoSP').val() == 'VS')
    {
        $(".disabilitaVersione").each(function ()
        {
            $(this).prop("disabled", true);
            $(this).removeAttr("href");
            $(this).removeAttr('onclick');
            $(this).removeAttr('title');
            $(this).removeClass("spcmenuitem");

            if ($(this).is("a"))
            {
                $(this).css("color", "lightgray");
                $(this).css("cursor", "not-allowed");
                $(this).removeAttr('title');
            }

            if ($(this).hasClass("select2"))
            {
                $(".select2-selection__choice").each(function ()
                {
                    $(this).removeAttr('title');
                }
                    );

                $(".select2-selection__rendered").each(function ()
                {
                    $(this).removeAttr('title');
                }
                    );

            }
        });
    }
}