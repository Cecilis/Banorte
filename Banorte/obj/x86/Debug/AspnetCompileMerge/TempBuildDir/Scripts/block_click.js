if (document.layers) {
    //Captura el evento MouseDown
    document.captureEvents(Event.MOUSEDOWN);

    //Desabilita el handler del evento OnMouseDown 
    $(document).mousedown(function () {
        return false;
    });
}
else {
    //Desabilita el handler del evento OnMouseUp
    $(document).mouseup(function (e) {
        if (e != null && e.type == "mouseup") {
            //Verifica el boton del ratón que fue utilizado
            if (e.which == 2 || e.which == 3) {
                //Si el botón el el del medio o el derecho lo desabilita
                return false;
            }
        }
    });
}

$(document).contextmenu(function () {
    return false;
});