const genUtil = window.generalUtility;
const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
        totalPrice: 'totalPrice',
        cd: 'cd',
        id: 'id',
        rd: 'rd',
        ait: 'ait',
        sd: 'sd',
        totalVat: 'totalVat',
        at: 'at',
        totalPriceIncludingAllTax: 'totalPriceIncludingAllTax'
    },
    payments: [],
    taxPayments: [],
    paymentProp: {
        paymentInfoId: 'paymentInfoId',
        paidAmount: 'paidAmount'
    },
    taxPaymentProp: {
        paymentInfoId: 'paymentInfoId',
        taxPaymentTypeId: 'taxPaymentTypeId',
        paidAmount: 'paidAmount'
    },
    taxPaymentType: {
        cd: 1,
        id: 2,
        rd: 3,
        ait: 4,
        sd: 5,
        vat: 6,
        at: 7
    },
    docs: [],
    docProp: {
        documentInfoId: 'documentInfoId'
    }
}

const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmProductInformation: document.getElementById('frmProductInformation'),
        frmPayment: document.getElementById('frmPayment'),
        frmTaxPayment: document.getElementById('frmTaxPayment'),
        frmDocument: document.getElementById('frmDocument')
    },
    mainFormFields: {
        drpOrgBranchId: document.getElementById('OrgBranchId'),
        drpPurchaseReasonId: document.getElementById('PurchaseReasonId'),
        drpVendorId: document.getElementById('VendorId'),
        txtVendorInvoiceNo: document.getElementById('VendorInvoiceNo'),
        txtLcNo: document.getElementById('LcNo'),
        dateLcDate: document.getElementById('LcDate'),
        txtBillEntry: document.getElementById('BillOfEntry'),
        dateBoeDate: document.getElementById('BoeDate'),
        dateDueDate: document.getElementById('DueDate'),
        txtLcTerm: document.getElementById('LcTerm'),
        txtPoNo: document.getElementById('PoNo'),
        dateAtpDate: document.getElementById('AtpDate'),
        drpVatCommissionarate: document.getElementById('VatCommissionarate'),
        drpAtpBankBranch: document.getElementById('AtpBankBranch'),
        txtBranch: document.getElementById('Branch'),
        hdnEconomicCodeId: document.getElementById('EconomicCodeId'),
        txtAtpChallanNo: document.getElementById('AtpChallanNo'),
        txtAdvTaxPaidAmount: document.getElementById('AdvTaxPaidAmount')
    },
    productField: {
        drpProductId: document.getElementById('ProductId'),
        txtProductDescription: document.getElementById('ProductDescription'),
        txtHSCode: document.getElementById('HSCode'),
        txtPartNo: document.getElementById('PartNo'),
        txtGoodsId: document.getElementById('GoodsId'),
        txtSKUId: document.getElementById('SKUId'),
        txtSKUNo: document.getElementById('SKUNo'),
        hdnMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        txtQuantity: document.getElementById('Quantity'),
        txtTotalPrice: document.getElementById('TotalPrice'),
        drpMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtUnitPrice: document.getElementById('UnitPrice'),
        txtSupplementaryDutyPercent: document.getElementById('SupplementaryDutyPercent'),
        txtProductSupplementaryDuty: document.getElementById('ProductSupplementaryDuty'),
        txtCustomDutyPercent: document.getElementById('CustomDutyPercent'),
        txtCustomDuty: document.getElementById('CustomDuty'),
        txtImportDutyPercent: document.getElementById('ImportDutyPercent'),
        txtImportDuty: document.getElementById('ImportDuty'),
        txtRegulatoryDutyPercent: document.getElementById('RegulatoryDutyPercent'),
        txtRegulatoryDuty: document.getElementById('RegulatoryDuty'),
        txtAdvanceTaxPercent: document.getElementById('AdvanceTaxPercent'),
        txtAdvanceTax: document.getElementById('AdvanceTax'),
        txtAdvanceIncomeTaxPercent: document.getElementById('AdvanceIncomeTaxPercent'),
        txtAdvanceIncomeTax: document.getElementById('AdvanceIncomeTax'),
//        txtVatAblePrice: document.getElementById('VatAblePrice'),
        drpProductVatTypeId: document.getElementById('ProductVatTypeId'),
        txtProductVatPercent: document.getElementById('ProductVatPercent'),
        txtProductVat: document.getElementById('ProductVat'),
        txtTotalPriceIncludingAllTax: document.getElementById('TotalPriceIncludingAllTax')
    },
    paymentField: {
        drpPaymentMethodId: document.getElementById('PaymentMethodId'),
        drpBankId: document.getElementById('BankId'),
        txtMobilePaymentWalletNo: document.getElementById('MobilePaymentWalletNo'),
        txtDocumentNoOrTransactionId: document.getElementById('DocumentNoOrTransactionId'),
        txtPaymentDocumentOrTransDate: document.getElementById('PaymentDocumentOrTransDate'),
        txtPaymentDate: document.getElementById('PaymentDate'),
        txtPaidAmount: document.getElementById('PaidAmount'),
        txtPaymentRemarks: document.getElementById('PaymentRemarks')
    },
    taxPaymentField: {
        drpPurchaseImportTaxPaymentTypeId: document.getElementById('PurchaseImportTaxPaymentTypeId'),
        drpPurchaseImportTaxPaymentVatCommissionarateId: document.getElementById(
            'PurchaseImportTaxPaymentVatCommissionarateId'),
        drpPurchaseImportTaxPaymentBankId: document.getElementById('PurchaseImportTaxPaymentBankId'),
        txtPurchaseImportTaxPaymentBankBranch: document.getElementById('PurchaseImportTaxPaymentBankBranch'),
        drpPurchaseImportTaxPaymentBankBranchDistrictId: document.getElementById(
            'PurchaseImportTaxPaymentBankBranchDistrictId'),
        txtPurchaseImportTaxPaymentAccCode: document.getElementById('PurchaseImportTaxPaymentAccCode'),
        txtPurchaseImportTaxPaymentDocOrChallanNo: document.getElementById('PurchaseImportTaxPaymentDocOrChallanNo'),
        txtPurchaseImportTaxPaymentDocOrChallanDate:
            document.getElementById('PurchaseImportTaxPaymentDocOrChallanDate'),
        txtPurchaseImportTaxPaymentDate: document.getElementById('PurchaseImportTaxPaymentDate'),
        txtPurchaseImportTaxPaymentPaidAmount: document.getElementById('PurchaseImportTaxPaymentPaidAmount'),
        txtPurchaseImportTaxPaymentRemarks: document.getElementById('PurchaseImportTaxPaymentRemarks')
    },
    documentField: {
        drpDocumentTypeId: document.getElementById('DocumentTypeId'),
        fileUploadedFile: document.getElementById('UploadedFile'),
        txtDocumentRemarks: document.getElementById('DocumentRemarks')
    },
    totalCalculatedTableCell: {
        productTotalPrice: document.getElementById('productTotalPrice'),
        productTotalCd: document.getElementById('productTotalCd'),
        productTotalId: document.getElementById('productTotalId'),
        productTotalRd: document.getElementById('productTotalRd'),
        productTotalAit: document.getElementById('productTotalAit'),
        productTotalSd: document.getElementById('productTotalSd'),
        productTotalVat: document.getElementById('productTotalVat'),
        productTotalAt: document.getElementById('productTotalAt'),
        productTotalPriceIncAllTax: document.getElementById('productTotalPriceIncAllTax')
    },
    totalPaidCalculatedTableCell: {
        paymentTotalPaid: document.getElementById('paymentTotalPaid')
    },
    totalTaxPaidCalculatedTableCell: {
        paymentTotalPaid: document.getElementById('importTaxPaymentTotalPaid')
    },
    listTables: {
        product: document.getElementById('productTable'),
        productBody: document.getElementById('productTableBody'),
        payment: document.getElementById('paymentTable'),
        paymentBody: document.getElementById('paymentTableBody'),
        taxPayment: document.getElementById('taxPaymentTable'),
        taxPaymentBody: document.getElementById('taxPaymentTableBody'),
        doc: document.getElementById('docTable'),
        docBody: document.getElementById('docTableBody')
    },
    btn: {
        btnAddProduct: document.getElementById('btnAddProduct'),
        btnResetProduct: document.getElementById('btnResetProduct'),
        btnAddPayment: document.getElementById('btnAddPayment'),
        btnAddTaxPayment: document.getElementById('btnAddTaxPayment'),
        btnAddDocument: document.getElementById('btnAddDocument'),
        btnSave: document.getElementById('btnSave'),
        btnSaveDraft: document.getElementById('btnSaveDraft'),
        btnResetForm: document.getElementById('btnResetForm')
    },
    otherBlock: {
        dueAmount: document.getElementById('dueAmount'),
        notificationArea: document.getElementById('notificationArea')
    }
}

