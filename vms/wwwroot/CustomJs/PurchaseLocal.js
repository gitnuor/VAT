const genUtil = window.generalUtility;
const listInfo = {
    products: [],
    productProp: {
        productInfoId: 'productInfoId',
        totalPrice: 'totalPrice',
        productDiscount: 'productDiscount',
        productPriceAfterDiscount: 'productPriceAfterDiscount',
        sdValue: 'sdValue',
        priceForVat: 'priceForVat',
        totalVat: 'totalVat',
        totalPriceWithVat: 'totalPriceWithVat',
        totalPriceWithVatAfterDiscount: 'totalPriceWithVatAfterDiscount'
    },
    payments: [],
    paymentProp: {
        paymentInfoId: 'paymentInfoId',
        paidAmount: 'paidAmount'
    },
    docs: [],
    docProp: {
        documentInfoId: 'documentInfoId'
    }
}

const logicalInformation = {
    isVds: false,
    isTds: false
}

const elementInformation = {
    formInfo: {
        frmMainInformation: document.getElementById('frmMainInformation'),
        frmProductInformation: document.getElementById('frmProductInformation'),
        frmPayment: document.getElementById('frmPayment'),
        frmDocument: document.getElementById('frmDocument')
    },
    mainFormFields: {
        chkIsTaxDeductedInSource: document.getElementById('IsTds'),
//        txtTdsRate: document.getElementById('TdsRate'),
        txtTdsAmount: document.getElementById('TdsAmount'),
        chkIsVatDeductedInSource: document.getElementById('IsVatDeductedInSource'),
        txtVdsAmount: document.getElementById('VdsAmount'),
        drpOrgBranchId: document.getElementById('OrgBranchId'),
        drpPurchaseReasonId: document.getElementById('PurchaseReasonId'),
        txtInvoiceNo: document.getElementById('InvoiceNo'),
        datePurchaseDate: document.getElementById('PurchaseDate'),
        drpPurchaseDate: document.getElementById('VendorId'),
        txtVendorInvoiceNo: document.getElementById('VendorInvoiceNo'),
        txtVatChallanNo: document.getElementById('VatChallanNo'),
        dateVatChallanIssueDate: document.getElementById('VatChallanIssueDate')
    },
    productField: {
        drpProductId: document.getElementById('ProductId'),
        txtHSCode: document.getElementById('HSCode'),
        txtSKUId: document.getElementById('SKUId'),
        txtSKUNo: document.getElementById('SKUNo'),
        hdnMeasurementUnitId: document.getElementById('MeasurementUnitId'),
        txtMeasurementUnitName: document.getElementById('MeasurementUnitName'),
        txtUnitPrice: document.getElementById('UnitPrice'),
        txtQuantity: document.getElementById('Quantity'),
        txtTotalPrice: document.getElementById('TotalPrice'),
        txtDiscountPerItem: document.getElementById('DiscountPerItem'),
        txtTotalProductDiscount: document.getElementById('TotalProductDiscount'),
        txtProductPriceWithDiscount: document.getElementById('ProductPriceWithDiscount'),
        txtSupplementaryDutyPercent: document.getElementById('SupplementaryDutyPercent'),
        txtProductSupplementaryDuty: document.getElementById('ProductSupplementaryDuty'),
        txtVatAblePrice: document.getElementById('VatAblePrice'),
        drpProductVatTypeId: document.getElementById('ProductVatTypeId'),
        txtProductVatPercent: document.getElementById('ProductVatPercent'),
        txtProductVat: document.getElementById('ProductVat'),
        txtProductPriceWithVat: document.getElementById('ProductPriceWithVat'),
        txtProductPriceWithVatAfterDiscount: document.getElementById('ProductPriceWithVatAfterDiscount')
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
    documentField: {
        drpDocumentTypeId: document.getElementById('DocumentTypeId'),
        fileUploadedFile: document.getElementById('UploadedFile'),
        txtDocumentRemarks: document.getElementById('DocumentRemarks')
    },
    totalCalculatedTableCell: {
        productTotalPrice: document.getElementById('productTotalPrice'),
        productTotalDiscount: document.getElementById('productTotalDiscount'),
        productTotalPriceAfterDiscount: document.getElementById('productTotalPriceAfterDiscount'),
        productTotalSd: document.getElementById('productTotalSd'),
        productTotalVatablePrice: document.getElementById('productTotalVatablePrice'),
        productTotalVat: document.getElementById('productTotalVat'),
        productTotalPriceWithVat: document.getElementById('productTotalPriceWithVat'),
        productTotalPriceWithVatAfterDiscount: document.getElementById('productTotalPriceWithVatAfterDiscount')
    },
    totalPaidCalculatedTableCell: {
        paymentTotalPaid: document.getElementById('paymentTotalPaid')
    },
    listTables: {
        product: document.getElementById('productTable'),
        productBody: document.getElementById('productTableBody'),
        payment: document.getElementById('paymentTable'),
        paymentBody: document.getElementById('paymentTableBody'),
        doc: document.getElementById('docTable'),
        docBody: document.getElementById('docTableBody')
    },
    btn: {
        btnAddProduct: document.getElementById('btnAddProduct'),
        btnResetProduct: document.getElementById('btnResetProduct'),
        btnAddPayment: document.getElementById('btnAddPayment'),
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
        return genUtil.roundNumberOption.byTwo(unitPrice * qty);
    },
    calculateUnitPrice: (price, qty) => {
        if (qty === 0)
            throw new exception('Quantity 0 not allowed!');
        return genUtil.roundNumberOption.byTwo(price / qty);
    },
    calculateDiscountPerItem: (qty, discount) => {
        if (qty === 0)
            throw new exception('Quantity 0 not allowed!');
        return genUtil.roundNumberOption.byTwo(discount / qty);
    },
    calculateDiscount: (qty, discountPerItem) => {
        return genUtil.roundNumberOption.byTwo(qty * discountPerItem);
    },
    calculatePriceAfterDiscount: (price, discount) => {
        return genUtil.roundNumberOption.byTwo(price - discount);
    },
    calculateSdPercent: (price, sd) => {
        if (price === 0)
            throw new exception('Price 0 not allowed!');
        return genUtil.roundNumberOption.byTwo(sd * 100 / price);
    },
    calculateSd: (price, sdPercent) => {
        return genUtil.roundNumberOption.byTwo(price * sdPercent / 100);
    },
    calculateVatablePrice: (price, sd) => {
        return generalUtility.roundNumberOption.byTwo(price + sd);
    },
    calculateVatPercent: (price, vat) => {
        if (price === 0)
            throw new exception('Price 0 not allowed!');
        return genUtil.roundNumberOption.byTwo(vat * 100 / price);
    },
    calculateVat: (vatablePrice, vatPercent) => {
        return genUtil.roundNumberOption.byTwo(vatablePrice * vatPercent / 100);
    },
    calculatePriceWithVatAndSd: (vatablePrice, vat) => {
        return genUtil.roundNumberOption.byTwo(vatablePrice + vat);
    },
    calculatePriceWithVatAndSdAfterDiscount: (priceWithVatAndSd, discount) => {
        return genUtil.roundNumberOption.byTwo(priceWithVatAndSd - discount);
    }
}

const commonChangeEventsForCalculation = {
    removePaymentEvent: () => {
        listInfo.payments = [];
        elementInformation.listTables.paymentBody.innerHTML = '';
        commonUtilityPurchase.showDueAmount();
        commonUtilityPurchase.showTotalPaidAmount();
    },
    removeVdsEvent: () => {
        window.generalUtility.displayOption.hideItemByClassName(specialItem.classesToShowHideInEvents.vdsRelated);
        logicalInformation.isVds = false;
        listInfo.products = [];
        elementInformation.listTables.productBody.innerHTML = '';
        commonUtilityPurchase.fixVdsAmount();
        commonUtilityPurchase.showDueAmount();
        commonUtilityPurchase.showTotalAmount();
        commonUtilityPurchase.showTotalPaidAmount();
    },
    removeTdsEvent: () => {
        window.generalUtility.displayOption.hideItemByClassName(specialItem.classesToShowHideInEvents.tdsRelated);
        logicalInformation.isTds = false;
        commonUtilityPurchase.fixTdsAmount();
        commonUtilityPurchase.showDueAmount();
        commonUtilityPurchase.showTotalAmount();
        commonUtilityPurchase.showTotalPaidAmount();
    },
    addVdsEvent: () => {
        window.generalUtility.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.vdsRelated);
        logicalInformation.isVds = true;
        commonUtilityPurchase.fixVdsAmount();
        commonUtilityPurchase.showDueAmount();
        commonUtilityPurchase.showTotalAmount();
        commonUtilityPurchase.showTotalPaidAmount();
    },
    addTdsEvent: () => {
        window.generalUtility.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.tdsRelated);
        logicalInformation.isTds = true;
        commonUtilityPurchase.fixTdsAmount();
        commonUtilityPurchase.showDueAmount();
        commonUtilityPurchase.showTotalAmount();
        commonUtilityPurchase.showTotalPaidAmount();
    },
    unitAndQtyChangeEvent: (unitPrice, qty) => {
        const totalPrice = priceCalculation.calculatePrice(unitPrice, qty);
        elementInformation.productField.txtTotalPrice.value = totalPrice.toFixed(2);
        const discount =
            priceCalculation.calculateDiscount(+(elementInformation.productField.txtDiscountPerItem.value), qty);
        elementInformation.productField.txtTotalProductDiscount.value = discount.toFixed(2);;
        elementInformation.productField.txtProductPriceWithDiscount.value =
            (priceCalculation.calculatePriceAfterDiscount(totalPrice, discount)).toFixed(2);;

        const sd = priceCalculation.calculateSd(totalPrice,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value = sd.toFixed(2);;

        const vatablePrice = priceCalculation.calculateVatablePrice(totalPrice, sd);
        elementInformation.productField.txtVatAblePrice.value = vatablePrice.toFixed(2);;

        const vat = priceCalculation.calculateVat(vatablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = vat.toFixed(2);;

        const priceWithVat = priceCalculation.calculatePriceWithVatAndSd(vatablePrice, vat);
        elementInformation.productField.txtProductPriceWithVat.value = priceWithVat.toFixed(2);;

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            (priceCalculation.calculatePriceWithVatAndSdAfterDiscount(priceWithVat, discount)).toFixed(2);;
    },
    totalPriceChangeEvent: (totalPrice) => {
        const qty = +elementInformation.productField.txtQuantity.value;
        const unitPrice = priceCalculation.calculateUnitPrice(totalPrice, qty);
        elementInformation.productField.txtUnitPrice.value = unitPrice.toFixed(2);;

        const totalDiscount = +(elementInformation.productField.txtTotalProductDiscount.value);
        elementInformation.productField.txtProductPriceWithDiscount.value =
            (priceCalculation.calculatePriceAfterDiscount(totalPrice, totalDiscount)).toFixed(2);;

        const sd = priceCalculation.calculateSd(totalPrice,
            +(elementInformation.productField.txtSupplementaryDutyPercent.value));
        elementInformation.productField.txtProductSupplementaryDuty.value = sd.toFixed(2);;

        const vatablePrice = priceCalculation.calculateVatablePrice(totalPrice, sd);
        elementInformation.productField.txtVatAblePrice.value = vatablePrice.toFixed(2);;

        const vat = priceCalculation.calculateVat(vatablePrice,
            +(elementInformation.productField.txtProductVatPercent.value));
        elementInformation.productField.txtProductVat.value = vat.toFixed(2);;

        const priceWithVat = priceCalculation.calculatePriceWithVatAndSd(vatablePrice, vat);
        elementInformation.productField.txtProductPriceWithVat.value = priceWithVat.toFixed(2);;

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            (priceCalculation.calculatePriceWithVatAndSdAfterDiscount(priceWithVat, totalDiscount)).toFixed(2);;
    },
    discountPerItemChangeEvent: (discountPerItem) => {
        const qty = +elementInformation.productField.txtQuantity.value;

        const totalDiscount = priceCalculation.calculateDiscount(qty, discountPerItem);
        elementInformation.productField.txtTotalProductDiscount.value = totalDiscount.toFixed(2);;

        const totalPrice = +elementInformation.productField.txtTotalPrice.value;

        elementInformation.productField.txtProductPriceWithDiscount.value =
            (priceCalculation.calculatePriceAfterDiscount(totalPrice, totalDiscount)).toFixed(2);;

        const totalPriceWithVat = +elementInformation.productField.txtProductPriceWithVat.value;

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            (priceCalculation.calculatePriceWithVatAndSdAfterDiscount(totalPriceWithVat, totalDiscount)).toFixed(2);;

    },
    productDiscountChangeEvent: (productDiscount) => {
        const qty = +elementInformation.productField.txtQuantity.value;

        const discountPerItem = priceCalculation.calculateDiscountPerItem(qty, productDiscount);
        elementInformation.productField.txtDiscountPerItem.value = discountPerItem.toFixed(2);;

        const totalPrice = +elementInformation.productField.txtTotalPrice.value;

        elementInformation.productField.txtProductPriceWithDiscount.value =
            (priceCalculation.calculatePriceAfterDiscount(totalPrice, productDiscount)).toFixed(2);;

        const totalPriceWithVat = +elementInformation.productField.txtProductPriceWithVat.value;

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            (priceCalculation.calculatePriceWithVatAndSdAfterDiscount(totalPriceWithVat, productDiscount)).toFixed(2);;

    },
    sdPercentChangeEvent: (sdPercent) => {
        const productPrice = +(elementInformation.productField.txtTotalPrice.value);
        const totalSd = priceCalculation.calculateSd(productPrice, sdPercent);
        elementInformation.productField.txtProductSupplementaryDuty.value = totalSd.toFixed(2);;

        const vatAblePrice = priceCalculation.calculateVatablePrice(productPrice, totalSd);
        elementInformation.productField.txtVatAblePrice.value = vatAblePrice.toFixed(2);;

        const vatPercent = +elementInformation.productField.txtProductVatPercent.value;
        const vat = priceCalculation.calculateVat(vatAblePrice, vatPercent);
        elementInformation.productField.txtProductVat.value = vat.toFixed(2);;

        const priceWithVat = priceCalculation.calculatePriceWithVatAndSd(vatAblePrice, vat);
        elementInformation.productField.txtProductPriceWithVat.value = priceWithVat.toFixed(2);;

        const totalDiscount = +(elementInformation.productField.txtTotalProductDiscount.value);

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            (priceCalculation.calculatePriceWithVatAndSdAfterDiscount(priceWithVat, totalDiscount)).toFixed(2);;
    },
    vatPercentChangeEvent: (vatPercent) => {
        const vatAblePrice = +(elementInformation.productField.txtVatAblePrice.value);

        const vat = priceCalculation.calculateVat(vatAblePrice, vatPercent);
        elementInformation.productField.txtProductVat.value = vat.toFixed(2);;

        const priceWithVat = priceCalculation.calculatePriceWithVatAndSd(vatAblePrice, vat);
        elementInformation.productField.txtProductPriceWithVat.value = priceWithVat;

        const totalDiscount = +(elementInformation.productField.txtTotalProductDiscount.value);

        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value =
            (priceCalculation.calculatePriceWithVatAndSdAfterDiscount(priceWithVat, totalDiscount)).toFixed(2);;
    },
    clearPriceEvent: () => {
        elementInformation.productField.txtUnitPrice.value = 0;
        elementInformation.productField.txtQuantity.value = 0;
        elementInformation.productField.txtTotalPrice.value = 0;
        elementInformation.productField.txtDiscountPerItem.value = 0;
        elementInformation.productField.txtTotalProductDiscount.value = 0;
        elementInformation.productField.txtProductPriceWithDiscount.value = 0;
        elementInformation.productField.txtProductSupplementaryDuty.value = 0;
        elementInformation.productField.txtVatAblePrice.value = 0;
        elementInformation.productField.txtProductVat.value = 0;
        elementInformation.productField.txtProductPriceWithVat.value = 0;
        elementInformation.productField.txtProductPriceWithVatAfterDiscount.value = 0;
    },
    resetForm: {
        product: () => {
            elementInformation.formInfo.frmProductInformation.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.productField.drpProductId);
            let unitDrpElements = `<option value>Select Unit</option>`;
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.hdnMeasurementUnitId,
                unitDrpElements);
            window.generalUtility.setDropdownValue.selectPickerReset(
                elementInformation.productField.drpProductVatTypeId);
        },
        payment: () => {
            elementInformation.formInfo.frmPayment.reset();
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.paymentField
                .drpPaymentMethodId);
            window.generalUtility.setDropdownValue.selectPickerReset(elementInformation.paymentField.drpBankId);
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
        vdsRelated: 'vds-related',
        tdsRelated: 'tds-related',
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

const commonChangeEventsForDisplay = {
    hidePaymentSpecialOptions: () => {
        specialItem.classesToShowHideInEvents.paymentRelatedList.forEach(element => {
            window.generalUtility.displayOption.hideItemByClassName(element);
        });
    },
    displayPaymentBank: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.bankPayment);
    },
    displayPaymentMobile: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.mobilePayment);
    }
}

