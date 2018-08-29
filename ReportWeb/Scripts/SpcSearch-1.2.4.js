function HideDiv(sender)
{
    var obj = $(sender).parent().parent().parent();
    CleanDiv(obj);
    obj.hide();
}

function CleanDiv(obj)
{
    obj.find('input:text').val('');
    obj.find('select').val(-1).trigger('change');
}

function GetRicercaArticoliJSON()
{
    
    var ricerca = {
        'descrizione': $('#txtDescrizione').val(),
        'stato': $('#ddlStato').val(),
        'stagione': $('#ddlStagione').val(),
        'evento': $('#ddlEvento').val() == null ? -1 : $('#ddlEvento').val(),
        'fasi': $('#ddlFasi').val(),
        'faseAvanzata': $('#chkFaseAvanzata').prop('checked'),
        'codiceProvvisorio': $('#txtCodiceProvvisorio').val(),
        'codiceDefinitivo': $('#txtCodiceDefinitivo').val(),
        'lineaProvvisoria': $('#ddlLineaProvvisoria').val(),
        'lineaDefinitiva': $('#ddlLineaDefinitiva').val(),
        'tipoPersona': $('#ddlTipoPersona').val(),
        'tipoScarpa': $('#ddlTipoScarpa').val(),
        'codiceMateriale': $('#txtCodiceMateriale').val(),
        'codiceColore': $('#txtCodiceColore').val(),
        'tipoArticolo': $('#ddlTipoArticolo').val(),
        'note': $('#txtNote').val(),

        'forma': GetDatiGeneraliObj('Forma'),

        'tomaia': GetTaglioObj('Tomaia'),
        'fodera': GetTaglioObj('Fodera'),
        'camoscina': GetTaglioObj('Camoscina'),
        'solettopulizia': GetTaglioObj('SolettoPulizia'),
        'solettone': GetTaglioObj('Solettone'),
        'tallonetta': GetTaglioObj('Tallonetta'),
        'filetto': GetTaglioObj('Filetto'),
        'punta': GetTaglioObj('Punta'),
        'fasciafondo': GetTaglioObj('FasciaFondo'),
        'fasciatacco': GetTaglioObj('FasciaTacco'),
        'fasciaplatform': GetTaglioObj('FasciaPlatform'),
        'fasciacava': GetTaglioObj('FasciaCava'),
        'altromateriale': GetAltroMateriale(),
        'materiaprima': GetMateriaPrima(),
        'accessorio': GetAccessoriObj('Accessorio'),
        'partespeciale': GetParteSpeciale(),
        'filo': GetFilo(),
        'sfilo': GetSfilo(),
        'costola': GetCostola(),
        'cerniera': GetCerniera(),
        'tiralampoPelle': GetTiralampoPelle(),
        'stringa': GetStringa(),
        'mignonlaccio': GetMignonlaccio(),
        'marcaturaArticolo': getMarcatureArticolo(),
        'marcaturaSolettoPulizia': GetMarchiaturaObj('MarcaturaSolettoPulizia'),
        'marcaturaSolettoCuoio': GetMarchiaturaObj('MarcaturaSolettoCuoio'),
        'marcaturaFussbett': GetMarchiaturaObj('MarcaturaFussbett'),
        'marcaturaTallonetta': GetMarchiaturaObj('MarcaturaTallonetta'),
        'timbro': GetMarchiaturaObj('Timbro'),

        'contrafforte': GetCalzoleria('Contrafforte'),
        'soletto': GetCalzoleria('Soletto'),
        'intersuola': GetCalzoleria('Intersuola'),
        'platform': GetCalzoleria('Platform'),
        'fussbett': GetCalzoleria('Fussbett'),
        'fondo': GetCalzoleria('Fondo'),
        'suola': GetCalzoleria('Suola'),
        'tacco': GetCalzoleria('Tacco'),

        'stagioneCalz': GetCalzoleria('Stagione'),
        'eventoCalz': GetCalzoleria('Evento'),
        'prefisso': GetCalzoleria('Prefisso'),
        'parte': GetCalzoleria('Parte'),
        'descCalz': GetCalzoleria('Descrizione'),
        'colore': GetCalzoleria('Colore'),
        'um': GetCalzoleria('UM'),
        'quanita': GetCalzoleria('Quantita'),

        'scatola': GetConfezionamento('Scatola'),
        'flanella': GetConfezionamento('Flanella'),
        'velinaTrasversale': GetConfezionamento('VelinaTrasversale'),
        'velinaLongitudinale': GetConfezionamento('VelinaLongitudinale'),
        'astuccioScatola': GetConfezionamento('AstuccioScatola'),
        'cavallotto': GetConfezionamento('Cavallotto'),
        'bustaSale': GetConfezionamento('BustaSale'),
        'bustaPlastica': GetConfezionamento('BustaPlastica'),
        'fascettaStringaRicambio': GetConfezionamento('FascettaStringaRicambio'),
        'sottotaccoRicambio': GetConfezionamento('SottotaccoRicambio'),
        'bustaSottotaccoRicambio': GetConfezionamento('BustaSottotaccoRicambio'),
        'stringaRicambio': GetStringaRicambio(),
        'cartellino': GetConfezionamento('Cartellino'),
        'bollino': GetConfezionamento('Bollino'),
        'pittogramma': GetConfezionamento('Pittogramma'),

        'gt': GetGT(),

    };

    var jsonString = JSON.stringify(ricerca);
    return jsonString;
}


function GetGT()
{
    var obj = null;
    if ($('#GT_divGT').is(':visible'))
    {
        obj = {
            'tipoElementoGT': $('#ddlGTTipoElementoGT').val() == null ? -1 : $('#ddlGTTipoElementoGT').val(),
            'elementoGT': $('#ddlGTElementoGT').val() == null ? -1 : $('#ddlGTElementoGT').val()
        }
    };
    return obj;
}