const priceCalculation = {
    calculatePrice: (unitPrice, qty) => {
        return window.generalUtility.roundNumberOption.byFour(unitPrice * qty);
    },
    calculateUnitPrice: (price, qty) => {
        if (qty === 0)
            throw new exception('Quantity 0 not allowed!');
        return window.generalUtility.roundNumberOption.byFour(price / qty);
    },
    calculateCdPercent: (price, cd) => {
        if (price === 0)
            throw new exception('Price 0 not allowed!');
        return window.generalUtility.roundNumberOption.byFour(cd * 100 / price);
    },
    calculateCd: (price, cdPercent) => {
        return window.generalUtility.roundNumberOption.byFour(price * cdPercent / 100);
    },
    calculateIdPercent: (price, id) => {
        if (price === 0)
            throw new exception('Price 0 not allowed!');
        return window.generalUtility.roundNumberOption.byFour(id * 100 / price);
    },
    calculateId: (price, idPercent) => {
        return window.generalUtility.roundNumberOption.byFour(price * idPercent / 100);
    },
    calculateRdPercent: (price, rd) => {
        if (price === 0)
            throw new exception('Price 0 not allowed!');
        return window.generalUtility.roundNumberOption.byFour(rd * 100 / price);
    },
    calculateRd: (price, rdPercent) => {
        return window.generalUtility.roundNumberOption.byFour(price * rdPercent / 100);
    },
    calculateSdImposablePrice: (price, id, cd, rd) => {
        return window.generalUtility.roundNumberOption.byFour(price + id + cd + rd);
    },
    calculateSdPercent: (sdImposableValue, sd) => {
        if (sdImposableValue === 0)
            throw new exception('Price 0 not allowed!');
        return window.generalUtility.roundNumberOption.byFour(sd * 100 / sdImposableValue);
    },
    calculateSd: (sdImposableValue, sdPercent) => {
        return window.generalUtility.roundNumberOption.byFour(sdImposableValue * sdPercent / 100);
    },
    calculateAitPercent: (aitImposableValue, ait) => {
        if (aitImposableValue === 0)
            throw new exception('Price 0 not allowed!');
        return window.generalUtility.roundNumberOption.byFour(ait * 100 / aitImposableValue);
    },
    calculateAit: (aitImposableValue, aitPercent) => {
        return window.generalUtility.roundNumberOption.byFour(aitImposableValue * aitPercent / 100);
    },
    calculateVatablePrice: (price, id, cd, sd, rd) => {
        return window.generalUtility.roundNumberOption.byFour(price + id + cd + sd + rd);
    },
    calculateVatPercent: (vatImposableValue, vat) => {
        if (vatImposableValue === 0)
            throw new exception('Price 0 not allowed!');
        return window.generalUtility.roundNumberOption.byFour(vat * 100 / vatImposableValue);
    },
    calculateVat: (vatImposableValue, vatPercent) => {
        return window.generalUtility.roundNumberOption.byFour(vatImposableValue * vatPercent / 100);
    },
    calculatePriceWithVat: (vatImposableValue, vat) => {
        return window.generalUtility.roundNumberOption.byFour(vatImposableValue + vat);
    },
    calculateAdvanceTaxImposableValue: (price, id, cd, sd, rd) => {
        return window.generalUtility.roundNumberOption.byFour(price + id + cd + sd + rd);
    },
    calculateAdvanceTax: (advanceTaxImposableValue, atPercent) => {
        return window.generalUtility.roundNumberOption.byFour(advanceTaxImposableValue * atPercent / 100);
    },
    calculatePriceIncludingAllTax: (price, cd, id, rd, sd, vat, at, ait) => {
        return window.generalUtility.roundNumberOption.byFour(price + cd + id + rd + sd + vat + at + ait);
    }
}

