

MochiKit.Base.update(MochiKit.Base, {
    ScriptFragment: '(?:<script.*?>)((\n|\r|.)*?)(?:<\/script>)',

    stripScripts: function (str) {
        return str.replace(new RegExp(MochiKit.Base.ScriptFragment, 'img'), '');
    },

    stripTags: function(str) {
        return str.replace(/<\/?[^>]+>/gi, '');
    },

    extractScripts: function (str) {
        var matchAll = new RegExp(MochiKit.Base.ScriptFragment, 'img');
        var matchOne = new RegExp(MochiKit.Base.ScriptFragment, 'im');
        return MochiKit.Base.map(function (scriptTag) {
            return (scriptTag.match(matchOne) || ['', ''])[1];
        }, str.match(matchAll) || []);
    },

    evalScripts: function (str) {
        return MochiKit.Base.map(function (scr) {
            eval(scr);
        }, MochiKit.Base.extractScripts(str));
    }
});

MochiKit.Form = {
    serialize: function (form) {
        var elements = MochiKit.Form.getElements(form);
        var queryComponents = [];

        for (var i = 0; i < elements.length; i++) {
            var queryComponent = MochiKit.Form.serializeElement(elements[i]);
            if (queryComponent) {
                queryComponents.push(queryComponent);
            }
        }

        return queryComponents.join('&');
    },

    getElements: function (form) {
        form = MochiKit.DOM.getElement(form);
        var elements = [];

        for (tagName in MochiKit.Form.Serializers) {
            var tagElements = form.getElementsByTagName(tagName);
            for (var j = 0; j < tagElements.length; j++) {
                elements.push(tagElements[j]);
            }
        }
        return elements;
    },

    serializeElement: function (element) {
        element = MochiKit.DOM.getElement(element);
        var method = element.tagName.toLowerCase();
        var parameter = MochiKit.Form.Serializers[method](element);

        if (parameter) {
            var key = encodeURIComponent(parameter[0]);
            if (key.length === 0) {
                return;
            }

            if (!(parameter[1] instanceof Array)) {
                parameter[1] = [parameter[1]];
            }

            return parameter[1].map(function (value) {
                return key + '=' + encodeURIComponent(value);
            }).join('&');
        }
    }
};

MochiKit.Form.Serializers = {
    input: function (element) {
        switch (element.type.toLowerCase()) {
            case 'submit':
            case 'hidden':
            case 'password':
            case 'text':
                return MochiKit.Form.Serializers.textarea(element);
            case 'checkbox':
            case 'radio':
                return MochiKit.Form.Serializers.inputSelector(element);
        }
        return false;
    },

    inputSelector: function (element) {
        if (element.checked) {
            return [element.name, element.value];
        }
    },

    textarea: function (element) {
        return [element.name, element.value];
    },

    select: function (element) {
        return MochiKit.Form.Serializers[element.type == 'select-one' ?
        'selectOne' : 'selectMany'](element);
    },

    selectOne: function (element) {
        var value = '', opt, index = element.selectedIndex;
        if (index >= 0) {
            opt = element.options[index];
            value = opt.value;
            if (!value && !('value' in opt)) {
                value = opt.text;
            }
        }
        return [element.name, value];
    },

    selectMany: function (element) {
        var value = [];
        for (var i = 0; i < element.length; i++) {
            var opt = element.options[i];
            if (opt.selected) {
                var optValue = opt.value;
                if (!optValue && !('value' in opt)) {
                    optValue = opt.text;
                }
                value.push(optValue);
            }
        }
        return [element.name, value];
    }
};

var Ajax = {
    activeRequestCount: 0
};

Ajax.Responders = {
    responders: [],

    register: function (responderToAdd) {
        if (MochiKit.Base.find(this.responders, responderToAdd) == -1) {
            this.responders.push(responderToAdd);
        }
    },

    unregister: function (responderToRemove) {
        this.responders = this.responders.without(responderToRemove);
    },

    dispatch: function (callback, request, transport, json) {
        MochiKit.Iter.forEach(this.responders, function (responder) {
            if (responder[callback] &&
                typeof(responder[callback]) == 'function') {
                try {
                    responder[callback].apply(responder, [request, transport, json]);
                } catch (e) {}
            }
        });
    }
};

Ajax.Responders.register({
    onCreate: function () {
        Ajax.activeRequestCount++;
    },

    onComplete: function () {
        Ajax.activeRequestCount--;
    }
});

Ajax.Base = function () {};