const commonUtilityPurchase = {
    getTotalPrice: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPrice);
    },
    getTotalDiscount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.productDiscount);
    },
    getTotalPriceAfterDiscount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.productPriceAfterDiscount);
    },
    getTotalSd: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.sdValue);
    },
    getTotalVatAblePrice: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.priceForVat);
    },
    getTotalVat: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalVat);
    },
    getTotalTax: () => {
        return (generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPrice) * 0.03);
    },
    getTotalPriceWithVat: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products, listInfo.productProp.totalPriceWithVat);
    },
    getTotalPriceWithVatAfterDiscount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.products,
            listInfo.productProp.totalPriceWithVatAfterDiscount);
    },
    getTotalPaidAmount: () => {
        return generalUtility.getSumFromObjectArray(listInfo.payments, listInfo.paymentProp.paidAmount);
    },
    notifyForNotifiablePriceChange: (productId, productName, unitPrice) => {
        console.log(window.mainUrls.product);
        window.$.ajax({
            url: `${window.mainUrls.product}/GetNumberOfFinishedProductsWithNotifiableChange/?productId=${productId}&unitPrice=${unitPrice}`,
            cache: false,
            method: "GET",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                if (d.totalItem > 0) {
                    elementInformation.otherBlock.notificationArea.insertAdjacentHTML('afterend', `<div class="alert alert-warning price-alert" role="alert">Price of ${productName} has been changed more than 7.5% from previous 1 products Input-Output co-efficient. Input-Output Co-efficient should be declare again.</div>`);
                }
            }
        });
    },
    removeProduct: (infoId) => {
        listInfo.products =
            generalUtility.removeFromObjectArray(listInfo.products, listInfo.productProp.productInfoId, infoId);
        commonUtilityPurchase.showTotalAmount();
        commonUtilityPurchase.fixVdsAmount();
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
    removeDoc: (infoId) => {
        listInfo.docs = generalUtility.removeFromObjectArray(listInfo.docs, listInfo.docProp.documentInfoId, infoId);
        generalUtility.removeElementById(`tr_${infoId}`);
    },
    showTotalAmount: () => {
        elementInformation.totalCalculatedTableCell.productTotalPrice.innerText =
            (commonUtilityPurchase.getTotalPrice()).toFixed(2);
        elementInformation.totalCalculatedTableCell.productTotalDiscount.innerText =
            (commonUtilityPurchase.getTotalDiscount()).toFixed(2);
        elementInformation.totalCalculatedTableCell.productTotalPriceAfterDiscount.innerText =
            (commonUtilityPurchase.getTotalPriceAfterDiscount()).toFixed(2);
        elementInformation.totalCalculatedTableCell.productTotalSd.innerText =
            (commonUtilityPurchase.getTotalSd()).toFixed(2);
        elementInformation.totalCalculatedTableCell.productTotalVatablePrice.innerText =
            (commonUtilityPurchase.getTotalVatAblePrice()).toFixed(2);
        elementInformation.totalCalculatedTableCell.productTotalVat.innerText =
            (commonUtilityPurchase.getTotalVat()).toFixed(2);
        elementInformation.totalCalculatedTableCell.productTotalPriceWithVat.innerText =
            (commonUtilityPurchase.getTotalPriceWithVat()).toFixed(2);
        elementInformation.totalCalculatedTableCell.productTotalPriceWithVatAfterDiscount.innerText =
            (commonUtilityPurchase.getTotalPriceWithVatAfterDiscount()).toFixed(2);
    },
    showTotalPaidAmount: () => {
        elementInformation.totalPaidCalculatedTableCell.paymentTotalPaid.innerText =
            new Intl.NumberFormat('en-IN').format((commonUtilityPurchase.getTotalPaidAmount()).toFixed(2));
    },
    showDueAmount: () => {
        const dueBlock = elementInformation.otherBlock.dueAmount;
        let vdsAdjustText = '';
        let vdsAmount = 0;
        if (logicalInformation.isVds) {
            vdsAdjustText = ' (VDS Adjusted)';
            vdsAmount = +elementInformation.mainFormFields.txtVdsAmount.value;
        }
        window.generalUtility.displayOption.removeMultipleCssClass(dueBlock, specialItem.classesOfDueAmount);
        let payableAmount = commonUtilityPurchase.getTotalPriceWithVatAfterDiscount() - vdsAmount;
        let dueAmount = payableAmount - commonUtilityPurchase.getTotalPaidAmount();

        dueAmount = (generalUtility.roundNumberOption.byTwo(dueAmount));
        payableAmount = (generalUtility.roundNumberOption.byTwo(payableAmount)).toFixed(2);

        if (dueAmount < 0) {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.dangerText);
        } else if (dueAmount > 0) {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.warningText);
        } else {
            window.generalUtility.displayOption.addCssClass(dueBlock, window.generallyUsedCssClass.successText);
        }

        elementInformation.otherBlock.dueAmount.innerText =
            `Payable Amount: ${payableAmount}${vdsAdjustText}; Due amount: ${dueAmount}`;
        elementInformation.paymentField.txtPaidAmount.value = dueAmount.toFixed(2);
    },
    fixVdsAmount: () => {
        if (logicalInformation.isVds) {
            elementInformation.mainFormFields.txtVdsAmount.value = commonUtilityPurchase.getTotalVat().toFixed(2);
        } else {
            elementInformation.mainFormFields.txtVdsAmount.value = '';
        }
    },
    fixTdsAmount: () => {
        if (logicalInformation.isTds) {
            elementInformation.mainFormFields.txtTdsAmount.value = commonUtilityPurchase.getTotalTax().toFixed(2);
        } else {
            elementInformation.mainFormFields.txtTdsAmount.value = '';
        }
    },
    getFormData: () => {
        const formData = new FormData(elementInformation.formInfo.frmMainInformation);
        let docIndex = 0;
        let productIndex = 0;
        let paymentIndex = 0;
        console.log(listInfo.products);
        listInfo.products.forEach(d => {
            console.log(productIndex);
            formData.append(`Products[${productIndex}].ProductId`, d.productId);
            formData.append(`Products[${productIndex}].SKUId`, d.sKUId);
            formData.append(`Products[${productIndex}].SKUNo`, d.sKUNo);
            formData.append(`Products[${productIndex}].MeasurementUnitId`, d.measurementUnitId);
            formData.append(`Products[${productIndex}].Quantity`, d.quantity);
            formData.append(`Products[${productIndex}].UnitPrice`, d.unitPrice);
            formData.append(`Products[${productIndex}].TotalPrice`, d.totalPrice);
            formData.append(`Products[${productIndex}].DiscountPerItem`, d.discountPerItem);
            formData.append(`Products[${productIndex}].TotalProductDiscount`, d.productDiscount);
            formData.append(`Products[${productIndex}].ProductPriceWithDiscount`, d.productPriceAfterDiscount);
            formData.append(`Products[${productIndex}].SupplementaryDutyPercent`, d.sdPercent);
            formData.append(`Products[${productIndex}].ProductSupplementaryDuty`, d.sdValue);
            formData.append(`Products[${productIndex}].VatAblePrice`, d.priceForVat);
            formData.append(`Products[${productIndex}].ProductVatTypeId`, d.vatTypeId);
            formData.append(`Products[${productIndex}].ProductVatPercent`, d.vatPercent);
            formData.append(`Products[${productIndex}].ProductVat`, d.totalVat);
            formData.append(`Products[${productIndex}].ProductPriceWithVat`, d.totalPriceWithVat);
            formData.append(`Products[${productIndex}].ProductPriceWithVatAfterDiscount`,
                d.totalPriceWithVatAfterDiscount);
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
            url: 'PurchaseLocal',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                console.log(d);
                console.log('ss');
                window.location.href = `Details/${d.id}`;
            },
            error: function (d) {
                console.log(d);
                console.log('tt');

                generalUtility.alterAttr.removeAttr(elementInformation.btn.btnSave, 'disabled');
            }
        });
    },
    postFormDataDraft: data => {
        window.$.ajax({
            url: 'PurchaseLocalDraft',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                console.log(d);
                console.log('ss');
                window.location.href = `Details/${d.id}`;
            },
            error: function (d) {
                console.log(d);
                console.log('tt');

                generalUtility.alterAttr.removeAttr(elementInformation.btn.btnSave, 'disabled');
            }
        });
    },
    getMeasurementUnitForProduct: async (productId) => {
        const url = `/api/measurementunit/${productId}`;
        return await genUtil.ajaxFun.simpleGet(url);
    },
}