const changeEffect = {
    priceChange: (totalPrice) => {
        const cd = priceCalculation.calculateCd(totalPrice,
            +(elementInformation.productField.txtCustomDutyPercent.value));
        elementInformation.productField.txtCustomDuty.value = cd;

        const rd = priceCalculation.calculateRd(totalPrice,
            +(elementInformation.productField.txtRegulatoryDutyPercent.value));
        elementInformation.productField.txtRegulatoryDuty.value = rd;

        const sd = priceCalculation.calculateSd(totalPrice,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value = sd;

        const ait = priceCalculation.calculateAit(totalPrice,
            +(elementInformation.productField.txtAdvanceIncomeTaxPercent.value));
        elementInformation.productField.txtAdvanceIncomeTax.value = ait;

        const vatablePrice = priceCalculation.calculateVatablePrice(totalPrice, cd, sd, rd);
//        elementInformation.productField.txtVatAblePrice.value = vatablePrice;

        const vat = priceCalculation.calculateVat(vatablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = vat;

        const priceWithVat = priceCalculation.calculatePriceIncludingAllTax(vatablePrice, vat, ait);
        elementInformation.productField.txtTotalPriceIncludingAllTax.value = priceWithVat;
    },
    taxChange: () => {
        const totalPrice = +elementInformation.productField.txtTotalPrice.value;

        const ait = priceCalculation.calculateAit(totalPrice,
            +(elementInformation.productField.txtAdvanceIncomeTaxPercent.value));
        elementInformation.productField.txtAdvanceIncomeTax.value = ait;

        const cd = priceCalculation.calculateCd(totalPrice,
            +(elementInformation.productField.txtCustomDutyPercent.value));
        elementInformation.productField.txtCustomDuty.value = cd;

        const id = priceCalculation.calculateId(totalPrice,
            +(elementInformation.productField.txtImportDutyPercent.value));
        elementInformation.productField.txtImportDuty.value = id;

        const rd = priceCalculation.calculateRd(totalPrice,
            +(elementInformation.productField.txtRegulatoryDutyPercent.value));
        elementInformation.productField.txtRegulatoryDuty.value = rd;

        const sdImposableValue = priceCalculation.calculateSdImposablePrice(totalPrice, id, cd, rd);

        const sd = priceCalculation.calculateSd(sdImposableValue,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value = sd;

        const vatImposablePrice = priceCalculation.calculateVatablePrice(totalPrice, id, cd, sd, rd);
//        elementInformation.productField.txtVatAblePrice.value = vatImposablePrice;

        const vat = priceCalculation.calculateVat(vatImposablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = vat;

        const atImposablePrice = priceCalculation.calculateAdvanceTaxImposableValue(totalPrice, id, cd, sd, rd);
        elementInformation.productField.txtAdvanceTax.value = atImposablePrice;

        const at = priceCalculation.calculateAdvanceTax(atImposablePrice,
            +(elementInformation.productField.txtAdvanceTaxPercent.value));
        elementInformation.productField.txtProductVat.value = at;

        const priceWithVat = priceCalculation.calculatePriceIncludingAllTax(vatImposablePrice, vat, ait);
        elementInformation.productField.txtTotalPriceIncludingAllTax.value = priceWithVat;
    },
    productValueOrTaxChange: () => {
        const totalPrice = +elementInformation.productField.txtTotalPrice.value;

        const ait = priceCalculation.calculateAit(totalPrice,
            +(elementInformation.productField.txtAdvanceIncomeTaxPercent.value));
        elementInformation.productField.txtAdvanceIncomeTax.value = window.generalUtility.roundNumberOption.byFour(ait);

        const cd = priceCalculation.calculateCd(totalPrice,
            +(elementInformation.productField.txtCustomDutyPercent.value));
        elementInformation.productField.txtCustomDuty.value = window.generalUtility.roundNumberOption.byFour(cd);

        const id = priceCalculation.calculateId(totalPrice,
            +(elementInformation.productField.txtImportDutyPercent.value));
        elementInformation.productField.txtImportDuty.value = window.generalUtility.roundNumberOption.byFour(id);;

        const rd = priceCalculation.calculateRd(totalPrice,
            +(elementInformation.productField.txtRegulatoryDutyPercent.value));
        elementInformation.productField.txtRegulatoryDuty.value = window.generalUtility.roundNumberOption.byFour(rd);

        const sdImposableValue = priceCalculation.calculateSdImposablePrice(totalPrice, id, cd, rd);

        const sd = priceCalculation.calculateSd(sdImposableValue,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value =
            window.generalUtility.roundNumberOption.byFour(sd);

        const vatImposablePrice = priceCalculation.calculateVatablePrice(totalPrice, id, cd, sd, rd);
        //elementInformation.productField.txtVatAblePrice.value = vatImposablePrice;

        const vat = priceCalculation.calculateVat(vatImposablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = window.generalUtility.roundNumberOption.byFour(vat);

        const atImposablePrice = priceCalculation.calculateAdvanceTaxImposableValue(totalPrice, id, cd, sd, rd);
        //elementInformation.productField.txtAdvanceTax.value = atImposablePrice;

        const at = priceCalculation.calculateAdvanceTax(atImposablePrice,
            +(elementInformation.productField.txtAdvanceTaxPercent.value));
        elementInformation.productField.txtAdvanceTax.value = window.generalUtility.roundNumberOption.byFour(at);

        const priceIncludingAllTax =
            priceCalculation.calculatePriceIncludingAllTax(totalPrice, cd, id, rd, sd, vat, at, ait);
        elementInformation.productField.txtTotalPriceIncludingAllTax.value =
            window.generalUtility.roundNumberOption.byFour(priceIncludingAllTax);
    },
    notifyForNotifiablePriceChange: (productId, productName, unitPrice) => {
        console.log(window.mainUrls.product);
        window.$.ajax({
            url: `${window.mainUrls.product}/GetNumberOfFinishedProductsWithNotifiableChange/?productId=${productId
                }&unitPrice=${unitPrice}`,
            cache: false,
            method: 'GET',
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function(d) {
                if (d.totalItem > 0) {
                    elementInformation.otherBlock.notificationArea.insertAdjacentHTML('afterend',
                        `<div class="alert alert-warning price-alert" role="alert">Price of ${productName
                        } has been changed more than 7.5% from previous 1 products Input-Output co-efficient. Input-Output Co-efficient should be declare again.</div>`);
                }
            }
        });
    }
}

const commonChangeEventsForCalculation = {
    removePaymentEvent: () => {
        listInfo.payments = [];
        elementInformation.listTables.paymentBody.innerHTML = '';
        commonUtilityPurchase.showDueAmount();
        commonUtilityPurchase.showTotalPaidAmount();
    },
    unitAndQtyChangeEvent: (unitPrice, qty) => {
        const totalPrice = priceCalculation.calculatePrice(unitPrice, qty);
        elementInformation.productField.txtTotalPrice.value = totalPrice;

        changeEffect.productValueOrTaxChange();
    },
    totalPriceChangeEvent: (totalPrice) => {
        const qty = +elementInformation.productField.txtQuantity.value;
        const unitPrice = priceCalculation.calculateUnitPrice(totalPrice, qty);
        elementInformation.productField.txtUnitPrice.value = unitPrice;

        changeEffect.productValueOrTaxChange();
    },
    cdPercentChangeEvent: () => {
        changeEffect.productValueOrTaxChange();
    },
    sdPercentChangeEvent: () => {
        changeEffect.productValueOrTaxChange();
    },
    rdPercentChangeEvent: () => {
        changeEffect.productValueOrTaxChange();
    },
    idPercentChangeEvent: () => {
        changeEffect.productValueOrTaxChange();
    },
    vatPercentChangeEvent: () => {
        changeEffect.productValueOrTaxChange();
    },
    aitPercentChangeEvent: () => {
        changeEffect.productValueOrTaxChange();
    },
    atPercentChangeEvent: () => {
        changeEffect.productValueOrTaxChange();
    },
    clearPriceEvent: () => {
        elementInformation.productField.txtUnitPrice.value = 0;
        elementInformation.productField.txtQuantity.value = 0;
        elementInformation.productField.txtTotalPrice.value = 0;
        elementInformation.productField.txtProductSupplementaryDuty.value = 0;
//        elementInformation.productField.txtVatAblePrice.value = 0;
        elementInformation.productField.txtProductVat.value = 0;
        elementInformation.productField.txtTotalPriceIncludingAllTax.value = 0;
    },
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmProductInformation.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductId);
            window.generalUtility.setDropdownValue.selectPickerReset(
                elementInformation.productField.drpProductVatTypeId);
        },
        payment: () => {
            elementInformation.formInfo.frmPayment.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.paymentField
                .drpPaymentMethodId);
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.paymentField.drpBankId);
        },
        taxPayment: () => {
            elementInformation.formInfo.frmTaxPayment.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.taxPaymentField
                .drpPurchaseImportTaxPaymentBankId);
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.taxPaymentField
                .drpPurchaseImportTaxPaymentBankBranchDistrictId);
        },
        doc: () => {
            elementInformation.formInfo.frmDocument.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.documentField
                .drpDocumentTypeId);
        }
    }
}

const specialItem = {
    classesToShowHideInEvents: {
        bankPayment: 'payment-bank',
        mobilePayment: 'payment-mobile'
    },
    classesOfDueAmount: [
        window.generallyUsedCssClass.successText, window.generallyUsedCssClass.warningText,
        window.generallyUsedCssClass.dangerText
    ],
    elementToValidate: {
        productChange: [
            elementInformation.productField.drpProductId, elementInformation.productField.drpProductVatTypeId
        ]
    }
}

specialItem.classesToShowHideInEvents.paymentRelatedList =
    [specialItem.classesToShowHideInEvents.bankPayment, specialItem.classesToShowHideInEvents.mobilePayment];

const classesToShowHideInEvents = {
    bankPayment: 'payment-bank',
    mobilePayment: 'payment-mobile'
}