Ajax.Base.prototype = {
    setOptions: function (options) {
        this.options = {
            method: 'post',
            asynchronous: true,
            parameters:   ''
        }
        MochiKit.Base.update(this.options, options || {});
    },

    responseIsSuccess: function () {
        return this.transport.status == undefined
            || this.transport.status === 0
            || (this.transport.status >= 200 && this.transport.status < 300);
    },

    responseIsFailure: function () {
        return !this.responseIsSuccess();
    }
};

Ajax.Request = function (url, options) {
    this.__init__(url, options);
};

Ajax.Request.Events = ['Uninitialized', 'Loading', 'Loaded',
                       'Interactive', 'Complete'];

MochiKit.Base.update(Ajax.Request.prototype, Ajax.Base.prototype);

MochiKit.Base.update(Ajax.Request.prototype, {
    __init__: function (url, options) {
        this.transport = MochiKit.Async.getXMLHttpRequest();
        this.setOptions(options);
        this.request(url);
    },

    request: function (url) {
        var parameters = this.options.parameters || '';
        if (parameters.length > 0){
            parameters += '&_=';
        }

        try {
            this.url = url;
            if (this.options.method == 'get' && parameters.length > 0) {
                this.url += (this.url.match(/\?/) ? '&' : '?') + parameters;
            }
            Ajax.Responders.dispatch('onCreate', this, this.transport);

            this.transport.open(this.options.method, this.url,
                                this.options.asynchronous);

            if (this.options.asynchronous) {
                this.transport.onreadystatechange = MochiKit.Base.bind(this.onStateChange, this);
                setTimeout(MochiKit.Base.bind(function () {
                    this.respondToReadyState(1);
                }, this), 10);
            }

            this.setRequestHeaders();

            var body = this.options.postBody ? this.options.postBody : parameters;
            this.transport.send(this.options.method == 'post' ? body : null);

        } catch (e) {
            this.dispatchException(e);
        }
    },

    setRequestHeaders: function () {
        var requestHeaders = ['X-Requested-With', 'XMLHttpRequest'];

        if (this.options.method == 'post') {
            requestHeaders.push('Content-type',
                                'application/x-www-form-urlencoded');

            /* Force 'Connection: close' for Mozilla browsers to work around
             * a bug where XMLHttpRequest sends an incorrect Content-length
             * header. See Mozilla Bugzilla #246651.
             */
            if (this.transport.overrideMimeType) {
                requestHeaders.push('Connection', 'close');
            }
        }

        if (this.options.requestHeaders) {
            requestHeaders.push.apply(requestHeaders, this.options.requestHeaders);
        }

        for (var i = 0; i < requestHeaders.length; i += 2) {
            this.transport.setRequestHeader(requestHeaders[i], requestHeaders[i+1]);
        }
    },

    onStateChange: function () {
        var readyState = this.transport.readyState;
        if (readyState != 1) {
            this.respondToReadyState(this.transport.readyState);
        }
    },

    header: function (name) {
        try {
          return this.transport.getResponseHeader(name);
        } catch (e) {}
    },

    evalJSON: function () {
        try {
          return eval(this.header('X-JSON'));
        } catch (e) {}
    },

    evalResponse: function () {
        try {
          return eval(this.transport.responseText);
        } catch (e) {
          this.dispatchException(e);
        }
    },

    respondToReadyState: function (readyState) {
        var event = Ajax.Request.Events[readyState];
        var transport = this.transport, json = this.evalJSON();

        if (event == 'Complete') {
            try {
                (this.options['on' + this.transport.status]
                || this.options['on' + (this.responseIsSuccess() ? 'Success' : 'Failure')]
                || MochiKit.Base.noop)(transport, json);
            } catch (e) {
                this.dispatchException(e);
            }

            if ((this.header('Content-type') || '').match(/^text\/javascript/i)) {
                this.evalResponse();
            }
        }

        try {
            (this.options['on' + event] || MochiKit.Base.noop)(transport, json);
            Ajax.Responders.dispatch('on' + event, this, transport, json);
        } catch (e) {
            this.dispatchException(e);
        }

        /* Avoid memory leak in MSIE: clean up the oncomplete event handler */
        if (event == 'Complete') {
            this.transport.onreadystatechange = MochiKit.Base.noop;
        }
    },

    dispatchException: function (exception) {
        (this.options.onException || MochiKit.Base.noop)(this, exception);
        Ajax.Responders.dispatch('onException', this, exception);
    }
});

Ajax.Updater = function (container, url, options) {
    this.__init__(container, url, options);
};

MochiKit.Base.update(Ajax.Updater.prototype, Ajax.Request.prototype);