const dataAttributes = {
    payment: {
        isBankingChannel: 'data-is-banking-channel',
        isMobileTransaction: 'data-is-mobile-transaction'
    },
    product: {
        hsCode: 'data-hs-code',
        measurementUnitId: 'data-mu-id',
        measurementUnitName: 'data-mu-name',
        vatPercent: 'data-vat-percent',
        vatTypeId: 'data-vat-type-id',
        isVatUpdatable: 'data-is-vat-updatable',
        sdPercent: 'data-sd-percent'
    },
    productMeasurement: {
        productMeasurementConvertionRatio: 'data-product-convertion-ratio'
    },
}

//Listeners
//VAT deducted at source
elementInformation.mainFormFields.chkIsVatDeductedInSource.addEventListener('change',
    () => {
        if (event.target.checked) {
            commonChangeEventsForCalculation.removePaymentEvent();
            commonChangeEventsForCalculation.addVdsEvent();
        } else {
            commonChangeEventsForCalculation.removePaymentEvent();
            commonChangeEventsForCalculation.removeVdsEvent();
        }
    });
elementInformation.mainFormFields.chkIsTaxDeductedInSource.addEventListener('change',
    () => {
        if (event.target.checked) {
//            commonChangeEventsForCalculation.removePaymentEvent();
            commonChangeEventsForCalculation.addTdsEvent();
        } else {
//            commonChangeEventsForCalculation.removePaymentEvent();
            commonChangeEventsForCalculation.removeTdsEvent();
        }
    });