const commonChangeEventsForDisplay = {
    hidePaymentSpecialOptions: () => {
        specialItem.classesToShowHideInEvents.paymentRelatedList.forEach(element => {
            window.generalUtility.displayOption.hideItemByClassName(element);
        });
    },
    displayPaymentBank: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(classesToShowHideInEvents.bankPayment);
    },
    displayPaymentMobile: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(classesToShowHideInEvents.mobilePayment);
    }
}

const commonUtilityPurchase = {
    getTotalPrice: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPrice);
    },
    getTotalCd: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.cd);
    },
    getTotalPaidCd: () => {
        return generalUtility.getSumFromObjectArrayWithFilter(listInfo.taxPayments,
            listInfo.taxPaymentProp.paidAmount,
            listInfo.taxPaymentProp.taxPaymentTypeId,
            listInfo.taxPaymentType.cd);
    },
    getTotalId: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.id);
    },
    getTotalPaidId: () => {
        return generalUtility.getSumFromObjectArrayWithFilter(listInfo.taxPayments,
            listInfo.taxPaymentProp.paidAmount,
            listInfo.taxPaymentProp.taxPaymentTypeId,
            listInfo.taxPaymentType.id);
    },
    getTotalRd: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.rd);
    },
    getTotalPaidRd: () => {
        return generalUtility.getSumFromObjectArrayWithFilter(listInfo.taxPayments,
            listInfo.taxPaymentProp.paidAmount,
            listInfo.taxPaymentProp.taxPaymentTypeId,
            listInfo.taxPaymentType.rd);
    },
    getTotalAit: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.ait);
    },
    getTotalPaidAit: () => {
        return generalUtility.getSumFromObjectArrayWithFilter(listInfo.taxPayments,
            listInfo.taxPaymentProp.paidAmount,
            listInfo.taxPaymentProp.taxPaymentTypeId,
            listInfo.taxPaymentType.ait);
    },
    getTotalSd: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.sd);
    },
    getTotalPaidSd: () => {
        return generalUtility.getSumFromObjectArrayWithFilter(listInfo.taxPayments,
            listInfo.taxPaymentProp.paidAmount,
            listInfo.taxPaymentProp.taxPaymentTypeId,
            listInfo.taxPaymentType.sd);
    },
    getTotalVat: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalVat);
    },
    getTotalPaidVat: () => {
        return generalUtility.getSumFromObjectArrayWithFilter(listInfo.taxPayments,
            listInfo.taxPaymentProp.paidAmount,
            listInfo.taxPaymentProp.taxPaymentTypeId,
            listInfo.taxPaymentType.vat);
    },
    getTotalAt: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.at);
    },
    getTotalPaidAt: () => {
        return generalUtility.getSumFromObjectArrayWithFilter(listInfo.taxPayments,
            listInfo.taxPaymentProp.paidAmount,
            listInfo.taxPaymentProp.taxPaymentTypeId,
            listInfo.taxPaymentType.at);
    },
    getTotalTaxByType: (typeId) => {
        if (typeId === listInfo.taxPaymentType.cd) {
            return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.cd);
        }
        if (typeId === listInfo.taxPaymentType.id) {
            return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.id);
        }
        if (typeId === listInfo.taxPaymentType.rd) {
            return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.rd);
        }
        if (typeId === listInfo.taxPaymentType.ait) {
            return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.ait);
        }
        if (typeId === listInfo.taxPaymentType.sd) {
            return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.sd);
        }
        if (typeId === listInfo.taxPaymentType.vat) {
            return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalVat);
        }
        if (typeId === listInfo.taxPaymentType.at) {
            return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.at);
        }

        return 0;
    },
    getTotalPaidTaxByType: (typeId) => {
        return generalUtility.getSumFromObjectArrayWithFilter(listInfo.taxPayments,
            listInfo.taxPaymentProp.paidAmount,
            listInfo.taxPaymentProp.taxPaymentTypeId,
            typeId);
    },
    getTotalTotalPriceIncAllTax: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPriceIncludingAllTax);
    },
    getTotalPaidAmount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.payments, listInfo.paymentProp.paidAmount);
    },
    getTotalTaxPaidAmount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.taxPayments, listInfo.paymentProp.paidAmount);
    },
    getTotalPayableAmount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPrice);
    },
    removeProduct: (infoId) => {
        listInfo.products =
            generalUtility.removeFromObjectArray(listInfo.products, listInfo.productProp.productInfoId, infoId);
        commonUtilityPurchase.showTotalAmount();
        commonUtilityPurchase.showDueAmount();
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    removePayment: (infoId) => {
        listInfo.payments =
            generalUtility.removeFromObjectArray(listInfo.payments, listInfo.paymentProp.paymentInfoId, infoId);
        commonUtilityPurchase.showTotalPaidAmount();
        commonUtilityPurchase.showDueAmount();
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    removeTaxPayment: (infoId) => {
        listInfo.taxPayments =
            generalUtility.removeFromObjectArray(listInfo.taxPayments, listInfo.taxPaymentProp.paymentInfoId, infoId);
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    removeDoc: (infoId) => {
        listInfo.docs = generalUtility.removeFromObjectArray(listInfo.docs, listInfo.docProp.documentInfoId, infoId);
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    showTotalAmount: () => {
        elementInformation.totalCalculatedTableCell.productTotalPrice.innerText = commonUtilityPurchase.getTotalPrice();
        elementInformation.totalCalculatedTableCell.productTotalCd.innerText = commonUtilityPurchase.getTotalCd();
        elementInformation.totalCalculatedTableCell.productTotalId.innerText = commonUtilityPurchase.getTotalId();
        elementInformation.totalCalculatedTableCell.productTotalRd.innerText = commonUtilityPurchase.getTotalRd();
        elementInformation.totalCalculatedTableCell.productTotalAit.innerText = commonUtilityPurchase.getTotalAit();
        elementInformation.totalCalculatedTableCell.productTotalSd.innerText = commonUtilityPurchase.getTotalSd();
        elementInformation.totalCalculatedTableCell.productTotalVat.innerText = commonUtilityPurchase.getTotalVat();
        elementInformation.totalCalculatedTableCell.productTotalAt.innerText = commonUtilityPurchase.getTotalAt();
        elementInformation.totalCalculatedTableCell.productTotalPriceIncAllTax.innerText =
            new Intl.NumberFormat('en-IN').format(commonUtilityPurchase.getTotalTotalPriceIncAllTax());
    },
    showTotalPaidAmount: () => {
        /*Change here*/
        elementInformation.totalPaidCalculatedTableCell.paymentTotalPaid.innerText =
            new Intl.NumberFormat('en-IN').format(commonUtilityPurchase.getTotalPaidAmount());
    },
    showDueAmount: () => {
        const dueBlock = elementInformation.otherBlock.dueAmount;
        window.generalUtility.displayOption.removeMultipleCssClass(dueBlock, specialItem.classesOfDueAmount);
        const payableAmount = commonUtilityPurchase.getTotalPayableAmount();
        const dueAmount = payableAmount - commonUtilityPurchase.getTotalPaidAmount();
        if (dueAmount < 0) {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.dangerText);
        } else if (dueAmount > 0) {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.warningText);
        } else {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.successText);
        }

        elementInformation.otherBlock.dueAmount.innerText =
            `Payable Amount: ${payableAmount}; Due amount: ${dueAmount}`;
        elementInformation.paymentField.txtPaidAmount.value = dueAmount;
    },
    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let docIndex = 0;
        let productIndex = 0;
        let paymentIndex = 0;
        let taxPaymentIndex = 0;
        listInfo.products.forEach(d => {
            //console.log(productIndex);
            formData.append(`Products[${productIndex}].ProductId`, d.productId);
            formData.append(`Products[${productIndex}].ProductDescription`, d.description);
            formData.append(`Products[${productIndex}].HSCode`, d.hsCode);
            formData.append(`Products[${productIndex}].PartNo`, d.partNo);
            formData.append(`Products[${productIndex}].GoodsId`, d.goodsId);
            formData.append(`Products[${productIndex}].SKUId`, d.sKUId);
            formData.append(`Products[${productIndex}].SKUNo`, d.sKUNo);
            formData.append(`Products[${productIndex}].MeasurementUnitId`, d.measurementUnitId);
            formData.append(`Products[${productIndex}].MeasurementUnitName`, d.measurementUnitName);
            formData.append(`Products[${productIndex}].Quantity`, d.quantity);
            formData.append(`Products[${productIndex}].UnitPrice`, d.unitPrice);
            formData.append(`Products[${productIndex}].CustomDutyPercent`, d.cdPercent);
            formData.append(`Products[${productIndex}].ImportDutyPercent`, d.idPercent);
            formData.append(`Products[${productIndex}].RegulatoryDutyPercent`, d.rdPercent);
            formData.append(`Products[${productIndex}].AdvanceIncomeTaxPercent`, d.aitPercent);
            formData.append(`Products[${productIndex}].SupplementaryDutyPercent`, d.sdPercent);
            formData.append(`Products[${productIndex}].ProductSupplementaryDuty`, d.sdValue);
            formData.append(`Products[${productIndex}].TotalPrice`, d.totalPrice);
            formData.append(`Products[${productIndex}].ProductVatTypeId`, d.vatTypeId);
            formData.append(`Products[${productIndex}].ProductVatPercent`, d.vatPercent);
            formData.append(`Products[${productIndex}].ProductVat`, d.totalVat);
            formData.append(`Products[${productIndex}].AdvanceTaxPercent`, d.atPercent);
            formData.append(`Products[${productIndex}].ProductPriceWithVat`, d.totalPriceWithVat);
            productIndex++;
        });
        listInfo.payments.forEach(d => {
            formData.append(`Payments[${paymentIndex}].PaymentMethodId`, d.paymentMethodId);
            formData.append(`Payments[${paymentIndex}].IsBankingChannel`, d.isBankingChannel);
            formData.append(`Payments[${paymentIndex}].IsMobileTransaction`, d.isMobileTransaction);
            formData.append(`Payments[${paymentIndex}].BankId`, d.bankInfoId);
            formData.append(`Payments[${paymentIndex}].MobilePaymentWalletNo`, d.mobilePaymentWalletNo);
            formData.append(`Payments[${paymentIndex}].PaidAmount`, d.paidAmount);
            formData.append(`Payments[${paymentIndex}].PaymentDate`, d.paymentDate);
            formData.append(`Payments[${paymentIndex}].DocumentNoOrTransactionId`, d.documentNoOrTransactionId);
            formData.append(`Payments[${paymentIndex}].PaymentDocumentOrTransDate`, d.paymentDocumentOrTransDate);
            formData.append(`Payments[${paymentIndex}].PaymentRemarks`, d.paymentRemarks);
            paymentIndex++;
        });
        listInfo.taxPayments.forEach(d => {
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentTypeId`, d.taxPaymentTypeId);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentTypeName`, d.taxPaymentTypeName);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentVatCommissionerateId`,
                d.paymentVatCommissionarateId);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentVatCommissionerateName`,
                d.paymentVatCommissionarateName);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentBankId`, d.paymentBankId);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentBankName`, d.paymentBankName);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentBankBranchName`, d.paymentBankBranchName);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentBankBranchDistrictId`,
                d.paymentBankBranchDistId);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentBankBranchDistrictName`,
                d.paymentBankBranchDistName);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentAccCode`, d.paymentAccCode);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentDocOrChallanNo`, d.paymentDocOrChallanNo);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentDocOrChallanDate`,
                d.paymentDocOrChallanDate);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentPaymentDate`, d.paymentDate);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentPaidAmount`, d.paidAmount);
            formData.append(`ImportTaxPayments[${taxPaymentIndex}].TaxPaymentRemarks`, d.paymentRemarks);
            taxPaymentIndex++;
        });
        listInfo.docs.forEach(d => {
            formData.append(`Documents[${docIndex}].DocumentTypeId`, d.documentTypeId);
            formData.append(`Documents[${docIndex}].UploadedFile`, d.uploadedDocument);
            formData.append(`Documents[${docIndex}].DocumentRemarks`, d.documentRemarks);
            docIndex++;
        });
        return formData;
    },
    postFormData: data => {
        window.$.ajax({
            url: 'PurchaseImport',
            data: data,
            cache: false,
            method: 'POST',
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function(d) {
                console.log(d);
                console.log('ss');
                window.location.href = `Details/${d.id}`;
            },
            error: function(d) {
                console.log(d);
                console.log('tt');
                generalUtility.alterAttr.removeAttr(elementInformation.btn.btnSave, 'disabled');
            }
        });
    }
}

