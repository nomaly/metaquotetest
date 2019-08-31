/* eslint-disable no-unused-vars */
/* eslint-disable no-undef */

$(function () {
    const leftBarItems = $("element");

    window.GeobaseVM = GeobaseForm.instance;
});

const singleton = Symbol("singleton");
const singletonEnforcer = Symbol("singletonEnforcer");

class GeobaseForm {
    constructor(enforcer) {
        if (enforcer !== singletonEnforcer) {
            throw new Error("Cannot construct singleton");
        }

        this._geobaseClient = new GeobaseClient();
        this._table = GeobaseTable.getInstance();
        this._currentState = GeobaseInitState.create();
        this._bindHandlers();
    }

    _bindHandlers() {
        const self = this;

        GeobaseButton.searchByIp.onClick(e => {
            self.setSearchByIpState();
        });

        GeobaseButton.searchByCity.onClick(e => {
            self.setSearchByCityState();
        });

        const textfield = GeobaseTextField.getInstance();
        GeobaseButton.makeRequest.onClick(e => {
            self._makeRequest.apply(self, [textfield.Value]);
        });
    }

    _makeRequest(value) {
        const client = this._geobaseClient;
        switch (this.state) {
            case "bycity":
                client.getByCity(value, this._onDataReceived.bind(this));
                break;
            case "byip":
                client.getByIp(value, this._onDataReceived.bind(this));
                break;
        }
    }

    _onDataReceived(data) {
        this._table.printData({ locations: data });
    }

    get state() {
        if (this._currentState instanceof GeobaseSearchByCityState
            || this._currentState instanceof GeobaseInitState) {
            return 'bycity';
        }

        if (this._currentState instanceof GeobaseSearchByIpState) {
            return 'byip';
        }

        return undefined;
    }

    get updated() {

    }

    set updated(value) {

    }

    static get instance() {
        if (!this[singleton]) {
            this[singleton] = new GeobaseForm(singletonEnforcer);
        }
        return this[singleton];
    }

    setSearchByCityState() {
        this._currentState = this._currentState.changeState(GeobaseSearchByCityState.create());
    }

    setSearchByIpState() {
        this._currentState = this._currentState.changeState(GeobaseSearchByIpState.create());
    }
}

class State {
    changeState(nextState) {
        this.restore();
        nextState.set();
        return nextState;
    }

    restore() {
        throw new Error("not implemented");
    }

    set() {
        throw new Error("not implemented");
    }
}

class GeobaseInitState extends State {
    constructor() {
        super();
        GeobaseButton.searchByCity.setEnabled();
        GeobaseButton.searchByIp.setDisabled();
        GeobaseTextField.getInstance().Value = "";
    }

    static create() {
        return new GeobaseInitState();
    }

    restore() { }
}

class GeobaseSearchByCityState extends State {
    constructor(byCityBtn, byIpBtn) {
        super();
        this._byCityBtn = byCityBtn;
        this._byIpBtn = byIpBtn;
        this._cityBtnOriginEnabled = this._byCityBtn.IsEnabled;
        this._ipBtnOriginEnabled = this._byIpBtn.IsEnabled;
    }

    set() {
        this._byIpBtn.setDisabled();
        this._byCityBtn.setEnabled();
    }

    restore() {
        if (this._cityBtnOriginEnabled) {
            this._byCityBtn.setEnabled();
        } else {
            this._byCityBtn.setDisabled();
        }

        if (this._ipBtnOriginEnabled) {
            this._byIpBtn.setEnabled();
        } else {
            this._byIpBtn.setDisabled();
        }
    }

    static create() {
        const citBtn = GeobaseButton.getInstance(BUTTON_SEARCH_BY_CITY);
        const ipBtn = GeobaseButton.getInstance(BUTTON_SEARCH_BY_IP);
        return new GeobaseSearchByCityState(citBtn, ipBtn);
    }
}

class GeobaseSearchByIpState extends State {
    constructor(byCityBtn, byIpBtn) {
        super();
        this._byCityBtn = byCityBtn;
        this._byIpBtn = byIpBtn;
        this._cityBtnOriginEnabled = this._byCityBtn.IsEnabled;
        this._ipBtnOriginEnabled = this._byIpBtn.IsEnabled;
    }