function GetAltroMateriale()
{
    var obj = null;
    if ($('#Taglio_divAltroMateriale').is(':visible'))
    {
        obj = {
            'prefisso': $('#ddlAltroMateriale_Prefisso').val() == null ? -1 : $('#ddlAltroMateriale_Prefisso').val(),
            'codice': $('#txtAltroMateriale_Codice').val(),
            'codicefornitore': $('#txtAltroMateriale_CodiceDaFornitore').val(),
            'descrizione': $('#txtAltroMateriale_Descrizione').val(),
            'fornitore': $('txtAltroMateriale_Fornitore').val(),
            'posizione': $('#ddlAltroMateriale_Pozitione').val() == null ? -1 : $('#ddlAltroMateriale_Pozitione').val(),
            'um': $('#ddlAltroMateriale_UM').val() == null ? -1 : $('#ddlAltroMateriale_UM').val(),
            'altezza': $('#ddlAltroMateriale_Altezza').val(),
            'colore': $('#txtAltroMateriale_Colore').val()
        }
    };
    return obj;
}

function GetMateriaPrima()
{
    var obj = null;
    if ($('#Taglio_divMateriaPrima').is(':visible'))
    {
        obj = {
            'materiale': $('#txtMateriaPrima_Materiale').val(),
            'materialeDescrizione': $('#txtMateriaPrima_DescrizioneMateriale').val(),
            'vuoto': $('#chkMateriaPrima_Vuota').prop('checked'),
            'materiaPrima': $('#txtMateriaPrima_MateriaPrima').val(),
            'materiaPrimaDescrizione': $('#txtMateriaPrima_DescrizioneMateriaPrima').val()
        }
    };
    return obj;
}

function GetDatiGeneraliObj(partialID)
{

    var obj = null;
    if ($('#DatiGenerali_div' + partialID).is(':visible'))
    {
        obj = {
            'codiceProvvisorio': $('#txt' + partialID + '_CodiceProvvisorio').val(),
            'codiceDefinitivo': $('#txt' + partialID + '_CodiceDefinitivo').val(),
            'fornitore': $('#txt' + partialID + '_Fornitore').val(),
            'stagione': $('#ddl' + partialID + '_Stagione').val(),
            'evento': $('#ddl' + partialID + '_Evento').val() == null ? -1 : $('#ddl' + partialID + '_Evento').val()
        }
    };
    return obj;
}

function GetTaglioObj(partialID)
{

    var obj = null;
    if ($('#Taglio_div' + partialID).is(':visible'))
    {
        obj = {
            'materiale': $('#txt' + partialID + '_Materiale').val(),
            'descrizioneMateriale': $('#txt' + partialID + '_DescrizioneMateriale').val(),
            'coloreMateriale': $('#txt' + partialID + '_ColoreMateriale').val(),
            'descrizioneColoreMateriale': $('#txt' + partialID + '_DescrizioneColoreMateriale').val(),
            'stagione': $('#ddl' + partialID + '_Stagione').val(),
            'evento': $('#ddl' + partialID + '_Evento').val() == null ? -1 : $('#ddl' + partialID + '_Evento').val(),
            'materiaPrima': $('#txt' + partialID + '_MateriaPrima').val(),
            'descrizioneMateriaPrima': $('#txt' + partialID + '_DescrizioneMateriaPrima').val(),
            'coloreMateriaPrima': $('#txt' + partialID + '_ColoreMateriaPrima').val(),
            'descrizioneColoreMateriaPrima': $('#txt' + partialID + '_DescrizioneColoreMateriaPrima').val(),
            'pozitione': $('#ddl' + partialID + '_Pozitione').val(),
            'um': $('#ddl' + partialID + '_UM').val(),
            'altezza': $('#txt' + partialID + '_Altezza').val()
        }
    };
    return obj; Please
}

function GetMarchiaturaObj(partialID)
{
    var obj = null;
    if ($('#Marcature_div' + partialID).is(':visible'))
    {
        obj = {
            'codice': $('#txt' + partialID + 'Codice').val(),
            'descrizione': $('#txt' + partialID + 'Descrizione').val(),
            'colore': $('#txt' + partialID + 'Colore').val(),
            'posizione': $('#ddl' + partialID + 'Posiz').val() == null ? -1 : $('#ddl' + partialID + 'Posiz').val(),
            'fornitore': null
        }
    };
    return obj; Please
}

function GetAccessoriObj(partialID)
{

    var obj = null;
    if ($('#Accessori_div' + partialID).is(':visible'))
    {
        obj = {
            'codiceProvvisorio': $('#txt' + partialID + '_CodiceProvvisorio').val(),
            'codiceDefinitivo': $('#txt' + partialID + '_CodiceDefinitivo').val(),
            'descrizione': $('#txt' + partialID + '_Descrizione').val(),
            'finitura': $('#txt' + partialID + '_Finitura').val(),
            'descrizioneFinitura': $('#txt' + partialID + '_DescrizioneFinitura').val(),
            'stagione': $('#ddl' + partialID + '_Stagione').val(),
            'evento': $('#ddl' + partialID + '_Evento').val() == null ? -1 : $('#ddl' + partialID + '_Evento').val()
        }
    };
    return obj;
}

