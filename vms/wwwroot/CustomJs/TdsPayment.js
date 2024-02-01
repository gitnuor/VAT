
const genUtil = window.generalUtility;
const listInfo = {
    tdsPayment: [],
    tdsProp: {
        purchaseId: 'purchaseId',
        paymentAmount: 'paymentAmount'
    },
}
const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmTdsInformation: document.getElementById('frmTds'),

    },
    mainFormFields: {
        drpMushakYear: document.getElementById('MushakYear'),
        drpMushakMonth: document.getElementById('MushakMonth'),
        drpCustomsAndVatcommissionarateId: document.getElementById('CustomsAndVatcommissionarateId'),
        //drpMushakReturnPaymentTypeId: document.getElementById('MushakReturnPaymentTypeId'),
        txtEconomicCode1stDisit: document.getElementById('EconomicCode1stDisit'),
        txtEconomicCode2ndDisit: document.getElementById('EconomicCode2ndDisit'),
        txtEconomicCode3rdDisit: document.getElementById('EconomicCode3rdDisit'),
        txtEconomicCode4thDisit: document.getElementById('EconomicCode4thDisit'),
        txtEconomicCode5thDisit: document.getElementById('EconomicCode5thDisit'),
        txtEconomicCode6thDisit: document.getElementById('EconomicCode6thDisit'),
        txtEconomicCode7thDisit: document.getElementById('EconomicCode7thDisit'),
        txtEconomicCode8thDisit: document.getElementById('EconomicCode8thDisit'),
        txtEconomicCode9thDisit: document.getElementById('EconomicCode9thDisit'),
        txtEconomicCode10thDisit: document.getElementById('EconomicCode10thDisit'),
        txtEconomicCode11thDisit: document.getElementById('EconomicCode11thDisit'),
        txtEconomicCode12thDisit: document.getElementById('EconomicCode12thDisit'),
        txtEconomicCode13thDisit: document.getElementById('EconomicCode13thDisit'),
        txtPaidAmount: document.getElementById('PaidAmount'),
        txtPaymentDate: document.getElementById('PaymentDate'),
        drpBankId: document.getElementById('BankId'),
        txtBankBranchName: document.getElementById('BankBranchName'),
        drpBankBranchCountryId: document.getElementById('BankBranchCountryId'),
        drpBankBranchDistrictOrCityId: document.getElementById('BankBranchDistrictOrCityId'),
    },
    tdsFormFields: {
        chqIsTds: document.getElementsByClassName("isTds"),
        chqisMainTds: document.getElementById('isMainTds')
    },
    btn: {
        btnSave: document.getElementById('btnSave'),
        btnSaveDraft: document.getElementById('btnSaveDraft'),
        btnResetForm: document.getElementById('btnResetForm')
    },
}

const dataAttributes = {
    tds: {
        paymentAmount: 'data-paidamount',
        purchaseId: 'data-purchaseid'
    },
    CusAndVatcom: {
        firstCode: 'data-Operational-Code-First-Digit',
        secondCode: 'data-Operational-Code-Second-Digit',
        thirdCode: 'data-Operational-Code-Third-Digit',
        fourthCode: 'data-Operational-Code-Fourth-Digit',
    },
    PaymentTypes: {
        firstDigit: 'data-Nbr-Economic-Code-First-Digit',
        secondDigit: 'data-Nbr-Economic-Code-Second-Digit',
        thirdDigit: 'data-Nbr-Economic-Code-Third-Digit',
        fourthDigit: 'data-Nbr-Economic-Code-Fourth-Digit',
        fifthDigit: 'data-Nbr-Economic-Code-Fifth-Digit',
        sixthDigit: 'data-Nbr-Economic-Code-Sixth-Digit',
        seventhDigit: 'data-Nbr-Economic-Code-Seventh-Digit',
        eighthDigit: 'data-Nbr-Economic-Code-Eighth-Digit',
        nighthDigit: 'data-Nbr-Economic-Code-Nighth-Digit',
        teenthDigit: 'data-Nbr-Economic-Code-Teenth-Digit',
        eleventhDigit: 'data-Nbr-Economic-Code-Eleventh-Digit',
        twelvethDigit: 'data-Nbr-Economic-Code-Twelveth-Digit',
        thirteenthDigit: 'data-Nbr-Economic-Code-Thirteenth-Digit'
    }
}

const commonUtility = {
    getTotalTds: () => {
        return generalUtility.getSumFromObjectArray(listInfo.tdsPayment, listInfo.tdsProp.paymentAmount);
    },
    showTotalTds: () => {
        elementInformation.mainFormFields.txtPaidAmount.value = commonUtility.getTotalTds();
    },
    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let tdsIndex = 0;

        listInfo.tdsPayment.forEach(d => {
            formData.append(`paymentForTdsList[${tdsIndex}].PurchaseId`, d.purchaseId);
            formData.append(`paymentForTdsList[${tdsIndex}].TdsPaidAmount`, d.paymentAmount);
            tdsIndex++;
        });
        return formData;
    },
    postFormData: data => {
        window.$.ajax({
            url: 'Create',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                console.log(d);
                window.location.href = `Index`;
            },
            error: function (d) {
                console.log(d);

            }
        });
    },
    removeArray: () => {
        listInfo.tdsPayment = [];
    }
}
const commonChangeEvents = {
    addTdsPayment: (tdspayment) => {
        listInfo.tdsPayment.push(tdspayment);
    },
    removeTdsPayment: (purchaseId) => {
        listInfo.tdsPayment = generalUtility.removeFromObjectArray(listInfo.tdsPayment, listInfo.tdsProp.purchaseId, purchaseId);
    },
}