MochiKit.Base.update(Ajax.Updater.prototype, {
    __init__: function (container, url, options) {
        this.containers = {
            success: container.success ? MochiKit.DOM.getElement(container.success) : MochiKit.DOM.getElement(container),
            failure: container.failure ? MochiKit.DOM.getElement(container.failure) :
                (container.success ? null : MochiKit.DOM.getElement(container))
        }
        this.transport = MochiKit.Async.getXMLHttpRequest();
        this.setOptions(options);

        var onComplete = this.options.onComplete || MochiKit.Base.noop;
        this.options.onComplete = MochiKit.Base.bind(function (transport, object) {
            this.updateContent();
            onComplete(transport, object);
        }, this);

        this.request(url);
    },

    updateContent: function () {
        var receiver = this.responseIsSuccess() ?
            this.containers.success : this.containers.failure;
        var response = this.transport.responseText;

        if (!this.options.evalScripts) {
            response = MochiKit.Base.stripScripts(response);
        }

        if (receiver) {
            if (this.options.insertion) {
                new this.options.insertion(receiver, response);
            } else {
                MochiKit.DOM.getElement(receiver).innerHTML =
                    MochiKit.Base.stripScripts(response);
                setTimeout(function () {
                    MochiKit.Base.evalScripts(response);
                }, 10);
            }
        }

        if (this.responseIsSuccess()) {
            if (this.onComplete) {
                setTimeout(MochiKit.Base.bind(this.onComplete, this), 10);
            }
        }
    }
});

var Field = {
    clear: function () {
        for (var i = 0; i < arguments.length; i++) {
            MochiKit.DOM.getElement(arguments[i]).value = '';
        }
    },

    focus: function (element) {
        MochiKit.DOM.getElement(element).focus();
    },

    present: function () {
        for (var i = 0; i < arguments.length; i++) {
            if (MochiKit.DOM.getElement(arguments[i]).value == '') {
                return false;
            }
        }
        return true;
    },

    select: function (element) {
        MochiKit.DOM.getElement(element).select();
    },

    activate: function (element) {
        element = MochiKit.DOM.getElement(element);
        element.focus();
        if (element.select) {
            element.select();
        }
    },

    scrollFreeActivate: function (field) {
        setTimeout(function () {
            Field.activate(field);
        }, 1);
    }
};

var Autocompleter = {};

Autocompleter.Base = function () {};

