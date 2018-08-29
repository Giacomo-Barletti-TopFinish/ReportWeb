function AddPageControls(specificSearchFunction, resultdiv, numeroElementi, start, length)
{
    
    if (numeroElementi == 0)
        return '<b>Nessun risultato trovato</b>';

    var NUMEROPAGINEVISUALIZZATE = 10;
    var paginaCorrente = start / length;

    var markup = '  <br/><div><table class="table table-condensed table-hover" style="font-size:11px">';
    markup += '<tbody>';
    markup += '<tr>';

    markup += '<td style="width:12px"><a onclick="LoadSpecificPage(' + (start - NUMEROPAGINEVISUALIZZATE * length) + ',' + specificSearchFunction + ',\'' + resultdiv + '\')"><i class="fa fa-fw  fa-angle-double-left"></i></a></td>';
    markup += '<td style="width:12px"><a onclick="LoadSpecificPage(' + (start - length) + ',' + specificSearchFunction + ',\'' + resultdiv + '\')"><i class="fa fa-fw  fa-angle-left"></i></a></td>';


    var numeroPagine = Math.floor(numeroElementi / length);
    if (numeroElementi % length != 0) numeroPagine++;
    var estremoSuperiore = Math.min(numeroPagine, Math.floor(paginaCorrente / NUMEROPAGINEVISUALIZZATE) * NUMEROPAGINEVISUALIZZATE + NUMEROPAGINEVISUALIZZATE);

    for (i = Math.floor(paginaCorrente / NUMEROPAGINEVISUALIZZATE) * NUMEROPAGINEVISUALIZZATE; i < estremoSuperiore; i++)
    {
        if (paginaCorrente == i)
            markup += '<td style="width:12px"><a style="font-weight:bold; font-size:larger" onclick="LoadSpecificPage(' + (i * length) + ',' + specificSearchFunction + ',\'' + resultdiv + '\')">' + (i + 1) + '</a></td>';
        else
            markup += '<td style="width:12px"><a onclick="LoadSpecificPage(' + (i * length) + ',' + specificSearchFunction + ',\'' + resultdiv + '\')">' + (i + 1) + '</a></td>';

    }
   
    var limiteSuperiore = (start + length > numeroElementi) ? numeroElementi : (start + length);
    markup += '<td style="width:12px"><a onclick="LoadSpecificPage(' + (start + length) + ',' + specificSearchFunction + ',\'' + resultdiv + '\',' + numeroElementi + ')"><i class="fa fa-fw  fa-angle-right"></i></a></td>';
    markup += '<td style="width:12px"><a onclick="LoadSpecificPage(' + (start + NUMEROPAGINEVISUALIZZATE * length) + ',' + specificSearchFunction + ',\'' + resultdiv + '\',' + numeroElementi + ')"><i class="fa fa-fw  fa-angle-double-right"></i></a></td>';
    markup += '<td>Risultati ' + (start + 1) + ' - ' + limiteSuperiore + ' di ' + numeroElementi + '</td>';
    markup += '</tr>';
    markup += '</tbody>';

    markup += '</table> </div>';

    return markup;

}

function LoadSpecificPage(newStart, specificSearchFunction, resultdiv, numeroElementi)
{
   
    if (newStart < 0)
        newStart = 0;
    if (newStart >= numeroElementi)
        return;
    specificSearchFunction(resultdiv, newStart);
}

