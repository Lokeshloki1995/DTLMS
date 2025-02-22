
if (typeof(MochiKit) == "undefined") {
    MochiKit = {};
}

if (typeof(MochiKit.MockDOM) == "undefined") {
    MochiKit.MockDOM = {};
}

MochiKit.MockDOM.NAME = "MochiKit.MockDOM";
MochiKit.MockDOM.VERSION = "1.4";

MochiKit.MockDOM.__repr__ = function () {
    return "[" + this.NAME + " " + this.VERSION + "]";
};

MochiKit.MockDOM.toString = function () {
    return this.__repr__();
};

MochiKit.MockDOM.createDocument = function () {
    var doc = new MochiKit.MockDOM.MockElement("DOCUMENT");
    doc.body = doc.createElement("BODY");
    doc.appendChild(doc.body);
    return doc;
};

MochiKit.MockDOM.MockElement = function (name, data) {
    this.nodeName = name.toUpperCase();
    if (typeof(data) == "string") {
        this.nodeValue = data;
        this.nodeType = 3;
    } else {
        this.nodeType = 1;
        this.childNodes = [];
    }
    if (name.substring(0, 1) == "<") {
        var nameattr = name.substring(
            name.indexOf('"') + 1, name.lastIndexOf('"'));
        name = name.substring(1, name.indexOf(" "));
        this.nodeName = name.toUpperCase();
        this.setAttribute("name", nameattr);
    }
};

MochiKit.MockDOM.MockElement.prototype = {
    createElement: function (nodeName) {
        return new MochiKit.MockDOM.MockElement(nodeName);
    },
    createTextNode: function (text) {
        return new MochiKit.MockDOM.MockElement("text", text);
    },
    setAttribute: function (name, value) {
        this[name] = value;
    },
    getAttribute: function (name) {
        return this[name];
    },
    appendChild: function (child) {
        this.childNodes.push(child);
    },
    toString: function () {
        return "MockElement(" + this.nodeName + ")";
    }
};

MochiKit.MockDOM.EXPORT_OK = [
    "mockElement",
    "createDocument"
];

MochiKit.MockDOM.EXPORT = [
    "document"
];

MochiKit.MockDOM.__new__ = function () {
    this.document = this.createDocument();
};

MochiKit.MockDOM.__new__();