function SetRicercaArticoliJSON(ParametriRicerca)
{
    var ricerca = JSON.parse(ParametriRicerca);
    $('#txtDescrizione').val(ricerca.descrizione);
    $('#ddlStato').val(ricerca.stato);
    $('#ddlStagione').val(ricerca.stagione);
    $('#ddlFasi').val(ricerca.fasi).trigger('change');
    $('#chkFaseAvanzata').prop('checked', ricerca.faseAvanzata);
    $('#txtCodiceProvvisorio').val(ricerca.codiceProvvisorio);
    $('#txtCodiceDefinitivo').val(ricerca.codiceDefinitivo);
    $('#ddlLineaProvvisoria').val(ricerca.lineaProvvisoria).trigger('change');
    $('#ddlLineaDefinitiva').val(ricerca.lineaDefinitiva).trigger('change');
    $('#ddlTipoPersona').val(ricerca.tipoPersona);
    $('#ddlTipoScarpa').val(ricerca.tipoScarpa);
    $('#txtCodiceMateriale').val(ricerca.codiceMateriale);
    $('#txtCodiceColore').val(ricerca.codiceColore);
    $('#ddlTipoArticolo').val(ricerca.tipoArticolo);
    $('#txtNote').val(ricerca.note);


    GetEventsWithFoundArticle($('#ddlStagione').val(), 'ddlEvento', ricerca.evento, 0);

    PopulateDatiGeneraliDiv('Forma', ricerca.forma);
    PopulateTaglioDiv('Tomaia', ricerca.tomaia);
    PopulateTaglioDiv('Fodera', ricerca.fodera);
    PopulateTaglioDiv('Camoscina', ricerca.camoscina);
    PopulateTaglioDiv('SolettoPulizia', ricerca.solettopulizia);
    PopulateTaglioDiv('Solettone', ricerca.solettone);
    PopulateTaglioDiv('Tallonetta', ricerca.tallonetta);
    PopulateTaglioDiv('Filetto', ricerca.filetto);
    PopulateTaglioDiv('Punta', ricerca.punta);
    PopulateTaglioDiv('FasciaFondo', ricerca.fasciafondo);
    PopulateTaglioDiv('FasciaTacco', ricerca.fasciatacco);
    PopulateTaglioDiv('FasciaPlatform', ricerca.fasciaplatform);
    PopulateTaglioDiv('FasciaCava', ricerca.fasciacava);
    PopulateTaglioDiv('AltroMateriale', ricerca.altromateriale);
    PopulateMateriaPrima('', ricerca.materiaprima);
    PopulateAccessoriDiv('Accessorio', ricerca.accessorio);
    PopulateAltroMaterialeDiv('', ricerca.altromateriale);
    PopulateParteSpecialeDiv('', ricerca.partespeciale);
    PopulateFiloDiv('', ricerca.filo);
    PopulateSfiloDiv('', ricerca.sfilo);
    PopulateMarcatureArticoloDiv('', ricerca.marcaturaArticolo);
    PopulateMarchiaturaDiv('MarcaturaSolettoPulizia', ricerca.marcaturaSolettoPulizia);
    PopulateMarchiaturaDiv('MarcaturaSolettoCuoio', ricerca.marcaturaSolettoCuoio);
    PopulateMarchiaturaDiv('MarcaturaFussbett', ricerca.marcaturaFussbett);
    PopulateMarchiaturaDiv('MarcaturaTallonetta', ricerca.marcaturaTallonetta);
    PopulateMarchiaturaDiv('Timbro', ricerca.timbro);
    PopulateCostola('', ricerca.costola);
    PopulateCerniera('', ricerca.cerniera);
    PopulateTiralampoPelle('', ricerca.tiralampoPelle);
    PopulateStringa('', ricerca.stringa);
    PopulateMignonLaccio('', ricerca.mignonlaccio);
    PopulateStringaRicambioDiv('', ricerca.stringaRicambio);

    PopulateCalzoleria('Contrafforte', ricerca.contrafforte);
    PopulateCalzoleria('Soletto', ricerca.soletto);
    PopulateCalzoleria('Intersuola', ricerca.intersuola);
    PopulateCalzoleria('Platform', ricerca.platform);
    PopulateCalzoleria('Fussbett', ricerca.fussbett);
    PopulateCalzoleria('Fondo', ricerca.fondo);
    PopulateCalzoleria('Suola', ricerca.suola);
    PopulateCalzoleria('Tacco', ricerca.tacco);

    PopulateConfezionamento('Scatola', ricerca.scatola);
    PopulateConfezionamento('Flanella', ricerca.flanella);
    PopulateConfezionamento('VelinaTrasversale', ricerca.velinaTrasversale);
    PopulateConfezionamento('VelinaLongitudinale', ricerca.velinaLongitudinale);
    PopulateConfezionamento('AstuccioScatola', ricerca.astuccioScatola);
    PopulateConfezionamento('Cavallotto', ricerca.cavallotto);
    PopulateConfezionamento('BustaSale', ricerca.bustaSale);
    PopulateConfezionamento('BustaPlastica', ricerca.bustaPlastica);
    PopulateConfezionamento('FascettaStringaRicambio', ricerca.fascettaStringaRicambio);
    PopulateConfezionamento('SottotaccoRicambio', ricerca.sottotaccoRicambio);
    PopulateConfezionamento('StringaRicambio', ricerca.stringaRicambio);
    PopulateConfezionamento('Cartellino', ricerca.cartellino);
    PopulateConfezionamento('Bollino', ricerca.bollino);
    PopulateConfezionamento('Pittogramma', ricerca.pittogramma);
    PopulateConfezionamento('BustaSottotaccoRicambio', ricerca.bustaSottotaccoRicambio);

    PopulateGT('', ricerca.gt)
}

function PopulateGT(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#GT_divGT').show();

        $('#ddlGTTipoElementoGT').val(jsonObj.tipoElementoGT);
        $('#ddlGTElementoGT').val(jsonObj.elementoGT);
    }
}

