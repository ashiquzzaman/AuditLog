/// <reference path="../bootbox.js" />

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
            
        }
    }).done(function (data) {
        $('#modelContent').html(data);
        $('#myModal').modal('show');
    }).fail(function (data) {
        bootbox.alert('Error in creating records');
    });
}
function SubmitOnSuccess(result) {
    if (result.redirectTo) {
        closeModal();
        bootbox(result.message);
        $('#' + result.position).load(result.redirectTo);
    } else {
        $('#modelContent').html(result);
        $('#myModal').modal('show');
    }
}
function redirectOnSuccess(result) {
    if (result.redirectTo) {
        closeModal();
        window.location.href = result.redirectTo;
    } else {
        $('#modelContent').html(result);
        $('#myModal').modal('show');
    }
}
function deleteModal(url, redirectTo) {
    bootbox.confirm("Are you sure want to Delete this Record?", function (result) {
        if (result) {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    showMessage("Record Deleted successfully!!!");
                    loadLink(redirectTo);
                },
                error: function (data) {
                    bootbox.alert('Error in getting result');
                }
            });
        }
    });

}
function loadLink(url, id) {
    if (typeof (id) === 'undefined') {
        $('#mainContent').load(url);
    } else {
        $('#mainContent').load(url);
    }
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
        _.templateSettings = {
            evaluate: /\{\{(.+?)\}\}/g,
            interpolate: /\{\{=(.+?)\}\}/g,
            escape: /\{\{-(.+?)\}\}/g
        };

        render.tmpl_cache[tmplName] = _.template(tmplString);
    }

    return render.tmpl_cache[tmplName](tmplData);
}