document.querySelectorAll(".mvc-grid").forEach(element => new MvcGrid(element));
const gridFilterButtons = document.querySelectorAll('button.mvc-grid-filter');

gridFilterButtons.forEach(elementInfo => {
    elementInfo.addEventListener('click', () => {
        const gridDateFilterDateFields =
            document.querySelectorAll('.mvc-grid-date-filter input.mvc-grid-value');
        gridDateFilterDateFields.forEach(fld => {
            fld.setAttribute('type', 'date');
        });
    });
});

var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new window.bootstrap.Tooltip(tooltipTriggerEl);
});


const gridPager = document.querySelector('.table-scroll-container .mvc-grid-pager');
if (gridPager) {
    document.getElementById('large-grid-pager').append(gridPager);
}

const largeGridRows = document.querySelectorAll('.large-index-grid tbody tr');

largeGridRows.forEach(elementInfo => {
    elementInfo.addEventListener('click', () => {
        const selectedRows = document.querySelectorAll('.large-index-grid tbody tr.grid-selected-row');
        selectedRows.forEach(sr => {
            sr.classList.remove('grid-selected-row');
        })
        elementInfo.classList.add('grid-selected-row');
    });
});

var generallyUsedCssClass = {
    hidn: 'd-none',
    successText: 'text-success',
    warningText: 'text-warning',
    dangerText: 'text-danger'
};