//VDS Amount
elementInformation.mainFormFields.txtVdsAmount.addEventListener('change',
    () => {
        const vdsAmountTextBox = event.target;
        if (!logicalInformation.isVds) {
            vdsAmountTextBox.value = '';
            return;
        }
        const totalVatAmount = commonUtilityPurchase.getTotalVat();
        const vdsAmount = +vdsAmountTextBox.value;
        if (vdsAmount > totalVatAmount) {
            window.generalUtility.message.showErrorMsg("VDS amount should be less than or equal to VAT amount!");

        }
    });

//Product change event
elementInformation.productField.drpProductId.addEventListener('change',
    (event) => {
        const drp = event.target;
        const prodDataAttr = dataAttributes.product;
        elementInformation.productField.txtHSCode.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.hsCode);
        //elementInformation.productField.hdnMeasurementUnitId.value =
        //    window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitId);

        elementInformation.productField.txtMeasurementUnitName.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.measurementUnitName);

        window.generalUtility.setDropdownValue.selectPickerByControl(
            elementInformation.productField.drpProductVatTypeId,
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatTypeId));

        elementInformation.productField.txtProductVatPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatPercent);

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

        elementInformation.productField.txtSupplementaryDutyPercent.value =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.sdPercent);

        commonChangeEventsForCalculation.clearPriceEvent();
        window.generalUtility.validateElement.elementByElementList(specialItem.elementToValidate.productChange);
        setMeasurementUnitByProduct(event);
    });