    set() {
        this._byCityBtn.setDisabled();
        this._byIpBtn.setEnabled();
        console.log(this);
    }

    restore() {
        if (this._cityBtnOriginEnabled) {
            this._byCityBtn.setEnabled();
        } else {
            this._byCityBtn.setDisabled();
        }

        if (this._ipBtnOriginEnabled) {
            this._byIpBtn.setEnabled();
        } else {
            this._byIpBtn.setDisabled();
        }
    }

    static create() {
        const citBtn = GeobaseButton.getInstance(BUTTON_SEARCH_BY_CITY);
        const ipBtn = GeobaseButton.getInstance(BUTTON_SEARCH_BY_IP);
        return new GeobaseSearchByIpState(citBtn, ipBtn);
    }
}

const BUTTON_SEARCH_BY_CITY = "#searchByCityBtn";
const BUTTON_SEARCH_BY_IP = "#searchByIpBtn";
const BUTTON_MAKE_REQUEST = "#makeRequestBtn";

class GeobaseButton {
    constructor(buttonId) {
        this._btnElement = $(buttonId);
    }

    get IsEnabled() {
        return this._btnElement.hasClass("enabled");
    }

    setEnabled() {
        if (!this.IsEnabled) {
            this._btnElement.addClass("enabled");
        }
    }

    setDisabled() {
        if (this.IsEnabled) {
            this._btnElement.removeClass("enabled");
        }
    }

    onClick(action) {
        this._btnElement.on("click", action);
    }

    static getInstance(buttonId) {
        return new GeobaseButton(buttonId);
    }

    static get searchByCity() {
        return GeobaseButton.getInstance(BUTTON_SEARCH_BY_CITY);
    }

    static get searchByIp() {
        return GeobaseButton.getInstance(BUTTON_SEARCH_BY_IP);
    }

    static get makeRequest() {
        return GeobaseButton.getInstance(BUTTON_MAKE_REQUEST);
    }
}

const TABLE_DATA = "#data";

class GeobaseTable {

    constructor(tableId) {
        this._table = $(tableId);
        this._views = GeobaseView.instance;
    }

    printData(data) {
        const html = this._views.renderData(data);
        this._table.html(html);
    }

    static getInstance() {
        return new GeobaseTable(TABLE_DATA);
    }
}

const TEXT_FIELD = "#textField";

class GeobaseTextField {
    constructor(textfieldId) {
        this._field = $(textfieldId);
    }

    get jqObject() {
        return this._field;
    }

    get Value() {
        return this._field.val();
    }

    set Value(value) {
        this._field.val(value);
    }

    static getInstance() {
        return new GeobaseTextField(TEXT_FIELD);
    }
}