const dataAttributes = {
    payment: {
        isBankingChannel: 'data-is-banking-channel',
        isMobileTransaction: 'data-is-mobile-transaction'
    },
    product: {
        measurementUnitId: 'data-mu-id',
        measurementUnitName: 'data-mu-name',
        isVatUpdatable: 'data-is-vat-updatable',
        vatTypeId: 'data-vat-type-id',
        hsCode: 'data-hs-code',
        vatPercent: 'data-vat-percent',
        sdPercent: 'data-sd-percent',
        idPercent: 'data-id-percent',
        cdPercent: 'data-cd-percent',
        rdPercent: 'data-rd-percent',
        atPercent: 'data-at-percent',
        aitPercent: 'data-ait-percent'
    },
    vechile: {
        vechileIsRequireRegistration: 'data-Is-Require-Registration'
    }
}

//Product change event
elementInformation.productField.drpProductId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.product;

        elementInformation.productField.hdnMeasurementUnitId.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);

        elementInformation.productField.txtMeasurementUnitName.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitName);

        window.generalUtility.setDropdownValue.selectPickerByControl(
            elementInformation.productField.drpProductVatTypeId,
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatTypeId));

        const isVatUpdatable =
            window.generalUtility.getDataAttributeValue.dropdownSelected(
                elementInformation.productField.drpProductVatTypeId,
                prodDataAttr.isVatUpdatable);
        if (isVatUpdatable === 'True') {
            window.generalUtility.alterAttr.removeAttr(elementInformation.productField.txtProductVatPercent,
                'readonly');
        } else {
            window.generalUtility.alterAttr.setAttr(elementInformation.productField.txtProductVatPercent,
                'readonly',
                'readonly');
        }

        elementInformation.productField.txtProductVatPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatPercent);

        elementInformation.productField.txtSupplementaryDutyPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.sdPercent);

        elementInformation.productField.txtImportDutyPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.idPercent);

        elementInformation.productField.txtCustomDutyPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.cdPercent);

        elementInformation.productField.txtRegulatoryDutyPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.rdPercent);

        elementInformation.productField.txtAdvanceIncomeTaxPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.aitPercent);

        elementInformation.productField.txtAdvanceTaxPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.atPercent);

        elementInformation.productField.txtHSCode.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.hsCode);

        commonChangeEventsForCalculation.clearPriceEvent();
        window.generalUtility.validateElement.elementByElementList(specialItem.elementToValidate.productChange);
    });