const setMeasurementUnitByProduct = (event) => {
    let unitDrpElements = `<option value>Select Unit</option>`;
    const productId = event.target.value;
    if (productId) {
        commonUtilityPurchase.getMeasurementUnitForProduct(productId).then(products => {
            products.forEach(element => {
                unitDrpElements += `
                    <option value='${element.measurementUnitId}'
                    data-product-convertion-ratio='${element.convertionRatio}'>
                        ${element.measurementUnitName}
                    </option>
                `;
            });
            genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.hdnMeasurementUnitId,
                unitDrpElements);
        }).then(() => {
            genUtil.setDropdownValue.selectPickerByControl(elementInformation.productField.hdnMeasurementUnitId,
                genUtil.getDataAttributeValue.dropdownSelected(elementInformation.productField.drpProductId, dataAttributes.product.measurementUnitId));
        });

    } else {
        genUtil.setDropdownValue.selectPickerUpdateItems(elementInformation.productField.hdnMeasurementUnitId,
            unitDrpElements);
    }

}

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

//Discount per Item
elementInformation.productField.txtDiscountPerItem.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.discountPerItemChangeEvent(+event.target.value);
    });

//Product Discount
elementInformation.productField.txtTotalProductDiscount.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.productDiscountChangeEvent(+event.target.value);
    });