var generalUtility = {
    validateElement: {
        formById: (formId) => {
            return window.$(`#${formId}`).valid();
        },
        formByForm: (formInfo) => {
            const formId = formInfo.getAttribute('id');
            return window.$(`#${formId}`).valid();
        },
        elementById: (elementId) => {
            const formInfo = window.$(`#${elementId}`).closest('form');
            return formInfo.validate().element(`#${elementId}`);
        },
        elementByElement: (elementInfo) => {
            const elementId = elementInfo.getAttribute('id');
            const formInfo = window.$(`#${elementId}`).closest('form');
            return formInfo.validate().element(`#${elementId}`);
        },
        elementByElementList: (elementList) => {
            let isValid = true;
            elementList.forEach(elementInfo => {
                const elementId = elementInfo.getAttribute('id');
                const formInfo = window.$(`#${elementId}`).closest('form');
                if (!formInfo.validate().element(`#${elementId}`)) {
                    isValid = false;
                } 
            });
            return isValid;
        }
    },
    setDropdownValue: {
        selectPickerReset: (drp) => {
            drp.selectedIndex = 0;
            const drpId = drp.getAttribute('id');
            window.$(`select[id=${drpId}]`).selectpicker('destroy').selectpicker();
        },
        selectPickerByControl: (drp, selectedValue) => {
            drp.value = selectedValue;
            const drpId = drp.getAttribute('id');
            window.$(`select[id=${drpId}]`).selectpicker('val', selectedValue);
        },
        selectPickerUpdateItems: (drp, selectItems) => {
            drp.innerHTML = selectItems;
            const drpId = drp.getAttribute('id');
            window.$(`select[id=${drpId}]`).selectpicker('destroy').selectpicker();
        }
    },
    getInputValue: {
        isValueTrue: (val) => {
            return val === 'True' || val === 'true';
        },
        isValueFalse: (val) => {
            return val === 'False' || val === 'false';
        }
    },
    getSumFromObjectArray: (arr, prop) => {
        if (arr.length === 0)
            return 0;
        return arr.map(o => o[prop]).reduce((a, c) => { return a + c });
    },
    getSumFromObjectArrayWithFilter: (arr, prop, filterProp, filterVal) => {
        const filterArr = arr.filter(a => a[filterProp] === filterVal);
        if (filterArr.length === 0)
            return 0;
        return filterArr.map(o => o[prop]).reduce((a, c) => { return a + c });
    },
    removeFromObjectArray: (arr, prop, val) => {
        return arr.filter(el => el[prop] !== val);
    },
    getRandomString: (stringLength = 6) => {
        let result = '';
        const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
        const charactersLength = characters.length;
        for (let i = 0; i < stringLength; i++) {
            result += characters.charAt(Math.floor(Math.random() *
                charactersLength));
        }
        return result;
    },
    removeElementById: (elId) => {
        const element = document.getElementById(elId);
        element.remove();
    },
    roundNumberOption: {
        byNumber: function (num, decimalPlace) {
            if (isNaN(num) || isNaN(decimalPlace)) {
                alert('Not a valid number');
                return 0;
            }
            const functioningNumber = 10 ** decimalPlace;
            return (Math.round(num * functioningNumber)) / functioningNumber;
        },
        byNumberToDisplay: function (num, decimalPlace) {
            if (isNaN(decimalPlace))
                decimalPlace = 0;
            return this.byNumber(num, decimalPlace).toFixed(decimalPlace);
        },
        byZero: function (num) {
            return this.byNumber(num, 0);
        },
        byZeroToDisplay: function (num) {
            return this.byNumberToDisplay(num, 0);
        },
        byOne: function (num) {
            return this.byNumber(num, 1);
        },
        byOneToDisplay: function (num) {
            return this.byNumberToDisplay(num, 1);
        },
        byTwo: function (num) {
            return this.byNumber(num, 2);
        },
        byTwoToDisplay: function (num) {
            return this.byNumberToDisplay(num, 2);
        },
        byThree: function (num) {
            return this.byNumber(num, 3);
        },
        byThreeToDisplay: function (num) {
            return this.byNumberToDisplay(num, 3);
        },
        byFour: function (num) {
            return this.byNumber(num, 4);
        },
        byFourToDisplay: function (num) {
            return this.byNumberToDisplay(num, 4);
        },
        byFive: function (num) {
            return this.byNumber(num, 5);
        },
        byFiveToDisplay: function (num) {
            return this.byNumberToDisplay(num, 5);
        },
        bySix: function (num) {
            return this.byNumber(num, 6);
        },
        bySixToDisplay: function (num) {
            return this.byNumberToDisplay(num, 6);
        }
    },
    displayOption: {
        hideItemByClassName: (className) => {
            const elements = document.getElementsByClassName(className);
            Array.prototype.forEach.call(elements,
                el => {
                    el.classList.add(generallyUsedCssClass.hidn);
                });
        },
        displayItemByClassName: (className) => {
            const elements = document.getElementsByClassName(className);
            Array.prototype.forEach.call(elements, el => {
                el.classList.remove(generallyUsedCssClass.hidn);
            });
        },
        hideItemById: (id) => {
            const element = document.getElementById(id);
            element.classList.add(generallyUsedCssClass.hidn);
        },
        displayItemById: (id) => {
            const element = document.getElementById(id);
            element.classList.remove(generallyUsedCssClass.hidn);
        },
        addCssClass: (el, cls) => {
            el.classList.add(cls);
        },
        addMultipleCssClass: (el, clsList) => {
            clsList.forEach(cls => {
                el.classList.add(cls);
            });
        },
        removeCssClass: (el, cls) => {
            el.classList.remove(cls);
        },
        removeMultipleCssClass: (el, clsList) => {
            clsList.forEach(cls => {
                el.classList.remove(cls);
            });
        }
    },
    getDataAttributeValue: {
        dropdownSelected: (drp, attributeName) => {
            return drp[drp.selectedIndex].getAttribute(attributeName);
        },
        dropdownSelectedText: (drp) => {
            return drp.options[drp.selectedIndex].text;
        },
        checkBoxSelected: (drp,attributeName) => {
            return drp.getAttribute(attributeName);
        }
    },
    message: {
        showErrorMsg: (msg) => {
            toastr.options.positionClass = 'toast-bottom-right';
            window.toastr.error(msg);
        },
        showWarningMsg: (msg) => {
            toastr.options.positionClass = 'toast-bottom-right';
            window.toastr.warning(msg);
        },
        showSuccessMsg: (msg) => {
            toastr.options.positionClass = 'toast-bottom-right';
            window.toastr.success(msg);
        }
    },
    alterAttr: {
        setAttr: (el, attrName, attrVal) => {
            el.setAttribute(attrName, attrVal);
        },
        removeAttr: (el, attrName) => {
            el.removeAttribute(attrName);
        }
    },
    cssClass: {
        hiddenClass: 'd-none',
        successText: 'text-success',
        warningText: 'text-warning',
        dangerText: 'text-danger'
    },
    elAttr: {
        readOnly : {
            name: 'readonly',
            value: 'readonly'
        }
    },
    ajaxFun: {
        simpleGet: async (url) => {
            const response = await fetch(url, { method: 'GET' })
                .then(r => r.json())
                .then(data => data)
                .catch(err => {
                    generalUtility.message.showErrorMsg(err);
                });
            return await response;
        }
    }
};

const printOption = {
    htmlPrintWithHtml: (sectionId, cssList) => {
        window.printJS({
            printable: sectionId,
            type: 'html',
            css: cssList,
            targetStyles: ['*'],
            scanStyles: false
        });
    }
};

var vmsPrint = {
    printA4Portrait: (sectionId) => {
        printOption.htmlPrintWithHtml(sectionId, ['/css/a4protrait.css']);
    },
    printA4Landscape: (sectionId) => {
        printOption.htmlPrintWithHtml(sectionId, ['/css/a4landscape.css']);
    },
    printA3Portrait: (sectionId) => {
        printOption.htmlPrintWithHtml(sectionId, ['/css/a3protrait.css']);
    },
    printA3Landscape: (sectionId) => {
        printOption.htmlPrintWithHtml(sectionId, ['/css/a3landscape.css']);
    },
    printLegalPortrait: (sectionId) => {
        printOption.htmlPrintWithHtml(sectionId, ['/css/legalprotrait.css']);
    },
    printLegalLandscape: (sectionId) => {
        printOption.htmlPrintWithHtml(sectionId, ['/css/legallandscape.css']);
    }
};