function PopulateConfezionamento(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Confezionamento_div' + partialID).show();
        if (partialID == 'Cartellino')
        {
            $('#ddlConfezionamento' + partialID + 'Stagione').val(jsonObj.stagione);
            $('#ddlConfezionamento' + partialID + 'Evento').val(jsonObj.evento);
        }
        $('#txtConfezionamento' + partialID + 'Prefisso').val(jsonObj.prefisso);
        $('#txtConfezionamento' + partialID + 'Parte').val(jsonObj.parte);
        $('#txtConfezionamento' + partialID + 'Descrizione').val(jsonObj.descrizione);
        $('#txtConfezionamento' + partialID + 'Colore').val(jsonObj.colore);
        $('#ddlConfezionamento' + partialID + 'UM').val(jsonObj.um);
        $('#txtConfezionamento' + partialID + 'Quantita').val(jsonObj.quantita);
    }

}

function PopulateCalzoleria(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Calzoleria_div' + partialID).show();

        $('#ddlCalzoleria' + partialID + 'Stagione').val(jsonObj.stagione);
        $('#ddlCalzoleria' + partialID + 'Evento').val(jsonObj.evento);
        $('#ddlCalzoleria' + partialID + 'TipoPersona').val(jsonObj.tipoPersona);
        $('#ddlCalzoleria' + partialID + 'TipoScarpa').val(jsonObj.tipoScarpa);
        $('#txtCalzoleria' + partialID + 'CodiceProvvisorio').val(jsonObj.codiceProvvisorio);
        $('#txtCalzoleria' + partialID + 'CodiceDefinitivo').val(jsonObj.codiceDefinitivo);
        $('#txtCalzoleria' + partialID + 'Descrizione').val(jsonObj.descrizione);
        $('#txtCalzoleria' + partialID + 'Fornitore').val(jsonObj.fornitore);
        $('#txtCalzoleria' + partialID + 'Colore').val(jsonObj.colore);
    }

}

function PopulateMignonLaccio(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Giunteria_divMignonLaccio').show();

        $('#txtMignonlaccioCodice').val(jsonObj.codice);
        $('#txtMignonlaccioMatCol').val(jsonObj.matCol);
        $('#txtMignonlaccioDescTec').val(jsonObj.descTec);
        $('#txtMignonlaccioFornitore').val(jsonObj.fornitore);
        $('#ddlMignonlaccioUmAlt').val(jsonObj.umAlt);
        $('#txtMignonlaccioAlt').val(jsonObj.alt);
        $('#ddlMignonlaccioUmLung').val(jsonObj.umLung);
        $('#txtMignonlaccioLung').val(jsonObj.lung);
        $('#ddlMignonlaccioUM').val(jsonObj.um);
        $('#txtMignonlaccioQuantita').val(jsonObj.codice);
    }
}

function PopulateStringa(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Giunteria_divStringa').show();

        $('#txtStringaCodice').val(jsonObj.codice);
        $('#txtStringaColore').val(jsonObj.colore);
        $('#txtStringaFornitore').val(jsonObj.fornitore);
        $('#ddlStringaPosizione').val(jsonObj.posizione);
        $('#ddlStringaUmLung').val(jsonObj.umLung);
        $('#txtStringaLung').val(jsonObj.lung);
        $('#ddlStringaUM').val(jsonObj.um);
        $('#txtStringaQuantita').val(jsonObj.quantita);
    }
}

function PopulateTiralampoPelle(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Giunteria_divTiralampoPelle').show();

        $('#txtTiralampoPelleDescrizione').val(jsonObj.descrizione);
        $('#txtTiralampoPelleFornitore').val(jsonObj.fornitore);
        $('#ddlTiralampoPelleUM').val(jsonObj.um);
        $('#txtTiralampoPelleQuantita').val(jsonObj.quantita);
    }
}

function PopulateCerniera(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Giunteria_divCerniera').show();

        $('#txtCernieraCodice').val(jsonObj.codice);
        $('#txtCernieraColore').val(jsonObj.colore);
        $('#txtCernieraGrana').val(jsonObj.grana);
        $('#txtCernieraFornitore').val(jsonObj.fornitore);
        $('#ddlCernieraPosizione').val(jsonObj.posizione);
        $('#ddlCernieraUmLung').val(jsonObj.umLung);
        $('#txtCernieraLung').val(jsonObj.lung);
        $('#ddlCernieraUM').val(jsonObj.um);
        $('#txtCernieraQuantita').val(jsonObj.quantita);
    }
}

function PopulateCostola(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Giunteria_divCostola').show();
        $('#txtCostolaDescrizione').val(jsonObj.descrizione);
        $('#txtCostolaColori').val(jsonObj.desccolore);
        $('#txtCostolaFornitori').val(jsonObj.fornitore);
        $('#ddlCostolaUM').val(jsonObj.um);
        $('#txtCostolaQuantita').val(jsonObj.quantita);
    }
}

function PopulateMateriaPrima(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Taglio_divMateriaPrima').show();
        $('#txtMateriaPrima_Materiale').val(jsonObj.materiale);
        $('#txtMateriaPrima_DescrizioneMateriale').val(jsonObj.materialeDescrizione);
        if (jsonObj.vuoto == true)
            $('#chkMateriaPrima_Vuota').prop('checked', true);
        $('#txtMateriaPrima_MateriaPrima').val(jsonObj.materiaPrima);
        $('#txtMateriaPrima_DescrizioneMateriaPrima').val(jsonObj.materiaPrimaDescrizione);
    }

}

function PopulateAltroMaterialeDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Taglio_divAltroMateriale').show();
        $('#txtAltroMateriale_Codice').val(jsonObj.codice);
        $('#txtAltroMateriale_CodiceDaFornitore').val(jsonObj.codicefornitore);
        $('#txtAltroMateriale_Descrizione').val(jsonObj.descrizione);
        $('#txtAltroMateriale_Fornitore').val(jsonObj.fornitore);
        $('#ddlAltroMateriale_UM').val(jsonObj.um);
        $('#ddlAltroMateriale_Altezza').val(jsonObj.altezza);
        $('#txtAltroMateriale_Colore').val(jsonObj.colore)
    }
}
function PopulateDatiGeneraliDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('DatiGenerali_div' + partialID).show();
        $('#txt' + partialID + '_CodiceProvvisorio').val(jsonObj.codiceProvvisorio);
        $('#txt' + partialID + '_CodiceDefinitivo').val(jsonObj.codiceDefinitivo);
        $('#txt' + partialID + '_Fornitore').val(jsonObj.fornitore);
        $('#ddl' + partialID + '_Stagione').val(jsonObj.stagione);
        if ($('#ddl' + partialID + '_Stagione').val() != '')
            GetEventsWithFoundArticle($('#ddl' + partialID + '_Stagione').val(), 'ddl' + partialID + '_Evento', jsonObj.evento, 0);
    }
}

var SPCSearchAjaxCounter;

function GetEventsWithFoundArticle(seasonsId, destDll, selectedValue, addAllEvents)
{
    var sessionId = $("#hfSessionID").val();
    SPCSearchAjaxCounter++;
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

            SPCSearchAjaxCounter--;
            var markup = '';
            for (var x = 0; x < data.length; x++)
            {
                markup += '<option value="' + data[x].Value + '">' + data[x].Text + '</option>';
            }
            $(destDllElement).html(markup).show();
            $(destDllElement).val(selectedValue);

            if (SPCSearchAjaxCounter == 0)
            {

                TrovaArticoli();
                $("#btnCollassaRicerca").click();
            }
        },
        error: function (response)
        {
            SPCSearchAjaxCounter--;
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

function PopulateTaglioDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Taglio_div' + partialID).show();
        $('#txt' + partialID + '_Materiale').val(jsonObj.materiale);
        $('#txt' + partialID + '_DescrizioneMateriale').val(jsonObj.descrizioneMateriale);
        $('#txt' + partialID + '_ColoreMateriale').val(jsonObj.coloreMateriale);
        $('#txt' + partialID + '_DescrizioneColoreMateriale').val(jsonObj.descrizioneColoreMateriale);
        $('#ddl' + partialID + '_Stagione').val(jsonObj.stagione);
        if ($('#ddl' + partialID + '_Stagione').val() != '')
            GetEventsWithFoundArticle($('#ddl' + partialID + '_Stagione').val(), 'ddl' + partialID + '_Evento', jsonObj.evento, 0);
        $('#txt' + partialID + '_MateriaPrima').val(jsonObj.materiaPrima);
        $('#txt' + partialID + '_DescrizioneMateriaPrima').val(jsonObj.descrizioneMateriaPrima);
        $('#txt' + partialID + '_ColoreMateriaPrima').val(jsonObj.coloreMateriaPrima);
        $('#txt' + partialID + '_DescrizioneColoreMateriaPrima').val(jsonObj.descrizioneColoreMateriaPrima);
        $('#ddl' + partialID + '_Pozitione').val(jsonObj.pozitione);
        $('#ddl' + partialID + '_UM').val(jsonObj.um);
        $('#txt' + partialID + '_Altezza').val(jsonObj.altezza);
    }
}

function PopulateAccessoriDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Accessori_div' + partialID).show();
        $('#txt' + partialID + '_CodiceProvvisorio').val(jsonObj.codiceProvvisorio);
        $('#txt' + partialID + '_CodiceDefinitivo').val(jsonObj.codiceDefinitivo);
        $('#txt' + partialID + '_Descrizione').val(jsonObj.descrizione);
        $('#txt' + partialID + '_Finitura').val(jsonObj.finitura);
        $('#txt' + partialID + '_DescrizioneFinitura').val(jsonObj.descrizioneFinitura);
        $('#ddl' + partialID + '_Stagione').val(jsonObj.stagione);
        if ($('#ddl' + partialID + '_Stagione').val() != '')
            GetEventsWithFoundArticle($('#ddl' + partialID + '_Stagione').val(), 'ddl' + partialID + '_Evento', jsonObj.evento, 0);
    }
}

function PulisciMascheraRicercaArticoli()
{
    $('#txtDescrizione').val('');
    $('#ddlStato').val(-1);
    $('#ddlStagione').val(-1);
    $('#ddlEvento').val('');
    $('#txtCodiceProvvisorio').val('');
    $('#txtCodiceDefinitivo').val('');
    $('.select2').val('').trigger('change');
    $('#ddlTipoPersona').val('');
    $('#ddlTipoScarpa').val('');
    $('#txtCodiceMateriale').val('');
    $('#txtCodiceColore').val('');
    $('#ddlTipoArticolo').val(-1);
    $('#txtNote').val('');

    $('.spcDivRicercaEstesa').each(function ()
    {
        CleanDiv($(this));
    });

    if (!$("#sezioneRisultati").hasClass("collapsed-box"))
    {
        $("#btnCollassaRisultati").click();
    }

    $('#divRisultati').html('').show();
}

function AggiungiFiltro(descrizione, valore)
{
    return '<td width="12%"><b>' + descrizione + '</b><br /><b style="color:limegreen; font-size:14px">' + valore + '</b></td>';
}