//SD Percent
elementInformation.productField.txtSupplementaryDutyPercent.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.sdPercentChangeEvent(+event.target.value);
    });

//VAT Percent
elementInformation.productField.txtProductVatPercent.addEventListener('input',
    (event) => {
        commonChangeEventsForCalculation.vatPercentChangeEvent(+event.target.value);
    });

//Vat type change event
elementInformation.productField.drpProductVatTypeId.addEventListener('change',
    (event) => {
        const drp = event.target;

        const prodDataAttr = dataAttributes.product;

        const defaultVatPercent =
            +window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.vatPercent);
        const isVatUpdatable =
            window.generalUtility.getDataAttributeValue.dropdownSelected(drp, prodDataAttr.isVatUpdatable);
        elementInformation.productField.txtProductVatPercent.value = defaultVatPercent.toFixed(2);

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
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmProductInformation)) {
            const productInfo = {
                productInfoId: window.generalUtility.getRandomString(6),
                productId: +elementInformation.productField.drpProductId.value,
                productName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation
                    .productField.drpProductId),
                sKUId: elementInformation.productField.txtSKUId.value,
                sKUNo: elementInformation.productField.txtSKUNo.value,
                measurementUnitId: +elementInformation.productField.hdnMeasurementUnitId.value,
                measurementUnitName: elementInformation.productField.txtMeasurementUnitName.value,
                unitPrice: +elementInformation.productField.txtUnitPrice.value,
                unitPriceToDisplay: (+elementInformation.productField.txtUnitPrice.value).toFixed(2),
                quantity: +elementInformation.productField.txtQuantity.value,
                quantityToDisplay: (+elementInformation.productField.txtQuantity.value).toFixed(2),
                totalPrice: priceCalculation.calculatePrice(+elementInformation.productField.txtUnitPrice.value,
                    +elementInformation.productField.txtQuantity.value),
                totalPriceToDisplay: (priceCalculation.calculatePrice(
                    +elementInformation.productField.txtUnitPrice.value,
                    +elementInformation.productField.txtQuantity.value)).toFixed(2),
                discountPerItem: +elementInformation.productField.txtDiscountPerItem.value,
                discountPerItemToDisplay: (+elementInformation.productField.txtDiscountPerItem.value).toFixed(2),
                productDiscount: +elementInformation.productField.txtTotalProductDiscount.value,
                productDiscountToDisplay: (+elementInformation.productField.txtTotalProductDiscount.value).toFixed(2),
                productPriceAfterDiscount: +elementInformation.productField.txtProductPriceWithDiscount.value,
                productPriceAfterDiscountToDisplay: (+elementInformation.productField.txtProductPriceWithDiscount.value)
                    .toFixed(2),
                sdPercent: +elementInformation.productField.txtSupplementaryDutyPercent.value,
                sdPercentToDisplay: (+elementInformation.productField.txtSupplementaryDutyPercent.value).toFixed(2),
                sdValue: +elementInformation.productField.txtProductSupplementaryDuty.value,
                sdValueToDisplay: (+elementInformation.productField.txtProductSupplementaryDuty.value).toFixed(2),
                priceForVat: +elementInformation.productField.txtVatAblePrice.value,
                priceForVatToDisplay: (+elementInformation.productField.txtVatAblePrice.value).toFixed(2),
                vatTypeId: +elementInformation.productField.drpProductVatTypeId.value,
                vatTypeName: window.generalUtility.getDataAttributeValue.dropdownSelectedText(elementInformation
                    .productField.drpProductVatTypeId),
                vatPercent: +elementInformation.productField.txtProductVatPercent.value,
                vatPercentToDisplay: (+elementInformation.productField.txtProductVatPercent.value).toFixed(2),
                totalVat: +elementInformation.productField.txtProductVat.value,
                totalVatToDisplay: (+elementInformation.productField.txtProductVat.value).toFixed(2),
                totalPriceWithVat: +elementInformation.productField.txtProductPriceWithVat.value,
                totalPriceWithVatToDisplay: (+elementInformation.productField.txtProductPriceWithVat.value).toFixed(2),
                totalPriceWithVatAfterDiscount: +elementInformation.productField.txtProductPriceWithVatAfterDiscount
                    .value,
                totalPriceWithVatAfterDiscountToDisplay: (+elementInformation.productField
                    .txtProductPriceWithVatAfterDiscount.value).toFixed(2)
            }

            listInfo.products.push(productInfo);

            const dataRow = `
            <tr id="tr_${productInfo.productInfoId}">
                <td>
                    ${productInfo.productName}
                </td>
               
                <td>
                    ${productInfo.measurementUnitName}
                </td>
                <td class="text-end">
                    ${productInfo.unitPriceToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.quantityToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.totalPriceToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.productDiscountToDisplay} (@${productInfo.discountPerItemToDisplay}/item)
                </td>
                <td class="text-end">
                    ${productInfo.productPriceAfterDiscountToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.sdValueToDisplay} (@${productInfo.sdPercentToDisplay}%)
                </td>
                <td class="text-end">
                    ${productInfo.priceForVatToDisplay}
                </td>
                <td>
                    ${productInfo.vatTypeName}
                </td>
                <td class="text-end">
                    ${productInfo.totalVatToDisplay} (@${productInfo.vatPercentToDisplay}%)
                </td>
                <td class="text-end">
                    ${productInfo.totalPriceWithVatToDisplay}
                </td>
                <td class="text-end">
                    ${productInfo.totalPriceWithVatAfterDiscountToDisplay}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilityPurchase.removeProduct('${productInfo.productInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;

        //    const dataRow = `
        //    <tr id="tr_${productInfo.productInfoId}">
        //        <td>
        //            ${productInfo.productName}
        //        </td>
        //        <td>
        //            ${productInfo.sKUNo}
        //        </td>
        //        <td>
        //            ${productInfo.sKUId}
        //        </td>
        //        <td>
        //            ${productInfo.measurementUnitName}
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.unitPriceToDisplay}
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.quantityToDisplay}
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.totalPriceToDisplay}
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.productDiscountToDisplay} (@${productInfo.discountPerItemToDisplay}/item)
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.productPriceAfterDiscountToDisplay}
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.sdValueToDisplay} (@${productInfo.sdPercentToDisplay}%)
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.priceForVatToDisplay}
        //        </td>
        //        <td>
        //            ${productInfo.vatTypeName}
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.totalVatToDisplay} (@${productInfo.vatPercentToDisplay}%)
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.totalPriceWithVatToDisplay}
        //        </td>
        //        <td class="text-end">
        //            ${productInfo.totalPriceWithVatAfterDiscountToDisplay}
        //        </td>
        //        <td>
        //            <button class="btn btn-danger btn-sm" onclick="commonUtilityPurchase.removeProduct('${productInfo.productInfoId}');" ><i class="bi bi-trash"></i></button>
        //        </td>
        //    </tr>
        //`;
            elementInformation.listTables.productBody.insertAdjacentHTML('beforeend', dataRow);
            commonUtilityPurchase.showTotalAmount();
            commonUtilityPurchase.fixVdsAmount();
            commonUtilityPurchase.fixTdsAmount();
            commonUtilityPurchase.showDueAmount();
            commonChangeEventsForCalculation.resetForm.product();
            commonUtilityPurchase.notifyForNotifiablePriceChange(productInfo.productId, productInfo.productName, productInfo.unitPrice);
        }
    });

