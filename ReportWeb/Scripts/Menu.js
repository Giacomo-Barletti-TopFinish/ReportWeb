var LEFT_MENU_CLOSED_KEY = 'spcleftmenu'
function spcToggleNav(sender)
{
    var divContainer = $(sender).parent();
    $(divContainer).toggleClass('Open');

    if ($(divContainer).hasClass('Open'))
    {
        spcOpenMenu(divContainer);
        sessionStorage.setItem(LEFT_MENU_CLOSED_KEY, 'false');
    }
    else
    {
        spcCloseMenu(divContainer);
        sessionStorage.setItem(LEFT_MENU_CLOSED_KEY, 'true');
    }

}

function onStartLeftMenu(containerStr)
{
    var divContainer = $(containerStr);
    if (sessionStorage.getItem(LEFT_MENU_CLOSED_KEY) == null)
    {
        spcOpenMenu(divContainer);
        return;
    }

    if (sessionStorage.getItem(LEFT_MENU_CLOSED_KEY) == 'false')
    {
        spcOpenMenu(divContainer);
    }
    else
    {
        spcCloseMenu(divContainer);
    }
}

function spcOpenMenu(divContainer)
{

    var icon = $(divContainer).find('.fa-lg');
    $(icon).removeClass('fa-unlock');
    $(icon).addClass('fa-lock');

    $(divContainer).width("200px");
    $('.spcBody').css('margin-left', '200px');
    var spans = $(divContainer).find('span');
    for (i = 0; i < spans.length; i++)
    {
        $(spans[i]).show();
    }
}

function spcCloseMenu(divContainer)
{
    var icon = $(divContainer).find('.fa-lg');
    $(icon).addClass('fa-unlock');
    $(icon).removeClass('fa-lock');

    var spans = $(divContainer).find('span');
    for (i = 0; i < spans.length; i++)
    {
        $(spans[i]).hide();
    }
    $(divContainer).width("40px");
    $('.spcBody').css('margin-left', '50px');
    var submenuUl = $(divContainer).find('.sub-menu');
    for (i = 0; i < submenuUl.length; i++)
    {
        $(submenuUl[i]).collapse('hide');
    }
}

function CreateSectionMenu(sezione, div)
{
    var id_matCol = $("#hfID_TDMATART_X_TDCOLART").val();
    var isOnlyProt = $("#hfIsOnlyPrototipia").val();
    var sessionId = $("#hfSessionID").val();
    var url = URL_SECTION_MENU;
    var divName = '#' + div;
    $.ajax({
        url: url,
        data: { sezione: sezione, id_matCol: id_matCol, isOnlyProt: isOnlyProt, SessionId: sessionId },
        cache: false,
        type: "POST",
        success: function (data)
        {
            $(divName).html(data).show();
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}

function RedirectToAddNewElementLevel2(sender, Id_AnTipoElemSP, selectedText, currentSezione, idElementoPadre)
{
    $("#hfID_TIPOELEMENTOSelected").val(Id_AnTipoElemSP);
    $("#hfDescTIPOELEMENTOSelected").val(selectedText);
    $("#hfID_SPELEMENTOPADRE").val(idElementoPadre);

    switch (currentSezione)
    {
        case 'CALZ':
            AggiungiCalzoleriaLivello2(null, $("#hfDescTIPOELEMENTOSelected").val(), idElementoPadre);
            break;
        default:
            alert('non implementato');
    }
}

function CreateSectionMenu2Level(sender, sezione, codiceElementoPadre, idElementoPadre, descrizioneElementoPadre)
{
    if ($(sender).attr("data-flg-menucaricato") == 'true')
    {
        return;
    }

    //$('.spcmenubtn').attr("data-flg-menucaricato", "false");
    $(sender).attr("data-flg-menucaricato", "true");

    var parent = $(sender).closest('div')[0];
    var dialogDiv = parent.children[1];
    $("#hfID_TIPOELEMENTOSelected").val("");
    $("#hfID_SPELEMENTOPADRE").val(idElementoPadre);
    var currentSezioneL2 = sezione;
    currentCodiceElementoPadre = codiceElementoPadre;
    currentDescrizioneElementoPadre = descrizioneElementoPadre;
    var divName = '#' + dialogDiv.id;
    var url = URL_SECTION_MENU_LEVEL2;

    $.ajax({
        url: url,
        async: false,
        data: { sezione: sezione, codiceElementoPadre: codiceElementoPadre },
        cache: false,
        type: "POST",
        success: function (data)
        {
            $(dialogDiv).html(data).show();
        },
        error: function (response)
        {
            document.open();
            document.write(response.responseText);
            document.close();
        }
    });
}