function uuidv4() {
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
        const r = Math.random() * 16 | 0;
        const v = c === "x" ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

class GeobaseClient {
    getByCity(city, cb) {
        return $.getJSON(`${window.location.origin}/city/locations?city=${city}`, cb);
    }

    getByIp(ip, cb) {
        return $.getJSON(`${window.location.origin}/ip/location?ip=${ip}`, cb);
    }
}

const viewSingleton = Symbol("viewSingleton");
const viewSingletonEnforcer = Symbol("viewSingletonEnforcer");

class GeobaseView {
    constructor(enforcer) {
        if (enforcer !== viewSingletonEnforcer) {
            throw new Error("Cannot construct singleton");
        }

        if (!window.Handlebars) {
            throw new Error("Handlebars is not installed");
        }

        this._handlebars = window.Handlebars;
        this._handlebars.partials.data = this._getdataView();
    }

    renderData(data) {
        return Handlebars.partials.data(data);
    }

    _getdataView() {
        var template = Handlebars.template;
        Handlebars.templates = Handlebars.templates || {};

        return template({
            1: function (container, depth0, helpers, partials, data) {
                var helper;

                return "	    <tr>\n			<td>" +
                    container.escapeExpression(((helper = (helper = helpers.Country || (depth0 != null ? depth0.Country : depth0)) != null ? helper : helpers.helperMissing), (typeof helper === "function" ? helper.call(depth0 != null ? depth0 : {}, {
                        name: "Country",
                        hash: {},
                        data: data
                    }) : helper))) +
                    "</td>\n			<td>" +
                    container.escapeExpression(((helper = (helper = helpers.Region || (depth0 != null ? depth0.Region : depth0)) != null ? helper : helpers.helperMissing), (typeof helper === "function" ? helper.call(depth0 != null ? depth0 : {}, {
                        name: "Region",
                        hash: {},
                        data: data
                    }) : helper))) +
                    "</td>\n			<td>" +
                    container.escapeExpression(((helper = (helper = helpers.Postal || (depth0 != null ? depth0.Postal : depth0)) != null ? helper : helpers.helperMissing), (typeof helper === "function" ? helper.call(depth0 != null ? depth0 : {}, {
                        name: "Postal",
                        hash: {},
                        data: data
                    }) : helper))) +
                    "</td>\n			<td>" +
                    container.escapeExpression(((helper = (helper = helpers.City || (depth0 != null ? depth0.City : depth0)) != null ? helper : helpers.helperMissing), (typeof helper === "function" ? helper.call(depth0 != null ? depth0 : {}, {
                        name: "City",
                        hash: {},
                        data: data
                    }) : helper))) +
                    "</td>\n			<td>" +
                    container.escapeExpression(((helper = (helper = helpers.Organization || (depth0 != null ? depth0.Organization : depth0)) != null ? helper : helpers.helperMissing), (typeof helper === "function" ? helper.call(depth0 != null ? depth0 : {}, {
                        name: "Organization",
                        hash: {},
                        data: data
                    }) : helper))) +
                    "</td>\n			<td>" +
                    container.escapeExpression(((helper = (helper = helpers.Latitude || (depth0 != null ? depth0.Latitude : depth0)) != null ? helper : helpers.helperMissing), (typeof helper === "function" ? helper.call(depth0 != null ? depth0 : {}, {
                        name: "Latitude",
                        hash: {},
                        data: data
                    }) : helper))) +
                    "</td>\n			<td>" +
                    container.escapeExpression(((helper = (helper = helpers.Longitude || (depth0 != null ? depth0.Longitude : depth0)) != null ? helper : helpers.helperMissing), (typeof helper === "function" ? helper.call(depth0 != null ? depth0 : {}, {
                        name: "Longitude",
                        hash: {},
                        data: data
                    }) : helper))) +
                    "</td>\n	    </tr>\n";
            },
            compiler: [7, ">= 4.0.0"],
            main: function (container, depth0, helpers, partials, data) {
                var stack1;
                var helper;
                var options;
                var buffer =
                    "<table class=\"zui-table\">\n	<thead>\n		<tr>\n			<th>Country</th>\n			<th>Region</th>\n			<th>Postal</th>\n			<th>City</th>\n			<th>Organization</th>\n			<th>Latitude</th>\n			<th>Longitude</th>\n		</tr>\n	</thead>\n	<tbody>\n";
                stack1 = ((helper = (helper = helpers.locations || (depth0 != null ? depth0.locations : depth0)) != null ? helper : helpers.helperMissing), (options = {
                    name: "locations",
                    hash: {},
                    fn: container.program(1, data, 0),
                    inverse: container.noop,
                    data: data
                }), (typeof helper === "function" ? helper.call(depth0 != null ? depth0 : {}, options) : helper));
                if (!helpers.locations) {
                    stack1 = helpers.blockHelperMissing.call(depth0, stack1, options);
                }
                if (stack1 != null) {
                    buffer += stack1;
                }
                return buffer + "	</tbody>\n</table>";
            },
            useData: true
        });
    }

    static get instance() {
        if (!this[viewSingleton]) {
            this[viewSingleton] = new GeobaseView(viewSingletonEnforcer);
        }
        return this[viewSingleton];
    }
}

window.GeobaseVm = GeobaseForm.instance;