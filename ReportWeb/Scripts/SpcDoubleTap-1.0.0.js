/*
    SIMULA E GESTISCE UN EVENTO DOUBLE TAP
    IL VALORE 250 INDICA IL TEMPO TRA I DUE TAP
*/

(function ($)
{
    $.fn.doubleTap = function (doubleTapCallback)
    {
       
        return this.each(function ()
        {
           
            var elm = this;
            var lastTap = 0;
            $(elm).bind('vmousedown', function (e)
            {
                var now = (new Date()).valueOf();
                var diff = (now - lastTap);
                lastTap = now;
                if (diff > 1000 && diff < 1500)
                {
                    debugger;
                    if ($.isFunction(doubleTapCallback))
                    {
                        doubleTapCallback.call(elm);
                    }
                }
            });
        });
    }
})(jQuery);