//LC date change event
elementInformation.mainFormFields.dateLcDate.addEventListener('change',
    (event) => {
        const dateLcDate = event.target;
        const lcDate = dateLcDate.value;
        let boeDate = new Date();
        boeDate.setDate((new Date(lcDate)).getDate() + 90);
        elementInformation.mainFormFields.dateBoeDate.value = boeDate.toISOString().slice(0, 10);;

    });

//Unit Price
elementInformation.productField.txtUnitPrice.addEventListener('input',
    (event) => {
        const unitPrice = +event.target.value;
        const qty = +(elementInformation.productField.txtQuantity.value);
        commonChangeEventsForCalculation.unitAndQtyChangeEvent(unitPrice, qty);
    });

//Quantity
elementInformation.productField.txtQuantity.addEventListener('input',
    (event) => {
        const unitPrice = +(elementInformation.productField.txtUnitPrice.value);
        const qty = +event.target.value;
        commonChangeEventsForCalculation.unitAndQtyChangeEvent(unitPrice, qty);
    });

//Total Price
elementInformation.productField.txtTotalPrice.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.totalPriceChangeEvent(+event.target.value);
    });

//CD Percent
elementInformation.productField.txtCustomDutyPercent.addEventListener('input',
    () => {
        commonChangeEventsForCalculation.cdPercentChangeEvent();
    });

//SD Percent
elementInformation.productField.txtSupplementaryDutyPercent.addEventListener('input',
    () => {
        commonChangeEventsForCalculation.sdPercentChangeEvent();
    });

//RD Percent
elementInformation.productField.txtRegulatoryDutyPercent.addEventListener('input',
    () => {
        commonChangeEventsForCalculation.rdPercentChangeEvent();
    });

//ID Percent
elementInformation.productField.txtImportDutyPercent.addEventListener('input',
    () => {
        commonChangeEventsForCalculation.idPercentChangeEvent();
    });

//AIT Percent
elementInformation.productField.txtAdvanceIncomeTaxPercent.addEventListener('input',
    () => {
        commonChangeEventsForCalculation.aitPercentChangeEvent();
    });

//AT Percent
elementInformation.productField.txtAdvanceTaxPercent.addEventListener('input',
    () => {
        commonChangeEventsForCalculation.atPercentChangeEvent();
    });

//VAT Percent
elementInformation.productField.txtProductVatPercent.addEventListener('input',
    () => {
        commonChangeEventsForCalculation.vatPercentChangeEvent();
    });

//Vat type change event
elementInformation.productField.drpProductVatTypeId.addEventListener('change',
    (event) => {
        const drp = event.target;

        const prodDataAttr = dataAttributes.product;

        const defaultVatPercent =
            +window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatPercent);
        elementInformation.productField.txtProductVatPercent.value = defaultVatPercent.toFixed(2);
        const isVatUpdatable =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.isVatUpdatable);

        if (isVatUpdatable === 'True') {
            window.generalUtility.alterAttr.removeAttr(elementInformation.productField.txtProductVatPercent,
                'readonly');
        } else {
            window.generalUtility.alterAttr.setAttr(elementInformation.productField.txtProductVatPercent,
                'readonly',
                'readonly');
        }
    });

//Payment method change
elementInformation.paymentField.drpPaymentMethodId.addEventListener('change',
    (event) => {
        const isBankingChannel =
            window.generalUtility.getDataAttributeValue.dropdownSelected(event.target,
                dataAttributes.payment.isBankingChannel);
        const isMobileTransaction =
            window.generalUtility.getDataAttributeValue.dropdownSelected(event.target,
                dataAttributes.payment.isMobileTransaction);
        if (generalUtility.getInputValue.isValueTrue(isBankingChannel)) {
            commonChangeEventsForDisplay.displayPaymentBank();
        } else if (generalUtility.getInputValue.isValueTrue(isMobileTransaction)) {
            commonChangeEventsForDisplay.displayPaymentMobile();
        } else {
            commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        }
    });

//Add Product
elementInformation.btn.btnAddProduct.addEventListener('click',
    () => {
        if (!isProductExit(elementInformation.productField.drpProductId.value)) {
            if (window.$('#frmProductInformation').valid())
                addRelatedProducts();
        } else {
            toastr.error('Product Already Exist');
        }
    });