function GetParteSpeciale()
{
    var obj = null;
    if ($('#PartiSpeciali_divParteSpeciale').is(':visible'))
    {
        obj = {
            'prefisso': $('#ddlParteSpecialePrefisso').val() == null ? -1 : $('#ddlParteSpecialePrefisso').val(),
            'codice': $('#txtParteSpecialeCodice').val(),
            'descrizione': $('#txtParteSpecialeDescrizione').val(),
            'fornitore': $('#txtParteSpecialeFornitore').val(),
            'codiceFornitore': $('#txtParteSpecialeCodForn').val(),
            'stagione': $('#ddlParteSpecialeStag').val() == null ? -1 : $('#ddlParteSpecialeStag').val(),
            'evento': $('#ddlParteSpecialeEv').val() == null ? -1 : $('#ddlParteSpecialeEv').val(),
            'descColore': $('#txtParteSpecialeColore').val(),
            'posizione': $('#ddlParteSpecialePosizione').val() == null ? -1 : $('#ddlParteSpecialePosizione').val(),
            'note': $('#txtParteSpecialeNota').val(),
            'um': $('#ddlParteSpecialeUM').val(),
            'quantita': $('#txtParteSpecialeQuantita').val(),
        }
    };
    return obj;
}

function PopulateParteSpecialeDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#PartiSpeciali_divParteSpeciale').show();
        $('#ddlParteSpecialePrefisso').val(jsonObj.prefisso);
        $('#txtParteSpecialeCodice').val(jsonObj.codice);
        $('#txtParteSpecialeDescrizione').val(jsonObj.descrizione);
        $('#txtParteSpecialeFornitore').val(jsonObj.fornitore);
        $('#txtParteSpecialeCodForn').val(jsonObj.codiceFornitore);
        $('#ddlParteSpecialeStag').val(jsonObj.stagione);
        $('#ddlParteSpecialeEv').val(jsonObj.evento);
        $('#txtParteSpecialeColore').val(jsonObj.descColore);
        $('#ddlParteSpecialePosizione').val(jsonObj.posizione);
        $('#txtParteSpecialeNota').val(jsonObj.note);
    }
}

function GetFilo()
{

    var obj = null;
    if ($('#Giunteria_divFilo').is(':visible'))
    {
        obj = {
            'titolo': $('#txtFiloTitolo').val(),
            'fornitore': $('#txtFiloFornitori').val(),
            'descrizione': $('#txtFiloDescrizione').val(),
            'coloriComp': $('#ddlFiloColoriComp').val() == null ? -1 : $('#ddlFiloColoriComp').val(),
            'passo': $('#ddlPassoFilo').val() == null ? -1 : $('#ddlPassoFilo').val(),
            'posizione': $('#ddlFiloPosizione').val() == null ? -1 : $('#ddlFiloPosizione').val(),
            'um': $('#ddlFiloUM').val() == null ? -1 : $('#ddlFiloUM').val(),
            'quantita': $('#txtFiloQuantita').val(),
        }
    };
    return obj;
}

function getMarcatureArticolo()
{
    var obj = null;
    if ($('#Marcature_divMarcaturaArticolo').is(':visible'))
    {
        obj = {
            'colore': $('#txtMarcArtColore1').val(),
            'codice': $('#txtMarcArtCodice1').val(),
            'fornitore': $('#txtMarcArtFornitore1').val(),
            'descrizione': $('#txtMarcArtDescrizione1').val(),
            'posizione': $('#ddlMarcArtPosiz').val() == null ? -1 : $('#ddlMarcArtPosiz').val()
        }
    };
    return obj;
}

function GetSfilo()
{
    var obj = null;
    if ($('#Giunteria_divSfilo').is(':visible'))
    {
        obj = {
            'descolore': $('#txtSfiloDescColore').val(),
            'fornitore': $('#txtSfiloFornitore').val(),
            'descrizione': $('#txtSfiloDescrizione').val(),
            'um': $('#ddlSfiloUM').val() == null ? -1 : $('#ddlSfiloUM').val(),
            'quantita': $('#txtSfiloQuantita').val()
        }
    };
    return obj;
}

function GetCostola()
{
    var obj = null;
    if ($('#Giunteria_divCostola').is(':visible'))
    {
        obj = {
            'descrizione': $('#txtCostolaDescrizione').val(),
            'desccolore': $('#txtCostolaColori').val(),
            'fornitore': $('#txtCostolaFornitori').val(),
            'um': $('#ddlCostolaUM').val() == null ? -1 : $('#ddlCostolaUM').val(),
            'quantita': $('#txtCostolaQuantita').val()
        }
    };
    return obj;
}

function GetCerniera()
{
    var obj = null;
    if ($('#Giunteria_divCerniera').is(':visible'))
    {
        obj = {
            'codice': $('#txtCernieraCodice').val(),
            'colore': $('#txtCernieraColore').val() == null ? -1 : $('#txtCernieraColore').val(),
            'grana': $('#txtCernieraGrana').val(),
            'fornitore': $('#txtCernieraFornitore').val(),
            'posizione': $('#ddlCernieraPosizione').val() == null ? -1 : $('#ddlCernieraPosizione').val(),
            'umLung': $('#ddlCernieraUmLung').val() == null ? -1 : $('#ddlCernieraUmLung').val(),
            'lung': $('#txtCernieraLung').val(),
            'um': $('#ddlCernieraUM').val() == null ? -1 : $('#ddlCernieraUM').val(),
            'quantita': $('#txtCernieraQuantita').val()
        }
    };
    return obj;
}

function GetTiralampoPelle()
{
    var obj = null;
    if ($('#Giunteria_divTiralampoPelle').is(':visible'))
    {
        obj = {
            'descrizione': $('#txtTiralampoPelleDescrizione').val(),
            'fornitore': $('#txtTiralampoPelleFornitore').val(),
            'um': $('#ddlTiralampoPelleUM').val() == null ? -1 : $('#ddlTiralampoPelleUM').val(),
            'quantita': $('#txtTiralampoPelleQuantita').val(),
        }
    };
    return obj;
}