Autocompleter.Base.prototype = {
    baseInitialize: function (element, update, options) {
        this.element = MochiKit.DOM.getElement(element);
        this.update = MochiKit.DOM.getElement(update);
        this.hasFocus = false;
        this.changed = false;
        this.active = false;
        this.index = 0;
        this.entryCount = 0;

        if (this.setOptions) {
            this.setOptions(options);
        }
        else {
            this.options = options || {};
        }

        this.options.paramName = this.options.paramName || this.element.name;
        this.options.tokens = this.options.tokens || [];
        this.options.frequency = this.options.frequency || 0.4;
        this.options.minChars = this.options.minChars || 1;
        this.options.onShow = this.options.onShow || function (element, update) {
                if (!update.style.position || update.style.position == 'absolute') {
                    update.style.position = 'absolute';
                    MochiKit.Position.clone(element, update, {
                        setHeight: false,
                        offsetTop: element.offsetHeight
                    });
                }
                MochiKit.Visual.appear(update, {duration:0.15});
            };
        this.options.onHide = this.options.onHide || function (element, update) {
                MochiKit.Visual.fade(update, {duration: 0.15});
            };

        if (typeof(this.options.tokens) == 'string') {
            this.options.tokens = new Array(this.options.tokens);
        }

        this.observer = null;

        this.element.setAttribute('autocomplete', 'off');

        MochiKit.Style.hideElement(this.update);

        MochiKit.Signal.connect(this.element, 'onblur', this, this.onBlur);
        MochiKit.Signal.connect(this.element, 'onkeypress', this, this.onKeyPress, this);
    },

    show: function () {
        if (MochiKit.DOM.getStyle(this.update, 'display') == 'none') {
            this.options.onShow(this.element, this.update);
        }
        if (!this.iefix && MochiKit.Base.isIE() && MochiKit.Base.isOpera() &&
            (MochiKit.DOM.getStyle(this.update, 'position') == 'absolute')) {
            new Insertion.After(this.update,
             '<iframe id="' + this.update.id + '_iefix" '+
             'style="display:none;position:absolute;filter:progid:DXImageTransform.Microsoft.Alpha(opacity=0);" ' +
             'src="javascript:false;" frameborder="0" scrolling="no"></iframe>');
            this.iefix = MochiKit.DOM.getElement(this.update.id + '_iefix');
        }
        if (this.iefix) {
            setTimeout(MochiKit.Base.bind(this.fixIEOverlapping, this), 50);
        }
    },

    fixIEOverlapping: function () {
        MochiKit.Position.clone(this.update, this.iefix);
        this.iefix.style.zIndex = 1;
        this.update.style.zIndex = 2;
        MochiKit.Style.showElement(this.iefix);
    },

    hide: function () {
        this.stopIndicator();
        if (MochiKit.DOM.getStyle(this.update, 'display') != 'none') {
            this.options.onHide(this.element, this.update);
        }
        if (this.iefix) {
            MochiKit.Style.hideElement(this.iefix);
        }
    },

    startIndicator: function () {
        if (this.options.indicator) {
            MochiKit.Style.showElement(this.options.indicator);
        }
    },

    stopIndicator: function () {
        if (this.options.indicator) {
            MochiKit.Style.hideElement(this.options.indicator);
        }
    },

    onKeyPress: function (event) {
        if (this.active) {
            if (event.keyString == "KEY_TAB" || event.keyString == "KEY_RETURN") {
                 this.selectEntry();
                 MochiKit.Event.stop(event);
            } else if (event.keyString == "KEY_ESCAPE") {
                 this.hide();
                 this.active = false;
                 MochiKit.Event.stop(event);
                 return;
            } else if (event.keyString == "KEY_LEFT" || event.keyString == "KEY_RIGHT") {
                 return;
            } else if (event.keyString == "KEY_UP") {
                 this.markPrevious();
                 this.render();
                 if (MochiKit.Base.isSafari()) {
                     event.stop();
                 }
                 return;
            } else if (event.keyString == "KEY_DOWN") {
                 this.markNext();
                 this.render();
                 if (MochiKit.Base.isSafari()) {
                     event.stop();
                 }
                 return;
            }
        } else {
            if (event.keyString == "KEY_TAB" || event.keyString == "KEY_RETURN") {
                return;
            }
        }

        this.changed = true;
        this.hasFocus = true;

        if (this.observer) {
            clearTimeout(this.observer);
        }
        this.observer = setTimeout(MochiKit.Base.bind(this.onObserverEvent, this),
                                   this.options.frequency*1000);
    },

    findElement: function (event, tagName) {
        var element = event.target;
        while (element.parentNode && (!element.tagName ||
              (element.tagName.toUpperCase() != tagName.toUpperCase()))) {
            element = element.parentNode;
        }
        return element;
    },

    onHover: function (event) {
        var element = this.findElement(event, 'LI');
        if (this.index != element.autocompleteIndex) {
            this.index = element.autocompleteIndex;
            this.render();
        }
        event.stop();
    },

    onClick: function (event) {
        var element = this.findElement(event, 'LI');
        this.index = element.autocompleteIndex;
        this.selectEntry();
        this.hide();
    },

    onBlur: function (event) {
        // needed to make click events working
        setTimeout(MochiKit.Base.bind(this.hide, this), 250);
        this.hasFocus = false;
        this.active = false;
    },

    render: function () {
        if (this.entryCount > 0) {
            for (var i = 0; i < this.entryCount; i++) {
                this.index == i ?
                    MochiKit.DOM.addElementClass(this.getEntry(i), 'selected') :
                    MochiKit.DOM.removeElementClass(this.getEntry(i), 'selected');
            }
            if (this.hasFocus) {
                this.show();
                this.active = true;
            }
        } else {
            this.active = false;
            this.hide();
        }
    },

    markPrevious: function () {
        if (this.index > 0) {
            this.index--
        } else {
            this.index = this.entryCount-1;
        }
    },

    markNext: function () {
        if (this.index < this.entryCount-1) {
            this.index++
        } else {
            this.index = 0;
        }
    },

    getEntry: function (index) {
        return this.update.firstChild.childNodes[index];
    },

    getCurrentEntry: function () {
        return this.getEntry(this.index);
    },

    selectEntry: function () {
        this.active = false;
        this.updateElement(this.getCurrentEntry());
    },

    collectTextNodesIgnoreClass: function (element, className) {
        return MochiKit.Base.flattenArray(MochiKit.Base.map(function (node) {
            if (node.nodeType == 3) {
                return node.nodeValue;
            } else if (node.hasChildNodes() && !MochiKit.DOM.hasElementClass(node, className)) {
                return this.collectTextNodesIgnoreClass(node, className);
            }
            return '';
        }, MochiKit.DOM.getElement(element).childNodes)).join('');
    },

    updateElement: function (selectedElement) {
        if (this.options.updateElement) {
            this.options.updateElement(selectedElement);
            return;
        }
        var value = '';
        if (this.options.select) {
            var nodes = document.getElementsByClassName(this.options.select, selectedElement) || [];
            if (nodes.length > 0) {
                value = MochiKit.DOM.scrapeText(nodes[0]);
            }
        } else {
            value = this.collectTextNodesIgnoreClass(selectedElement, 'informal');
        }
        var lastTokenPos = this.findLastToken();
        if (lastTokenPos != -1) {
            var newValue = this.element.value.substr(0, lastTokenPos + 1);
            var whitespace = this.element.value.substr(lastTokenPos + 1).match(/^\s+/);
            if (whitespace) {
                newValue += whitespace[0];
            }
            this.element.value = newValue + value;
        } else {
            this.element.value = value;
        }
        this.element.focus();

        if (this.options.afterUpdateElement) {
            this.options.afterUpdateElement(this.element, selectedElement);
        }
    },

    updateChoices: function (choices) {
        if (!this.changed && this.hasFocus) {
            this.update.innerHTML = choices;
            var d = MochiKit.DOM;
            d.removeEmptyTextNodes(this.update);
            d.removeEmptyTextNodes(this.update.firstChild);

            if (this.update.firstChild && this.update.firstChild.childNodes) {
                this.entryCount = this.update.firstChild.childNodes.length;
                for (var i = 0; i < this.entryCount; i++) {
                    var entry = this.getEntry(i);
                    entry.autocompleteIndex = i;
                    this.addObservers(entry);
                }
            } else {
                this.entryCount = 0;
            }

            this.stopIndicator();

            this.index = 0;
            this.render();
        }
    },

    addObservers: function (element) {
        MochiKit.Signal.connect(element, 'onmouseover', this, this.onHover);
        MochiKit.Signal.connect(element, 'onclick', this, this.onClick);
    },

    onObserverEvent: function () {
        this.changed = false;
        if (this.getToken().length >= this.options.minChars) {
            this.startIndicator();
            this.getUpdatedChoices();
        } else {
            this.active = false;
            this.hide();
        }
    },

    getToken: function () {
        var tokenPos = this.findLastToken();
        if (tokenPos != -1) {
            var ret = this.element.value.substr(tokenPos + 1).replace(/^\s+/,'').replace(/\s+$/,'');
        } else {
            var ret = this.element.value;
        }
        return /\n/.test(ret) ? '' : ret;
    },

    findLastToken: function () {
        var lastTokenPos = -1;

        for (var i = 0; i < this.options.tokens.length; i++) {
            var thisTokenPos = this.element.value.lastIndexOf(this.options.tokens[i]);
            if (thisTokenPos > lastTokenPos) {
                lastTokenPos = thisTokenPos;
            }
        }
        return lastTokenPos;
    }
}