$(function () {
    window.$.fn.selectpicker.Constructor.BootstrapVersion = '5';
    const url = window.location.pathname;
    var urlRegExp = new RegExp(url.replace(/\/$/, '') + "$", 'i');
    window.$('a.nav-link').each(function () {
        if (urlRegExp.test(this.href.replace(/\/$/, ''), 'i')) {
            window.$(this).parent().addClass('active');
        }
    });

    window.$('.report-mushak-navbar a.nav-link').each(function () {
        if (urlRegExp.test(this.href.replace(/\/$/, ''), 'i')) {
            window.$(this).addClass('active');
        }
    });

    window.$("li.active").parent().parent().parent().children('button').removeClass("collapsed");
    window.$("li.active").parent().parent().parent().children('button').attr("aria-expanded", "true");
    window.$("li.active").parent().parent('.collapse').addClass("show");
    window.$("li.active").parent().parent(".nav-item").addClass("open active");
    window.$("li.active").parent().parent().parent().parent(".nav-item").addClass("open active");
    window.$(".nav-item.active.open .arrow ").addClass("open");

    window.$('.searchable-dropdown').selectpicker();

    window.$('input[type=text],input[type=date],input[type=datetime],input[type=datetime-local],input[type=radio],input[type=checkbox],textarea,select,input[type=email],input[type=number],input[type=file]').each(function () {
        const req = window.$(this).attr('data-val-required');
        const exclude = window.$(this).attr('data-exclude');
        if (undefined != req && undefined == exclude) {
            const label = window.$(`label[for="${window.$(this).attr('id')}"]`);
            const text = label.text();
            if (text.length > 0) {
                label.append('<span style="color:red"> *</span>');
            }
        }
    });

    window.$("[data-requiredconfirmation=true]").click(function (e) {
       const tag = this;
       const  tagname = window.$(tag).get(0).tagName.toLowerCase();
        window.$("#uxConfMessage").empty();
        window.$("#uxModalTitle").empty();
        window.$("#uxModalTitle").html(window.$(tag).text());
        if (tagname === 'a') {
            window.$("#uxConfMessage").html(`You are about to ${window.$(tag).text()} this`);
            window.$("#uxGlobalConfirmation").show();
            e.preventDefault();
            window.$("#uxModalBtnSave").click(function () {
                if (tagname === "a") {
                    const actionUrl = window.$(tag).attr("href");
                    window.location = actionUrl;
                }
            });
            window.$("button[data-dismiss='modal']").click(function () {
                window.$("#uxGlobalConfirmation").hide();
            });
        }
        else {
            window.$(tag).closest('form').data("validator").settings.submitHandler = function (form) {

                window.$("#uxConfMessage").html(`You are about to ${window.$(tag).text()} this`);
                window.$("#uxGlobalConfirmation").show();

                window.$("#uxModalBtnSave").click(function () {
                    if (tagname === "button") {
                        form.submit();
                    }
                    else if (tagname === "a") {
                        const actionUrl = window.$(tag).attr("data-url");
                        window.location = actionUrl;
                    }
                });
                window.$("button[data-dismiss='modal']").click(function () {
                    window.$("#uxGlobalConfirmation").hide();
                });

            };
        }
    });

});

var showMessage = (msg) => {
    toastr.options.positionClass = 'toast-bottom-right';
    if (msg === "Success") {
        toastr.success('Added Successfully!');
    }
    else if (msg === 'Updated') {
        toastr.success('Updated Successfully!');
    }
    else if (msg === 'Active') {
        toastr.success('Activated Successfully!');
    }
    else if (msg === 'InActive') {
        toastr.success('In Active Successfully!');
    }
    else if (msg === 'Error') {
        toastr.error('Can Not be Processed! Please Check');
    }
    else if (msg === 'NotFound') {
        toastr.error('Data not found!');
    }
    else if (msg.startsWith('Error: ')) {
        toastr.error(msg);
    }
};

function processForm(ele, arr) {
    const formId = ele.id; // <- the ID of the form
    if (formId != null) {
        const form = window.$(`#${formId}`);                       // <- the form object
        window.$(form).find('.field-validation-error span').html('');
    }

    window.$(ele).find(':input').each(function () {

        switch (this.type) {
            case 'password':
            case 'select-multiple':
            case 'select-one':
            case 'text':
            case 'datetime-local':
            case 'date':
            case 'textarea':
            case 'number':
            case 'file':
                window.$(this).val('');
                break;
            case 'checkbox':
            case 'radio':
                this.checked = false;
        }
    });
    if (arr != null) {
        arr.forEach(function (item) {
            const splitArr = item.split("+");
            if (splitArr[0] === "checkbox") {
                if (splitArr[2] === "true" || splitArr[2] === true) {
                    window.$(`#${splitArr[1]}`).prop('checked', true);
                }
                else {
                    window.$(`#${splitArr[1]}`).prop('checked', false);
                }
            }
            else if (splitArr[0] === "dropdown") {
                window.$(`#${splitArr[1]}`).val(splitArr[2]);
            }
        });
    }
}
