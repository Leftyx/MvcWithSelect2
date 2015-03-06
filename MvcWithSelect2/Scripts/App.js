(function (MyApp, $, undefined) {
    'use strict';

    MyApp.Name = 'MyApplication';

    MyApp.init = function () {
    };

    MyApp.render = function () {
    };

    MyApp.CountriesLookup = function (lookupCode, lookupDescription, urlFetchCountries, multiple) {
        $(lookupCode).select2({
            minimumInputLength: 0,
            multiple: multiple || false,
            quietMillis: 400,
            allowClear: true,
            ajax: {
                url: urlFetchCountries,
                dataType: 'json',
                type: "POST",
                data: function (term, page) { return { country: term }; },
                results: function (data, page) {
                    return {
                        results: $.map(data, function (item) {
                            return {
                                id: item.Code,
                                text: item.Description
                            };
                        })
                    };
                }
            },
            initSelection: function (item, callback) {
                if (!multiple) {
                    var id = item.val();
                    var text = item.data('option');
                    if (!text) {
                        text = "";
                    }
                    var data = { id: id, text: text };
                    callback(data);
                }
                else {
                    var data = [];
                    var items = item.val().split(',');
                    $.each(items, function (index, item) {
                        data.push({ id: item, text: item });
                    });
                    $(lookupCode).val('');
                    callback(data);
                }
            },
            formatResult: function (item) { return ('<div>' + item.id + ' - ' + item.text + '</div>'); },
            formatSelection: function (item) {
                if (jQuery.isEmptyObject(item)) {
                    return ('');
                }
                if (lookupDescription) {
                    $(lookupDescription).val(item.text);
                }
                return (multiple ? item.id : item.text);
            },
            dropdownCssClass: "bigdrop",
            escapeMarkup: function (m) { return m; }
        }).on('change', function (e) {
            if ((!jQuery.isEmptyObject(e.removed) && (!e.added)) && (lookupDescription)) {
                $(lookupDescription).val('');
            }
        });
    };

}(window.MyApp = window.MyApp || {}, jQuery));