Ajax.Autocompleter = function (element, update, url, options) {
    this.__init__(element, update, url, options);
};

MochiKit.Base.update(Ajax.Autocompleter.prototype, Autocompleter.Base.prototype);

MochiKit.Base.update(Ajax.Autocompleter.prototype, {
    __init__: function (element, update, url, options) {
        this.baseInitialize(element, update, options);
        this.options.asynchronous = true;
        this.options.onComplete = MochiKit.Base.bind(this.onComplete, this);
        this.options.defaultParams = this.options.parameters || null;
        this.url = url;
    },

    getUpdatedChoices: function () {
        var entry = encodeURIComponent(this.options.paramName) + '=' +
            encodeURIComponent(this.getToken());

        this.options.parameters = this.options.callback ?
            this.options.callback(this.element, entry) : entry;

        if (this.options.defaultParams) {
            this.options.parameters += '&' + this.options.defaultParams;
        }
        new Ajax.Request(this.url, this.options);
    },

    onComplete: function (request) {
        this.updateChoices(request.responseText);
    }
});

/***

The local array autocompleter. Used when you'd prefer to
inject an array of autocompletion options into the page, rather
than sending out Ajax queries, which can be quite slow sometimes.

The constructor takes four parameters. The first two are, as usual,
the id of the monitored textbox, and id of the autocompletion menu.
The third is the array you want to autocomplete from, and the fourth
is the options block.

Extra local autocompletion options:
- choices - How many autocompletion choices to offer

- partialSearch - If false, the autocompleter will match entered
                                       text only at the beginning of strings in the
                                       autocomplete array. Defaults to true, which will
                                       match text at the beginning of any *word* in the
                                       strings in the autocomplete array. If you want to
                                       search anywhere in the string, additionally set
                                       the option fullSearch to true (default: off).

- fullSsearch - Search anywhere in autocomplete array strings.

- partialChars - How many characters to enter before triggering
                                    a partial match (unlike minChars, which defines
                                    how many characters are required to do any match
                                    at all). Defaults to 2.

- ignoreCase - Whether to ignore case when autocompleting.
                                Defaults to true.

It's possible to pass in a custom function as the 'selector'
option, if you prefer to write your own autocompletion logic.
In that case, the other options above will not apply unless
you support them.

***/