//Add Payment
elementInformation.btn.btnAddPayment.addEventListener('click',
    () => {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmPayment)) {
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
                paidAmount: +elementInformation.paymentField.txtPaidAmount.value,
                //Changed here
                paidAmountToDisplay: (+elementInformation.paymentField.txtPaidAmount.value).toFixed(2),
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
            commonUtilityPurchase.showDueAmount();
            //showTotalPaidAmount
            commonChangeEventsForCalculation.resetForm.payment();
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
                window.generalUtility.message.showErrorMsg("Can not save without product!!!");
                return;
            }
            generalUtility.alterAttr.setAttr(elementInformation.btn.btnSave, 'disabled', 'disabled');
            generalUtility.alterAttr.setAttr(elementInformation.btn.btnSaveDraft, 'disabled', 'disabled');
            commonUtilityPurchase.postFormData(commonUtilityPurchase.getFormData());
        }
    });

//Save as a draft information
elementInformation.btn.btnSaveDraft.addEventListener('click',
    () => {
        if (window.generalUtility.validateElement.formByForm(elementInformation.formInfo.frmMainInformation)) {
            if (listInfo.products.length <= 0) {
                window.generalUtility.message.showErrorMsg("Can not save as draft without product!!!");
                return;
            }
            generalUtility.alterAttr.setAttr(elementInformation.btn.btnSave, 'disabled', 'disabled');
            generalUtility.alterAttr.setAttr(elementInformation.btn.btnSaveDraft, 'disabled', 'disabled');
            commonUtilityPurchase.postFormDataDraft(commonUtilityPurchase.getFormData());
        }
    });

//Reset Whole form
elementInformation.btn.btnResetForm.addEventListener('click',
    () => {
        location.reload();
    });

elementInformation.btn.btnResetProduct.addEventListener('click',
    () => {
        commonChangeEventsForCalculation.resetForm.product();
    });