elementInformation.btn.btnSave.addEventListener('click', () => {
    if (genUtil.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
        if (listInfo.tdsPayment.length <= 0) {
            genUtil.message.showErrorMsg("Can not save without payment!!!");
            return;
        } else {
            commonUtility.postFormData(commonUtility.getFormData());
        }

    }
});

const chkList = Array.prototype.slice.call(elementInformation.tdsFormFields.chqIsTds);

chkList.forEach(chk => {
    chk.addEventListener('click', (event) => {
        const eventTarget = event.target;
        const prodDataAttr = dataAttributes.tds;
        const purchaseId = genUtil.getDataAttributeValue.checkBoxSelected(eventTarget, prodDataAttr.purchaseId);
        const paymentAmount = genUtil.getDataAttributeValue.checkBoxSelected(eventTarget, prodDataAttr.paymentAmount);
        const tdsPayment = {
            purchaseId: purchaseId,
            paymentAmount: +(paymentAmount)
        }
        if (eventTarget.checked == true) {
            commonChangeEvents.addTdsPayment(tdsPayment);
            console.log(listInfo.tdsPayment);
        } else {
            commonChangeEvents.removeTdsPayment(purchaseId);
            console.log(listInfo.tdsPayment);
        }
        commonUtility.showTotalTds();
    });
})

const GetTdsGridDataforCheck = () => {
    commonUtility.removeArray();
    chkList.forEach(chk => {
        if (chk.type == 'checkbox')
            chk.checked = true;
        const prodDataAttr = dataAttributes.tds;
        const purchaseId = genUtil.getDataAttributeValue.checkBoxSelected(chk, prodDataAttr.purchaseId);
        const paymentAmount = genUtil.getDataAttributeValue.checkBoxSelected(chk, prodDataAttr.paymentAmount);
        const tdsPayment = {
            purchaseId: purchaseId,
            paymentAmount: +(paymentAmount)
        }
        commonChangeEvents.addTdsPayment(tdsPayment);
    })
    commonUtility.showTotalTds();
}


const GetTdsGridDataforUnCheck = () => {
    chkList.forEach(chk => {
        if (chk.type == 'checkbox')
            chk.checked = false;
        const prodDataAttr = dataAttributes.tds;
        const purchaseId = genUtil.getDataAttributeValue.checkBoxSelected(chk, prodDataAttr.purchaseId);
        commonChangeEvents.removeTdsPayment(purchaseId);
    })
    commonUtility.showTotalTds();
}

elementInformation.tdsFormFields.chqisMainTds.addEventListener('change', (event) => {
    const eventTarget = event.target;
    if (eventTarget.checked == true) {
        GetTdsGridDataforCheck();
    } else {
        GetTdsGridDataforUnCheck();
    }
});

elementInformation.mainFormFields.drpCustomsAndVatcommissionarateId.addEventListener('change', (event) => {
    const drp = event.target;
    const cusAndVatcomDataAttr = dataAttributes.CusAndVatcom;
    const mainFld = elementInformation.mainFormFields;
    mainFld.txtEconomicCode6thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, cusAndVatcomDataAttr.firstCode);
    mainFld.txtEconomicCode7thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, cusAndVatcomDataAttr.secondCode);
    mainFld.txtEconomicCode8thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, cusAndVatcomDataAttr.thirdCode);
    mainFld.txtEconomicCode9thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, cusAndVatcomDataAttr.fourthCode);
});


//elementInformation.mainFormFields.drpMushakReturnPaymentTypeId.addEventListener('change', (event) => {
//    const drp = event.target;
//    const paymentDataAttr = dataAttributes.PaymentTypes;
//    const mainFld = elementInformation.mainFormFields;
//    const eleven = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.eleventhDigit);
//    mainFld.txtEconomicCode1stDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.firstDigit);
//    mainFld.txtEconomicCode2ndDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.secondDigit);
//    mainFld.txtEconomicCode3rdDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.thirdDigit);
//    mainFld.txtEconomicCode4thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.fourthDigit);
//    mainFld.txtEconomicCode5thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.fifthDigit);
//    mainFld.txtEconomicCode10thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.teenthDigit);
//    mainFld.txtEconomicCode11thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.eleventhDigit);
//    mainFld.txtEconomicCode12thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.twelvethDigit);
//    mainFld.txtEconomicCode13thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.thirteenthDigit);
//});