Autocompleter.Local = function (element, update, array, options) {
    this.__init__(element, update, array, options);
};

MochiKit.Base.update(Autocompleter.Local.prototype, Autocompleter.Base.prototype);

MochiKit.Base.update(Autocompleter.Local.prototype, {
    __init__: function (element, update, array, options) {
        this.baseInitialize(element, update, options);
        this.options.array = array;
    },

    getUpdatedChoices: function () {
        this.updateChoices(this.options.selector(this));
    },

    setOptions: function (options) {
        this.options = MochiKit.Base.update({
            choices: 10,
            partialSearch: true,
            partialChars: 2,
            ignoreCase: true,
            fullSearch: false,
            selector: function (instance) {
                var ret = [];  // Beginning matches
                var partial = [];  // Inside matches
                var entry = instance.getToken();
                var count = 0;

                for (var i = 0; i < instance.options.array.length &&
                    ret.length < instance.options.choices ; i++) {

                    var elem = instance.options.array[i];
                    var foundPos = instance.options.ignoreCase ?
                        elem.toLowerCase().indexOf(entry.toLowerCase()) :
                        elem.indexOf(entry);

                    while (foundPos != -1) {
                        if (foundPos === 0 && elem.length != entry.length) {
                            ret.push('<li><strong>' + elem.substr(0, entry.length) + '</strong>' +
                                elem.substr(entry.length) + '</li>');
                            break;
                        } else if (entry.length >= instance.options.partialChars &&
                            instance.options.partialSearch && foundPos != -1) {
                            if (instance.options.fullSearch || /\s/.test(elem.substr(foundPos - 1, 1))) {
                                partial.push('<li>' + elem.substr(0, foundPos) + '<strong>' +
                                    elem.substr(foundPos, entry.length) + '</strong>' + elem.substr(
                                    foundPos + entry.length) + '</li>');
                                break;
                            }
                        }

                        foundPos = instance.options.ignoreCase ?
                            elem.toLowerCase().indexOf(entry.toLowerCase(), foundPos + 1) :
                            elem.indexOf(entry, foundPos + 1);

                    }
                }
                if (partial.length) {
                    ret = ret.concat(partial.slice(0, instance.options.choices - ret.length))
                }
                return '<ul>' + ret.join('') + '</ul>';
            }
        }, options || {});
    }
});

/***

AJAX in-place editor

see documentation on http://wiki.script.aculo.us/scriptaculous/show/Ajax.InPlaceEditor

Use this if you notice weird scrolling problems on some browsers,
the DOM might be a bit confused when this gets called so do this
waits 1 ms (with setTimeout) until it does the activation

***/

Ajax.InPlaceEditor = function (element, url, options) {
    this.__init__(element, url, options);
};

Ajax.InPlaceEditor.defaultHighlightColor = '#FFFF99';

