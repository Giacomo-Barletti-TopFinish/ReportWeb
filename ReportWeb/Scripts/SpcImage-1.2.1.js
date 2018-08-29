
function MakeDraggable()
{
    $(".divDraggable")
        .draggable({
            scroll: true
                , scrollSensitivity: 60
                , cursor: "move"
                , stop: function (evt, ui)
                {
                    DraggableMoveFinished(evt, ui);
                }
                , containment: "#disegno"

        }).css("position", "absolute");

    $('.divDraggable').hover(function ()
    {
        $(this).css('cursor', 'hand');
    });

    $('.divDraggable').dblclick(function ()
    {
        DeleteCurrentPoint(this);
    });
}

function MakeDraggableForAddNewItems()
{

    $(".draggableNew")
        .draggable({
            stop: function (evt, ui)
            {
                DraggableNewOnStop(this, ui);
                MakeDraggable();
                MakeDraggableForAddNewItems();
            }
            , helper: 'clone'
            , revert: false
            , appendTo: '#imgContainer'
            , cursor: 'hand'
            , containment: "#disegno"
        });
}

function DraggableNewOnStop(sender, ui)
{

    var newElement = $(sender).clone();

    $(newElement).removeClass('draggableNew');
    $(newElement).addClass('divDraggable');

    var elemVisual = $('<div></div>');
    $(elemVisual).addClass('draggableNew');
    $(elemVisual).attr('addClass', $(newElement).attr('addClass'));
    $(elemVisual).attr('pointType', $(newElement).attr('pointType'));
    $(elemVisual).attr('id-father', $(newElement).attr('id-father'));
    $(elemVisual).text($(newElement).text());
    $(newElement).parent().prepend(elemVisual);

    $('#imgContainer').append($(newElement));

    $(newElement).addClass($(newElement).attr('addClass'));

    var dataElemId = 'ip' + $(newElement).attr('pointType') + "_Add" + GetNextAddPointCount($(newElement).attr('pointType'));
    //var elemData = $("<input type='hidden' id='" + dataElemId + "' name='" + dataElemId + "' value='' />");
    //$('#imgContainer').append(elemData);

    $(newElement).attr('data-holding-element', dataElemId);

    AjustPositionOnAddedElement(newElement, ui);
}

function DraggableMoveFinished(evt, ui)
{

    try
    {
        var dataHoldingElement = document.getElementById($(ui.helper.context).attr('data-holding-element'));
        var fatherId = $(ui.helper.context).attr('id-father');
        if (ui.position.left >= 0 && ui.position.left <= 15 && ui.position.top >= 0 && ui.position.top <= 15)
        {
            DeleteCurrentPoint($(ui.helper));
        }

        var data = ui.position.left + ';' + ui.position.top + (fatherId != null && fatherId != '' ? ';' + fatherId : '');
        if (dataHoldingElement != null)
            dataHoldingElement.setAttribute('value', data);
    }
    catch (Err)
    { }
}

function DeleteCurrentPoint(elem)
{

    var dataHoldingElement = document.getElementById($(elem).attr('data-holding-element'));
    if (dataHoldingElement != null)
        dataHoldingElement.parentElement.removeChild(dataHoldingElement);

    try
    {
        $(elem).draggable('disable');
        $(elem).remove();
    }
    catch (Err) { }
}

function AjustPositionOnAddedElement(elem, ui)
{

    var x = (ui.offset.left - $('#imgContainer').offset().left) + $('#imgContainer').scrollLeft();
    var y = (ui.offset.top - $('#imgContainer').offset().top) + $('#imgContainer').scrollTop();
    $(elem).css('left', x);
    $(elem).css('top', y);

    var dataHoldingElement = document.getElementById($(elem).attr('data-holding-element'));
    var fatherId = $(elem).attr('id-father');
    var data = x + ';' + y + (fatherId != null && fatherId != '' ? ';' + fatherId : '');
    if (dataHoldingElement != null)
        dataHoldingElement.setAttribute('value', data);
}

function GetNextAddPointCount(pointType)
{

    var counter = 0;
    var matchId = 'ip' + pointType + '_Add';
    $("input[id^='" + matchId + "']").each(function ()
    {
        counter++;
    });
    return counter;
}
/* Funzioni Client per la gestione dei punti sull'immagine*/
function setDetails(as_obj, as_ip, type)
{
    strx = as_obj.x
    stry = as_obj.y;
    obx = getId("ip" + type + "X_" + as_ip);
    oby = getId("ip" + type + "Y_" + as_ip);
    obx.value = strx;
    oby.value = stry;

}