//Add Payment
elementInformation.btn.btnAddPayment.addEventListener('click',
    () => {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmPayment)) {
            const dueAmount = commonUtilityPurchase.getTotalPayableAmount() -
                commonUtilityPurchase.getTotalPaidAmount();
            const paidAmount = +elementInformation.paymentField.txtPaidAmount.value;
            if (paidAmount > dueAmount) {
                window.generalUtility.message.showErrorMsg(`Paid amount should not be greater than ${dueAmount}!`);
                return;
            }

            const drpBankId = elementInformation.paymentField.drpBankId;
            const drpPaymentMethodId = elementInformation.paymentField.drpPaymentMethodId;
            let bankInfoId = null;
            let bankName = null;
            let paymentDocumentOrTransDate = null;
            let bankNameOrWalletDisplayValue = '';

            const isBankingChannel =
                generalUtility.getInputValue.isValueTrue(
                    window.generalUtility.getDataAttributeValue.dropdownSelected(drpPaymentMethodId,
                        dataAttributes.payment.isBankingChannel));
            const isMobileTransaction =
                generalUtility.getInputValue.isValueTrue(
                    window.generalUtility.getDataAttributeValue.dropdownSelected(drpPaymentMethodId,
                        dataAttributes.payment.isMobileTransaction));
            if (isBankingChannel) {
                bankInfoId = drpBankId.value;
                bankName = window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation
                    .paymentField
                    .drpBankId);
                paymentDocumentOrTransDate = elementInformation.paymentField.txtPaymentDocumentOrTransDate.value;
                bankNameOrWalletDisplayValue = bankName;
            } else if (isMobileTransaction) {
                paymentDocumentOrTransDate = elementInformation.paymentField.txtPaymentDocumentOrTransDate.value;
                bankNameOrWalletDisplayValue = elementInformation.paymentField.txtMobilePaymentWalletNo.value;
            }

            const payment = {
                paymentInfoId: window.generalUtility.getRandomString(6),
                paymentMethodId: +elementInformation.paymentField.drpPaymentMethodId.value,
                paymentMethodName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation
                    .paymentField.drpPaymentMethodId),
                isBankingChannel: isBankingChannel,
                isMobileTransaction: isMobileTransaction,
                bankInfoId: bankInfoId,
                bankName: bankName,
                mobilePaymentWalletNo: elementInformation.paymentField.txtMobilePaymentWalletNo.value,
                documentNoOrTransactionId: elementInformation.paymentField.txtDocumentNoOrTransactionId.value,
                paymentDocumentOrTransDate: paymentDocumentOrTransDate,
                paymentDate: elementInformation.paymentField.txtPaymentDate.value,
                paidAmount: paidAmount,
                paidAmountToDisplay: +elementInformation.paymentField.txtPaidAmount.value,
                paymentRemarks: elementInformation.paymentField.txtPaymentRemarks.value
            }

            listInfo.payments.push(payment);

            const dataRow = `
            <tr id="tr_${payment.paymentInfoId}">
                <td>
                    ${payment.paymentMethodName}
                </td>
                <td>
                    ${bankNameOrWalletDisplayValue}
                </td>
                <td>
                    ${payment.documentNoOrTransactionId}
                </td>
                <td>
                    ${payment.paymentDocumentOrTransDate === null ? '' : payment.paymentDocumentOrTransDate}
                </td>
                <td>
                    ${payment.paymentDate}
                </td>
                <td class="text-end">
                    ${payment.paidAmountToDisplay}
                </td>
                <td>
                    ${payment.paymentRemarks}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilityPurchase.removePayment('${
                payment.paymentInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
            elementInformation.listTables.paymentBody.insertAdjacentHTML('beforeend', dataRow);
            commonUtilityPurchase.showTotalPaidAmount();
            //showTotalPaidAmount
            commonChangeEventsForCalculation.resetForm.payment();
            commonUtilityPurchase.showDueAmount();
        }
    });

//Add Tax Payment
elementInformation.btn.btnAddTaxPayment.addEventListener('click',
    () => {

        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmTaxPayment)) {

            const paymentTypeId = +elementInformation.taxPaymentField.drpPurchaseImportTaxPaymentTypeId.value;
            const paymentTypeName =
                window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation.taxPaymentField
                    .drpPurchaseImportTaxPaymentTypeId);
            const paidAmount = window.generalUtility.roundNumberOption.byFour(+ elementInformation.taxPaymentField
                .txtPurchaseImportTaxPaymentPaidAmount.value);

            const taxDueInPaymentType = commonUtilityPurchase.getTotalTaxByType(paymentTypeId) -
                commonUtilityPurchase.getTotalPaidTaxByType(paymentTypeId);

//            if (paidAmount > taxDueInPaymentType) {
//                window.generalUtility.message.showErrorMsg(
//                    `${paymentTypeName} should not be greater than ${taxDueInPaymentType}!`);
//                return;
//            }

            const payment = {
                paymentInfoId: window.generalUtility.getRandomString(6),
                taxPaymentTypeId: paymentTypeId,
                taxPaymentTypeName: paymentTypeName,
                paymentVatCommissionarateId: +elementInformation.taxPaymentField
                    .drpPurchaseImportTaxPaymentVatCommissionarateId.value,
                paymentVatCommissionarateName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(
                    elementInformation.taxPaymentField.drpPurchaseImportTaxPaymentVatCommissionarateId),
                paymentBankId: +elementInformation.taxPaymentField.drpPurchaseImportTaxPaymentBankId.value,
                paymentBankName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation
                    .taxPaymentField.drpPurchaseImportTaxPaymentBankId),
                paymentBankBranchName: elementInformation.taxPaymentField.txtPurchaseImportTaxPaymentBankBranch.value,
                paymentBankBranchDistId: +elementInformation.taxPaymentField
                    .drpPurchaseImportTaxPaymentBankBranchDistrictId.value,
                paymentBankBranchDistName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(
                    elementInformation.taxPaymentField.drpPurchaseImportTaxPaymentBankBranchDistrictId),
                paymentAccCode: elementInformation.taxPaymentField.txtPurchaseImportTaxPaymentAccCode.value,
                paymentDocOrChallanNo: elementInformation.taxPaymentField.txtPurchaseImportTaxPaymentDocOrChallanNo
                    .value,
                paymentDocOrChallanDate: elementInformation.taxPaymentField.txtPurchaseImportTaxPaymentDocOrChallanDate
                    .value,
                paymentDate: elementInformation.taxPaymentField.txtPurchaseImportTaxPaymentDate.value,
                paidAmount: paidAmount,
                paidAmountToDisplay: paidAmount,
                paymentRemarks: elementInformation.taxPaymentField.txtPurchaseImportTaxPaymentRemarks.value
            }

            listInfo.taxPayments.push(payment);

            const dataRow = `
            <tr id="tr_${payment.paymentInfoId}">
                <td>
                    ${payment.taxPaymentTypeName}
                </td>
                <td>
                    ${payment.paymentVatCommissionarateName}
                </td>
                <td>
                    <div>Name: ${payment.paymentBankName}</div>
                    <div>Branch: ${payment.paymentBankBranchName}</div>
                    <div>District: ${payment.paymentBankBranchDistName}</div>
                </td>
                <td>
                    ${payment.paymentAccCode}
                </td>
                <td>
                    <div>${payment.paymentDocOrChallanNo}</div>
                    <div>${payment.paymentDocOrChallanDate}</div>
                </td>
                <td>
                    ${payment.paymentDate}
                </td>
                <td class="text-end">
                    ${payment.paidAmountToDisplay}
                </td>
                <td>
                    ${payment.paymentRemarks}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilityPurchase.removeTaxPayment('${
                payment.paymentInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
            elementInformation.listTables.taxPaymentBody.insertAdjacentHTML('beforeend', dataRow);
            commonChangeEventsForCalculation.resetForm.taxPayment();
            elementInformation.totalTaxPaidCalculatedTableCell.paymentTotalPaid.innerHTML =
                window.generalUtility.getSumFromObjectArray(listInfo.taxPayments, 'paidAmount');
        }
    });

