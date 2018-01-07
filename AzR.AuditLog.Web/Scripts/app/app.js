function closeModal() {
    $('#myModal').modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}
function createModal(url) {
    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            Spiner.show();
        }
    }).done(function (data) {
        $('#modelContent').html(data);
        $('#myModal').modal('show');
    }).fail(function (data) {
        bootbox.alert('Error in creating records');
    });
}

function render(tmplName, tmplData) {
    if (!render.tmpl_cache) {
        render.tmpl_cache = {};
    }

    if (!render.tmpl_cache[tmplName]) {
        var tmplDir = '/Templates';
        var tmplUrl = tmplDir + '/' + tmplName + '.html';

        var tmplString;
        $.ajax({
            url: tmplUrl,
            method: 'GET',
            dataType: 'html', //** Must add 
            async: false,
            success: function (data) {
                tmplString = data;
            }
        });
        //_.templateSettings = {
        //    interpolate: /\{\{(.+?)\}\}/g
        //};
        render.tmpl_cache[tmplName] = _.template(tmplString);
    }

    return render.tmpl_cache[tmplName](tmplData);
}