Ajax.InPlaceEditor.prototype = {
    __init__: function (element, url, options) {
        this.url = url;
        this.element = MochiKit.DOM.getElement(element);

        this.options = MochiKit.Base.update({
            okButton: true,
            okText: 'ok',
            cancelLink: true,
            cancelText: 'cancel',
            savingText: 'Saving...',
            clickToEditText: 'Click to edit',
            okText: 'ok',
            rows: 1,
            onComplete: function (transport, element) {
                new MochiKit.Visual.Highlight(element, {startcolor: this.options.highlightcolor});
            },
            onFailure: function (transport) {
                alert('Error communicating with the server: ' + MochiKit.Base.stripTags(transport.responseText));
            },
            callback: function (form) {
                return MochiKit.DOM.formContents(form);
            },
            handleLineBreaks: true,
            loadingText: 'Loading...',
            savingClassName: 'inplaceeditor-saving',
            loadingClassName: 'inplaceeditor-loading',
            formClassName: 'inplaceeditor-form',
            highlightcolor: Ajax.InPlaceEditor.defaultHighlightColor,
            highlightendcolor: '#FFFFFF',
            externalControl: null,
            submitOnBlur: false,
            ajaxOptions: {}
        }, options || {});

        if (!this.options.formId && this.element.id) {
            this.options.formId = this.element.id + '-inplaceeditor';
            if (MochiKit.DOM.getElement(this.options.formId)) {
                // there's already a form with that name, don't specify an id
                this.options.formId = null;
            }
        }

        if (this.options.externalControl) {
            this.options.externalControl = MochiKit.DOM.getElement(this.options.externalControl);
        }

        this.originalBackground = MochiKit.DOM.getStyle(this.element, 'background-color');
        if (!this.originalBackground) {
            this.originalBackground = 'transparent';
        }

        this.element.title = this.options.clickToEditText;

        this.onclickListener = MochiKit.Signal.connect(this.element, 'onclick', this, this.enterEditMode);
        this.mouseoverListener = MochiKit.Signal.connect(this.element, 'onmouseover', this, this.enterHover);
        this.mouseoutListener = MochiKit.Signal.connect(this.element, 'onmouseout', this, this.leaveHover);
        if (this.options.externalControl) {
            this.onclickListenerExternal = MochiKit.Signal.connect(this.options.externalControl,
                'onclick', this, this.enterEditMode);
            this.mouseoverListenerExternal = MochiKit.Signal.connect(this.options.externalControl,
                'onmouseover', this, this.enterHover);
            this.mouseoutListenerExternal = MochiKit.Signal.connect(this.options.externalControl,
                'onmouseout', this, this.leaveHover);
        }
    },

    enterEditMode: function (evt) {
        if (this.saving) {
            return;
        }
        if (this.editing) {
            return;
        }
        this.editing = true;
        this.onEnterEditMode();
        if (this.options.externalControl) {
            MochiKit.Style.hideElement(this.options.externalControl);
        }
        MochiKit.Style.hideElement(this.element);
        this.createForm();
        this.element.parentNode.insertBefore(this.form, this.element);
        Field.scrollFreeActivate(this.editField);
        // stop the event to avoid a page refresh in Safari
        if (evt) {
            evt.stop();
        }
        return false;
    },

    createForm: function () {
        this.form = document.createElement('form');
        this.form.id = this.options.formId;
        MochiKit.DOM.addElementClass(this.form, this.options.formClassName)
        this.form.onsubmit = MochiKit.Base.bind(this.onSubmit, this);

        this.createEditField();

        if (this.options.textarea) {
            var br = document.createElement('br');
            this.form.appendChild(br);
        }

        if (this.options.okButton) {
            okButton = document.createElement('input');
            okButton.type = 'submit';
            okButton.value = this.options.okText;
            this.form.appendChild(okButton);
        }

        if (this.options.cancelLink) {
            cancelLink = document.createElement('a');
            cancelLink.href = '#';
            cancelLink.appendChild(document.createTextNode(this.options.cancelText));
            cancelLink.onclick = MochiKit.Base.bind(this.onclickCancel, this);
            this.form.appendChild(cancelLink);
        }
    },

    hasHTMLLineBreaks: function (string) {
        if (!this.options.handleLineBreaks) {
            return false;
        }
        return string.match(/<br/i) || string.match(/<p>/i);
    },

    convertHTMLLineBreaks: function (string) {
        return string.replace(/<br>/gi, '\n').replace(/<br\/>/gi, '\n').replace(/<\/p>/gi, '\n').replace(/<p>/gi, '');
    },

    createEditField: function () {
        var text;
        if (this.options.loadTextURL) {
            text = this.options.loadingText;
        } else {
            text = this.getText();
        }

        var obj = this;

        if (this.options.rows == 1 && !this.hasHTMLLineBreaks(text)) {
            this.options.textarea = false;
            var textField = document.createElement('input');
            textField.obj = this;
            textField.type = 'text';
            textField.name = 'value';
            textField.value = text;
            textField.style.backgroundColor = this.options.highlightcolor;
            var size = this.options.size || this.options.cols || 0;
            if (size !== 0) {
                textField.size = size;
            }
            if (this.options.submitOnBlur) {
                textField.onblur = MochiKit.Base.bind(this.onSubmit, this);
            }
            this.editField = textField;
        } else {
            this.options.textarea = true;
            var textArea = document.createElement('textarea');
            textArea.obj = this;
            textArea.name = 'value';
            textArea.value = this.convertHTMLLineBreaks(text);
            textArea.rows = this.options.rows;
            textArea.cols = this.options.cols || 40;
            if (this.options.submitOnBlur) {
                textArea.onblur = MochiKit.Base.bind(this.onSubmit, this);
            }
            this.editField = textArea;
        }

        if (this.options.loadTextURL) {
            this.loadExternalText();
        }
        this.form.appendChild(this.editField);
    },

    getText: function () {
        return this.element.innerHTML;
    },

    loadExternalText: function () {
        MochiKit.DOM.addElementClass(this.form, this.options.loadingClassName);
        this.editField.disabled = true;
        new Ajax.Request(
            this.options.loadTextURL,
            MochiKit.Base.update({
                asynchronous: true,
                onComplete: MochiKit.Base.bind(this.onLoadedExternalText, this)
            }, this.options.ajaxOptions)
        );
    },

    onLoadedExternalText: function (transport) {
        MochiKit.DOM.removeElementClass(this.form, this.options.loadingClassName);
        this.editField.disabled = false;
        this.editField.value = MochiKit.Base.stripTags(transport);
    },

    onclickCancel: function () {
        this.onComplete();
        this.leaveEditMode();
        return false;
    },

    onFailure: function (transport) {
        this.options.onFailure(transport);
        if (this.oldInnerHTML) {
            this.element.innerHTML = this.oldInnerHTML;
            this.oldInnerHTML = null;
        }
        return false;
    },

    onSubmit: function () {
        // onLoading resets these so we need to save them away for the Ajax call
        var form = this.form;
        var value = this.editField.value;

        // do this first, sometimes the ajax call returns before we get a
        // chance to switch on Saving which means this will actually switch on
        // Saving *after* we have left edit mode causing Saving to be
        // displayed indefinitely
        this.onLoading();

        new Ajax.Updater(
            {
                success: this.element,
                 // dont update on failure (this could be an option)
                failure: null
            },
            this.url,
            MochiKit.Base.update({
                parameters: this.options.callback(form, value),
                onComplete: MochiKit.Base.bind(this.onComplete, this),
                onFailure: MochiKit.Base.bind(this.onFailure, this)
            }, this.options.ajaxOptions)
        );
        // stop the event to avoid a page refresh in Safari
        if (arguments.length > 1) {
            arguments[0].stop();
        }
        return false;
    },

    onLoading: function () {
        this.saving = true;
        this.removeForm();
        this.leaveHover();
        this.showSaving();
    },

    showSaving: function () {
        this.oldInnerHTML = this.element.innerHTML;
        this.element.innerHTML = this.options.savingText;
        MochiKit.DOM.addElementClass(this.element, this.options.savingClassName);
        this.element.style.backgroundColor = this.originalBackground;
        MochiKit.Style.showElement(this.element);
    },

    removeForm: function () {
        if (this.form) {
            if (this.form.parentNode) {
                MochiKit.DOM.removeElement(this.form);
            }
            this.form = null;
        }
    },

    enterHover: function () {
        if (this.saving) {
            return;
        }
        this.element.style.backgroundColor = this.options.highlightcolor;
        if (this.effect) {
            this.effect.cancel();
        }
        MochiKit.DOM.addElementClass(this.element, this.options.hoverClassName)
    },

    leaveHover: function () {
        if (this.options.backgroundColor) {
            this.element.style.backgroundColor = this.oldBackground;
        }
        MochiKit.DOM.removeElementClass(this.element, this.options.hoverClassName)
        if (this.saving) {
            return;
        }
        this.effect = new MochiKit.Visual.Highlight(this.element, {
            startcolor: this.options.highlightcolor,
            endcolor: this.options.highlightendcolor,
            restorecolor: this.originalBackground
        });
    },

    leaveEditMode: function () {
        MochiKit.DOM.removeElementClass(this.element, this.options.savingClassName);
        this.removeForm();
        this.leaveHover();
        this.element.style.backgroundColor = this.originalBackground;
        MochiKit.Style.showElement(this.element);
        if (this.options.externalControl) {
            MochiKit.Style.showElement(this.options.externalControl);
        }
        this.editing = false;
        this.saving = false;
        this.oldInnerHTML = null;
        this.onLeaveEditMode();
    },

    onComplete: function (transport) {
        this.leaveEditMode();
        MochiKit.Base.bind(this.options.onComplete, this)(transport, this.element);
    },

    onEnterEditMode: function () {},

    onLeaveEditMode: function () {},

    dispose: function () {
        if (this.oldInnerHTML) {
            this.element.innerHTML = this.oldInnerHTML;
        }
        this.leaveEditMode();
        MochiKit.Signal.disconnect(this.onclickListener);
        MochiKit.Signal.disconnect(this.mouseoverListener);
        MochiKit.Signal.disconnect(this.mouseoutListener);
        if (this.options.externalControl) {
            MochiKit.Signal.disconnect(this.onclickListenerExternal);
            MochiKit.Signal.disconnect(this.mouseoverListenerExternal);
            MochiKit.Signal.disconnect(this.mouseoutListenerExternal);
        }
    }
};