//Add Document
elementInformation.btn.btnAddDocument.addEventListener('click',
    () => {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmDocument)) {
            const uploadedFiles = elementInformation.documentField.fileUploadedFile.files;
            const docRemarks = elementInformation.documentField.txtDocumentRemarks.value;
            const documentTypeId = elementInformation.documentField.drpDocumentTypeId.value;
            const documentTypeName =
                window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation.documentField
                    .drpDocumentTypeId);
            for (let i = 0; i < uploadedFiles.length; i++) {
                const documentInfo = {
                    documentInfoId: window.generalUtility.getRandomString(6),
                    documentTypeId: documentTypeId,
                    documentTypeName: documentTypeName,
                    uploadedDocumentName: uploadedFiles[i].name,
                    uploadedDocument: uploadedFiles[i],
                    documentRemarks: docRemarks
                }

                listInfo.docs.push(documentInfo);
                const dataRow = `
            <tr id="tr_${documentInfo.documentInfoId}">
                <td>
                    ${documentInfo.documentTypeName}
                </td>
                <td>
                    ${documentInfo.uploadedDocumentName}
                </td>
                <td>
                    ${documentInfo.documentRemarks}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilityPurchase.removeDoc('${documentInfo
                    .documentInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
                elementInformation.listTables.docBody.insertAdjacentHTML('beforeend', dataRow);
            }
            commonChangeEventsForCalculation.resetForm.doc();
        }
    });

//Save Information
elementInformation.btn.btnSave.addEventListener('click',
    () => {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
            if (listInfo.products.length <= 0) {
                window.generalUtility.message.showErrorMsg('Can not save without product!!!');
                return;
            }

            const totalTaxAmount = commonUtilityPurchase.getTotalCd() +
                commonUtilityPurchase.getTotalId() +
                commonUtilityPurchase.getTotalRd() +
                commonUtilityPurchase.getTotalAit() +
                commonUtilityPurchase.getTotalSd() +
                commonUtilityPurchase.getTotalVat() +
                commonUtilityPurchase.getTotalAt();
            const taxDue = totalTaxAmount - commonUtilityPurchase.getTotalTaxPaidAmount();

//            if (taxDue > 0) {
//                window.generalUtility.message.showErrorMsg('Please provide total tax payment information!!!');
//                return;
//            }
            generalUtility.alterAttr.setAttr(elementInformation.btn.btnSave, 'disabled', 'disabled');
            //todo
            commonUtilityPurchase.postFormData(commonUtilityPurchase.getFormData());
        }
    });

//Save as a draft information
elementInformation.btn.btnSaveDraft.addEventListener('click',
    () => {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
            if (listInfo.products.length <= 0) {
                window.generalUtility.message.showErrorMsg('Can not save as draft without product!!!');
                return;
            }
        }
    });

//Reset Whole form
elementInformation.btn.btnResetForm.addEventListener('click',
    () => {
        location.reload();
    });


//Add Product List


const addRelatedProducts = () => {
    const productInfo = {
        productInfoId: window.generalUtility.getRandomString(6),
        productId: elementInformation.productField.drpProductId.value,
        productName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation.productField
            .drpProductId),
        hsCode: elementInformation.productField.txtHSCode.value,
        description: elementInformation.productField.txtProductDescription.value,
        partNo: elementInformation.productField.txtPartNo.value,
        goodsId: elementInformation.productField.txtGoodsId.value,
        sKUId: elementInformation.productField.txtSKUId.value,
        sKUNo: elementInformation.productField.txtSKUNo.value,
        measurementUnitId: +elementInformation.productField.hdnMeasurementUnitId.value,
        measurementUnitName: elementInformation.productField.txtMeasurementUnitName.value,
        unitPrice: +elementInformation.productField.txtUnitPrice.value,
        unitPriceToDisplay: +elementInformation.productField.txtUnitPrice.value,
        quantity: +elementInformation.productField.txtQuantity.value,
        quantityToDisplay: +elementInformation.productField.txtQuantity.value,
        totalPrice: priceCalculation.calculatePrice(+elementInformation.productField.txtUnitPrice.value,
            +elementInformation.productField.txtQuantity.value),
        totalPriceToDisplay: priceCalculation.calculatePrice(+elementInformation.productField.txtUnitPrice.value,
            +elementInformation.productField.txtQuantity.value),
        cdPercent: +elementInformation.productField.txtCustomDutyPercent.value,
        cdPercentToDisplay: +elementInformation.productField.txtCustomDutyPercent.value,
        cd: +elementInformation.productField.txtCustomDuty.value,
        cdToDisplay: +elementInformation.productField.txtCustomDuty.value,
        idPercent: +elementInformation.productField.txtImportDutyPercent.value,
        idPercentToDisplay: +elementInformation.productField.txtImportDutyPercent.value,
        id: +elementInformation.productField.txtImportDuty.value,
        idToDisplay: +elementInformation.productField.txtImportDuty.value,
        rdPercent: +elementInformation.productField.txtRegulatoryDutyPercent.value,
        rdPercentToDisplay: +elementInformation.productField.txtRegulatoryDutyPercent.value,
        rd: +elementInformation.productField.txtRegulatoryDuty.value,
        rdToDisplay: +elementInformation.productField.txtRegulatoryDuty.value,
        aitPercent: +elementInformation.productField.txtAdvanceIncomeTaxPercent.value,
        aitPercentToDisplay: +elementInformation.productField.txtAdvanceIncomeTaxPercent.value,
        ait: +elementInformation.productField.txtAdvanceIncomeTax.value,
        aitToDisplay: +elementInformation.productField.txtAdvanceIncomeTax.value,
        sdPercent: +elementInformation.productField.txtSupplementaryDutyPercent.value,
        sdPercentToDisplay: +elementInformation.productField.txtSupplementaryDutyPercent.value,
        sd: +elementInformation.productField.txtProductSupplementaryDuty.value,
        sdToDisplay: +elementInformation.productField.txtProductSupplementaryDuty.value,
        vatTypeId: +elementInformation.productField.drpProductVatTypeId.value,
        vatTypeName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation.productField
            .drpProductVatTypeId),
        vatPercent: +elementInformation.productField.txtProductVatPercent.value,
        vatPercentToDisplay: +elementInformation.productField.txtProductVatPercent.value,
        totalVat: +elementInformation.productField.txtProductVat.value,
        totalVatToDisplay: +elementInformation.productField.txtProductVat.value,
        atPercent: +elementInformation.productField.txtAdvanceTaxPercent.value,
        atPercentToDisplay: +elementInformation.productField.txtAdvanceTaxPercent.value,
        at: +elementInformation.productField.txtAdvanceTax.value,
        atToDisplay: +elementInformation.productField.txtAdvanceTax.value,
        totalPriceIncludingAllTax: +elementInformation.productField.txtTotalPriceIncludingAllTax.value,
        totalPriceIncludingAllTaxToDisplay: +elementInformation.productField.txtTotalPriceIncludingAllTax.value
    }

    listInfo.products.push(productInfo);

    const dataRow = `
        <tr id="tr_${productInfo.productInfoId}">
            <td>${productInfo.productName}</td>
            <td>${productInfo.measurementUnitName}</td>
            <td class="text-end">${productInfo.unitPriceToDisplay}</td>
            <td class="text-end">${productInfo.quantityToDisplay}</td>
            <td class="text-end">${productInfo.totalPriceToDisplay}</td>
            <td class="text-end">${productInfo.cdToDisplay} (@${productInfo.cdPercentToDisplay}%)</td>
            <td class="text-end">${productInfo.idToDisplay} (@${productInfo.idPercentToDisplay}%)</td>
            <td class="text-end">${productInfo.rdToDisplay} (@${productInfo.rdPercentToDisplay}%)</td>
            <td class="text-end">${productInfo.aitToDisplay} (@${productInfo.aitPercentToDisplay}%)</td>
            <td class="text-end">${productInfo.sdToDisplay} (@${productInfo.sdPercentToDisplay}%)</td>
            <td class="text-end">${productInfo.vatTypeName}</td>
            <td class="text-end">${productInfo.totalVatToDisplay} (@${productInfo.vatPercentToDisplay}%)</td>
            <td class="text-end">${productInfo.atToDisplay} (@${productInfo.atPercentToDisplay}%)</td>
            <td class="text-end">${productInfo.totalPriceIncludingAllTaxToDisplay}</td>
            <td>
                <button class="btn btn-danger btn-sm" onclick="commonUtilityPurchase.removeProduct('${
        productInfo.productInfoId}');" ><i class="bi bi-trash"></i></button>
            </td>
        </tr>
    `;
    elementInformation.listTables.productBody.insertAdjacentHTML('beforeend', dataRow);
    commonUtilityPurchase.showTotalAmount();
    commonUtilityPurchase.showDueAmount();
    commonChangeEventsForCalculation.resetForm.product();
    changeEffect.notifyForNotifiablePriceChange(productInfo.productId, productInfo.productName, productInfo.unitPrice);
}

const removeRelatedProduct = (productInfoId) => {
    const fileDisplayRowId = document.getElementById(`row_${productInfoId}`);
    fileDisplayRowId.remove();
    listInfo.products = listInfo.products.filter(f => f.RndId !== productInfoId);
}

const isProductExit = (productId) => {
    if (listInfo.products.find(f => f.ProductId === productId)) {
        return true;
    }
    return false;
}
//End  Product List


//saveData Request Start
const saveData = (data) => {
    console.log('ttt');
    console.log(data);
    //disContentById('loaderAnimation');
    $.ajax({
        url: 'PurchaseImport',
        data: data,
        cache: false,
        method: 'POST',
        contentType: false,
        processData: false,
        dataType: 'json',
        success: function(d) {
            window.location.href = `Details/${d.id}`;
        },
        error: function() {
            // viewErrorFeedBack();
            //hideContentById('loaderAnimation');
        }
    });
}
//saveData Request End


elementInformation.btn.btnResetProduct.addEventListener('click',
    () => {
        commonChangeEventsForCalculation.resetForm.product();
    });