function GetStringa()
{
    var obj = null;
    if ($('#Giunteria_divStringa').is(':visible'))
    {
        obj = {
            'codice': $('#txtStringaCodice').val(),
            'colore': $('#txtStringaColore').val(),
            'fornitore': $('#txtStringaFornitore').val(),
            'posizione': $('#ddlStringaPosizione').val() == null ? -1 : $('#ddlStringaPosizione').val(),
            'umLung': $('#ddlStringaUmLung').val() == null ? -1 : $('#ddlStringaUmLung').val(),
            'lung': $('#txtStringaLung').val(),
            'um': $('#ddlStringaUM').val() == null ? -1 : $('#ddlStringaUM').val(),
            'quantita': $('#txtStringaQuantita').val()
        }
    };
    return obj;
}


function GetStringaRicambio()
{
    var obj = null;
    if ($('#Confezionamento_divStringaRicambio').is(':visible'))
    {
        obj = {
            'codice': $('#txtStringaRicambioCodice').val(),
            'colore': $('#txtStringaRicambioColore').val(),
            'fornitore': $('#txtStringaRicambioFornitore').val(),
            'umLung': $('#ddlStringaRicambioUmLung').val() == null ? -1 : $('#ddlStringaRicambioUmLung').val(),
            'lung': $('#txtStringaRicambioLung').val(),
            'um': $('#ddlStringaRicambioUM').val() == null ? -1 : $('#ddlStringaRicambioUM').val(),
            'quantita': $('#txtStringaRicambioQuantita').val()
        }
    };
    return obj;
}

function GetMignonlaccio()
{
    var obj = null;
    if ($('#Giunteria_divMignonLaccio').is(':visible'))
    {
        obj = {
            'codice': $('#txtMignonlaccioCodice').val(),
            'matCol': $('#txtMignonlaccioMatCol').val(),
            'descTec': $('#txtMignonlaccioDescTec').val(),
            'fornitore': $('#txtMignonlaccioFornitore').val(),
            'umAlt': $('#ddlMignonlaccioUmAlt').val() == null ? -1 : $('#ddlMignonlaccioUmAlt').val(),
            'alt': $('#txtMignonlaccioAlt').val(),
            'umLung': $('#ddlMignonlaccioUmLung').val() == null ? -1 : $('#ddlMignonlaccioUmLung').val(),
            'lung': $('#txtMignonlaccioLung').val(),
            'um': $('#ddlMignonlaccioUM').val() == null ? -1 : $('#ddlMignonlaccioUM').val(),
            'quantita': $('#txtMignonlaccioQuantita').val(),
        }
    };
    return obj;
}

function PopulateFiloDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Giunteria_divFilo').show();
        $('#txtFiloTitolo').val(jsonObj.titolo);
        $('#txtFiloFornitori').val(jsonObj.fornitore);
        $('#txtFiloDescrizione').val(jsonObj.descrizione);
        $('#ddlFiloColoriComp').val(jsonObj.coloriComp);
        $('#ddlPassoFilo').val(jsonObj.passo);
        $('#ddlFiloPosizione').val(jsonObj.posizione);
        $('#ddlFiloUM').val(jsonObj.um);
        $('#txtFiloQuantita').val(jsonObj.quantita);
    }
}
function PopulateSfiloDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Giunteria_divSfilo').show();
        $('#txtSfiloDescColore').val(jsonObj.descolore);
        $('#txtSfiloFornitore').val(jsonObj.fornitore);
        $('#txtSfiloDescrizione').val(jsonObj.descrizione);
        $('#ddlSfiloUM').val(jsonObj.um);
        $('#txtSfiloQuantita').val(jsonObj.quantita);
    }
}
function PopulateMarcatureArticoloDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Marcature_divMarcaturaArticolo').show();
        $('#txtMarcArtCodice1').val(jsonObj.codice);
        $('#txtMarcArtFornitore1').val(jsonObj.fornitore);
        $('#txtMarcArtDescrizione1').val(jsonObj.descrizione);
        $('#ddlMarcArtPosiz').val(jsonObj.posizione);
        $('#txtMarcArtColore1').val(jsonObj.colore);
    }
}
function PopulateMarchiaturaDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Marcature_div' + partialID).show();
        $('#txt' + partialID + 'Codice').val(jsonObj.codice);
        $('#txt' + partialID + 'Descrizione').val(jsonObj.descrizione);
        $('#txt' + partialID + 'Colore').val(jsonObj.colore);
        $('#ddl' + partialID + 'Posiz').val(jsonObj.posizione);
    }
}
function PopulateCostolaDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Giunteria_divCostola').show();
        $('#txtCostolaDescrizione').val(jsonObj.descolore);
        $('#txtCostolaColori').val(jsonObj.colori);
        $('#txtCostolaFornitori').val(jsonObj.fornitori);
        $('#ddlCostolaUM').val(jsonObj.um);
        $('#txtCostolaQuantita').val(jsonObj.quantita);
    }
}
function PopulateCernieraDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {

        $('#Giunteria_divCerniera').show();
        $('#txtCernieraCodice').val(jsonObj.codice);
        $('#txtCernieraColori').val(jsonObj.colore);
        $('#txtCernieraGrana').val(jsonObj.grana);
        $('#txtCernieraFornitore').val(jsonObj.fornitore);
        $('#ddlCernieraPosizione').val(jsonObj.posizione);
        $('#ddlCernieraUmLung').val(jsonObj.umLung);
        $('#txtCernieraLung').val(jsonObj.lung);
        $('#ddlCernieraUM').val(jsonObj.um);
        $('#txtCernieraQuantita').val(jsonObj.quantita);
    }
}
function PopulateTiralampoPelleDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {

        $('#Giunteria_divTiralampoPelle').show();
        $('#txtTiralampoPelleDescrizione').val(jsonObj.descrizione);
        $('#txtTiralampoPelleFornitore').val(jsonObj.fornitore);
        $('#ddlTiralampoPelleUM').val(jsonObj.um);
        $('#txtTiralampoPelleQuantita').val(jsonObj.quantita);
    }
}
function PopulateStringaDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {

        $('#Giunteria_divStringa').show();
        $('#txtStringaCodice').val(jsonObj.codice);
        $('#txtStringaColore').val(jsonObj.colore);
        $('#txtStringaFornitore').val(jsonObj.fornitore);
        $('#ddlStringaPosizione').val(jsonObj.posizione);
        $('#ddlStringaUmLung').val(jsonObj.umLung);
        $('#txtStringaLung').val(jsonObj.lung);
        $('#ddlStringaUM').val(jsonObj.um);
        $('#txtStringaQuantita').val(jsonObj.quantita);
    }
}

function PopulateStringaRicambioDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {

        $('#Confezionamento_divStringaRicambio').show();
        $('#txtStringaRicambioCodice').val(jsonObj.codice);
        $('#txtStringaRicambioColore').val(jsonObj.colore);
        $('#txtStringaRicambioFornitore').val(jsonObj.fornitore);
        $('#ddlStringaRicambioUmLung').val(jsonObj.umLung);
        $('#txtStringaRicambioLung').val(jsonObj.lung);
        $('#ddlStringaRicambioUM').val(jsonObj.um);
        $('#txtStringaRicambioQuantita').val(jsonObj.quantita);
    }
}

function PopulateMignonlaccioDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {

        $('#Giunteria_divMignonLaccio').show();
        $('#txtMignonlaccioCodice').val(jsonObj.codice);
        $('#txtMignonlaccioMatCol').val(jsonObj.matCol);
        $('#txtMignonlaccioDescTec').val(jsonObj.descTec);
        $('#txtMignonlaccioFornitore').val(jsonObj.fornitore);
        $('#ddlMignonlaccioUmAlt').val(jsonObj.umAlt);
        $('#txtMignonlaccioAlt').val(jsonObj.alt);
        $('#ddlMignonlaccioUmLung').val(jsonObj.umLung);
        $('#txtMignonlaccioLung').val(jsonObj.lung);
        $('#ddlMignonlaccioUM').val(jsonObj.um);
        $('#txtMignonlaccioQuantita').val(jsonObj.quantita);
    }
}

function GetCalzoleria(partialID)
{    
    var obj = null;
    if ($('#Calzoleria_div' + partialID).is(':visible'))
    {
        obj = {
            'stagione': $('#ddlCalzoleria' + partialID + 'Stagione').val() == null ? -1 : $('#ddlCalzoleria' + partialID + 'Stagione').val(),
            'evento': $('#ddlCalzoleria' + partialID + 'Evento').val() == null ? -1 : $('#ddlCalzoleria' + partialID + 'Evento').val(),
            'tipoPersona': $('#ddlCalzoleria' + partialID + 'TipoPersona').val() == null ? -1 : $('#ddlCalzoleria' + partialID + 'TipoPersona').val(),
            'tipoScarpa': $('#ddlCalzoleria' + partialID + 'TipoScarpa').val() == null ? -1 : $('#ddlCalzoleria' + partialID + 'TipoScarpa').val(),
            'codiceProvvisorio': $('#txtCalzoleria' + partialID + 'CodiceProvvisorio').val(),
            'codiceDefinitivo': $('#txtCalzoleria' + partialID + 'CodiceDefinitivo').val(),
            'descrizione': $('#txtCalzoleria' + partialID + 'Descrizione').val(),
            'fornitore': $('#txtCalzoleria' + partialID + 'Fornitore').val(),
            'colore': $('#txtCalzoleria' + partialID + 'Colore').val()
        }
    };
    return obj; Please
}

function GetConfezionamento(partialID)
{
    var obj = null;
    if ($('#Confezionamento_div' + partialID).is(':visible'))
    {
        obj = {
            'stagione': $('#ddlConfezionamento' + partialID + 'Stagione').val() == null ? -1 : $('#ddlCalzoleria' + partialID + 'Stagione').val(),
            'evento': $('#ddlConfezionamento' + partialID + 'Evento').val() == null ? -1 : $('#ddlCalzoleria' + partialID + 'Evento').val(),
            'prefisso': $('#txtConfezionamento' + partialID + 'Prefisso').val(),
            'parte': $('#txtConfezionamento' + partialID + 'Parte').val(),
            'descrizione': $('#txtConfezionamento' + partialID + 'Descrizione').val(),
            'colore': $('#txtConfezionamento' + partialID + 'Colore').val(),
            'um': $('#ddlConfezionamento' + partialID + 'UM').val() == null ? -1 : $('#ddlConfezionamento' + partialID + 'UM').val(),
            'quantita': $('#txtConfezionamento' + partialID + 'Quantita').val(),
        }
    };
    return obj; Please
}

function PopulateConfezionamentoDiv(partialID, jsonObj)
{
    if (jsonObj != null)
    {
        $('#Confezionamento_div' + partialID).show();
        $('#ddlConfezionamento' + partialID + 'Stagione').val(jsonObj.stagione);
        $('#txtConfezionamento' + partialID + 'Evento').val(jsonObj.evento);
        $('#txtConfezionamento' + partialID + 'Prefisso').val(jsonObj.prefisso);
        $('#txtConfezionamento' + partialID + 'Parte').val(jsonObj.parte);
        $('#txtConfezionamento' + partialID + 'Descrizione').val(jsonObj.descrizione);
        $('#txtConfezionamento' + partialID + 'Colore').val(jsonObj.colore);
        $('#txtConfezionamento' + partialID + 'Descrizione').val(jsonObj.descrizione);
        $('#ddlConfezionamento' + partialID + 'UM').val(jsonObj.um);
        $('#txtConfezionamento' + partialID + 'Quantita').val(jsonObj.quanita);
    }
}
