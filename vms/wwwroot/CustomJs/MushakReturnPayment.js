const genUtil = window.generalUtility;
const dataAttributes = {
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


const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),

    },
    mainFormFields: {
        drpBankBranchDistrictOrCityId: document.getElementById('BankBranchDistrictOrCityId'),
        drpBankBranchCountryId: document.getElementById('BankBranchCountryId'),
        drpMushakYear: document.getElementById('MushakYear'),
        drpMushakMonth: document.getElementById('MushakMonth'),
        drpCustomsAndVatcommissionarateId: document.getElementById('CustomsAndVatcommissionarateId'),
        drpMushakReturnPaymentTypeId: document.getElementById('MushakReturnPaymentTypeId'),

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
    },
    btn: {
        btnSave: document.getElementById('btnSave'),
        btnSaveDraft: document.getElementById('btnSaveDraft'),
        btnResetForm: document.getElementById('btnResetForm')
    },
}


elementInformation.mainFormFields.drpCustomsAndVatcommissionarateId.addEventListener('change', (event) => {
    const drp = event.target;
    const cusAndVatcomDataAttr = dataAttributes.CusAndVatcom;
    const mainFld = elementInformation.mainFormFields;
    mainFld.txtEconomicCode6thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, cusAndVatcomDataAttr.firstCode);
    mainFld.txtEconomicCode7thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, cusAndVatcomDataAttr.secondCode);
    mainFld.txtEconomicCode8thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, cusAndVatcomDataAttr.thirdCode);
    mainFld.txtEconomicCode9thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, cusAndVatcomDataAttr.fourthCode);
});


elementInformation.mainFormFields.drpMushakReturnPaymentTypeId.addEventListener('change', (event) => {
    const drp = event.target;
    const paymentDataAttr = dataAttributes.PaymentTypes;
    const mainFld = elementInformation.mainFormFields;
    const eleven = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.eleventhDigit);
    mainFld.txtEconomicCode1stDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.firstDigit);
    mainFld.txtEconomicCode2ndDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.secondDigit);
    mainFld.txtEconomicCode3rdDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.thirdDigit);
    mainFld.txtEconomicCode4thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.fourthDigit);
    mainFld.txtEconomicCode5thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.fifthDigit);
    mainFld.txtEconomicCode10thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.teenthDigit);
    mainFld.txtEconomicCode11thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.eleventhDigit);
    mainFld.txtEconomicCode12thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.twelvethDigit);
    mainFld.txtEconomicCode13thDisit.value = genUtil.getDataAttributeValue.dropdownSelected(drp, paymentDataAttr.thirteenthDigit);
});


